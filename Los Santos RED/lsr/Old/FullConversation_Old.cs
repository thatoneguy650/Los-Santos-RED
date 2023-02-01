//using LosSantosRED.lsr.Interface;
//using Rage;
//using Rage.Native;
//using RAGENativeUI;
//using RAGENativeUI.Elements;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Forms;

//public class FullConversation : Interaction, IAdvancedConversationable
//{
//    private uint GameTimeStartedConversing;
//    private bool IsActivelyConversing;
//    private bool IsTasked;
//    private bool IsBlockedEvents;
//    private PedExt Ped;
//    private IInteractionable Player;
//    private bool CancelledConversation;
//    private bool IsDisposed;
//    private ISettingsProvideable Settings;
//    private ICrimes Crimes;
//    private dynamic pedHeadshotHandle;
//    private AdvancedConversation AdvancedConversation;
//    private IModItems ModItems;
//    private IZones Zones;
//    private IShopMenus ShopMenus;
//    private IPlacesOfInterest PlacesOfInterest;
//    private IGangs Gangs;
//    private IGangTerritories GangTerritories;
//    public FullConversation(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems, IZones zones, IShopMenus shopMenus, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories)
//    {
//        Player = player;
//        Ped = ped;
//        Settings = settings;
//        Crimes = crimes;
//        ModItems = modItems;
//        Zones = zones;
//        ShopMenus = shopMenus;
//        PlacesOfInterest = placesOfInterest;
//        Gangs = gangs;
//        GangTerritories = gangTerritories;
//    }
//    public override string DebugString => $"TimesInsultedByPlayer {Ped.TimesInsultedByPlayer} FedUp {Ped.IsFedUpWithPlayer}";
//    public override bool CanPerformActivities { get; set; } = true;
//    private bool CanContinueConversation => Ped.Pedestrian.Exists() && Player.Character.DistanceTo2D(Ped.Pedestrian) <= 6f && Ped.CanConverse && Player.ActivityManager.CanConverse;
//    public PedExt ConversingPed => Ped;

//    public override void Dispose()
//    {
//        if(IsDisposed)
//        {
//            return;
//        }
//        Player.ButtonPrompts.RemovePrompts("Conversation");
//        Player.ActivityManager.IsConversing = false;
//        if (Ped != null && Ped.Pedestrian.Exists() && IsTasked)
//        {
//            Ped.Pedestrian.BlockPermanentEvents = false;
//            Ped.Pedestrian.KeepTasks = false;
//            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
//        }
//        NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
//        IsDisposed = true;       
//    }







//    public override void Start()
//    {
//        if(Ped == null || !Ped.Pedestrian.Exists())
//        {
//            return;
//        }
//        Player.ActivityManager.IsConversing = true;
//        NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
//        GameFiber.StartNew(delegate
//        {
//            try
//            {
//                Greet();
//                Tick();
//                Dispose();
//            }
//            catch (Exception ex)
//            {
//                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
//                EntryPoint.ModController.CrashUnload();
//            }
//        }, "Conversation");    
//    }



//    public void TransitionToTransaction()
//    {
//        CancelledConversation = true;
//        IsDisposed = true;
//        Player.ButtonPrompts.RemovePrompts("Conversation");
//        Player.ActivityManager.IsConversing = false;
//    }

//    public void OnAdvancedConversationStopped()
//    {
//        IsActivelyConversing = false;
//    }



//    private void Tick()
//    {
//        while (CanContinueConversation)
//        {
//            if (CancelledConversation)
//            {
//                Dispose();
//                break;
//            }
//            UpdateButtonPrompts();
//            CheckInput();
//            GameFiber.Yield();
//        }
//        Dispose();
//    }







//    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete, bool isPlayer)
//    {
//        if(!CanContinueConversation)
//        {
//            return false;
//        }
//        bool Spoke = false;
//        foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()))
//        {
//            string voiceName = null;
//            if (isPlayer && Player.CharacterModelIsFreeMode)
//            {
//                voiceName = Player.FreeModeVoice;
//            }
//            else if (!isPlayer && Ped.VoiceName != "")
//            {
//                voiceName = Ped.VoiceName;
//            }
//            bool hasContext = NativeFunction.Natives.DOES_CONTEXT_EXIST_FOR_THIS_PED<bool>(ToSpeak, AmbientSpeech, false);
//            ToSpeak.PlayAmbientSpeech(voiceName, AmbientSpeech, 0, SpeechModifier.Force | SpeechModifier.AllowRepeat);
//            GameFiber.Sleep(300);//100
//            if (ToSpeak.IsAnySpeechPlaying)
//            {
//                Spoke = true;
//            }
//            EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}", 5);
//            if (Spoke)
//            {
//                break;
//            }
//        }
//        GameFiber.Sleep(100);
//        while (ToSpeak.IsAnySpeechPlaying && WaitForComplete && CanContinueConversation)
//        {
//            Spoke = true;
//            GameFiber.Yield();
//        }
        
