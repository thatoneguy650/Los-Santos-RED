using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Teller : PedExt
{
    public Bank AssociatedBank { get; set; }
    public Teller(Ped _Pedestrian, ISettingsProvideable settings, string _Name, ICrimes crimes, IWeapons weapons, IEntityProvideable world) : base(_Pedestrian, settings, crimes, weapons, _Name, "Teller", world)
    {
       // WasModSpawned = true;
        //Money = RandomItems.GetRandomNumberInt(settings.SettingsManager.CivilianSettings.MerchantMoneyMin, settings.SettingsManager.CivilianSettings.MerchantMoneyMax);
    }
    public override bool KnowsDrugAreas => false;
    public override bool KnowsGangAreas => false;
    public override bool CanTransact => IsNearSpawnPosition && base.CanTransact;
    public override bool WillCallPolice { get; set; } = true;
    public override bool WillCallPoliceIntense { get; set; } = true;
    public override bool IsMerchant { get; set; } = false;
    public override bool CanBeIdleTasked => false;
    public override string InteractPrompt(IButtonPromptable player)
    {
        return $"Transact with {FormattedName}";
    }
    public override void AddSpecificInteraction(ILocationInteractable player,MenuPool menuPool, UIMenu headerMenu, AdvancedConversation advancedConversation)
    {
        BankInteraction BankInteraction = new BankInteraction(player, AssociatedBank);
        BankInteraction.Start(menuPool, headerMenu, true);
        base.AddSpecificInteraction(player, menuPool, headerMenu, advancedConversation);
    }
    public void SetStats(DispatchablePerson dispatchablePerson, IShopMenus shopMenus, IWeapons weapons, bool addBlip, bool forceMelee, bool forceSidearm, bool forceLongGun, GameLocation store)
    {
        if (!Pedestrian.Exists())
        {
            return;
        }
        Pedestrian.Money = 0;
        IsTrustingOfPlayer = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PercentageTrustingOfPlayer);


        /*
        Money = RandomItems.GetRandomNumberInt(Settings.SettingsManager.CivilianSettings.MerchantMoneyMin, Settings.SettingsManager.CivilianSettings.MerchantMoneyMax);
        WillFight = RandomItems.RandomPercent(CivilianFightPercentage());
        WillCallPolice = RandomItems.RandomPercent(CivilianCallPercentage());
        WillCallPoliceIntense = RandomItems.RandomPercent(CivilianSeriousCallPercentage());
        WillFightPolice = RandomItems.RandomPercent(CivilianFightPolicePercentage());
        WillCower = RandomItems.RandomPercent(CivilianCowerPercentage());
        CanSurrender = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PossibleSurrenderPercentage);*/

        if (store == null || store.VendorMoneyMin == -1 || store.VendorMoneyMax == -1)
        {
            Money = RandomItems.GetRandomNumberInt(MerchantMoneyMin(), MerchantMoneyMax());
        }
        else
        {
            Money = RandomItems.GetRandomNumberInt(store.VendorMoneyMin, store.VendorMoneyMax);
        }

        WillFight = RandomItems.RandomPercent(store == null || store.VendorFightPercentage == -1f ? CivilianFightPercentage() : store.VendorFightPercentage);
        WillCallPolice = RandomItems.RandomPercent(store == null || store.VendorFightPercentage == -1f ? CivilianCallPercentage() : store.VendorCallPolicePercentage);
        WillCallPoliceIntense = RandomItems.RandomPercent(store == null || store.VendorFightPercentage == -1f ? CivilianSeriousCallPercentage() : store.VendorCallPoliceForSeriousCrimesPercentage);
        WillFightPolice = RandomItems.RandomPercent(store == null || store.VendorFightPercentage == -1f ? CivilianFightPolicePercentage() : store.VendorFightPolicePercentage);
        WillCower = RandomItems.RandomPercent(store == null || store.VendorFightPercentage == -1f ? CivilianCowerPercentage() : store.VendorCowerPercentage);
        CanSurrender = RandomItems.RandomPercent(store == null || store.VendorFightPercentage == -1f ? Settings.SettingsManager.CivilianSettings.PossibleSurrenderPercentage : store.VendorSurrenderPercentage);




        LocationTaskRequirements = new LocationTaskRequirements() { TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_STAND_IMPATIENT" } };


        if (addBlip)
        {
            AddBlip();
        }
        if (dispatchablePerson == null)
        {
            return;
        }
        dispatchablePerson.SetPedExtPermanentStats(this, Settings.SettingsManager.CivilianSettings.OverrideHealth, false, Settings.SettingsManager.CivilianSettings.OverrideAccuracy);//has a yield
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (Pedestrian.Exists() && Settings.SettingsManager.CivilianSettings.SightDistance > 60f)
        {
            NativeFunction.Natives.SET_PED_SEEING_RANGE(Pedestrian, Settings.SettingsManager.CivilianSettings.SightDistance);
        }
    }




}

