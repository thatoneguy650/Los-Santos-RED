using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The player's roster of familiar faces.
///
/// HOW IDENTITY IS APPLIED. The dispatcher chooses which ped model spawns from a private
/// field it fills itself (GangDispatcher.GetHitSquadSpawnTypes → Gang.GetRandomPed), so
/// forcing a specific man to appear would mean editing the dispatcher. Instead identity is
/// claimed AFTER the spawn: when a ped joins the player's group we look for a roster member
/// of the same gang whose stored model matches, and if we find one, that ped simply IS him
/// — same name, same face. Only when no stored member matches the model do we mint a new
/// one. Since each gang draws from a small pool of models, the roster converges quickly on
/// one man per model and the player starts recognising them.
///
/// The cost of that choice: the game decides who turns up, we only decide who they are. A
/// specific man cannot be requested. That would need the dispatcher to accept a
/// DispatchablePerson, which is a bigger and more invasive change than this feature is
/// worth until the roster itself proves fun.
/// </summary>
public class GangCrewManager
{
    private readonly Mod.Player Player;
    private readonly ITimeReportable Time;
    private readonly ISettingsProvideable Settings;
    private readonly INameProvideable Names;
    private readonly IWeapons Weapons;

    private readonly List<GangCrewMember> Roster = new List<GangCrewMember>();

    /// <summary>Which roster member is currently walking around as which ped.</summary>
    private readonly Dictionary<uint, int> ActiveBodies = new Dictionary<uint, int>();

    /// <summary>
    /// Monotonic ID source, persisted. Restored BEFORE the roster it numbers, so a load can
    /// never mint an ID that is already in use.
    /// </summary>
    private int NextCrewID = 1;

    public GangCrewManager(Mod.Player player, ITimeReportable time, ISettingsProvideable settings, INameProvideable names, IWeapons weapons)
    {
        Weapons = weapons;
        KickUp = new GangKickUpManager(this, player, settings);
        Player = player;
        Time = time;
        Settings = settings;
        Names = names;
    }

    private GangProgressionSettings Config => Settings?.SettingsManager?.GangProgressionSettings;
    public bool IsEnabled => Config != null && Config.EnableGangProgression && Config.EnableGangCrew;
    private int RosterMaximum => Config == null || Config.CrewRosterMaximum <= 0 ? 12 : Config.CrewRosterMaximum;
    private int JailDays => Config == null || Config.CrewJailDays <= 0 ? 3 : Config.CrewJailDays;

    /// <summary>The kick-up racket. Owned here so it costs no player-interface declaration.</summary>
    public GangKickUpManager KickUp { get; private set; }

    public List<GangCrewMember> AllMembers => Roster.ToList();

    public List<GangCrewMember> LivingMembersFor(Gang gang)
    {
        if (gang == null)
        {
            return new List<GangCrewMember>();
        }
        return Roster.Where(x => x != null && !x.IsDead && x.GangID == gang.ID).ToList();
    }

    public void Setup()
    {
    }

    /// <summary>Bodies are disposable; their captured baselines must go with them.</summary>
    private void ForgetBaseline(uint handle)
    {
        if (Baselines.ContainsKey(handle))
        {
            Baselines.Remove(handle);
        }
    }

    public void Reset()
    {
        Baselines.Clear();
        Roster.Clear();
        ActiveBodies.Clear();
        NextCrewID = 1;
    }

    public void Dispose()
    {
        Baselines.Clear();
        ActiveBodies.Clear();
    }

    // -------------------------------------------------------------------------
    // Claiming a body
    // -------------------------------------------------------------------------

    /// <summary>
    /// Give this ped an identity from the roster, minting one if he is a new face.
    /// Called when a man joins the player's group.
    /// </summary>
    public void ClaimIdentity(PedExt gangMember, Gang gang)
    {
        if (!IsEnabled || gangMember?.Pedestrian == null || !gangMember.Pedestrian.Exists() || gang == null)
        {
            return;
        }
        if (ActiveBodies.ContainsKey(gangMember.Handle))
        {
            // Already somebody — but not necessarily still kitted the way his rank says.
            // GangBackup calls GroupManager.Add (which claims him) and only THEN
            // ApplyBackupLoadout, which strips and re-arms the ped. So a man claimed on the
            // way in has his weapons overwritten a moment later. Re-applying here is what
            // makes the weapon tier survive that; the stat work is idempotent anyway.
            int claimedID = ActiveBodies[gangMember.Handle];
            ApplyVeterancy(gangMember, Roster.FirstOrDefault(x => x != null && x.ID == claimedID));
            return;
        }
        string modelName = ModelNameOf(gangMember);
        if (string.IsNullOrEmpty(modelName))
        {
            return;
        }
        DateTime now = Time.CurrentDateTime;

        // Least-dispatched man wins, not the first in the roster.
        //
        // FirstOrDefault meant roster order decided, so for a model several men share, the
        // earliest-created one was claimed EVERY time and the rest never turned out. Four of
        // this roster share G_M_Y_FAMDNF_01: one had six callouts, another had one, and two
        // sat at zero and two. Spreading it by TimesDispatched gives every man a turn.
        //
        // What this cannot fix: a man who is the only living holder of his model gets every
        // spawn of it. The dispatcher picks the model; we only decide who it is.
        GangCrewMember member = Roster.Where(x => x != null
                && x.GangID == gang.ID
                && x.ModelName != null && x.ModelName.Equals(modelName, StringComparison.OrdinalIgnoreCase)
                && x.IsAvailableAt(now)
                && !ActiveBodies.ContainsValue(x.ID))
            .OrderBy(x => x.TimesDispatched)
            .FirstOrDefault();

        if (member == null)
        {
            member = MintMember(gangMember, gang, modelName, now);
        }
        if (member == null)
        {
            return; // roster full
        }

        member.TimesDispatched++;
        ActiveBodies[gangMember.Handle] = member.ID;

        // The two lines the whole feature exists for.
        gangMember.Name = member.Name;
        gangMember.PlayerKnownsName = true;

        ApplyVeterancy(gangMember, member);

        int callXP = Config == null || Config.CrewExperiencePerCallout <= 0 ? 2 : Config.CrewExperiencePerCallout;
        RecordEvent(member, member.TimesDispatched <= 1 ? "Turned out for you" : "Answered the call", callXP);
        Log($"{member.Name} is out with us again (call {member.TimesDispatched}, {member.Rank})");
    }

