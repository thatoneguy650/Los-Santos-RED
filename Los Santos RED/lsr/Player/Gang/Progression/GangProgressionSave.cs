using System;
using System.Collections.Generic;

/// <summary>
/// Persisted progression record, one per gang, carried on GameSave.
///
/// Deliberately a SEPARATE DTO rather than new fields on GangRepSave: it keeps the
/// whole feature out of GangRepSave, GameSave.SaveReputation, LoadRelationships and
/// SetRepStats, which are all upstream code we would rather not touch.
///
/// Every field defaults to a value that means "new member, nothing earned yet", so an
/// older save with no GangProgressionSaves element deserialises straight into a valid
/// starting state and needs no migration.
///
/// No Rage types.
/// </summary>
[Serializable]
public class GangProgressionSave
{
    public GangProgressionSave()
    {
    }
    public GangProgressionSave(string gangID)
    {
        GangID = gangID;
    }

    public string GangID { get; set; }

    /// <summary>Lifetime earned. Drives rank. Never spent.</summary>
    public int Standing { get; set; } = 0;

    /// <summary>Spendable balance for favours. Credited alongside Standing, drawn down by favours.</summary>
    public int Goodwill { get; set; } = 0;

    /// <summary>
    /// High-water mark of rank Order already announced to the player. Persisted so a save
    /// load cannot re-fire the promotion message — the failure mode this whole design is
    /// built to avoid. Does not fall on demotion; it records "have I been told", not "what am I".
    /// </summary>
    public int HighestRankOrder { get; set; } = 0;

    // --- observation baselines -------------------------------------------------
    // The manager derives standing by watching upstream counters change. These record
    // what was last observed so that a reload does not replay history as a windfall.

    /// <summary>Last observed GangReputation.TasksCompleted.</summary>
    public int TasksCompletedSeen { get; set; } = 0;
    /// <summary>Last observed GangKickUp.MissedPeriods.</summary>
    public int MissedPeriodsSeen { get; set; } = 0;
    /// <summary>Last observed GangKickUp.DueDate, used to detect that a due period rolled over.</summary>
    public DateTime DueDateSeen { get; set; } = DateTime.MinValue;

    /// <summary>
    /// False until the manager has taken its first reading. Prevents the first tick after
    /// joining (or after loading a save written before this feature existed) from treating
    /// an existing TasksCompleted total as freshly earned.
    /// </summary>
    public bool IsSeeded { get; set; } = false;

    // --- requisition cooldowns --------------------------------------------------
    // These live here rather than on GangRepSave for the reason stated at the top of the
    // file, and because this is the only per-gang, persisted, list-shaped DateTime store
    // in the codebase. DateTime.MinValue reads as "never", so every cooldown is elapsed
    // on a save that predates the feature and no migration is needed.

    /// <summary>In-game time the player last drew a weapon from the gang armoury.</summary>
    public DateTime LastWeaponRequisition { get; set; } = DateTime.MinValue;
    /// <summary>In-game time the player last drew body armor.</summary>
    public DateTime LastArmorRequisition { get; set; } = DateTime.MinValue;
    /// <summary>In-game time the player last called for backup.</summary>
    public DateTime LastBackupRequest { get; set; } = DateTime.MinValue;
    /// <summary>In-game time the squad trickle last paid out.</summary>
    public DateTime LastSquadUpkeep { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Lifetime count of requisitioned men who did not come home. Not spent against
    /// anything — it is there so the debug surface can show whether the loss penalty is
    /// firing at all, which is the hard thing to confirm from a standing total alone.
    /// </summary>
    public int BackupMembersLost { get; set; } = 0;

    public override string ToString() => $"{GangID} standing:{Standing} goodwill:{Goodwill} rank:{HighestRankOrder}";
}
