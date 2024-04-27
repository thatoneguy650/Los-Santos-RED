using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class IllicitMarketplace : GameLocation
{
    private bool ShownWarning = false;
    private bool IsVendorSpawned = false;
    public IllicitMarketplace() : base()
    {

    }
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Illicit Marketplace";
    public override int MapIcon { get; set; } = 514;//441;
    public override string ButtonPromptText { get; set; }
    public override int MinPriceRefreshHours { get; set; } = 12;
    public override int MaxPriceRefreshHours { get; set; } = 24;
    public override int MinRestockHours { get; set; } = 12;
    public override int MaxRestockHours { get; set; } = 24;



    public override float ExtaVendorSpawnPercentage { get; set; } = 25f;
    public override string VendorHeadDataGroupID { get; set; }
    public override string VendorPersonnelID { get; set; } = "IllicitMarketplacePeds";

    public override float VendorMeleePercent { get; set; } = 55f;
    public override float VendorSidearmPercent { get; set; } = 25f;
    public override float VendorLongGunPercent { get; set; } = 5f;


    public override int VendorMoneyMin { get; set; } = 5;
    public override int VendorMoneyMax { get; set; } = 550;

    public override float VendorFightPercentage { get; set; } = 35f;
    public override float VendorCallPolicePercentage { get; set; } = 0f;
    public override float VendorCallPoliceForSeriousCrimesPercentage { get; set; } = 0f;
    public override float VendorFightPolicePercentage { get; set; } = 35f;
    public override float VendorCowerPercentage { get; set; } = 1f;
    public override float VendorSurrenderPercentage { get; set; } = 5f;




    public List<AppearPercentage> AppearPercentages { get; set; }
    public IllicitMarketplace(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Discretly shop At {Name}";
        return true;
    }
    public override void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
    {
        ShownWarning = false;
        base.Activate(interiors, settings, crimes, weapons, time, world);
    }
    public override void AttemptVendorSpawn(bool isOpen, IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world, bool isInterior)
    {
        if(AppearPercentages != null && AppearPercentages.Any())
        {
            AppearPercentage appearPercentage = AppearPercentages.FirstOrDefault(x => x.Hour == time.CurrentHour);
            if(appearPercentage != null)
            {
                if(!RandomItems.RandomPercent(appearPercentage.Percentage))
                {
                    EntryPoint.WriteToConsole("VENDOR IS NOT APPEARING (PROBABILITY)");
                    IsVendorSpawned = false;
                    return;
                }
            }
        }
        IsVendorSpawned = true;
        base.AttemptVendorSpawn(isOpen, interiors, settings, crimes, weapons, time, world, isInterior);
    }
    public override void OnPlayerBecameClose()
    {
        if(!ShownWarning && !IsVendorSpawned)
        {
            Game.DisplayHelp("Nobody Around, Come Back Later");
            ShownWarning = true;
        }
        base.OnPlayerBecameClose();
    }
}

