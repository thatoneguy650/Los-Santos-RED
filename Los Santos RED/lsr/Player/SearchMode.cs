using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
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
    public class SearchMode : ISearchMode
    {
        private IWorld World;
        private IPlayer CurrentPlayer;
        private IPolice Police;
        private bool areStarsGreyedOut;
        private bool PrevIsInSearchMode;
        private bool PrevIsInActiveMode;
        private uint GameTimeStartedSearchMode;
        private uint GameTimeStartedActiveMode;
        private uint GameTimeLastStarsGreyedOut;
        private uint GameTimeLastStarsNotGreyedOut;
        private StopVanillaSeachMode StopSearchMode = new StopVanillaSeachMode();

        public SearchMode(IWorld world, IPlayer currentPlayer, IPolice police)
        {
            World = world;
            CurrentPlayer = currentPlayer;
            Police = police;
        }
        public bool AreStarsGreyedOut
        {
            get => areStarsGreyedOut;
            private set
            {
                if (areStarsGreyedOut != value)
                {
                    areStarsGreyedOut = value;
                    AreStarsGreyedOutChanged();
                }
            }
        }
        public Color BlipColor
        {
            get
            {
                if (IsInActiveMode)
                {
                    return Color.Red;
                }
                else
                {
                    return Color.Orange;
                }
            }
        }
        public float BlipSize//probably gonna remove these both and have a static size or do something else.
        {
            get
            {
                if (IsInActiveMode)
                {
                    return 100f;
                }
                else
                {
                    if (CurrentSearchTime == 0)
                    {
                        return 100f;
                    }
                    //else
                    //{
                    //    return ArrestWarrant.SearchRadius * SearchMode.TimeInSearchMode / SearchMode.CurrentSearchTime;
                    //}
                }
                return 100f;
            }
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
                return (uint)CurrentPlayer.WantedLevel * 30000;//30 seconds each
            }
        }
        public uint CurrentActiveTime
        {
            get
            {
                return (uint)CurrentPlayer.WantedLevel * 30000;//30 seconds each
            }
        }
        public string SearchModeDebug => string.Format("IsInSearchMode {0} IsInActiveMode {1}, TimeInSearchMode {2}, TimeInActiveMode {3}", IsInSearchMode, IsInActiveMode, TimeInSearchMode, TimeInActiveMode);
        public bool StarsRecentlyActive => GameTimeLastStarsNotGreyedOut != 0 && Game.GameTime - GameTimeLastStarsNotGreyedOut <= 1500;
        public bool StarsRecentlyGreyedOut => GameTimeLastStarsGreyedOut != 0 && Game.GameTime - GameTimeLastStarsGreyedOut <= 1500;
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
            CurrentPlayer.IsInSearchMode = IsInSearchMode;
            //HandleFlashing();
        }
        public void StopVanilla()
        {
            StopSearchMode.Tick(CurrentPlayer.IsWanted,CurrentPlayer.IsInVehicle);
        }
        private void DetermineMode()
        {
            if (CurrentPlayer.IsWanted)
            {
                if (Police.AnyRecentlySeenPlayer)
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
            Game.Console.Print("SearchMode Start Search Mode");
        }
        private void StartActiveMode()
        {
            IsInActiveMode = true;
            IsInSearchMode = false;
            PrevIsInSearchMode = IsInSearchMode;
            PrevIsInActiveMode = IsInActiveMode;
            GameTimeStartedActiveMode = Game.GameTime;
            GameTimeStartedSearchMode = 0;
            Game.Console.Print("SearchMode Start Active Mode");
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
            Game.Console.Print("SearchMode Stop Search Mode");

        }
        private void AreStarsGreyedOutChanged()
        {
            if (AreStarsGreyedOut)
            {
                GameTimeLastStarsGreyedOut = Game.GameTime;
            }
            else
            {
                GameTimeLastStarsNotGreyedOut = Game.GameTime;
            }
            Game.Console.Print(string.Format("AreStarsGreyedOut Changed to: {0}", AreStarsGreyedOut));
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
            public void Tick(bool IsWanted, bool TargetIsInVehicle)
            {
                if (IsWanted)
                    StopSearchMode = true;
                else
                    StopSearchMode = false;


                if (PrevStopSearchMode != StopSearchMode)
                {
                    PrevStopSearchMode = StopSearchMode;
                }

                if (!StopSearchMode)
                    return;

                if (!GhostCop.Exists())
                {
                    CreateGhostCop();
                }
                if (IsWanted)// && Police.AnyRecentlySeenPlayer)// Needed for the AI to keep the player in the wanted position
                {
                    MoveGhostCopToPosition(TargetIsInVehicle);
                }
                else
                {
                    MoveGhostCopToOrigin();
                }
            }
            private void MoveGhostCopToPosition(bool TargetIsInVehicle)
            {
                if (GhostCop.Exists())
                {
                    Entity ToCheck = TargetIsInVehicle && Game.LocalPlayer.Character.CurrentVehicle.Exists() ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character;
                    if (!NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", GhostCop, ToCheck))
                    {
                        if (TargetIsInVehicle)
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
                        Game.Console.Print(string.Format("MoveGhostCopToPosition! CurrentOffset {0}", CurrentOffset));
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