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
    public class SearchMode
    {
        private IPoliceRespondable Player;
        private bool PrevIsInSearchMode;
        private bool PrevIsInActiveMode;
        private uint GameTimeStartedSearchMode;
        private uint GameTimeStartedActiveMode;
        private StopVanillaSeachMode StopSearchMode = new StopVanillaSeachMode();
        private ISettingsProvideable Settings;
        public bool IsActive { get; private set; } = true;
        public SearchMode(IPoliceRespondable currentPlayer, ISettingsProvideable settings)
        {
            Player = currentPlayer;
            Settings = settings;
        }
        public float SearchModePercentage => IsInSearchMode ? 1.0f - ((float)TimeInSearchMode / (float)CurrentSearchTime) : 0;
        public bool IsInSearchMode { get; private set; }
        public bool IsInActiveMode { get; private set; }
        public uint TimeInSearchMode => IsInSearchMode && GameTimeStartedSearchMode != 0 ? Game.GameTime - GameTimeStartedSearchMode : 0;
        public uint TimeInActiveMode => IsInActiveMode ? Game.GameTime - GameTimeStartedActiveMode : 0;
        public uint CurrentSearchTime => (uint)Player.WantedLevel * Settings.SettingsManager.PlayerSettings.SearchMode_SearchTimeMultiplier;//30000;//30 seconds each
        public uint CurrentActiveTime => (uint)Player.WantedLevel * 30000;//30 seconds each
        public string SearchModeDebug => IsInSearchMode ? $"TimeInSearchMode: {TimeInSearchMode}, CurrentSearchTime: {CurrentSearchTime}" + $" SearchModePercentage: {SearchModePercentage}, DebugPos: {StopSearchMode.DebugPosition}" : $"TimeInActiveMode: {TimeInActiveMode}, CurrentActiveTime: {CurrentActiveTime}, DebugPos: {StopSearchMode.DebugPosition}";
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

            if (IsActive)
            {
                DetermineMode();
                ToggleModes();
                Player.IsInSearchMode = IsInSearchMode;
            }
        }
        public void Dispose()
        {
            IsActive = false;
            StopSearchMode.Dispose();
        }
        public void StopVanilla()
        {
            if(IsActive)
            {
                if (Settings.SettingsManager.PlayerSettings.SearchMode_FakeActiveWanted)
                {
                    StopSearchMode.Tick(Player.IsWanted, Player.IsInVehicle);
                }
            } 
        }
        private void DetermineMode()
        {
            if (Player.IsWanted && Player.HasBeenWantedFor >= 5000)
            {
                if (Player.AnyPoliceRecentlySeenPlayer)
                {
                    IsInActiveMode = true;
                    IsInSearchMode = false;
                }
                else
                {
                    if (IsInSearchMode && TimeInSearchMode >= CurrentSearchTime)
                    {
                        IsInActiveMode = false;
                        IsInSearchMode = false;
                        Player.OnSuspectEluded();
                    }
                    else
                    {
                        IsInActiveMode = false;
                        IsInSearchMode = true;
                    }
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
            Player.OnWantedSearchMode();
            EntryPoint.WriteToConsole("SearchMode Start Search Mode",3);
        }
        private void StartActiveMode()
        {
            IsInActiveMode = true;
            IsInSearchMode = false;
            PrevIsInSearchMode = IsInSearchMode;
            PrevIsInActiveMode = IsInActiveMode;
            GameTimeStartedActiveMode = Game.GameTime;
            GameTimeStartedSearchMode = 0;
            Player.OnWantedActiveMode();
            //EntryPoint.WriteToConsole("SearchMode Start Active Mode",3);
        }
        private void EndSearchMode()
        {
            IsInActiveMode = false;
            IsInSearchMode = false;
            PrevIsInSearchMode = IsInSearchMode;
            PrevIsInActiveMode = IsInActiveMode;
            GameTimeStartedSearchMode = 0;
            GameTimeStartedActiveMode = 0;
            Player.SetWantedLevel(0, "Search Mode Timeout", true);
            //EntryPoint.WriteToConsole("SearchMode End Search Mode", 3);
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

            private bool hasGamefiberRunning;
            public Ped SpotterCop
            {
                get
                {
                    return GhostCop;
                }
            }
            public bool CanCreateGhostCop => GameTimeLastGhostCopCreated == 0 || Game.GameTime - GameTimeLastGhostCopCreated >= 4000;
            public StopVanillaSeachMode()
            {
                CopModel.LoadAndWait();
                CopModel.LoadCollisionAndWait();
            }
            public Vector3 DebugPosition => PositionSet;
            public void Tick(bool IsWanted, bool TargetIsInVehicle)
            {
                if (IsWanted)
                {
                    StopSearchMode = true;
                }
                else
                {
                    StopSearchMode = false;
                }
                if (PrevStopSearchMode != StopSearchMode)
                {
                    PrevStopSearchMode = StopSearchMode;
                }

                if (!StopSearchMode)
                {
                    return;
                }
                if (!GhostCop.Exists())
                {
                    CreateGhostCop();
                    return;//new to split up the create and move/los call?
                }
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", GhostCop, 2725352035, true);
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", GhostCop, false);
                if (IsWanted)// && Police.AnyRecentlySeenPlayer)// Needed for the AI to keep the player in the wanted position
                {
                    MoveGhostCopToPosition(TargetIsInVehicle);
                }
                else
                {
                    MoveGhostCopToOrigin();
                }
            }
            public void Dispose()
            {
                if(GhostCop.Exists())
                {
                    GhostCop.Delete();
                    PositionSet = Vector3.Zero;
                }
            }
            private void MoveGhostCopToPosition(bool TargetIsInVehicle)
            {
                if (GhostCop.Exists())
                {
                    if (!hasGamefiberRunning)
                    {
                        GameFiber.StartNew(delegate
                        {
                            hasGamefiberRunning = true;
                            while (hasGamefiberRunning && GhostCop.Exists())
                            {
                                Entity ToCheck = TargetIsInVehicle && Game.LocalPlayer.Character.CurrentVehicle.Exists() ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character;
                                if (!NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", GhostCop, ToCheck))
                                {
                                    if (TargetIsInVehicle)
                                    {
                                        CurrentOffset = new List<Vector3>() { new Vector3(9f, 0f, 8f), new Vector3(6f, 0f, 8f), new Vector3(-6f, 0f, 8f) }.PickRandom();//CurrentOffset = new List<Vector3>() { new Vector3(9f, 0f, 2f), new Vector3(6f, 0f, 2f), new Vector3(-6f, 0f, 2f) }.PickRandom();
                                    }
                                    else
                                    {
                                        CurrentOffset = new List<Vector3>() { new Vector3(0f, 9f, 2f), new Vector3(9f, 0f, 2f), new Vector3(0f, -9f, 2f)
                                                        , new Vector3(0f, -6f, 2f), new Vector3(0f, 6f, 2f), new Vector3(6f, 0f, 2f)
                                                        , new Vector3(-6f, 0f, 2f) , new Vector3(-6f, 6f, 1f), new Vector3(-6f, -6f, 2f)


                                                            }.PickRandom();
                                    }
                                }
                                Vector3 DesiredPosition = Game.LocalPlayer.Character.GetOffsetPosition(CurrentOffset);
                                PositionSet = DesiredPosition;
                                GhostCop.Position = PositionSet;

                                Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, GhostCop.Position);
                                GhostCop.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
                                GameFiber.Sleep(100);
                            }
                            if(!GhostCop.Exists())
                            {
                                hasGamefiberRunning = false;
                            }
                        }, "Run Ghost Cop");

                    }







                   // Entity ToCheck = TargetIsInVehicle && Game.LocalPlayer.Character.CurrentVehicle.Exists() ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character;
                   // if (!NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", GhostCop, ToCheck))
                   // {
                   //     if (TargetIsInVehicle)
                   //     {
                   //         CurrentOffset = new List<Vector3>() { new Vector3(9f, 0f, 2f), new Vector3(6f, 0f, 2f), new Vector3(-6f, 0f, 2f) }.PickRandom();
                   //     }
                   //     else
                   //     {
                   //         CurrentOffset = new List<Vector3>() { new Vector3(0f, 9f, 2f), new Vector3(9f, 0f, 2f), new Vector3(0f, -9f, 2f)
                   //                                     , new Vector3(0f, -6f, 2f), new Vector3(0f, 6f, 2f), new Vector3(6f, 0f, 2f)
                   //                                     , new Vector3(-6f, 0f, 2f) , new Vector3(-6f, 6f, 1f), new Vector3(-6f, -6f, 2f)


                   //                                     }.PickRandom();
                   //     }
                   //     //if (TargetIsInVehicle)
                   //     //{
                   //     //    CurrentOffset = new List<Vector3>() { new Vector3(6f, 0f, 1f), new Vector3(3f, 0f, 1f), new Vector3(-3f, 0f, 1f) }.PickRandom();
                   //     //}
                   //     //else
                   //     //{
                   //     //    CurrentOffset = new List<Vector3>() { new Vector3(0f, 6f, 1f), new Vector3(6f, 0f, 1f), new Vector3(0f, -6f, 1f)
                   //     //                                , new Vector3(0f, -3f, 1f), new Vector3(0f, 3f, 1f), new Vector3(3f, 0f, 1f)
                   //     //                                , new Vector3(-3f, 0f, 1f) , new Vector3(-3f, 3f, 1f), new Vector3(-3f, -3f, 1f)


                   //     //                                }.PickRandom();
                   //     //}
                   //     //EntryPoint.WriteToConsole(string.Format("MoveGhostCopToPosition! CurrentOffset {0}", CurrentOffset));
                   // }
                   // Vector3 DesiredPosition = Game.LocalPlayer.Character.GetOffsetPosition(CurrentOffset);
                   // PositionSet = DesiredPosition;
                   //// GhostCop.Position = PositionSet;
                   // GhostCop.AttachTo(Game.LocalPlayer.Character, Game.LocalPlayer.Character.GetBoneIndex(PedBoneId.Head), DesiredPosition, Rotator.Zero);

                   // Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, GhostCop.Position);
                   // GhostCop.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
                }
            }
            private void MoveGhostCopToOrigin()
            {
                hasGamefiberRunning = false;
                if (GhostCop != null)
                {
                    GhostCop.Position = new Vector3(0f, 0f, 0f);
                }
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
                    GhostCop.MaxHealth = 1;
                    GhostCop.IsInvincible = true;
                    GhostCop.IsVisible = false;

                    //Blip myBlip = GhostCop.GetAttachedBlip();
                    //if (myBlip != null)
                    //{
                    //    myBlip.Delete();
                    //}
                    foreach(Blip blip in GhostCop.GetAttachedBlips())
                    {
                        if (blip != null)
                        {
                            blip.Delete();
                        }
                    }
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
                    NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", GhostCop, 272, true);//CPED_CONFIG_FLAG_DontBlipCop 
                    NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", GhostCop, 330, true);//CPED_CONFIG_FLAG_DontBlip 


                }
            }
        }
    }
}