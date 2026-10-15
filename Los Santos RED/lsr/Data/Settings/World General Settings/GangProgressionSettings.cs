using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

/// <summary>
/// Feature flags and tunables for gang progression (rank within a gang the player
/// has already joined).
///
/// The master flag defaults to FALSE. While it is off the manager never ticks and
/// GangProgressionManager.GetLevel returns the TOP rung, so any future rank gate is
/// satisfied by construction and behaviour is identical to upstream. A flag that
/// changes behaviour when disabled is not a flag.
///
/// Everything numeric lives here rather than in code so a rebalance is an XML edit
/// and a reload rather than a rebuild — which matters because tuning these costs a
/// game session each time.
/// </summary>
public class GangProgressionSettings : ISettingsDefaultable
{
    [Description("Master switch for the gang progression feature. When false, nothing ticks and every rank gate reports the top rank, so the mod behaves exactly as it does without this feature.")]
    public bool EnableGangProgression { get; set; }

    [Description("Award standing for killing rival gang members inside your gang's territory. Requires EnableGangProgression.")]
    public bool EnableStandingFromHostility { get; set; }

    [Description("Show the Crew app on the burner phone: where you stand with the gang, who is running with you, and what each man has been through.")]
    public bool EnableCrewApp { get; set; }

    [Description("Write every standing change to the RagePluginHook console.")]
    public bool LogStandingChanges { get; set; }

    // --- accrual ---------------------------------------------------------------

    [Description("Standing awarded when a gang task is completed.")]
    public int StandingPerTaskCompleted { get; set; }

    [Description("Standing removed when a gang task is failed or expires. Enter a positive number.")]
    public int StandingPerTaskFailed { get; set; }

    [Description("Standing awarded when member dues are paid on time.")]
    public int StandingPerDuesPaid { get; set; }

    [Description("Standing removed for each missed dues period. Enter a positive number.")]
    public int StandingPerDuesMissed { get; set; }

    [Description("Standing awarded for killing a member of another gang inside your own gang's territory. Off your turf it counts for nothing, which is what makes staying local worth something. Requires EnableStandingFromHostility.")]
    public int StandingPerRivalKillOnTurf { get; set; }

    [Description("Extra standing when the gang you killed is a declared enemy of yours, on top of StandingPerRivalKillOnTurf.")]
    public int StandingBonusPerEnemyKill { get; set; }

    [Description("Fraction of a kill's value paid as GOODWILL instead of standing when the player was not on a job for the gang at the time. Sanctioned work moves rank; freelancing does not.")]
    public float UnsanctionedKillGoodwillFraction { get; set; }

    [Description("Standing removed for killing a member of your OWN gang, anywhere. Enter a positive number. Requires EnableStandingFromHostility.")]
    public int StandingPerOwnGangKill { get; set; }

    // --- street earnings (goodwill only) ------------------------------------------

    [Description("Earn goodwill from working your own neighbourhood: leaning on rival dealers and taking their stash. Goodwill only — street earnings never move rank. Requires EnableGangProgression.")]
    public bool EnableTurfEarnings { get; set; }

    [Description("Goodwill for beating a rival gang's dealer unconscious without killing him, on your own turf.")]
    public int GoodwillPerDealerShakedown { get; set; }

    [Description("Goodwill for looting a rival gang's dealer on your own turf.")]
    public int GoodwillPerDealerLoot { get; set; }

    [Description("Fraction of the payout earned on turf ADJACENT to yours rather than your own.")]
    public float AdjacentTurfMultiplier { get; set; }

    [Description("Fraction of the payout earned nowhere near your turf. Gang territories are small and some gangs are far apart, so hustling still pays out of the area — just less.")]
    public float ForeignTurfMultiplier { get; set; }

    [Description("How far, in metres, counts as adjacent to your turf. Zones carry no neighbour data, so adjacency is measured by sampling the game's zone lookup in a ring at this distance.")]
    public float TurfAdjacencyProbeDistance { get; set; }

