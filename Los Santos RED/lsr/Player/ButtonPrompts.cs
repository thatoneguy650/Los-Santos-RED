using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ButtonPrompts
{
    
    private IButtonPromptable Player;
    private ISettingsProvideable Settings;
    private bool CanInteractWithClosestLocation => Player.ClosestInteractableLocation != null && !Player.IsInteractingWithLocation && !Player.IsInteracting && (Player.IsNotWanted || (Player.ClosestInteractableLocation.CanInteractWhenWanted && Player.ClosestPoliceDistanceToPlayer >= 80f && !Player.AnyPoliceRecentlySeenPlayer));
    public ButtonPrompts(IButtonPromptable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public void Setup()
    {
        Player.ButtonPromptList.Clear();
    }
    public void Update()
    {
        bool addedPromptGroup = false;
        if (!addedPromptGroup && !Player.IsInteracting && Player.CanConverseWithLookedAtPed && Settings.SettingsManager.ActivitySettings.AllowPedConversations)
        {
            PersonInteractingPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "StartConversation");
            Player.ButtonPromptList.RemoveAll(x => x.Group == "StartTransaction");
        }
        if(!addedPromptGroup)
        {
            if (Player.CanLootLookedAtPed && Settings.SettingsManager.ActivitySettings.AllowPedLooting)
            {
                PersonLootingPrompts();
                addedPromptGroup = true;
            }
            else
            {
                Player.ButtonPromptList.RemoveAll(x => x.Group == "Search");
            }
            if (Player.CanDragLookedAtPed && Settings.SettingsManager.ActivitySettings.AllowDraggingOtherPeds)
            {
                PersonDraggingPrompts();
                addedPromptGroup = true;
            }
            else
            {
                Player.ButtonPromptList.RemoveAll(x => x.Group == "Drag");
            }
        }
        if (Player.CanGrabLookedAtPed && Settings.SettingsManager.ActivitySettings.AllowTakingOtherPedsHostage)
        {
            PersonGrabPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "Grab");
        }
        if (!addedPromptGroup && !Player.IsInteracting && CanInteractWithClosestLocation)//no cops around
        {
            LocationInteractingPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "InteractableLocation");
        }
        if (!addedPromptGroup && !Player.IsInteracting && Player.CanPerformActivities && Player.IsNearScenario && Settings.SettingsManager.ActivitySettings.AllowStartingScenarios)//currently isnearscenario is turned off
        {
            ScenarioPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "StartScenario");
        }
    }
    public void Dispose()
    {
        Player.ButtonPromptList.Clear();
    }
    public void AddPrompt(string groupName, string prompt, string identifier, Keys interactKey, int order)
    {
        if (!Player.ButtonPromptList.Any(x => x.Identifier == identifier))
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == groupName);
            Player.ButtonPromptList.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, order));
        }
    }
    public bool HasPrompt(string identifier)
    {
        return Player.ButtonPromptList.Any(x => x.Identifier == identifier);
    }
    public bool IsPressed(string identifier)
    {
        return Player.ButtonPromptList.Any(x => x.Identifier == identifier && x.IsPressedNow);
    }
    public bool IsHeld(string identifier)
    {
        return Player.ButtonPromptList.Any(x => x.Identifier == identifier && x.IsHeldNow);
    }
    private void PersonInteractingPrompts()
    {
        Player.ButtonPromptList.RemoveAll(x => x.Group == "InteractableLocation");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartScenario");

        if (!Player.ButtonPromptList.Any(x => x.Identifier == $"Talk {Player.CurrentLookedAtPed.Handle}"))
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "StartConversation");
            Player.ButtonPromptList.Add(new ButtonPrompt($"Talk to {Player.CurrentLookedAtPed.FormattedName}", "StartConversation", $"Talk {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractStart, 1));
        }
        if (((Player.CurrentLookedAtPed.GetType() == typeof(Merchant) && Player.CurrentLookedAtPed.IsNearSpawnPosition) || Player.CurrentLookedAtPed.HasMenu) && (!Player.IsInVehicle || !Player.CurrentLookedAtPed.IsInVehicle) && !Player.ButtonPromptList.Any(x => x.Identifier == $"Purchase {Player.CurrentLookedAtPed.Pedestrian.Handle}"))
        {
            bool toSell = false;
            bool toBuy = false;
            if (Player.CurrentLookedAtPed.HasMenu)
            {
                toSell = Player.CurrentLookedAtPed.ShopMenu.Items.Any(x => x.Sellable);
                toBuy = Player.CurrentLookedAtPed.ShopMenu.Items.Any(x => x.Purchaseable);
            }
            Player.ButtonPromptList.RemoveAll(x => x.Group == "StartTransaction");
            string promptText = $"Purchase from {Player.CurrentLookedAtPed.FormattedName}";
            if (toSell && toBuy)
            {
                promptText = $"Transact with {Player.CurrentLookedAtPed.FormattedName}";
            }
            else if (toBuy)
            {
                promptText = $"Buy from {Player.CurrentLookedAtPed.FormattedName}";
            }
            else if (toSell)
            {
                promptText = $"Sell to {Player.CurrentLookedAtPed.FormattedName}";
            }
            else
            {
                promptText = $"Transact with {Player.CurrentLookedAtPed.FormattedName}";
            }
            Player.ButtonPromptList.Add(new ButtonPrompt(promptText, "StartTransaction", $"Purchase {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 2));
        }
        else
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "StartTransaction");
        }
    }
    private void PersonLootingPrompts()
    {
        Player.ButtonPromptList.RemoveAll(x => x.Group == "InteractableLocation");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartScenario");
        if (!Player.ButtonPromptList.Any(x => x.Identifier == $"Search {Player.CurrentLookedAtPed.Handle}"))
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "Search");
            Player.ButtonPromptList.Add(new ButtonPrompt($"Search {Player.CurrentLookedAtPed.FormattedName}", "Search", $"Search {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractStart, 1));
        }
        //else
        //{
        //    Player.ButtonPromptList.RemoveAll(x => x.Group == "Loot");
        //}
    }
    private void PersonDraggingPrompts()
    {
        Player.ButtonPromptList.RemoveAll(x => x.Group == "InteractableLocation");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartScenario");
        if (!Player.ButtonPromptList.Any(x => x.Identifier == $"Drag {Player.CurrentLookedAtPed.Handle}"))
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "Drag");
            Player.ButtonPromptList.Add(new ButtonPrompt($"Drag {Player.CurrentLookedAtPed.FormattedName}", "Drag", $"Drag {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
        }
        //else
        //{
        //    Player.ButtonPromptList.RemoveAll(x => x.Group == "Loot");
        //}
    }
    private void PersonGrabPrompts()
    {
        Player.ButtonPromptList.RemoveAll(x => x.Group == "InteractableLocation");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartScenario");
        if (!Player.ButtonPromptList.Any(x => x.Identifier == $"Grab {Player.CurrentLookedAtPed.Handle}"))
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "Grab");
            Player.ButtonPromptList.Add(new ButtonPrompt($"Grab {Player.CurrentLookedAtPed.FormattedName}", "Grab", $"Grab {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractCancel, 999));
        }
        //else
        //{
        //    Player.ButtonPromptList.RemoveAll(x => x.Group == "Loot");
        //}
    }
    public void RemovePrompts(string groupName)
    {
        Player.ButtonPromptList.RemoveAll(x => x.Group == groupName);
    }
    private void LocationInteractingPrompts()
    {
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartConversation");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartTransaction");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartScenario");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "Search");

        if (!Player.ButtonPromptList.Any(x => x.Identifier == $"{Player.ClosestInteractableLocation.ButtonPromptText}"))
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "InteractableLocation");
            Player.ButtonPromptList.Add(new ButtonPrompt($"{Player.ClosestInteractableLocation.ButtonPromptText}", "InteractableLocation", $"{Player.ClosestInteractableLocation.ButtonPromptText}", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
        }
    }
    private void ScenarioPrompts()
    {
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartConversation");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartTransaction");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "InteractableLocation");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "Search");


        if (!Player.ButtonPromptList.Any(x => x.Identifier == $"StartScenario"))
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "StartScenario");
            Player.ButtonPromptList.Add(new ButtonPrompt($"{Player.ClosestScenario?.Name}", "StartScenario", $"StartScenario", Settings.SettingsManager.KeySettings.ScenarioStart, 2));
        }
    }
}

