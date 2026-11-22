using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The ladder, plus the lookup that turns a Standing total into a rung.
/// Shaped after LoanParameters: a serialisable list with a lookup that always
/// resolves to something, so a missing or truncated config can never leave a
/// caller without a rank.
///
/// No Rage types. Safe to compile into a headless test project.
/// </summary>
[Serializable]
public class GangStandingLevels
{
    public GangStandingLevels()
    {
    }

    public List<GangStandingLevel> LevelList { get; set; } = new List<GangStandingLevel>();

    /// <summary>
    /// The rung for a given Standing total: the highest Order whose requirement is met.
    /// Falls back to the built-in ladder when the configured list is empty or malformed,
    /// then to a hard floor, so this never returns null.
    /// </summary>
    public GangStandingLevel GetLevel(int standing)
    {
        List<GangStandingLevel> toSearch = UsableLevels();
        GangStandingLevel found = toSearch
            .Where(x => standing >= x.StandingRequired)
            .OrderByDescending(x => x.Order)
            .FirstOrDefault();
        if (found != null)
        {
            return found;
        }
        // Below every threshold: hand back the lowest rung rather than null.
        return toSearch.OrderBy(x => x.Order).FirstOrDefault() ?? Floor();
    }

    /// <summary>The most senior rung on the ladder. Used as the "feature disabled" answer.</summary>
    public GangStandingLevel GetTopLevel()
    {
        return UsableLevels().OrderByDescending(x => x.Order).FirstOrDefault() ?? Floor();
    }

    /// <summary>The next rung up, or null when already at the top.</summary>
    public GangStandingLevel GetNextLevel(int standing)
    {
        GangStandingLevel current = GetLevel(standing);
        return UsableLevels()
            .Where(x => x.Order > current.Order)
            .OrderBy(x => x.Order)
            .FirstOrDefault();
    }

    /// <summary>Standing still needed for the next rung. Zero when already at the top.</summary>
    public int StandingToNextLevel(int standing)
    {
        GangStandingLevel next = GetNextLevel(standing);
        if (next == null)
        {
            return 0;
        }
        int remaining = next.StandingRequired - standing;
        return remaining > 0 ? remaining : 0;
    }

    public GangStandingLevel GetLevelByOrder(int order)
    {
        return UsableLevels().Where(x => x.Order == order).OrderBy(x => x.StandingRequired).FirstOrDefault();
    }

    private List<GangStandingLevel> UsableLevels()
    {
        if (LevelList != null && LevelList.Any(x => x != null))
        {
            return LevelList.Where(x => x != null).ToList();
        }
        return DefaultLadder();
    }

    private static GangStandingLevel Floor()
    {
        return new GangStandingLevel("ASSOCIATE", "Associate", 0, 0);
    }

    /// <summary>
    /// The built-in ladder. Deliberately lives in code rather than only in XML so
    /// that a config file written by an older build, or one that omits the list,
    /// still yields a working ladder instead of locking every rank gate.
    /// </summary>
    public static List<GangStandingLevel> DefaultLadder()
    {
        return new List<GangStandingLevel>()
        {
            new GangStandingLevel("ASSOCIATE",  "Associate",  0, 0),
            new GangStandingLevel("SOLDIER",    "Soldier",    1, 400),
            new GangStandingLevel("ENFORCER",   "Enforcer",   2, 1200),
            new GangStandingLevel("SHOTCALLER", "Shotcaller", 3, 2800),
        };
    }
}