    [Description("Goodwill per surviving crew member each time the squad upkeep trickle pays out. Small on purpose — it should top up a wallet over a session, not replace doing the work.")]
    public int GoodwillPerSquadTick { get; set; }

    [Description("In-game minutes between squad upkeep payouts. Paced off in-game time so fast-forward cannot turn a trickle into a flood.")]
    public int SquadGoodwillTickMinutes { get; set; }

    // --- cashing out ---------------------------------------------------------------

    // --- discouraging rampages ------------------------------------------------------

    [Description("Charge goodwill for killing civilians. Framed as heat rather than morality: the gang minds that you drew police to the block, so this costs most on your own turf. Requires EnableGangProgression.")]
    public bool EnableCivilianKillPenalty { get; set; }

    [Description("Goodwill charged per civilian killed on your own turf, scaled down further out. Enter a positive number.")]
    public int GoodwillPerCivilianKilled { get; set; }

    [Description("Real milliseconds before another civilian death can be charged. Stops one bad firefight or a car through a crowd reading as a dozen separate mistakes.")]
    public int CivilianKillPenaltyCooldownMS { get; set; }

    // --- patrol ---------------------------------------------------------------------

    // --- crew roster ------------------------------------------------------------------

    [Description("Remember the men who answer your calls. They keep their names and faces between callouts, stay dead when killed, and sit out a spell when arrested. Requires EnableGangProgression.")]
    public bool EnableGangCrew { get; set; }

    [Description("Most living men the roster will hold. Once full, new faces are not remembered.")]
    public int CrewRosterMaximum { get; set; }

    [Description("In-game days a crew member is unavailable after being arrested.")]
    public int CrewJailDays { get; set; }

    [Description("How many history entries each crew member keeps. Persisted, so a roster of twelve carrying unbounded histories would bloat every save.")]
    public int CrewEventLogSize { get; set; }

    [Description("Experience a crew member earns for answering a call.")]
    public int CrewExperiencePerCallout { get; set; }

    [Description("Experience a crew member earns for putting someone down while out with you.")]
    public int CrewExperiencePerKill { get; set; }

    [Description("Experience every crew member with you earns when a job pays out.")]
    public int CrewExperiencePerJob { get; set; }

    [Description("Experience every crew member with you earns for work YOU did while they backed you: leaning on a dealer, going through a body, popping a lock. They were not the principals, but standing beside you while you work is how a man gets known.")]
    public int CrewExperiencePerPlayerWork { get; set; }

    [Description("Make experience mean something. A man you have only just met fights below the standard the group settings give every recruit; a Veteran fights at it. Without this, experience is only a label. Requires EnableGangCrew.")]
    public bool EnableCrewVeterancy { get; set; }

    [Description("Percentage of the group's normal combat standard a crew member fights at, one entry per rank: Untested, Green, Reliable, Solid, Veteran. The last entry should be 100 - that is the point where a man is as good as an ordinary recruit, and where behaviour matches the group settings exactly.")]
    public string CrewVeterancyPercentByRank { get; set; }

    [Description("Also scale a crew member's HEALTH by his rank, not just his accuracy, armor and weapons. Off by default: the player cannot choose who turns up, so making green men fragile penalises him for something he does not control, and a man who dies easily never survives long enough to become a veteran.")]
    public bool CrewVeterancyScalesHealth { get; set; }

    [Description("Floor for a scaled crew member's health. Only used when CrewVeterancyScalesHealth is on. Note that 100 is the game's effective zero - every ped is built as random(85,125)+100 - so a value of 120 leaves a man twenty real hit points and he dies to a fender bender. Values at or below 100 are ignored.")]
    public int CrewVeterancyMinimumHealth { get; set; }

    [Description("What a crew member is trusted to carry, one entry per rank: Untested, Green, Reliable, Solid, Veteran. 0 melee only, 1 adds a sidearm, 2 adds a long gun. The last entry should be 2 - that is where a man carries what every backup ped carries today.")]
    public string CrewVeterancyWeaponTierByRank { get; set; }

