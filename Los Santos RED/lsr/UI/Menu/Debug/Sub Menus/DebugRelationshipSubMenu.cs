using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugRelationshipSubMenu : DebugSubMenu
{
    private ModDataFileManager ModDataFileManager;
    public DebugRelationshipSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, ModDataFileManager modDataFileManager) : base(debug, menuPool, player)
    {
        ModDataFileManager = modDataFileManager;
    }
    public override void AddItems()
    {
        UIMenu PlayerStateItemsMenu = MenuPool.AddSubMenu(Debug, "Other Relationships Menu");
        PlayerStateItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various non gang relationship items.";


        foreach(PhoneContact phoneContact in ModDataFileManager.Contacts.PossibleContacts.AllContacts().ToList())
        {
            UIMenuItem contactAdd = new UIMenuItem($"Add {phoneContact.Name}", $"Add {phoneContact.Name} contact and set relationship to friendly");
            contactAdd.Activated += (menu, item) =>
            {
                Player.RelationshipManager.Add(phoneContact.GetRelationship());// new ContactRelationship(phoneContact.Name));
                Player.RelationshipManager.ResetRelationship(phoneContact.Name, false);
                Player.RelationshipManager.SetMaxReputation(phoneContact.Name, false);
                Player.RelationshipManager.SetMoneySpent(phoneContact.Name, 90000, false);
                Player.CellPhone.AddContact(phoneContact, false);/// new CorruptCopContact(StaticStrings.OfficerFriendlyContactName), false);
                menu.Visible = false;
            };
            PlayerStateItemsMenu.AddItem(contactAdd);
        }


        //UIMenuItem AddOfficerFriendly = new UIMenuItem("Add Officer Friendly", "Add officer friendly contact and set relationship to friendly");
        //AddOfficerFriendly.Activated += (menu, item) =>
        //{
        //    Player.RelationshipManager.Add(new OfficerFriendlyRelationship(StaticStrings.OfficerFriendlyContactName));
        //    Player.RelationshipManager.ResetRelationship(StaticStrings.OfficerFriendlyContactName, false);
        //    Player.RelationshipManager.SetMaxReputation(StaticStrings.OfficerFriendlyContactName, false);
        //    Player.RelationshipManager.SetMoneySpent(StaticStrings.OfficerFriendlyContactName, 90000, false);
        //    Player.CellPhone.AddContact(new CorruptCopContact(StaticStrings.OfficerFriendlyContactName), false);
        //    menu.Visible = false;
        //};
        //UIMenuItem AddUndergroundGuns = new UIMenuItem("Add Underground Guns", "Add underground guns contact and set relationship to friendly");
        //AddUndergroundGuns.Activated += (menu, item) =>
        //{
        //    Player.RelationshipManager.Add(new GunDealerRelationship(StaticStrings.UndergroundGunsContactName));
        //    Player.RelationshipManager.ResetRelationship(StaticStrings.UndergroundGunsContactName, false);
        //    Player.RelationshipManager.SetMaxReputation(StaticStrings.UndergroundGunsContactName, false);
        //    Player.RelationshipManager.SetMoneySpent(StaticStrings.UndergroundGunsContactName, 90000, false);
        //    Player.CellPhone.AddContact(new GunDealerContact(StaticStrings.UndergroundGunsContactName), false);
        //    menu.Visible = false;
        //};
        //UIMenuItem AddVehicleExporter = new UIMenuItem("Add Vehicle Exporter", "Add vehicle exporter contact");
        //AddVehicleExporter.Activated += (menu, item) =>
        //{
        //    Player.CellPhone.AddContact(new VehicleExporterContact(StaticStrings.VehicleExporterContactName), false);
        //    menu.Visible = false;
        //};

        //UIMenuItem AddTaxiService = new UIMenuItem("Add Taxi Service", "Add taxi service contact");
        //AddTaxiService.Activated += (menu, item) =>
        //{
        //    ModDataFileManager.Organizations.GetAssociations();
        //    Player.CellPhone.AddContact(new TaxiServiceContact(StaticStrings.DowntownCabCoContactName) { IconName = "CHAR_TAXI" }, false);
        //    menu.Visible = false;
        //};

        //PlayerStateItemsMenu.AddItem(AddOfficerFriendly);
        //PlayerStateItemsMenu.AddItem(AddUndergroundGuns);

        //PlayerStateItemsMenu.AddItem(AddVehicleExporter);
        //PlayerStateItemsMenu.AddItem(AddTaxiService);
    }
}