    /// <summary>
    /// Makes experience mean something.
    ///
    /// WHY THIS SCALES DOWN RATHER THAN UP. Every group member has already been handed the
    /// group standard by GroupMember.OnBecameGroupMember: health from IncreasedHealthMin/Max
    /// (250-350), armor from AutoArmorMin/Max (100-150), and - because AlwaysSetSpecialist
    /// defaults true - SetSpecialist's accuracy 95. There is no headroom above that; a man
    /// cannot be made a better shot than 95. So veterancy asks the opposite question: how
    /// much of that standard has this man EARNED? A stranger fights well below it, and a
    /// Veteran fights at it. The top rung is therefore exactly upstream behaviour.
    ///
    /// COMPUTED FROM A CAPTURED BASELINE, NOT FROM CURRENT VALUES. This is the whole reason
    /// Baselines exists, and it is not a refinement - it is a crash fix. This method runs
    /// TWICE per callout (once when GroupManager.Add claims the man, once after
    /// ApplyBackupLoadout re-arms him), and the first version scaled whatever it found. Two
    /// passes at 40% squared the multiplier: the log showed accuracy 95 -> 38 -> 15, armor
    /// 128 -> 51 -> 0, health 350 -> 140 -> 120. Reading the values back is not idempotent;
    /// remembering what they started as is.
    ///
    /// HEALTH HAS A FLOOR OF 100, NOT 0. DispatchablePerson.SetPedExtPermanentStats builds
    /// every ped as `random(85,125) + 100`. That +100 is the game's effective zero, so a ped
    /// left at 120 health has twenty real hit points and dies to a fender bender - which is
    /// exactly what happened. Only the part ABOVE 100 may be scaled.
    ///
    /// WHAT IS SCALED, AND WHY NOT HEALTH. Rank scales what a man can DO - how straight he
    /// shoots, what he is trusted to carry, how much plate he is given - not how long he
    /// lasts. Health was scaled in the first version and it was the wrong axis, for a reason
    /// that is mechanical rather than aesthetic: identity is claimed AFTER the spawn, so the
    /// player cannot field his veterans or bench his green men. He gets whoever the
    /// dispatcher sends. Scaling survivability by rank therefore penalises him for something
    /// he has no control over - and it spirals, because Solid is 200 experience and Veteran
    /// 400, so a man has to survive a great many callouts to become one, and a fragile man
    /// never does. Nobody would ever own a veteran.
    ///
    /// Armor is still scaled: it is a buffer that is spent within a callout rather than a
    /// life, so a green man having less of it costs him a fight, not his existence.
    /// CrewVeterancyScalesHealth turns the old behaviour back on for anyone who wants it.
    ///
    /// Shoot rate and combat ability are deliberately untouched: both are write-only natives
    /// with no getter, so scaling them would mean copying SetSpecialist's constants here and
    /// drifting from them if upstream retunes.
    /// </summary>
    private void ApplyVeterancy(PedExt gangMember, GangCrewMember member)
    {
        if (member == null || Config == null || !Config.EnableCrewVeterancy)
        {
            return;
        }
        if (gangMember?.Pedestrian == null || !gangMember.Pedestrian.Exists())
        {
            return;
        }
        int percent = VeterancyPercentFor(member.RankOrder);
        try
        {
            VeterancyBaseline baseline = BaselineFor(gangMember);
            if (baseline == null)
            {
                return;
            }
            if (percent < 100)
            {
                int accuracy = Scale(baseline.Accuracy, percent, 1);
                gangMember.Pedestrian.Accuracy = accuracy;
                gangMember.Accuracy = accuracy;

                gangMember.Pedestrian.Armor = Scale(baseline.Armor, percent, 0);

                // Health is NOT scaled by default. See the note on this method: rank scales
                // what a man can DO, not how long he lasts, because the player cannot choose
                // who turns up.
                if (Config.CrewVeterancyScalesHealth)
                {
                    gangMember.Pedestrian.MaxHealth = ScaleHealth(baseline.MaxHealth, percent);
                    if (gangMember.Pedestrian.Health > gangMember.Pedestrian.MaxHealth)
                    {
                        gangMember.Pedestrian.Health = gangMember.Pedestrian.MaxHealth;
                    }
                }
                Log($"{member.Name} is {member.Rank} - {percent}% (acc {accuracy}, armor {gangMember.Pedestrian.Armor}, hp {gangMember.Pedestrian.MaxHealth})");
            }
            ApplyWeaponTier(gangMember.Pedestrian, member);
        }
        catch (Exception e)
        {
            EntryPoint.WriteToConsole($"GangCrew: veterancy failed for {member.Name} {e.Message}", 0);
        }
    }

    /// <summary>What a man's body was worth before we touched it. Keyed by ped, not by member.</summary>
    private class VeterancyBaseline
    {
        public int Accuracy;
        public int Armor;
        public int MaxHealth;
    }

    private readonly Dictionary<uint, VeterancyBaseline> Baselines = new Dictionary<uint, VeterancyBaseline>();