    // --- crew tribute and kick-up -----------------------------------------------------

    [Description("Crew members run their own rackets and kick a share up to you. Money accrues to a man only from what he DID while out with you, never from time passing, and is put up on a racket at the den rather than simply collected. Requires EnableGangCrew.")]
    public bool EnableCrewTribute { get; set; }

    [Description("Tribute a crew member earns when a job he was out on pays.")]
    public int CrewTributePerJob { get; set; }

    [Description("Tribute a crew member earns for putting down a civilian or unaffiliated target.")]
    public int CrewTributePerKill { get; set; }

    [Description("Tribute a crew member earns for putting down a rival gang member - a bigger share, because that is the work that builds the name.")]
    public int CrewTributePerRivalKill { get; set; }

    [Description("Tribute every man still with you earns when you shake off a wanted level together.")]
    public int CrewTributePerEvasion { get; set; }

    [Description("Minimum wanted level that has to be reached before shaking the police pays anything. A one-star flicker is not an escape.")]
    public int CrewEvasionMinimumWantedLevel { get; set; }

    [Description("In-game minutes the heat must run continuously before losing it counts. Without this the wanted level flickering during a single chase pays several times over - it paid six times in one session.")]
    public int CrewEvasionMinimumMinutes { get; set; }

    [Description("In-game minutes before shaking the police can pay again. Stops one long chase being farmed.")]
    public int CrewEvasionCooldownMinutes { get; set; }

    [Description("Tribute every man with you earns when you actually take something off a body. Doubled if the body was a rival or a dealer. Nothing is paid for searching an empty one.")]
    public int CrewTributePerLoot { get; set; }

    [Description("Multiplier applied to a man's tribute earnings by rank: Untested, Green, Reliable, Solid, Veteran, as percentages. An established man has better rackets.")]
    public string CrewTributePercentByRank { get; set; }

    [Description("Smallest stake the den will take on a racket.")]
    public int CrewKickUpMinimumStake { get; set; }

    [Description("Chance in 100 that each risk tier pays off, low to high risk.")]
    public string CrewKickUpSuccessChanceByTier { get; set; }

    [Description("Cash per point of tribute staked when a racket comes off, low to high risk.")]
    public string CrewKickUpSuccessRateByTier { get; set; }

    [Description("Cash per point of tribute staked when it does not - never zero, because a man who tried still comes back with something.")]
    public string CrewKickUpFailureRateByTier { get; set; }

    [Description("Extra percent added to a successful payout for every whole multiple of the minimum stake put up, rewarding a bigger wager. 0 disables the bonus.")]
    public int CrewKickUpVolumeBonusPercent { get; set; }

    [Description("Most the volume bonus can add, as a percentage, so a huge stake cannot run away with it.")]
    public int CrewKickUpVolumeBonusMaximum { get; set; }

    // --- crew customisation -----------------------------------------------------------

    [Description("The fixed short list the den will issue, one plain weapon per category, as model names. These are what an outfit hands somebody for tonight, not a catalogue - the per-model armoury is still what backup peds draw from. Leave empty for the built-in list.")]
    public string RequisitionIssueWeapons { get; set; }

    [Description("Let the player make a proven man his own: give him a name and change his face. Something to spend a reputation on, so levelling a man up pays out in someone you recognise rather than a number. Requires EnableGangCrew.")]
    public bool EnableCrewCustomisation { get; set; }

    [Description("Rank a man must reach before you can name him. 0 Untested, 1 Green, 2 Reliable, 3 Solid, 4 Veteran.")]
    public int CrewRenameMinimumRank { get; set; }

    [Description("Rank a man must reach before you can change his face. Deliberately later than naming - a face is the bigger reward, and the one that changes who turns up.")]
    public int CrewReskinMinimumRank { get; set; }

