using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Merchant : PedExt
{
    public GameLocation AssociatedStore { get; set; }
   // public GameLocation Store { get; set; }
    public Merchant(Ped _Pedestrian, ISettingsProvideable settings, string _Name, ICrimes crimes, IWeapons weapons, IEntityProvideable world) : base(_Pedestrian, settings, crimes, weapons, _Name, "Vendor", world)
    {
        Money = RandomItems.GetRandomNumberInt(settings.SettingsManager.CivilianSettings.MerchantMoneyMin, settings.SettingsManager.CivilianSettings.MerchantMoneyMax);
    }
    public override bool KnowsDrugAreas => false;
    public override bool KnowsGangAreas => false;
    public override bool CanTransact => IsNearSpawnPosition && base.CanTransact;
    public override bool WillCallPolice { get; set; } = true;
    public override bool WillCallPoliceIntense { get; set; } = true;
    public override bool IsMerchant { get; set; } = true;
}

