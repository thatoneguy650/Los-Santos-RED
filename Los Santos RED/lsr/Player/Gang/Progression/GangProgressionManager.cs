using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Rank progression inside a gang the player has already joined.
///
/// DESIGN NOTE — why this observes instead of hooking.
/// Upstream already maintains everything this needs: GangReputation.TasksCompleted is
/// incremented on task success and decremented on failure, and GangKickUp tracks
/// DueDate and MissedPeriods. All of it is public, all of it is already persisted, and
/// none of it is read by any gameplay code. So rather than editing GangRelationships,
/// PlayerTasks or GangKickUp to call us — four edits to busy upstream files, four future
/// merge conflicts — we poll those counters and act on the deltas. The whole accrual
/// system costs zero lines in upstream gameplay code.
///
/// The cost of that choice is that we cannot see a change smaller than our tick, and we
/// infer intent from counters rather than being told. For the events we care about
/// (a task resolved, a dues period rolled over) the counters are unambiguous, so the
/// trade is worth it. If a later slice needs something the counters cannot express,
/// that is the point to add a real hook — deliberately, one at a time.
/// </summary>
public class GangProgressionManager
{
    private readonly IGangRelateable Player;
    private readonly ITimeReportable Time;
    private readonly ISettingsProvideable Settings;

    private readonly List<GangProgressionSave> States = new List<GangProgressionSave>();
    private readonly List<GangStandingEvent> RecentEvents = new List<GangStandingEvent>();

    public GangProgressionManager(IGangRelateable player, ITimeReportable time, ISettingsProvideable settings)
    {
        Player = player;
        Time = time;
        Settings = settings;
    }

    private GangProgressionSettings Config => Settings?.SettingsManager?.GangProgressionSettings;
    private bool IsEnabled => Config != null && Config.EnableGangProgression;

    /// <summary>Newest first. In-memory only; a tuning instrument, not state.</summary>
    public List<GangStandingEvent> EventLog => RecentEvents.ToList();

    /// <summary>Every progression record, for the save layer and the debug surfaces.</summary>
    public List<GangProgressionSave> AllStates => States.ToList();

    public void Setup()
    {
    }

    public void Dispose()
    {
        RecentEvents.Clear();
    }

    /// <summary>Called when the character changes. Progression is per-character, so it all goes.</summary>
    public void Reset()
    {
        States.Clear();
        RecentEvents.Clear();
    }

    // -------------------------------------------------------------------------
    // Public reads
    // -------------------------------------------------------------------------

    /// <summary>
    /// The player's current rung with this gang.
    ///
    /// Returns the TOP rung whenever the feature is disabled, or when the player is not
    /// a member. That is deliberate: any future rank gate can be written as a plain
    /// "is my rank at least X" test and will simply pass when the feature is off, leaving
    /// upstream behaviour untouched without every gate needing to know the flag exists.
    /// </summary>
    public GangStandingLevel GetLevel(Gang gang)
    {
        GangStandingLevels ladder = Ladder();
        if (!IsEnabled || gang == null)
        {
            return ladder.GetTopLevel();
        }
        GangProgressionSave state = FindState(gang.ID);
        if (state == null)
        {
            return ladder.GetLevel(0);
        }
        return ladder.GetLevel(state.Standing);
    }

    /// <summary>Convenience for gates: "is the player at least this senior with this gang".</summary>
    public bool IsAtLeast(Gang gang, int requiredOrder)
    {
        GangStandingLevel level = GetLevel(gang);
        return level != null && level.Order >= requiredOrder;
    }

    public int GetStanding(Gang gang)
    {
        GangProgressionSave state = gang == null ? null : FindState(gang.ID);
        return state == null ? 0 : state.Standing;
    }

    public int GetGoodwill(Gang gang)
    {
        GangProgressionSave state = gang == null ? null : FindState(gang.ID);
        return state == null ? 0 : state.Goodwill;
    }

    public int GetStandingToNextLevel(Gang gang)
    {
        return Ladder().StandingToNextLevel(GetStanding(gang));
    }