    private VeterancyBaseline BaselineFor(PedExt gangMember)
    {
        uint handle = gangMember.Handle;
        VeterancyBaseline existing;
        if (Baselines.TryGetValue(handle, out existing))
        {
            return existing;
        }
        VeterancyBaseline captured = new VeterancyBaseline
        {
            Accuracy = gangMember.Pedestrian.Accuracy,
            Armor = gangMember.Pedestrian.Armor,
            MaxHealth = gangMember.Pedestrian.MaxHealth,
        };
        Baselines[handle] = captured;
        return captured;
    }

    /// <summary>
    /// Scales only the survivable part. 100 is the game's zero, so a 350 man at 40% keeps
    /// 100 + 250*0.4 = 200, not 140 - which is a hundred real hit points rather than forty.
    /// </summary>
    private int ScaleHealth(int baseline, int percent)
    {
        const int DeathFloor = 100;
        int above = baseline - DeathFloor;
        if (above <= 0)
        {
            return baseline;
        }
        int scaled = DeathFloor + (int)Math.Round(above * percent / 100.0);
        int minimum = Config == null || Config.CrewVeterancyMinimumHealth <= DeathFloor ? 175 : Config.CrewVeterancyMinimumHealth;
        if (minimum > baseline)
        {
            minimum = baseline;
        }
        return scaled < minimum ? minimum : scaled;
    }

    private static int Scale(int value, int percent, int floor)
    {
        int scaled = (int)Math.Round(value * percent / 100.0);
        return scaled < floor ? floor : scaled;
    }

    /// <summary>
    /// The ladder as percentages, one per rank. The fallback lives HERE rather than in the
    /// settings initialiser: an addon XML that omits the element, or supplies a malformed
    /// one, must not be able to leave the table empty and every crew member fighting at zero.
    /// </summary>
    /// <summary>What share of the group's combat standard this man fights at, for display.</summary>
    public int FightingPercentFor(GangCrewMember member)
    {
        if (member == null || Config == null || !Config.EnableCrewVeterancy)
        {
            return 100;
        }
        return VeterancyPercentFor(member.RankOrder);
    }

    private int VeterancyPercentFor(int rankOrder)
    {
        int[] table = ParseRankTable(Config?.CrewVeterancyPercentByRank, DefaultVeterancyPercents);
        if (rankOrder < 0) { rankOrder = 0; }
        if (rankOrder >= table.Length) { rankOrder = table.Length - 1; }
        return table[rankOrder];
    }

    private static readonly int[] DefaultVeterancyPercents = new int[] { 40, 55, 70, 85, 100 };

    private Gang MyGang => Player?.RelationshipManager?.GangRelationships?.CurrentGang;

    // -------------------------------------------------------------------------
    // The memorial
    // -------------------------------------------------------------------------

    /// <summary>
    /// Everyone ever lost, including men the memorial has since dropped. Persisted, because
    /// the whole point of a count is that it survives the pruning.
    /// </summary>
    public int TotalCrewLost { get; private set; }

    public bool MemorialEnabled => IsEnabled && Config != null && Config.EnableCrewMemorial;

    private int MemorialSize => Config == null || Config.CrewMemorialSize <= 0 ? 10 : Config.CrewMemorialSize;

    /// <summary>The dead the memorial still holds, most recent first.</summary>
    public List<GangCrewMember> TheFallen()
    {
        return Roster.Where(x => x != null && x.IsDead)
            .OrderByDescending(x => x.DiedDate)
            .ToList();
    }

    public int MemorialRemembered => Roster.Count(x => x != null && x.IsDead && x.IsRemembered);

    /// <summary>Pin or unpin a man so pruning cannot drop him. Returns the line to show.</summary>
    public string ToggleRemembered(GangCrewMember member)
    {
        if (member == null || !member.IsDead)
        {
            return "";
        }
        if (member.IsRemembered)
        {
            member.IsRemembered = false;
            return $"{member.Name} fades with the rest.";
        }
        if (MemorialRemembered >= MemorialSize)
        {
            return "You can only carry so many.";
        }
        member.IsRemembered = true;
        return $"You won't forget ~g~{member.Name}~s~.";
    }

    /// <summary>
    /// Keep the memorial to its size. Pinned men are never dropped and never counted against
    /// the cull, so a player who has pinned his full ten keeps exactly those; everyone else
    /// falls off oldest first. TotalCrewLost is what survives the pruning.
    /// </summary>
    private void PruneMemorial()
    {
        List<GangCrewMember> dead = Roster.Where(x => x != null && x.IsDead && !x.IsRemembered)
            .OrderByDescending(x => x.DiedDate)
            .ToList();
        int keep = MemorialSize - MemorialRemembered;
        if (keep < 0) { keep = 0; }
        for (int i = keep; i < dead.Count; i++)
        {
            Roster.Remove(dead[i]);
        }
    }

    /// <summary>
    /// Only ever what could actually be determined. There is no "killed by" field to read on
    /// a group member, so this reports the circumstance rather than inventing an assailant.
    /// </summary>
    private string CauseOfDeath(GangMember gangMember)
    {
        try
        {
            if (gangMember != null && gangMember.WasKilledByPlayer)
            {
                return "By your hand";
            }
            if (Player != null && Player.IsWanted)
            {
                return "Shot by police";
            }
            return "Killed in a fight";
        }
        catch
        {
            return "Killed";
        }
    }

    private string ZoneOfDeath()
    {
        try
        {
            return Player?.CurrentLocation?.CurrentZone?.DisplayName ?? "";
        }
        catch
        {
            return "";
        }
    }

    // -------------------------------------------------------------------------
    // Customisation: what a reputation actually buys
    // -------------------------------------------------------------------------

    public bool CustomisationEnabled => IsEnabled && Config != null && Config.EnableCrewCustomisation;

