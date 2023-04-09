using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ActivitySettings : ISettingsDefaultable
{

    [Description("Will teleport to the sitting entry point instead of walking. Useful when there are objects in the way like a large table you dont want to hit.")]
    public bool TeleportWhenSitting { get; set; }
    [Description("Allow the player to start converstaions with random peds.")]
    public bool AllowPedConversations { get; set; }
    [Description("Allow the player to loot dead or unconscious peds.")]
    public bool AllowPedLooting { get; set; }
    [Description("Allow the player to hold up peds.")]
    public bool AllowPedHoldUps { get; set; }
    [Description("Maximum distance in meters that you can hold up a ped.")]
    public float HoldUpDistance { get; set; }
    [Description("Allow the player to drag dead or unconscious peds.")]
    public bool AllowDraggingOtherPeds { get; set; }
    [Description("Plays an animation the dragged ped.")]
    public bool PlayDraggingPedAnimation { get; set; }
    [Description("Allow the player to hold a ped hostage at gunpoint.")]
    public bool AllowTakingOtherPedsHostage { get; set; }
    [Description("Allow the player to start random scenarios around the world.")]
    public bool AllowStartingScenarios { get; set; }
    [Description("Allow the player to go into crouch mode.")]
    public bool AllowPlayerCrouching { get; set; }
    [Description("Change player movement when in crouch mode.")]
    public bool CrouchingAdjustsMovementSpeed { get; set; }
    [Description("Amount of override player movement when crouching.")]
    public float CrouchMovementSpeedOverride { get; set; }
    [Description("Set a cinematic camera when sitting")]
    public bool UseAltCameraWhenSitting { get; set; }
    [Description("Force sitting when close to a seat")]
    public bool ForceSitWhenClose { get; set; }
    [Description("Time (in ms) before force sit kicks in")]
    public uint ForceSitTimeOut { get; set; }
    [Description("Distance (in meters) before force sit kicks in")]
    public float ForceSitDistance { get; set; }
    [Description("Distance (in meters) to slide when sitting")]
    public float SittingSlideDistance { get; set; }
    [Description("Will add an exclamation point to the prompt if the ped wants to buy an item you have")]
    public bool ShowInPromptWhenPedsWantToBuyItemsYouHave { get; set; }
    public float PlateTheftFloat { get; set; }
    public uint DrinkTimeBetween { get; set; }
    public bool DrinkStartsBase { get; set; }
    public ActivitySettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        TeleportWhenSitting = false;
        AllowPedConversations = true;
        AllowPedLooting = true;
        AllowPedHoldUps = true;
        AllowDraggingOtherPeds = true;
        AllowTakingOtherPedsHostage = true;
        AllowStartingScenarios = false;
        AllowPlayerCrouching = true;
        PlayDraggingPedAnimation = true;
        CrouchingAdjustsMovementSpeed = true;
        CrouchMovementSpeedOverride = 5.0f;
        UseAltCameraWhenSitting = false;
        ForceSitWhenClose = true;
        ForceSitTimeOut = 3000;
        ForceSitDistance = 0.7f;
        SittingSlideDistance = 0.5f;//0.1f
        ShowInPromptWhenPedsWantToBuyItemsYouHave = true;
        HoldUpDistance = 15f;
        PlateTheftFloat = 1.0f;
        DrinkTimeBetween = 0;
        DrinkStartsBase = false;
    }
}