using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using NAudio.Wave;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Organizations : IOrganizations
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Organizations.xml";
    //private List<Organization> OrganizationsList;
    private Organization DefaultOrganization;
    private TaxiFirm DowntownCabCoOld;
    private Organization VehicleExports;
    private Organization Exportotopia;
    private Organization UndergroundGuns;
    private Organization LSRGuns;
    private TaxiFirm RydeMe;
    private TaxiFirm Schlecht;

    public PossibleOrganizations PossibleOrganizations { get; private set; }

    public Organizations()
    {

    }
    public void ReadConfig()
    {
        DirectoryInfo taskDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo taskFile = taskDirectory.GetFiles("Organizations*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (taskFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Organizations Config: {taskFile.FullName}", 0);
            PossibleOrganizations = Serialization.DeserializeParam<PossibleOrganizations>(taskFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Organizations Config  {ConfigFileName}", 0);
            PossibleOrganizations = Serialization.DeserializeParam<PossibleOrganizations>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Organizations config found, creating default", 0);
            SetupDefault();
            DefaultConfig_FullExpanded();
            DefaultConfig_LibertyCity();
            DefaultConfig();
        }
    }
    public void Setup(IHeads heads, IDispatchableVehicles dispatchableVehicles, IDispatchablePeople dispatchablePeople, IIssuableWeapons issuableWeapons, IContacts contacts)
    {
        foreach (Organization organization in PossibleOrganizations.AllOrganizations())//OrganizationsList)
        {
            organization.LessLethalWeapons = issuableWeapons.GetWeaponData(organization.LessLethalWeaponsID);
            organization.LongGuns = issuableWeapons.GetWeaponData(organization.LongGunsID);
            organization.SideArms = issuableWeapons.GetWeaponData(organization.SideArmsID);
            organization.Personnel = dispatchablePeople.GetPersonData(organization.PersonnelID);
            organization.Vehicles = dispatchableVehicles.GetVehicleData(organization.VehiclesID);
            organization.PossibleHeads = heads.GetHeadData(organization.HeadDataGroupID);
            organization.PhoneContact = contacts.GetContactData(organization.ContactName);
        }
    }
    public TaxiFirm GetDefaultTaxiFirm()
    {
        return PossibleOrganizations.TaxiFirms.FirstOrDefault(x => x.IsDefault);
    }
    public TaxiFirm GetRandomTaxiFirm(bool includeRideShare)
    {
        if(includeRideShare)
        {
            PossibleOrganizations.TaxiFirms.PickRandom();
        }
        return PossibleOrganizations.TaxiFirms.Where(x=> !x.IsRideShare).PickRandom();
    }
    public TaxiFirm GetTaxiFirmFromVehicle(string v, int liveryID)
    {
        return PossibleOrganizations.TaxiFirms.Where(x => x.Vehicles != null && x.Vehicles.Any(y => y.ModelName == v && y.RequiredLiveries != null && y.RequiredLiveries.Contains(liveryID))).PickRandom();
    }
    public Organization GetOrganizations(string AgencyInitials)
    {
        if (string.IsNullOrEmpty(AgencyInitials))
        {
            return null;
        }
        return PossibleOrganizations.AllOrganizations().Where(x => x.ID.ToLower() == AgencyInitials.ToLower()).FirstOrDefault();
    }
    public Organization GetOrganizationByContact(string contactName)
    {
        if (string.IsNullOrEmpty(contactName))
        {
            return null;
        }
        return PossibleOrganizations.AllOrganizations().Where(x => x.PhoneContact != null && x.PhoneContact.Name.ToLower() == contactName.ToLower()).FirstOrDefault();
    }
    public List<Organization> GetOrganizations(Ped ped)
    {
        return PossibleOrganizations.AllOrganizations().Where(x => x.Personnel != null && x.Personnel.Any(b => b.ModelName.ToLower() == ped.Model.Name.ToLower())).ToList();
    }
    public List<Organization> GetOrganizations(Vehicle vehicle)
    {
        return PossibleOrganizations.AllOrganizations().Where(x => x.Vehicles != null && x.Vehicles.Any(b => b.ModelName.ToLower() == vehicle.Model.Name.ToLower())).ToList();
    }
    public List<Organization> GetOrganizations()
    {
        return PossibleOrganizations.AllOrganizations();
    }
    private void SetupDefault()
    {
        DowntownCabCoOld = new TaxiFirm("~y~", "DTCAB", "Downtown Cab Co.", "Downtown Cab Co.", "Yellow", "TaxiDrivers", "TaxiVehicles", "DT ", "MeleeWeapons", "TaxiSidearms", "TaxiLongGuns", "Cabbie") {
            Description = "In transit since 1922",
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.DowntownCabCoContactName,
            IsDefault = true,
            BannerImagePath = "stores\\downtownvanilla.png",
            BaseFare = 10,
            PricePerMile = 5,
            FastSpeedFee = 20,
            CrazySpeedFee = 100,
        };
        VehicleExports = new Organization("~w~", "VEHEXP", StaticStrings.VehicleExporterContactName, StaticStrings.VehicleExporterContactName, "White", "", "", "", "", "", "", "Exporter")
        {
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.VehicleExporterContactName,
        };
        Exportotopia = new Organization("~w~", "EXPORTO", "Exportotopia", "Exportotopia", "White", "", "", "", "", "", "", "Exporter")
        {
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.ExportotopiaContactName,
        };
        UndergroundGuns = new Organization("~r~", "UNDRGUN", StaticStrings.UndergroundGunsContactName, StaticStrings.UndergroundGunsContactName, "Red", "", "", "", "", "", "", "Gun Dealer")
        {
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.UndergroundGunsContactName,
        };
        LSRGuns = new Organization("~r~", "LSRGUN", "LSR Gun Dealer", "LSR Gun Dealer", "Red", "", "", "", "", "", "", "Gun Dealer")
        {
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.LSRGunDealerContactName,
        };

        RydeMe = new TaxiFirm("~r~", "rydeme", "RydeMe", "RydeMe", "Red", "TaxiDrivers", "RideshareVehicles", "", "MeleeWeapons", "TaxiSidearms", "TaxiLongGuns", "Contractor")
        {
            Description = "Come on and RydeMe!",
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.RydeMeContactName,
            IsDefault = true,
            BannerImagePath = "stores\\rydeme.png",
            BaseFare = 12,
            PricePerMile = 4,
            FastSpeedFee = 50,
            CrazySpeedFee = 300,
            IsRideShare = true,
        };
        Schlecht = new TaxiFirm("~b~", "schlecht", "Schlecht", "Schlecht", "Black", "RegularPeds", "RideshareVehicles", "", "MeleeWeapons", "TaxiSidearms", "TaxiLongGuns", "Contractor")
        {
            Description = "Service Beyond Reproach",
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.SchlechtContactName,
            IsDefault = true,
            BannerImagePath = "stores\\schlecht.png",
            BaseFare = 12,
            PricePerMile = 4,
            FastSpeedFee = 50,
            CrazySpeedFee = 300,
            IsRideShare = true,
        };
    }
    private void DefaultConfig()
    {
        DefaultOrganization = new Organization("~b~", "ASSOC", "Association", "Association", "Blue", "", "", "LS ", "", "", "", "Employee");
        PossibleOrganizations = new PossibleOrganizations();
        PossibleOrganizations.GeneralOrganizations = new List<Organization>
        {     
            VehicleExports,
            UndergroundGuns,
        };
        PossibleOrganizations.TaxiFirms = new List<TaxiFirm>
        {
            DowntownCabCoOld,
            RydeMe,
            Schlecht,
        };
        Serialization.SerializeParam(PossibleOrganizations, ConfigFileName);
    }
    private void DefaultConfig_FullExpanded()
    {
        PossibleOrganizations PossibleOrganizations_FullExpanded = new PossibleOrganizations();


        TaxiFirm DowntownCabCo = new TaxiFirm("~y~", "DTCAB", "Downtown Cab Co.", "Downtown Cab Co.", "Yellow", "TaxiDrivers", "DowntownTaxiVehicles", "DT ", "MeleeWeapons", "TaxiSidearms", "TaxiLongGuns", "Cabbie")
        {
            Description = "In transit since 1922",
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.DowntownCabCoContactName,
            IsDefault = true,
            BannerImagePath = "stores\\downtownnew.png",
        };

        TaxiFirm HellCab = new TaxiFirm("~g~", "HELLCAB", "Hell Cab", "Hell Cab", "Green", "TaxiDrivers", "HellTaxiVehicles", "HC ", "MeleeWeapons", "TaxiSidearms", "TaxiLongGuns", "Cabbie")
        {
            Description = "Open the door to HELL and RIDE!",
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.HellCabContactName,
            BannerImagePath = "stores\\hellcab.png",
        };
        TaxiFirm PurpleCabCo = new TaxiFirm("~y~", "PRPLCAB", "Purple Cab Co.", "Purple Cab Co.", "Yellow", "TaxiDrivers", "PurpleTaxiVehicles", "PC ", "MeleeWeapons", "TaxiSidearms", "TaxiLongGuns", "Cabbie")
        {
            Description = "High Vis Meets High Class",
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.PurpleCabContactName,
            BannerImagePath = "stores\\purplecab.png",
        };
        TaxiFirm ShitiCabCo = new TaxiFirm("~y~", "SHITICAB", "Shiti Cab", "Shiti Cab", "Yellow", "TaxiDrivers", "ShitiTaxiVehicles", "PC ", "MeleeWeapons", "TaxiSidearms", "TaxiLongGuns", "Cabbie")
        {
            Description = "Our service is anything BUT!",
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.ShitiCabContactName,
            BannerImagePath = "stores\\shiticab.png",
        };
        TaxiFirm SunderedCab = new TaxiFirm("~y~", "SUNDERED", "Sundered Codependent Taxi", "Sundered Taxi", "Yellow", "TaxiDrivers", "SunderedTaxiVehicles", "SI ", "MeleeWeapons", "TaxiSidearms", "TaxiLongGuns", "Cabbie")
        {
            Description = "We contemptuously drive L.S.",
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.SunderedDependentCabContactName,
            BannerImagePath = "stores\\sunderedtaxi.png",
        };

        PossibleOrganizations_FullExpanded.GeneralOrganizations = new List<Organization>
        {
            VehicleExports,
            UndergroundGuns,
        };
        PossibleOrganizations_FullExpanded.TaxiFirms = new List<TaxiFirm>
        {
            DowntownCabCo,
            HellCab,
            PurpleCabCo,
            ShitiCabCo,
            SunderedCab,
            RydeMe,
            Schlecht,
        };
        Serialization.SerializeParam(PossibleOrganizations_FullExpanded, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Full\\Organizations_FullExpandedJurisdiction.xml");
        Serialization.SerializeParam(PossibleOrganizations_FullExpanded, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\Organizations_LosSantos2008.xml");
        Serialization.SerializeParam(PossibleOrganizations_FullExpanded, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Vanilla Peds\\Organizations_FullExpandedJurisdiction.xml");
        //
    }
    private void DefaultConfig_LibertyCity()
    {
        PossibleOrganizations PossibleOrganizations_LibertyCity = new PossibleOrganizations();


        TaxiFirm NYCTaxiCo = new TaxiFirm("~o~", "LCTAXI", "LC Taxi", "LC Taxi", "Orange", "TaxiDrivers", "LCTaxiVehicles", "LC ", "MeleeWeapons", "TaxiSidearms", "TaxiLongGuns", "Cabbie")
        {
            Description = "Your Only Choice Since 1924",
            HeadDataGroupID = "AllHeads",
            ContactName = StaticStrings.LCTaxiContactName,
            IsDefault = true,
            BannerImagePath = "stores\\lctaxi.png",
        };


        PossibleOrganizations_LibertyCity.GeneralOrganizations = new List<Organization>
        {
            VehicleExports,
            UndergroundGuns,
        };
        PossibleOrganizations_LibertyCity.TaxiFirms = new List<TaxiFirm>
        {
            NYCTaxiCo,
            RydeMe,
            Schlecht
        };
        Serialization.SerializeParam(PossibleOrganizations_LibertyCity, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Organizations_{StaticStrings.LibertyConfigSuffix}.xml");
        //

        PossibleOrganizations PossibleOrganizations_LibertyCityPP = new PossibleOrganizations();
        PossibleOrganizations_LibertyCityPP.GeneralOrganizations = new List<Organization>()
        {
            VehicleExports,
            UndergroundGuns,
        };
        PossibleOrganizations_LibertyCityPP.TaxiFirms = new List<TaxiFirm>
        {
            DowntownCabCoOld,
            NYCTaxiCo,
            RydeMe,
            Schlecht
        };
        Serialization.SerializeParam(PossibleOrganizations_LibertyCityPP, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\Organizations_{StaticStrings.LPPConfigSuffix}.xml");
    }
}