using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class Conversation : Interaction, IAdvancedConversationable
{
    private uint GameTimeStartedConversing;
    private bool IsActivelyConversing;
    private bool IsTasked;
    private bool IsBlockedEvents;
    private PedExt Ped;
    private IInteractionable Player;
    private bool CancelledConversation;
    private bool IsDisposed;
    private ISettingsProvideable Settings;
    private ICrimes Crimes;
    private uint pedHeadshotHandle;
    private AdvancedConversation AdvancedConversation;
    private IModItems ModItems;
    private IZones Zones;
    private IShopMenus ShopMenus;
    private IPlacesOfInterest PlacesOfInterest;
    private IGangs Gangs;
    private ISpeeches Speeches;
    private IGangTerritories GangTerritories;
    private List<string> GreetPlayerNegativePossibilites;
    private List<string> GreetPlayerPositivePossibilities;
    private List<string> GreetPedNegativePossibilites;
    private List<string> GreetPedPositivePossibilities;
    private IEntityProvideable World;
    private ILocationInteractable LocationInteractable;
    public Conversation(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems, IZones zones, IShopMenus shopMenus, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories,
        ISpeeches speeches, IEntityProvideable world, ILocationInteractable locationInteractable)
    {
        Player = player;
        Ped = ped;
        Settings = settings;
        Crimes = crimes;
        ModItems = modItems;
        Zones = zones;
        ShopMenus = shopMenus;
        PlacesOfInterest = placesOfInterest;
        Gangs = gangs;
        GangTerritories = gangTerritories;
        Speeches = speeches;
        LocationInteractable = locationInteractable;
        GreetPlayerNegativePossibilites = new List<string>() { "PROVOKE_GENERIC", "GENERIC_WHATEVER" };
        GreetPlayerPositivePossibilities = new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" };
        GreetPedNegativePossibilites = new List<string>() { "GENERIC_WHATEVER" };
        GreetPedPositivePossibilities = new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" };
        Speeches = speeches;
        World = world;
    }
    public override string DebugString => $"TimesInsultedByPlayer {Ped.TimesInsultedByPlayer} FedUp {Ped.IsFedUpWithPlayer}";
    public override bool CanPerformActivities { get; set; } = true;
    private bool CanContinueConversation => Ped.Pedestrian.Exists() && Player.Character.DistanceTo2D(Ped.Pedestrian) <= 6f && Ped.CanConverse && Player.ActivityManager.CanConverse;
    public PedExt ConversingPed => Ped;
    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }
        IsActivelyConversing = false;//should not matter
        Player.ButtonPrompts.RemovePrompts("Conversation");
        Player.ActivityManager.IsConversing = false;
        if (Ped != null && Ped.Pedestrian.Exists() && IsTasked)
        {
            Ped.Pedestrian.BlockPermanentEvents = false;
            Ped.Pedestrian.KeepTasks = false;
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        }
        NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
        IsDisposed = true;
        //EntryPoint.WriteToConsoleTestLong("CONVERSATION DISPOSE RAN");
    }
    public override void Start()
    {
        if (Ped == null || !Ped.Pedestrian.Exists())
        {
            return;
        }
        Player.ActivityManager.IsConversing = true;
        if (Settings.SettingsManager.PlayerOtherSettings.SetCameraHintWhenConversing)
        {
            NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
        }
        pedHeadshotHandle = NativeFunction.Natives.RegisterPedheadshot<uint>(Ped.Pedestrian);
        GameFiber.StartNew(delegate
        {
            try
            {
                Greet();
                Tick();
                Dispose();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Conversation");
    }
    public void TransitionToTransaction()
    {
        CancelledConversation = true;
        IsDisposed = true;
        Player.ButtonPrompts.RemovePrompts("Conversation");
        Player.ActivityManager.IsConversing = false;
    }
    public void OnAdvancedConversationStopped()
    {
        IsActivelyConversing = false;
    }
    public void SaySpeech(SpeechData tosay, UIMenu toShow)
    {
        IsActivelyConversing = true;
        Player.ButtonPrompts.Clear();
        SayAvailableAmbient(Player.Character, new List<string>() { tosay.Name }, true, true);
        GenerateReply(tosay);
        GameFiber.Sleep(200);
        IsActivelyConversing = false;
        if(!IsDisposed && !CancelledConversation && toShow != null)
        {
            toShow.Visible = true;
        }
    }
    private void GenerateReply(SpeechData tosay)
    {
        if(tosay.SpeechType == eSpeechType.Insult)
        {
            SayInsult(Ped.Pedestrian, false);
            Ped.OnInsultedByPlayer(Player);
            if (Ped.IsFedUpWithPlayer)
            {
                if (Ped.IsCop)
                {
                    Player.SetAngeredCop();
                }
                else
                {
                    
                }
                CancelledConversation = true;
            }
        }
        else if(tosay.SpeechType == eSpeechType.Apology)
        {
            SayApology(Ped.Pedestrian, true, false);
            Ped.ApolgizedToByPlayer();
        }
        else 
        {
            SayAvailableAmbient(Ped.Pedestrian, GetSpeechReplies(tosay).Select(x=>x.Name).ToList(), true, false);
        }
    }
    private List<SpeechData> GetSpeechReplies(SpeechData tosay)
    {
        List<SpeechData> SpeechesList = new List<SpeechData>();
        if (tosay.SpeechType == eSpeechType.Greeting || tosay.SpeechType == eSpeechType.Goodbye || tosay.SpeechType == eSpeechType.Reaction || tosay.SpeechType == eSpeechType.AgreeDisagree || tosay.SpeechType == eSpeechType.General)
        {
            SpeechesList.AddRange(Speeches.SpeechLookups.Where(x => x.SpeechType == tosay.SpeechType));
        }
        if (!SpeechesList.Any())
        {
            SpeechesList.Add(tosay);
        }
        return SpeechesList;
    }
    private void Tick()
    {
        while (CanContinueConversation)
        {
            if (CancelledConversation)
            {
                Dispose();
                break;
            }
            UpdateButtonPrompts();
            CheckInput();
            GameFiber.Yield();
        }
        Dispose();
    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete, bool isPlayer)
    {
        if (!CanContinueConversation)
        {
            return false;
        }
        bool Spoke = false;
        foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()))
        {
            string voiceName = null;
            if (isPlayer && Player.CharacterModelIsFreeMode)
            {
                voiceName = Player.FreeModeVoice;
            }
            else if (!isPlayer && Ped.VoiceName != "")
            {
                voiceName = Ped.VoiceName;
            }
            bool hasContext = NativeFunction.Natives.DOES_CONTEXT_EXIST_FOR_THIS_PED<bool>(ToSpeak, AmbientSpeech, false);
            ToSpeak.PlayAmbientSpeech(voiceName, AmbientSpeech, 0, SpeechModifier.Force | SpeechModifier.AllowRepeat);
            GameFiber.Sleep(300);//100
            if (ToSpeak.IsAnySpeechPlaying)
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
        while (ToSpeak.IsAnySpeechPlaying && WaitForComplete && CanContinueConversation)
        {
            Spoke = true;
            GameFiber.Yield();
        }

        return Spoke;
    }
    private void Greet()
    {
        GameTimeStartedConversing = Game.GameTime;
        IsActivelyConversing = true;
        DoPlayerGreet();
        if (!CanContinueConversation || CancelledConversation)
        {
            return;
        }
        DoPedGreet();
        IsActivelyConversing = false;
        if (!CanContinueConversation || CancelledConversation)
        {
            return;
        }
        Ped.ShowPedInfoNotification(pedHeadshotHandle);
    }
    private void DoPlayerGreet()
    {
        SayAvailableAmbient(Player.Character, Ped.TimesInsultedByPlayer <= 0 ? GreetPlayerPositivePossibilities : GreetPlayerNegativePossibilites, false, true);
        while (CanContinueConversation && Game.GameTime - GameTimeStartedConversing <= 1000)
        {
            GameFiber.Yield();
        }
    }
    private void DoPedGreet()
    {
        if(Ped.IsFedUpWithPlayer)
        {
            return;
        }
        DoTasking();
        if (!CanContinueConversation || CancelledConversation)
        {
            return;
        }
        SayAvailableAmbient(Ped.Pedestrian, Ped.TimesInsultedByPlayer <= 0 ? GreetPedPositivePossibilities : GreetPedNegativePossibilites, true, false);
        DoPostGreetTask();
        Ped.PlayerKnownsName = true;
    }
    private void DoTasking()
    {
        TaskPed();
        TaskPlayer();    
        uint GameTimeStartedFacing = Game.GameTime;
        while (CanContinueConversation && Game.GameTime - GameTimeStartedFacing <= 500)
        {
            GameFiber.Yield();
        }
    }
    private void TaskPed()
    {
        if (Ped.IsInVehicle)
        {
            IsTasked = true;
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            IsBlockedEvents = true;
            //EntryPoint.WriteToConsoleTestLong("CONVERSATION BLOCKED EVENTS");
            NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Ped.Pedestrian, Player.Character, -1, 0, 2);
        }
        else if (NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", Ped.Pedestrian))
        {
            IsTasked = false;
        }
        else
        {
            if (!(Ped.Pedestrian.Inventory.EquippedWeapon == null))
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Ped.Pedestrian, (uint)2725352035, true);
            }

            IsTasked = true;
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
    }
    private void TaskPlayer()
    {
        if (Player.IsInVehicle || Player.ActivityManager.IsSitting)
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
    }
    private void DoPostGreetTask()
    {
        if(!Ped.Pedestrian.Exists() || !IsBlockedEvents)
        {
            return;
        }
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Ped.Pedestrian, Player.Character, -1, 0, 2);     
    }
    private void CheckInput()
    {
        if (Player.ButtonPrompts.IsPressed("Cancel"))
        {
            CancelledConversation = true;
        }
        else if (Player.ButtonPrompts.IsPressed("PositiveReply"))
        {
            Positive();
        }
        else if (Player.ButtonPrompts.IsPressed("NegativeReply"))
        {
            Negative();
        }
        else if (Player.ButtonPrompts.IsPressed("AskQuestion"))
        {
            AskQuestion();
        }
    }
    private void AskQuestion()
    {
        AdvancedConversation = new AdvancedConversation(Player, this, ModItems, Zones, ShopMenus, PlacesOfInterest, Gangs, GangTerritories, Speeches, World, LocationInteractable);
        AdvancedConversation.Setup();
        AdvancedConversation.Show();
        IsActivelyConversing = true;
    }
    private void UpdateButtonPrompts()
    {
        if (IsActivelyConversing)
        {
            Player.ButtonPrompts.RemovePrompts("Conversation");
        }
        else
        {
            if (!Player.ButtonPrompts.HasPrompt("Conversation"))
            {
                Player.ButtonPrompts.AddPrompt("Conversation", Ped.TimesInsultedByPlayer <= 0 ? "Quick Chat +" : "Quick Apologize +", "PositiveReply", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1);
                Player.ButtonPrompts.AddPrompt("Conversation", Ped.TimesInsultedByPlayer <= 0 ? "Quick Insult -" : "Quick Antagonize -", "NegativeReply", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);
                Player.ButtonPrompts.AddPrompt("Conversation", "Interact Menu", "AskQuestion", Settings.SettingsManager.KeySettings.InteractStart, 5);
                Player.ButtonPrompts.AddPrompt("Conversation", "Stop Talking", "Cancel", Settings.SettingsManager.KeySettings.InteractCancel, 90);
            }
        }
    }
    private bool CanSay(Ped ToSpeak, string Speech)
    {
        bool CanSay = NativeFunction.CallByHash<bool>(0x49B99BF3FDA89A7A, ToSpeak, Speech, 0);
        return CanSay;
    }
    private void SayInsult(Ped ToReply, bool isPlayer)
    {
        if (Ped.TimesInsultedByPlayer <= 0)
        {
            SayAvailableAmbient(ToReply, new List<string>() { "PROVOKE_GENERIC", "GENERIC_WHATEVER" }, true, isPlayer);//
        }
        else if (Ped.TimesInsultedByPlayer <= 2)
        {
            SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_INSULT_MED", "GENERIC_CURSE_MED" }, true, isPlayer);
        }
        else
        {
            SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_INSULT_HIGH", "GENERIC_CURSE_HIGH", "PROVOKE_GENERIC", "PROVOKE_BAR" }, true, isPlayer);
        }
    }
    private void SaySmallTalk(Ped ToReply, bool IsReply, bool isPlayer)
    {

        if (IsReply)
        {
            SayAvailableAmbient(ToReply, new List<string>() { "CHAT_RESP", "PED_RANT_RESP", "CHAT_STATE", "PED_RANT",
                "PED_RANT_01",
            }, true, isPlayer);
        }
        else
        {
            SayAvailableAmbient(ToReply, new List<string>() { "CHAT_STATE", "PED_RANT", "CHAT_RESP", "PED_RANT_RESP", "CULT_TALK",
                "PED_RANT_01",
            }, true, isPlayer);
        }
    }
    public void Negative()
    {
        IsActivelyConversing = true;
        Player.ButtonPrompts.Clear();

        SayInsult(Player.Character, true);
        SayInsult(Ped.Pedestrian, false);

        //Ped.TimesInsultedByPlayer++;
        Ped.OnInsultedByPlayer(Player);
        if (Ped.IsFedUpWithPlayer)
        {
            //if (Ped.IsCop)
            //{
            //    Player.SetAngeredCop();
            //}
            //else
            //{
            //    Ped.AddWitnessedPlayerCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == "Harassment"), Player.Character.Position);
            //}
            CancelledConversation = true;
        }
        GameFiber.Sleep(200);

        IsActivelyConversing = false;
    }
    public void Positive()
    {
        IsActivelyConversing = true;
        Player.ButtonPrompts.Clear();
        if (Ped.TimesInsultedByPlayer >= 1)
        {
            SayApology(Player.Character, false, true);
            if (!Ped.IsFedUpWithPlayer)
            {
                SayApology(Ped.Pedestrian, true, false);
            }
        }
        else
        {
            SaySmallTalk(Player.Character, false, true);
            SaySmallTalk(Ped.Pedestrian, true, false);
        }
        if (Ped.TimesInsultedByPlayer > 0)
        {
            //Ped.TimesInsultedByPlayer--;
            Ped.ApolgizedToByPlayer();
        }
        GameFiber.Sleep(200);
        IsActivelyConversing = false;
    }
    public void PedReply(string toDisplay)
    {
        IsActivelyConversing = true;
        Player.ButtonPrompts.Clear();
        //SaySmallTalk(Player.Character, false, true);
        Game.DisplaySubtitle(toDisplay);
        SaySmallTalk(Ped.Pedestrian, true, false);
        GameFiber.Sleep(200);
        IsActivelyConversing = false;
    }
    private void SayApology(Ped ToReply, bool IsReply, bool isPlayer)
    {
        if (IsReply)
        {
            if (Ped.TimesInsultedByPlayer >= 3)
            {
                //say nothing
            }
            else if (Ped.TimesInsultedByPlayer >= 2)
            {
                SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_WHATEVER" }, true, isPlayer);
            }
            else
            {
                SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_THANKS" }, true, isPlayer);
            }
        }
        else
        {
            SayAvailableAmbient(ToReply, new List<string>() { "APOLOGY_NO_TROUBLE", "GENERIC_HOWS_IT_GOING", "GETTING_OLD", "LISTEN_TO_RADIO" }, true, isPlayer);
        }
    }
}