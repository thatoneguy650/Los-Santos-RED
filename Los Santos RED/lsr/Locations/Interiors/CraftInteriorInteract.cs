using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System.Collections.Generic;

public class CraftInteriorInteract : InteriorInteract
{
    public string CraftingFlag { get; set; }
    public CraftInteriorInteract()
    {
    }
    public CraftInteriorInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {
    }
    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        SetupCamera(false);
        if (!MoveToPosition())
        {
            Interior.IsMenuInteracting = false;
            Game.DisplayHelp("Access Failed");
            LocationCamera?.StopImmediately(true);
            return;
        }
        Player.InteriorManager.OnStartedInteriorInteract();
        CreateCraftingMenu();
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);
        Interior.IsMenuInteracting = false;
        Player.InteriorManager.OnEndedInteriorInteract();
    }
    public override void AddPrompt()
    {

        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AttemptAddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
    public void CreateCraftingMenu()
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        Player.IsTransacting = true;
        CraftingMenu menu = LocationInteractable.Crafting.CraftingMenu;
        Crafting crafting = LocationInteractable.Crafting;
        menu.Show(CraftingFlag);
        //bool withAnimations = Interior?.IsTeleportEntry == true;
        while (menu.IsAnyMenuVisible || crafting.IsCrafting)
        {
            GameFiber.Yield();
        }
        menu.Hide();
        Player.ActivityManager.IsInteractingWithLocation = false;
        if (Interior != null)
        {
            Interior.IsMenuInteracting = false;
        }
    }
}