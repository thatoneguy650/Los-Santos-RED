using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Contacts : IContacts
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Contacts.xml";
    private TaxiServiceContact downtownCabContact;
    private EmergencyServicesContact emergencyServicesContact;
    private VehicleExporterContact vehicleExporterContact;
    private TaxiServiceContact rydeMeContact;
    private TaxiServiceContact schlechtContact;
    private CorruptCopContact corruptCopContact;
    private GunDealerContact gunDealerContact;

    public PossibleContacts PossibleContacts { get; private set; }
    public Contacts()
    {
        PossibleContacts = new PossibleContacts();
    }
    public void ReadConfig(string configName)
    {
        string fileName = string.IsNullOrEmpty(configName) ? "Contacts*.xml" : $"Contacts_{configName}.xml";

        DirectoryInfo taskDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = taskDirectory.GetFiles(fileName).OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null && !configName.Equals("Default"))
        {
            EntryPoint.WriteToConsole($"Loaded Contacts Config: {ConfigFile.FullName}", 0);
            PossibleContacts = Serialization.DeserializeParam<PossibleContacts>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Contacts Config  {ConfigFileName}", 0);
            PossibleContacts = Serialization.DeserializeParam<PossibleContacts>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Contacts config found, creating default", 0);
            SetupDefault();
            DefaultConfig_FullExpanded();
            DefaultConfig_LC();
            DefaultConfig();
        }

    }


    public GangContact GetGangContactData(string contactName)
    {
        GangContact toReturn = PossibleContacts.GangContacts.FirstOrDefault(x => x.Name == contactName);
        if(toReturn != null)
        {
            return toReturn;
        }
        return new GangContact(contactName, "CHAR_BLANK_ENTRY");
    }
    public PhoneContact GetContactData(string contactName)
    {
        return PossibleContacts.AllContacts().FirstOrDefault(x => x.Name == contactName);
    }
    public List<PhoneContact> GetDefaultContacts()
    {
        return PossibleContacts.AllContacts().Where(x => x.IsDefault).ToList();
    }
    private void SetupDefault()
    {
        emergencyServicesContact = new EmergencyServicesContact(StaticStrings.EmergencyServicesContactName, "CHAR_CALL911") { Number = "911", FullNumber = "911", IsDefault = true };
        downtownCabContact = new TaxiServiceContact(StaticStrings.DowntownCabCoContactName, "CHAR_TAXI") { FullNumber = "3235555555", Number = "5555555", IsDefault = true };
        gunDealerContact = new GunDealerContact(StaticStrings.UndergroundGunsContactName);
        corruptCopContact = new CorruptCopContact(StaticStrings.OfficerFriendlyContactName);
        vehicleExporterContact = new VehicleExporterContact(StaticStrings.VehicleExporterContactName);



        rydeMeContact = new TaxiServiceContact(StaticStrings.RydeMeContactName, "CHAR_BLANK_ENTRY") { FullNumber = "3235558295", Number = "5558295", IsDefault = true };
        schlechtContact = new TaxiServiceContact(StaticStrings.SchlechtContactName, "CHAR_BLANK_ENTRY") { FullNumber = "3235552356", Number = "5552356", IsDefault = true };

    }
    private void DefaultConfig()
    {
        PossibleContacts = new PossibleContacts();
        PossibleContacts.GunDealerContacts.Add(gunDealerContact);
        PossibleContacts.CorruptCopContact = corruptCopContact;
        PossibleContacts.VehicleExporterContacts.Add(vehicleExporterContact);
        PossibleContacts.EmergencyServicesContact = emergencyServicesContact;
        PossibleContacts.TaxiServiceContacts.Add(downtownCabContact);
        PossibleContacts.TaxiServiceContacts.Add(rydeMeContact);
        PossibleContacts.TaxiServiceContacts.Add(schlechtContact);
        Serialization.SerializeParam(PossibleContacts, ConfigFileName);
    }
    private void DefaultConfig_FullExpanded()
    {
        PossibleContacts PossibleContacts_FullExpanded = new PossibleContacts();
        PossibleContacts_FullExpanded.GunDealerContacts.Add(gunDealerContact);
        PossibleContacts_FullExpanded.CorruptCopContact = corruptCopContact;
        PossibleContacts_FullExpanded.VehicleExporterContacts.Add(vehicleExporterContact);
        PossibleContacts_FullExpanded.EmergencyServicesContact = emergencyServicesContact;
        PossibleContacts_FullExpanded.TaxiServiceContacts.Add(downtownCabContact);



        TaxiServiceContact hellCabContact = new TaxiServiceContact(StaticStrings.HellCabContactName, "CHAR_BLANK_ENTRY") { FullNumber = "8884355222", Number = "4355222" };
        PossibleContacts_FullExpanded.TaxiServiceContacts.Add(hellCabContact);

        TaxiServiceContact purpleCabContact = new TaxiServiceContact(StaticStrings.PurpleCabContactName, "CHAR_BLANK_ENTRY") { FullNumber = "5558008", Number = "3235558008" };
        PossibleContacts_FullExpanded.TaxiServiceContacts.Add(purpleCabContact);

        TaxiServiceContact shitiCabContact = new TaxiServiceContact(StaticStrings.ShitiCabContactName, "CHAR_BLANK_ENTRY") { FullNumber = "4484222", Number = "8874484222" };
        PossibleContacts_FullExpanded.TaxiServiceContacts.Add(shitiCabContact);

        TaxiServiceContact sunderedDependentCabContact = new TaxiServiceContact(StaticStrings.SunderedDependentCabContactName, "CHAR_BLANK_ENTRY") { FullNumber = "5555050", Number = "3235555050" };
        PossibleContacts_FullExpanded.TaxiServiceContacts.Add(sunderedDependentCabContact);

        PossibleContacts_FullExpanded.TaxiServiceContacts.Add(rydeMeContact);
        PossibleContacts_FullExpanded.TaxiServiceContacts.Add(schlechtContact);

        //Vanilla Peds
        Serialization.SerializeParam(PossibleContacts_FullExpanded, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Vanilla Peds\\Contacts_FullExpandedJurisdiction.xml");
        Serialization.SerializeParam(PossibleContacts_FullExpanded, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Full\\Contacts_FullExpandedJurisdiction.xml");
    }
    private void DefaultConfig_LC()
    {

        TaxiServiceContact LCTaxiContact = new TaxiServiceContact(StaticStrings.LCTaxiContactName, "CHAR_BLANK_ENTRY") { FullNumber = "5557854444", Number = "7854444", IsDefault = true };

        PossibleContacts PossibleContacts_LC = new PossibleContacts();
        PossibleContacts_LC.GunDealerContacts.Add(gunDealerContact);
        PossibleContacts_LC.CorruptCopContact = corruptCopContact;
        PossibleContacts_LC.VehicleExporterContacts.Add(vehicleExporterContact);
        PossibleContacts_LC.EmergencyServicesContact = emergencyServicesContact;
        PossibleContacts_LC.TaxiServiceContacts.Add(LCTaxiContact);
        PossibleContacts_LC.TaxiServiceContacts.Add(rydeMeContact);
        PossibleContacts_LC.TaxiServiceContacts.Add(schlechtContact);
        Serialization.SerializeParam(PossibleContacts_LC, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Contacts_{StaticStrings.LibertyConfigSuffix}.xml");

        PossibleContacts PossibleContacts_LCPP = new PossibleContacts();
        PossibleContacts_LCPP.GunDealerContacts.Add(gunDealerContact);
        PossibleContacts_LCPP.CorruptCopContact = corruptCopContact;
        PossibleContacts_LCPP.VehicleExporterContacts.Add(vehicleExporterContact);
        PossibleContacts_LCPP.EmergencyServicesContact = emergencyServicesContact;
        PossibleContacts_LCPP.TaxiServiceContacts.Add(downtownCabContact);
        PossibleContacts_LCPP.TaxiServiceContacts.Add(LCTaxiContact);
        PossibleContacts_LCPP.TaxiServiceContacts.Add(rydeMeContact);
        PossibleContacts_LCPP.TaxiServiceContacts.Add(schlechtContact);
        Serialization.SerializeParam(PossibleContacts_LCPP, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\Contacts_{StaticStrings.LPPConfigSuffix}.xml");
    }
    public PhoneContact GetContactByNumber(string numpadString)
    {
        return PossibleContacts.AllContacts().FirstOrDefault(x => x.Number == numpadString || x.FullNumber == numpadString);
    }
    public void Setup(IOrganizations organizations)
    {
        foreach(TaxiServiceContact taxiServiceContact in PossibleContacts.TaxiServiceContacts)
        {
            taxiServiceContact.TaxiFirm = organizations.PossibleOrganizations.TaxiFirms.Where(x => x.ContactName == taxiServiceContact.Name).FirstOrDefault();
        }
    }
}