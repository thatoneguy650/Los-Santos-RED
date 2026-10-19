using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// What the gang will hand you, and what it costs.
///
/// Rank decides access, goodwill decides price and frequency. Every entry point returns
/// a refusal STRING (null meaning success) rather than a bool, because the den's own
/// idiom — see GangDen.DropoffKick — is to give the player a distinct in-character
/// reason for each way a request can fail. "Come back when you actually have the cash"
/// and "not time yet" are different sentences, and collapsing them into false loses the
/// only thing that makes a refusal feel like a person rather than a locked door.
///
/// Charging happens LAST, after every check and after the thing actually succeeded, so
/// a failed spawn or an empty armoury can never take the player's goodwill.
/// </summary>
public class GangRequisitionManager
{
    private readonly Mod.Player Player;
    private readonly ITimeReportable Time;
    private readonly ISettingsProvideable Settings;
    private readonly IModItems ModItems;
    private readonly IWeapons Weapons;
    private readonly IEntityProvideable World;

    /// <summary>
    /// Handles of men the gang actually dispatched for us, per gang.
    ///
    /// This exists because GangBackup.GetBackupMembers scrapes any same-gang mod-spawned
    /// ped that is in a vehicle or within 55m, so the squad routinely contains bystanders
    /// nobody paid for. Charging the player when one of those dies would be charging him
    /// for a stranger's bad luck. Only handles in here are ours.
    /// </summary>
    private readonly Dictionary<string, List<uint>> LiveRequisitioned = new Dictionary<string, List<uint>>();

    /// <summary>
    /// How many men the player has actually PAID for, per gang.
    ///
    /// This exists because of a trap in GangDispatcher.DispatchGangBackup: when the
    /// player is within 150m of his own gang's den it takes a completely different
    /// branch and spawns with a ped limit of 99, ignoring the requested count entirely.
    /// Every one of those is flagged as backup. Without this cap the player could pay
    /// for one man, be handed ninety, and then be billed for ninety deaths.
    /// </summary>
    private readonly Dictionary<string, int> AuthorizedCount = new Dictionary<string, int>();

    public GangRequisitionManager(Mod.Player player, ITimeReportable time, ISettingsProvideable settings, IModItems modItems, IWeapons weapons, IEntityProvideable world)
    {
        Player = player;
        Time = time;
        Settings = settings;
        ModItems = modItems;
        Weapons = weapons;
        World = world;
    }

    private GangProgressionSettings Config => Settings?.SettingsManager?.GangProgressionSettings;
    private GangProgressionManager Progression => Player?.GangProgressionManager;
    private bool ProgressionOn => Config != null && Config.EnableGangProgression;

    public bool IsEnabled => ProgressionOn && Config.EnableRequisition;
    public bool BackupLimitsEnabled => ProgressionOn && Config.EnableBackupLimits;
    public bool LossPenaltyEnabled => BackupLimitsEnabled && Config.EnableBackupLossPenalty;

    public void Setup()
    {
    }

    public void Reset()
    {
        LiveRequisitioned.Clear();
        AuthorizedCount.Clear();
    }

    /// <summary>
    /// Called when the backup manager tears every squad down — disbanding, or a respawn.
    /// Men the player has released are no longer his liability, so the books close rather
    /// than leaving stale handles that would bill him for a death hours later.
    /// </summary>
    public void ReleaseAllSquads()
    {
        LiveRequisitioned.Clear();
        AuthorizedCount.Clear();
    }

    public void Dispose()
    {
        LiveRequisitioned.Clear();
        AuthorizedCount.Clear();
    }

    public GangRankPrivilege PrivilegeFor(Gang gang)
    {
        return Progression == null ? new GangRankPrivilege(0) : Progression.GetPrivilege(gang);
    }

    // -------------------------------------------------------------------------
    // Cooldowns
    // -------------------------------------------------------------------------

    /// <summary>
    /// True when the wait is over. Argument order follows GangKickUp.IsPassedDueDate —
    /// "now" first, deadline second, >= 0 meaning we are past it. The codebase uses both
    /// orderings for this and they read identically, so it is worth being explicit.
    /// </summary>
    private bool Elapsed(DateTime last, double hours)
    {
        if (last == DateTime.MinValue)
        {
            return true;
        }
        return DateTime.Compare(Time.CurrentDateTime, last.AddHours(hours)) >= 0;
    }

    private string WaitMessage(DateTime last, double hours, string what)
    {
        DateTime ready = last.AddHours(hours);
        TimeSpan remaining = ready - Time.CurrentDateTime;
        if (remaining.TotalHours >= 1.0)
        {
            return $"Too soon for another {what}. Try again in about {(int)Math.Ceiling(remaining.TotalHours)} hours.";
        }
        return $"Too soon for another {what}. Give it {Math.Max(1, (int)Math.Ceiling(remaining.TotalMinutes))} more minutes.";
    }

    // -------------------------------------------------------------------------
    // Weapons
    // -------------------------------------------------------------------------