    private int RenameRank => Config == null ? 2 : Config.CrewRenameMinimumRank;
    private int ReskinRank => Config == null ? 3 : Config.CrewReskinMinimumRank;
    private int NameMaximum => Config == null || Config.CrewNameMaximumLength <= 0 ? 24 : Config.CrewNameMaximumLength;

    public bool CanRename(GangCrewMember member) => CustomisationEnabled && member != null && !member.IsDead && member.RankOrder >= RenameRank;

    public bool CanReskin(GangCrewMember member) => CustomisationEnabled && member != null && !member.IsDead && member.RankOrder >= ReskinRank;

    public string RankNameForOrder(int order)
    {
        GangCrewMember probe = new GangCrewMember();
        return probe.RankNameAt(order);
    }

    public string RenameLockedReason() => $"He's got to be {RankNameForOrder(RenameRank)} first.";

    public string ReskinLockedReason() => $"Not until he's {RankNameForOrder(ReskinRank)}.";

    /// <summary>Give a man a name. Returns the line to show the player.</summary>
    public string Rename(GangCrewMember member, string newName)
    {
        if (!CanRename(member))
        {
            return "Not yet.";
        }
        if (string.IsNullOrWhiteSpace(newName))
        {
            return "Left him as he was.";
        }
        newName = newName.Trim();
        if (newName.Length > NameMaximum)
        {
            newName = newName.Substring(0, NameMaximum).Trim();
        }
        if (newName == member.Name)
        {
            return "Left him as he was.";
        }
        string was = member.Name;
        member.Name = newName;
        RecordEvent(member, "You named him", 0);
        Log($"{was} is now {newName}");
        return $"~g~{newName}~s~ it is, then.";
    }

    /// <summary>
    /// The models this man could wear.
    ///
    /// Restricted to his own gang's Personnel, and this is the load-bearing constraint, not a
    /// convenience. Identity is claimed by matching a spawned ped's model against the stored
    /// one — so a man re-skinned to a model his gang never spawns would simply stop turning
    /// up, and the player would have customised him out of existence. The pool IS the set of
    /// faces that can come back.
    /// </summary>
    public List<string> AvailableModelsFor(GangCrewMember member)
    {
        List<string> models = new List<string>();
        if (member == null)
        {
            return models;
        }
        Gang gang = MyGang != null && MyGang.ID == member.GangID ? MyGang : null;
        if (gang?.Personnel == null)
        {
            return models;
        }
        foreach (DispatchablePerson person in gang.Personnel)
        {
            if (person == null || string.IsNullOrEmpty(person.ModelName))
            {
                continue;
            }
            if (!models.Any(x => x.Equals(person.ModelName, StringComparison.OrdinalIgnoreCase)))
            {
                models.Add(person.ModelName);
            }
        }
        return models;
    }

    /// <summary>Is another living man already wearing this face? Not forbidden, but worth saying.</summary>
    public GangCrewMember OtherMemberWearing(GangCrewMember member, string modelName)
    {
        if (string.IsNullOrEmpty(modelName))
        {
            return null;
        }
        return Roster.FirstOrDefault(x => x != null && !x.IsDead && x != member
            && x.ModelName != null && x.ModelName.Equals(modelName, StringComparison.OrdinalIgnoreCase));
    }

    public string Reskin(GangCrewMember member, string modelName)
    {
        if (!CanReskin(member))
        {
            return "Not yet.";
        }
        if (string.IsNullOrEmpty(modelName))
        {
            return "Left him as he was.";
        }
        if (!AvailableModelsFor(member).Any(x => x.Equals(modelName, StringComparison.OrdinalIgnoreCase)))
        {
            // Refusing here rather than trusting the caller: this is the one edit that can
            // make a man unreachable, and the menu is not the only thing that could call it.
            return "Nobody round here looks like that.";
        }
        if (modelName.Equals(member.ModelName, StringComparison.OrdinalIgnoreCase))
        {
            return "Left him as he was.";
        }
        member.ModelName = modelName;
        // He is out with us right now wearing the old face. The record is what persists, so
        // the change shows next time he answers the phone rather than mid-callout.
        RecordEvent(member, "Changed his look", 0);
        Log($"{member.Name} now claims model {modelName}");
        return $"~g~{member.Name}~s~ will look different next time.";
    }

    // -------------------------------------------------------------------------
    // Tribute: what a man earns from what he actually did
    // -------------------------------------------------------------------------

    /// <summary>
    /// Work the PLAYER did with his men standing there.
    ///
    /// Distinct from OnBodyKilledSomeone, which pays a man for his own kill. These are the
    /// jobs where the crew are backup rather than principals — leaning on a dealer, going
    /// through a body, popping a lock. They did not do it, but they were there for it, which
    /// is what backup is, so the whole active squad splits the credit rather than one man
    /// taking it.
    ///
    /// Deliberately pays experience even where it pays no tribute: the player is the one
    /// earning here, and the crew's share of the money is his to hand over, but standing
    /// beside him while he works is exactly how a man gets known.
    /// </summary>
    public void OnPlayerWorkedWithCrew(string eventText, int experience, int tribute)
    {
        if (!IsEnabled || !ActiveBodies.Any() || string.IsNullOrEmpty(eventText))
        {
            return;
        }
        if (!TributeEnabled)
        {
            tribute = 0;
        }
        foreach (int id in ActiveBodies.Values.Distinct().ToList())
        {
            GangCrewMember member = Roster.FirstOrDefault(x => x != null && x.ID == id);
            RecordEvent(member, eventText, experience, tribute);
        }
    }