    public GangStandingLevel GetNextLevel(Gang gang)
    {
        return Ladder().GetNextLevel(GetStanding(gang));
    }

    // -------------------------------------------------------------------------
    // The single choke point
    // -------------------------------------------------------------------------

    /// <summary>
    /// The only place standing and goodwill move. One place to guard on the flag, one
    /// place to clamp, one place to log, one place to feed the debug surfaces.
    /// </summary>
    public void ChangeStanding(Gang gang, int standingDelta, string reason)
    {
        if (!IsEnabled || gang == null || (standingDelta == 0))
        {
            return;
        }
        GangProgressionSave state = GetOrCreateState(gang.ID);
        int previousOrder = Ladder().GetLevel(state.Standing).Order;

        state.Standing = Clamp(state.Standing + standingDelta, Config.StandingMinimum, Config.StandingMaximum);

        // Goodwill is credited by the same events that credit standing, but is never
        // debited by them — losing standing should not also empty the wallet.
        int goodwillDelta = 0;
        if (standingDelta > 0)
        {
            goodwillDelta = (int)Math.Round(standingDelta * Config.GoodwillPerStandingEarned);
            state.Goodwill = Clamp(state.Goodwill + goodwillDelta, 0, Config.GoodwillMaximum);
        }

        RecordEvent(gang.ID, standingDelta, goodwillDelta, reason);
        CheckPromotion(gang, state, previousOrder);
    }

    /// <summary>
    /// Draw down goodwill for a favour. Returns false and changes nothing when the
    /// balance is short, so callers can use it directly as an affordability test.
    /// Standing is untouched — spending must never demote.
    /// </summary>
    public bool SpendGoodwill(Gang gang, int amount, string reason)
    {
        if (gang == null || amount <= 0)
        {
            return false;
        }
        if (!IsEnabled)
        {
            return true; // feature off: favours are not gated, behave as upstream
        }
        GangProgressionSave state = GetOrCreateState(gang.ID);
        if (state.Goodwill < amount)
        {
            return false;
        }
        state.Goodwill -= amount;
        RecordEvent(gang.ID, 0, -amount, reason);
        return true;
    }

    /// <summary>
    /// Credit goodwill without touching standing.
    ///
    /// The counterpart to SpendGoodwill, and the reason street earnings exist as their own
    /// path: shaking down a corner should fill the wallet without moving the rank axis.
    /// If it credited standing too, grinding dealers would promote you, and rank would stop
    /// meaning what the gang thinks of you.
    /// </summary>
    public void AddGoodwill(Gang gang, int amount, string reason)
    {
        if (!IsEnabled || gang == null || amount <= 0)
        {
            return;
        }
        GangProgressionSave state = GetOrCreateState(gang.ID);
        int before = state.Goodwill;
        state.Goodwill = Clamp(state.Goodwill + amount, 0, Config.GoodwillMaximum);
        int actual = state.Goodwill - before;
        if (actual > 0)
        {
            RecordEvent(gang.ID, 0, actual, reason);
        }
    }

    public bool CanAfford(Gang gang, int amount)
    {
        if (!IsEnabled)
        {
            return true;
        }
        return GetGoodwill(gang) >= amount;
    }

    /// <summary>
    /// A slow trickle of goodwill while men the player paid for are out with him.
    ///
    /// Being seen around the neighbourhood with your crew is its own kind of standing with
    /// the gang, and it gives the backup economy a reason to keep men alive between jobs
    /// rather than calling them only for a fight. Deliberately small: this should top up a
    /// wallet over a session, never replace doing the work.
    ///
    /// Paced off in-game time rather than the tick, so time acceleration and fast-forward
    /// do not turn a trickle into a flood.
    /// </summary>
    private void ObserveSquadUpkeep(Gang gang, GangProgressionSave state)
    {
        if (Config.GoodwillPerSquadTick <= 0 || Config.SquadGoodwillTickMinutes <= 0)
        {
            return;
        }
        int squad = Player?.GangRequisitionManager == null ? 0 : Player.GangRequisitionManager.LiveSquadSize(gang);
        if (squad <= 0)
        {
            return;
        }
        DateTime now = Time.CurrentDateTime;
        if (state.LastSquadUpkeep != DateTime.MinValue
            && DateTime.Compare(now, state.LastSquadUpkeep.AddMinutes(Config.SquadGoodwillTickMinutes)) < 0)
        {
            return;
        }
        state.LastSquadUpkeep = now;
        AddGoodwill(gang, squad * Config.GoodwillPerSquadTick,
            squad == 1 ? "out with the crew" : $"out with {squad} of the crew");
    }

