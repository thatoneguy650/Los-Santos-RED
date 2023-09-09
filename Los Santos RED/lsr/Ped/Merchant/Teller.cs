using LosSantosRED.lsr.Interface;
using Rage;
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
        Money = RandomItems.GetRandomNumberInt(settings.SettingsManager.CivilianSettings.MerchantMoneyMin, settings.SettingsManager.CivilianSettings.MerchantMoneyMax);
    }
    public override bool KnowsDrugAreas => false;
    public override bool KnowsGangAreas => false;
    public override bool CanTransact => IsNearSpawnPosition && base.CanTransact;
    public override bool WillCallPolice { get; set; } = true;
    public override bool WillCallPoliceIntense { get; set; } = true;
    public override bool IsMerchant { get; set; } = false;
    public override string InteractPrompt(IButtonPromptable player)
    {
        return $"Transact with {FormattedName}";
    }
    public override void AddSpecificInteraction(ILocationInteractable player,MenuPool menuPool, UIMenu headerMenu, AdvancedConversation advancedConversation)
    {
        BankInteraction BankInteraction = new BankInteraction(player, AssociatedBank);
        BankInteraction.Start(menuPool, headerMenu);
        base.AddSpecificInteraction(player, menuPool, headerMenu, advancedConversation);
    }
}