    /// <summary>A dealer taken down by the player. Paid whether or not he was in a gang.</summary>
    public void OnPlayerTookDealer(bool looted)
    {
        int xp = Config == null || Config.CrewExperiencePerPlayerWork <= 0 ? 4 : Config.CrewExperiencePerPlayerWork;
        int tribute = TributeEnabled ? TributeSetting(Config.CrewTributePerKill, 15) : 0;
        OnPlayerWorkedWithCrew(looted ? "Took a dealer's stash" : "Stood on a dealer", xp, tribute);
    }

    /// <summary>
    /// The player put a rival down with his men standing there.
    ///
    /// OnBodyKilledSomeone pays a man for HIS OWN kill; this is the other half, and it was
    /// missing. A player killed a Ballas on a job with a crew member beside him and the man
    /// earned nothing, which reads as a bug even though every line of code was doing what it
    /// said. Being there for the shooting is the job.
    /// </summary>
    public void OnPlayerKilledRival(Gang victimGang)
    {
        int xp = Config == null || Config.CrewExperiencePerPlayerWork <= 0 ? 4 : Config.CrewExperiencePerPlayerWork;
        int tribute = TributeEnabled ? TributeSetting(Config.CrewTributePerRivalKill, 45) : 0;
        string who = victimGang?.ShortName;
        OnPlayerWorkedWithCrew(string.IsNullOrEmpty(who) ? "Backed you in a shootout" : $"Backed you against {who}", xp, tribute);
    }

    /// <summary>A lock popped by the player with the crew watching the street.</summary>
    public void OnPlayerBrokeIntoVehicle()
    {
        int xp = Config == null || Config.CrewExperiencePerPlayerWork <= 0 ? 4 : Config.CrewExperiencePerPlayerWork;
        OnPlayerWorkedWithCrew("Watched the street", xp, 0);
    }

    /// <summary>
    /// You went through a body while your men were standing there.
    ///
    /// Robbery is the work these men are actually for, so it pays them — but it pays them for
    /// being present at YOUR score, not for a score of their own, which is why the whole
    /// active crew splits it rather than one man taking it. Hooked from PedInspect.LootPed,
    /// the single place the game moves cash and items off a ped.
    ///
    /// Paid only when something was actually taken. Rifling an empty corpse is not a robbery,
    /// and paying for the attempt would make searching every body in Los Santos a payroll.
    /// </summary>
    public void OnBodyLooted(PedExt victim, int cashTaken, bool tookItems)
    {
        if (!TributeEnabled || !ActiveBodies.Any())
        {
            return;
        }
        if (cashTaken <= 0 && !tookItems)
        {
            return;
        }
        int tribute = TributeSetting(Config.CrewTributePerLoot, 25);
        if (GangPedTests.IsRival(victim, MyGang) || GangPedTests.IsShakeableDealer(victim, MyGang))
        {
            tribute *= 2; // a rival or a dealer is carrying something worth taking
        }
        foreach (int id in ActiveBodies.Values.Distinct().ToList())
        {
            GangCrewMember member = Roster.FirstOrDefault(x => x != null && x.ID == id);
            int lootXP = Config == null || Config.CrewExperiencePerPlayerWork <= 0 ? 4 : Config.CrewExperiencePerPlayerWork;
            RecordEvent(member, "Watched you rob a body", lootXP, tribute);
        }
    }

    public bool TributeEnabled => IsEnabled && Config != null && Config.EnableCrewTribute;

    private static int TributeSetting(int configured, int fallback) => configured <= 0 ? fallback : configured;

    private static readonly int[] DefaultTributePercents = new int[] { 60, 80, 100, 120, 150 };

    /// <summary>An established man has better rackets, so the same deed pays him more.</summary>
    private int ScaleTribute(GangCrewMember member, int amount)
    {
        if (member == null || amount <= 0)
        {
            return 0;
        }
        int[] table = ParseRankTable(Config?.CrewTributePercentByRank, DefaultTributePercents);
        int order = member.RankOrder;
        if (order < 0) { order = 0; }
        if (order >= table.Length) { order = table.Length - 1; }
        int scaled = (int)Math.Round(amount * table[order] / 100.0);
        return scaled < 1 ? 1 : scaled;
    }

    /// <summary>
    /// Getting you clear of the police, observed rather than hooked.
    ///
    /// Watches the player's own wanted level, which the mod already maintains, instead of
    /// editing the wanted system to announce itself. The award needs the men to have been
    /// with you WHILE wanted, so the roster is snapshotted when the heat starts and paid
    /// when it ends — otherwise calling backup after the chase would earn the money.
    ///
    /// Anyone arrested is dropped from ActiveBodies by the jail branch below before this
    /// runs, so a man who got picked up is not paid for an escape he did not make.
    /// </summary>
    private void CheckEvasion()
    {
        if (!TributeEnabled || Player == null)
        {
            return;
        }
        DateTime now = Time.CurrentDateTime;
        int minimumStars = Config.CrewEvasionMinimumWantedLevel <= 0 ? 2 : Config.CrewEvasionMinimumWantedLevel;

        if (Player.IsWanted)
        {
            if (!WasWantedWithCrew)
            {
                WasWantedWithCrew = true;
                HeatStartedDate = now;
                HeatPeakWantedLevel = 0;
            }
            if (Player.WantedLevel > HeatPeakWantedLevel)
            {
                HeatPeakWantedLevel = Player.WantedLevel;
            }
            foreach (int id in ActiveBodies.Values.Distinct())
            {
                if (!WantedWitnesses.Contains(id)) { WantedWitnesses.Add(id); }
            }
            return;
        }
        if (!WasWantedWithCrew)
        {
            return;
        }
        WasWantedWithCrew = false;
        List<int> earned = WantedWitnesses.Where(x => ActiveBodies.Values.Contains(x)).ToList();
        WantedWitnesses.Clear();

        // Three guards, all learned from one session that paid this out SIX times. The wanted
        // level flickers to zero and back repeatedly inside a single chase, so a bare
        // wanted-to-not-wanted edge is not an escape — it is a rounding error with a payday
        // attached. Seven men were paid six times each for what the player experienced as one
        // pursuit.
        if (HeatPeakWantedLevel < minimumStars)
        {
            return; // a one-star flicker is not an escape
        }
        int minimumMinutes = Config.CrewEvasionMinimumMinutes <= 0 ? 2 : Config.CrewEvasionMinimumMinutes;
        if (HeatStartedDate == DateTime.MinValue || (now - HeatStartedDate).TotalMinutes < minimumMinutes)
        {
            return; // over too fast to have been a chase
        }
        int cooldown = Config.CrewEvasionCooldownMinutes <= 0 ? 45 : Config.CrewEvasionCooldownMinutes;
        if (LastEvasionPaidDate != DateTime.MinValue && (now - LastEvasionPaidDate).TotalMinutes < cooldown)
        {
            return; // still inside the last one
        }
        if (!earned.Any())
        {
            return;
        }
        LastEvasionPaidDate = now;
        int tribute = TributeSetting(Config.CrewTributePerEvasion, 60);
        foreach (int id in earned)
        {
            GangCrewMember member = Roster.FirstOrDefault(x => x != null && x.ID == id);
            RecordEvent(member, "Lost the cops", 0, tribute);
        }
        Log($"shook {HeatPeakWantedLevel} stars with {earned.Count} of the crew");
    }

