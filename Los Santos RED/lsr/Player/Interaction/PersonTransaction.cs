using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PersonTransaction : Interaction
{
    private IActivityPerformable Player;
    private PedExt Ped;
    private ShopMenu ShopMenu;
    private Transaction Transaction;
    private MenuPool MenuPool;
    private UIMenu InteractionMenu;
   // private Texture BannerImage;

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

    public InteractableLocation AssociatedStore { get; set; }


    private bool CanContinueConversation => Player.IsAliveAndFree && Ped.CanConverse && Ped.Pedestrian.Exists() && Ped.Pedestrian.DistanceTo2D(Player.Character) <= 15f;// && Ped.Pedestrian.Speed <= 3.0f;// ((AssociatedStore != null && AssociatedStore.HasVendor && Player.Character.DistanceTo2D(AssociatedStore.VendorPosition) <= 6f) && (Ped.Pedestrian.Exists() && Ped.Pedestrian.DistanceTo2D(Player.Character) <= 6f)) && Player.CanConverse && Ped.CanConverse;
    //was Player.CanConverse
    
    public override string DebugString => "";

    public PersonTransaction(IActivityPerformable player, PedExt ped, ShopMenu shopMenu, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
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

        if (ShopMenu == null)
        {
            EntryPoint.WriteToConsole("Shop Menu is null for some fucking reason !");

        }
        else
        {
            Player.IsConversing = false;
            Player.IsTransacting = false;
        }

        GameFiber.StartNew(delegate
        {
            CreateInteractionMenu();

            if (Ped != null && Ped.Pedestrian.Exists())
            {
                PedWasPersistent = Ped.WasEverSetPersistent;


                //Ped.Pedestrian.IsPersistent = true;
                Player.IsConversing = true;
                Player.IsTransacting = true;

                PedCanBeTasked = Ped.CanBeTasked;
                PedCanBeAmbientTasked = Ped.CanBeAmbientTasked;


                Ped.CanBeTasked = false;
                Ped.CanBeAmbientTasked = false;

                AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
                AnimationDictionary.RequestAnimationDictionay("mp_common");

                NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);



                Transaction = new Transaction(MenuPool, InteractionMenu, ShopMenu, AssociatedStore);
                Transaction.PreviewItems = false;
                Transaction.PersonTransaction = this;
                Transaction.CreateTransactionMenu(Player, ModItems, World, Settings, Weapons, Time);

                AddAdditionalOptions();
                StopVehicleActions();

                Greet();


                if (InteractionMenu != null)
                {

                    InteractionMenu.Visible = true;
                    InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                    //Transaction.ProcessTransactionMenu();
                    while ((MenuPool.IsAnyMenuOpen() || isPaused) && CanContinueConversation && !PanickedByPlayer)
                    {
                        UpdateOptions();


                        if (Ped != null && (Ped.HasSeenPlayerCommitMajorCrime || Ped.HasSeenPlayerCommitTrafficCrime || Player.VehicleSpeedMPH >= 85f || Player.RecentlyCrashedVehicle) && PedCanBeTasked && PedCanBeAmbientTasked)
                        {
                            PanickedByPlayer = true;
                            EntryPoint.WriteToConsole($"Person Transaction PanickedByPlayer HasSeenPlayerCommitMajorCrime {Ped.HasSeenPlayerCommitMajorCrime} Ped.HasSeenPlayerCommitTrafficCrime {Ped.HasSeenPlayerCommitTrafficCrime} VehicleSpeedMPH {Player.VehicleSpeedMPH} RecentlyCrashedVehicle {Player.RecentlyCrashedVehicle}");
                        }


                        if (isPaused && Player.ButtonPromptList.Any(x => x.Group == "ContinueTransaction" && x.IsPressedNow))
                        {
                            Player.ButtonPromptList.RemoveAll(x => x.Group == "ContinueTransaction");
                            isPaused = false;

                            if (Ped != null && Ped.Pedestrian.Exists() && !Ped.IsInVehicle && !Player.IsInVehicle)
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
                            EntryPoint.WriteToConsole("Unpased Person Transaction");
                        }

                        MenuPool.ProcessMenus();
                        Transaction.PurchaseMenu?.Update();
                        Transaction.SellMenu?.Update();
                        GameFiber.Yield();
                    }
                }
                else
                {
                    EntryPoint.WriteToConsole("Person Transaction Dispose 1");
                    Dispose();
                }
            }
            EntryPoint.WriteToConsole("Person Transaction Dispose 2");
            Dispose();

        }, "PersonTransaction");
    }
    public override void Dispose()
    {
        EntryPoint.WriteToConsole($"PERSON TRANSACTION Dispose IsDisposed {IsDisposed}");
        if (!IsDisposed)
        {
            IsDisposed = true;
            Player.IsConversing = false;
            Player.IsTransacting = false;
            Transaction?.DisposeTransactionMenu();
            DisposeInteractionMenu();
            Player.LastFriendlyVehicle = null;

            if(PedCanBeTasked)
            {
                Ped.CanBeTasked = true;
            }
            if(PedCanBeAmbientTasked)
            {
                Ped.CanBeAmbientTasked = true;
            }


            Player.ButtonPromptList.RemoveAll(x => x.Group == "ContinueTransaction");
            if (Ped != null && Ped.Pedestrian.Exists())
            {
               // Ped.Pedestrian.CanBePulledOutOfVehicles = true;
               if(!PedWasPersistent)
                {
                    //Ped.Pedestrian.IsPersistent = false;
                }
                if(PanickedByPlayer)
                {
                    





                    NativeFunction.Natives.TASK_SMART_FLEE_PED(Ped.Pedestrian, Player.Character, 100f, -1, false, false);
                    EntryPoint.WriteToConsole($"PersonTransaction: DISPOSE PANIC 1", 3);
                }

                else if(Ped.Pedestrian.IsInAnyVehicle(false) && PedEnteringPlayerVehicle && Ped.Pedestrian.CurrentVehicle.Exists() && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Ped.Pedestrian.CurrentVehicle.Handle == Player.CurrentVehicle.Vehicle.Handle)
                {
               
                        NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                        Ped.Pedestrian.BlockPermanentEvents = false;
                        Ped.Pedestrian.KeepTasks = false;
                    
                    NativeFunction.Natives.TASK_LEAVE_VEHICLE(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 64);
                    EntryPoint.WriteToConsole($"PersonTransaction: DISPOSE CAR 1", 3);
                }
                else if (AssociatedStore != null && AssociatedStore.VendorHeading != 0f)
                {
       
                        Ped.Pedestrian.BlockPermanentEvents = false;
                        Ped.Pedestrian.KeepTasks = false;
                    
                    NativeFunction.Natives.TASK_ACHIEVE_HEADING(Ped.Pedestrian, AssociatedStore.VendorHeading, -1);
                    EntryPoint.WriteToConsole($"PersonTransaction: DISPOSE Set Heading", 3);
                }
                else
                {
               
                        NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                        Ped.Pedestrian.BlockPermanentEvents = false;
                        Ped.Pedestrian.KeepTasks = false;
                    
                    EntryPoint.WriteToConsole($"PersonTransaction: DISPOSE UnTasking", 3);
                }
                //if (Ped.Pedestrian.CurrentVehicle.Exists())
                //{
                //    NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 27, -1);
                //}

            }
            NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
        }
    }
    private void StopVehicleActions()
    {
        GameFiber.StartNew(delegate
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
            StartBuyAnimation(modItem, menuItem, totalItems);

            if (Ped.GetType() == typeof(GangMember))
            {
                GangMember gm = (GangMember)Ped;
                Player.GangRelationships.ChangeReputation(gm.Gang, menuItem.PurchasePrice * totalItems, true);
            }
            Transaction.PurchaseMenu?.Show();
        }
    }
    public void OnItemSold(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        if (modItem != null)
        {
            MenuPool.CloseAllMenus();
            StartSellAnimation(modItem, menuItem, totalItems);
            if (Ped.GetType() == typeof(GangMember))
            {
                GangMember gm = (GangMember)Ped;
                Player.GangRelationships.ChangeReputation(gm.Gang, menuItem.SalesPrice * totalItems, true);
            }
            Transaction.SellMenu?.Show();
        }
    }

    private void InteractionMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Buy")
        {
            Transaction?.SellMenu?.Dispose();
            Transaction?.PurchaseMenu?.Show();
        }
        else if (selectedItem.Text == "Sell")
        {
            Transaction?.PurchaseMenu?.Dispose();
            Transaction?.SellMenu?.Show();
        }
        else if (selectedItem == GetInCar)
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
                NativeFunction.Natives.TASK_ENTER_VEHICLE(passenegerToAdd, vehicleToEnter, -1, seatIndex, 0.5f, 0);
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
            EntryPoint.WriteToConsole("Paused Person Transaction");

            InteractionMenu.Visible = false;
            isPaused = true;

            if (!Player.ButtonPromptList.Any(x => x.Group == "ContinueTransaction"))
            {
                Player.ButtonPromptList.Add(new ButtonPrompt("Continue Transaction", "ContinueTransaction", "ContinueTransaction", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 101));
            }


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

            if (!Player.ButtonPromptList.Any(x => x.Group == "ContinueTransaction"))
            {
                Player.ButtonPromptList.Add(new ButtonPrompt("Continue Transaction", "ContinueTransaction", "ContinueTransaction", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 101));
            }

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
            EntryPoint.WriteToConsole("Paused Person Transaction");
            SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY" }, true);
            SayAvailableAmbient(personToFollow, new List<string>() { "GENERIC_YES" }, true);
            
            InteractionMenu.Visible = false;
            

            if (!Player.ButtonPromptList.Any(x => x.Group == "ContinueTransaction"))
            {
                Player.ButtonPromptList.Add(new ButtonPrompt("Continue Transaction", "ContinueTransaction", "ContinueTransaction", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 101));
            }

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
                    EntryPoint.WriteToConsole("PersonTransaction, THEY HAVE BEEN BLOCKED< CLEARED< AND FUCKING TASKED SO FUCK OFF");
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
                Ped.HasSpokenWithPlayer = true;
            }
            IsActivelyConversing = false;
        }
    }
    private void StartBuyAnimation(ModItem modItem, MenuItem menuItem, int totalItems)
    {

       //MenuPool.CloseAllMenus();

        //Hide();
        IsActivelyConversing = true;
        //if (hideShowMenu)
        //{
        //    Hide();
        //}


        string modelName = "";
        bool HasProp = false;
        bool isWeapon = false;
        if (modItem.PackageItem != null && modItem.PackageItem.ModelName != "")
        {
            modelName = modItem.PackageItem.ModelName;
            HasProp = true;
            if (modItem.PackageItem.Type == ePhysicalItemType.Weapon)
            {
                isWeapon = true;
            }
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
        Player.ButtonPromptList.Clear();
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY", "GENERIC_YES", "BLOCKED_GENEIRC" }, true);
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "mp_common", "givetake1_a", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", "givetake1_b", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
        }
        GameFiber.Sleep(500);
        if (!isWeapon && Ped.Pedestrian.Exists() && HasProp && modelName != "")
        {
            SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
            GameFiber.Yield();
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, 57005), modItem.ModelItem.AttachOffsetOverride, modItem.ModelItem.AttachRotationOverride);
            }
        }
        GameFiber.Sleep(500);
        if (isWeapon)
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "WEAPON_PURCHASE", "HUD_AMMO_SHOP_SOUNDSET", 0);
        }
        else
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
        }
        Game.RemoveNotification(NotificationHandle);
        if (modItem.MeasurementName == "Item")
        {
            NotificationHandle = Game.DisplayNotification($"You have purchased {totalItems} ~r~{modItem.Name}(s)~s~");
        }
        else
        {
            NotificationHandle = Game.DisplayNotification($"You have purchased {totalItems} {modItem.MeasurementName}(s) of ~r~{modItem.Name}~s~");
        }



        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, 57005), modItem.ModelItem.AttachOffsetOverride, modItem.ModelItem.AttachRotationOverride);
            }
        }
        GameFiber.Sleep(1000);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.Delete();
            }
            SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, true);
            SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BYE", "GENERIC_THANKS", "PED_RANT" }, true);
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
        //if (hideShowMenu)
        //{
        //    Show();
        //}
    }
    private void StartSellAnimation(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        //Hide();

      //  MenuPool.CloseAllMenus();

        string modelName = "";
        bool HasProp = false;
        bool isWeapon = false;
        if (modItem.PackageItem != null && modItem.PackageItem.ModelName != "")
        {
            modelName = modItem.PackageItem.ModelName;
            HasProp = true;
            if (modItem.PackageItem.Type == ePhysicalItemType.Weapon)
            {
                isWeapon = true;
            }
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
        Player.ButtonPromptList.Clear();
        SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BUY", "GENERIC_YES", "BLOCKED_GENEIRC" }, true);
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "mp_common", "givetake1_b", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", "givetake1_a", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
        }
        GameFiber.Sleep(500);

        if (!isWeapon && Ped.Pedestrian.Exists() && HasProp && modelName != "")
        {
            SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
            GameFiber.Yield();
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, 57005), modItem.ModelItem.AttachOffsetOverride, modItem.ModelItem.AttachRotationOverride);
            }
        }
        GameFiber.Sleep(500);
        if(isWeapon)
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "WEAPON_PURCHASE", "HUD_AMMO_SHOP_SOUNDSET", 0);
        }
        else
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
        }

        
        Game.RemoveNotification(NotificationHandle);
        if (modItem.MeasurementName == "Item")
        {
            NotificationHandle = Game.DisplayNotification($"You have sold {totalItems} ~r~{modItem.Name}(s)~s~");
        }
        else
        {
            NotificationHandle = Game.DisplayNotification($"You have sold {totalItems} {modItem.MeasurementName}(s) of ~r~{modItem.Name}~s~");
        }



        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, 57005), modItem.ModelItem.AttachOffsetOverride, modItem.ModelItem.AttachRotationOverride);
            }
        }
        GameFiber.Sleep(1000);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.Delete();
            }
            SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, true);
            //SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BYE", "GENERIC_THANKS", "PED_RANT" }, true);
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

}

