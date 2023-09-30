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
    public PossibleContacts PossibleContacts { get; private set; }
    public Contacts()
    {
        PossibleContacts = new PossibleContacts();
    }
    public void ReadConfig()
    {
        DirectoryInfo taskDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo taskFile = taskDirectory.GetFiles("Contacts*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (taskFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Contacts Config: {taskFile.FullName}", 0);
            PossibleContacts = Serialization.DeserializeParam<PossibleContacts>(taskFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Contacts Config  {ConfigFileName}", 0);
            PossibleContacts = Serialization.DeserializeParam<PossibleContacts>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Contacts config found, creating default", 0);
            //SetupDefault();
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
    private void DefaultConfig()
    {
        GunDealerContact gunDealerContact = new GunDealerContact(StaticStrings.UndergroundGunsContactName);
        PossibleContacts.GunDealerContacts.Add(gunDealerContact);

        CorruptCopContact corruptCopContact = new CorruptCopContact(StaticStrings.OfficerFriendlyContactName);
        PossibleContacts.CorruptCopContacts.Add(corruptCopContact);

        VehicleExporterContact vehicleExporterContact = new VehicleExporterContact(StaticStrings.VehicleExporterContactName);
        PossibleContacts.VehicleExporterContacts.Add(vehicleExporterContact);

        EmergencyServicesContact emergencyServicesContact = new EmergencyServicesContact(StaticStrings.EmergencyServicesContactName, "CHAR_CALL911") { IsDefault = true };
        PossibleContacts.EmergencyServicesContacts.Add(emergencyServicesContact);

        TaxiServiceContact downtownCabContact = new TaxiServiceContact(StaticStrings.DowntownCabCoContactName, "CHAR_TAXI");
        downtownCabContact.Number = "3235555555";
        PossibleContacts.TaxiServiceContacts.Add(downtownCabContact);

        Serialization.SerializeParam(PossibleContacts, ConfigFileName);
    }

    public PhoneContact GetContactByNumber(string numpadString)
    {
        return PossibleContacts.AllContacts().FirstOrDefault(x => x.Number == numpadString);
    }
}