    private bool WasWantedWithCrew;
    private DateTime HeatStartedDate = DateTime.MinValue;
    private DateTime LastEvasionPaidDate = DateTime.MinValue;
    private int HeatPeakWantedLevel;
    private readonly List<int> WantedWitnesses = new List<int>();

    /// <summary>Spend a man's tribute. Returns what was actually taken, never more than he has.</summary>
    public int TakeTribute(GangCrewMember member, int amount)
    {
        if (member == null || amount <= 0)
        {
            return 0;
        }
        int taken = amount > member.Tribute ? member.Tribute : amount;
        member.Tribute -= taken;
        return taken;
    }

    public List<GangCrewMember> MembersWithTribute()
    {
        return Roster.Where(x => x != null && !x.IsDead && x.Tribute > 0)
            .OrderByDescending(x => x.Tribute).ToList();
    }

    /// <summary>Melee only, plus a sidearm, plus a long gun. Above this there is nothing left to earn.</summary>
    private const int TopWeaponTier = 2;

    private static readonly int[] DefaultWeaponTiers = new int[] { 0, 1, 1, 2, 2 };

    /// <summary>
    /// What a man is trusted to carry.
    ///
    /// Upstream hands every backup ped melee AND a sidearm AND a long gun regardless of the
    /// gang's own weapon percentages (the comment on GangRequisitionManager.ApplyBackupLoadout
    /// says as much). So, exactly as with the combat stats, there is nothing to add — the
    /// ladder is expressed by taking away, and the top rung leaves the ped untouched.
    ///
    /// Removal is by hash off the mod's own weapon list. GTA has no native that enumerates
    /// what a ped is carrying, so the only honest way to ask "does he have a long gun" is to
    /// ask about each weapon in turn. That is a few hundred HAS_PED_GOT_WEAPON calls once per
    /// callout, not per tick.
    /// </summary>
    private void ApplyWeaponTier(Ped ped, GangCrewMember member)
    {
        if (Weapons == null || ped == null || !ped.Exists() || member == null)
        {
            return;
        }
        int tier = WeaponTierFor(member.RankOrder);
        if (tier >= TopWeaponTier)
        {
            return; // carries what any backup ped carries
        }
        List<WeaponInformation> all = Weapons.GetAllWeapons();
        if (all == null)
        {
            return;
        }
        int removed = 0;
        foreach (WeaponInformation weapon in all)
        {
            if (weapon == null || TierOf(weapon.Category) <= tier)
            {
                continue;
            }
            uint hash = weapon.Hash != 0 ? weapon.Hash : (uint)Game.GetHashKey(weapon.ModelName);
            if (hash == 0)
            {
                continue;
            }
            if (NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(ped, hash, false))
            {
                NativeFunction.Natives.REMOVE_WEAPON_FROM_PED(ped, hash);
                removed++;
            }
        }
        if (removed > 0)
        {
            Log($"{member.Name} ({member.Rank}) is not trusted with that yet - took back {removed} weapon(s), tier {tier}");
        }
    }

    /// <summary>
    /// Which rung of the ladder a weapon category sits on. Throwables and misc ride with
    /// melee deliberately: a molotov is not the thing a man has to earn, a rifle is.
    /// </summary>
    private static int TierOf(WeaponCategory category)
    {
        switch (category)
        {
            case WeaponCategory.Pistol:
                return 1;
            case WeaponCategory.SMG:
            case WeaponCategory.Shotgun:
            case WeaponCategory.AR:
            case WeaponCategory.LMG:
            case WeaponCategory.Sniper:
            case WeaponCategory.Heavy:
                return 2;
            default:
                return 0;
        }
    }

    private int WeaponTierFor(int rankOrder)
    {
        int[] table = ParseRankTable(Config?.CrewVeterancyWeaponTierByRank, DefaultWeaponTiers);
        if (rankOrder < 0) { rankOrder = 0; }
        if (rankOrder >= table.Length) { rankOrder = table.Length - 1; }
        return table[rankOrder];
    }

