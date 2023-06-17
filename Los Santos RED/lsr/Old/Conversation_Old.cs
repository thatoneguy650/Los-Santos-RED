//using LosSantosRED.lsr.Interface;
//using Rage;
//using Rage.Native;
//using RAGENativeUI;
//using RAGENativeUI.Elements;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Forms;

//public class Conversation_Old : Interaction
//{
//    private uint GameTimeStartedConversing;
//    private bool IsActivelyConversing;
//    private bool IsTasked;
//    private bool IsBlockedEvents;
//    private PedExt Ped;
//    private IInteractionable Player;
//    private bool CancelledConversation;
//    private ISettingsProvideable Settings;
//    private ICrimes Crimes;
//    private dynamic pedHeadshotHandle;
//    private MenuPool MenuPool;
//    private UIMenu ConversationMenu;
//    private ISpeeches Speeches;
//    private List<SpeechData> PlayerPossible = new List<SpeechData>();
//    private List<SpeechData> PedPossible = new List<SpeechData>();
//    public Conversation_Old(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, ISpeeches speeches)
//    {
//        Player = player;
//        Ped = ped;
//        Settings = settings;
//        Crimes = crimes;
//        Speeches = speeches;
//    }
//    public override string DebugString => $"TimesInsultedByPlayer {Ped.TimesInsultedByPlayer} FedUp {Ped.IsFedUpWithPlayer}";
//    public override bool CanPerformActivities { get; set; } = true;
//    private bool CanContinueConversation => Ped.Pedestrian.Exists() && Player.Character.DistanceTo2D(Ped.Pedestrian) <= 6f && Ped.CanConverse && Player.ActivityManager.CanConverse;
//    public override void Dispose()
//    {
//        Player.ButtonPrompts.RemovePrompts("Conversation");
//        Player.ActivityManager.IsConversing = false;
//        if (Ped != null && Ped.Pedestrian.Exists() && IsTasked && Ped.GetType() != typeof(Merchant))
//        {
//            Ped.Pedestrian.BlockPermanentEvents = false;
//            Ped.Pedestrian.KeepTasks = false;
//            EntryPoint.WriteToConsole("CONVERSATION UNBLOCKED EVENTS 1");
//            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
//        }
//        else if (Ped != null && Ped.Pedestrian.Exists() && IsTasked)
//        {

//            Ped.Pedestrian.BlockPermanentEvents = false;
//            Ped.Pedestrian.KeepTasks = false;
//            EntryPoint.WriteToConsole("CONVERSATION UNBLOCKED EVENTS 2");
//            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
//        }
//        NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
//        ConversationMenu.Visible = false;
//    }
//    public override void Start()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            CreateMenu();
//            Player.ActivityManager.IsConversing = true;
//            NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
//            GameFiber.StartNew(delegate
//            {
//                try
//                {
//                    Greet();
//                    Tick();
//                    Dispose();
//                }
//                catch (Exception ex)
//                {
//                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
//                    EntryPoint.ModController.CrashUnload();
//                }
//            }, "Conversation");
//        }
//    }
//    private void CreateMenu()
//    {
//        MenuPool = new MenuPool();
//        ConversationMenu = new UIMenu("Conversation", "Select an Option");
//        ConversationMenu.RemoveBanner();
//        MenuPool.Add(ConversationMenu);
//        foreach (var speechGroup in Speeches.SpeechLookups.GroupBy(x => x.GroupName).Select(x => x))
//        {
//            UIMenu GroupMenu = MenuPool.AddSubMenu(ConversationMenu, speechGroup.Key);
//            foreach (var SpeechData in speechGroup.OrderBy(x => x.SimpleName).ThenBy(x => x.SubName))
//            {
//                bool PlayerCanSay = NativeFunction.Natives.DOES_CONTEXT_EXIST_FOR_THIS_PED<bool>(Player.Character, SpeechData.Name, true);
//                bool PedCanSay = NativeFunction.Natives.DOES_CONTEXT_EXIST_FOR_THIS_PED<bool>(Ped.Pedestrian, SpeechData.Name, true);
//                if(PlayerCanSay)
//                {
//                    PlayerPossible.Add(SpeechData);
//                }
//                if(PedCanSay)
//                {
//                    PedPossible.Add(SpeechData);
//                }
//                UIMenuItem speechMenuItem = new UIMenuItem(SpeechData.SimpleName, $"{SpeechData.Description} Player: {PlayerCanSay} Ped: {PedCanSay}") { RightLabel = SpeechData.SubName };
//                speechMenuItem.Activated += (menu, item) =>
//                {
//                    SaySpeech(SpeechData);
//                };
//                GroupMenu.AddItem(speechMenuItem);
//            }
//        }
//        UIMenuItem Cancel = new UIMenuItem("Cancel", "Stop Conversation");
//        Cancel.Activated += (menu, item) =>
//        {
//            CancelledConversation = true;
//        };
//        ConversationMenu.AddItem(Cancel);
//    }
//    private void Tick()
//    {     
//        while (CanContinueConversation)
//        {
//            CheckInput();
//            if (CancelledConversation)
//            {
//                Dispose();
//                break;
//            }
//            MenuPool.ProcessMenus();
//            GameFiber.Yield();
//        }
//        Dispose();
//        GameFiber.Sleep(1000);
//    }
//    private void CheckInput()
//    {
//        if(Game.IsKeyDown(Settings.SettingsManager.KeySettings.MenuKey))
//        {
//            if(!IsActivelyConversing && !ConversationMenu.Visible)
//            {
//                ConversationMenu.Visible = true;
//            }
//        }
//    }
//    private void SaySpeech(SpeechData tosay)
//    {
//        IsActivelyConversing = true;
//        Player.ButtonPrompts.Clear();
//        ConversationMenu.Visible = false;
//        SayAvailableAmbient(Player.Character, new List<string>() { tosay.Name }, true, true);
//        SayAvailableAmbient(Ped.Pedestrian, new List<string>() { tosay.Name }, true, false);
//        GameFiber.Sleep(200);
//        IsActivelyConversing = false;