    /// <summary>
    /// Finishing a job with your crew still standing is worth more than finishing it alone.
    /// They saw you do it, and a story with witnesses travels.
    ///
    /// Also the quiet counterweight to the loss penalty: bringing men and bringing them home
    /// pays, which is the behaviour the whole backup economy is trying to encourage.
    /// </summary>
    private void AwardWitnesses(Gang gang)
    {
        if (Config.StandingPerWitnessOnJob <= 0)
        {
            return;
        }
        Player?.GangCrewManager?.OnJobCompleted();
        int witnesses = Player?.GangRequisitionManager == null ? 0 : Player.GangRequisitionManager.LiveSquadSize(gang);
        if (witnesses <= 0)
        {
            return;
        }
        ChangeStanding(gang, witnesses * Math.Abs(Config.StandingPerWitnessOnJob),
            witnesses == 1 ? "crew witnessed the job" : $"{witnesses} crew witnessed the job");
    }

    /// <summary>
    /// Violence, scored by whose ground it happened on.
    ///
    /// Called from GangMember.OnKilledByPlayer, which already resolves the victim's gang and
    /// the kill zone for its own reputation maths — so this reads what is in hand rather than
    /// recomputing it. Killing on your own turf is the only kill that pays, which is what
    /// makes a territory worth standing in.
    /// </summary>
    public void OnRivalKilled(Gang victimGang, bool onOwnTurf, bool isDeclaredEnemy)
    {
        if (!IsEnabled || victimGang == null || Config == null || !Config.EnableStandingFromHostility)
        {
            return;
        }
        Gang myGang = Player?.RelationshipManager?.GangRelationships?.CurrentGang;
        if (myGang == null)
        {
            return;
        }
        if (myGang.ID == victimGang.ID)
        {
            if (Config.StandingPerOwnGangKill > 0)
            {
                ChangeStanding(myGang, -1 * Math.Abs(Config.StandingPerOwnGangKill), "killed one of our own");
            }
            return;
        }

        // The men standing with you were part of that, whatever the standing rules decide
        // below. Hooked here rather than at the standing award because a kill that pays the
        // player nothing - wrong turf, undeclared enemy - was still a shootout they were in.
        Player?.GangCrewManager?.OnPlayerKilledRival(victimGang);

        int award = Math.Abs(Config.StandingPerRivalKillOnTurf);
        if (isDeclaredEnemy)
        {
            award += Math.Abs(Config.StandingBonusPerEnemyKill);
        }
        if (award <= 0)
        {
            return;
        }

        // Sanctioned work moves rank; freelancing does not.
        //
        // A kill while you are on a job the gang gave you is gang business, so it pays
        // STANDING. The same kill off the clock is you doing your own thing — the gang will
        // hear you handled yourself, which is worth GOODWILL, but it is not the sort of
        // thing that gets a man promoted. Without this split a player could rank all the
        // way to Shotcaller by camping his own block and never once taking a job.
        if (IsOnSanctionedWork(myGang))
        {
            ChangeStanding(myGang, award, isDeclaredEnemy
                ? $"killed {victimGang.ShortName} on the job"
                : $"killed a {victimGang.ShortName} outsider on the job");
            return;
        }
        if (!onOwnTurf)
        {
            return; // freelance violence, nowhere near home: nothing owed
        }
        int freelance = (int)Math.Round(award * (Config.UnsanctionedKillGoodwillFraction <= 0f ? 0.5f : Config.UnsanctionedKillGoodwillFraction));
        if (freelance > 0)
        {
            AddGoodwill(myGang, freelance, $"handled {victimGang.ShortName} on our block");
        }
    }