//        return Spoke;
//    }
//    private void Greet()
//    {
//        GameTimeStartedConversing = Game.GameTime;
//        IsActivelyConversing = true;
//        if (Ped.TimesInsultedByPlayer <= 0)
//        {
//            SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" }, false, true);
//        }
//        else
//        {
//            SayAvailableAmbient(Player.Character, new List<string>() { "PROVOKE_GENERIC", "GENERIC_WHATEVER" }, false, true);
//        }
//        while (CanContinueConversation && Game.GameTime - GameTimeStartedConversing <= 1000)
//        {
//            GameFiber.Yield();
//        }
//        if (!CanContinueConversation)
//        {
//            return;
//        }
//        pedHeadshotHandle = NativeFunction.Natives.RegisterPedheadshot<uint>(Ped.Pedestrian);
//        if (!Ped.IsFedUpWithPlayer)
//        {
//            if (Ped.IsInVehicle)
//            {
//                IsTasked = true;
//                Ped.Pedestrian.BlockPermanentEvents = true;
//                Ped.Pedestrian.KeepTasks = true;
//                IsBlockedEvents = true;
//                EntryPoint.WriteToConsole("CONVERSATION BLOCKED EVENTS");
//                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Ped.Pedestrian, Player.Character, -1, 0, 2);
//            }
//            else if (NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", Ped.Pedestrian))
//            {
//                IsTasked = false;
//            }
//            else
//            {

//                if (!(Ped.Pedestrian.Inventory.EquippedWeapon == null))
//                {
//                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Ped.Pedestrian, (uint)2725352035, true);
//                }

//                IsTasked = true;
//                unsafe
//                {
//                    int lol = 0;
//                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//                    NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 2000);
//                    NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
//                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
//                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
//                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//                }
//            }
//            if (Player.IsInVehicle || Player.ActivityManager.IsSitting)
//            {
//                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Player.Character, Ped.Pedestrian, -1, 0, 2);
//            }
//            else
//            {
//                unsafe
//                {
//                    int lol = 0;
//                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//                    NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Ped.Pedestrian, 2000);
//                    NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Ped.Pedestrian, -1, 0, 2);
//                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
//                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
//                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//                }
//            }
//            uint GameTimeStartedFacing = Game.GameTime;
//            while (CanContinueConversation && Game.GameTime - GameTimeStartedFacing <= 500)
//            {
//                GameFiber.Yield();
//            }
//            if (!CanContinueConversation || CancelledConversation)
//            {
//                return;
//            }
//            if (Ped.TimesInsultedByPlayer <= 0)
//            {
//                SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_HI", "GENERIC_HOWS_IT_GOING" }, true, false);
//            }
//            else
//            {
//                SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_WHATEVER" }, true, false);
//            }
//            if (Ped.Pedestrian.Exists() && IsBlockedEvents)
//            {
//                Ped.Pedestrian.BlockPermanentEvents = true;
//                Ped.Pedestrian.KeepTasks = true;
//                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Ped.Pedestrian, Player.Character, -1, 0, 2);
//            }
//            Ped.PlayerKnownsName = true;
//        }
//        IsActivelyConversing = false;
//        if(!CanContinueConversation || CancelledConversation)
//        {
//            return;
//        }



