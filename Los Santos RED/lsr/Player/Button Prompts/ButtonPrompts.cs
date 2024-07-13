using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
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
    private IEntityProvideable World;
    private bool addedPromptGroup;
    private Action CancelCurrentActivityAction;
    private Action PauseCurrentActivityAction;
    private Action ContinueCurrentActivityAction;

    private bool CanInteractWithClosestLocation => Player.ClosestInteractableLocation != null
        && !Player.ActivityManager.IsInteractingWithLocation
        && !Player.ActivityManager.IsInteracting;
    public List<ButtonPrompt> Prompts { get; private set; }
    public bool IsSuspended { get; set; } = false;
    public bool IsUsingKeyboard { get; private set; } = true;
    public ButtonPrompts(IButtonPromptable player, ISettingsProvideable settings, IEntityProvideable world)
    {
        Player = player;
        Settings = settings;
        World = world;
        Prompts = new List<ButtonPrompt>();
    }
    public void Setup()
    {
        CancelCurrentActivityAction = () => { Player.ActivityManager.CancelCurrentActivity(); };
        PauseCurrentActivityAction = () => { Player.ActivityManager.PauseCurrentActivity(); };
        ContinueCurrentActivityAction = () => { Player.ActivityManager.ContinueCurrentActivity(); };
        Prompts.Clear();
    }
    public void Update()
    {
        addedPromptGroup = false;
        IsUsingKeyboard = NativeFunction.Natives.IS_USING_KEYBOARD_AND_MOUSE<bool>(2);
        AttemptAddConversationPrompts();
        AttemptAddAdvancedInteractionPrompts();
        AttemptAddVehicleInteractPrompts();
        AttemptAddHotwireInteractPrompts();
        AttemptAddLocationPrompts();
        AttemptAddActivityPrompts();
        AttemptRemoveMenuPrompts();
    }

    private void AttemptAddHotwireInteractPrompts()
    {
        VehicleExt toConsider = Player.CurrentVehicle;
        if (Settings.SettingsManager.VehicleSettings.AutoHotwire ||
            Player.ActivityManager.IsInteractingWithLocation ||
            Player.IsShowingFrontEndMenus ||
            Player.Surrendering.HandsAreUp ||
            !Player.IsAliveAndFree ||
            !Player.IsInVehicle || 
            toConsider == null ||

            !toConsider.IsHotWireLocked
            )
        {
            RemovePrompts("VehicleHotwire");
            return;
        }
        toConsider.UpdateHotwirePrompt(Player);
    }

    private void AttemptAddVehicleInteractPrompts()
    {
        VehicleExt toConsider = Player.InterestedVehicle;
        if (!Settings.SettingsManager.UIGeneralSettings.ShowVehicleInteractionPrompt || 
            Player.ActivityManager.IsInteractingWithLocation || 
            Player.IsShowingFrontEndMenus || 
            Player.Surrendering.HandsAreUp || 
            !Player.IsAliveAndFree || 
            //(!Settings.SettingsManager.UIGeneralSettings.ShowVehicleInteractionPromptInVehicle && Player.IsInVehicle) || //send this to the vehicle?
            //Player.ActivityManager.IsPerformingActivity || 
            toConsider == null
            )
        {
            RemovePrompts("VehicleInteract");
            return;
        }
        toConsider.UpdateInteractPrompts(Player);
        //string interactPrompt = toConsider.InteractPrompt(Player, Settings);
        //if (string.IsNullOrEmpty(interactPrompt))
        //{
        //    RemovePrompts("VehicleInteract");
        //    return;
        //}
        //if (!HasPrompt($"VehicleInteract"))
        //{
        //    AttemptAddPrompt("VehicleInteract", interactPrompt, $"VehicleInteract", Settings.SettingsManager.KeySettings.VehicleInteractModifier, Settings.SettingsManager.KeySettings.VehicleInteract, 999);
        //}
    }
    public void Dispose()
    {
        Prompts.Clear();
    }
    public void Clear()
    {
        Prompts.Clear();
    }
    public void RemovePrompt(string identifier)
    {
        Prompts.RemoveAll(x => x.Identifier == identifier);
    }
    public void RemovePrompts(string groupName)
    {
        Prompts.RemoveAll(x => x.Group == groupName);
    }
    public void AttemptAddPrompt(string groupName, string prompt, string identifier, Keys modifierKey, Keys interactKey, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier) && !Prompts.Any(x => x.Key == interactKey))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, modifierKey, order));
        }
    }
    public void AttemptAddPrompt(string groupName, string prompt, string identifier, Keys interactKey, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier) && !Prompts.Any(x => x.Key == interactKey))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, order));
        }
    }


    public void AttemptAddPrompt(string groupName, string prompt, string identifier, Keys modifierKey, Keys interactKey, int order, Action action)
    {
        if (!Prompts.Any(x => x.Identifier == identifier) && !Prompts.Any(x => x.Key == interactKey))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, modifierKey, order) { Action = action });
        }
    }
    public void AttemptAddPrompt(string groupName, string prompt, string identifier, Keys interactKey, int order, Action action)
    {
        if (!Prompts.Any(x => x.Identifier == identifier) && !Prompts.Any(x => x.Key == interactKey))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, order) { Action = action });
        }
    }


    public void AddPrompt(string groupName, string prompt, string identifier, Keys interactKey, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, order));
        }
    }
    public void AddPrompt(string groupName, string prompt, string identifier, Keys interactKey, int order, Action action)
    {
        if (!Prompts.Any(x => x.Identifier == identifier))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, order) { Action = action });
        }
    }
    public void AddPrompt(string groupName, string prompt, string identifier, Keys modifierKey, Keys interactKey, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, modifierKey, order));
        }
    }
    public void AddPrompt(string groupName, string prompt, string identifier, ControllerButtons interactKey, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, order));
        }
    }
    public void AddPrompt(string groupName, string prompt, string identifier, ControllerButtons modifierKey, ControllerButtons interactKey, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, interactKey, modifierKey, order));
        }
    }
    public void AddPrompt(string groupName, string prompt, string identifier, GameControl gameControl, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, gameControl, order));
        }
    }
    public void AttemptAddPrompt(string groupName, string prompt, string identifier, GameControl gameControl, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier) && !Prompts.Any(x=> x.GameControl == gameControl))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, gameControl, order));
        }
    }

    public void AttemptAddPrompt(string groupName, string prompt, string identifier, GameControl gameControl, int order, Action action)
    {
        if (!Prompts.Any(x => x.Identifier == identifier) && !Prompts.Any(x => x.GameControl == gameControl))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, gameControl, order) { Action = action });
        }
    }


    public void AddPrompt(string groupName, string prompt, string identifier, GameControl modifierControl, GameControl gameControl, int order)
    {
        if (!Prompts.Any(x => x.Identifier == identifier))
        {
            Prompts.Add(new ButtonPrompt(prompt, groupName, identifier, gameControl, modifierControl, order));
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
    private void PersonConversationPrompts()
    {
        RemovePrompts("InteractableLocation");
        RemovePrompts("StartScenario");
        if (!HasPrompt($"Talk {Player.CurrentLookedAtPed.Handle}"))
        {
            RemovePrompts("StartConversation");
            string promptText = Player.CurrentLookedAtPed.InteractPrompt(Player);
            AddPrompt("StartConversation", promptText, $"Talk {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractStart, 1);
        }
    }
    private void PersonInspectingPrompts()
    {
        RemovePrompts("InteractableLocation");
        RemovePrompts("StartScenario");
        if (!HasPrompt($"Search {Player.CurrentLookedAtPed.Handle}"))
        {
            RemovePrompts("Search");
            AddPrompt("Search", $"Inspect {Player.CurrentLookedAtPed.FormattedName}", $"Search {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractStart, 1);
        }
    }
    private void PersonDraggingPrompts()
    {
        RemovePrompts("InteractableLocation");
        RemovePrompts("StartScenario");
        if (!HasPrompt($"Drag {Player.CurrentLookedAtPed.Handle}"))
        {
            RemovePrompts("Drag");
            AttemptAddPrompt("Drag", $"Drag {Player.CurrentLookedAtPed.FormattedName}", $"Drag {Player.CurrentLookedAtPed.Handle}", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 2);
        }
    }
    private void PersonGrabPrompts()
    {
        RemovePrompts("InteractableLocation");
        RemovePrompts("StartScenario");
        if (!HasPrompt($"Grab {Player.CurrentLookedAtPed.Handle}"))
        {
            RemovePrompts("Grab");
            AddPrompt("Grab", $"Grab {Player.CurrentLookedAtPed.FormattedName}", $"Grab {Player.CurrentLookedAtPed.Handle}", (GameControl)Settings.SettingsManager.KeySettings.GrabPedGameControl, 999);
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
        RemovePrompts("Treat");//new




        if (!HasPrompt($"{Player.ClosestInteractableLocation.ButtonPromptText}"))
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
        RemovePrompts("Treat");//new
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
        if (!Player.ActivityManager.IsInteractingWithLocation && !Player.IsShowingFrontEndMenus && Player.ActivityManager.IsPerformingActivity)
        {
            if (Player.ActivityManager.CanPauseCurrentActivity && !Player.ActivityManager.IsCurrentActivityPaused)
            {
                AttemptAddPrompt("ActivityControlPause", Player.ActivityManager.PauseCurrentActivityPrompt, "ActivityControlPause", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 998, PauseCurrentActivityAction);
            }
            else
            {
                RemovePrompts("ActivityControlPause");
            }
            if (Player.ActivityManager.CanCancelCurrentActivity)
            {
                AttemptAddPrompt("ActivityControlCancel", Player.ActivityManager.CancelCurrentActivityPrompt, "ActivityControlCancel", Settings.SettingsManager.KeySettings.InteractCancel, 999, CancelCurrentActivityAction);
            }
            else
            {
                RemovePrompts("ActivityControlCancel");
            }
            RemovePrompts("ActivityControlContinue");
        }
        else
        {
            if(Player.ActivityManager.PausedActivites.Any(x=>x.IsPaused()))
            {
                AttemptAddPrompt("ActivityControlContinue", "Continue An Activity", "ActivityControlContinue", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 998, ContinueCurrentActivityAction);
            }
            else
            {
                RemovePrompts("ActivityControlContinue");
                RemovePrompts("ActivityControlCancel");
            }
            RemovePrompts("ActivityControlCancel");
            RemovePrompts("ActivityControlPause");
        }
    }
    public void RemoveActivityPrompts()
    {
        RemovePrompts("ActivityControlContinue");
        RemovePrompts("ActivityControlCancel");
        RemovePrompts("ActivityControlPause");
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
    private void AttemptAddConversationPrompts()
    {
        if (!Player.ActivityManager.IsInteractingWithLocation && !Player.IsShowingFrontEndMenus && !addedPromptGroup && !Player.ActivityManager.IsInteracting && Player.ActivityManager.CanConverseWithLookedAtPed && Settings.SettingsManager.ActivitySettings.AllowPedConversations)
        {
            PersonConversationPrompts();
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
        if(!Player.ActivityManager.IsInteractingWithLocation && !Player.IsShowingFrontEndMenus && Player.ActivityManager.CanRecruitLookedAtGangMember && !Player.ActivityManager.IsConversing)
        {
            PersonRecruitingPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Prompts.RemoveAll(x => x.Group == "Recruit");
        }
        if (!Player.ActivityManager.IsInteractingWithLocation && !Player.IsShowingFrontEndMenus && !addedPromptGroup)
        {
            if (Player.CurrentLookedAtObject.Exists() && Player.CanSitOnCurrentLookedAtObject && Player.ActivityManager.CanPerformActivitiesExtended && !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivitiesExtended && !Player.ActivityManager.IsSitting && !Player.IsInVehicle)
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
        if (!Player.ActivityManager.IsInteractingWithLocation && !Player.IsShowingFrontEndMenus && !addedPromptGroup)
        {
            if (Player.ActivityManager.CanInspectLookedAtPed && Settings.SettingsManager.ActivitySettings.AllowPedInspecting)
            {
                PersonInspectingPrompts();
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

        //if(Player.ActivityManager.CanGrabLookedAtPed || Player.ActivityManager.CanGrabPed)
        //{
        //    EntryPoint.WriteToConsole($"CanGrabLookedAtPed{Player.ActivityManager.CanGrabLookedAtPed} CanGrabPed{Player.ActivityManager.CanGrabPed}");
        //}

        if (Settings.SettingsManager.KeySettings.GrabPedGameControl >= 0 && !Player.ActivityManager.IsInteractingWithLocation && !Player.IsShowingFrontEndMenus && Player.ActivityManager.CanGrabLookedAtPed && Settings.SettingsManager.ActivitySettings.AllowGrabbingPeds)
        {
            PersonGrabPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Prompts.RemoveAll(x => x.Group == "Grab");
        }
        if (!Player.ActivityManager.IsInteractingWithLocation && !Player.IsShowingFrontEndMenus && Player.IsWanted && Player.AnyPoliceRecentlySeenPlayer && Player.ClosestPoliceDistanceToPlayer <= 40f && Player.IsAliveAndFree && !Player.PoliceResponse.IsWeaponsFree && Player.Surrendering.CanSurrender)
        {
            AddPrompt("ShowSurrender", "Surrender", "ShowSurrender", Settings.SettingsManager.KeySettings.SurrenderKeyModifier, Settings.SettingsManager.KeySettings.SurrenderKey, 999);
        }
        else
        {
            RemovePrompts("ShowSurrender");
        }
        if(!Player.ActivityManager.IsInteractingWithLocation && !Player.IsShowingFrontEndMenus && Player.Surrendering.HandsAreUp && Player.IsAliveAndFree)
        {
            AddPrompt("ShowStopSurrender", "Stop Surrendering", "ShowStopSurrender", Settings.SettingsManager.KeySettings.SurrenderKeyModifier, Settings.SettingsManager.KeySettings.SurrenderKey, 999);
        }
        else
        {
            RemovePrompts("ShowStopSurrender");
        }



        if (Settings.SettingsManager.PlayerOtherSettings.AllowYellGetDownPrompt && Player.CurrentLookedAtPed == null && !Player.ActivityManager.IsInteractingWithLocation && !Prompts.Any(x=> x.Identifier != "YellGetDown") && !Player.IsShowingFrontEndMenus && !Player.AnyPoliceRecentlySeenPlayer && Player.ClosestPoliceDistanceToPlayer > 40f && Player.IsAliveAndFree && Player.IsAiming && Player.IsOnFoot)
        {
            AddPrompt("YellGetDown", "Force All Down", "YellGetDown", Settings.SettingsManager.KeySettings.YellKeyModifier, Settings.SettingsManager.KeySettings.YellKey, 999);
        }
        else
        {
            RemovePrompts("YellGetDown");
        }


        if (Settings.SettingsManager.GroupSettings.EnableGroupButtonPrompts && Player.GroupManager.CurrentGroupMembers.Any() && !Player.ActivityManager.IsInteractingWithLocation && !Player.IsShowingFrontEndMenus && Player.IsAliveAndFree)
        {
            AddPrompt("ToggleGroupMode", "Toggle Group Mode", "ToggleGroupMode", Settings.SettingsManager.KeySettings.GroupModeToggleKeyModifier, Settings.SettingsManager.KeySettings.GroupModeToggleKey, 999);
        }
        else
        {
            RemovePrompts("ToggleGroupMode");
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
        if (!addedPromptGroup && !Player.IsShowingFrontEndMenus && CanInteractWithClosestLocation)
        {
            LocationInteractingPrompts();
            addedPromptGroup = true;
        }
        else
        {
            Prompts.RemoveAll(x => x.Group == "InteractableLocation");
        }


        if (!Player.ActivityManager.IsInteractingWithLocation && !Player.IsShowingFrontEndMenus && !addedPromptGroup && !Player.ActivityManager.IsInteracting && Player.ActivityManager.CanPerformActivitiesExtended && Player.IsNearScenario && Settings.SettingsManager.ActivitySettings.AllowStartingScenarios)//currently isnearscenario is turned off
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

