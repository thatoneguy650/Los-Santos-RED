using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class KillerInteraction : IContactMenuInteraction
{
    private IContactInteractable Player;
    private UIMenu KillerMenu;
    private MenuPool MenuPool;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private UIMenuItem RequestLocation;
    private UIMenuItem RequestWork;
    private UIMenu LocationSubMenu;
    private PhoneContact AnsweredContact;
    private UIMenuItem TaskCancel;
    private UIMenuItem GunPickup;
    private ISettingsProvideable Settings;

    public KillerInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        MenuPool = new MenuPool();
    }
    public void Start(PhoneContact contact)
    {
        //AnsweredContact = contact;
        //KillerMenu = new UIMenu("", "Select an Option");
        //KillerMenu.RemoveBanner();
        //MenuPool.Add(KillerMenu);
        //KillerMenu.OnItemSelect += OnTopItemSelect;


        //GunPickup = new UIMenuItem("Gun Pickup", "Pickup some guns and bring them to a shop. ~r~WIP~s~") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.UndergroundGunsGunPickupPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.UndergroundGunsGunPickupPaymentMax:C0}~s~" };

        //if (Player.PlayerTasks.HasTask(AnsweredContact.Name))
        //{
        //    TaskCancel = new UIMenuItem("Cancel Task", "Tell the gun dealer you can't complete the task.") { RightLabel = "~o~$?~s~" };
        //    KillerMenu.AddItem(TaskCancel);
        //}
        //else
        //{
        //    KillerMenu.AddItem(GunPickup);
        //}
        //foreach (GunStore gl in PlacesOfInterest.PossibleLocations.GunStores)
        //{
        //    if (gl.ContactName == AnsweredContact.Name && gl.IsEnabled)
        //    {
        //        KillerMenu.AddItem(new UIMenuItem(gl.Name, gl.Description + "~n~Address: " + gl.FullStreetAddress));
        //    }
        //}
        //KillerMenu.Visible = true;
        //GameFiber.StartNew(delegate
        //{
        //    try
        //    {
        //        while (MenuPool.IsAnyMenuOpen())
        //        {
        //            GameFiber.Yield();
        //        }
        //        Player.CellPhone.Close(250);
        //    }
        //    catch (Exception ex)
        //    {
        //        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
        //        EntryPoint.ModController.CrashUnload();
        //    }
        //}, "CellPhone");
    }
    //private void OnTopItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    //{
    //    if (selectedItem == TaskCancel)
    //    {
    //        Player.PlayerTasks.CancelTask(AnsweredContact.Name);
    //        sender.Visible = false;
    //    }
    //    else if (selectedItem == GunPickup)
    //    {
    //        Player.PlayerTasks.UndergroundGunsTasks.GunPickupTask.Start();
    //        sender.Visible = false;
    //    }
    //    else
    //    {
    //        RequestLocations(selectedItem.Text);
    //        sender.Visible = false;
    //    }
    //}

    public void Update()
    {
        MenuPool.ProcessMenus();
    }
    //private void RequestLocations(string name)
    //{
    //    GunStore gunStore = PlacesOfInterest.PossibleLocations.GunStores.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
    //    if (gunStore != null)
    //    {
    //        Player.GPSManager.AddGPSRoute(gunStore.Name, gunStore.EntrancePosition);
    //        List<string> Replies = new List<string>() {
    //                $"Our shop is located on {gunStore.FullStreetAddress} come see us.",
    //                $"Come check out our shop on {gunStore.FullStreetAddress}.",
    //                $"You can find our shop on {gunStore.FullStreetAddress}.",
    //                $"{gunStore.FullStreetAddress}.",
    //                $"It's on {gunStore.FullStreetAddress} come see us.",
    //                $"The shop? It's on {gunStore.FullStreetAddress}.",

    //                };
    //        Player.CellPhone.AddPhoneResponse(AnsweredContact.Name, AnsweredContact.IconName, Replies.PickRandom());
    //    }
    //}
}

