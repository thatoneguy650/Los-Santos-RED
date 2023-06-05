using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GangWheelmanTask
    {
        private ITaskAssignable Player;
        private ITimeControllable Time;
        private IGangs Gangs;
        private PlayerTasks PlayerTasks;
        private IPlacesOfInterest PlacesOfInterest;
        private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
        private ISettingsProvideable Settings;
        private IEntityProvideable World;
        private ICrimes Crimes;
        private IModItems ModItems;

        private IWeapons Weapons;
        private INameProvideable Names;
        private IPedGroups PedGroups;
        private IShopMenus ShopMenus;
        private Gang HiringGang;
        private GangDen HiringGangDen;
        private GameLocation RobberyLocation;
        private PlayerTask CurrentTask;
        private int GameTimeToWaitBeforeComplications;
        private int MoneyToRecieve;
        private bool HasAddedComplications;
        private bool WillAddComplications;

        private int HoursToRobbery;
        private DateTime RobberyTime;
        private bool hasStartedGetaway;
        private bool hasSpawnedRobbers;
        private bool hasStartedRobbery;
        private bool hasSentCompleteMessage;
        private int PlayerGroup;
        private RelationshipGroup RobberRelationshipGroup;
        private bool isFadedOut;


        private List<GangMember> SpawnedRobbers = new List<GangMember>();
        private int RobbersToSpawn;
        private uint GameTimeRobberLastSpoke;
        private uint GameTimeBetweenRobberSpeech;
        private bool hasSetRobbersViolent;
        private bool hasAddedArmedRobberyCrime;
        private Camera CutsceneCamera;
        private Vector3 EgressCamPosition;
        private float EgressCamFOV;
        private bool hasAddedButtonPrompt;
        private GangContact Contact;

        private string ButtonPromptIdentifier => "RobberyStart" + RobberyLocation?.Name + HiringGang?.ID;
        private bool HasLocations => RobberyLocation != null && HiringGangDen != null;
        public GangWheelmanTask(ITaskAssignable player, ITimeControllable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, IWeapons weapons, INameProvideable names, IPedGroups pedGroups, IShopMenus shopMenus, IModItems modItems)
        {
            Player = player;
            Time = time;
            Gangs = gangs;
            PlayerTasks = playerTasks;
            PlacesOfInterest = placesOfInterest;
            ActiveDrops = activeDrops;
            Settings = settings;
            World = world;
            Crimes = crimes;
            Weapons = weapons;
            Names = names;
            PedGroups = pedGroups;
            ShopMenus = shopMenus;
            ModItems = modItems;
        }
        public void Setup()
        {
            
        }
        public void Dispose()
        {
            if(RobberyLocation != null)
            {
                RobberyLocation.IsPlayerInterestedInLocation = false;
            }
            Player.ButtonPrompts.RemovePrompts("RobberyStart");
            CleanupRobbers();
        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            Contact = new GangContact(HiringGang.ContactName, HiringGang.ContactIcon);
            if (PlayerTasks.CanStartNewTask(ActiveGang?.ContactName))
            {
                GetRobberyInformation();
                if (HasLocations)
                {
                    GetPayment();
                    SendInitialInstructionsMessage();
                    AddTask();
                    GameFiber PayoffFiber = GameFiber.StartNew(delegate
                    {
                        try
                        {
                            Loop();
                            FinishTask();
                        }
                        catch (Exception ex)
                        {
                            EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                            EntryPoint.ModController.CrashUnload();
                        }
                    }, "PayoffFiber");
                }
                else
                {
                    SendTaskAbortMessage();
                }
            }
        }
        private void Loop()
        {
            while (true)
            {
                CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
                if (CurrentTask == null || !CurrentTask.IsActive)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Task Inactive for {HiringGang.ContactName}");
                    break;
                }
                if(!hasStartedRobbery)
                {
                    PreSpawnLoop();
                }
                else
                {
                    if(!IsRobberyValid())
                    {
                        break;
                    }
                    PostSpawnLoop();
                    CheckRobberyReadyForPayment();
                }
                GameFiber.Sleep(250);
            }
        }
        private void PostSpawnLoop()
        {
            if (Player.AnyPoliceCanSeePlayer && (Player.WantedLevel <= 3 || !Player.PoliceResponse.IsDeadlyChase))
            {
                CheckRobberCrimes();
            }
            RobberSpeech();
            if (Player.IsWanted && Player.WantedLevel >= 2 && !hasSetRobbersViolent && SpawnedRobbers.Any(x=> x.WantedLevel >= 2 || x.IsDeadlyChase))
            {
                SetRobbersViolent();
            }
            if(Player.IsWanted && !hasAddedArmedRobberyCrime)
            {
                Player.AddCrime(Crimes.CrimeList?.FirstOrDefault(x => x.ID == "ResistingArrest"), true, Player.Character.Position, Player.CurrentVehicle, Player.WeaponEquipment.CurrentWeapon, true, true, true);
                hasAddedArmedRobberyCrime = true;
            }
        }
        private void PreSpawnLoop()
        {
            float distanceTo = Player.Character.DistanceTo2D(RobberyLocation.EntrancePosition);
            if (DateTime.Compare(Time.CurrentDateTime, RobberyTime) >= 0)
            {
                hasStartedRobbery = true;
                if (distanceTo <= 50f)
                {
                    if(!isFadedOut)
                    {
                        Game.FadeScreenOut(500);
                    }
                    //Game.FadeScreenOut(1500, true);


                    World.Pedestrians.ClearGangMembers();

                    hasSpawnedRobbers = SpawnRobbers();
                   // GameFiber.Sleep(100);
                    Game.FadeScreenIn(1000);
                    //Game.FadeScreenIn(1500, true);
                    PlayScene();
                    if(hasSpawnedRobbers)
                    {
                        Player.AddCrime(Crimes.CrimeList?.FirstOrDefault(x => x.ID == "ArmedRobbery"), false, Player.Character.Position, Player.CurrentVehicle, null, true, true, true);
                    }
                }
                Player.ButtonPrompts.RemovePrompts("RobberyStart");
            }
            else 
            {
                if (distanceTo <= 35f && Player.Character.Speed <= 0.25f && !Time.IsFastForwarding)
                {
                    hasAddedButtonPrompt = true;
                    Player.ButtonPrompts.AddPrompt("RobberyStart", "Hold To Start Robbery", ButtonPromptIdentifier, Settings.SettingsManager.KeySettings.InteractCancel, 99);
                    if (Player.ButtonPrompts.IsHeld(ButtonPromptIdentifier))
                    {
                        //EntryPoint.WriteToConsoleTestLong("RobberyStart You pressed fastforward to time");
                        Player.ButtonPrompts.RemovePrompts("RobberyStart");
                        Time.FastForward(RobberyTime);
                        Game.FadeScreenOut(1000, true);
                        Time.SetDateTime(RobberyTime);
                        isFadedOut = true;
                    }
                }
                else
                {
                    if (hasAddedButtonPrompt)
                    {
                        Player.ButtonPrompts.RemovePrompts("RobberyStart");
                        hasAddedButtonPrompt = false;
                    }
                }          
            }
        }
        private void PlayScene()
        {

            GangMember Star = SpawnedRobbers.PickRandom();
            if (Star != null && Star.Pedestrian.Exists())
            {
                NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Star.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
            }


            //if (!CutsceneCamera.Exists())
            //{
            //    CutsceneCamera = new Camera(false);
            //}
            //if (1 == 1)//far away camera
            //{
            //    float distanceAway = 10f;
            //    float distanceAbove = 7f;
            //    Vector3 InitialCameraPosition = NativeHelper.GetOffsetPosition(RobberyLocation.EntrancePosition, RobberyLocation.EntranceHeading + 90f, distanceAway);
            //    InitialCameraPosition = new Vector3(InitialCameraPosition.X, InitialCameraPosition.Y, InitialCameraPosition.Z + distanceAbove);
            //    CutsceneCamera.Position = InitialCameraPosition;
            //    Vector3 ToLookAt = new Vector3(RobberyLocation.EntrancePosition.X, RobberyLocation.EntrancePosition.Y, RobberyLocation.EntrancePosition.Z + 2f);
            //    Vector3 _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
            //    CutsceneCamera.Direction = _direction;

            //}
            //else //close door camera
            //{
            //    Vector3 ToLookAtPos = NativeHelper.GetOffsetPosition(RobberyLocation.EntrancePosition, RobberyLocation.EntranceHeading + 90f, 5f);
            //    EgressCamPosition = NativeHelper.GetOffsetPosition(ToLookAtPos, RobberyLocation.EntranceHeading, 7f);
            //    EgressCamPosition += new Vector3(0f, 0f, 0.4f);
            //    ToLookAtPos += new Vector3(0f, 0f, 0.4f);
            //    CutsceneCamera.Position = EgressCamPosition;
            //    CutsceneCamera.FOV = 55f;
            //    Vector3 _direction = (ToLookAtPos - EgressCamPosition).ToNormalized();
            //    CutsceneCamera.Direction = _direction;
            //}

            //CutsceneCamera.Active = true;
            //hasSpawnedRobbers = SpawnRobbers();
            foreach (GangMember gangMember in SpawnedRobbers)
            {
                if(gangMember.Pedestrian.Exists())
                {
                    uint bestWeapon = NativeFunction.Natives.GET_BEST_PED_WEAPON<uint>(gangMember.Pedestrian);
                    uint currentWeapon;
                    NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(gangMember.Pedestrian, out currentWeapon, true);

                    //EntryPoint.WriteToConsoleTestLong($"PLAY SCENE currentWeapon {currentWeapon}");

                    if (currentWeapon != bestWeapon)
                    {
                        NativeFunction.Natives.SET_CURRENT_PED_WEAPON(gangMember.Pedestrian, bestWeapon, true);
                       // NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(gangMember.Pedestrian, false);
                        //EntryPoint.WriteToConsoleTestLong($"PLAY SCENE SETTING TO bestWeapon {bestWeapon} currentWeapon {currentWeapon}");
                    }
                }
            }
            //GangMember Star = SpawnedRobbers.PickRandom();
            if(Star != null && Star.Pedestrian.Exists())
            {
                uint currentWeapon;
                NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Star.Pedestrian, out currentWeapon, true);
                if (currentWeapon != 2725352035)
                {
                    //EntryPoint.WriteToConsoleTestLong($"PLAY SCENE STARTING currentWeapon {currentWeapon}");
                    PlaySpeech(Star, "COVER_ME", false);
                    NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Star.Pedestrian, RobberyLocation.EntrancePosition.X, RobberyLocation.EntrancePosition.Y, RobberyLocation.EntrancePosition.Z + 0.5f, true);
                    if (RandomItems.RandomPercent(50))
                    {
                        GameFiber.Sleep(500);
                        if (Star.Pedestrian.Exists())
                        {
                            NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Star.Pedestrian, RobberyLocation.EntrancePosition.X, RobberyLocation.EntrancePosition.Y, RobberyLocation.EntrancePosition.Z + 0.5f, true);
                        }
                    }
                    if (RandomItems.RandomPercent(50))
                    {
                        GameFiber.Sleep(500);
                        if (Star.Pedestrian.Exists())
                        {
                            NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Star.Pedestrian, RobberyLocation.EntrancePosition.X, RobberyLocation.EntrancePosition.Y, RobberyLocation.EntrancePosition.Z + 0.5f, true);
                        }
                    }
                }
            }
            GameFiber.Sleep(2000);
            NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
            //if (CutsceneCamera.Exists())
            //{
            //    CutsceneCamera.Delete();
            //}
        }
        private bool IsRobberyValid()
        {
            Player.ButtonPrompts.RemovePrompts("RobberyStart");
            if (!hasSpawnedRobbers)
            {
                //EntryPoint.WriteToConsoleTestLong($"FAILED!  as you werent close enough of the robbers didnt spawn!");
                return false;
            }
            if (hasSpawnedRobbers && !AreRobbersNormal())
            {
                //EntryPoint.WriteToConsoleTestLong($"FAILED! ROBBER ISSUES!");
                return false;
            }
            //if(!Player.IsAliveAndFree)//should handle on the respawn event, want to allow undie,,,,
            //{
            //    EntryPoint.WriteToConsole($"FAILED! You got busted or died!");
            //    return false;
            //}
            return true;
        }
        private void CheckRobberyReadyForPayment()
        {
            if (Player.IsNotWanted && !Player.Investigation.IsActive && !CurrentTask.IsReadyForPayment)
            {
                CurrentTask.OnReadyForPayment(true);
                if (!hasSentCompleteMessage)
                {
                    SendMoneyPickupMessage();
                    hasSentCompleteMessage = true;
                }
                //EntryPoint.WriteToConsoleTestLong($"You lost the cops so it is now ready for payment!");
            }
        }
        private void SetRobbersViolent()
        {
            foreach (GangMember RobberAccomplice in SpawnedRobbers)
            {
                //RobberAccomplice.Pedestrian.RelationshipGroup = RobberRelationshipGroup;
                RelationshipGroup.Cop.SetRelationshipWith(RobberRelationshipGroup, Relationship.Hate);
                RobberRelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
                NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(RobberAccomplice.Pedestrian, 500000, 0);//TR
            }
            hasSetRobbersViolent = true;
        }
        private void RobberSpeech()
        {
            if(Game.GameTime - GameTimeRobberLastSpoke >= GameTimeBetweenRobberSpeech)
            {
                GangMember tospeak = SpawnedRobbers.PickRandom();
                if(tospeak != null && tospeak.Pedestrian.Exists())
                {
                    if(Player.IsWanted && Player.RecentlyShot && Player.AnyPoliceRecentlySeenPlayer)
                    {
                        List<string> PossibleSpeech = new List<string>() { "COVER_ME","RELOADING","TAKE_COVER","PINNED_DOWN", "GENERIC_FRIGHTENED_MED", "GENERIC_FRIGHTENED_HIGH" };
                        PlaySpeech(tospeak, PossibleSpeech.PickRandom(), false);
                        GameTimeBetweenRobberSpeech = RandomItems.GetRandomNumber(2000, 5000);


                        //EntryPoint.WriteToConsoleTestLong($"WHEELMAN SPEECH CHECK DEADLY SHOOTING!");
                    }
                    else if (Player.IsWanted && tospeak.IsWanted && Player.AnyPoliceRecentlySeenPlayer)
                    {
                        List<string> PossibleSpeech = new List<string>() { "GENERIC_WAR_CRY","SHOUT_INSULT", "CHALLENGE_THREATEN", "FIGHT", "GENERIC_CURSE_HIGH" };
                        PlaySpeech(tospeak, PossibleSpeech.PickRandom(), false);
                        GameTimeBetweenRobberSpeech = RandomItems.GetRandomNumber(2000, 5000);


                        //EntryPoint.WriteToConsoleTestLong($"WHEELMAN SPEECH CHECK WANTED!");

                    }
                    else if(Player.IsNotWanted && tospeak.IsNotWanted)
                    {
                        List<string> PossibleSpeech = new List<string>() { "CHAT_STATE", "PED_RANT", "CHAT_RESP", "PED_RANT_RESP", "CULT_TALK","CHAT_RESP",
                //"PHONE_CONV1_CHAT1", "PHONE_CONV1_CHAT2", "PHONE_CONV1_CHAT3", "PHONE_CONV1_INTRO", "PHONE_CONV1_OUTRO",
                //"PHONE_CONV2_CHAT1", "PHONE_CONV2_CHAT2", "PHONE_CONV2_CHAT3", "PHONE_CONV2_INTRO", "PHONE_CONV2_OUTRO",
                //"PHONE_CONV3_CHAT1", "PHONE_CONV3_CHAT2", "PHONE_CONV3_CHAT3", "PHONE_CONV3_INTRO", "PHONE_CONV3_OUTRO",
                //"PHONE_CONV4_CHAT1", "PHONE_CONV4_CHAT2", "PHONE_CONV4_CHAT3", "PHONE_CONV4_INTRO", "PHONE_CONV4_OUTRO",
                //"PHONE_SURPRISE_PLAYER_APPEARANCE_01","SEE_WEIRDO_PHONE",
                "PED_RANT_01", };
                        PlaySpeech(tospeak, PossibleSpeech.PickRandom(), false);
                        GameTimeBetweenRobberSpeech = RandomItems.GetRandomNumber(5000, 15000);


                        //EntryPoint.WriteToConsoleTestLong($"WHEELMAN SPEECH CHECK IDLE!");

                    }
                    else
                    {
                        GameTimeBetweenRobberSpeech = RandomItems.GetRandomNumber(5000, 15000);
                    }
                }
                GameTimeBetweenRobberSpeech = RandomItems.GetRandomNumber(5000, 15000);
                GameTimeRobberLastSpoke = Game.GameTime;
                //EntryPoint.WriteToConsoleTestLong($"WHEELMAN SPEECH CHECK NO SPEECH!");
            }
        }
        private void PlaySpeech(GangMember gangMember, string speechName, bool useMegaphone)
        {
            if (gangMember.VoiceName != "")// isFreeMode)
            {
                if (useMegaphone)
                {
                    gangMember.Pedestrian.PlayAmbientSpeech(gangMember.VoiceName, speechName, 0, SpeechModifier.ForceMegaphone);

                }
                else
                {
                    gangMember.Pedestrian.PlayAmbientSpeech(gangMember.VoiceName, speechName, 0, SpeechModifier.Force);
                }
                //EntryPoint.WriteToConsoleTestLong($"FREEMODE GANG SPEAK {gangMember.Pedestrian.Handle} freeModeVoice {gangMember.VoiceName} speechName {speechName}");
            }
            else
            {
                gangMember.Pedestrian.PlayAmbientSpeech(speechName, useMegaphone);
                //EntryPoint.WriteToConsoleTestLong($"REGULAR GANG SPEAK {gangMember.Pedestrian.Handle} freeModeVoice {gangMember.VoiceName} speechName {speechName}");
            }
        }
        private void CheckRobberCrimes()
        {
            foreach (GangMember gm in SpawnedRobbers)
            {
                if (gm.Pedestrian.Exists() && gm.DistanceToPlayer <= 20f)
                {
                    if(gm.WantedLevel > Player.WantedLevel && gm.PedViolations.WorstObservedCrime != null)
                    {
                        //EntryPoint.WriteToConsoleTestLong($"WANTED LEVEL Adding Crime {gm.PedViolations.WorstObservedCrime.Name}!");
                        Player.AddCrime(gm.PedViolations.WorstObservedCrime, true, Player.Character.Position, Player.CurrentVehicle, null, true, true, true);
                    }
                    else if(gm.IsDeadlyChase && !Player.PoliceResponse.IsDeadlyChase && gm.PedViolations.WorstObservedCrime != null)
                    {
                        //EntryPoint.WriteToConsoleTestLong($"DEADLY CHASE Adding Crime {gm.PedViolations.WorstObservedCrime.Name}!");
                        Player.AddCrime(gm.PedViolations.WorstObservedCrime, true, Player.Character.Position, Player.CurrentVehicle, Player.WeaponEquipment.CurrentWeapon, true, true, true);
                    }
                    else if(gm.PedViolations.WorstObservedCrime == null)
                    {
                        //EntryPoint.WriteToConsoleTestLong($"WHEELMAN NO CRIMES!!!!");
                    }
                }
            }
            //EntryPoint.WriteToConsole($"WHEELMAN CHECK ROBBER CRIMES RAN!!!!");
        }
        private bool AreRobbersNormal()
        {
            foreach(GangMember gm in SpawnedRobbers)
            {
                if(!gm.Pedestrian.Exists())
                {
                    //EntryPoint.WriteToConsoleTestLong($"FAILED!  A robber got despawned soemhow!");
                    return false;
                }
                else if(gm.IsBusted)
                {
                    //EntryPoint.WriteToConsoleTestLong($"FAILED!  The robber got caught!");
                    return false;
                }
                else if (gm.Pedestrian.IsDead)
                {
                    //EntryPoint.WriteToConsoleTestLong($"FAILED!  A robber died!");
                    return false;
                }
                else if (gm.Pedestrian.DistanceTo2D(Player.Character) >= 250f)
                {
                    //EntryPoint.WriteToConsoleTestLong($"FAILED!  A robber got left!");
                    return false;
                }
            }
            return true;
        }
        private bool SpawnRobbers()
        {
            bool spawnedOneRobber = false;
            RobberRelationshipGroup = new RelationshipGroup("ROBBERS");
            RelationshipGroup.Cop.SetRelationshipWith(RobberRelationshipGroup, Relationship.Neutral);
            RobberRelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Neutral);
            for (int i = 0; i < RobbersToSpawn; i++)
            {
                if(SpawnRobber(i+2f))
                {
                    spawnedOneRobber = true;
                }
            }
            return spawnedOneRobber;
        }
        private bool SpawnRobber(float offset)
        {
            if (RobberyLocation.EntrancePosition != Vector3.Zero)
            {
                DispatchablePerson RobberAccompliceInfo = HiringGang.Personnel.Where(x => x.CanCurrentlySpawn(0)).PickRandom();
                if (RobberAccompliceInfo != null)
                {
                    Vector3 ToSpawn = NativeHelper.GetOffsetPosition(RobberyLocation.EntrancePosition, RobberyLocation.EntranceHeading + 90f, offset);
                    SpawnLocation toSpawn = new SpawnLocation(ToSpawn);
                    GangSpawnTask gmSpawn = new GangSpawnTask(HiringGang, toSpawn, null, RobberAccompliceInfo, Settings.SettingsManager.GangSettings.ShowSpawnedBlip, Settings, Weapons, Names, false, Crimes, PedGroups, ShopMenus, World, ModItems, false, true, false);
                    gmSpawn.PlacePedOnGround = true;
                    gmSpawn.AllowAnySpawn = true;
                    gmSpawn.AllowBuddySpawn = false;
                    gmSpawn.AttemptSpawn();
                    GangMember RobberAccomplice = (GangMember)gmSpawn.CreatedPeople.FirstOrDefault();
                    if(RobberAccomplice != null && RobberAccomplice.Pedestrian.Exists())
                    {
                        SpawnedRobbers.Add(RobberAccomplice);
                        RobberAccomplice.Pedestrian.IsPersistent = true;
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(RobberAccomplice.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(RobberAccomplice.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
                        NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(RobberAccomplice.Pedestrian, 0, false);
                        NativeFunction.Natives.SET_PED_ALERTNESS(RobberAccomplice.Pedestrian, 3);
                        NativeFunction.Natives.SET_PED_USING_ACTION_MODE(RobberAccomplice.Pedestrian, true, -1, "DEFAULT_ACTION");
                        RobberAccomplice.WeaponInventory.IssueWeapons(Weapons, true, true, true, RobberAccompliceInfo);
                        RobberAccomplice.CanBeTasked = false;
                        RobberAccomplice.CanBeAmbientTasked = false;


                        RobberAccomplice.Money = RandomItems.GetRandomNumberInt(2000, 5000);

                        //if(WillAddComplications)
                        //{
                            RobberAccomplice.Pedestrian.RelationshipGroup = RobberRelationshipGroup;
                        //    RelationshipGroup.Cop.SetRelationshipWith(RobberRelationshipGroup, Relationship.Hate);
                        //    RobberRelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
                            NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(RobberAccomplice.Pedestrian, 500000, 0);//TR
                        //}
                        RobberAccomplice.Pedestrian.RelationshipGroup = RobberRelationshipGroup;
                        NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(RobberAccomplice.Pedestrian, 500000, 0);//TR

                        Player.GroupManager.Add(RobberAccomplice);




                        //PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
                        //NativeFunction.Natives.SET_PED_AS_GROUP_MEMBER(RobberAccomplice.Pedestrian, PlayerGroup);
                        //NativeFunction.Natives.SET_PED_AS_GROUP_LEADER(Player.Character, PlayerGroup);
                        RobberAccomplice.Pedestrian.KeepTasks = true;
                        return true;
                    }
                } 
            }
            return false;
        }
        private void SetFailed()
        {
            //EntryPoint.WriteToConsoleTestLong("Gang Wheelman FAILED");
            //CleanupRobbers();
            if (RobberyLocation != null)
            {
                RobberyLocation.IsPlayerInterestedInLocation = false;
            }
            foreach (GangMember gm in SpawnedRobbers)
            {
                if(gm.IsBusted)
                {
                    gm.CanBeTasked = true;
                    gm.CanBeAmbientTasked = true;
                }
            }


            SendFailMessage();
            PlayerTasks.FailTask(HiringGang.ContactName);
        }
        private void SetCompleted()
        {
            if (RobberyLocation != null)
            {
                RobberyLocation.IsPlayerInterestedInLocation = false;
            }
            //EntryPoint.WriteToConsoleTestLong("Gang Wheelman COMPLETED");
            CleanupRobbers();
            //GameFiber.Sleep(RandomItems.GetRandomNumberInt(5000, 15000));
            //SendMoneyPickupMessage();
        }
        private void CleanupRobbers()
        {
            foreach (GangMember RobberAccomplice in SpawnedRobbers)
            {
                if (RobberAccomplice != null && RobberAccomplice.Pedestrian.Exists())
                {
                    //RobberAccomplice.Pedestrian.IsPersistent = false;
                    Blip attachedBlip = RobberAccomplice.Pedestrian.GetAttachedBlip();
                    if (attachedBlip.Exists())
                    {
                        attachedBlip.Delete();
                    }
                    RobberAccomplice.ResetCrimes();
                    RobberAccomplice.ResetPlayerCrimes();
                    RobberAccomplice.CanBeTasked = true;
                    RobberAccomplice.CanBeAmbientTasked = true;
                    Player.GroupManager.Remove(RobberAccomplice);
                }
            }
        }
        private void FinishTask()
        {
            Player.ButtonPrompts.RemovePrompts("RobberyStart");
            if (CurrentTask != null && CurrentTask.WasCompleted)
            {
                SetCompleted();
            }
            if (CurrentTask != null && CurrentTask.IsActive)
            {
                SetFailed();
            }
            else
            {
                Dispose();
            }
        }
        private void GetRobberyInformation()
        {
            List<GameLocation> PossibleSpots = new List<GameLocation>();
            //PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.Banks);
            //PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.BeautyShops);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.ConvenienceStores);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.Dispensaries);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.GasStations);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.HardwareStores);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.HeadShops);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.LiquorStores);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.PawnShops);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.Pharmacies);
            //PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.Restaurants);
            //RobberyLocation = PossibleSpots.Where(x=> x..PickRandom();

            List<GameLocation> AvailableSpots = new List<GameLocation>();

            foreach (GameLocation possibleSpot in PossibleSpots)
            {
                bool isNear = false;



                foreach(GameLocation policeStation in PlacesOfInterest.PossibleLocations.PoliceStations)//do not want to do robberies outside the police stations.....
                {
                    if(possibleSpot.CheckIsNearby(policeStation.CellX,policeStation.CellY,2))
                    {
                        isNear = true;
                        break;
                    }
                }



                if(!isNear)
                {
                    AvailableSpots.Add(possibleSpot);
                }



            }
            //   RobberyLocation = PlacesOfInterest.PossibleLocations.ConvenienceStores.FirstOrDefault(x => x.Name == "24/7 Chumash");
            RobberyLocation = AvailableSpots.PickRandom();


            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded);
            HoursToRobbery = RandomItems.GetRandomNumberInt(8, 12);
            RobberyTime = Time.CurrentDateTime.AddHours(HoursToRobbery);


//#if DEBUG
//            RobbersToSpawn = RandomItems.GetRandomNumberInt(2, 3);
//#else
            RobbersToSpawn = RandomItems.GetRandomNumberInt(1, 3);
//#endif
        }
        private void GetPayment()
        {
            MoneyToRecieve = RandomItems.GetRandomNumberInt(HiringGang.WheelmanPaymentMin, HiringGang.WheelmanPaymentMax).Round(500);
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
        }
        private void AddTask()
        {
            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;
            WillAddComplications = false;// RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.RivalGangHitComplicationsPercentage);

            hasStartedGetaway = false;
            hasSpawnedRobbers = false;
            hasSentCompleteMessage = false;
            hasStartedRobbery = false;
            hasSetRobbersViolent = false;
            hasAddedArmedRobberyCrime = false;
            isFadedOut = false;
            SpawnedRobbers.Clear();


            if(RobberyLocation != null)
            {
                RobberyLocation.IsPlayerInterestedInLocation = true;
            }

            //EntryPoint.WriteToConsoleTestLong($"You are hired to wheelman!");
            PlayerTasks.AddTask(HiringGang.ContactName, MoneyToRecieve, 2000, 2000, -500, 7, "Gang Wheelman");
            CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
            CurrentTask.FailOnStandardRespawn = true;
        }
        private void SendInitialInstructionsMessage()
        {
            string NumberToSpawnString = "";
            if(RobbersToSpawn == 1)
            {
                NumberToSpawnString = $"Be sure to have room for my guy";
            }
            else
            {
                NumberToSpawnString = $"The car needs room for {RobbersToSpawn} guys";
            }
            
            List<string> Replies = new List<string>() {
                $"We need a wheelman for a score that is going down. Location is the {RobberyLocation.Name} {RobberyLocation.FullStreetAddress} in {HoursToRobbery} hours. {NumberToSpawnString}. Once you are done come back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. ${MoneyToRecieve} to you",
                $"Get a car and head to the {RobberyLocation.Name} {RobberyLocation.FullStreetAddress}. It will go down in {HoursToRobbery} hours. {NumberToSpawnString}. When you are finished, get back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. I'll have ${MoneyToRecieve} waiting for you.",
                $"We need a driver for a job that we got planned. Get to the {RobberyLocation.Name} {RobberyLocation.FullStreetAddress}. Scheduled for {HoursToRobbery} hours. {NumberToSpawnString}. Afterwards, come back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ${MoneyToRecieve}",
                $"Sombody said you can drive, we'll see. Need you at the {RobberyLocation.Name} {RobberyLocation.FullStreetAddress}. We get going in {HoursToRobbery} hours. {NumberToSpawnString}. When you are finished, get back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ${MoneyToRecieve}",
                $"Need someone good behind the wheel. Location is {RobberyLocation.Name} {RobberyLocation.FullStreetAddress}. The fun starts in {HoursToRobbery} hours. {NumberToSpawnString}. Afterwards, come back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ${MoneyToRecieve}",
                $"Need a quick taxi service for a job. Get your ass to the {RobberyLocation.Name} {RobberyLocation.FullStreetAddress}. You have {HoursToRobbery} hours before the job. {NumberToSpawnString}. When you are finished, get back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ${MoneyToRecieve}",


            };
            Player.CellPhone.AddPhoneResponse(HiringGang.ContactName, HiringGang.ContactIcon, Replies.PickRandom());
        }
        private void SendMoneyPickupMessage()
        {
            List<string> Replies = new List<string>() {
                                $"Seems like that thing we discussed is done? Come by the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} to collect the ${MoneyToRecieve}",
                                $"Word got around that you are done with that thing for us, Come back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ${MoneyToRecieve}",
                                $"Get back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ${MoneyToRecieve}",
                                $"{HiringGangDen.FullStreetAddress} for ${MoneyToRecieve}",
                                $"Heard you were done, see you at the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. We owe you ${MoneyToRecieve}",
                                };
            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 1);
        }
        private void SendFailMessage()
        {
            List<string> Replies = new List<string>() {
                        $"You fucked that up pretty bad.",
                        $"Do you enjoy pissing me off? The whole job is ruined.",
                        $"You completely fucked up the job",
                        $"The job is fucked.",
                        $"How did you fuck this up so badly?",
                        $"You just cost me a lot with this fuckup.",
                        };
            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 1);
        }
        private void SendTaskAbortMessage()
        {
            List<string> Replies = new List<string>() {
                    "Nothing yet, I'll let you know",
                    "I've got nothing for you yet",
                    "Give me a few days",
                    "Not a lot to be done right now",
                    "We will let you know when you can do something for us",
                    "Check back later.",
                    };
            Player.CellPhone.AddPhoneResponse(HiringGang.ContactName, Replies.PickRandom());
        }
    }
}
