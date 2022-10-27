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
    [Description("Use the simplified conversation system (Similar to RDR2).")]
    public bool UseSimpleConversation { get; set; }
    [Description("Allow the player to loot dead or unconscious peds.")]
    public bool AllowPedLooting { get; set; }
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



    public bool UseAltCameraWhenSitting { get; set; }
    public bool ForceSitWhenClose { get; set; }
    public uint ForceSitTimeOut { get; set; }
    public float ForceSitDistance { get; set; }
    public float SittingSlideDistance { get; set; }




    public float ShovelAnimationStopTime { get; set; }
    public bool ShovelFadeOut { get; set; }
    public bool ShovelUseAltCamera { get; set; }
    public float ShovelHoleOffsetX { get; set; }
    public float ShovelHoleOffsetY { get; set; }
    public float ShovelStartOffsetX { get; set; }
    public float ShovelStartOffsetY { get; set; }
    public float ShovelCameraOffsetX { get; set; }
    public float ShovelCameraOffsetY { get; set; }
    public float ShovelCameraOffsetZ { get; set; }
    public bool ShovelDebugDrawMarkers { get; set; }

    public ActivitySettings()
    {
        SetDefault();

    }
    public void SetDefault()
    {
        TeleportWhenSitting = false;
        AllowPedConversations = true;
        AllowPedLooting = true;
        AllowDraggingOtherPeds = true;
        AllowTakingOtherPedsHostage = true;
        AllowStartingScenarios = false;
        AllowPlayerCrouching = true;
        PlayDraggingPedAnimation = false;
        UseSimpleConversation = true;
        CrouchingAdjustsMovementSpeed = true;
        CrouchMovementSpeedOverride = 5.0f;





        UseAltCameraWhenSitting = false;

        ForceSitWhenClose = true;
        ForceSitTimeOut = 3000;
        ForceSitDistance = 0.7f;
        SittingSlideDistance = 0.5f;//0.1f

        ShovelAnimationStopTime = 0.7f;
        ShovelFadeOut = true;
        ShovelUseAltCamera = true;


        ShovelHoleOffsetX = -1.0f;
        ShovelHoleOffsetY = 0.0f;


        ShovelStartOffsetX = 0.0f;
        ShovelStartOffsetY = 1.0f;

        ShovelCameraOffsetX = 2.0f;
        ShovelCameraOffsetY = 3.5f;
        ShovelCameraOffsetZ = 0.5f;




        ShovelDebugDrawMarkers = false;
    }
}