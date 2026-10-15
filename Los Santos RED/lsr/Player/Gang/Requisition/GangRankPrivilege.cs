using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// What one rung of a gang's ladder is allowed to ask the gang for.
///
/// Rank decides ACCESS — which categories of weapon are on the menu at all, how many
/// men will come when you call, whether a car is on offer. Goodwill decides FREQUENCY
/// and PRICE. Keeping those two axes apart is what stops a favour from demoting you,
/// which is the same reason Standing and Goodwill are separate integers.
///
/// Category lists are comma-separated strings rather than nested lists because
/// XmlSerializer handles a flat string cleanly in an addon file, and because a mod pack
/// author editing "Pistol,Shotgun,SMG" in a text editor is far less likely to produce
/// something unparseable than one editing nested elements. Unknown names are ignored.
///
/// No Rage types — this is pure data.
/// </summary>
[Serializable]
public class GangRankPrivilege
{
    public GangRankPrivilege()
    {
    }

    public GangRankPrivilege(int order)
    {
        Order = order;
    }

    /// <summary>Matches GangStandingLevel.Order. This is the join key onto the ladder.</summary>
    public int Order { get; set; }

    // --- backup ----------------------------------------------------------------

    /// <summary>
    /// Men who will actually come. Upstream's cap is a hard-coded 7 in four places; this
    /// is deliberately lower at every rung so that cap is never reached and never needs
    /// editing. Note that upstream backup today is far stronger than this — full melee,
    /// sidearm AND long gun regardless of the gang's own weapon percentages, plus 250-350
    /// health and 100-150 armor from GroupSettings. The top rung here is roughly parity
    /// with that; everything below it is something the player climbs toward.
    /// </summary>
    public int BackupMaximum { get; set; }

    /// <summary>Goodwill charged per man dispatched.</summary>
    public int BackupGoodwillPerMember { get; set; }

    /// <summary>Goodwill charged per man who does not come home. Applied on death only.</summary>
    public int BackupGoodwillPerLoss { get; set; }

    /// <summary>Standing lost when the entire squad is wiped out. Ordinary attrition costs none.</summary>
    public int BackupStandingOnWipe { get; set; }

    /// <summary>In-game minutes before backup can be called again.</summary>
    public int BackupCooldownMinutes { get; set; }

    /// <summary>
    /// What the men carry. Empty means unarmed. Applied AFTER GroupManager.Add, because
    /// GroupMember.OnBecameGroupMember would otherwise overwrite armor from GroupSettings.
    /// </summary>
    public string BackupWeaponCategories { get; set; }

    /// <summary>Armor for dispatched men. Overrides GroupSettings.AutoArmor. Zero means none.</summary>
    public int BackupArmor { get; set; }

    /// <summary>Health for dispatched men. Zero or less leaves whatever upstream assigned.</summary>
    public int BackupHealth { get; set; }

    // --- personal requisition ---------------------------------------------------

    /// <summary>
    /// Weapon categories this rung may draw from the armoury. Priced per category from
    /// settings, so "pistol now, shotgun when you can afford it" falls out of the economy
    /// rather than needing sub-ranks.
    /// </summary>
    public string WeaponCategories { get; set; }

    /// <summary>In-game hours between weapon draws.</summary>
    public int WeaponCooldownHours { get; set; }

    /// <summary>Body armor item granted, by ModItem name. Empty means no armor at this rung.</summary>
    public string ArmorItemName { get; set; }

    public int ArmorGoodwill { get; set; }

    public int ArmorCooldownHours { get; set; }

    // --- helpers ----------------------------------------------------------------

    public List<WeaponCategory> ParsedWeaponCategories()
    {
        return ParseCategories(WeaponCategories);
    }

    public List<WeaponCategory> ParsedBackupWeaponCategories()
    {
        return ParseCategories(BackupWeaponCategories);
    }

    /// <summary>
    /// Tolerant on purpose. A category name a future build renames, or a typo in an addon
    /// file, drops out silently rather than throwing during config load — which in this
    /// codebase means during startup, before the player sees anything.
    /// </summary>
    private static List<WeaponCategory> ParseCategories(string raw)
    {
        List<WeaponCategory> parsed = new List<WeaponCategory>();
        if (string.IsNullOrWhiteSpace(raw))
        {
            return parsed;
        }
        foreach (string piece in raw.Split(','))
        {
            string trimmed = piece == null ? "" : piece.Trim();
            if (trimmed == "")
            {
                continue;
            }
            WeaponCategory category;
            if (Enum.TryParse(trimmed, true, out category) && !parsed.Contains(category))
            {
                parsed.Add(category);
            }
        }
        return parsed;
    }

    public override string ToString() => $"Rank {Order} backup:{BackupMaximum} weapons:{WeaponCategories}";
}
