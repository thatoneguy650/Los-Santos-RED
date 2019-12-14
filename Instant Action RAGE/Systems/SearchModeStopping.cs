using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class SearchModeStopping
{
    private static Model CopModel;
    private static bool SingleStopActive = false;
    private static Ped GhostCop;
    public static bool StopSearchMode { get; set; }
    public static bool IsRunning { get; set; }
    public static Ped SpotterCop
    {
        get
        {
            return GhostCop;
        }
    }
    public static void Initialize()
    {
        CopModel = new Model("s_m_y_cop_01");
        SingleStopActive = false;
        GhostCop = null;
        StopSearchMode = false;
        IsRunning = true;
        CopModel.LoadAndWait();
        CopModel.LoadCollisionAndWait();
        MainLoop();
    }
    private static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    if (StopSearchMode)
                        HauntPlayer();
                    GameFiber.Sleep(50);
                }
            }
            catch (Exception e)
            {
                InstantAction.Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void StopSearchModeSingle()
    {
        if (SingleStopActive)
            return;

        SingleStopActive = true;
        Debugging.WriteToLog("StopSearchModeSingle", "Started Stop Search Mod Single");
        GameFiber StopSearchSingle = GameFiber.StartNew(delegate
        {
            try
            {

                if (!GhostCop.Exists())
                {
                    CreateGhostCop();
                }
                while (Police.PlayerStarsGreyedOut)
                {
                    MoveGhostCopToPosition();
                    GameFiber.Sleep(50);
                }
                MoveGhostCopToOrigin();
                SingleStopActive = false;
            }
            catch (Exception e)
            {
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        }, "StopSearchSingle");

        Debugging.GameFibers.Add(StopSearchSingle);
    }
    private static void HauntPlayer()
    {
        if (InstantAction.PlayerInVehicle)
            return;
        if (!GhostCop.Exists())
        {
            CreateGhostCop();
        }
        if (Police.AnyPoliceRecentlySeenPlayer)// Needed for the AI to keep the player in the wanted position
        {
            MoveGhostCopToPosition();
        }
        else
        {
            MoveGhostCopToOrigin();
        }
    }
    private static void MoveGhostCopToPosition()
    {
        if (GhostCop != null)
        {
            Vector3 DesiredPosition;
            if (Police.PlayerStarsGreyedOut)
                DesiredPosition = Game.LocalPlayer.Character.Position.Around2D(1,5);//Must not be working, move them around
            else
                DesiredPosition = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 1f));// the standard spot to move them to

            Vector3 PlacedPosition = Vector3.Zero;
            bool FoundPlace;
            unsafe
            {
                FoundPlace = NativeFunction.CallByName<bool>("GET_SAFE_COORD_FOR_PED", DesiredPosition.X, DesiredPosition.Y, DesiredPosition.Z, false, &PlacedPosition, 16);
            }

            if (FoundPlace)
                GhostCop.Position = PlacedPosition;
            else
                GhostCop.Position = DesiredPosition;

            Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, GhostCop.Position);
            GhostCop.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
        }
    }
    private static void MoveGhostCopToOrigin()
    {
        if (GhostCop != null)
            GhostCop.Position = new Vector3(0f, 0f, 0f);
    }
    private static void CreateGhostCop()
    {
        GhostCop = new Ped(CopModel, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 0f)), Game.LocalPlayer.Character.Heading)
        {
            BlockPermanentEvents = false,
            IsPersistent = true,
            IsCollisionEnabled = false,
            IsVisible = false
        };
        Blip myBlip = GhostCop.GetAttachedBlip();
        if (myBlip != null)
            myBlip.Delete();
        GhostCop.VisionRange = 100f;
        GhostCop.HearingRange = 100f;
        GhostCop.CanRagdoll = false;
        const ulong SetPedMute = 0x7A73D05A607734C7;
        NativeFunction.CallByHash<uint>(SetPedMute, GhostCop);
        NativeFunction.CallByName<bool>("STOP_PED_SPEAKING", GhostCop, true);
        NativeFunction.CallByName<uint>("SET_PED_CONFIG_FLAG", GhostCop, 69, true);
        NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", GhostCop, (uint)2725352035, true); //Unequip weapon so you don't get shot
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", GhostCop, false);
        NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", GhostCop, 0f);
    }
}