    /// <summary>
    /// Forwarded to the requisition manager, which owns the turf probe. Lives here because
    /// IViolateable — what a dying ped is handed — carries the progression manager, and
    /// adding a second manager to a seventh player interface is a merge cost worth avoiding.
    /// </summary>
    public void OnCivilianKilled(Rage.Vector3 position)
    {
        Player?.GangRequisitionManager?.OnCivilianKilled(position);
    }

    /// <summary>
    /// Passthrough, for the same reason as OnCivilianKilled: a dying ped is handed an
    /// IViolateable, which carries this manager but not the requisition one.
    /// </summary>
    public bool IsShakeableDealer(PedExt ped)
    {
        return Player?.GangRequisitionManager != null && Player.GangRequisitionManager.IsShakeableDealer(ped);
    }

    /// <summary>
    /// Is the player currently carrying a job for this gang?
    ///
    /// PlayerTasks keys on the gang's phone contact name, which is the same lookup GangDen
    /// uses to decide whether to offer a payout, so "the gang asked for this" has a single
    /// consistent meaning across the feature.
    /// </summary>
    private bool IsOnSanctionedWork(Gang gang)
    {
        if (gang == null || string.IsNullOrEmpty(gang.ContactName) || Player?.PlayerTasks == null)
        {
            return false;
        }
        PlayerTask task = Player.PlayerTasks.GetTask(gang.ContactName);
        return task != null && task.IsActive;
    }

    // -------------------------------------------------------------------------
    // Access for the requisition layer
    // -------------------------------------------------------------------------

    /// <summary>
    /// The persisted record for a gang, created on demand. Exposed so the requisition
    /// layer can read and stamp its own cooldown fields without this class growing an
    /// accessor pair per favour. The record is ours, not upstream's, so handing it out
    /// costs nothing in merge surface.
    /// </summary>
    public GangProgressionSave GetRecord(Gang gang)
    {
        return gang == null ? null : GetOrCreateState(gang.ID);
    }

    /// <summary>
    /// What the player's current rung with this gang is allowed to ask for.
    ///
    /// Follows the same rule as GetLevel: with the feature off this returns the TOP row,
    /// so a gate written as "may I have this" passes by construction and upstream
    /// behaviour is untouched.
    /// </summary>
    public GangRankPrivilege GetPrivilege(Gang gang)
    {
        GangRankPrivileges table = Config?.RankPrivileges ?? new GangRankPrivileges();
        GangStandingLevel level = GetLevel(gang);
        return table.GetPrivilege(level == null ? 0 : level.Order);
    }

    // -------------------------------------------------------------------------
    // The tick
    // -------------------------------------------------------------------------

    public void Update()
    {
        // The crew roster rides this tick rather than owning one: arrests have no event to
        // hook, so somebody has to poll, and a second 2s task for one dictionary scan is
        // not worth the scheduler slot.
        Player?.GangCrewManager?.Update();
        if (!IsEnabled)
        {
            return;
        }
        GangRelationships relationships = Player?.RelationshipManager?.GangRelationships;
        if (relationships == null)
        {
            return;
        }
        Gang currentGang = relationships.CurrentGang;
        if (currentGang == null)
        {
            return; // not a member of anything; progression only exists inside a gang
        }
        GangReputation reputation = relationships.GetReputation(currentGang);
        if (reputation == null || !reputation.IsMember)
        {
            return;
        }
        GangKickUp kickUp = relationships.CurrentGangKickUp;
        GangProgressionSave state = GetOrCreateState(currentGang.ID);

        if (!state.IsSeeded)
        {
            Seed(state, reputation, kickUp);
            return; // never award on the first reading
        }
        ObserveTasks(currentGang, state, reputation);
        ObserveDues(currentGang, state, kickUp);
        ObserveSquadUpkeep(currentGang, state);
    }

