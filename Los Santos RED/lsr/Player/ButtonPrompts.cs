﻿using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
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
    private bool addedPromptGroup;

    private bool CanInteractWithClosestLocation => Player.ClosestInteractableLocation != null && !Player.ActivityManager.IsInteractingWithLocation && !Player.ActivityManager.IsInteracting && (Player.IsNotWanted || (Player.ClosestInteractableLocation.CanInteractWhenWanted && Player.ClosestPoliceDistanceToPlayer >= 80f && !Player.AnyPoliceRecentlySeenPlayer));
    public List<ButtonPrompt> Prompts { get; private set; }
    public ButtonPrompts(IButtonPromptable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
        Prompts = new List<ButtonPrompt>();
    }
    public void Setup()
    {
        Prompts.Clear();
        //Player.ButtonPromptList.Clear();
    }
    public void Update()
    {
        addedPromptGroup = false;
        AttemptAddInteractionPrompts();
        AttemptAddAdvancedInteractionPrompts();
        AttemptAddLocationPrompts();
        AttemptAddActivityPrompts();
        AttemptRemoveMenuPrompts();
    }
    public void Dispose()
    {
        Prompts.Clear();
    }
    public void Clear()
    {
        Prompts.Clear();
    }
    public void RemovePrompts(string groupName)
    {
        Prompts.RemoveAll(x => x.Group == groupName);
    }
    public void AttemptAddPrompt(string groupName, string prompt, string identifier, Keys interactKey, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier) && !Prompts.Any(x => x.Key == interactKey))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, order));
        }
    }
    public void AddPrompt(string groupName, string prompt, string identifier, Keys interactKey, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, order));
        }
    }

    public void AddPrompt(string groupName, string prompt, string identifier, Keys modifierKey, Keys interactKey, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, modifierKey, order));
        }
    }
    public void AddPrompt(string groupName, string prompt, string identifier, GameControl gameCcontrol, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, gameCcontrol, order));
        }
    }
    public bool HasPrompt(string identifier)
    {
        return Prompts.Any(x => x.Identifier == identifier);
    }
    public bool HasPrompt(Keys key, string excludedIdentifier)
    {
        return Prompts.Any(x => x.Key == key && x.Identifier != excludedIdentifier);
    }
    public bool IsPressed(string identifier)
    {
        return Prompts.Any(x => x.Identifier == identifier && x.IsPressedNow);
    }
    public bool IsHeld(string identifier)
    {
        return Prompts.Any(x => x.Identifier == identifier && x.IsHeldNow);
    }
    public bool IsGroupPressed(string group)
    {
        return Prompts.Any(x => x.Group == group && x.IsPressedNow);
    }
    private void PersonInteractingPrompts()
    {
        RemovePrompts("InteractableLocation");
        RemovePrompts("StartScenario");
        if (!HasPrompt($"Talk {Player.CurrentLookedAtPed.Handle}"))
        {
            RemovePrompts("StartConversation");
            AddPrompt("StartConversation", $"Talk to {Player.CurrentLookedAtPed.FormattedName}", $"Talk {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractStart, 1);
        }
        if (((Player.CurrentLookedAtPed.GetType() == typeof(Merchant) && Player.CurrentLookedAtPed.IsNearSpawnPosition) || Player.CurrentLookedAtPed.HasMenu) && (!Player.IsInVehicle || !Player.CurrentLookedAtPed.IsInVehicle) && !HasPrompt($"Purchase {Player.CurrentLookedAtPed.Pedestrian.Handle}"))
        {
            bool toSell = false;
            bool toBuy = false;
            if (Player.CurrentLookedAtPed.HasMenu)
            {
                toSell = Player.CurrentLookedAtPed.ShopMenu.Items.Any(x => x.Sellable);
                toBuy = Player.CurrentLookedAtPed.ShopMenu.Items.Any(x => x.Purchaseable);
            }
            RemovePrompts("StartTransaction");
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

            AddPrompt("StartTransaction", promptText, $"Purchase {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 2);
        }
        else
        {
            RemovePrompts("StartTransaction");
        }
    }
    private void PersonLootingPrompts()
    {
        RemovePrompts("InteractableLocation");
        RemovePrompts("StartScenario");
        if (!HasPrompt($"Search {Player.CurrentLookedAtPed.Handle}"))
        {
            RemovePrompts("Search");
            AddPrompt("Search", $"Search {Player.CurrentLookedAtPed.FormattedName}", $"Search {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractStart, 1);
        }
    }
    private void PersonDraggingPrompts()
    {
        RemovePrompts("InteractableLocation");
        RemovePrompts("StartScenario");
        if (!HasPrompt($"Drag {Player.CurrentLookedAtPed.Handle}"))
        {
            RemovePrompts("Drag");
            AddPrompt("Drag", $"Drag {Player.CurrentLookedAtPed.FormattedName}", $"Drag {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 2);
        }
    }
    private void PersonGrabPrompts()
    {
        RemovePrompts("InteractableLocation");
        RemovePrompts("StartScenario");
        if (!HasPrompt($"Grab {Player.CurrentLookedAtPed.Handle}"))
        {
            RemovePrompts("Grab");
            AddPrompt("Grab", $"Grab {Player.CurrentLookedAtPed.FormattedName}", $"Grab {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractCancel, 999);
        }
    }
    private void LocationInteractingPrompts()
    {
        RemovePrompts("StartConversation");
        RemovePrompts("StartTransaction");
        RemovePrompts("StartScenario");
        RemovePrompts("Search");
        RemovePrompts("Drag");//new
        RemovePrompts("Grab");//new
        if(!HasPrompt($"{Player.ClosestInteractableLocation.ButtonPromptText}"))
        {
            RemovePrompts("InteractableLocation");
            AddPrompt("InteractableLocation", $"{Player.ClosestInteractableLocation.ButtonPromptText}", $"{Player.ClosestInteractableLocation.ButtonPromptText}", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1);
        }

    }
    private void ScenarioPrompts()
    {
        RemovePrompts("StartConversation");
        RemovePrompts("StartTransaction");
        RemovePrompts("InteractableLocation");
        RemovePrompts("Search");
        RemovePrompts("Drag");//new
        RemovePrompts("Grab");//new
        if (!HasPrompt($"StartScenario"))
        {
            RemovePrompts("StartScenario");
            AddPrompt("StartScenario", $"{Player.ClosestScenario?.Name}", $"StartScenario", Settings.SettingsManager.KeySettings.ScenarioStart, 2);
        }
    }
    private void AttemptAddActivityPrompts()
    {
        if(HasPrompt(Settings.SettingsManager.KeySettings.InteractNegativeOrNo, "ActivityControlPause"))
        {
            RemovePrompts("ActivityControlPause");
        }
        if (HasPrompt(Settings.SettingsManager.KeySettings.InteractNegativeOrNo, "ActivityControlContinue"))
        {
            RemovePrompts("ActivityControlContinue");
        }
        if (HasPrompt(Settings.SettingsManager.KeySettings.InteractCancel, "ActivityControlCancel"))
        {
            RemovePrompts("ActivityControlCancel");
        }

        if (Player.ActivityManager.IsPerformingActivity)
        {
            if (Player.ActivityManager.CanPauseCurrentActivity && !Player.ActivityManager.IsCurrentActivityPaused)
            {
                AttemptAddPrompt("ActivityControlPause", Player.ActivityManager.PauseCurrentActivityPrompt, "ActivityControlPause", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 998);
            }
            else
            {
                RemovePrompts("ActivityControlPause");
            }
            if (Player.ActivityManager.CanCancelCurrentActivity)
            {
                AttemptAddPrompt("ActivityControlCancel", Player.ActivityManager.CancelCurrentActivityPrompt, "ActivityControlCancel", Settings.SettingsManager.KeySettings.InteractCancel, 999);
            }
            else
            {
                RemovePrompts("ActivityControlCancel");
            }
            RemovePrompts("ActivityControlContinue");
        }
        else
        {
            if (Player.ActivityManager.CanPauseCurrentActivity && Player.ActivityManager.IsCurrentActivityPaused)
            {
                AttemptAddPrompt("ActivityControlContinue", Player.ActivityManager.ContinueCurrentActivityPrompt, "ActivityControlContinue", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 998);
                if (Player.ActivityManager.CanCancelCurrentActivity)
                {
                    AttemptAddPrompt("ActivityControlCancel", Player.ActivityManager.CancelCurrentActivityPrompt, "ActivityControlCancel", Settings.SettingsManager.KeySettings.InteractCancel, 999);
                }
                else
                {
                    RemovePrompts("ActivityControlCancel");
                }
            }
            else
            {
                RemovePrompts("ActivityControlContinue");
                RemovePrompts("ActivityControlCancel");
            }
            RemovePrompts("ActivityControlPause");
        }
    }
    private void AttemptRemoveMenuPrompts()
    {
        if(!Player.IsDead)
        {
            RemovePrompts("MenuShowDead");
        }
        if (!Player.IsBusted)
        {
            RemovePrompts("MenuShowBusted");
        }
    }
    private void AttemptAddInteractionPrompts()
    {
        if (!addedPromptGroup && !Player.ActivityManager.IsInteracting && Player.ActivityManager.CanConverseWithLookedAtPed && Settings.SettingsManager.ActivitySettings.AllowPedConversations)
        {
            PersonInteractingPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Prompts.RemoveAll(x => x.Group == "StartConversation");
            Prompts.RemoveAll(x => x.Group == "StartTransaction");
        }
    }
    private void AttemptAddAdvancedInteractionPrompts()
    {

            if(Player.ActivityManager.CanRecruitLookedAtGangMember && !Player.ActivityManager.IsConversing)
            {
                PersonRecruitingPrompts();
                addedPromptGroup = true;
            }
            else
            {
                Prompts.RemoveAll(x => x.Group == "Recruit");
            }


        if (!addedPromptGroup)
        {
            if (Player.CurrentLookedAtObject.Exists() && Player.CanSitOnCurrentLookedAtObject && Player.ActivityManager.CanPerformActivities && !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities && !Player.ActivityManager.IsSitting && !Player.IsInVehicle)
            {
                SittingPrompts();
                addedPromptGroup = true;
            }
            else
            {
                Prompts.RemoveAll(x => x.Group == "Sit");
            }
        }
        else
        {
            Prompts.RemoveAll(x => x.Group == "Sit");
        }

        if (!addedPromptGroup)
        {
            if (Player.ActivityManager.CanLootLookedAtPed && Settings.SettingsManager.ActivitySettings.AllowPedLooting)
            {
                PersonLootingPrompts();
                addedPromptGroup = true;
            }
            else
            {
                Prompts.RemoveAll(x => x.Group == "Search");
            }
            if (Player.ActivityManager.CanDragLookedAtPed && Settings.SettingsManager.ActivitySettings.AllowDraggingOtherPeds)
            {
                PersonDraggingPrompts();
                addedPromptGroup = true;
            }
            else
            {
                Prompts.RemoveAll(x => x.Group == "Drag");
            }
        }

        if (Player.ActivityManager.CanGrabLookedAtPed && Settings.SettingsManager.ActivitySettings.AllowTakingOtherPedsHostage)
        {
            PersonGrabPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Prompts.RemoveAll(x => x.Group == "Grab");
        }


        if(Player.IsWanted && Player.AnyPoliceRecentlySeenPlayer && Player.ClosestPoliceDistanceToPlayer <= 40f && Player.IsAliveAndFree && !Player.PoliceResponse.IsWeaponsFree && Player.Surrendering.CanSurrender)
        {
            AddPrompt("ShowSurrender", "Surrender (Hold)", "ShowSurrender", Settings.SettingsManager.KeySettings.SurrenderKeyModifier, Settings.SettingsManager.KeySettings.SurrenderKey, 999);
        }
        else
        {
            RemovePrompts("ShowSurrender");
        }

    }

    private void SittingPrompts()
    {
        RemovePrompts("InteractableLocation");
        RemovePrompts("StartScenario");
        if (!HasPrompt($"Sit"))
        {
            RemovePrompts("Sit");
            AttemptAddPrompt("Sit", $"Sit", $"Sit", Settings.SettingsManager.KeySettings.InteractCancel, 9);
        }
    }

    private void PersonRecruitingPrompts()
    {
        if (!HasPrompt($"Recruit {Player.CurrentLookedAtGangMember.Handle}"))
        {
            RemovePrompts("Recruit");
            AttemptAddPrompt("Recruit", $"Recruit {Player.CurrentLookedAtGangMember.FormattedName}", $"Recruit {Player.CurrentLookedAtGangMember.Handle}", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 3);
        }
    }

    private void AttemptAddLocationPrompts()
    {
        if (!addedPromptGroup && !Player.ActivityManager.IsInteracting && CanInteractWithClosestLocation)//no cops around
        {
            LocationInteractingPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Prompts.RemoveAll(x => x.Group == "InteractableLocation");
        }



        if (!addedPromptGroup && !Player.ActivityManager.IsInteracting && Player.ActivityManager.CanPerformActivities && Player.IsNearScenario && Settings.SettingsManager.ActivitySettings.AllowStartingScenarios)//currently isnearscenario is turned off
        {
            ScenarioPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Prompts.RemoveAll(x => x.Group == "StartScenario");
        }
    }
}

