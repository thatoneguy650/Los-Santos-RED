using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class SearchModeStopping
{
    private static Vector3 CurrentOffset = new Vector3(0f, 6f, 1f);
    private static bool PrevStopSearchMode;
    private static Vector3 PositionSet;
    private static Model CopModel;
    private static Ped GhostCop;
    private static uint GameTimeLastGhostCopCreated;
    private static bool StopSearchMode;
    public static bool IsRunning { get; set; }
    public static Ped SpotterCop
    {
        get
        {
            return GhostCop;
        }
    }
    public static bool CanCreateGhostCop
    {
        get
        {
            if (GameTimeLastGhostCopCreated == 0)
                return true;
            else if (Game.GameTime - GameTimeLastGhostCopCreated >= 4000)
                return true;
            else
                return false;
        }
    }
    public static void Initialize()
    {
        CopModel = new Model("s_m_y_cop_01");
        GhostCop = null;
        StopSearchMode = false;
        IsRunning = true;
        CopModel.LoadAndWait();
        CopModel.LoadCollisionAndWait();
        GameTimeLastGhostCopCreated = 0;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            if (PlayerState.IsWanted)
                StopSearchMode = true;
            else
                StopSearchMode = false;


            if (PrevStopSearchMode != StopSearchMode)
            {
                PrevStopSearchMode = StopSearchMode;
                Debugging.WriteToLog("StopSearchMode", string.Format("Changed To: {0}, AnyPoliceRecentlySeenPlayer {1}", StopSearchMode, Police.AnyRecentlySeenPlayer));
            }

            if (!StopSearchMode)
                return;

            if (!GhostCop.Exists())
            {
                CreateGhostCop();
            }
            if (PlayerState.IsWanted)// && Police.AnyRecentlySeenPlayer)// Needed for the AI to keep the player in the wanted position
            {
                MoveGhostCopToPosition();
            }
            else
            {
                MoveGhostCopToOrigin();
            }
        }
    }
    public static void Dispose()
    {
        IsRunning = false;
        if (GhostCop.Exists())
            GhostCop.Delete();
    }
    private static void MoveGhostCopToPosition()
    {
        if (GhostCop.Exists())
        {
            Entity ToCheck = PlayerState.IsInVehicle && Game.LocalPlayer.Character.CurrentVehicle.Exists() ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character;   
            if (!NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", GhostCop, ToCheck))
            {
                if(PlayerState.IsInVehicle)
                {
                    CurrentOffset = new List<Vector3>() { new Vector3(6f, 0f, 1f), new Vector3(3f, 0f, 1f), new Vector3(-3f, 0f, 1f) }.PickRandom();
                }
                else
                {
                    CurrentOffset = new List<Vector3>() { new Vector3(0f, 6f, 1f), new Vector3(6f, 0f, 1f), new Vector3(0f, -6f, 1f)
                                                        , new Vector3(0f, -3f, 1f), new Vector3(0f, 3f, 1f), new Vector3(3f, 0f, 1f)
                                                        , new Vector3(-3f, 0f, 1f) , new Vector3(-3f, 3f, 1f), new Vector3(-3f, -3f, 1f)


                                                        }.PickRandom();
                }
                Debugging.WriteToLog("MoveGhostCopToPosition", string.Format("CurrentOffset {0}", CurrentOffset));
            }
            Vector3 DesiredPosition = Game.LocalPlayer.Character.GetOffsetPosition(CurrentOffset);
            PositionSet = DesiredPosition;
            GhostCop.Position = PositionSet;

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
        GhostCop = new Ped(CopModel, new Vector3(0f, 0f, 0f), 0f);
        GameTimeLastGhostCopCreated = Game.GameTime;
        if (GhostCop.Exists())
        {
            GhostCop.BlockPermanentEvents = false;
            GhostCop.IsPersistent = true;
            GhostCop.IsCollisionEnabled = false;
            GhostCop.IsVisible = false;
        
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
}