    /// <summary>
    /// Shared parse for the per-rank tables. The fallback lives HERE rather than in the
    /// settings initialiser, because an addon XML that omits the element or supplies a
    /// malformed one must not be able to leave every crew member disarmed and useless.
    /// </summary>
    private static int[] ParseRankTable(string configured, int[] fallback)
    {
        if (string.IsNullOrWhiteSpace(configured))
        {
            return fallback;
        }
        List<int> parsed = new List<int>();
        foreach (string piece in configured.Split(','))
        {
            int value;
            if (!int.TryParse(piece.Trim(), out value) || value < 0)
            {
                return fallback;
            }
            parsed.Add(value);
        }
        return parsed.Count == GangCrewMember.RankCount ? parsed.ToArray() : fallback;
    }

    private GangCrewMember MintMember(PedExt gangMember, Gang gang, string modelName, DateTime now)
    {
        if (Roster.Count(x => x != null && !x.IsDead) >= RosterMaximum)
        {
            return null;
        }
        // Ask the ped what it is rather than assuming. The first roster member minted was
        // a woman handed a man's name, which is exactly the sort of detail that breaks the
        // familiarity this feature exists to build.
        bool isMale = true;
        try { isMale = gangMember?.Pedestrian == null || !gangMember.Pedestrian.Exists() || gangMember.Pedestrian.IsMale; } catch { }
        string name = Names == null ? "Unknown" : Names.GetRandomName(isMale);
        GangCrewMember member = new GangCrewMember(NextCrewID, name, modelName, gang.ID, now);
        NextCrewID++;
        Roster.Add(member);
        Log($"{name} joined the crew");
        return member;
    }

    /// <summary>
    /// Add to a man's history and his experience. Newest first, capped — a roster of twelve
    /// men each carrying an unbounded history would bloat every save.
    /// </summary>
    public void RecordEvent(GangCrewMember member, string text, int experienceDelta)
    {
        RecordEvent(member, text, experienceDelta, 0);
    }

    /// <summary>
    /// One deed, recorded once, paying both currencies.
    ///
    /// Experience and tribute are deliberately awarded from the SAME call rather than from
    /// separate hooks. Two ledgers fed by two code paths drift the moment one of them gains
    /// a caller the other does not, and the player then sees a man whose history does not
    /// explain his money.
    /// </summary>
    public void RecordEvent(GangCrewMember member, string text, int experienceDelta, int tributeDelta)
    {
        if (member == null || string.IsNullOrEmpty(text))
        {
            return;
        }
        if (member.Events == null)
        {
            member.Events = new List<GangCrewEvent>();
        }
        if (tributeDelta > 0)
        {
            tributeDelta = ScaleTribute(member, tributeDelta);
            member.Tribute += tributeDelta;
            member.TributeEarnedLifetime += tributeDelta;
        }
        else
        {
            tributeDelta = 0;
        }
        member.Experience += experienceDelta;
        member.Events.Insert(0, new GangCrewEvent(Time.CurrentDateTime, text, experienceDelta, tributeDelta));
        int cap = Config == null || Config.CrewEventLogSize <= 0 ? 10 : Config.CrewEventLogSize;
        while (member.Events.Count > cap)
        {
            member.Events.RemoveAt(member.Events.Count - 1);
        }
    }

    /// <summary>
    /// A man of ours put somebody down while he was out with us.
    ///
    /// Hooked from GangMember.OnKilledPed, which already fires for the inter-gang aggression
    /// tracking, so this costs no new hook. Being in ActiveBodies is what makes it ours —
    /// a gang member killing someone across town is not our man earning his keep.
    /// </summary>
    public void OnBodyKilledSomeone(GangMember killer, PedExt victim)
    {
        if (!IsEnabled || killer == null)
        {
            return;
        }
        GangCrewMember member = MemberFor(killer.Handle);
        if (member == null)
        {
            return;
        }
        Gang myGang = MyGang;
        bool wasRival = GangPedTests.IsRival(victim, myGang);
        bool wasDealer = !wasRival && GangPedTests.IsShakeableDealer(victim, myGang);
        if (!wasRival && !wasDealer)
        {
            // A dead bystander is not a day's work. Paying for it would make the cheapest
            // possible act — walking up to anybody and shooting them — the fastest way to
            // level a man, which is neither the fantasy nor anything worth rewarding.
            return;
        }
        int xp = Config == null || Config.CrewExperiencePerKill <= 0 ? 5 : Config.CrewExperiencePerKill;
        string who = (victim as GangMember)?.Gang?.ShortName;
        int tribute = !TributeEnabled ? 0
            : wasRival ? TributeSetting(Config.CrewTributePerRivalKill, 45)
            : TributeSetting(Config.CrewTributePerKill, 15);
        RecordEvent(member, wasRival && !string.IsNullOrEmpty(who) ? $"Killed a {who}"
            : wasRival ? "Killed a rival"
            : "Killed a dealer", xp, tribute);
    }

    /// <summary>Everyone still out with us shares the credit for a finished job.</summary>
    public void OnJobCompleted()
    {
        if (!IsEnabled || !ActiveBodies.Any())
        {
            return;
        }
        int xp = Config == null || Config.CrewExperiencePerJob <= 0 ? 15 : Config.CrewExperiencePerJob;
        int tribute = TributeEnabled ? TributeSetting(Config.CrewTributePerJob, 120) : 0;
        foreach (int id in ActiveBodies.Values.Distinct().ToList())
        {
            GangCrewMember member = Roster.FirstOrDefault(x => x != null && x.ID == id);
            if (member != null) { member.JobsCompleted++; }
            RecordEvent(member, "Job paid out", xp, tribute);
        }
    }

    private string ModelNameOf(PedExt gangMember)
    {
        try
        {
            return gangMember.Pedestrian.Model.Name;
        }
        catch
        {
            return null;
        }
    }

    // -------------------------------------------------------------------------
    // Losing people
    // -------------------------------------------------------------------------