    /// <summary>
    /// Take the first reading without awarding anything.
    ///
    /// This matters more than it looks. A save written before this feature existed can
    /// already carry a large TasksCompleted total, and joining a gang mid-session starts
    /// from whatever that counter happens to hold. Without seeding, the first tick would
    /// read the whole history as freshly earned and hand out a windfall.
    /// </summary>
    private void Seed(GangProgressionSave state, GangReputation reputation, GangKickUp kickUp)
    {
        state.TasksCompletedSeen = reputation.TasksCompleted;
        state.MissedPeriodsSeen = kickUp == null ? 0 : kickUp.MissedPeriods;
        state.DueDateSeen = kickUp == null ? DateTime.MinValue : kickUp.DueDate;
        state.IsSeeded = true;
        Log($"seeded progression for {state.GangID} at tasks:{state.TasksCompletedSeen} missed:{state.MissedPeriodsSeen}");
    }

    /// <summary>
    /// Upstream increments TasksCompleted on success and decrements it on failure, so the
    /// sign of the delta tells us which happened without touching the task system.
    /// </summary>
    private void ObserveTasks(Gang gang, GangProgressionSave state, GangReputation reputation)
    {
        int current = reputation.TasksCompleted;
        if (current == state.TasksCompletedSeen)
        {
            return;
        }
        int delta = current - state.TasksCompletedSeen;
        state.TasksCompletedSeen = current;

        if (delta > 0)
        {
            ChangeStanding(gang, delta * Math.Abs(Config.StandingPerTaskCompleted),
                delta == 1 ? "job completed" : $"{delta} jobs completed");
            AwardWitnesses(gang);
        }
        else
        {
            int failures = Math.Abs(delta);
            ChangeStanding(gang, -1 * failures * Math.Abs(Config.StandingPerTaskFailed),
                failures == 1 ? "job failed" : $"{failures} jobs failed");
        }
    }

    /// <summary>
    /// Both paying and missing dues advance GangKickUp.DueDate, so the rollover alone does
    /// not say which happened — MissedPeriods does. If it rose, the period was missed;
    /// otherwise it was paid.
    ///
    /// When missing dues finally expels the player, upstream clears CurrentGang and the
    /// kick-up object, so Update() stops early on the following tick and we simply leave
    /// the record in place. Rejoining later resumes from the standing already earned.
    /// </summary>
    private void ObserveDues(Gang gang, GangProgressionSave state, GangKickUp kickUp)
    {
        if (kickUp == null)
        {
            return;
        }
        if (state.DueDateSeen == DateTime.MinValue)
        {
            state.DueDateSeen = kickUp.DueDate;
            state.MissedPeriodsSeen = kickUp.MissedPeriods;
            return;
        }
        if (DateTime.Compare(kickUp.DueDate, state.DueDateSeen) <= 0)
        {
            return; // the period has not rolled over
        }
        int missedNow = kickUp.MissedPeriods;
        bool wasMissed = missedNow > state.MissedPeriodsSeen;

        state.DueDateSeen = kickUp.DueDate;
        state.MissedPeriodsSeen = missedNow;

        if (wasMissed)
        {
            ChangeStanding(gang, -1 * Math.Abs(Config.StandingPerDuesMissed), "dues missed");
        }
        else
        {
            ChangeStanding(gang, Math.Abs(Config.StandingPerDuesPaid), "dues paid");
        }
    }

    // -------------------------------------------------------------------------
    // Promotion
    // -------------------------------------------------------------------------

    /// <summary>
    /// Fires only when the newly reached Order exceeds the PERSISTED high-water mark.
    /// Because the gate is saved rather than held in a transient field, loading a game
    /// cannot replay the promotion — the failure this design exists to avoid.
    ///
    /// HighestRankOrder deliberately does not fall on demotion: it records "already
    /// announced", not "current rank". Losing a rank and regaining it is therefore quiet.
    /// </summary>
    private void CheckPromotion(Gang gang, GangProgressionSave state, int previousOrder)
    {
        GangStandingLevel level = Ladder().GetLevel(state.Standing);
        if (level == null)
        {
            return;
        }
        if (level.Order > state.HighestRankOrder)
        {
            state.HighestRankOrder = level.Order;
            SendPromotionMessage(gang, level);
            Log($"promoted with {gang.ID} to {level.Name} (order {level.Order})");
        }
        else if (level.Order < previousOrder)
        {
            Log($"demoted with {gang.ID} to {level.Name} (order {level.Order})");
        }
    }

