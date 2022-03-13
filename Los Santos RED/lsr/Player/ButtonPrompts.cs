using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ButtonPrompts
{
    
    private IButtonPromptable Player;
    private ISettingsProvideable Settings;


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
        if (!addedPromptGroup && !Player.IsInteracting && Player.CanConverseWithLookedAtPed)
        {
            PersonInteractingPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "StartConversation");
            Player.ButtonPromptList.RemoveAll(x => x.Group == "StartTransaction");
        }
        if (!addedPromptGroup && Player.ClosestInteractableLocation != null && !Player.IsInteractingWithLocation)
        {
            LocationInteractingPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "InteractableLocation");
        }
        if (!addedPromptGroup && !Player.IsInteracting && Player.CanPerformActivities && Player.IsNearScenario)//currently isnearscenario is turned off
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
    private void LocationInteractingPrompts()
    {
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartConversation");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartTransaction");
        Player.ButtonPromptList.RemoveAll(x => x.Group == "StartScenario");

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


        if (!Player.ButtonPromptList.Any(x => x.Identifier == $"StartScenario"))
        {
            Player.ButtonPromptList.RemoveAll(x => x.Group == "StartScenario");
            Player.ButtonPromptList.Add(new ButtonPrompt($"{Player.ClosestScenario?.Name}", "StartScenario", $"StartScenario", Settings.SettingsManager.KeySettings.ScenarioStart, 2));
        }
    }
}