    /// <summary>Permanent. Called from the death hook, which fires exactly once.</summary>
    public void OnBodyKilled(GangMember gangMember)
    {
        if (!IsEnabled || gangMember == null)
        {
            return;
        }
        GangCrewMember member = MemberFor(gangMember.Handle);
        if (member == null)
        {
            return;
        }
        member.IsDead = true;
        member.DiedDate = Time.CurrentDateTime;
        member.DeathCause = CauseOfDeath(gangMember);
        member.DeathZone = ZoneOfDeath();
        TotalCrewLost++;
        RecordEvent(member, "Didn't make it", 0);
        ActiveBodies.Remove(gangMember.Handle);
        ForgetBaseline(gangMember.Handle);
        PruneMemorial();
        Log($"{member.Name} is dead ({member.DeathCause}, {member.DeathZone}). He is not coming back.");
        Player?.CellPhone?.AddScheduledText(GangContactFor(member), $"We lost {member.Name}. That's on you.", 2, false);
    }

    /// <summary>
    /// Watch the men we have out for an arrest.
    ///
    /// Polled rather than hooked because nothing raises an event when a ped is busted —
    /// PedExt.IsBusted and IsArrested are plain flags set during the arrest sequence. Ticked
    /// from the progression manager's existing 2s task rather than adding another.
    /// </summary>
    public void Update()
    {
        if (!IsEnabled)
        {
            return;
        }
        // Before the early-out: the heat can end after the last man has gone out of range,
        // and the payout is owed to whoever was standing there when it started.
        CheckEvasion();
        if (!ActiveBodies.Any())
        {
            return;
        }
        DateTime now = Time.CurrentDateTime;

        // ARREST IS CHECKED FIRST, AND THAT ORDER IS THE WHOLE POINT.
        //
        // GroupManager.Update removes a busted member from the group itself, so if the
        // reconciliation below ran first it would see him missing, quietly drop him as
        // "wandered off", and the jail record would never be written. That is exactly what
        // happened: a man was arrested mid-job and his JailedUntilDate stayed at MinValue.
        // Losing somebody to the police has to be observed before the group forgets him.
        List<GangMember> live = Player?.World?.Pedestrians?.GangMemberList;
        foreach (uint handle in ActiveBodies.Keys.ToList())
        {
            GangMember body = live?.FirstOrDefault(x => x != null && x.Handle == handle);
            if (body == null)
            {
                continue; // out of range, not necessarily gone
            }
            if (body.IsBusted || body.IsArrested)
            {
                GangCrewMember member = MemberFor(handle);
                ActiveBodies.Remove(handle);
                ForgetBaseline(handle);
                if (member != null && !member.IsDead)
                {
                    member.JailedUntilDate = now.AddDays(JailDays);
                    RecordEvent(member, $"Jailed {member.JailedUntilDate:M/d}", 0);
                    Log($"{member.Name} got picked up. Out on {member.JailedUntilDate:d}.");
                    Player?.CellPhone?.AddScheduledText(GangContactFor(member), $"{member.Name} got picked up. He'll be inside a few days.", 2, false);
                }
            }
        }

        // Then reconcile against the GROUP, which is the only thing that actually knows who
        // is still with the player. ActiveBodies used to be removed from only on death and
        // arrest, so a man who despawned or wandered off stayed on the books all session and
        // every later job, evasion and robbery paid him — "shook the police with 6 of the
        // crew", then 7; the number only ever went up.
        List<GroupMember> group = Player?.GroupManager?.CurrentGroupMembers;
        if (group != null)
        {
            foreach (uint handle in ActiveBodies.Keys.ToList())
            {
                if (!group.Any(x => x?.PedExt != null && x.PedExt.Handle == handle))
                {
                    ActiveBodies.Remove(handle);
                    ForgetBaseline(handle);
                }
            }
        }
    }

    private GangCrewMember MemberFor(uint handle)
    {
        int id;
        if (!ActiveBodies.TryGetValue(handle, out id))
        {
            return null;
        }
        return Roster.FirstOrDefault(x => x != null && x.ID == id);
    }

    private PhoneContact GangContactFor(GangCrewMember member)
    {
        return Player?.RelationshipManager?.GangRelationships?.CurrentGang?.Contact;
    }

    // -------------------------------------------------------------------------
    // Save / load
    // -------------------------------------------------------------------------

    public List<GangCrewMember> GetSaveRecords()
    {
        return Roster.Where(x => x != null).ToList();
    }

    public int GetNextID() => NextCrewID;

    public int GetTotalLost() => TotalCrewLost;

    /// <summary>
    /// Restore the counter BEFORE the roster, so an ID can never be minted that a restored
    /// member already holds. Reading it back from the records instead would break the moment
    /// the highest-numbered man died and was pruned.
    /// </summary>
    public void RestoreSaveRecords(List<GangCrewMember> records, int nextID, int totalLost)
    {
        Roster.Clear();
        ActiveBodies.Clear();
        Baselines.Clear();
        NextCrewID = nextID <= 0 ? 1 : nextID;
        TotalCrewLost = totalLost < 0 ? 0 : totalLost;
        if (records == null)
        {
            return;
        }
        foreach (GangCrewMember record in records)
        {
            if (record != null && record.ID > 0)
            {
                Roster.Add(record);
            }
        }
        int living = Roster.Count(x => !x.IsDead);
        // An older save has no lost count. The dead still on the roster are the floor.
        int deadHeld = Roster.Count(x => x.IsDead);
        if (TotalCrewLost < deadHeld) { TotalCrewLost = deadHeld; }
        Log($"restored {Roster.Count} crew record(s), {living} still breathing, {TotalCrewLost} lost, next id {NextCrewID}");
    }

    private void Log(string message)
    {
        if (Config != null && Config.LogStandingChanges)
        {
            EntryPoint.WriteToConsole($"GangCrew: {message}", 5);
        }
    }
}
