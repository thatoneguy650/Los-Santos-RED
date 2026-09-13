using System;
using System.Collections.Generic;

/// <summary>
/// One man on the player's roster, remembered across sessions.
///
/// A ped cannot survive a save, but a RECORD can, and a new ped can be dressed in the old
/// one's name and face. That is the whole trick: identity lives here, the body is
/// disposable, and the player sees the same crew because we keep putting the same names
/// and models back on whoever turns up.
///
/// State rules that matter for this type:
///  - ID is minted once from a persisted counter and always restored from the save. It is
///    never derived from list position, or a death would renumber everyone below it.
///  - IsDead is permanent. That is the point of permadeath; nothing may clear it.
///  - Jail is a DATE, not a flag, so it resolves itself on load without a timer surviving.
///
/// No Rage types.
/// </summary>
[Serializable]
public class GangCrewMember
{
    public GangCrewMember()
    {
    }

    public GangCrewMember(int id, string name, string modelName, string gangID, DateTime firstMet)
    {
        ID = id;
        Name = name;
        ModelName = modelName;
        GangID = gangID;
        FirstMetDate = firstMet;
    }

    /// <summary>Monotonic, from GameSave. Never reused, never derived from an index.</summary>
    public int ID { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// The ped model this man was first seen as. Matching on it is how the same face keeps
    /// the same name — the dispatcher chooses models from a small per-gang pool, so once a
    /// model is claimed the pairing tends to hold.
    /// </summary>
    public string ModelName { get; set; }

    public string GangID { get; set; }

    /// <summary>Permanent. Never cleared.</summary>
    public bool IsDead { get; set; } = false;

    /// <summary>MinValue means not jailed. A date rather than a flag so it expires on its own.</summary>
    public DateTime JailedUntilDate { get; set; } = DateTime.MinValue;

    public int TimesDispatched { get; set; } = 0;

    /// <summary>
    /// Earned by being useful: turning up, fighting, and coming home from jobs. Drives Rank,
    /// which is derived rather than stored so it can never disagree with the experience that
    /// produced it.
    /// </summary>
    public int Experience { get; set; } = 0;

    /// <summary>
    /// His own money, from his own rackets, waiting to be kicked up.
    ///
    /// Earned ONLY from things he did — a job that paid, a shootout, getting you clear of the
    /// police. Never from elapsed time: a roster that pays for existing rewards owning men
    /// rather than running with them, which is the opposite of the point.
    ///
    /// Spent, not collected. See GangKickUpManager — the player puts it up on a racket.
    /// </summary>
    public int Tribute { get; set; } = 0;

    /// <summary>Lifetime total, kept so the phone can show who actually earns. Never spent.</summary>
    public int TributeEarnedLifetime { get; set; } = 0;

    /// <summary>What he has been through, newest first. Capped by the manager.</summary>
    public List<GangCrewEvent> Events { get; set; } = new List<GangCrewEvent>();

    /// <summary>
    /// The ladder itself, in one place. Rank and RankOrder both read it, so the name a player
    /// sees and the fighting ability he gets can never disagree — they are the same number
    /// twice. Thresholds are deliberately shallow: this is meant to give a man a reputation
    /// over a few sessions, not to be a levelling treadmill.
    /// </summary>
    private static readonly int[] ExperienceForRank = new int[] { 0, 25, 80, 200, 400 };

    private static readonly string[] RankNames = new string[] { "Untested", "Green", "Reliable", "Solid", "Veteran" };

    /// <summary>Highest rank index whose threshold this man has reached. 0..4, derived, never stored.</summary>
    public int RankOrder
    {
        get
        {
            int order = 0;
            for (int i = 0; i < ExperienceForRank.Length; i++)
            {
                if (Experience >= ExperienceForRank[i]) { order = i; }
            }
            return order;
        }
    }

    /// <summary>Derived, never stored.</summary>
    public string Rank => RankNames[RankOrder];

    /// <summary>How many ranks there are, so callers sizing a table do not hardcode five.</summary>
    public static int RankCount => RankNames.Length;

    /// <summary>The name of an arbitrary rung, for describing a lock the player has not reached.</summary>
    public string RankNameAt(int order)
    {
        if (order < 0) { order = 0; }
        if (order >= RankNames.Length) { order = RankNames.Length - 1; }
        return RankNames[order];
    }

    public DateTime FirstMetDate { get; set; } = DateTime.MinValue;

    // --- the dead --------------------------------------------------------------------

    /// <summary>When he died. MinValue while he is alive.</summary>
    public DateTime DiedDate { get; set; } = DateTime.MinValue;

    /// <summary>Short, and only ever what we could actually determine. Never a guess.</summary>
    public string DeathCause { get; set; }

    /// <summary>Where it happened, for the headstone.</summary>
    public string DeathZone { get; set; }

    /// <summary>
    /// Jobs he was out on that paid. Counted rather than derived from Events, because the
    /// event log is capped and the dead outlive their own history.
    /// </summary>
    public int JobsCompleted { get; set; } = 0;

    /// <summary>
    /// Pinned by the player so pruning cannot drop him. The memorial keeps a fixed number of
    /// men; this is how the player says which ones.
    /// </summary>
    public bool IsRemembered { get; set; } = false;

    public bool IsJailedAt(DateTime now)
    {
        return JailedUntilDate != DateTime.MinValue && DateTime.Compare(now, JailedUntilDate) < 0;
    }

    public bool IsAvailableAt(DateTime now)
    {
        return !IsDead && !IsJailedAt(now);
    }

    public override string ToString() => $"{Name} ({ModelName}) dead:{IsDead} jailed:{JailedUntilDate}";
}
