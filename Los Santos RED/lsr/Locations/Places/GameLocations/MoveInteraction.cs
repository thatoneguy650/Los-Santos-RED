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

    public uint TimeLimit { get; set; } = 5000;
    public uint TimeGrace { get; set; } = 3000;
    public float DistanceGrace { get; set; } = 0.4f;
    public float HeadingGrace { get; set; } = 2.0f;
    public bool MoveToMachine(float speed)
    {
        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, FinalPlayerPosition.X, FinalPlayerPosition.Y, FinalPlayerPosition.Z, speed, -1, 0.25f, 0, FinalPlayerHeading);
        uint GameTimeStarted = Game.GameTime;
        float heading = Game.LocalPlayer.Character.Heading;
        bool IsFacingDirection = false;
        bool IsCloseEnough = false;
        while (Game.GameTime - GameTimeStarted <= TimeLimit && !IsCloseEnough && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            float currentDistanceAway = Game.LocalPlayer.Character.DistanceTo2D(FinalPlayerPosition);
            IsCloseEnough = currentDistanceAway < CloseDistance;

            if(currentDistanceAway <= CloseDistance + DistanceGrace && Game.GameTime - GameTimeStarted >= TimeGrace)
            {
                IsCloseEnough = true;
            }

           // Game.DisplaySubtitle($"Distance: {Game.LocalPlayer.Character.DistanceTo2D(FinalPlayerPosition)} IsCloseEnough{IsCloseEnough}");
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        GameTimeStarted = Game.GameTime;
        while (Game.GameTime - GameTimeStarted <= TimeLimit && !IsFacingDirection && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            heading = Game.LocalPlayer.Character.Heading;

            float currentHeadingDifference = Math.Abs(ExtensionsMethods.Extensions.GetHeadingDifference(heading, FinalPlayerHeading));
            if (currentHeadingDifference <= CloseHeading)//0.5f)
            {
                IsFacingDirection = true;
            }
            if(currentHeadingDifference <= CloseHeading + HeadingGrace && Game.GameTime - GameTimeStarted >= TimeGrace)
            {
                IsFacingDirection = true;
            }
           // Game.DisplaySubtitle($"Current Heading: {heading} PropEntryHeading: {FinalPlayerHeading}");
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

