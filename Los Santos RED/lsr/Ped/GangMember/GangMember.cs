using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class GangMember : PedExt, IWeaponIssuable
{
    public GangMember(Ped _Pedestrian, ISettingsProvideable settings, Gang gang, bool wasModSpawned, string _Name, ICrimes crimes, IWeapons weapons, IEntityProvideable world) : base(_Pedestrian, settings, crimes, weapons, _Name,gang.MemberName, world)
    {
        Gang = gang;
        WasModSpawned = wasModSpawned;
        WeaponInventory = new WeaponInventory(this, settings);

        ReputationReport = new ReputationReport(this);
        PedBrain = new GangBrain(this, Settings, world, weapons);
        Voice = new GangVoice(this, Settings);
    }
    public List<ReputationReport> WitnessedReports { get; private set; } = new List<ReputationReport>();
    public ReputationReport ReputationReport { get; private set; }
    public override int ShootRate { get; set; } = 400;
    public override int Accuracy { get; set; } = 5;
    public override int CombatAbility { get; set; } = 0;
    public override int TaserAccuracy { get; set; } = 10;
    public override int TaserShootRate { get; set; } = 100;
    public override int VehicleAccuracy { get; set; } = 10;
    public override int VehicleShootRate { get; set; } = 100;
    public override int TurretAccuracy { get; set; } = 10;
    public override int TurretShootRate { get; set; } = 1000;
    public override int InsultLimit => 2;
    public override int CollideWithPlayerLimit => 0;
    public override int PlayerStandTooCloseLimit => 1;
    public bool IsUsingMountedWeapon { get; set; } = false;
    public WeaponInventory WeaponInventory { get; private set; }
    public GangVoice Voice { get; private set; }
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons) => Gang.GetRandomMeleeWeapon(weapons);
    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => Gang.GetRandomWeapon(v, weapons);
    public Gang Gang { get; set; } = new Gang();
    public override Color BlipColor => Gang != null ? Gang.Color : base.BlipColor;
    public override float BlipSize => 0.3f;
    public bool HasTaser { get; set; } = false;
    public override string BlipName => "Gang Member";
    public bool IsHitSquad { get; set; } = false;
    public bool IsBackupSquad { get; set; } = false;
    public bool IsAddedToPlayerGroup { get; set; } = false;
    public new string FormattedName => (PlayerKnownsName ? Name : GroupName);
    public override bool KnowsDrugAreas => true;
    public override bool KnowsGangAreas => true;
    public override bool IsGangMember { get; set; } = true;
    public bool IsGeneralBackup { get; internal set; }
    public override bool HasWeapon => WeaponInventory.HasPistol || WeaponInventory.HasLongGun;

    public override void Update(IPerceptable perceptable, IPoliceRespondable policeRespondable, Vector3 placeLastSeen, IEntityProvideable world)
    {
        PlayerToCheck = policeRespondable;
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (Pedestrian.IsAlive)
        {
            if (NeedsFullUpdate)
            {
                IsInWrithe = Pedestrian.IsInWrithe;
                UpdatePositionData();
                PlayerPerception.Update(perceptable, placeLastSeen);
                UpdateVehicleState();
                if (!IsUnconscious && PlayerPerception.DistanceToTarget <= 200f)//was 150 only care in a bubble around the player, nothing to do with the player tho
                {
                    if (!PlayerPerception.RanSightThisUpdate && !Settings.SettingsManager.PerformanceSettings.EnableIncreasedUpdateMode)
                    {
                        GameFiber.Yield();//TR TEST 28
                    }
                    PedViolations.Update(policeRespondable);//possible yield in here!, REMOVED FOR NOW
                    PedPerception.Update();
                    if (policeRespondable.CanBustPeds)
                    {
                        CheckPlayerBusted();
                    }
                }
                GameTimeLastUpdated = Game.GameTime;
            }
        }
        ReputationReport.Update(perceptable, world, Settings);
        CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok       
    }

    public void UpdateSpeech(IPoliceRespondable currentPlayer)
    {
        Voice.Speak(currentPlayer);
    }
    public override void OnBecameWanted()
    {
        if (!Pedestrian.Exists() || Gang == null)
        {
            return;
        }
        RelationshipGroup.Cop.SetRelationshipWith(Pedestrian.RelationshipGroup, Relationship.Hate);
        Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);

        RelationshipGroup.SecurityGuard.SetRelationshipWith(Pedestrian.RelationshipGroup, Relationship.Hate);
        Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.SecurityGuard, Relationship.Hate);


        Gang.HasWantedMembers = true;
        //EntryPoint.WriteToConsoleTestLong($"{Pedestrian.Handle} BECAME WANTED (GANG MEMBER) SET {Gang.ID} TO HATES COPS");
    }
    public override void OnLostWanted()
    {
        if(!Pedestrian.Exists())
        {
            return;
        }
        PedViolations.Reset();
        //EntryPoint.WriteToConsoleTestLong($"{Pedestrian.Handle} LOST WANTED (GANG MEMBER)");
    }
    public bool IsPlayerMember(IInteractionable player)
    {
        if (Gang == null)
        {
            return false;
        }
        if (player.RelationshipManager.GangRelationships.CurrentGang != null && player.RelationshipManager.GangRelationships.CurrentGang.ID == Gang.ID)
        {
            return true;
        }
        if (IsGroupMember)
        {
            return true;
        }
        return false;
    }
    public void SetStats(DispatchablePerson dispatchablePerson, IShopMenus shopMenus, IWeapons weapons, bool addBlip, bool forceMelee, bool forceSidearm, bool forceLongGun)
    {
        if (!Pedestrian.Exists())
        {
            return;
        }
        Pedestrian.Money = 0;
        IsTrustingOfPlayer = RandomItems.RandomPercent(Gang.PercentageTrustingOfPlayer);
        Money = RandomItems.GetRandomNumberInt(Gang.AmbientMemberMoneyMin, Gang.AmbientMemberMoneyMax);
        WillFight = RandomItems.RandomPercent(Gang.FightPercentage);
        WillCallPolice = false;
        WillFightPolice = RandomItems.RandomPercent(Gang.FightPolicePercentage);
        WillAlwaysFightPolice = RandomItems.RandomPercent(Gang.AlwaysFightPolicePercentage);
        if (IsHitSquad || IsBackupSquad || IsGeneralBackup)
        {
            WillFight = true;
            WillFightPolice = true;
            WillAlwaysFightPolice = true;
        }
        if (RandomItems.RandomPercent(Gang.DrugDealerPercentage))
        {
            SetupTransactionItems(shopMenus.GetWeightedRandomMenuFromGroup(Gang.DealerMenuGroup), false);
            Money = RandomItems.GetRandomNumberInt(Gang.DealerMemberMoneyMin, Gang.DealerMemberMoneyMax);
        }
        if (addBlip)
        {
            AddBlip();
        }
        if (dispatchablePerson == null)
        {
            return;
        }
        dispatchablePerson.SetPedExtPermanentStats(this, Settings.SettingsManager.GangSettings.OverrideHealth, Settings.SettingsManager.GangSettings.OverrideArmor, Settings.SettingsManager.GangSettings.OverrideAccuracy);//has a yield
        if (!Pedestrian.Exists())
        {
            return;
        }
        WeaponInventory.IssueWeapons(weapons, IsHitSquad || IsBackupSquad || IsGeneralBackup || forceMelee || RandomItems.RandomPercent(Gang.PercentageWithMelee), IsHitSquad || IsBackupSquad ||IsGeneralBackup || forceSidearm || RandomItems.RandomPercent(Gang.PercentageWithSidearms), IsHitSquad || IsBackupSquad || IsGeneralBackup || forceLongGun || RandomItems.RandomPercent(Gang.PercentageWithLongGuns), dispatchablePerson);
        if (Pedestrian.Exists() && Settings.SettingsManager.CivilianSettings.SightDistance > 60f)
        {
            NativeFunction.Natives.SET_PED_SEEING_RANGE(Pedestrian, Settings.SettingsManager.CivilianSettings.SightDistance);
        }
    }
    public override void OnItemPurchased(ILocationInteractable player, ModItem modItem, int numberPurchased, int moneySpent)
    {
        player.RelationshipManager.GangRelationships.ChangeReputation(Gang, moneySpent, true);
        base.OnItemPurchased(player, modItem, numberPurchased, moneySpent);
    }
    public override void OnItemSold(ILocationInteractable player, ModItem modItem, int numberPurchased, int moneySpent)
    {
        player.RelationshipManager.GangRelationships.ChangeReputation(Gang, moneySpent, true);
        base.OnItemSold(player, modItem, numberPurchased, moneySpent);
    }
    public override void OnInsultedByPlayer(IInteractionable player)
    {
        base.OnInsultedByPlayer(player);
        if(IsPlayerMember(player) || Gang == null || player == null)
        {
            return;
        }
        player.RelationshipManager.GangRelationships.ChangeReputation(Gang, -100, true);  
    }
    public override void OnPlayerIsClose(IInteractionable player)
    {
        if (IsPlayerMember(player) || Gang == null)
        {
            return;
        }
        base.OnPlayerIsClose(player);
    }
    public override void OnCollidedWithPlayer(IInteractionable player)
    {
        if (IsPlayerMember(player) || Gang == null)
        {
            return;
        }
        base.OnCollidedWithPlayer(player);
    }
    public override void OnPlayerDamagedCarOnFoot(IInteractionable player)
    {
        if (Game.GameTime - GameTimePlayerLastDamagedCarOnFoot < 3000)
        {
            return;
        }
        GameTimePlayerLastDamagedCarOnFoot = Game.GameTime;
        if (IsPlayerMember(player) || Gang == null)
        {
            return;
        }
        PlayerPerception.SetFakeSeen();
        TimesInsultedByPlayer += 10;
        EntryPoint.WriteToConsole($"OnPlayerDamagedCarOnFoot triggered {Handle}");
    }
    public override void OnPlayerStoodOnCar(IInteractionable player)
    {
        if (Game.GameTime - GameTimePlayerLastStoodOnCar < 3000)
        {
            return;
        }
        GameTimePlayerLastStoodOnCar = Game.GameTime;
        if (IsPlayerMember(player) || Gang == null)
        {
            return;
        }
        PlayerPerception.SetFakeSeen();
        TimesInsultedByPlayer += 10;
        EntryPoint.WriteToConsole($"OnPlayerStoodOnCar triggered {Handle}");
    }
    public override void OnPlayerDidBodilyFunctionsNear(IInteractionable player)
    {
        if (Game.GameTime - GameTimePlayerLastDidBodilyFunctionsNear < 3000)
        {
            return;
        }
        GameTimePlayerLastDidBodilyFunctionsNear = Game.GameTime;
        if (IsPlayerMember(player) || Gang == null)
        {
            return;
        }
        TimesInsultedByPlayer += 10;
        EntryPoint.WriteToConsole($"OnPlayerDidBodilyFunctionsNear triggered {Handle}");
    }
    public override void OnKilledByPlayer(IViolateable Player, IZones Zones, IGangTerritories GangTerritories)
    {
        int RepToRemove = -Settings.SettingsManager.GangSettings.RepDeductedKilled; 
        GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(Gang);//.MembersKilled++;
        if (gr != null)
        {
            gr.MembersKilled++;
            //EntryPoint.WriteToConsole($"VIOLATIONS: Killing GangMemeber {gm.Gang.ShortName} {gr.MembersKilled}", 5);
            if (Pedestrian.Exists())
            {
                Zone KillingZone = Zones.GetZone(Pedestrian.Position);
                if (KillingZone != null)
                {
                    //EntryPoint.WriteToConsole($"VIOLATIONS: isKilled {isKilled} GangMemeber {gm.Gang.ShortName} zone {KillingZone.InternalGameName}", 5);
                    List<ZoneJurisdiction> totalTerritories = GangTerritories.GetGangTerritory(Gang.ID);
                    if (totalTerritories != null && totalTerritories.Any(x => x.ZoneInternalGameName.ToLower() == KillingZone.InternalGameName.ToLower()))
                    {
                        //EntryPoint.WriteToConsole($"VIOLATIONS: isKilled {isKilled} GangMemeber {gm.Gang.ShortName} zone {KillingZone.InternalGameName} IS GANG TERRITORY!", 5);
                        RepToRemove -= Settings.SettingsManager.GangSettings.RepDeductedKilledTerritory;// 1000;
                        gr.MembersKilledInTerritory++;
                        //EntryPoint.WriteToConsole($"VIOLATIONS: Killing GangMemeber {gm.Gang.ShortName} On Own Turf {gr.MembersKilledInTerritory}", 5);
                    }
                }
                else
                {
                    // EntryPoint.WriteToConsole($"VIOLATIONS: isKilled {isKilled} GangMemeber {gm.Gang.ShortName} zone fail", 5);
                }
            }
        }

       // ReputationReport.WasKilledByPlayer = true;
        ReputationReport.AddReputation(RepToRemove);

        EntryPoint.WriteToConsole($"KILLED ReputationReport.ReputationChangeAmount:{ReputationReport.ReputationChangeAmount} ({RepToRemove}) MembersKilled:{gr?.MembersKilled} {Gang.ShortName}");

        //Player.RelationshipManager.GangRelationships.ChangeReputation(Gang, RepToRemove, true);
        //Player.RelationshipManager.GangRelationships.AddAttacked(Gang);



        base.OnKilledByPlayer(Player, Zones, GangTerritories);
    }
    public override void OnInjuredByPlayer(IViolateable Player, IZones Zones, IGangTerritories GangTerritories)
    {
        int RepToRemove = -Settings.SettingsManager.GangSettings.RepDeductedInjured;
        GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(Gang);//.MembersKilled++;
        if (gr != null)
        {
            gr.MembersHurt++;
            //EntryPoint.WriteToConsole($"VIOLATIONS: Hurting GangMemeber {gm.Gang.ShortName} {gr.MembersHurt}", 5);
            if (Pedestrian.Exists())
            {
                Zone KillingZone = Zones.GetZone(Pedestrian.Position);
                if (KillingZone != null)
                {
                    //EntryPoint.WriteToConsole($"VIOLATIONS: isKilled {isKilled} GangMemeber {gm.Gang.ShortName} zone {KillingZone.InternalGameName}", 5);
                    List<ZoneJurisdiction> totalTerritories = GangTerritories.GetGangTerritory(Gang.ID);
                    if (totalTerritories != null && totalTerritories.Any(x => x.ZoneInternalGameName.ToLower() == KillingZone.InternalGameName.ToLower()))
                    {
                        //EntryPoint.WriteToConsole($"VIOLATIONS: isKilled {isKilled} GangMemeber {gm.Gang.ShortName} zone {KillingZone.InternalGameName} IS GANG TERRITORY!", 5);
                        RepToRemove -= Settings.SettingsManager.GangSettings.RepDeductedInjuredTerritory;// 500;
                        gr.MembersHurtInTerritory++;
                        //EntryPoint.WriteToConsole($"VIOLATIONS: Hurting GangMemeber {gm.Gang.ShortName} On Own Turf {gr.MembersHurtInTerritory}", 5);
                    }
                }
                else
                {
                    // EntryPoint.WriteToConsole($"VIOLATIONS: isKilled {isKilled} GangMemeber {gm.Gang.ShortName} zone fail", 5);
                }
            }
        }
        WillFight = true;
        ReputationReport.AddReputation(RepToRemove);
        EntryPoint.WriteToConsole($"INJURED ReputationReport.ReputationChangeAmount:{ReputationReport.ReputationChangeAmount} ({RepToRemove})");
        base.OnInjuredByPlayer(Player, Zones, GangTerritories);
    }
    public override void OnCarjackedByPlayer(IViolateable Player, IZones Zones, IGangTerritories GangTerritories)
    {
        int RepToRemove = -Settings.SettingsManager.GangSettings.RepDeductedCarjacked;
        GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(Gang);//.MembersKilled++;
        if (gr != null)
        {
            gr.MembersCarJacked++;
            //EntryPoint.WriteToConsole($"VIOLATIONS: Carjacking GangMemeber {gm.Gang.ShortName} {gr.MembersCarJacked}", 5);
            if (Pedestrian.Exists())
            {
                Zone KillingZone = Zones.GetZone(Pedestrian.Position);
                if (KillingZone != null)
                {
                    List<ZoneJurisdiction> totalTerritories = GangTerritories.GetGangTerritory(Gang.ID);
                    if (totalTerritories.Any(x => x.ZoneInternalGameName == KillingZone.InternalGameName))
                    {
                        RepToRemove -= Settings.SettingsManager.GangSettings.RepDeductedCarjackedTerritory;
                        gr.MembersCarJackedInTerritory++;
                        //EntryPoint.WriteToConsole($"VIOLATIONS: Carjacking GangMemeber {gm.Gang.ShortName} On Own Turf {gr.MembersCarJackedInTerritory}", 5);
                    }
                }
            }
        }
        //ReputationReport.WasCarjackedByPlayer = true;
        ReputationReport.AddReputation(RepToRemove);
        WillFight = true;

        EntryPoint.WriteToConsole($"CARJACKED ReputationReport.ReputationChangeAmount:{ReputationReport.ReputationChangeAmount} ({RepToRemove})");
        //Player.RelationshipManager.GangRelationships.ChangeReputation(Gang, RepToRemove, true);
        //Player.RelationshipManager.GangRelationships.AddAttacked(Gang);
        base.OnCarjackedByPlayer(Player, Zones, GangTerritories);
    }
    protected override string GetPedInfoForDisplay()
    {
        string ExtraItems = base.GetPedInfoForDisplay();
        if(Gang != null)
        {
            ExtraItems += $"~n~Gang: {Gang.ShortName}";
        }
        return ExtraItems;
    }

}
