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
    [Description("Attempt to set any blocking object as no collision with the player when sitting down. Used to stop from tipping over tables.")]
    public bool SetNoTableCollisionWhenSitting { get; set; }
    [Description("If disabled, the gesture menu will stay open for you to continually use gestures.")]
    public bool CloseMenuOnGesture { get; set; }


    [Description("Allow the player to start converstaions with random peds.")]
    public bool AllowPedConversations { get; set; }
    [Description("Allow the player to loot dead or unconscious peds.")]
    public bool AllowPedLooting { get; set; }
    [Description("Allow the player to drag dead or unconscious peds.")]
    public bool AllowDraggingOtherPeds { get; set; }
    [Description("Allow the player to hold a ped hostage at gunpoint.")]
    public bool AllowTakingOtherPedsHostage { get; set; }
    [Description("Allow the player to start random scenarios around the world.")]
    public bool AllowStartingScenarios { get; set; }
    [Description("Allow the player to go into crouch mode.")]
    public bool AllowPlayerCrouching { get; set; }

    public ActivitySettings()
    {
        SetDefault();
#if DEBUG
        AllowTakingOtherPedsHostage = true;
#endif
    }
    public void SetDefault()
    {
        TeleportWhenSitting = false;
        SetNoTableCollisionWhenSitting = true;
        CloseMenuOnGesture = false;

        AllowPedConversations = true;
        AllowPedLooting = true;
        AllowDraggingOtherPeds = true;
        AllowTakingOtherPedsHostage = false;
        AllowStartingScenarios = false;
        AllowPlayerCrouching = true;

    }
}