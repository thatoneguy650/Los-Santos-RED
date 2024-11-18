using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class PersonTransaction : Interaction
{
    private ILocationInteractable Player;
    private PedExt Ped;
    private ShopMenu ShopMenu;
    private Transaction Transaction;
    private MenuPool MenuPool;
    private UIMenu InteractionMenu;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private bool IsDoingTransactionAnimation = false;
    private bool IsActivelyConversing = false;
    private uint GameTimeStartedConversing;
    private bool IsVendorTasked = false;
    private bool IsDisposed = false;
    private Rage.Object SellingProp;
    private UIMenuItem GetInCar;
    private UIMenuItem InviteInCar;
    private UIMenuItem Follow;
    private bool PedWasPersistent = false;
    private bool isPaused;
    private bool isFollowing;
    private bool PanickedByPlayer = false;
    private bool PlayerEnteringOtherVehicle;
    private bool PedEnteringPlayerVehicle;
    private bool PedCanBeTasked;
    private bool PedCanBeAmbientTasked;
    private uint NotificationHandle;
    private bool IsCancelled = false;
    private int MoneySpent = 0;
    public GameLocation AssociatedStore { get; set; }
    public PedExt TransactionPed => Ped;
    private bool CanContinueConversation => Player.IsAliveAndFree && Ped.CanConverse && Ped.Pedestrian.Exists() && Ped.Pedestrian.DistanceTo2D(Player.Character) <= 15f;// && Ped.Pedestrian.Speed <= 3.0f;// ((AssociatedStore != null && AssociatedStore.HasVendor && Player.Character.DistanceTo2D(AssociatedStore.VendorPosition) <= 6f) && (Ped.Pedestrian.Exists() && Ped.Pedestrian.DistanceTo2D(Player.Character) <= 6f)) && Player.CanConverse && Ped.CanConverse;
    public override string DebugString => "";
    public override bool CanPerformActivities { get; set; } = false;
    public bool DoGreet { get; set; } = true;
    public PersonTransaction(ILocationInteractable player, PedExt ped, ShopMenu shopMenu, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        Ped = ped;
        ShopMenu = shopMenu;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
    }
    public override void Start()
    {
        Player.ActivityManager.IsConversing = true;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                AssociatedStore?.HandleVariableItems();
                CreateInteractionMenu();
                if(Ped == null || !Ped.Pedestrian.Exists())
                {
                    return;
                }
                Setup();              
                AddAdditionalOptions();
                StopVehicleActions();
                Greet();
                if(InteractionMenu == null)
                {
                    Dispose();
                    return;
                }
                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                InteractionMenu.OnMenuClose += (sender) =>
                {
                    if (Ped != null && Ped.Pedestrian.Exists() && CanContinueConversation && !PanickedByPlayer && !IsCancelled)
                    {
                        SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BYE", "GENERIC_THANKS", "PED_RANT" }, true);
                    }
                };
                while ((MenuPool.IsAnyMenuOpen() || isPaused) && CanContinueConversation && !PanickedByPlayer && !IsCancelled)
                {
                    UpdateOptions();
                    CheckPedPanicLevel();
                    CheckButtonPrompts();
                    MenuPool.ProcessMenus();
                    Transaction?.Update();
                    GameFiber.Yield();
                }
                Dispose();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "PersonTransaction");
    }
    public override void Dispose()
    {
        //EntryPoint.WriteToConsoleTestLong($"PERSON TRANSACTION Dispose IsDisposed {IsDisposed}");
        if(IsDisposed)
        {
            return;
        }
        IsDisposed = true;
        Player.ActivityManager.IsConversing = false;
        Player.IsTransacting = false;
        Transaction?.DisposeTransactionMenu();
        DisposeInteractionMenu();
        Player.LastFriendlyVehicle = null;
        if (SellingProp.Exists())
        {
            SellingProp.Delete();
        }
        if (PedCanBeTasked && Ped != null)
        {
            Ped.CanBeTasked = true;
        }
        if(PedCanBeAmbientTasked && Ped != null)
        {
            Ped.CanBeAmbientTasked = true;
        }
        RemovePauseButtonPrompts();
        if (Ped != null && Ped.Pedestrian.Exists())
        {
            if(PanickedByPlayer)
            {
                NativeFunction.Natives.TASK_SMART_FLEE_PED(Ped.Pedestrian, Player.Character, 100f, -1, false, false);
                //EntryPoint.WriteToConsole($"PersonTransaction: DISPOSE PANIC 1");
            }
            else if(Ped.Pedestrian.IsInAnyVehicle(false) && PedEnteringPlayerVehicle && Ped.Pedestrian.CurrentVehicle.Exists() && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Ped.Pedestrian.CurrentVehicle.Handle == Player.CurrentVehicle.Vehicle.Handle)
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                Ped.Pedestrian.BlockPermanentEvents = false;
                Ped.Pedestrian.KeepTasks = false;
                NativeFunction.Natives.TASK_LEAVE_VEHICLE(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, (int)eEnter_Exit_Vehicle_Flags.ECF_RESUME_IF_INTERRUPTED | (int) eEnter_Exit_Vehicle_Flags.ECF_WAIT_FOR_ENTRY_POINT_TO_BE_CLEAR);//64);
                Ped.Pedestrian.ClearLastVehicle();
                if (!Ped.BlackListedVehicles.Any(x => x == Player.CurrentVehicle.Vehicle.Handle))
                {
                    Ped.BlackListedVehicles.Add(Player.CurrentVehicle.Vehicle.Handle);
                }
                //EntryPoint.WriteToConsole($"PersonTransaction: DISPOSE CAR 1");
            }
            else if (AssociatedStore != null && Ped.SpawnHeading != 0f)
            {
                Ped.Pedestrian.BlockPermanentEvents = false;
                Ped.Pedestrian.KeepTasks = false;
                NativeFunction.Natives.TASK_ACHIEVE_HEADING(Ped.Pedestrian, Ped.SpawnHeading, -1);// AssociatedStore.VendorHeading, -1);
                //EntryPoint.WriteToConsole($"PersonTransaction: DISPOSE Set Heading");
            }
            else
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                Ped.Pedestrian.BlockPermanentEvents = false;
                Ped.Pedestrian.KeepTasks = false;
                //EntryPoint.WriteToConsole($"PersonTransaction: DISPOSE UnTasking");
            }
        }
        NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);     
    }
    private void Setup()
    {
        PedWasPersistent = Ped.WasEverSetPersistent;
        Player.ActivityManager.IsConversing = true;
        Player.IsTransacting = true;
        PedCanBeTasked = Ped.CanBeTasked;
        PedCanBeAmbientTasked = Ped.CanBeAmbientTasked;
        Ped.CanBeTasked = false;
        Ped.CanBeAmbientTasked = false;
        AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
        AnimationDictionary.RequestAnimationDictionay("mp_common");
        if (Settings.SettingsManager.PlayerOtherSettings.SetCameraHintWhenConversing)
        {
            NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
        }
        Transaction = new Transaction(MenuPool, InteractionMenu, ShopMenu, AssociatedStore);


        if(Settings.SettingsManager.PlayerOtherSettings.PersonTransactionNeverPreviewItems)
        {
            Transaction.PreviewItems = false;
        }

        //if(AssociatedStore == null || !AssociatedStore.VendorsShowItemsPreview)
        //{
        //    Transaction.PreviewItems = false;
        //}

        
        Transaction.PersonTransaction = this;

        bool useAccounts = true;
        if (Ped.ShopMenu == null || Ped.ShopMenu.Items.Any(x => x.IsIllicilt))
        {
            useAccounts = false;
        }
        Transaction.UseAccounts = useAccounts;// Ped.ShopMenu != null || !Ped.ShopMenu.Items.Any(x => x.IsIllicilt);
        Transaction.CreateTransactionMenu(Player, ModItems, World, Settings, Weapons, Time);

    }
    private void CheckButtonPrompts()
    {
        if (isPaused && Player.ButtonPrompts.IsPressed("ContinueTransaction"))
        {
            RemovePauseButtonPrompts();
            isPaused = false;
            if (Ped != null && Ped.Pedestrian.Exists() && !Ped.IsInVehicle && !Player.IsInVehicle && Settings.SettingsManager.PlayerOtherSettings.SetCameraHintWhenConversing)
            {
                NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
            }
            InteractionMenu.Visible = true;
            if (!Ped.IsInVehicle)
            {
                HavePedLookAtPlayer();
            }
            if (!Player.IsInVehicle)
            {
                HavePlayerLookAtPed();
            }
            //EntryPoint.WriteToConsoleTestLong("Unpased Person Transaction");
        }
        if (isPaused && Player.ButtonPrompts.IsPressed("CancelTransaction"))
        {
            //EntryPoint.WriteToConsoleTestLong("Cancelled Paused Person Transaction");
            IsCancelled = true;
        }
    }
    private void CheckPedPanicLevel()
    {
        if (Ped != null && (Ped.HasSeenPlayerCommitMajorCrime || Ped.HasSeenPlayerCommitTrafficCrime || Player.VehicleSpeedMPH >= 85f || Player.RecentlyCrashedVehicle) && PedCanBeTasked && PedCanBeAmbientTasked)
        {
            PanickedByPlayer = true;
            //EntryPoint.WriteToConsoleTestLong($"Person Transaction PanickedByPlayer HasSeenPlayerCommitMajorCrime {Ped.HasSeenPlayerCommitMajorCrime} Ped.HasSeenPlayerCommitTrafficCrime {Ped.HasSeenPlayerCommitTrafficCrime} VehicleSpeedMPH {Player.VehicleSpeedMPH} RecentlyCrashedVehicle {Player.RecentlyCrashedVehicle}");
        }
    }
    private void StopVehicleActions()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (!IsDisposed)
                {
                    if (Ped != null && Ped.Pedestrian.Exists())
                    {
                        Ped.Pedestrian.BlockPermanentEvents = true;
                        Ped.Pedestrian.KeepTasks = true;

                        if (Ped.Pedestrian.CurrentVehicle.Exists() && Ped.IsDriver)
                        {
                            NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 27, -1);
                        }
                    }
                    GameFiber.Yield();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "PersonTransaction");
    }
    private void AddAdditionalOptions()
    {
        GetInCar = new UIMenuItem("Get In Car", "Get in the car to do the deal");
        InviteInCar = new UIMenuItem("Invite In Car", "Invite them in the car to do the deal");
        Follow = new UIMenuItem("Follow", "Have the ped follow you to somewhere more discreet");
        if (Ped != null && Ped.HasMenu && Ped.ShopMenu.Items.Any(x=> x.IsIllicilt) && Ped.IsTrustingOfPlayer)
        {
            InteractionMenu.AddItem(GetInCar);
            InteractionMenu.AddItem(InviteInCar);
            InteractionMenu.AddItem(Follow);   
        }
    }
    private void UpdateOptions()
    {
        if(Ped.IsInVehicle && !Player.IsInVehicle && Ped.IsTrustingOfPlayer)
        {
            GetInCar.Enabled = true;
        }
        else
        {
            GetInCar.Enabled = false;
        }
        if(!Ped.IsInVehicle && Player.IsInVehicle && Ped.IsTrustingOfPlayer)
        {
            InviteInCar.Enabled = true;
        }
        else
        {
            InviteInCar.Enabled = false;
        }
        if(!Player.IsInVehicle && !Ped.IsInVehicle && Ped.IsTrustingOfPlayer)
        {
            Follow.Enabled = true;
        }
        else
        {
            Follow.Enabled = false;
        }
    }
    public void OnItemPurchased(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        if (modItem != null)
        {
            MenuPool.CloseAllMenus();
            Transaction.ClearPreviews();
            StartBuyAnimation(modItem, menuItem, totalItems, MoneySpent == 0);
            Ped.OnItemPurchased(Player, modItem,totalItems, menuItem.PurchasePrice * totalItems);
            MoneySpent += menuItem.PurchasePrice * totalItems;
            Transaction.PurchaseMenu?.Show();
        }
    }
    public void OnItemSold(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        if (modItem != null)
        {
            MenuPool.CloseAllMenus();
            Transaction.ClearPreviews();
            StartSellAnimation(modItem, menuItem, totalItems, MoneySpent == 0);
            Ped.OnItemSold(Player, modItem, totalItems, menuItem.SalesPrice * totalItems);
            MoneySpent += menuItem.SalesPrice * totalItems;
            Transaction.SellMenu?.Show();
        }
    }
    private void InteractionMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        //if(selectedItem.Text == "Buy" || selectedItem.Text == "Select")
        //{
        //    Transaction?.SellMenu?.Dispose();
        //    Transaction?.PurchaseMenu?.Show();
        //}
        //else if (selectedItem.Text == "Sell")
        //{
        //    Transaction?.PurchaseMenu?.Dispose();
        //    Transaction?.SellMenu?.Show();
        //}
        //else 
        
        
        if (selectedItem == GetInCar)
        {
            PlayerGetInCar();
        }
        else if (selectedItem == InviteInCar)
        {
            PedGetInCar();
        }
        else if (selectedItem == Follow)
        {
            FollowPlayer(Ped.Pedestrian);
        }
    }
    private void GetInVehicleAsPassenger(Ped passenegerToAdd, Vehicle vehicleToEnter)
    {
        if(passenegerToAdd.Exists() && vehicleToEnter.Exists())
        {
            int? seatIndex = vehicleToEnter.GetFreePassengerSeatIndex();
            if (seatIndex != null)
            {
                NativeFunction.Natives.TASK_ENTER_VEHICLE(passenegerToAdd, vehicleToEnter, -1, seatIndex, 0.5f, (int)eEnter_Exit_Vehicle_Flags.ECF_RESUME_IF_INTERRUPTED | (int)eEnter_Exit_Vehicle_Flags.ECF_DONT_JACK_ANYONE);// 0);
                //NativeFunction.Natives.TASK_ENTER_VEHICLE(passenegerToAdd, vehicleToEnter, 5000, seatIndex, 1f, 9);
            }
        }
    }
    private void HavePedLookAtPlayer()
    {
        IsVendorTasked = true;
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 2000);
            NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
    }
    private void HavePlayerLookAtPed()
    {
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Ped.Pedestrian, 2000);
            NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Ped.Pedestrian, -1, 0, 2);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
    }
    private void PlayerGetInCar()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists())
        {
            Ped.Pedestrian.CanBePulledOutOfVehicles = false;
            Ped.Pedestrian.CurrentVehicle.IsStolen = false;
            NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
            Player.LastFriendlyVehicle = null;
            Player.LastFriendlyVehicle = Ped.Pedestrian.CurrentVehicle;
            PlayerEnteringOtherVehicle = true;
            PedEnteringPlayerVehicle = false;
            //EntryPoint.WriteToConsoleTestLong("Paused Person Transaction");
            InteractionMenu.Visible = false;
            isPaused = true;
            AddPausedButtonPrompts();
            GetInVehicleAsPassenger(Player.Character, Ped.Pedestrian.CurrentVehicle);
        }
    }
    private void PedGetInCar()
    {
        if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        {
            //Ped.Pedestrian.StaysInVehiclesWhenJacked = true;
            NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
            InteractionMenu.Visible = false;
            isPaused = true;
            AddPausedButtonPrompts();
            PlayerEnteringOtherVehicle = false;
            PedEnteringPlayerVehicle = true;
            GetInVehicleAsPassenger(Ped.Pedestrian, Player.CurrentVehicle?.Vehicle);
        }
    }
    private void FollowPlayer(Ped personToFollow)
    {
        if (personToFollow.Exists())
        {
            NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
            isPaused = true;
            //EntryPoint.WriteToConsoleTestLong("Paused Person Transaction");
            SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY" }, true);
            SayAvailableAmbient(personToFollow, new List<string>() { "GENERIC_YES" }, true);    
            InteractionMenu.Visible = false;
            AddPausedButtonPrompts();
            isFollowing = true;
            personToFollow.BlockPermanentEvents = true;
            personToFollow.KeepTasks = true;
            //NativeFunction.Natives.TASK_FOLLOW_TO_OFFSET_OF_ENTITY(0, Player.Character, 2.0f, 2.0f, 0f, 0.75f, -1, 3f, true);
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_FOLLOW_TO_OFFSET_OF_ENTITY", 0, Player.Character, 0.0f, 2.0f, 0f, 1.0f, -1, 10.0f, true);//NativeFunction.Natives.TASK_FOLLOW_TO_OFFSET_OF_ENTITY(0, Player.Character, 3.0f, 3.0f, 0f, 0.75f, 20000, 5f, true);
                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 2000);
                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
            // vehicleToEnter, 5000, seatIndex, 0.5f, 0);
        }
    }
    public void CreateInteractionMenu()
    {
        MenuPool = new MenuPool();  
        if (AssociatedStore != null && AssociatedStore.HasBannerImage)
        {
            InteractionMenu = new UIMenu(AssociatedStore.Name, AssociatedStore.Description);
            AssociatedStore.BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{AssociatedStore.BannerImagePath}");
            InteractionMenu.SetBannerType(AssociatedStore.BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
        }
        else if (AssociatedStore == null || AssociatedStore.RemoveBanner)
        {
            InteractionMenu = new UIMenu("", "");
            InteractionMenu.RemoveBanner();
        }
        else if (AssociatedStore != null)
        {
            InteractionMenu = new UIMenu(AssociatedStore.Name, AssociatedStore.Description);
        }
        MenuPool.Add(InteractionMenu);
        Player.OnInteractionMenuCreated(AssociatedStore, MenuPool, InteractionMenu);
    }
    public void DisposeInteractionMenu()
    {
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
        if (InteractionMenu != null)
        {
            InteractionMenu.Visible = false;
        }

    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        if (CanContinueConversation)
        {
            foreach (string AmbientSpeech in Possibilities)
            {
                if (ToSpeak.Handle == Player.Character.Handle)
                {
                    if (Player.CharacterModelIsFreeMode)
                    {
                        ToSpeak.PlayAmbientSpeech(Player.FreeModeVoice, AmbientSpeech, 0, SpeechModifier.Force);
                    }
                    else
                    {
                        ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                    }
                }
                else
                {
                    if (Ped.VoiceName != "")
                    {
                        ToSpeak.PlayAmbientSpeech(Ped.VoiceName, AmbientSpeech, 0, SpeechModifier.Force);
                    }
                    else
                    {
                        ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                    }
                }
                GameFiber.Sleep(300);
                if (ToSpeak.Exists() && ToSpeak.IsAnySpeechPlaying)
                {
                    Spoke = true;
                }
                //EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}");
                if (Spoke)
                {
                    break;
                }
            }
            GameFiber.Sleep(100);
            uint GameTimeStartedWatiing = Game.GameTime;
            while (ToSpeak.Exists() && ToSpeak.IsAnySpeechPlaying && WaitForComplete && CanContinueConversation)
            {
                Spoke = true;
                GameFiber.Yield();
            }
            if (!Spoke)
            {
                //Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
            }
        }
        return Spoke;
    }
    private void Greet()
    {
        if (IsDisposed)
        {
            return;
        }
        if(!DoGreet)
        {
            return;
        }
        if (Ped != null && Ped.Pedestrian.Exists())
        {
            if (Ped.Pedestrian.CurrentVehicle.Exists() && Ped.IsDriver)
            {
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 27, -1);
            }
            IsActivelyConversing = true;
            GameTimeStartedConversing = Game.GameTime;
            IsActivelyConversing = true;
            if (Ped.TimesInsultedByPlayer <= 0)
            {
                SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" }, false);
            }
            else
            {
                SayAvailableAmbient(Player.Character, new List<string>() { "PROVOKE_GENERIC", "GENERIC_WHATEVER" }, false);
            }
            while (CanContinueConversation && Game.GameTime - GameTimeStartedConversing <= 1000)
            {
                GameFiber.Yield();
            }
            if (!CanContinueConversation)
            {
                return;
            }

            if (!Ped.IsFedUpWithPlayer)
            {
                //Ped.Pedestrian.IsPersistent = true;
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
                NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                

                if (Ped.IsInVehicle)
                {
                    //Ped.Pedestrian.BlockPermanentEvents = true;
                    //Ped.Pedestrian.KeepTasks = true;

                    if (Ped.Pedestrian.CurrentVehicle.Exists() && Ped.IsDriver)
                    {
                        //NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 27, -1);


                        IsVendorTasked = true;
                        //NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Ped.Pedestrian, Player.Character, -1, 0, 2);
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Ped.Pedestrian.CurrentVehicle, 27, 2000);
                            NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        }
                    }


                    Ped.Pedestrian.BlockPermanentEvents = true;
                    Ped.Pedestrian.KeepTasks = true;
                    //EntryPoint.WriteToConsoleTestLong("PersonTransaction, THEY HAVE BEEN BLOCKED< CLEARED< AND FUCKING TASKED SO FUCK OFF");
                }
                else
                {
                    IsVendorTasked = true;
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 2000);
                        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }
                if (Player.IsInVehicle)
                {
                    NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Player.Character, Ped.Pedestrian, -1, 0, 2);
                }
                else
                {
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Ped.Pedestrian, 2000);
                        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Ped.Pedestrian, -1, 0, 2);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }
                uint GameTimeStartedFacing = Game.GameTime;
                while (CanContinueConversation && Game.GameTime - GameTimeStartedFacing <= 500)
                {
                    GameFiber.Yield();
                }
                if (!CanContinueConversation)
                {
                    return;
                }
                if (Ped.TimesInsultedByPlayer <= 0)
                {
                    SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" }, true);
                }
                else
                {
                    SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_WHATEVER" }, true);
                }
                Ped.PlayerKnownsName = true;
            }
            IsActivelyConversing = false;
        }
    }
    private void StartBuyAnimation(ModItem modItem, MenuItem menuItem, int totalItems, bool allowSpeaking)
    {
        IsActivelyConversing = true;
        string modelName = "";
        bool HasProp = false;
        bool isWeapon = false;
        bool isPackage = false;
        if (modItem.PackageItem != null && modItem.PackageItem.ModelName != "")
        {
            modelName = modItem.PackageItem.ModelName;
            HasProp = true;
            if (modItem.PackageItem.Type == ePhysicalItemType.Weapon)
            {
                isWeapon = true;
            }
            isPackage = true;
        }
        else if (modItem.ModelItem != null && modItem.ModelItem.ModelName != "")
        {
            modelName = modItem.ModelItem.ModelName;
            HasProp = true;
            if (modItem.ModelItem.Type == ePhysicalItemType.Weapon)
            {
                isWeapon = true;
            }
        }
        IsActivelyConversing = true;
        if (menuItem.IsIllicilt)
        {
            if (isWeapon)
            {
                Player.IsDealingIllegalGuns = true;
                Ped.IsDealingIllegalGuns = true;
            }
            else
            {
                Player.IsDealingDrugs = true;
                Ped.IsDealingDrugs = true;
            }
        }
        Player.ButtonPrompts.Clear();
        if (allowSpeaking)
        {
            SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY", "GENERIC_YES", "BLOCKED_GENEIRC" }, true);
        }
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "mp_common", "givetake1_a", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", "givetake1_b", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
        }
        GameFiber.Sleep(500);
        string HandBoneName = "BONETAG_R_PH_HAND";
        Vector3 HandOffset = Vector3.Zero;
        Rotator HandRotator = Rotator.Zero;

        PropAttachment pa = null;
        if (isPackage)
        {
            pa = modItem?.PackageItem?.Attachments?.FirstOrDefault(x => x.Name == "RightHandPass" && (x.Gender == "U" || x.Gender == Player.Gender));
            if (pa == null)
            {
                EntryPoint.WriteToConsole("PERSON TRANSACTION RIGHTHANDPASS IS NULL FALLING BACK TO RIGHT HAND");
                pa = modItem?.PackageItem?.Attachments?.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
            }
        }
        else
        {
            pa = modItem?.ModelItem?.Attachments?.FirstOrDefault(x => x.Name == "RightHandPass" && (x.Gender == "U" || x.Gender == Player.Gender));
            if (pa == null)
            {
                EntryPoint.WriteToConsole("PERSON TRANSACTION RIGHTHANDPASS IS NULL FALLING BACK TO RIGHT HAND");
                pa = modItem?.ModelItem?.Attachments?.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
            }
        }

        if (pa != null)
        {
            HandOffset = pa.Attachment;
            HandRotator = pa.Rotation;
            HandBoneName = pa.BoneName;
        }


        EntryPoint.WriteToConsole($"PERSON TRANSACTION FINAL BUY isPackage{isPackage} HandBoneName{HandBoneName} HandOffset{HandOffset} HandRotator{HandRotator}");

        if (Ped.Pedestrian.Exists() && HasProp && modelName != "")
        {
            SellingProp = modItem.SpawnAndAttachItem(Player, true, true);
            //try
            //{
            //    SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
            //}
            //catch (Exception ex)
            //{
            //    //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
            //}
            GameFiber.Yield();
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Ped.Pedestrian, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Ped.Pedestrian, HandBoneName), HandOffset, HandRotator);
            }
        }
        GameFiber.Sleep(500);
        Transaction.DisplayItemPurchasedMessage(modItem, totalItems);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.Detach();

                Vector3 HandOffsetPlayer = new Vector3(HandOffset.X + 0.07f, HandOffset.Y, HandOffset.Z - 0.05f);

                SellingProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffsetPlayer, HandRotator);
            }
        }
        GameFiber.Sleep(1000);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.Delete();
            }
            if (allowSpeaking)
            {
                SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, true);
                //SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BYE", "GENERIC_THANKS", "PED_RANT" }, true);
            }
        }
        IsActivelyConversing = false;
        if (menuItem.IsIllicilt)
        {
            if (isWeapon)
            {
                Player.IsDealingIllegalGuns = false;
                Ped.IsDealingIllegalGuns = false;
            }
            else
            {
                Player.IsDealingDrugs = false;
                Ped.IsDealingDrugs = false;
            }
        }
    }
    private void StartSellAnimation(ModItem modItem, MenuItem menuItem, int totalItems, bool allowSpeaking)
    {
        string modelName = "";
        bool HasProp = false;
        bool isWeapon = false;
        bool isPackage = false;
        if (modItem.PackageItem != null && modItem.PackageItem.ModelName != "")
        {
            modelName = modItem.PackageItem.ModelName;
            HasProp = true;
            if (modItem.PackageItem.Type == ePhysicalItemType.Weapon)
            {
                isWeapon = true;
            }
            isPackage = true;
        }
        else if (modItem.ModelItem != null && modItem.ModelItem.ModelName != "")
        {
            modelName = modItem.ModelItem.ModelName;
            HasProp = true;
            if (modItem.ModelItem.Type == ePhysicalItemType.Weapon)
            {
                isWeapon = true;
            }
        }
        IsActivelyConversing = true;
        if (menuItem.IsIllicilt)
        {
            if (isWeapon)
            {
                Player.IsDealingIllegalGuns = true;
                Ped.IsDealingIllegalGuns = true;
            }
            else
            {
                Player.IsDealingDrugs = true;
                Ped.IsDealingDrugs = true;
            }
        }
        Player.ButtonPrompts.Clear();
        if (allowSpeaking)
        {
            SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BUY", "GENERIC_YES", "BLOCKED_GENEIRC" }, true);
        }
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "mp_common", "givetake1_b", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", "givetake1_a", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
        }

        GameFiber.Sleep(500);
        string HandBoneName = "BONETAG_R_PH_HAND";
        Vector3 HandOffset = Vector3.Zero;
        Rotator HandRotator = Rotator.Zero;
        PropAttachment pa = null;
        if (isPackage)
        {
            pa = modItem?.PackageItem?.Attachments?.FirstOrDefault(x => x.Name == "RightHandPass" && (x.Gender == "U" || x.Gender == Player.Gender));
            if (pa == null)
            {
                EntryPoint.WriteToConsole("PERSON TRANSACTION RIGHTHANDPASS IS NULL FALLING BACK TO RIGHT HAND");
                pa = modItem?.PackageItem?.Attachments?.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
            }
        }
        else
        {
            pa = modItem?.ModelItem?.Attachments?.FirstOrDefault(x => x.Name == "RightHandPass" && (x.Gender == "U" || x.Gender == Player.Gender));
            if (pa == null)
            {
                EntryPoint.WriteToConsole("PERSON TRANSACTION RIGHTHANDPASS IS NULL FALLING BACK TO RIGHT HAND");
                pa = modItem?.ModelItem?.Attachments?.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
            }
        }

        if (pa != null)
        {
            HandOffset = pa.Attachment;
            HandRotator = pa.Rotation;
            HandBoneName = pa.BoneName;
        }


        EntryPoint.WriteToConsole($"PERSON TRANSACTION FINAL SELL isPackage{isPackage} HandBoneName{HandBoneName} HandOffset{HandOffset} HandRotator{HandRotator}");

        if (Ped.Pedestrian.Exists() && HasProp && modelName != "")
        {
            SellingProp = modItem.SpawnAndAttachItem(Player, true, true);



            //try
            //{
            //    SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
            //}
            //catch (Exception ex)
            //{
            //    //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
            //}
            GameFiber.Yield();
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
            }
        }
        GameFiber.Sleep(500);
        Transaction.DisplayItemSoldMessage(modItem, totalItems);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.Detach();
                Vector3 HandOffsetPed = new Vector3(HandOffset.X + 0.07f, HandOffset.Y, HandOffset.Z - 0.05f);
                SellingProp.AttachTo(Ped.Pedestrian, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Ped.Pedestrian, HandBoneName), HandOffsetPed, HandRotator);
            }
        }
        GameFiber.Sleep(1000);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.Delete();
            }
            if (allowSpeaking)
            {
                SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, true);
            }
        }
        IsActivelyConversing = false;
        if (menuItem.IsIllicilt)
        {
            if (isWeapon)
            {
                Player.IsDealingIllegalGuns = false;
                Ped.IsDealingIllegalGuns = false;
            }
            else
            {
                Player.IsDealingDrugs = false;
                Ped.IsDealingDrugs = false;
            }
        }
        //Show();     
    }
    private void AddPausedButtonPrompts()
    {
        if (!Player.ButtonPrompts.HasPrompt("ContinueTransaction"))
        {
            Player.ButtonPrompts.AddPrompt("PausedTransaction", "Continue Transaction", "ContinueTransaction", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 101);
        }
        if (!Player.ButtonPrompts.HasPrompt("CancelTransaction"))
        {
            Player.ButtonPrompts.AddPrompt("PausedTransaction", "Cancel Transaction", "CancelTransaction", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 102);
        }
    }
    private void RemovePauseButtonPrompts()
    {
        Player.ButtonPrompts.RemovePrompts("PausedTransaction");
    }
}

