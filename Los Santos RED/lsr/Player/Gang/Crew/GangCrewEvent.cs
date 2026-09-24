using System;

/// <summary>
/// One thing that happened to a crew member, kept so the player can look a man up and see
/// what he has actually been through rather than a bare number.
///
/// Persisted, unlike the standing event log — that one is a tuning instrument and dies with
/// the session, but a crew member's history IS the feature. A man you remember surviving
/// three shootouts with you is a different man from one you called yesterday.
///
/// No Rage types.
/// </summary>
[Serializable]
public class GangCrewEvent
{
    public GangCrewEvent()
    {
    }

    public GangCrewEvent(DateTime when, string text, int experienceDelta)
        : this(when, text, experienceDelta, 0)
    {
    }

    public GangCrewEvent(DateTime when, string text, int experienceDelta, int tributeDelta)
    {
        When = when;
        Text = text;
        ExperienceDelta = experienceDelta;
        TributeDelta = tributeDelta;
    }

    public DateTime When { get; set; } = DateTime.MinValue;
    public string Text { get; set; }
    public int ExperienceDelta { get; set; }

    /// <summary>
    /// What this deed put in the man's own pocket. Absent from older saves, which yields 0
    /// from the initialiser — no migration needed.
    /// </summary>
    public int TributeDelta { get; set; }

    public override string ToString() => ExperienceDelta > 0 ? $"{Text} (+{ExperienceDelta})" : Text;
}
