using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class GangMember : PedExt, IWeaponIssuable
{
    private uint GameTimeSpawned;
    public GangMember(Ped _Pedestrian, ISettingsProvideable settings, Gang gang, bool wasModSpawned, string _Name, ICrimes crimes, IWeapons weapons, IEntityProvideable world) : base(_Pedestrian, settings, crimes, weapons, _Name,gang.MemberName, world)
    {
        Gang = gang;
        WasModSpawned = wasModSpawned;
        WeaponInventory = new WeaponInventory(this, settings);
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
    }
    public override int ShootRate { get; set; } = 400;
    public override int Accuracy { get; set; } = 5;
    public override int CombatAbility { get; set; } = 0;
    public override int TaserAccuracy { get; set; } = 10;
    public override int TaserShootRate { get; set; } = 100;
    public override int VehicleAccuracy { get; set; } = 10;
    public override int VehicleShootRate { get; set; } = 100;
    public override int TurretAccuracy { get; set; } = 10;
    public override int TurretShootRate { get; set; } = 1000;
    public bool IsUsingMountedWeapon { get; set; } = false;
    public WeaponInventory WeaponInventory { get; private set; }
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons) => Gang.GetRandomMeleeWeapon(weapons);
    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => Gang.GetRandomWeapon(v, weapons);
    public Gang Gang { get; set; } = new Gang();
    public override Color BlipColor => Gang != null ? Gang.Color : base.BlipColor;
    public override float BlipSize => 0.3f;
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool HasTaser { get; set; } = false;
    public new string FormattedName => (PlayerKnownsName ? Name : GroupName);
    public override bool KnowsDrugAreas => true;
    public override bool KnowsGangAreas => true;
    public override bool IsGangMember { get; set; } = true;
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
                if (Settings.SettingsManager.PerformanceSettings.IsGangMemberYield1Active)
                {
                    GameFiber.Yield();//TR TEST 28
                }
                UpdateVehicleState();
                if (!IsUnconscious)
                {
                    if (PlayerPerception.DistanceToTarget <= 200f && ShouldCheckCrimes)//was 150 only care in a bubble around the player, nothing to do with the player tho
                    {
                        if (Settings.SettingsManager.PerformanceSettings.IsGangMemberYield2Active)//THIS IS THGE BEST ONE?
                        {
                            GameFiber.Yield();//TR TEST 28
                        }
                        if (Settings.SettingsManager.PerformanceSettings.GangMemberUpdatePerformanceMode1 && !PlayerPerception.RanSightThisUpdate)
                        {
                            GameFiber.Yield();//TR TEST 28
                        }
                        PedViolations.Update(policeRespondable);//possible yield in here!, REMOVED FOR NOW
                        if (Settings.SettingsManager.PerformanceSettings.IsGangMemberYield3Active)
                        {
                            GameFiber.Yield();//TR TEST 28
                        }
                        PedPerception.Update();
                        if (Settings.SettingsManager.PerformanceSettings.IsGangMemberYield4Active)
                        {
                            GameFiber.Yield();//TR TEST 28
                        }
                        if (Settings.SettingsManager.PerformanceSettings.GangMemberUpdatePerformanceMode2 && !PlayerPerception.RanSightThisUpdate)
                        {
                            GameFiber.Yield();//TR TEST 28
                        }
                    }
                    if (Pedestrian.Exists() && policeRespondable.IsCop && !policeRespondable.IsIncapacitated)
                    {
                        CheckPlayerBusted();
                    }
                }
                GameTimeLastUpdated = Game.GameTime;
            }
        }
        CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok
        
    }
    public override void OnBecameWanted()
    {
        if (Pedestrian.Exists())
        {
            if (Gang != null)
            {
                RelationshipGroup.Cop.SetRelationshipWith(Pedestrian.RelationshipGroup, Relationship.Hate);
                Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
                Gang.HasWantedMembers = true;
                //EntryPoint.WriteToConsoleTestLong($"{Pedestrian.Handle} BECAME WANTED (GANG MEMBER) SET {Gang.ID} TO HATES COPS");
            }
            //EntryPoint.WriteToConsoleTestLong($"{Pedestrian.Handle} BECAME WANTED (GANG MEMBER)");
        }
    }
    public override void OnLostWanted()
    {
        if(Pedestrian.Exists())
        {
            PedViolations.Reset();
            //EntryPoint.WriteToConsoleTestLong($"{Pedestrian.Handle} LOST WANTED (GANG MEMBER)");
        }
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
        if (RandomItems.RandomPercent(Gang.DrugDealerPercentage))
        {
            SetupTransactionItems(shopMenus.GetWeightedRandomMenuFromGroup(Gang.DealerMenuGroup));
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
        WeaponInventory.IssueWeapons(weapons, forceMelee || RandomItems.RandomPercent(Gang.PercentageWithMelee), forceSidearm || RandomItems.RandomPercent(Gang.PercentageWithSidearms), forceLongGun || RandomItems.RandomPercent(Gang.PercentageWithLongGuns), dispatchablePerson);       
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
    public override void InsultedByPlayer(IInteractionable player)
    {
        base.InsultedByPlayer(player);
        PlayerToCheck.RelationshipManager.GangRelationships.ChangeReputation(Gang, -100, true);  
    }
}