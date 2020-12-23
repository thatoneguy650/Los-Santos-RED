using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LosSantosRED.lsr
{
    public class SearchMode
    {
        private Player CurrentPlayer;
        private bool PrevIsInSearchMode;
        private bool PrevIsInActiveMode;
        private uint GameTimeStartedSearchMode;
        private uint GameTimeStartedActiveMode;
        private StopVanillaSeachMode StopSearchMode = new StopVanillaSeachMode();

        public SearchMode(Player currentPlayer)
        {
            CurrentPlayer = currentPlayer;
        }
        public bool IsInSearchMode { get; private set; }
        public bool IsInActiveMode { get; private set; }
        public uint TimeInSearchMode
        {
            get
            {
                if (IsInSearchMode)
                {
                    if (GameTimeStartedSearchMode == 0)
                        return 0;
                    else
                        return (Game.GameTime - GameTimeStartedSearchMode);
                }
                else
                {
                    return 0;
                }

            }
        }
        public uint TimeInActiveMode
        {
            get
            {
                if (IsInActiveMode)
                {
                    return (Game.GameTime - GameTimeStartedActiveMode);
                }
                else
                {
                    return 0;
                }

            }
        }
        public uint CurrentSearchTime
        {
            get
            {
                return (uint)Mod.Player.WantedLevel * 30000;//30 seconds each
            }
        }
        public uint CurrentActiveTime
        {
            get
            {
                return (uint)Mod.Player.WantedLevel * 30000;//30 seconds each
            }
        }
        public bool IsSpotterCop(uint Handle)
        {
            if (StopSearchMode.SpotterCop != null && StopSearchMode.SpotterCop.Handle == Handle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void UpdateWanted()
        {
            DetermineMode();
            ToggleModes();
            //HandleFlashing();
        }
        public void StopVanilla()
        {
            StopSearchMode.Tick();
        }
        private void DetermineMode()
        {
            if (CurrentPlayer.IsWanted)
            {
                if (Mod.World.AnyPoliceRecentlySeenPlayer)
                {
                    IsInActiveMode = true;
                    IsInSearchMode = false;
                }
                else
                {
                    IsInActiveMode = false;
                    IsInSearchMode = true;
                }


                if (IsInSearchMode && TimeInSearchMode >= CurrentSearchTime)
                {
                    IsInActiveMode = false;
                    IsInSearchMode = false;
                }
            }
            else
            {
                IsInActiveMode = false;
                IsInSearchMode = false;
            }
        }
        private void ToggleModes()
        {
            if (PrevIsInActiveMode != IsInActiveMode)
            {
                if (IsInActiveMode)
                {
                    StartActiveMode();
                }
            }

            if (PrevIsInSearchMode != IsInSearchMode)
            {
                if (IsInSearchMode)
                {
                    StartSearchMode();
                }
                else
                {
                    EndSearchMode();
                }
            }
        }
        private void StartSearchMode()
        {
            IsInActiveMode = false;
            IsInSearchMode = true;
            PrevIsInSearchMode = IsInSearchMode;
            PrevIsInActiveMode = IsInActiveMode;
            GameTimeStartedSearchMode = Game.GameTime;
            GameTimeStartedActiveMode = 0;
            Mod.Debug.WriteToLog("SearchMode", "Start Search Mode");
        }
        private void StartActiveMode()
        {
            IsInActiveMode = true;
            IsInSearchMode = false;
            PrevIsInSearchMode = IsInSearchMode;
            PrevIsInActiveMode = IsInActiveMode;
            GameTimeStartedActiveMode = Game.GameTime;
            GameTimeStartedSearchMode = 0;
            Mod.Debug.WriteToLog("SearchMode", "Start Active Mode");
        }
        private void EndSearchMode()
        {
            IsInActiveMode = false;
            IsInSearchMode = false;
            PrevIsInSearchMode = IsInSearchMode;
            PrevIsInActiveMode = IsInActiveMode;
            GameTimeStartedSearchMode = 0;
            GameTimeStartedActiveMode = 0;
            CurrentPlayer.CurrentPoliceResponse.SetWantedLevel(0, "Search Mode Timeout", true);
            Mod.Debug.WriteToLog("SearchMode", "Stop Search Mode");

        }
        private class StopVanillaSeachMode
        {
            private Vector3 CurrentOffset = new Vector3(0f, 6f, 1f);
            private bool PrevStopSearchMode;
            private Vector3 PositionSet;
            private Model CopModel = new Model("s_m_y_cop_01");
            private Ped GhostCop;
            private uint GameTimeLastGhostCopCreated;
            private bool StopSearchMode;
            public Ped SpotterCop
            {
                get
                {
                    return GhostCop;
                }
            }
            public bool CanCreateGhostCop
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
            public StopVanillaSeachMode()
            {
                CopModel.LoadAndWait();
                CopModel.LoadCollisionAndWait();
            }
            public void Tick()
            {
                if (Mod.Player.IsWanted)
                    StopSearchMode = true;
                else
                    StopSearchMode = false;


                if (PrevStopSearchMode != StopSearchMode)
                {
                    PrevStopSearchMode = StopSearchMode;
                    Mod.Debug.WriteToLog("StopSearchMode", string.Format("Changed To: {0}, AnyPoliceRecentlySeenPlayer {1}", StopSearchMode, Mod.World.AnyPoliceRecentlySeenPlayer));
                }

                if (!StopSearchMode)
                    return;

                if (!GhostCop.Exists())
                {
                    CreateGhostCop();
                }
                if (Mod.Player.IsWanted)// && Police.AnyRecentlySeenPlayer)// Needed for the AI to keep the player in the wanted position
                {
                    MoveGhostCopToPosition();
                }
                else
                {
                    MoveGhostCopToOrigin();
                }
            }
            private void MoveGhostCopToPosition()
            {
                if (GhostCop.Exists())
                {
                    Entity ToCheck = Mod.Player.IsInVehicle && Game.LocalPlayer.Character.CurrentVehicle.Exists() ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character;
                    if (!NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", GhostCop, ToCheck))
                    {
                        if (Mod.Player.IsInVehicle)
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
                        Mod.Debug.WriteToLog("MoveGhostCopToPosition", string.Format("CurrentOffset {0}", CurrentOffset));
                    }
                    Vector3 DesiredPosition = Game.LocalPlayer.Character.GetOffsetPosition(CurrentOffset);
                    PositionSet = DesiredPosition;
                    GhostCop.Position = PositionSet;

                    Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, GhostCop.Position);
                    GhostCop.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
                }
            }
            private void MoveGhostCopToOrigin()
            {
                if (GhostCop != null)
                    GhostCop.Position = new Vector3(0f, 0f, 0f);
            }
            private void CreateGhostCop()
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
    }
}