    [Description("Longest name the player may give a crew member.")]
    public int CrewNameMaximumLength { get; set; }

    // --- the memorial -----------------------------------------------------------------

    [Description("Keep a memorial of the men you have lost, with when and where they died. Requires EnableGangCrew.")]
    public bool EnableCrewMemorial { get; set; }

    [Description("How many of the dead the memorial holds. Beyond this the oldest unpinned man is dropped, but the total lost is still counted. Pin a man on his page to keep him.")]
    public int CrewMemorialSize { get; set; }

    // --- fencing ----------------------------------------------------------------------

    [Description("Extra standing per surviving crew member when you finish a gang job with backup still alive. Men who saw you do the work can vouch for it.")]
    public int StandingPerWitnessOnJob { get; set; }

    [Description("Goodwill credited per point of standing earned. Standing drives rank and is never spent; goodwill is the spendable balance.")]
    public float GoodwillPerStandingEarned { get; set; }

    [Description("Maximum goodwill that can be banked with one gang.")]
    public int GoodwillMaximum { get; set; }

    [Description("Lowest standing can fall. Standing is a lifetime total but is allowed to drop, which is what makes demotion possible.")]
    public int StandingMinimum { get; set; }

    [Description("Highest standing that can be accumulated with one gang.")]
    public int StandingMaximum { get; set; }

    // --- ladder ----------------------------------------------------------------

    [Description("The rank ladder, lowest first. Leave empty to use the built-in Associate / Soldier / Enforcer / Shotcaller ladder.")]
    public GangStandingLevels StandingLevels { get; set; }

    // --- requisition -------------------------------------------------------------

    [Description("Allow the player to draw weapons, body armor and vehicles from their gang at the den, gated by rank and paid for in goodwill. Requires EnableGangProgression.")]
    public bool EnableRequisition { get; set; }

    [Description("Cap the gang backup squad by rank, charge goodwill per man, and enforce a cooldown between calls. When false, backup behaves exactly as it does upstream. Requires EnableGangProgression.")]
    public bool EnableBackupLimits { get; set; }

    [Description("Charge goodwill for each requisitioned backup member killed, and standing when the whole squad is wiped out. Requires EnableBackupLimits.")]
    public bool EnableBackupLossPenalty { get; set; }

    [Description("Goodwill price per weapon category, as Category:Cost pairs. Rank decides which categories are on the menu; this decides what each one costs, which is what lets one rung span a cheap pistol and an expensive shotgun without needing sub-ranks.")]
    public string WeaponGoodwillCosts { get; set; }

    [Description("Goodwill price for specific weapon models, as Model:Cost pairs, overriding the category price. A bat and a switchblade are both melee but should not cost the same.")]
    public string WeaponGoodwillModelCosts { get; set; }

    [Description("What each rank is allowed to ask the gang for. Leave empty to use the built-in Associate / Soldier / Enforcer / Shotcaller table.")]
    public GangRankPrivileges RankPrivileges { get; set; }

    [Description("Make un-dying cost the gang something. Goodwill first; standing only when the player has no goodwill left to spend. Requires EnableGangProgression.")]
    public bool EnableUndieCost { get; set; }

    [Description("Goodwill charged for un-dying.")]
    public int UndieGoodwillCost { get; set; }

    [Description("Standing lost for un-dying when goodwill cannot cover it. Enter a positive number.")]
    public int UndieStandingCost { get; set; }

    // --- debug -----------------------------------------------------------------

    [Description("How many recent standing changes the debug log keeps in memory. Not persisted.")]
    public int StandingEventLogSize { get; set; }

