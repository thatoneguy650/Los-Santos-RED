using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Windows.Forms;

public class MoveInteraction
{
    private ILocationInteractable Player;

    private string PlayingDict;
    private string PlayingAnim;
    private bool IsCancelled;
    private Vector3 FinalPlayerPosition;
    private float FinalPlayerHeading;

    public MoveInteraction(ILocationInteractable player, Vector3 finalPlayerPosition, float finalPlayerHeading)
    {
        Player = player;
        FinalPlayerPosition = finalPlayerPosition;
        FinalPlayerHeading = finalPlayerHeading;
    }
    public float CloseDistance { get; set; } = 0.35f;
    public float CloseHeading { get; set; } = 0.5f;
    public bool MoveToMachine(float speed)
    {
        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, FinalPlayerPosition.X, FinalPlayerPosition.Y, FinalPlayerPosition.Z, speed, -1, 0.25f, 0, FinalPlayerHeading);
        uint GameTimeStartedSitting = Game.GameTime;
        float heading = Game.LocalPlayer.Character.Heading;
        bool IsFacingDirection = false;
        bool IsCloseEnough = false;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsCloseEnough && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(FinalPlayerPosition) < CloseDistance;
            //Game.DisplaySubtitle($"Distance: {Game.LocalPlayer.Character.DistanceTo2D(PropEntryPosition)} IsCloseEnough{IsCloseEnough}");
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        GameTimeStartedSitting = Game.GameTime;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsFacingDirection && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            heading = Game.LocalPlayer.Character.Heading;
            if (Math.Abs(ExtensionsMethods.Extensions.GetHeadingDifference(heading, FinalPlayerHeading)) <= CloseHeading)//0.5f)
            {
                IsFacingDirection = true;
            }
            //Game.DisplaySubtitle($"Current Heading: {heading} PropEntryHeading: {PropEntryHeading}");
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        if (IsCloseEnough && IsFacingDirection && !IsCancelled)
        {
            EntryPoint.WriteToConsole("MOVE TO MACHINE, CLOSE ENOUGH");
            return true;
        }
        else
        {
            EntryPoint.WriteToConsole("MOVE TO MACHINE, NOT CLOSE");
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            return false;
        }
    }
}

