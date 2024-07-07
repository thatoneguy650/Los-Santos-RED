using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Serialization;

public class DebugHelperSubMenu : DebugSubMenu
{
    private Vector3 Offset;
    private Rotator Rotation;
    private bool isPrecise;
    private bool isRunning;
    private uint GameTimeLastAttached;
    private bool IsBigMapActive;
    private bool IsSSMode;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private IPoliceRespondable PoliceRespondable;
    private ModDataFileManager ModDataFileManager;
    private IGangs Gangs;
    private UIMenu HelperMenuItem;

    public DebugHelperSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IEntityProvideable world, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, ITimeControllable time, IPoliceRespondable policeRespondable, ModDataFileManager modDataFileManager, IGangs gangs) : base(debug, menuPool, player)
    {
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Time = time;
        PoliceRespondable = policeRespondable;
        ModDataFileManager = modDataFileManager;
        Gangs = gangs;
    }
    public override void AddItems()
    {
        HelperMenuItem = MenuPool.AddSubMenu(Debug, "Helper Menu");
        HelperMenuItem.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Various helper items";

        UIMenuItem SetBigMap = new UIMenuItem("Toggle Big MiniMap", "Toggles the big GTAO style mini map");
        SetBigMap.Activated += (menu, item) =>
        {
            IsBigMapActive = !IsBigMapActive;
            NativeFunction.Natives.SET_BIGMAP_ACTIVE(IsBigMapActive, false);
            //Game.DisplaySubtitle($"IsBigMapActive:{IsBigMapActive}"); 
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(SetBigMap);



        UIMenuItem setSSMode = new UIMenuItem("Toggle Screenshot", "Toggles Screenshot mode");
        setSSMode.Activated += (menu, item) =>
        {
            IsSSMode = !IsSSMode;
            
            if(IsSSMode)
            {
                Settings.SettingsManager.SetScreenshotMode();
            }
            else
            {
                Settings.SettingsManager.DisableScreenshotMode();
            }
            //Game.DisplaySubtitle($"IsBigMapActive:{IsBigMapActive}"); 
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(setSSMode);

        UIMenuItem spawnNearbyItems = new UIMenuItem("Fill Nearby Station", "Fill nearby station with all cars descending");
        spawnNearbyItems.Activated += (menu, item) =>
        {
            FillClosestStaion();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(spawnNearbyItems);






        UIMenuListScrollerItem<Agency> spawnCarPark = new UIMenuListScrollerItem<Agency>("Fill Car Park", "Spawn le agency car park", World.ModDataFileManager.Agencies.GetAgencies());
        spawnCarPark.Activated += (menu, item) =>
        {
            FillCarPark(spawnCarPark.SelectedItem);
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(spawnCarPark);



        UIMenuListScrollerItem<string> setlocalSirens = new UIMenuListScrollerItem<string>("Set Local Sirens", "force local sirens for cars", new List<string> { "On","Off" });
        setlocalSirens.Activated += (menu, item) =>
        {
            SetSirens(setlocalSirens.SelectedItem == "On");
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(setlocalSirens);



        UIMenuItem highlightProp = new UIMenuItem("Highlight Prop", "Get some info about the nearest prop.");
        highlightProp.Activated += (menu, item) =>
        {
            HighlightProp();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(highlightProp);

        UIMenuItem getPropPosition = new UIMenuItem("Get Nearby Prop Positions", "Get nearby prop positions");
        getPropPosition.Activated += (menu, item) =>
        {
            PropPosition();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(getPropPosition);

        //X:-280.5973 Y:6227.909 Z:30.72211

        UIMenuItem propAttachMenu = new UIMenuItem("Prop Attachment", "Do some prop attachments");
        propAttachMenu.Activated += (menu, item) =>
        {
            SetPropAttachment();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(propAttachMenu);



        UIMenuItem propVehicleAttachMenu = new UIMenuItem("Prop Vehicle Attachment", "Do some prop vehicle attachments");
        propVehicleAttachMenu.Activated += (menu, item) =>
        {
            SetPropVehicleAttachment();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(propVehicleAttachMenu);

        UIMenuItem weaponAliasMenu = new UIMenuItem("Weapon Alias Attachment", "Do some weapon alias prop attachments");
        weaponAliasMenu.Activated += (menu, item) =>
        {
            SetWeaponAliasAttachment();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(weaponAliasMenu);

        UIMenuItem particleAttachMenu = new UIMenuItem("Particle Attachment", "Get some particle offsets");
        particleAttachMenu.Activated += (menu, item) =>
        {
            SetParticleAttachment();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(particleAttachMenu);


        UIMenuItem VehicleShowcaseMenu = new UIMenuItem("Showcase Vehicle", "Showcase the current looked at vehicle");
        VehicleShowcaseMenu.Activated += (menu, item) =>
        {
            VehicleShowcase vs = new VehicleShowcase(World.Vehicles.GetClosestVehicleExt(Player.Character.Position, true, 15f), Settings, Time);
            vs.Start();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(VehicleShowcaseMenu);


        UIMenuItem ShowcaseLocations = new UIMenuItem("Showcase Teleport", "Teleport to showcase location");
        ShowcaseLocations.Activated += (menu, item) =>
        {
            Game.LocalPlayer.Character.Position = new Vector3(229.028f, -988.8007f, -99.52672f);
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(ShowcaseLocations);



        UIMenuItem PrintClassStuffMenu = new UIMenuItem("Print Vehicle Class", "Print some select class stuff to the log. DispatchableVehicles");
        PrintClassStuffMenu.Activated += (menu, item) =>
        {
            PrintVehicleClasses();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(PrintClassStuffMenu);

        UIMenuItem PrintLocClassStuffMenu = new UIMenuItem("Print Location Class", "Print some select class stuff to the log. Locations.");
        PrintLocClassStuffMenu.Activated += (menu, item) =>
        {
            PrintLocationClasses(null);
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(PrintLocClassStuffMenu);



        UIMenuItem PrintInteriors = new UIMenuItem("Print Interiors Class", "Print some select class stuff to the log. Interiors.");
        PrintInteriors.Activated += (menu, item) =>
        {
            PrintInteriorClasses(null);
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(PrintInteriors);





        UIMenuItem PrintLocBAseClassStuffMenu = new UIMenuItem("Print Location Class Base", "Print some select class stuff to the log. Locations Base.");
        PrintLocBAseClassStuffMenu.Activated += (menu, item) =>
        {
            PrintLocationClasses(new List<string>() { "Name", "Description", "EntrancePosition", "EntranceHeading", "OpenTime", "CloseTime", "StateLocation", "AssignedAssociationID", "MenuID" });
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(PrintLocBAseClassStuffMenu);



        //Print Zone
        UIMenuItem PrintCurrentZone = new UIMenuItem("Display Current Zone", "Display the current zone");
        PrintCurrentZone.Activated += (menu, item) =>
        {
            menu.Visible = false;
            IntPtr ptr = Rage.Native.NativeFunction.Natives.GET_NAME_OF_ZONE<IntPtr>(Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z);
            Game.DisplaySubtitle(Marshal.PtrToStringAnsi(ptr));
            // Game.DisplaySubtitle(System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Rage.Native.NativeFunction.Natives.GET_NAME_OF_ZONE<IntPtr>(Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z)));
        };
        HelperMenuItem.AddItem(PrintCurrentZone);

        //Print Street
        UIMenuItem PrintCurrentStreet = new UIMenuItem("Display Current Street", "Display the current street");
        PrintCurrentStreet.Activated += (menu, item) =>
        {
            menu.Visible = false;


            Vector3 position = Game.LocalPlayer.Character.Position;
            bool hasNode = NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE<bool>(position.X, position.Y, position.Z, out Vector3 outPos, 0, 3.0f, 0f);
            Vector3 ClosestNode = outPos;

            int StreetHash = 0;
            int CrossingHash = 0;
            string CurrentStreetName;
            unsafe
            {
                NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", ClosestNode.X, ClosestNode.Y, ClosestNode.Z, &StreetHash, &CrossingHash);
            }
            string StreetName = string.Empty;
            if (StreetHash != 0)
            {
                unsafe
                {
                    IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);
                    StreetName = Marshal.PtrToStringAnsi(ptr);
                }
                CurrentStreetName = StreetName;
                GameFiber.Yield();
            }
            else
            {
                CurrentStreetName = "";
            }


            Game.DisplaySubtitle($"StreetHash {StreetHash} CurrentStreetName {CurrentStreetName} StreetName {StreetName}");
            GameFiber.Sleep(200);
        };
        HelperMenuItem.AddItem(PrintCurrentStreet);
        //

        //VehicleShowcase


        UIMenuItem SetNearestPedUnconscious = new UIMenuItem("Set Unconscious", "Set the nearest ped as unconscious.");
        SetNearestPedUnconscious.Activated += (menu, item) =>
        {
            menu.Visible = false;
            PedExt toGet = World.Pedestrians.CivilianList.OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
            if (toGet == null)
            {
                return;
            }
            toGet.CurrentHealthState.SetUnconscious(PoliceRespondable);
        };
        HelperMenuItem.AddItem(SetNearestPedUnconscious);



        UIMenuItem printBankAccounts = new UIMenuItem("Print Bank Accounts", "Print Bank Accounts");
        printBankAccounts.Activated += (menu, item) =>
        {
            Player.BankAccounts.WriteToConsole();
            menu.Visible = false;

        };
        HelperMenuItem.AddItem(printBankAccounts);



        UIMenuItem killScripts = new UIMenuItem("Kill Script", "Kill a script with the given name. DANGEROUS!");
        killScripts.Activated += (menu, item) =>
        {
            string scriptName = NativeHelper.GetKeyboardInput("");
            if (!string.IsNullOrEmpty(scriptName))
            {
                Game.TerminateAllScriptsWithName(scriptName);
            }

            menu.Visible = false;

        };
        HelperMenuItem.AddItem(killScripts);



        UIMenuItem dotoiletsittingmenu = new UIMenuItem("Sit On Toilet", "");
        dotoiletsittingmenu.Activated += (menu, item) =>
        {
            Player.ActivityManager.StartSittingOnToilet(false, false);

            menu.Visible = false;

        };
        HelperMenuItem.AddItem(dotoiletsittingmenu);


        AddPhoneItems();



        AddAnimationItems();

        string audioName = "";
        string audioRef = "";
        UIMenuItem playAudioName = new UIMenuItem("Set Audio Name", "");
        playAudioName.Activated += (menu, item) =>
        {
            string scriptName = NativeHelper.GetKeyboardInput("");
            if (!string.IsNullOrEmpty(scriptName))
            {
                playAudioName.RightLabel = scriptName;
                audioName = scriptName;
            }
        };
        HelperMenuItem.AddItem(playAudioName);

        UIMenuItem playAudioRef = new UIMenuItem("Set Audio Ref", "");
        playAudioRef.Activated += (menu, item) =>
        {
            string scriptName = NativeHelper.GetKeyboardInput("");
            if (!string.IsNullOrEmpty(scriptName))
            {
                playAudioRef.RightLabel = scriptName;
                audioRef = scriptName;
            }
        };
        HelperMenuItem.AddItem(playAudioRef);

        UIMenuItem playAudio = new UIMenuItem("Play Audio", "");
        playAudio.Activated += (menu, item) =>
        {

            if (!string.IsNullOrEmpty(audioRef) && !string.IsNullOrEmpty(audioName))
            {
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, audioName, audioRef, 0);
            }
        };
        HelperMenuItem.AddItem(playAudio);







    }

    private void SetSirens(bool setOn)
    {
        foreach(VehicleExt vehicleExt in World.Vehicles.PoliceVehicles.ToList())
        {
            if(!vehicleExt.Vehicle.Exists())
            {
                continue;
            }
            if(!vehicleExt.Vehicle.HasSiren)
            {
                continue;
            }
            vehicleExt.Vehicle.IsSirenOn= setOn;
        }
    }

    private void FillClosestStaion()
    {
        GameLocation gl = World.Places.ActiveLocations.Where(x=> x.AssignedAgency != null && x.PossibleVehicleSpawns.Any()).OrderBy(x=> x.DistanceToPlayer).FirstOrDefault();
        if(gl == null)
        {
            return;
        }
        World.Vehicles.ClearSpawned(true);
        Player.Dispatcher.LocationDispatcher.ForceSpawnAllVehicles(gl);
    }
    private void FillCarPark(Agency agency)
    {
        if(agency == null)
        {
            return;
        }
        if(Player.Character.DistanceTo(new Vector3(638.7681f, 635.7558f, 128.9111f))>= 100f)
        {
            return;
        }
        List<ConditionalLocation> list = new List<ConditionalLocation>()
        {
            new LEConditionalLocation(new Vector3(638.7681f, 635.7558f, 128.9111f), 248.3244f, 100f),
            new LEConditionalLocation(new Vector3(637.551f, 632.4586f, 128.9111f), 249.2319f, 100f),
            new LEConditionalLocation(new Vector3(636.3085f, 629.0381f, 128.9111f), 247.6597f, 100f),
            new LEConditionalLocation(new Vector3(634.639f, 626.0833f, 128.9111f), 249.1287f, 100f),
            new LEConditionalLocation(new Vector3(633.4773f, 622.7151f, 128.9111f), 247.7914f, 100f),
            new LEConditionalLocation(new Vector3(632.2473f, 619.6294f, 128.9111f), 247.3871f, 100f),
            new LEConditionalLocation(new Vector3(630.8295f, 616.2911f, 128.9111f), 246.7872f, 100f),
            new LEConditionalLocation(new Vector3(629.7604f, 612.856f, 128.9111f), 249.0674f, 100f),
            new LEConditionalLocation(new Vector3(628.8015f, 609.6461f, 128.9111f), 248.2168f, 100f),
            new LEConditionalLocation(new Vector3(627.4628f, 606.449f, 128.9111f), 245.4982f, 100f),
            new LEConditionalLocation(new Vector3(622.7714f, 637.9483f, 128.9111f), 248.426f, 100f),
            new LEConditionalLocation(new Vector3(621.7044f, 634.6458f, 128.9111f), 248.9079f, 100f),
            new LEConditionalLocation(new Vector3(620.6009f, 631.4076f, 128.9111f), 249.7423f, 100f),
            new LEConditionalLocation(new Vector3(619.4406f, 627.96f, 128.9111f), 247.8293f, 100f),
            new LEConditionalLocation(new Vector3(618.3074f, 624.7338f, 128.9111f), 247.3478f, 100f),
            new LEConditionalLocation(new Vector3(617.0331f, 621.3472f, 128.9111f), 248.2468f, 100f),
            new LEConditionalLocation(new Vector3(615.9576f, 618.2318f, 128.9111f), 243.997f, 100f),
            new LEConditionalLocation(new Vector3(614.4206f, 614.8268f, 128.9111f), 246.6588f, 100f),
            new LEConditionalLocation(new Vector3(613.1279f, 611.5945f, 128.9111f), 248.3828f, 100f),

        };
        World.Vehicles.ClearSpawned(true);
        bool preSettings = Settings.SettingsManager.WorldSettings.CheckAreaBeforeVehicleSpawn;
        Settings.SettingsManager.WorldSettings.CheckAreaBeforeVehicleSpawn = false;
        Player.Dispatcher.LocationDispatcher.ForceSpawnAllVehicles(list, agency);
        Settings.SettingsManager.WorldSettings.CheckAreaBeforeVehicleSpawn = preSettings;
    }
    private void AddTrunkItems()
    {

    }
    private void AddAnimationItems()
    {
        //dead_1
        UIMenuItem playFacialAnimMenu = new UIMenuItem("Play Facial Anim", "Play a facial animation on the current ped!");
        playFacialAnimMenu.Activated += (menu, item) =>
        {
            string animDictionary = NativeHelper.GetKeyboardInput("");//: facials@gen_male@base
            string anim = NativeHelper.GetKeyboardInput("dead_1");
            if(string.IsNullOrEmpty(anim))
            {
                return;
            }
            NativeFunction.Natives.SET_FACIAL_IDLE_ANIM_OVERRIDE(Player.Character, anim, string.IsNullOrEmpty(animDictionary) ? "" : animDictionary);
        };
        HelperMenuItem.AddItem(playFacialAnimMenu);

        UIMenuItem stopFacialAnimation = new UIMenuItem("Stop Facial Anim", "Stop the facial animation on the current ped!");
        stopFacialAnimation.Activated += (menu, item) =>
        {
            NativeFunction.Natives.CLEAR_FACIAL_IDLE_ANIM_OVERRIDE(Player.Character);
        };
        HelperMenuItem.AddItem(stopFacialAnimation);
    }

    private void AddPhoneItems()
    {
        //GET_PED_PHONE_PALETTE_IDX
        UIMenuItem setPedMobile = new UIMenuItem("Set Ped Mobile", "Set closest ped mobile!");
        setPedMobile.Activated += (menu, item) =>
        {
            PedExt pedExt = World.Pedestrians.PedExts.OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
            if(pedExt == null || !pedExt.Pedestrian.Exists())
            {
                return;
            }
           // int phonePalette = NativeFunction.Natives.GET_PED_PHONE_PALETTE_IDX<int>(pedExt.Pedestrian);
           // EntryPoint.WriteToConsole($"phonePalette {phonePalette}");
            NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", pedExt.Pedestrian, 8000);
           // EntryPoint.WriteToConsole($"phonePalette {phonePalette}");



            menu.Visible = false;

            GameFiber.Sleep(500);
            if (pedExt == null || !pedExt.Pedestrian.Exists())
            {
                return;
            }
            List<Rage.Object> Objects = Rage.World.GetAllObjects().ToList(); //World.GetAllObjects().ToList(); //EntryPoint.ModController.AllObjects.ToList();//Rage.World.GetAllObjects().ToList();
            foreach (Rage.Object obj in Objects)
            {
                if (obj.Exists() && obj.DistanceTo2D(pedExt.Pedestrian) <= 5f)
                {
                    EntryPoint.WriteToConsole($"{obj.Model.Name} {obj.Model.Hash}");
                }
            }




        };
        HelperMenuItem.AddItem(setPedMobile);



        UIMenuNumericScrollerItem<int> setPedMobilePalette = new UIMenuNumericScrollerItem<int>("Set Ped Phone Color", "Sets the phone color of a ped?",0,99,1);
        setPedMobilePalette.Value = 0;
        setPedMobilePalette.Activated += (menu, item) =>
        {
            PedExt pedExt = World.Pedestrians.PedExts.OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
            if (pedExt == null || !pedExt.Pedestrian.Exists())
            {
                return;
            }
            pedExt.Pedestrian.BlockPermanentEvents = true;
            NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", pedExt.Pedestrian, -1);
            NativeFunction.Natives.SET_PED_PHONE_PALETTE_IDX(pedExt.Pedestrian, setPedMobilePalette.Value);

            menu.Visible = false;
        };
        HelperMenuItem.AddItem(setPedMobilePalette);


        UIMenuItem doPedRagdollLocation = new UIMenuItem("Flag Ragdoll", "Flag the nearest ped position");
        doPedRagdollLocation.Activated += (menu, item) =>
        {
            FlagRagdollLocation();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(doPedRagdollLocation);
        //        

    }
    private void FlagRagdollLocation()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                PedExt pedExt = World.Pedestrians.PedExts.Where(x => x.IsDead || x.IsUnconscious).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
                if (pedExt == null || !pedExt.Pedestrian.Exists())
                {
                    return;
                }
                if (!int.TryParse(NativeHelper.GetKeyboardInput("0"), out int boneid))
                {
                    return;
                }
                Vector3 DesiredPosition = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", pedExt.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", pedExt.Pedestrian, boneid));
                float calcHeading = (float)GetHeading(Player.Character.Position, DesiredPosition);
                float calcHeading2 = (float)CalculeAngle(DesiredPosition, Player.Character.Position);
                float DesiredHeading = calcHeading2;
                uint GameTimeStarted = Game.GameTime;
                while(Game.GameTime - GameTimeStarted <= 10000 && EntryPoint.ModController.IsRunning)
                {
                    Rage.Debug.DrawArrowDebug(DesiredPosition, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.White);
                    Game.DisplaySubtitle($"{Game.LocalPlayer.Character.Heading} {calcHeading} {calcHeading2}");
                    GameFiber.Yield();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "ATM Interact");
    }

    private double GetHeading(Vector3 a, Vector3 b)
    {
        double x = b.X - a.X;
        double y = b.Y - a.Y;
        return 270 - Math.Atan2(y, x) * (180 / Math.PI);
    }
    private double CalculeAngle(Vector3 start, Vector3 arrival)
    {
        var deltaX = Math.Pow((arrival.X - start.X), 2);
        var deltaY = Math.Pow((arrival.Y - start.Y), 2);
        var radian = Math.Atan2((arrival.Y - start.Y), (arrival.X - start.X));
        var angle = (radian * (180 / Math.PI) + 360) % 360;
        return angle;
    }



    private void WriteToClassCreator(String TextToLog, int test, string fileName)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + $"{fileName}.txt", sb.ToString());
        sb.Clear();
    }

    private void SetPropAttachment()
    {
        string PropName = NativeHelper.GetKeyboardInput("prop_cash_pile_02");
        try
        {
            Rage.Object SmokedItem = new Rage.Object(Game.GetHashKey(PropName), Player.Character.GetOffsetPositionUp(50f));
            GameFiber.Yield();
            string headBoneName = "BONETAG_HEAD";
            string handRBoneName = "BONETAG_R_PH_HAND";
            string handLBoneName = "BONETAG_L_PH_HAND";
            string boneName = headBoneName;
            string thighRBoneName = "BONETAG_R_THIGH";
            string thighLBoneName = "BONETAG_L_THIGH";
            string pelvisBoneName = "BONETAG_PELVIS";
            string spineRootBoneName = "BONETAG_SPINE_ROOT";
            string spineBoneName = "BONETAG_SPINE";
            string wantedBone = NativeHelper.GetKeyboardInput("RHand");
            if (wantedBone == "RHand")
            {
                boneName = handRBoneName;
            }
            else if (wantedBone == "LHand")
            {
                boneName = handLBoneName;
            }
            else if (wantedBone == "LThigh")
            {
                boneName = thighLBoneName;
            }
            else if (wantedBone == "RThigh")
            {
                boneName = thighRBoneName;
            }
            else if (wantedBone == "Pelvis")
            {
                boneName = pelvisBoneName;
            }
            else if (wantedBone == "SpineRoot")
            {
                boneName = spineRootBoneName;
            }
            else if (wantedBone == "Spine")
            {
                boneName = spineBoneName;
            }
            else if (wantedBone == "Head")
            {
                boneName = headBoneName;
            }
            else
            {
                boneName = wantedBone;
            }
            uint GameTimeLastAttached = 0;
            Offset = new Vector3();
            Rotation = new Rotator();
            isPrecise = false;
            if (SmokedItem.Exists())
            {
                string dictionary = NativeHelper.GetKeyboardInput("mp_common");
                string animation = NativeHelper.GetKeyboardInput("givetake1_a");




                //            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "mp_common", "givetake1_a", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", "givetake1_b", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
                AnimationDictionary.RequestAnimationDictionay(dictionary);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 4.0f, -4.0f, -1, (int)(AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask), 0, false, false, false);//-1
                isRunning = true;
                AttachItem(SmokedItem, boneName, new Vector3(0.0f, 0.0f, 0f), new Rotator(0f, 0f, 0f));
                GameFiber.StartNew(delegate
                {
                    try
                    {
                        while (!Game.IsKeyDownRightNow(Keys.Space) && SmokedItem.Exists())
                        {
                            if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
                            {
                                AttachItem(SmokedItem, boneName, Offset, Rotation);
                                GameTimeLastAttached = Game.GameTime;
                            }
                            if (Game.IsKeyDown(Keys.B))
                            {
                                //EntryPoint.WriteToConsoleTestLong($"Item {PropName} Attached to  {boneName} new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");

                                EntryPoint.WriteToConsole($"new PropAttachment(\"NAMEHERE\", \"{boneName}\", new Vector3({Offset.X}f, {Offset.Y}f, {Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)),");
                                GameFiber.Sleep(500);
                            }
                            if (Game.IsKeyDown(Keys.N))
                            {
                                isPrecise = !isPrecise;
                                GameFiber.Sleep(500);
                            }
                            if (Game.IsKeyDown(Keys.D0))
                            {
                                isRunning = !isRunning;
                                NativeFunction.Natives.SET_ENTITY_ANIM_SPEED(Player.Character, dictionary, animation, isRunning ? 1.0f : 0.0f);
                                GameFiber.Sleep(500);
                            }
                            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, dictionary, animation);
                            Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise} ~n~Press 0 Pause{isRunning}");
                            Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
                            //Game.DisplaySubtitle($"Current Animation Time {AnimationTime}");
                            GameFiber.Yield();
                        }

                        if (SmokedItem.Exists())
                        {
                            SmokedItem.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        EntryPoint.ModController.CrashUnload();
                    }
                }, "Run Debug Logic");
            }
        }
        catch (Exception e)
        {
            Game.DisplayNotification("ERROR DEBUG");
        }
    }


    private void SetPropVehicleAttachment()
    {

        //hub_lr =  new Vector3(0.07999999f, 0.05f, 0f);new Rotator(-30f, -80f, -120f);


        string PropName = NativeHelper.GetKeyboardInput("prop_cs_fuel_nozle");
        try
        {
            Rage.Object SmokedItem = new Rage.Object(Game.GetHashKey(PropName), Player.Character.GetOffsetPositionUp(50f));
            GameFiber.Yield();
            string headBoneName = "wheel_lf";
            string boneName = headBoneName;
            string wantedBone = NativeHelper.GetKeyboardInput("wheel_lr");
            boneName = wantedBone;   
            uint GameTimeLastAttached = 0;
            Offset = new Vector3();
            Rotation = new Rotator();
            isPrecise = false;
            if (SmokedItem.Exists())
            {
               isRunning = true;
                AttachVehicleItem(Player.InterestedVehicle, SmokedItem, boneName, new Vector3(0.0f, 0.0f, 0f), new Rotator(0f, 0f, 0f));
                GameFiber.StartNew(delegate
                {
                    try
                    {
                        while (!Game.IsKeyDownRightNow(Keys.Space) && SmokedItem.Exists())
                        {
                            if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
                            {
                                AttachVehicleItem(Player.InterestedVehicle, SmokedItem, boneName, Offset, Rotation);
                                GameTimeLastAttached = Game.GameTime;
                            }
                            if (Game.IsKeyDown(Keys.B))
                            {
                                //EntryPoint.WriteToConsoleTestLong($"Item {PropName} Attached to  {boneName} new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");

                                EntryPoint.WriteToConsole($"new PropAttachment(\"NAMEHERE\", \"{boneName}\", new Vector3({Offset.X}f, {Offset.Y}f, {Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)),");
                                GameFiber.Sleep(500);
                            }
                            if (Game.IsKeyDown(Keys.N))
                            {
                                isPrecise = !isPrecise;
                                GameFiber.Sleep(500);
                            }
                            Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise} ~n~Press 0 Pause{isRunning}");
                            Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
                            //Game.DisplaySubtitle($"Current Animation Time {AnimationTime}");
                            GameFiber.Yield();
                        }

                        if (SmokedItem.Exists())
                        {
                            SmokedItem.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        EntryPoint.ModController.CrashUnload();
                    }
                }, "Run Debug Logic");
            }
        }
        catch (Exception e)
        {
            Game.DisplayNotification("ERROR DEBUG");
        }
    }



    private void AttachItem(Rage.Object SmokedItem, string boneName, Vector3 offset, Rotator rotator)
    {
        if (SmokedItem.Exists())
        {
            SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, boneName), offset, rotator);

        }
    }
    private void AttachVehicleItem(VehicleExt vehicleExt, Rage.Object SmokedItem, string boneName, Vector3 offset, Rotator rotator)
    {
        if (SmokedItem.Exists() && vehicleExt != null && vehicleExt.Vehicle.Exists())
        {
            SmokedItem.AttachTo(vehicleExt.Vehicle, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", vehicleExt.Vehicle, boneName), offset, rotator);

        }
    }
    private bool CheckAttachmentkeys()
    {

        float adderOffset = isPrecise ? 0.001f : 0.01f;
        float rotatorOFfset = isPrecise ? 1.0f : 10f;
        if (Game.IsKeyDownRightNow(Keys.T))//X UP?
        {
            Offset = new Vector3(Offset.X + adderOffset, Offset.Y, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.G))//X Down?
        {
            Offset = new Vector3(Offset.X - adderOffset, Offset.Y, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.Y))//Y UP?
        {
            Offset = new Vector3(Offset.X, Offset.Y + adderOffset, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.H))//Y Down?
        {
            Offset = new Vector3(Offset.X, Offset.Y - adderOffset, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.U))//Z Up?
        {
            Offset = new Vector3(Offset.X, Offset.Y, Offset.Z + adderOffset);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.J))//Z Down?
        {
            Offset = new Vector3(Offset.X, Offset.Y, Offset.Z - adderOffset);
            return true;
        }

        else if (Game.IsKeyDownRightNow(Keys.I))//XR Up?
        {
            Rotation = new Rotator(Rotation.Pitch + rotatorOFfset, Rotation.Roll, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.K))//XR Down?
        {
            Rotation = new Rotator(Rotation.Pitch - rotatorOFfset, Rotation.Roll, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.O))//YR Up?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll + rotatorOFfset, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.L) || Game.IsKeyDownRightNow(Keys.OemPeriod))//YR Down?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll - rotatorOFfset, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.OemOpenBrackets))//ZR Up?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll, Rotation.Yaw + rotatorOFfset);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.OemSemicolon))//ZR Down?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll, Rotation.Yaw - rotatorOFfset);
            return true;
        }
        return false;
    }
    private void SetWeaponAliasAttachment()
    {
        //shovel replacing baseball bat?
        string propName = NativeHelper.GetKeyboardInput("gr_prop_gr_hammer_01");
        string weaponHashText = NativeHelper.GetKeyboardInput("1317494643");
        if (uint.TryParse(weaponHashText, out uint WeaponHash))
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(Game.LocalPlayer.Character, WeaponHash, 200, false, false);
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(Game.LocalPlayer.Character, WeaponHash, true);
            if (Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Exists())
            {
                Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.IsVisible = false;
            }
            //BONETAG_R_PH_HAND new Vector3(-0.03f,-0.2769999f,-0.06200002f),new Rotator(20f, -101f, 81f)
            Rage.Object weaponObject = null;
            try
            {
                weaponObject = new Rage.Object(propName, Player.Character.GetOffsetPositionUp(50f));
                string HandBoneName = "BONETAG_R_PH_HAND";
                Offset = new Vector3(0f, 0f, 0f);
                Rotation = new Rotator(0f, 0f, 0f);
                if (weaponObject.Exists())
                {
                    AttachItem(weaponObject, HandBoneName, new Vector3(0.0f, 0.0f, 0f), new Rotator(0f, 0f, 0f));

                    GameFiber.StartNew(delegate
                    {
                        try
                        {
                            while (!Game.IsKeyDownRightNow(Keys.Space))
                            {
                                uint currentWeapon;
                                NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Game.LocalPlayer.Character, out currentWeapon, true);
                                if (currentWeapon != WeaponHash)
                                {
                                    break;
                                }
                                if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
                                {
                                    AttachItem(weaponObject, HandBoneName, Offset, Rotation);
                                    GameTimeLastAttached = Game.GameTime;
                                }
                                if (Game.IsKeyDown(Keys.B))
                                {
                                    //EntryPoint.WriteToConsoleTestLong($"Item {weaponObject} Attached to  {HandBoneName} new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");
                                    GameFiber.Sleep(500);
                                }
                                if (Game.IsKeyDown(Keys.N))
                                {
                                    isPrecise = !isPrecise;
                                    GameFiber.Sleep(500);
                                }
                                Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
                                Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise}");
                                GameFiber.Yield();
                            }
                            if (weaponObject.Exists())
                            {
                                weaponObject.Delete();
                            }
                            if (Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Exists())
                            {
                                Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                            EntryPoint.ModController.CrashUnload();
                        }
                    }, "Run Debug Logic");
                }
            }
            catch (Exception ex)
            {
                //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
            }
        }
    }
    private void SetParticleAttachment()
    {
        //shovel replacing baseball bat?
        //string propName = NativeHelper.GetKeyboardInput("prop_cigar_02");
        string particleGroupName = NativeHelper.GetKeyboardInput("core");
        string particleName = NativeHelper.GetKeyboardInput("ent_amb_peeing");
        Rage.Object weaponObject = null;
        try
        {
            //weaponObject = new Rage.Object(propName, Player.Character.GetOffsetPositionUp(50f));
            //string HandBoneName = "BONETAG_R_PH_HAND";

            Offset = new Vector3(0.0f, 0.0f, 0.0f);
            Rotation = new Rotator(0f, 0f, 0f);


            Vector3 CoolOffset = new Vector3(0.0f, 0.0f, 0.0f);
            Rotator CoolRotation = new Rotator(0f, 0f, 0f);
            string dictionary = NativeHelper.GetKeyboardInput("missfbi3ig_0");
            string animation = NativeHelper.GetKeyboardInput("shit_loop_trev");
            float scale = 1f;

            AnimationDictionary.RequestAnimationDictionay(dictionary);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 4.0f, -4.0f, -1, (int)(AnimationFlags.Loop), 0, false, false, false);//-1

            bool isRunning = true;

            //AttachItem(weaponObject, HandBoneName, CoolOffset, CoolRotation);
            LoopedParticle particle = new LoopedParticle(particleGroupName, particleName, Player.Character, PedBoneId.Pelvis, new Vector3(0.0f, 0.0f, 0f), Rotator.Zero, scale);
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (!Game.IsKeyDownRightNow(Keys.Space))
                    {
                        if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
                        {
                            particle.Stop();

                            particle = new LoopedParticle(particleGroupName, particleName, Player.Character, PedBoneId.Pelvis, Offset, Rotation, scale);
                            //AttachItem(weaponObject, HandBoneName, Offset, Rotation);
                            GameTimeLastAttached = Game.GameTime;
                        }
                        if (Game.IsKeyDown(Keys.B))
                        {
                            EntryPoint.WriteToConsole($"Attached to  new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");
                            GameFiber.Sleep(500);
                        }
                        if (Game.IsKeyDown(Keys.N))
                        {
                            isPrecise = !isPrecise;
                            GameFiber.Sleep(500);
                        }

                        if (Game.IsKeyDown(Keys.D0))
                        {
                            isRunning = !isRunning;
                            NativeFunction.Natives.SET_ENTITY_ANIM_SPEED(Player.Character, dictionary, animation, isRunning ? 1.0f : 0.0f);
                            GameFiber.Sleep(500);
                        }


                        if (Game.IsKeyDown(Keys.D1))
                        {
                            scale -= 0.1f;
                            particle.Stop();

                            particle = new LoopedParticle(particleGroupName, particleName, Player.Character, PedBoneId.Pelvis, Offset, Rotation, scale);
                            //AttachItem(weaponObject, HandBoneName, Offset, Rotation);
                            GameTimeLastAttached = Game.GameTime;
                            GameFiber.Sleep(100);
                        }
                        if (Game.IsKeyDown(Keys.D2))
                        {
                            scale += 0.1f;
                            particle.Stop();

                            particle = new LoopedParticle(particleGroupName, particleName, Player.Character, PedBoneId.Pelvis, Offset, Rotation, scale);
                            //AttachItem(weaponObject, HandBoneName, Offset, Rotation);
                            GameTimeLastAttached = Game.GameTime;
                            GameFiber.Sleep(100);
                        }
                        if (Game.IsKeyDown(Keys.D3))
                        {

                            string test = NativeHelper.GetKeyboardInput("");
                            Color myCOlor = Color.FromName(test);
                            if(myCOlor != null)
                            {
                                particle.SetColor(myCOlor);
                            }
                            GameFiber.Sleep(100);
                        }





                        Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
                        Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise}~n~Press 0 Pause{isRunning} 1 Scale-, 2 Scale+, 3 COlor");
                        GameFiber.Yield();
                    }
                    if (weaponObject.Exists())
                    {
                        weaponObject.Delete();
                    }
                    if (particle != null)
                    {
                        particle.Stop();
                    }
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "Run Debug Logic");
            
        }
        catch (Exception ex)
        {
            //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
        }

    }

    //private void SetParticleAttachment_Old()
    //{
    //    //shovel replacing baseball bat?
    //    string propName = NativeHelper.GetKeyboardInput("prop_cigar_02");
    //    string particleGroupName = NativeHelper.GetKeyboardInput("core");
    //    string particleName = NativeHelper.GetKeyboardInput("veh_sub_leak");
    //    Rage.Object weaponObject = null;
    //    try
    //    {
    //        weaponObject = new Rage.Object(propName, Player.Character.GetOffsetPositionUp(50f));
    //        string HandBoneName = "BONETAG_R_PH_HAND";

    //        Offset = new Vector3(0.0f, 0.0f, 0.0f);
    //        Rotation = new Rotator(0f, 0f, 0f);


    //        Vector3 CoolOffset = new Vector3(0.0f, 0.0f, 0.0f);
    //        Rotator CoolRotation = new Rotator(0f, 0f, 0f);
    //        if (weaponObject.Exists())
    //        {
    //            string dictionary = "missbigscore1switch_trevor_piss";
    //            string animation = "piss_loop";

    //            AnimationDictionary.RequestAnimationDictionay(dictionary);
    //            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 4.0f, -4.0f, -1, (int)(AnimationFlags.Loop), 0, false, false, false);//-1

    //            bool isRunning = true;

    //            AttachItem(weaponObject, HandBoneName, CoolOffset, CoolRotation);
    //            LoopedParticle particle = new LoopedParticle(particleGroupName, particleName, weaponObject, new Vector3(0.0f, 0.0f, 0f), Rotator.Zero, 1.5f);
    //            GameFiber.StartNew(delegate
    //            {
    //                try
    //                {
    //                    while (!Game.IsKeyDownRightNow(Keys.Space))
    //                    {
    //                        if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
    //                        {
    //                            particle.Stop();
    //                            particle = new LoopedParticle(particleGroupName, particleName, weaponObject, Offset, Rotation, 1.5f);
    //                            //AttachItem(weaponObject, HandBoneName, Offset, Rotation);
    //                            GameTimeLastAttached = Game.GameTime;
    //                        }
    //                        if (Game.IsKeyDown(Keys.B))
    //                        {
    //                            //EntryPoint.WriteToConsoleTestLong($"Item {weaponObject} Attached to  {HandBoneName} new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");
    //                            GameFiber.Sleep(500);
    //                        }
    //                        if (Game.IsKeyDown(Keys.N))
    //                        {
    //                            isPrecise = !isPrecise;
    //                            GameFiber.Sleep(500);
    //                        }

    //                        if (Game.IsKeyDown(Keys.D0))
    //                        {
    //                            isRunning = !isRunning;
    //                            NativeFunction.Natives.SET_ENTITY_ANIM_SPEED(Player.Character, dictionary, animation, isRunning ? 1.0f : 0.0f);
    //                            GameFiber.Sleep(500);
    //                        }

    //                        Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
    //                        Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise}~n~Press 0 Pause{isRunning}");
    //                        GameFiber.Yield();
    //                    }
    //                    if (weaponObject.Exists())
    //                    {
    //                        weaponObject.Delete();
    //                    }
    //                    if (particle != null)
    //                    {
    //                        particle.Stop();
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
    //                    EntryPoint.ModController.CrashUnload();
    //                }
    //            }, "Run Debug Logic");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
    //    }

    //}

    private void PrintVehicleClasses()
    {


        foreach (DispatchableVehicleGroup dvg in ModDataFileManager.DispatchableVehicles.AllVehicles)
        {
            WriteToClassCreator($"new DispatchableVehicleGroup() {{", 0, "DispatchableVehicles");
            PrintClass(dvg, null, "DispatchableVehicles");
            WriteToClassCreator($"}},", 0, "DispatchableVehicles");
        }



        //foreach (Gang gang in Gangs.AllGangs)
        //{
        //    if (gang.Vehicles == null)
        //    {
        //        continue;
        //    }
        //    WriteToClassCreator($"--{gang.ShortName} {gang.VehiclesID}", 0, "DispatchableVehicles");





        //    //foreach (DispatchableVehicle dispatchableVehicle in gang.Vehicles)
        //    //{
        //    //    WriteToClassCreator($"DispatchableVehicle {dispatchableVehicle.DebugName.Replace(" ", String.Empty)} = new DispatchableVehicle() {{", 0, "DispatchableVehicles");
        //    //    PrintClass(dispatchableVehicle, null, "DispatchableVehicles");
        //    //    WriteToClassCreator($"}},", 0, "DispatchableVehicles");
        //    //}
        //}
    }
    private void PrintGangDens()
    {
        foreach (GangDen gangDen in ModDataFileManager.PlacesOfInterest.PossibleLocations.GangDens)
        {
            WriteToClassCreator($"GangDen {gangDen.Name.Replace(" ", String.Empty)} = new GangDen() {{", 0, "GangDens");
            PrintClass(gangDen, null, "GangDens");
            WriteToClassCreator($"}};", 0, "GangDens");
        }
    }
    private void PrintLocationClasses(List<string> AllowedProperties)
    {
        File.WriteAllText(@"Plugins\\LosSantosRED\\Locations.txt", string.Empty);
        int Number = 1;
        foreach (string locationType in ModDataFileManager.PlacesOfInterest.PossibleLocations.InteractableLocations().GroupBy(x => x.GetType().ToString()).Distinct().Select(x => x.Key))
        {
            foreach (GameLocation location2 in ModDataFileManager.PlacesOfInterest.PossibleLocations.InteractableLocations().Where(x => x.GetType().ToString() == locationType))
            {
                string type = location2.GetType().ToString();
                WriteToClassCreator($"new {type}(new Vector3({location2.EntrancePosition.X}f,{location2.EntrancePosition.Y}f,{location2.EntrancePosition.Z}f),{location2.EntranceHeading}f,\"{location2.Name}\",\"{(string.IsNullOrEmpty(location2.Description) ? "" : location2.Description)}\") {{", 0, "Locations");
                PrintClass(location2, AllowedProperties, "Locations");
                WriteToClassCreator($"}},", 0, "Locations");
                Number++;
            }
        }
    }


    private void PrintInteriorClasses(List<string> AllowedProperties)
    {
        File.WriteAllText(@"Plugins\\LosSantosRED\\Interiors.txt", string.Empty);
        int Number = 1;
        foreach (string locationType in ModDataFileManager.Interiors.PossibleInteriors.AllInteriors().GroupBy(x => x.GetType().ToString()).Distinct().Select(x => x.Key))
        {
            foreach (Interior location2 in ModDataFileManager.Interiors.PossibleInteriors.AllInteriors().Where(x => x.GetType().ToString() == locationType))
            {
                string type = location2.GetType().ToString();
                WriteToClassCreator($"new {type}({location2.LocalID},\"{location2.Name}\") {{", 0, "Interiors");
                PrintClass(location2, AllowedProperties, "Interiors");
                WriteToClassCreator($"}},", 0, "Interiors");
                Number++;
            }
        }
    }


    private void PrintClass(object dv, List<string> AllowedProperties, string fileName)
    {
        if (dv == null)
        {
            return;
        }
        PropertyInfo[] properties = dv.GetType().GetProperties();



        object dvBase = Activator.CreateInstance(dv.GetType());

        foreach (PropertyInfo property in properties)
        {

            Type type1 = property.PropertyType;
            Type itemType1 = null;
            if (type1 != null)
            {
                if (type1.GetGenericArguments() != null && type1.GetGenericArguments().Any())
                {
                    itemType1 = type1.GetGenericArguments()[0];
                }
            }
            //EntryPoint.WriteToConsole($"{property.Name} {type1} {property is IEnumerable} {property.IsGenericList()} {property.PropertyType.IsGenericList()} {itemType1?.Name}");

            if (!property.CanWrite)
            {
                //EntryPoint.WriteToConsole($"{property.Name} CANT WRITE");
                continue;
            }
            if (property.GetSetMethod(false) == null)
            {
                //EntryPoint.WriteToConsole($"{property.Name} GETSETMETHOD IS NULL");
                continue;
            }
            if (property.GetCustomAttributes(false).Any(a => a is XmlIgnoreAttribute))
            {
                //EntryPoint.WriteToConsole($"{property.Name} XmlIgnoreAttribute");
                continue;
            }
            else if (AllowedProperties != null && !AllowedProperties.Any(x => x == property.Name))
            {
                continue;
            }

            EntryPoint.WriteToConsole($"{property.Name} {property.PropertyType}");

            if (property.PropertyType == typeof(String) || property.PropertyType == typeof(string) || property.PropertyType == typeof(System.Drawing.Color))
            {
                object main = property.GetValue(dv);
                object secondary = property.GetValue(dvBase);
                if (main == null || secondary == null || !main.Equals(secondary))
                {
                    WriteToClassCreator($"{property.Name} = \"{property.GetValue(dv)}\",", 0, fileName);
                }
            }
            else if (property.PropertyType.IsEnum)
            {
               // var enumValueName = Enum.GetName(property.GetType(), property.GetValue(dv));
                object main = property.GetValue(dv);
                object secondary = property.GetValue(dvBase);
                if (main == null || secondary == null || !main.Equals(secondary))
                {
                    WriteToClassCreator($"{property.Name} = {property.Name}.{property.GetValue(dv)},", 0, fileName);
                }
            }
            else if (property.PropertyType == typeof(float))
            {
                EntryPoint.WriteToConsole($"{property.Name} {property.PropertyType} TEST");
                object main = property.GetValue(dv);
                object secondary = property.GetValue(dvBase);
                if (main == null || secondary == null || !main.Equals(secondary))
                {
                    WriteToClassCreator($"{property.Name} = {property.GetValue(dv)}f,", 0, fileName);
                }
            }
            else if (property.PropertyType == typeof(Vector3))
            {
                object main = property.GetValue(dv);
                object secondary = property.GetValue(dvBase);
                if (main == null || secondary == null || !main.Equals(secondary))
                {
                    Vector3 propertyValue = (Vector3)property.GetValue(dv);
                    WriteToClassCreator($"{property.Name} = new Vector3({propertyValue.X}f,{propertyValue.Y}f,{propertyValue.Z}f),", 0, fileName);
                }
            }
            else if (property.PropertyType == typeof(Rotator))
            {
                object main = property.GetValue(dv);
                object secondary = property.GetValue(dvBase);
                if (main == null || secondary == null || !main.Equals(secondary))
                {
                    Rotator propertyValue = (Rotator)property.GetValue(dv);
                    WriteToClassCreator($"{property.Name} = new Rotator({propertyValue.Pitch}f,{propertyValue.Roll}f,{propertyValue.Yaw}f),", 0, fileName);
                }
            }
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(bool))
            {
                object main = property.GetValue(dv);
                object secondary = property.GetValue(dvBase);
                if (main == null || secondary == null || !main.Equals(secondary))
                {
                    if (property.CanWrite)
                    {
                        WriteToClassCreator($"{property.Name} = {property.GetValue(dv).ToString().ToLower()},", 0, fileName);
                    }
                }
            }
            else if (property.PropertyType == typeof(VehicleVariation) || property.PropertyType == typeof(LicensePlate) || property.PropertyType == typeof(SpawnPlace))
            {
                WriteToClassCreator($"{property.Name} = new {property.PropertyType}() {{", 0, fileName);
                PrintClass(property.GetValue(dv), AllowedProperties, fileName);
                WriteToClassCreator($"}},", 0, fileName);
            }
            //else
            //{
            //    WriteToClassCreator($"{property.Name} = new {property.PropertyType}() {{", 0, fileName);
            //    PrintClass(property.GetValue(dv), AllowedProperties, fileName);
            //    WriteToClassCreator($"}},", 0, fileName);
            //}
            if (itemType1 != null)
            {
                EntryPoint.WriteToConsole($"IS LIST {property.Name} {itemType1.Name}");
                DoListItem(property, dv, $"List<{itemType1.Name}>", AllowedProperties, fileName);
            }
        }
    }
    private void DoListItem(PropertyInfo property, object dv, string ListType, List<string> AllowedProperties, string fileName)
    {
        if(property== null)
        {
            return;
        }
        WriteToClassCreator($"{property.Name} = new {ListType}() {{", 0, fileName);
        var collection = (IEnumerable)property.GetValue(dv, null);
        if(collection== null)
        {
            return;
        }

        foreach (object obj in collection)
        {

            if (obj.GetType() == typeof(string) || obj.GetType() == typeof(String))
            {
                EntryPoint.WriteToConsole($"LIST ITEM IS STRING");
                WriteToClassCreator($@"""{obj}"",", 0, fileName);
            }
            else if (obj.GetType() == typeof(Vector3))
            {
                Vector3 propertyValue = (Vector3)obj;
                EntryPoint.WriteToConsole($"LIST ITEM IS VECTOR3");
                WriteToClassCreator($"new Vector3({propertyValue.X}f,{propertyValue.Y}f,{propertyValue.Z}f),", 0, fileName);
            }
            else
            {
                WriteToClassCreator($"new {obj.GetType()}() {{", 0, fileName);
                PrintClass(obj, null, fileName);
                WriteToClassCreator($"}},", 0, fileName);
            }
        }
        WriteToClassCreator($"}},", 0, fileName);
    }
    private void HighlightProp()
    {
        Entity ClosestEntity = Rage.World.GetClosestEntity(Game.LocalPlayer.Character.GetOffsetPositionFront(2f), 2f, GetEntitiesFlags.ConsiderAllObjects | GetEntitiesFlags.ExcludePlayerPed);
        if (ClosestEntity.Exists())
        {
            Vector3 DesiredPos = ClosestEntity.GetOffsetPositionFront(-0.5f);
            EntryPoint.WriteToConsole($"Closest Object = {ClosestEntity.Model.Name} {ClosestEntity.Model.Hash}", 5);
            EntryPoint.WriteToConsole($"Closest Object Dimensions X {ClosestEntity.Model.Dimensions.X} Y {ClosestEntity.Model.Dimensions.Y} Z {ClosestEntity.Model.Dimensions.Z}", 5);

            EntryPoint.WriteToConsole($"Closest: {ClosestEntity.Model.Hash},new Vector3({ClosestEntity.Position.X}f, {ClosestEntity.Position.Y}f, {ClosestEntity.Position.Z}f) //{ClosestEntity.Heading}f", 5);

            uint GameTimeStartedDisplaying = Game.GameTime;
            while (Game.GameTime - GameTimeStartedDisplaying <= 2000)
            {
                Rage.Debug.DrawArrowDebug(DesiredPos + new Vector3(0f, 0f, 0.5f), Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Yellow);
                GameFiber.Yield();
            }

        }
    }


    private void PropPosition()
    {
        foreach(Rage.Object ClosestEntity in Rage.World.GetAllObjects())
        {
            if (ClosestEntity.Exists() && ClosestEntity.DistanceTo2D(Game.LocalPlayer.Character) <= 10.0f)
            {
  
                EntryPoint.WriteToConsole($"Closest:{ClosestEntity.Model.Name} {ClosestEntity.Model.Hash},new Vector3({ClosestEntity.Position.X}f, {ClosestEntity.Position.Y}f, {ClosestEntity.Position.Z}f), {ClosestEntity.Heading}f", 5);



            }
        }


        //Entity ClosestEntity = Rage.World.GetClosestEntity(Game.LocalPlayer.Character.GetOffsetPositionFront(2f), 2f, GetEntitiesFlags.ConsiderAllObjects | GetEntitiesFlags.ExcludePlayerPed);
        //if (ClosestEntity.Exists())
        //{
        //    Vector3 DesiredPos = ClosestEntity.GetOffsetPositionFront(-0.5f);
        //    EntryPoint.WriteToConsole($"Closest Object = {ClosestEntity.Model.Name} {ClosestEntity.Model.Hash}", 5);
        //    EntryPoint.WriteToConsole($"Closest Object Dimensions X {ClosestEntity.Model.Dimensions.X} Y {ClosestEntity.Model.Dimensions.Y} Z {ClosestEntity.Model.Dimensions.Z}", 5);

        //    EntryPoint.WriteToConsole($"Closest: {ClosestEntity.Model.Hash},new Vector3({ClosestEntity.Position.X}f, {ClosestEntity.Position.Y}f, {ClosestEntity.Position.Z}f) //{ClosestEntity.Heading}f", 5);

        //    uint GameTimeStartedDisplaying = Game.GameTime;
        //    while (Game.GameTime - GameTimeStartedDisplaying <= 2000)
        //    {
        //        Rage.Debug.DrawArrowDebug(DesiredPos + new Vector3(0f, 0f, 0.5f), Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Yellow);
        //        GameFiber.Yield();
        //    }

        //}
    }



    private void PositionSetter()
    {

        //new Vector3(-282.1472f, 6227.552f, 31.88528f)


    }

}