    public GangProgressionSettings()
    {
        SetDefault();
    }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }

    public void SetDefault()
    {
        EnableGangProgression = false;
        EnableStandingFromHostility = false;
        EnableCrewApp = true;
        LogStandingChanges = true;

        StandingPerTaskCompleted = 60;
        StandingPerTaskFailed = 90;
        StandingPerDuesPaid = 50;
        StandingPerDuesMissed = 150;
        StandingPerRivalKillOnTurf = 25;
        StandingBonusPerEnemyKill = 15;
        StandingPerOwnGangKill = 200;
        UnsanctionedKillGoodwillFraction = 0.5f;

        EnableTurfEarnings = false;
        GoodwillPerDealerShakedown = 60;
        GoodwillPerDealerLoot = 40;
        AdjacentTurfMultiplier = 0.5f;
        ForeignTurfMultiplier = 0.25f;
        TurfAdjacencyProbeDistance = 150f;
        GoodwillPerSquadTick = 5;
        SquadGoodwillTickMinutes = 60;

        EnableCivilianKillPenalty = false;
        GoodwillPerCivilianKilled = 50;
        CivilianKillPenaltyCooldownMS = 8000;

        EnableGangCrew = false;
        CrewRosterMaximum = 12;
        CrewJailDays = 3;
        CrewEventLogSize = 10;
        CrewExperiencePerCallout = 2;
        CrewExperiencePerKill = 5;
        CrewExperiencePerJob = 15;
        CrewExperiencePerPlayerWork = 4;
        EnableCrewVeterancy = true;//inside EnableGangCrew, which is off, so this changes nothing upstream
        CrewVeterancyPercentByRank = "40,55,70,85,100";
        CrewVeterancyScalesHealth = false;//rank scales what a man can do, not how long he lasts
        CrewVeterancyMinimumHealth = 175;//100 is the game's zero, so this is 75 real hit points
        CrewVeterancyWeaponTierByRank = "0,1,1,2,2";

        EnableCrewTribute = false;
        CrewTributePerJob = 120;
        CrewTributePerKill = 15;
        CrewTributePerRivalKill = 45;
        CrewTributePerEvasion = 60;
        CrewEvasionMinimumWantedLevel = 2;
        CrewEvasionMinimumMinutes = 2;
        CrewEvasionCooldownMinutes = 45;
        CrewTributePerLoot = 25;
        CrewTributePercentByRank = "60,80,100,120,150";
        CrewKickUpMinimumStake = 100;
        CrewKickUpSuccessChanceByTier = "85,60,35";
        CrewKickUpSuccessRateByTier = "3,5,8";
        CrewKickUpFailureRateByTier = "2,1,1";
        CrewKickUpVolumeBonusPercent = 4;
        CrewKickUpVolumeBonusMaximum = 60;

        RequisitionIssueWeapons = "";//empty means the built-in list, which lives in the lookup
        EnableCrewCustomisation = true;//inside EnableGangCrew, which is off, so nothing changes upstream
        CrewRenameMinimumRank = 2;
        CrewReskinMinimumRank = 3;
        CrewNameMaximumLength = 24;
        EnableCrewMemorial = true;
        CrewMemorialSize = 10;

        StandingPerWitnessOnJob = 40;

        GoodwillPerStandingEarned = 2.0f;
        GoodwillMaximum = 5000;

        StandingMinimum = 0;
        StandingMaximum = 20000;

        StandingLevels = new GangStandingLevels() { LevelList = GangStandingLevels.DefaultLadder() };

        StandingEventLogSize = 20;

        EnableRequisition = false;
        EnableBackupLimits = false;
        EnableBackupLossPenalty = false;
        WeaponGoodwillCosts = "Melee:50,Pistol:150,Shotgun:300,SMG:300,AR:600,LMG:900,Sniper:900,Heavy:1200,Throwable:400";
        WeaponGoodwillModelCosts = "weapon_bat:25,weapon_crowbar:25,weapon_hammer:25,weapon_knuckle:20,weapon_machete:60,weapon_hatchet:60";
        RankPrivileges = new GangRankPrivileges() { PrivilegeList = GangRankPrivileges.DefaultTable() };

        EnableUndieCost = false;
        UndieGoodwillCost = 300;
        UndieStandingCost = 75;
    }
}
