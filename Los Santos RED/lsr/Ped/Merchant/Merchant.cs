using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Merchant : PedExt
{
    public InteractableLocation AssociatedStore { get; set; }
   // public GameLocation Store { get; set; }
    public Merchant(Ped _Pedestrian, ISettingsProvideable settings, bool _WillFight, bool _WillCallPolice, bool _IsGangMember, string _Name, ICrimes crimes, IWeapons weapons, IEntityProvideable world, bool willFightPolice) : base(_Pedestrian, settings, _WillFight, _WillCallPolice, _IsGangMember, true, _Name, crimes, weapons, "Vendor", world, willFightPolice)
    {
        Money = RandomItems.GetRandomNumberInt(settings.SettingsManager.CivilianSettings.MerchantMoneyMin, settings.SettingsManager.CivilianSettings.MerchantMoneyMax);
    }
    public override bool KnownsDrugAreas => false;

}

