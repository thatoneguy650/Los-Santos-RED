using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class Transaction : Interaction
{
    private uint GameTimeStartedConversing;
    private bool IsActivelyConversing;
    private bool IsTasked;
    private PedExt Ped;
    private IInteractionable Player;
    private bool CancelledConversation;
    private ISettingsProvideable Settings;
    private Rage.Object SellingProp;
    public Transaction(IInteractionable player, PedExt ped, ISettingsProvideable settings)
    {
        Player = player;
        Ped = ped;
        Settings = settings;
    }
    public override string DebugString => $"TimesInsultedByPlayer {Ped.TimesInsultedByPlayer} FedUp {Ped.IsFedUpWithPlayer}";
    private bool CanContinueConversation => Player.Character.DistanceTo2D(Ped.Pedestrian) <= 6f && Ped.CanConverse && Player.CanConverse;
    private string BuyPrompt => "Buy " + Wares;
    private string Wares
    {
        get
        {
            if (Ped.MerchantType == MerchantType.HotDog)
            {
                return "Hot Dog";//"prop_cs_hotdog_01"
            }
            else if (Ped.MerchantType == MerchantType.Hamburger)
            {
                return "Hamburger";//"prop_cs_burger_01"
            }
            else if (Ped.MerchantType == MerchantType.Donut)
            {
                return "Donut";//"prop_donut_01","prop_donut_02"
            }
            else if (Ped.MerchantType == MerchantType.Cigarette)
            {
                return "Cigarette";//"ng_proc_cigarette01a"
            }
            else if (Ped.MerchantType == MerchantType.Beer)
            {
                return "Beer";//"prop_cs_beer_bot_40oz", "prop_cs_beer_bot_40oz_02", "prop_cs_beer_bot_40oz_03"
            }
            else if (Ped.MerchantType == MerchantType.Pizza)
            {
                return "Pizza";//"v_res_tt_pizzaplate"
            }
            else
            {
                return "";
            }
        }
    }
    public override void Dispose()
    {
        Player.ButtonPrompts.RemoveAll(x => x.Group == "Transaction");
        Player.IsConversing = false;
        if (Ped != null && Ped.Pedestrian.Exists() && IsTasked)
        {
            //Ped.Pedestrian.Tasks.Clear();
            //NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        }
        NativeFunction.Natives.STOP_GAMEPLAY_HINT(true);
    }
    public override void Start()
    {
        Player.IsConversing = true;
        NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);

        EntryPoint.WriteToConsole($"Transaction START", 3);

        //EntryPoint.WriteToConsole($"Conversation Started");
        GameFiber.StartNew(delegate
        {
            Greet();
            Tick();
            Dispose();
        }, "Conversation");
    }
    private bool CanSay(Ped ToSpeak, string Speech)
    {
        bool CanSay = NativeFunction.CallByHash<bool>(0x49B99BF3FDA89A7A, ToSpeak, Speech, 0);
        //EntryPoint.WriteToConsole($"CONVERSATION Can {ToSpeak.Handle} Say {Speech}? {CanSay}");
        return CanSay;
    }
    private void CheckInput()
    {
        if (IsActivelyConversing)
        {
            Player.ButtonPrompts.RemoveAll(x => x.Group == "Transaction");
        }
        else
        {
            if (!Player.ButtonPrompts.Any(x => x.Group == "Transaction"))
            {

                Player.ButtonPrompts.Add(new ButtonPrompt(BuyPrompt, "Transaction", BuyPrompt, Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
                Player.ButtonPrompts.Add(new ButtonPrompt("Cancel", "Transaction", "Cancel", Settings.SettingsManager.KeySettings.InteractCancel, 3));
            }
        }
        if (Player.ButtonPrompts.Any(x => x.Identifier == "Cancel" && x.IsPressedNow))
        {
            CancelledConversation = true;
        }
        else if (Player.ButtonPrompts.Any(x => x.Identifier == BuyPrompt && x.IsPressedNow))
        {
            Positive();
        }
    }
    private void Positive()
    {
        IsActivelyConversing = true;
        Player.ButtonPrompts.Clear();

        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS" }, true);
        SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BYE" }, true);

        GameFiber.Sleep(2000);
        IsActivelyConversing = false;
        CancelledConversation = true;
        Player.GiveMoney(-5);
        Dispose();
        StartActivity();
    }
    private void StartActivity()
    {
        if(SellingProp.Exists())
        {
            SellingProp.Delete();
        }
        if (Ped.MerchantType == MerchantType.Cigarette)
        {
            Player.StartSmoking();
        }
        else if(Ped.MerchantType == MerchantType.Beer)
        {
            Player.DrinkBeer();
        }
        else 
        {
            if(Ped.MerchantType == MerchantType.HotDog)
            {
                DoPostAnimation("prop_cs_hotdog_01");
            }
            else if (Ped.MerchantType == MerchantType.Hamburger)
            {
                DoPostAnimation("prop_cs_burger_01");
            }
            if (Ped.MerchantType == MerchantType.Pizza)
            {
                DoPostAnimation("v_res_tt_pizzaplate");
            }
            if (Ped.MerchantType == MerchantType.Donut)
            {
                DoPostAnimation("prop_donut_01");
            }
        }
        EntryPoint.WriteToConsole($"Transaction COMPLETED", 3);
    }
    private void DoPostAnimation(string PropName)
    {
        Vector3 HandOffset = new Vector3(0.141f, 0.03f, -0.033f);
        Rotator HandRotator = new Rotator(0.0f, -168f, -84f);
        SellingProp = new Rage.Object("prop_donut_01", Player.Character.GetOffsetPositionUp(50f));
        if (SellingProp.Exists())
        {
            SellingProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, 57005), HandOffset, HandRotator);
        }
        AnimationDictionary.RequestAnimationDictionay("amb@code_human_wander_eating_donut@male@idle_a");
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "amb@code_human_wander_eating_donut@male@idle_a", "idle_c", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
        GameFiber.Sleep(5000);
        if (SellingProp.Exists())
        {
            SellingProp.Delete();
        }
        Player.Character.Health += 20;
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        if (CanContinueConversation)
        {
            foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()))
            {
                ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                GameFiber.Sleep(100);
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
            if (!Spoke)
            {
                Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
            }
        }
        return Spoke;
    }
    private void Greet()
    {
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
            if (NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", Ped.Pedestrian))
            {
                IsTasked = false;
            }
            else
            {
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
    private void Tick()
    {
        while (CanContinueConversation)
        {
            CheckInput();
            if (CancelledConversation)
            {
                Dispose();
                break;
            }
            GameFiber.Yield();
        }
        Dispose();
        GameFiber.Sleep(1000);
    }
}