    private Dictionary<WeaponCategory, int> WeaponCosts()
    {
        Dictionary<WeaponCategory, int> costs = new Dictionary<WeaponCategory, int>();
        string raw = Config?.WeaponGoodwillCosts;
        if (string.IsNullOrWhiteSpace(raw))
        {
            return costs;
        }
        foreach (string pair in raw.Split(','))
        {
            string[] halves = pair == null ? null : pair.Split(':');
            if (halves == null || halves.Length != 2)
            {
                continue;
            }
            WeaponCategory category;
            int cost;
            if (Enum.TryParse(halves[0].Trim(), true, out category) && int.TryParse(halves[1].Trim(), out cost))
            {
                costs[category] = cost;
            }
        }
        return costs;
    }

    public int WeaponCostFor(WeaponCategory category)
    {
        Dictionary<WeaponCategory, int> costs = WeaponCosts();
        int cost;
        return costs.TryGetValue(category, out cost) ? cost : 0;
    }

    /// <summary>
    /// What one specific weapon costs. The category price unless the model is listed in
    /// WeaponGoodwillModelCosts — a bat and a switchblade are both melee, and pricing them
    /// the same is what made the whole category read as one item.
    /// </summary>
    public int WeaponCostFor(IssuableWeapon issuable)
    {
        if (issuable == null)
        {
            return 0;
        }
        string raw = Config?.WeaponGoodwillModelCosts;
        if (!string.IsNullOrWhiteSpace(raw))
        {
            foreach (string pair in raw.Split(','))
            {
                string[] halves = pair == null ? null : pair.Split(':');
                if (halves == null || halves.Length != 2)
                {
                    continue;
                }
                int modelCost;
                if (halves[0].Trim().Equals(issuable.ModelName, StringComparison.OrdinalIgnoreCase)
                    && int.TryParse(halves[1].Trim(), out modelCost))
                {
                    return modelCost;
                }
            }
        }
        return WeaponCostFor(CategoryOf(issuable));
    }

