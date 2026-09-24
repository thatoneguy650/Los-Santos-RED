using System;
using System.Collections.Generic;

/// <summary>
/// One rung on a gang's internal ladder. Everything above <see cref="GangRespect.Member"/>.
/// Pure data, no Rage types, so it can be exercised in a headless test project.
/// </summary>
[Serializable]
public class GangStandingLevel
{
    public GangStandingLevel()
    {
    }
    public GangStandingLevel(string id, string name, int order, int standingRequired)
    {
        ID = id;
        Name = name;
        Order = order;
        StandingRequired = standingRequired;
    }

    /// <summary>Stable identifier. Never shown to the player; used for save comparison and config overrides.</summary>
    public string ID { get; set; } = "UNK";
    /// <summary>Display name. Free to differ per gang flavour without changing behaviour.</summary>
    public string Name { get; set; } = "Unknown";
    /// <summary>Position on the ladder. Higher is more senior. Compared numerically, unlike GangRespect.</summary>
    public int Order { get; set; } = 0;
    /// <summary>Minimum Standing to hold this rung.</summary>
    public int StandingRequired { get; set; } = 0;

    public override string ToString() => $"{Order}:{Name} ({StandingRequired})";
}