//        string Description = $"~p~{Ped.GroupName}~s~";
//        if (Ped.IsFedUpWithPlayer)
//        {
//            Description += "~n~~r~Fed Up~s~";
//        }
//        else if (Ped.TimesInsultedByPlayer > 0)
//        {
//            Description += $"~n~~o~Insulted {Ped.TimesInsultedByPlayer} time(s)~s~";
//        }
//        if (Ped.HasMenu)
//        {
//            Description += $"~n~~g~Can Transact~s~";
//        }
//        if (NativeFunction.Natives.IsPedheadshotReady<bool>(pedHeadshotHandle))
//        {
//            string str = NativeFunction.Natives.GetPedheadshotTxdString<string>(pedHeadshotHandle);
//            Game.DisplayNotification(str, str, "~b~Ped Info", $"~y~{Ped.Name}", Description);
//        }
//        else
//        {
//            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Ped Info", $"~y~{Ped.Name}", Description);
//        }     
//    }
//    private void CheckInput()
//    {
//        if (Player.ButtonPrompts.IsPressed("Cancel"))
//        {
//            CancelledConversation = true;
//        }
//        else if (Player.ButtonPrompts.IsPressed("PositiveReply"))
//        {
//            Positive();
//        }
//        else if (Player.ButtonPrompts.IsPressed("NegativeReply"))
//        {
//            Negative();
//        }
//        else if (Player.ButtonPrompts.IsPressed("AskQuestion"))
//        {
//            AskQuestion();
//        }
//    }
//    private void AskQuestion()
//    {
//        AdvancedConversation = new AdvancedConversation(Player, this, ModItems, Zones, ShopMenus, PlacesOfInterest, Gangs, GangTerritories);
//        AdvancedConversation.Setup();
//        AdvancedConversation.Show();
//        IsActivelyConversing = true;
//    }
//    private void UpdateButtonPrompts()
//    {
//        if (IsActivelyConversing)
//        {
//            Player.ButtonPrompts.RemovePrompts("Conversation");
//        }
//        else
//        {
//            if (!Player.ButtonPrompts.HasPrompt("Conversation"))
//            {
//                Player.ButtonPrompts.AddPrompt("Conversation", Ped.TimesInsultedByPlayer <= 0 ? "Chat" : "Apologize", "PositiveReply", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1);
//                Player.ButtonPrompts.AddPrompt("Conversation", Ped.TimesInsultedByPlayer <= 0 ? "Insult" : "Antagonize", "NegativeReply", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);
//                Player.ButtonPrompts.AddPrompt("Conversation", "Transact/Question", "AskQuestion", Settings.SettingsManager.KeySettings.InteractStart, 5);
//                Player.ButtonPrompts.AddPrompt("Conversation", "Cancel", "Cancel", Settings.SettingsManager.KeySettings.InteractCancel, 90);
//            }
//        }
//    }




//    private bool CanSay(Ped ToSpeak, string Speech)
//    {
//        bool CanSay = NativeFunction.CallByHash<bool>(0x49B99BF3FDA89A7A, ToSpeak, Speech, 0);
//        return CanSay;
//    }
//    private void SayInsult(Ped ToReply, bool isPlayer)
//    {
//        if (Ped.TimesInsultedByPlayer <= 0)
//        {
//            SayAvailableAmbient(ToReply, new List<string>() { "PROVOKE_GENERIC", "GENERIC_WHATEVER" }, true, isPlayer);//
//        }
//        else if (Ped.TimesInsultedByPlayer <= 2)
//        {
//            SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_INSULT_MED", "GENERIC_CURSE_MED" }, true, isPlayer);
//        }
//        else
//        {
//            SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_INSULT_HIGH", "GENERIC_CURSE_HIGH", "PROVOKE_GENERIC", "PROVOKE_BAR" }, true, isPlayer);
//        }
//    }
//    private void SaySmallTalk(Ped ToReply, bool IsReply, bool isPlayer)
//    {

//        if (IsReply)
//        {
//            SayAvailableAmbient(ToReply, new List<string>() { "CHAT_RESP", "PED_RANT_RESP", "CHAT_STATE", "PED_RANT",
//                //"PHONE_CONV1_CHAT1", "PHONE_CONV1_CHAT2", "PHONE_CONV1_CHAT3", "PHONE_CONV1_INTRO", "PHONE_CONV1_OUTRO",
//                //"PHONE_CONV2_CHAT1", "PHONE_CONV2_CHAT2", "PHONE_CONV2_CHAT3", "PHONE_CONV2_INTRO", "PHONE_CONV2_OUTRO",
//                //"PHONE_CONV3_CHAT1", "PHONE_CONV3_CHAT2", "PHONE_CONV3_CHAT3", "PHONE_CONV3_INTRO", "PHONE_CONV3_OUTRO",
//                //"PHONE_CONV4_CHAT1", "PHONE_CONV4_CHAT2", "PHONE_CONV4_CHAT3", "PHONE_CONV4_INTRO", "PHONE_CONV4_OUTRO",
//                //"PHONE_SURPRISE_PLAYER_APPEARANCE_01","SEE_WEIRDO_PHONE",
//                "PED_RANT_01",