    /// <summary>Every weapon this rank may draw and the gang actually stocks, cheapest first.</summary>
    /// <summary>
    /// What the den will actually hand you: one plain weapon per category, not a catalogue.
    ///
    /// This used to enumerate every model in the gang's armoury, which produced a scroller
    /// dozens of entries long where most of the choices were cosmetic — three near-identical
    /// pistols is a chaotic menu, not a decision. What an outfit gives somebody it is sending
    /// out tonight is a bat, a pistol, maybe a Luzi: serviceable, disposable, nobody's pride.
    /// The interesting choice is which TIER you have earned, so the menu now shows exactly
    /// that and nothing else.
    ///
    /// The per-model armoury is untouched and still feeds ApplyBackupLoadout — the men who
    /// turn up for you keep drawing gang-flavoured weapons. Only the player's own menu is
    /// fixed.
    /// </summary>
    public List<IssuableWeapon> AvailableWeapons(Gang gang)
    {
        List<WeaponCategory> allowed = PrivilegeFor(gang).ParsedWeaponCategories();
        List<IssuableWeapon> issue = new List<IssuableWeapon>();
        foreach (string modelName in IssueWeaponModels())
        {
            if (Weapons?.GetWeapon(modelName) == null)
            {
                continue; // a model this install does not have is silently skipped, never shown
            }
            IssuableWeapon issuable = new IssuableWeapon(modelName, null);
            if (!allowed.Contains(CategoryOf(issuable)))
            {
                continue;
            }
            if (issue.Any(x => x.ModelName.Equals(modelName, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }
            issue.Add(issuable);
        }
        return issue.OrderBy(x => WeaponCostFor(x)).ToList();
    }

    /// <summary>
    /// The issue list. The fallback lives HERE rather than in the settings initialiser: an
    /// addon XML that omits the element would otherwise overwrite it with an empty string and
    /// leave the armoury silently bare, which reads as a broken feature rather than a
    /// configuration choice.
    /// </summary>
    private List<string> IssueWeaponModels()
    {
        string configured = Config?.RequisitionIssueWeapons;
        if (!string.IsNullOrWhiteSpace(configured))
        {
            List<string> parsed = configured.Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
            if (parsed.Any())
            {
                return parsed;
            }
        }
        return DefaultIssueWeapons;
    }

    private static readonly List<string> DefaultIssueWeapons = new List<string>
    {
        "WEAPON_BAT",            // Melee
        "WEAPON_PISTOL",         // Pistol
        "WEAPON_MICROSMG",       // SMG
        "WEAPON_PUMPSHOTGUN",    // Shotgun
        "WEAPON_ASSAULTRIFLE",   // AR
    };

    /// <summary>
    /// The readable name. WeaponInformation carries only ModelName — the human-facing name
    /// lives on the ModItem, which is what every shop menu displays.
    /// </summary>
    public string DisplayNameFor(IssuableWeapon issuable)
    {
        if (issuable == null)
        {
            return "";
        }
        WeaponItem weaponItem = ModItems?.GetWeapon(issuable.ModelName);
        return weaponItem != null && !string.IsNullOrEmpty(weaponItem.Name) ? weaponItem.Name : issuable.ModelName;
    }

    /// <summary>
    /// Everything in the gang's own three weapon groups, flattened. The gang can only
    /// hand over what it actually stocks, so an "allowed" category the gang has no guns
    /// for is honestly reported as empty rather than quietly substituted from elsewhere.
    /// </summary>
    private List<IssuableWeapon> GangArmoury(Gang gang)
    {
        List<IssuableWeapon> all = new List<IssuableWeapon>();
        if (gang == null)
        {
            return all;
        }
        if (gang.MeleeWeapons != null) { all.AddRange(gang.MeleeWeapons.Where(x => x != null)); }
        if (gang.SideArms != null) { all.AddRange(gang.SideArms.Where(x => x != null)); }
        if (gang.LongGuns != null) { all.AddRange(gang.LongGuns.Where(x => x != null)); }
        return all;
    }

    private WeaponCategory CategoryOf(IssuableWeapon issuable)
    {
        WeaponInformation info = issuable == null ? null : Weapons?.GetWeapon(issuable.ModelName);
        return info == null ? WeaponCategory.Unknown : info.Category;
    }

    /// <summary>Categories this rank may draw AND the gang can actually supply.</summary>
    public List<WeaponCategory> AvailableWeaponCategories(Gang gang)
    {
        List<WeaponCategory> allowed = PrivilegeFor(gang).ParsedWeaponCategories();
        List<IssuableWeapon> armoury = GangArmoury(gang);
        return allowed.Where(c => armoury.Any(w => CategoryOf(w) == c)).ToList();
    }

    /// <summary>Null on success, otherwise the reason to show the player.</summary>
    public string RequestWeapon(Gang gang, IssuableWeapon chosen)
    {
        if (!IsEnabled || gang == null || Progression == null || chosen == null)
        {
            return "Not available.";
        }
        GangRankPrivilege privilege = PrivilegeFor(gang);
        if (!privilege.ParsedWeaponCategories().Contains(CategoryOf(chosen)))
        {
            return "You're not carrying that kind of weight for us yet.";
        }
        GangProgressionSave record = Progression.GetRecord(gang);
        if (record == null)
        {
            return "Not available.";
        }
        if (!Elapsed(record.LastWeaponRequisition, privilege.WeaponCooldownHours))
        {
            return WaitMessage(record.LastWeaponRequisition, privilege.WeaponCooldownHours, "piece");
        }
        int modelCost = WeaponCostFor(chosen);
        if (!Progression.CanAfford(gang, modelCost))
        {
            return $"That'll run you {modelCost} in goodwill. Come back when you've earned it.";
        }
        if (!GiveWeapon(chosen))
        {
            return "Couldn't get that to you.";
        }
        Progression.SpendGoodwill(gang, modelCost, $"requisition {DisplayNameFor(chosen)}");
        record.LastWeaponRequisition = Time.CurrentDateTime;
        return null;
    }

    private string RequestWeaponByCategory(Gang gang, WeaponCategory category)
    {
        if (!IsEnabled || gang == null || Progression == null)
        {
            return "Not available.";
        }
        GangRankPrivilege privilege = PrivilegeFor(gang);
        if (!privilege.ParsedWeaponCategories().Contains(category))
        {
            return "You're not carrying that kind of weight for us yet.";
        }
        GangProgressionSave record = Progression.GetRecord(gang);
        if (record == null)
        {
            return "Not available.";
        }
        if (!Elapsed(record.LastWeaponRequisition, privilege.WeaponCooldownHours))
        {
            return WaitMessage(record.LastWeaponRequisition, privilege.WeaponCooldownHours, "piece");
        }
        List<IssuableWeapon> candidates = GangArmoury(gang).Where(x => CategoryOf(x) == category).ToList();
        if (!candidates.Any())
        {
            return "We're dry on those right now.";
        }
        int cost = WeaponCostFor(category);
        if (!Progression.CanAfford(gang, cost))
        {
            return $"That'll run you {cost} in goodwill. Come back when you've earned it.";
        }

        IssuableWeapon chosen = PickWeighted(candidates);
        if (!GiveWeapon(chosen))
        {
            return "Couldn't get that to you.";
        }
        Progression.SpendGoodwill(gang, cost, $"requisition {category}");
        record.LastWeaponRequisition = Time.CurrentDateTime;
        return null;
    }

    private IssuableWeapon PickWeighted(List<IssuableWeapon> candidates)
    {
        int total = candidates.Sum(x => Math.Max(1, x.SpawnChance));
        int roll = RandomItems.MyRand.Next(0, total);
        int running = 0;
        foreach (IssuableWeapon candidate in candidates)
        {
            running += Math.Max(1, candidate.SpawnChance);
            if (roll < running)
            {
                return candidate;
            }
        }
        return candidates.FirstOrDefault();
    }

    private bool GiveWeapon(IssuableWeapon issuable)
    {
        if (issuable == null || Player?.Character == null || !Player.Character.Exists())
        {
            return false;
        }
        WeaponInformation info = Weapons?.GetWeapon(issuable.ModelName);
        if (info == null)
        {
            return false;
        }
        // Mirrors Gang.GetRandomWeapon: SetIssued is what populates ModelHash, and without
        // it GetHash() returns 0 and the native silently does nothing.
        issuable.SetIssued(Game.GetHashKey(issuable.ModelName), info.PossibleComponents, info.IsTaser);
        NativeFunction.Natives.GIVE_WEAPON_TO_PED(Player.Character, issuable.GetHash(), info.AmmoAmount, false, false);
        issuable.ApplyVariation(Player.Character);
        return true;
    }

    // -------------------------------------------------------------------------
    // Body armor
    // -------------------------------------------------------------------------

    public string ArmorItemNameFor(Gang gang) => PrivilegeFor(gang).ArmorItemName;

    public string RequestArmor(Gang gang)
    {
        if (!IsEnabled || gang == null || Progression == null)
        {
            return "Not available.";
        }
        GangRankPrivilege privilege = PrivilegeFor(gang);
        if (string.IsNullOrWhiteSpace(privilege.ArmorItemName))
        {
            return "Plates are for people who've earned them.";
        }
        GangProgressionSave record = Progression.GetRecord(gang);
        if (record == null)
        {
            return "Not available.";
        }
        if (!Elapsed(record.LastArmorRequisition, privilege.ArmorCooldownHours))
        {
            return WaitMessage(record.LastArmorRequisition, privilege.ArmorCooldownHours, "vest");
        }
        if (!Progression.CanAfford(gang, privilege.ArmorGoodwill))
        {
            return $"That'll run you {privilege.ArmorGoodwill} in goodwill. Come back when you've earned it.";
        }
        BodyArmorItem armor = ModItems?.Get(privilege.ArmorItemName) as BodyArmorItem;
        if (armor == null)
        {
            return "We're out of plates.";
        }
        // Into the inventory rather than straight onto the body: it survives a save, and
        // the player chooses when to put it on, which is how every other armor in the mod
        // behaves. ArmorManager.RemoveArmor uses this same call to hand a plate back.
        Player.Inventory.Add(armor, 1.0f);
        Progression.SpendGoodwill(gang, privilege.ArmorGoodwill, "requisition armor");
        record.LastArmorRequisition = Time.CurrentDateTime;
        return null;
    }

    // -------------------------------------------------------------------------
    // Turf
    // -------------------------------------------------------------------------

    /// <summary>Where a position sits relative to the player's own gang's territory.</summary>
    public enum TurfStatus { Foreign = 0, Adjacent = 1, Own = 2 }

    /// <summary>
    /// Work out whether a position is on your gang's turf, next to it, or nowhere near.
    ///
    /// THERE IS NO ADJACENCY DATA. A Zone carries no neighbour list, and Boundaries — the
    /// only geometry it has — is populated for exactly ONE of the 94 zones in the shipped
    /// config, so polygon adjacency is not available either. CountyID exists but is far too
    /// coarse to mean anything: 43 zones share CityOfLosSantos.
    ///
    /// So adjacency is measured rather than looked up. Zones.GetZone falls through to the
    /// game's own native zone lookup for any point, which means we can ask "whose turf is
    /// over there" about arbitrary coordinates. Probing a ring around the position and
    /// asking whether any sample lands on our territory defines "adjacent" as "within
    /// ProbeDistance of our turf" — which is what the phrase was always meant to convey,
    /// and is more useful than true polygon adjacency: it scales with how far the player
    /// has strayed rather than with how the map happens to be carved up.
    ///
    /// Cost is one native call per sample. Called on discrete events, and once per squad
    /// trickle, so a handful of calls a second at worst.
    /// </summary>
    public TurfStatus GetTurfStatus(Vector3 position)
    {
        Gang myGang = Player?.RelationshipManager?.GangRelationships?.CurrentGang;
        if (myGang == null || World?.ModDataFileManager == null)
        {
            return TurfStatus.Foreign;
        }
        Zones zones = World.ModDataFileManager.Zones;
        GangTerritories territories = World.ModDataFileManager.GangTerritories;
        if (zones == null || territories == null)
        {
            return TurfStatus.Foreign;
        }
        List<GangTerritory> mine = territories.GetGangTerritory(myGang.ID);
        if (mine == null || !mine.Any())
        {
            return TurfStatus.Foreign;
        }

        if (IsOurZone(zones.GetZone(position), mine))
        {
            return TurfStatus.Own;
        }

        float probe = Config == null || Config.TurfAdjacencyProbeDistance <= 0f ? 150f : Config.TurfAdjacencyProbeDistance;
        for (int i = 0; i < 8; i++)
        {
            double angle = i * Math.PI / 4.0;
            Vector3 sample = new Vector3(
                position.X + (float)(Math.Cos(angle) * probe),
                position.Y + (float)(Math.Sin(angle) * probe),
                position.Z);
            if (IsOurZone(zones.GetZone(sample), mine))
            {
                return TurfStatus.Adjacent;
            }
        }
        return TurfStatus.Foreign;
    }

    private bool IsOurZone(Zone zone, List<GangTerritory> mine)
    {
        if (zone == null || string.IsNullOrEmpty(zone.InternalGameName))
        {
            return false;
        }
        return mine.Any(x => x != null && x.ZoneInternalGameName != null
            && x.ZoneInternalGameName.Equals(zone.InternalGameName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Full value on your own turf, a fraction next door, and — since gang territories are
    /// tiny and some gangs are nowhere near each other — still something further out.
    /// Hustling is hustling wherever you do it; where you do it only changes the rate.
    /// </summary>
    private int ScaleByTurf(int baseAmount, TurfStatus status)
    {
        if (baseAmount <= 0)
        {
            return 0;
        }
        if (status == TurfStatus.Own)
        {
            return baseAmount;
        }
        float multiplier = status == TurfStatus.Adjacent
            ? (Config == null ? 0.5f : Config.AdjacentTurfMultiplier)
            : (Config == null ? 0.25f : Config.ForeignTurfMultiplier);
        return (int)Math.Round(baseAmount * multiplier);
    }

    // -------------------------------------------------------------------------
    // Street earnings — goodwill only, never standing
    // -------------------------------------------------------------------------

    private bool TurfEarningsOn => ProgressionOn && Config.EnableTurfEarnings;

    private static string TurfSuffix(TurfStatus status)
    {
        if (status == TurfStatus.Own) { return " on our block"; }
        if (status == TurfStatus.Adjacent) { return " near our block"; }
        return " out of the area";
    }

    /// <summary>
    /// Killing civilians is charged as heat, not morality. A gang does not care that you
    /// are a bad person; it cares that you brought police attention to its doorstep, which
    /// is why this costs most on your own block and least far away — the exact inverse of
    /// how earning scales.
    ///
    /// Rate-limited, because a single bad firefight or a car through a bus stop can produce
    /// several deaths in seconds and should read as one mistake rather than a wipeout.
    /// </summary>
    public void OnCivilianKilled(Vector3 position)
    {
        if (!ProgressionOn || Config == null || !Config.EnableCivilianKillPenalty || Progression == null)
        {
            return;
        }
        Gang myGang = Player?.RelationshipManager?.GangRelationships?.CurrentGang;
        if (myGang == null || Config.GoodwillPerCivilianKilled <= 0)
        {
            return;
        }
        if (Game.GameTime - GameTimeLastCivilianPenalty < Config.CivilianKillPenaltyCooldownMS)
        {
            return;
        }
        GameTimeLastCivilianPenalty = Game.GameTime;

        TurfStatus status = GetTurfStatus(position);
        float multiplier = status == TurfStatus.Own ? 1.0f
            : status == TurfStatus.Adjacent ? (Config.AdjacentTurfMultiplier <= 0f ? 0.5f : Config.AdjacentTurfMultiplier)
            : (Config.ForeignTurfMultiplier <= 0f ? 0.25f : Config.ForeignTurfMultiplier);
        int cost = (int)Math.Round(Config.GoodwillPerCivilianKilled * multiplier);
        if (cost <= 0)
        {
            return;
        }
        Progression.SpendGoodwill(myGang, cost, status == TurfStatus.Own
            ? "bodies on our own block"
            : "heat from a civilian body");
    }

    private uint GameTimeLastCivilianPenalty = 0;

    /// <summary>
    /// Is this someone worth shaking down?
    ///
    /// Most street dealers are NOT gang members. Pedestrians.AddAmbientPed hands ordinary
    /// civilians a menu from Zone.GetIllicitMenu, so the common corner dealer is a plain
    /// PedExt — an earlier version of this test required GangMember and therefore missed
    /// nearly every dealer in the game.
    ///
    /// There is no IsDealer flag anywhere (GangMember.WillDealDrugs is private), so the
    /// menu is the tell — and specifically the SELL side of it, because GetIllicitMenu
    /// hands out customer menus too and beating up drug buyers is not a business model.
    /// Items with NumberOfItemsToSellToPlayer are also the ones loaded into PedInventory,
    /// which is the same stock looting pays out for.
    ///
    /// Shopkeepers are excluded: Merchant gets a store menu through the same call, and
    /// robbing the corner store is not gang work.
    /// </summary>
    /// <summary>
    /// Passthrough to the crew roster. A ped reaches the player through IPoliceRespondable,
    /// which carries this manager but not the crew one.
    /// </summary>
    public void OnCrewMemberKilledSomeone(GangMember killer, PedExt victim)
    {
        Player?.GangCrewManager?.OnBodyKilledSomeone(killer, victim);
    }

    /// <summary>Kept as the name every call site already uses; the test itself lives in GangPedTests.</summary>
    public bool IsShakeableDealer(PedExt ped)
    {
        return GangPedTests.IsShakeableDealer(ped, Player?.RelationshipManager?.GangRelationships?.CurrentGang);
    }

    private static string DealerLabel(PedExt ped)
    {
        GangMember gangMember = ped as GangMember;
        return gangMember?.Gang == null ? "a" : $"a {gangMember.Gang.ShortName}";
    }

    /// <summary>
    /// Beating a rival dealer senseless on or near your turf. Goodwill, not standing:
    /// shaking down a corner is street earnings, not a loyalty question, and keeping it
    /// off the rank axis means grinding dealers can never promote anyone.
    /// </summary>
    public void OnDealerBeaten(PedExt ped)
    {
        if (!TurfEarningsOn || !IsShakeableDealer(ped) || Progression == null)
        {
            return;
        }
        if (ped.IsDead || ped.Pedestrian == null || !ped.Pedestrian.Exists())
        {
            return; // killing pays nothing; the money is in leaving him able to talk
        }
        Gang myGang = Player?.RelationshipManager?.GangRelationships?.CurrentGang;
        if (myGang == null)
        {
            return;
        }
        TurfStatus status = GetTurfStatus(ped.Pedestrian.Position);
        int award = ScaleByTurf(Config.GoodwillPerDealerShakedown, status);
        if (award <= 0)
        {
            return;
        }
        Progression.AddGoodwill(myGang, award, $"leaned on {DealerLabel(ped)} dealer{TurfSuffix(status)}");
        Player?.GangCrewManager?.OnPlayerTookDealer(false);
    }

    /// <summary>Taking a rival dealer's stash. Same rules, separate event and price.</summary>
    /// <summary>Taking a dealer's stash. Pays whether he is standing, out cold or dead —
    /// the stash is the point.</summary>
    public void OnDealerLooted(PedExt ped)
    {
        if (!TurfEarningsOn || !IsShakeableDealer(ped) || Progression == null)
        {
            return;
        }
        if (ped.Pedestrian == null || !ped.Pedestrian.Exists())
        {
            return;
        }
        Gang myGang = Player?.RelationshipManager?.GangRelationships?.CurrentGang;
        if (myGang == null)
        {
            return;
        }
        TurfStatus status = GetTurfStatus(ped.Pedestrian.Position);
        int award = ScaleByTurf(Config.GoodwillPerDealerLoot, status);
        if (award <= 0)
        {
            return;
        }
        Progression.AddGoodwill(myGang, award, $"took {DealerLabel(ped)} dealer's stash{TurfSuffix(status)}");
        Player?.GangCrewManager?.OnPlayerTookDealer(true);
    }

    // -------------------------------------------------------------------------
    // Cashing out
    // -------------------------------------------------------------------------




    /// <summary>
    /// Goodwill on hand with this gang. A passthrough so callers that already hold this
    /// manager do not also need the progression manager declared on their interface —
    /// the player is split across some sixty of them and each addition is a merge risk.
    /// </summary>
    public int GoodwillBalance(Gang gang)
    {
        return Progression == null ? 0 : Progression.GetGoodwill(gang);
    }
    // -------------------------------------------------------------------------
    // Backup
    // -------------------------------------------------------------------------

    /// <summary>
    /// Men this rank can have out at once. With limits off this returns upstream's 7 so
    /// the feature flag genuinely means "behave as upstream".
    /// </summary>
    public int BackupCap(Gang gang)
    {
        if (!BackupLimitsEnabled)
        {
            return 7;
        }
        int cap = PrivilegeFor(gang).BackupMaximum;
        return cap <= 0 ? 1 : Math.Min(cap, 7);
    }

    /// <summary>
    /// Clamp the request to the rank cap, check the cooldown, and charge. Returns null
    /// on success with <paramref name="requestedCount"/> reduced to what was paid for.
    /// </summary>
    public string AuthorizeBackup(Gang gang, ref int requestedCount)
    {
        if (!BackupLimitsEnabled || gang == null || Progression == null)
        {
            return null; // limits off: upstream behaviour, no cap, no charge
        }
        GangRankPrivilege privilege = PrivilegeFor(gang);
        GangProgressionSave record = Progression.GetRecord(gang);
        if (record == null)
        {
            return null;
        }
        int cap = BackupCap(gang);
        if (requestedCount > cap)
        {
            requestedCount = cap;
        }
        if (requestedCount <= 0)
        {
            return "Nobody to send.";
        }
        double cooldownHours = privilege.BackupCooldownMinutes / 60.0;
        if (!Elapsed(record.LastBackupRequest, cooldownHours))
        {
            return WaitMessage(record.LastBackupRequest, cooldownHours, "crew");
        }
        int cost = privilege.BackupGoodwillPerMember * requestedCount;
        if (!Progression.SpendGoodwill(gang, cost, $"backup x{requestedCount}"))
        {
            return $"Sending {requestedCount} costs {cost} in goodwill. You're short.";
        }
        record.LastBackupRequest = Time.CurrentDateTime;

        List<uint> alreadyLive;
        int live = LiveRequisitioned.TryGetValue(gang.ID, out alreadyLive) ? alreadyLive.Count : 0;
        AuthorizedCount[gang.ID] = live + requestedCount;
        return null;
    }

    /// <summary>
    /// Give the money back when nobody turns up.
    ///
    /// AuthorizeBackup charges, stamps the cooldown and reserves the headcount BEFORE the
    /// dispatcher is asked for anybody, because the charge has to happen at the one entry
    /// point every request passes through. But GangBackup.Setup can fail outright - the log
    /// reads "GangBackup SETUP FAIL NO DISPATCH" - and the player then gets "Can't spare
    /// anyone now" while being 75 goodwill lighter AND on cooldown for men he never saw.
    /// Observed in play: charged at 5:23:46.205, dispatch failed 19ms later.
    ///
    /// So this undoes all three: the goodwill, the cooldown stamp, and the reservation. It
    /// must undo the cooldown too, or a refunded player is still locked out of trying again.
    /// </summary>
    public void RefundBackup(Gang gang, int requestedCount)
    {
        if (!BackupLimitsEnabled || gang == null || Progression == null || requestedCount <= 0)
        {
            return;
        }
        GangRankPrivilege privilege = PrivilegeFor(gang);
        int refund = privilege.BackupGoodwillPerMember * requestedCount;
        if (refund > 0)
        {
            Progression.AddGoodwill(gang, refund, $"nobody came, refunded x{requestedCount}");
        }
        GangProgressionSave record = Progression.GetRecord(gang);
        if (record != null)
        {
            record.LastBackupRequest = DateTime.MinValue;
        }
        List<uint> alreadyLive;
        int live = LiveRequisitioned.TryGetValue(gang.ID, out alreadyLive) ? alreadyLive.Count : 0;
        AuthorizedCount[gang.ID] = live;
    }

    /// <summary>
    /// Strip and re-arm a dispatched man to match the rank that called him.
    ///
    /// Must run AFTER GroupManager.Add, because GroupMember.OnBecameGroupMember applies
    /// GroupSettings health and armor and would otherwise overwrite this. Upstream arms
    /// every backup ped with melee AND sidearm AND long gun regardless of the gang's own
    /// weapon percentages, so the honest way to express a rank ladder is to clear the
    /// loadout and rebuild it rather than try to subtract.
    /// </summary>
    public void ApplyBackupLoadout(GangMember gangMember, Gang gang)
    {
        if (!BackupLimitsEnabled || gangMember == null || gang == null)
        {
            return;
        }
        Ped ped = gangMember.Pedestrian;
        if (ped == null || !ped.Exists())
        {
            return;
        }
        GangRankPrivilege privilege = PrivilegeFor(gang);

        NativeFunction.Natives.REMOVE_ALL_PED_WEAPONS(ped, true);
        List<WeaponCategory> allowed = privilege.ParsedBackupWeaponCategories();
        if (allowed.Any())
        {
            WeaponCategory pick = allowed[RandomItems.MyRand.Next(0, allowed.Count)];
            List<IssuableWeapon> candidates = GangArmoury(gang).Where(x => CategoryOf(x) == pick).ToList();
            IssuableWeapon chosen = candidates.Any() ? PickWeighted(candidates) : null;
            if (chosen != null)
            {
                WeaponInformation info = Weapons?.GetWeapon(chosen.ModelName);
                if (info != null)
                {
                    chosen.SetIssued(Game.GetHashKey(chosen.ModelName), info.PossibleComponents, info.IsTaser);
                    NativeFunction.Natives.GIVE_WEAPON_TO_PED(ped, chosen.GetHash(), 200, false, true);
                    chosen.ApplyVariation(ped);
                }
            }
        }

        ped.Armor = privilege.BackupArmor < 0 ? 0 : privilege.BackupArmor;

        // BackupHealth defaults to 0, which leaves GroupSettings' 250-350 standing.
        //
        // The table used to read 150/200/275/350, written before anyone here knew that
        // DispatchablePerson builds every ped as random(85,125) + 100 - so 100 is the game's
        // effective zero and an Associate's man had FIFTY real hit points, half an ordinary
        // pedestrian. Crew veterancy hit the same wall and was fixed the same way: rank
        // scales cost, squad size, armor and what a man carries. It does not scale how long
        // he lives. Set a positive value here to bring the old ladder back.
        if (privilege.BackupHealth > 0)
        {
            ped.MaxHealth = privilege.BackupHealth;
            ped.Health = privilege.BackupHealth;
        }

        // Track whoever actually turns up, capped at what was paid for. Marking peds at
        // spawn time does not work: GangBackup.Setup skips spawning altogether when enough
        // same-gang peds are already within 55m, which is the common case in your own turf.
        if (TrackLive(gang.ID, gangMember.Handle))
        {
            gangMember.WasRequisitionedBackup = true;
        }
    }

    /// <summary>
    /// Men actually standing with the player right now.
    ///
    /// Counts the GROUP, not the requisition ledger. An earlier version counted only men
    /// the player had paid for, which meant somebody recruited directly off the street
    /// contributed nothing to the squad trickle or the witness bonus — they were plainly
    /// there, and the game acted as though the player was alone. Who is with you is a
    /// question about the group, and the group already knows the answer.
    ///
    /// Reads live ped state rather than trusting the list, because a man deleted by
    /// distance recall never dies and so is never struck off — he simply stops existing,
    /// and a witness has to actually be there.
    /// </summary>
    public int LiveSquadSize(Gang gang)
    {
        List<GroupMember> members = Player?.GroupManager?.CurrentGroupMembers;
        if (members == null)
        {
            return 0;
        }
        return members.Count(x => x?.PedExt != null
            && x.PedExt.Pedestrian != null && x.PedExt.Pedestrian.Exists()
            && !x.PedExt.IsDead && !x.PedExt.IsUnconscious);
    }

    /// <summary>True when this man is now on the books as one the player paid for.</summary>
    private bool TrackLive(string gangID, uint handle)
    {
        List<uint> handles;
        if (!LiveRequisitioned.TryGetValue(gangID, out handles))
        {
            handles = new List<uint>();
            LiveRequisitioned[gangID] = handles;
        }
        if (handles.Contains(handle))
        {
            return true;
        }
        int authorized;
        if (!AuthorizedCount.TryGetValue(gangID, out authorized))
        {
            authorized = 0;
        }
        if (handles.Count >= authorized)
        {
            return false; // beyond what was paid for — see AuthorizedCount
        }
        handles.Add(handle);
        return true;
    }

    /// <summary>
    /// Called from GangMember.OnDeath, which fires exactly once per ped while the ped
    /// still exists. That matters: the squad list also drops men who were merely knocked
    /// out or deleted by distance recall, and charging for either would be charging the
    /// player for something that is not a death.
    /// </summary>
    /// <summary>
    /// Dying and taking it back costs the gang something.
    ///
    /// Goodwill first, because that is the favour currency and undoing your own death is
    /// the biggest favour there is. Standing only when the wallet cannot cover it — the
    /// player always gets to undie, since blocking it could strand him, but he pays in
    /// reputation once he has no favours left to spend.
    /// </summary>
    public void ChargeForUndie()
    {
        if (!ProgressionOn || Config == null || !Config.EnableUndieCost || Progression == null)
        {
            return;
        }
        Gang gang = Player?.RelationshipManager?.GangRelationships?.CurrentGang;
        if (gang == null)
        {
            return; // not in a gang: nobody to owe
        }
        int owed = Math.Max(0, Config.UndieGoodwillCost);
        int onHand = Progression.GetGoodwill(gang);
        int paid = Math.Min(owed, onHand);
        if (paid > 0)
        {
            Progression.SpendGoodwill(gang, paid, "un-died");
        }
        int shortfall = owed - paid;
        if (shortfall <= 0 || Config.UndieStandingCost <= 0)
        {
            return;
        }
        // Standing covers only the part goodwill could not, pro rata. All-or-nothing meant a
        // player one point short paid the same as one with an empty wallet, and standing is
        // the axis that is hardest to earn back.
        int standingCost = (int)Math.Ceiling(Math.Abs(Config.UndieStandingCost) * (shortfall / (float)Math.Max(1, owed)));
        if (standingCost > 0)
        {
            Progression.ChangeStanding(gang, -1 * standingCost, paid > 0 ? "un-died, part paid" : "un-died, nothing to pay with");
        }
    }

    public void OnBackupMemberKilled(GangMember gangMember)
    {
        // The roster hears about every death, whether or not the player was paying for
        // this man — permadeath is about who he was, not who was billed for him.
        Player?.GangCrewManager?.OnBodyKilled(gangMember);
        if (!LossPenaltyEnabled || gangMember == null || Progression == null)
        {
            return;
        }
        if (!gangMember.WasRequisitionedBackup)
        {
            return;
        }
        Gang gang = gangMember.Gang;
        if (gang == null)
        {
            return;
        }
        List<uint> handles;
        if (!LiveRequisitioned.TryGetValue(gang.ID, out handles) || !handles.Remove(gangMember.Handle))
        {
            return; // not one of ours, or already counted
        }
        gangMember.WasRequisitionedBackup = false;

        GangRankPrivilege privilege = PrivilegeFor(gang);
        GangProgressionSave record = Progression.GetRecord(gang);
        if (record != null)
        {
            record.BackupMembersLost++;
        }
        Progression.SpendGoodwill(gang, privilege.BackupGoodwillPerLoss, "backup member lost");

        // Standing only on a wipe. Ordinary attrition is a cost of doing business; losing
        // everyone you were given is a competence question, which is what rank measures.
        if (!handles.Any() && privilege.BackupStandingOnWipe > 0)
        {
            Progression.ChangeStanding(gang, -1 * privilege.BackupStandingOnWipe, "backup squad wiped out");
        }
    }
}
