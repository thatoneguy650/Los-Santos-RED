using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Xml.Serialization;
using RAGENativeUI;
using LosSantosRED.lsr;
using ExtensionsMethods;

public class BankDrawerInteract : InteriorInteract
{
    private string drawerStealPromptText = "Steal from Drawer";
    private string drawerStealEmptyText = "Drawer Empty";
    public int TotalCash { get; set; }
    public Bank Bank { get; set; }
    public BankDrawerInteract()
    {

    }

    public BankDrawerInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }
    public override void OnInteriorLoaded()
    {
        if (Bank == null)
        {
            return;
        }
        TotalCash = RandomItems.GetRandomNumberInt(Bank.DrawerCashMin, Bank.DrawerCashMax);
        EntryPoint.WriteToConsole($"BankDrawerInteract OnInteriorLoaded SET TOTALCASH ${TotalCash} OF DRAWER");
    }
    public override void OnInteract()
    {
        if(Bank == null)
        {
            return;
        }
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        SetupCamera(false);
        if (!WithWarp)
        {
            if (!MoveToPosition(3.0f))
            {
                Interior.IsMenuInteracting = false;
                Game.DisplayHelp("Interact Failed");
                LocationCamera?.StopImmediately(true);
                return;
            }
        }
        if (TotalCash == 0)
        {
            Interior.IsMenuInteracting = false;
            Game.DisplaySubtitle(drawerStealEmptyText);
            LocationCamera?.StopImmediately(true);
            return;
        }
        PerformAnimation();
        Interior.IsMenuInteracting = false;
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);

    }
    public override void AddPrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
    public bool StopPerformingAnimation()
    {
        return true;
    }
    private bool PerformAnimation()
    {
        Player.ActivityManager.StopDynamicActivity();
        AnimationDictionary.RequestAnimationDictionay("oddjobs@shop_robbery@rob_till");
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "oddjobs@shop_robbery@rob_till", "enter", 4.0f, -4.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "oddjobs@shop_robbery@rob_till", "loop", 4.0f, -4.0f, -1, 1, 0, false, false, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        bool IsCancelled = false;
        Player.Violations.TheftViolations.IsRobbingBank = true;
        uint GameTimeLastGotCash = Game.GameTime;
        while (Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            if (Game.GameTime - GameTimeLastGotCash >= 900)
            {
                GiveCash();
                GameTimeLastGotCash = Game.GameTime;
            }
            if (TotalCash <= 0)
            {
                break;
            }
            if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            {
                IsCancelled = true;
                break;
            }
            GameFiber.Yield();
        }
        EntryPoint.WriteToConsole($"BANK PlayMoneyAnimation IsCancelled: {IsCancelled}");
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.Violations.TheftViolations.IsRobbingBank = false;
        if (IsCancelled)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void GiveCash()
    {
        if (TotalCash <= Bank.DrawerCashGainedPerAnimation)
        {
            Player.BankAccounts.GiveMoney(TotalCash, false);
            Bank.PlaySuccessSound();
            TotalCash = 0;
            EntryPoint.WriteToConsole($"BANK GAVE CASH 1 {TotalCash}");
        }
        else
        {
            Player.BankAccounts.GiveMoney(Bank.DrawerCashGainedPerAnimation, false);
            Bank.PlaySuccessSound();
            TotalCash -= Bank.DrawerCashGainedPerAnimation;
            EntryPoint.WriteToConsole($"BANK GAVE CASH 2 {TotalCash}");
        }
        Game.DisplaySubtitle($"Drawer Cash: ${TotalCash}");
    }
}