    private void SendPromotionMessage(Gang gang, GangStandingLevel level)
    {
        if (gang?.Contact == null || Player?.CellPhone == null)
        {
            return;
        }
        List<string> replies = new List<string>()
        {
            $"Word came down. You're a {level.Name} now. Don't make us regret it.",
            $"You've been earning. Consider yourself a {level.Name}.",
            $"Everyone's saying good things. You're {level.Name} from here.",
            $"You put in the work. {level.Name}. Act like it.",
        };
        string message = replies[Math.Abs(level.Order) % replies.Count];
        Player.CellPhone.AddScheduledText(gang.Contact, message, 1, false);
    }

    // -------------------------------------------------------------------------
    // Save / load
    // -------------------------------------------------------------------------

    public List<GangProgressionSave> GetSaveRecords()
    {
        return States.Where(x => x != null && !string.IsNullOrEmpty(x.GangID)).ToList();
    }

    /// <summary>
    /// Restore from a save. An older save has no records at all, in which case every gang
    /// starts unseeded and the first tick takes a baseline reading rather than awarding.
    /// </summary>
    public void RestoreSaveRecords(List<GangProgressionSave> records)
    {
        States.Clear();
        RecentEvents.Clear();
        if (records == null)
        {
            return;
        }
        foreach (GangProgressionSave record in records)
        {
            if (record == null || string.IsNullOrEmpty(record.GangID))
            {
                continue;
            }
            States.Add(record);
        }
        Log($"restored {States.Count} gang progression record(s)");
    }

    // -------------------------------------------------------------------------
    // Debug helpers — used by the debug menu and the burner phone app
    // -------------------------------------------------------------------------

    // -------------------------------------------------------------------------
    // Internals
    // -------------------------------------------------------------------------

    private GangStandingLevels Ladder()
    {
        GangStandingLevels configured = Config?.StandingLevels;
        return configured ?? new GangStandingLevels();
    }

    private GangProgressionSave FindState(string gangID)
    {
        if (string.IsNullOrEmpty(gangID))
        {
            return null;
        }
        return States.FirstOrDefault(x => x != null && x.GangID != null && x.GangID.Equals(gangID, StringComparison.OrdinalIgnoreCase));
    }

    private GangProgressionSave GetOrCreateState(string gangID)
    {
        GangProgressionSave existing = FindState(gangID);
        if (existing != null)
        {
            return existing;
        }
        GangProgressionSave created = new GangProgressionSave(gangID);
        States.Add(created);
        return created;
    }

    private void RecordEvent(string gangID, int standingDelta, int goodwillDelta, string reason)
    {
        int cap = Config == null || Config.StandingEventLogSize <= 0 ? 20 : Config.StandingEventLogSize;
        RecentEvents.Insert(0, new GangStandingEvent(Time == null ? DateTime.MinValue : Time.CurrentDateTime, gangID, standingDelta, goodwillDelta, reason));
        while (RecentEvents.Count > cap)
        {
            RecentEvents.RemoveAt(RecentEvents.Count - 1);
        }
        if (Config != null && Config.LogStandingChanges)
        {
            Log($"{gangID} standing {standingDelta:+#;-#;0} goodwill {goodwillDelta:+#;-#;0} :: {reason}");
        }
    }

    private static int Clamp(int value, int min, int max)
    {
        if (max < min)
        {
            return min;
        }
        if (value < min)
        {
            return min;
        }
        return value > max ? max : value;
    }

    private void Log(string message)
    {
        EntryPoint.WriteToConsole($"GangProgression: {message}");
    }
}