//            }, true, isPlayer);
//        }
//        else
//        {
//            SayAvailableAmbient(ToReply, new List<string>() { "CHAT_STATE", "PED_RANT", "CHAT_RESP", "PED_RANT_RESP", "CULT_TALK",
//                //"PHONE_CONV1_CHAT1", "PHONE_CONV1_CHAT2", "PHONE_CONV1_CHAT3", "PHONE_CONV1_INTRO", "PHONE_CONV1_OUTRO",
//                //"PHONE_CONV2_CHAT1", "PHONE_CONV2_CHAT2", "PHONE_CONV2_CHAT3", "PHONE_CONV2_INTRO", "PHONE_CONV2_OUTRO",
//                //"PHONE_CONV3_CHAT1", "PHONE_CONV3_CHAT2", "PHONE_CONV3_CHAT3", "PHONE_CONV3_INTRO", "PHONE_CONV3_OUTRO",
//                //"PHONE_CONV4_CHAT1", "PHONE_CONV4_CHAT2", "PHONE_CONV4_CHAT3", "PHONE_CONV4_INTRO", "PHONE_CONV4_OUTRO",
//                //"PHONE_SURPRISE_PLAYER_APPEARANCE_01","SEE_WEIRDO_PHONE",
//                "PED_RANT_01",

//            }, true, isPlayer);
//        }
//    }
//    public void Negative()
//    {
//        IsActivelyConversing = true;
//        Player.ButtonPrompts.Clear();

//        SayInsult(Player.Character, true);
//        SayInsult(Ped.Pedestrian, false);

//        //Ped.TimesInsultedByPlayer++;
//        Ped.InsultedByPlayer();



//        GameFiber.Sleep(200);


//        if (Ped.IsFedUpWithPlayer)
//        {
//            if (Ped.IsCop)
//            {
//                Player.SetAngeredCop();
//            }
//            else
//            {
//                Ped.AddWitnessedPlayerCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == "Harassment"), Player.Character.Position);
//            }
//            CancelledConversation = true;
//        }
//        IsActivelyConversing = false;
//    }
//    public void Positive()
//    {
//        IsActivelyConversing = true;
//        Player.ButtonPrompts.Clear();
//        if (Ped.TimesInsultedByPlayer >= 1)
//        {
//            SayApology(Player.Character, false, true);
//            if (!Ped.IsFedUpWithPlayer)
//            {
//                SayApology(Ped.Pedestrian, true, false);
//            }
//        }
//        else
//        {
//            SaySmallTalk(Player.Character, false, true);
//            SaySmallTalk(Ped.Pedestrian, true, false);
//        }
//        if (Ped.TimesInsultedByPlayer > 0)
//        {
//            //Ped.TimesInsultedByPlayer--;
//            Ped.ApolgizedToPlayer();
//        }
//        GameFiber.Sleep(200);
//        IsActivelyConversing = false;
//    }
//    public void PedReply(string toDisplay)
//    {
//        IsActivelyConversing = true;
//        Player.ButtonPrompts.Clear();
//        SaySmallTalk(Player.Character, false, true);
//        Game.DisplaySubtitle(toDisplay);
//        SaySmallTalk(Ped.Pedestrian, true, false);
//        GameFiber.Sleep(200);
//        IsActivelyConversing = false;
//    }
//    private void SayApology(Ped ToReply, bool IsReply, bool isPlayer)
//    {
//        if (IsReply)
//        {
//            if (Ped.TimesInsultedByPlayer >= 3)
//            {
//                //say nothing
//            }
//            else if (Ped.TimesInsultedByPlayer >= 2)
//            {
//                SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_WHATEVER" }, true, isPlayer);
//            }
//            else
//            {
//                SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_THANKS" }, true, isPlayer);
//            }
//        }
//        else
//        {
//            SayAvailableAmbient(ToReply, new List<string>() { "APOLOGY_NO_TROUBLE", "GENERIC_HOWS_IT_GOING", "GETTING_OLD", "LISTEN_TO_RADIO" }, true, isPlayer);
//        }
//    }








//}