//    }
//    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete, bool isPlayer)
//    {   
//        bool Spoke = false;
//        if (CanContinueConversation)
//        {
//            foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()).Take(3))
//            {
//                string voiceName = null;
//                bool IsOverWrittingVoice = false;
//                if(isPlayer && Player.CharacterModelIsFreeMode)
//                {
//                    voiceName = Player.FreeModeVoice;
//                    IsOverWrittingVoice = true;
//                }
//                else if (!isPlayer && Ped.VoiceName != "")
//                {
//                    voiceName = Ped.VoiceName;
//                    IsOverWrittingVoice = true;
//                }
//                bool hasContext = NativeFunction.Natives.DOES_CONTEXT_EXIST_FOR_THIS_PED<bool>(ToSpeak, AmbientSpeech, false);
//                //if (IsOverWrittingVoice || hasContext)
//                //{
//                //    ToSpeak.PlayAmbientSpeech(voiceName, AmbientSpeech, 0, SpeechModifier.Force);
//                //}

//                ToSpeak.PlayAmbientSpeech(voiceName, AmbientSpeech, 0, SpeechModifier.Force | SpeechModifier.AllowRepeat);

//                GameFiber.Sleep(300);//100
//                if (ToSpeak.IsAnySpeechPlaying)
//                {
//                    Spoke = true;
//                }
//                EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}",5);
//                if (Spoke)
//                {
//                    break;
//                }
//            }
//            GameFiber.Sleep(100);
//            while (ToSpeak.IsAnySpeechPlaying && WaitForComplete && CanContinueConversation)
//            {
//                Spoke = true;
//                GameFiber.Yield();
//            }
//            //if (!Spoke)
//            //{
//            //    Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
//            //}
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
//        while(CanContinueConversation && Game.GameTime - GameTimeStartedConversing <= 1000)
//        {
//            GameFiber.Yield();
//        }
//        if(!CanContinueConversation)
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
//            if (Player.IsInVehicle)
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
//            if (!CanContinueConversation)
//            {
//                return;
//            }

//            if (Ped.TimesInsultedByPlayer <= 0)
//            {
//                SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_HI","GENERIC_HOWS_IT_GOING"  }, true,false);
//            }
//            else
//            {
//                SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_WHATEVER" }, true, false);
//            }


//            if(Ped.Pedestrian.Exists() && IsBlockedEvents)
//            {
//                Ped.Pedestrian.BlockPermanentEvents = true;
//                Ped.Pedestrian.KeepTasks = true;
//                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Ped.Pedestrian, Player.Character, -1, 0, 2);
//            }
//            Ped.PlayerKnownsName = true;
//        }
//        IsActivelyConversing = false;


//        if (CanContinueConversation && !CancelledConversation)
//        {
//            string Description = $"~p~{Ped.GroupName}~s~";
//            if (Ped.IsFedUpWithPlayer)
//            {
//                Description += "~n~~r~Fed Up~s~";
//            }
//            else if (Ped.TimesInsultedByPlayer > 0)
//            {
//                Description += $"~n~~o~Insulted {Ped.TimesInsultedByPlayer} time(s)~s~";
//            }
//            if (Ped.HasMenu)
//            {
//                Description += $"~n~~g~Can Transact~s~";
//            }
//            if (NativeFunction.Natives.IsPedheadshotReady<bool>(pedHeadshotHandle))
//            {
//                string str = NativeFunction.Natives.GetPedheadshotTxdString<string>(pedHeadshotHandle);
//                Game.DisplayNotification(str, str, "~b~Ped Info", $"~y~{Ped.Name}", Description);
//            }
//            else
//            {
//                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Ped Info", $"~y~{Ped.Name}", Description);
//            }


//        }



//    }

//}