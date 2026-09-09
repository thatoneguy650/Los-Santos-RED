using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The privilege table and its lookup.
///
/// Shaped after GangStandingLevels for the same reason that class is shaped the way it
/// is: the default table lives INSIDE the lookup, not in a property initialiser. An
/// addon XML that omits a property overwrites it with the C# default rather than falling
/// back to the base file, so a Gangs+_*.xml written before this feature existed would
/// arrive with an empty list. If the fallback lived in an initialiser, every gang from
/// every third-party pack would have no privileges and every gate would silently lock.
/// </summary>
[Serializable]
public class GangRankPrivileges
{
    public GangRankPrivileges()
    {
    }

    public List<GangRankPrivilege> PrivilegeList { get; set; } = new List<GangRankPrivilege>();

    private List<GangRankPrivilege> Usable()
    {
        if (PrivilegeList != null && PrivilegeList.Any())
        {
            return PrivilegeList;
        }
        return DefaultTable();
    }

    /// <summary>
    /// The row for a rank, or the closest rung below it. Never returns null: an Order
    /// above every configured row gets the highest row, and an empty table gets the
    /// built-in one. A caller can always ask "what am I allowed" and get an answer.
    /// </summary>
    public GangRankPrivilege GetPrivilege(int order)
    {
        List<GangRankPrivilege> toSearch = Usable();
        GangRankPrivilege exact = toSearch.FirstOrDefault(x => x != null && x.Order == order);
        if (exact != null)
        {
            return exact;
        }
        GangRankPrivilege below = toSearch.Where(x => x != null && x.Order <= order).OrderByDescending(x => x.Order).FirstOrDefault();
        if (below != null)
        {
            return below;
        }
        GangRankPrivilege lowest = toSearch.Where(x => x != null).OrderBy(x => x.Order).FirstOrDefault();
        return lowest ?? new GangRankPrivilege(0);
    }

    /// <summary>
    /// Associate to Shotcaller. The shape of the arc: an Associate is trusted with a car
    /// and one warm body, a Shotcaller can arm himself properly and bring armored men.
    ///
    /// Weapon categories widen by rung rather than being sub-tiered, because price does
    /// the tiering inside a rung — a Soldier can draw a pistol most days or save for a
    /// shotgun. Costs live in settings so a rebalance is an XML edit, not a rebuild.
    /// </summary>
    public static List<GangRankPrivilege> DefaultTable()
    {
        return new List<GangRankPrivilege>()
        {
            new GangRankPrivilege(0)   // Associate — you are running errands, not raids.
            {
                BackupMaximum = 1,
                BackupGoodwillPerMember = 50,
                BackupGoodwillPerLoss = 150,
                BackupStandingOnWipe = 40,
                BackupCooldownMinutes = 180,
                BackupWeaponCategories = "",          // a body, not a shooter
                BackupArmor = 0,
                BackupHealth = 0,//0 leaves the group settings alone. Rank scales cost, cap, armor and loadout - never how long a man lives; see ApplyBackupLoadout.
                WeaponCategories = "Melee",
                WeaponCooldownHours = 12,
                ArmorItemName = "",
                ArmorGoodwill = 0,
                ArmorCooldownHours = 0,
            },
            new GangRankPrivilege(1)   // Soldier — on the street with a gun.
            {
                BackupMaximum = 1,
                BackupGoodwillPerMember = 75,
                BackupGoodwillPerLoss = 200,
                BackupStandingOnWipe = 60,
                BackupCooldownMinutes = 120,
                BackupWeaponCategories = "Pistol",
                BackupArmor = 0,
                BackupHealth = 0,//0 leaves the group settings alone. Rank scales cost, cap, armor and loadout - never how long a man lives; see ApplyBackupLoadout.
                WeaponCategories = "Melee,Pistol,Shotgun,SMG",
                WeaponCooldownHours = 8,
                ArmorItemName = "Light Body Armor",
                ArmorGoodwill = 200,
                ArmorCooldownHours = 12,
            },
            new GangRankPrivilege(2)   // Enforcer — sent at people deliberately.
            {
                BackupMaximum = 3,
                BackupGoodwillPerMember = 100,
                BackupGoodwillPerLoss = 275,
                BackupStandingOnWipe = 90,
                BackupCooldownMinutes = 90,
                BackupWeaponCategories = "Pistol,Shotgun,SMG,AR",
                BackupArmor = 50,
                BackupHealth = 0,//0 leaves the group settings alone. Rank scales cost, cap, armor and loadout - never how long a man lives; see ApplyBackupLoadout.
                WeaponCategories = "Melee,Pistol,Shotgun,SMG,AR",
                WeaponCooldownHours = 6,
                ArmorItemName = "Medium Body Armor",
                ArmorGoodwill = 300,
                ArmorCooldownHours = 8,
            },
            new GangRankPrivilege(3)   // Shotcaller — the armoury is open.
            {
                BackupMaximum = 4,
                BackupGoodwillPerMember = 125,
                BackupGoodwillPerLoss = 350,
                BackupStandingOnWipe = 120,
                BackupCooldownMinutes = 60,
                BackupWeaponCategories = "AR,SMG,Shotgun",
                BackupArmor = 100,
                BackupHealth = 0,//0 leaves the group settings alone. Rank scales cost, cap, armor and loadout - never how long a man lives; see ApplyBackupLoadout.
                WeaponCategories = "Melee,Pistol,Shotgun,SMG,AR,LMG,Sniper,Heavy,Throwable",
                WeaponCooldownHours = 4,
                ArmorItemName = "Heavy Body Armor",
                ArmorGoodwill = 400,
                ArmorCooldownHours = 6,
            },
        };
    }
}
