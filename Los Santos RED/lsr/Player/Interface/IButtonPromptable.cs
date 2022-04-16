using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IButtonPromptable
    {
        PedExt CurrentLookedAtPed { get; }
        List<ButtonPrompt> ButtonPromptList { get; }
       // GameLocation ClosestSimpleTransaction { get; }
        bool CanConverseWithLookedAtPed { get; }
        bool IsInteracting { get; }
        bool IsInteractingWithLocation { get; }
        InteractableLocation ClosestInteractableLocation { get; }
        bool CanExitCurrentInterior { get; }
       // GameLocation CurrentInteriorLocation { get; }
        bool CanPerformActivities { get; }
        bool IsNearScenario { get; }
        Scenario ClosestScenario { get; }
        bool IsInVehicle { get; }
        bool IsNotWanted { get; }
        bool CanLootLookedAtPed { get; }
        bool CanGrabLookedAtPed { get; }
        float ClosestPoliceDistanceToPlayer { get; }
        bool AnyPoliceRecentlySeenPlayer { get; }
        bool CanDragLookedAtPed { get; }
        //GameLocation ClosestTeleportEntrance { get; }
    }
}
