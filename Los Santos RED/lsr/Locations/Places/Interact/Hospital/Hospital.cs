using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Hospital : GameLocation, ILocationRespawnable
{
    private List<MedicalTreatment> MedicalTreatments;
    public Hospital(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public Hospital() : base()
    {

    }
    public override string TypeName { get; set; } = "Hospital";
    public override int MapIcon { get; set; } = (int)BlipSprite.Hospital;
    public Vector3 RespawnLocation { get; set; }
    public float RespawnHeading { get; set; }
    public void StoreData(IAgencies agencies)
    {
        if (AssignedAssociationID != null)
        {
            AssignedAgency = agencies.GetAgency(AssignedAssociationID);
        }
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
        if (IsLocationClosed())
        {
            return;
        }
        if (CanInteract)
        {
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;
            Player.IsTransacting = true;

            GameFiber.StartNew(delegate
            {
                try
                {
                    StoreCamera = new LocationCamera(this, Player);
                    StoreCamera.Setup();

                    CreateInteractionMenu();

                    CreateHospitalMenu();
                    InteractionMenu.Visible = true;
                    ProcessInteractionMenu();

                    DisposeInteractionMenu();

                    StoreCamera.Dispose();
                    Player.IsTransacting = false;
                    Player.ActivityManager.IsInteractingWithLocation = false;
                    CanInteract = true;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "BarInteract");
        }
    }
    private void CreateHospitalMenu()
    {
        UIMenu treatmentOptionsSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Treatment Options");
        treatmentOptionsSubMenu.SubtitleText = "Pick a Treatment";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Pick one of our state of the art treatment options!";
        SetupTreatments();
        if (MedicalTreatments != null && MedicalTreatments.Any())
        {
            foreach (MedicalTreatment medicalTreatment in MedicalTreatments)
            {
                medicalTreatment.AddToMenu(this, treatmentOptionsSubMenu,Player);
            }
        }
    }
    private void SetupTreatments()
    {
        MedicalTreatments = new List<MedicalTreatment>()
        {
            new MedicalTreatment("Medical Student Once-Over","Have one of our newest medical students attempt to fix your issues",5,500),
            new MedicalTreatment("Short Nurse Visit","Have an overworked nurse briefly ask you a question.",25,2000),
            new MedicalTreatment("Regular Doctor Visit","One of our less qualified doctors will surely be able to help you out.",50,3500),
            new MedicalTreatment("Decent Doctor Visit","Look at Mr. Rockefeller, shelling out for a ~r~real~s~ doctor.",75,4400),
            new MedicalTreatment("Full Body Treatment","Our crack team will scan, poke, and prod you until you are like new!",100,5500),
        };
    }
    public void DisplayInsufficientFundsMessage()
    {
        PlayErrorSound();
        DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation.");
    }
    public void DisplayPurchaseMessage()
    {
        PlaySuccessSound();
        DisplayMessage("~g~Purchase", $"Thank you for your purchase.");
    }
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        if (RespawnLocation != Vector3.Zero)
        {
            RespawnLocation += offsetToAdd;
        }
        base.AddDistanceOffset(offsetToAdd);
    }
}

