using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;

namespace LosSantosRED.lsr.Player
{
    public class IntoxicatingEffect
    {
        private uint GameTimeStartedSwerving;
        private uint GameTimeToStopSwerving;
        private uint GameTimeUntilNextSwerve;
        private bool IsSwerving;
        private float SteeringBias;
        private string CurrentClipset;
        private uint GameTimeLastSetTimecycle;
        private uint GameTimeStartedIntoxicating;
        private uint GameTimeStoppedIntoxicating;   
        private uint IntoxicatingIntervalTime;
        private float MaxEffectAllowed;
        private IIntoxicatable Player;
        private uint SoberingIntervalTime;
        private string OverLayEffect;
        public IntoxicatingEffect(IIntoxicatable player, float maxEffectAllowed, uint intoxicatingIntervalTime, uint soberingIntervalTime, string overlay)
        {
            Player = player;
            MaxEffectAllowed = maxEffectAllowed;
            IntoxicatingIntervalTime = intoxicatingIntervalTime;
            SoberingIntervalTime = soberingIntervalTime;
            OverLayEffect = overlay;
        }
        public float CurrentIntensity => (float)((float)TotalTimeIntoxicated / IntoxicatingIntervalTime - (float)TotalTimeSober / SoberingIntervalTime).Clamp(0.0f, MaxEffectAllowed);
        public string DebugString => $"IsIntox {Player.IsIntoxicated} IsBeingIntoxicated {Player.IsPerformingActivity} I {CurrentIntensity} HBDF {HasBeenIntoxicatedFor} HBNDF {HasBeenNoIntoxicatedFor} TTD {TotalTimeIntoxicated} TTS {TotalTimeSober}";
        private string ClipsetAtCurrentIntensity
        {
            get
            {
                if (CurrentIntensity < 1.5)
                {
                    return "NONE";
                }
                else if (CurrentIntensity >= 2)
                {
                    return "move_m@drunk@moderatedrunk";
                }
                else if (CurrentIntensity >= 3)
                {
                    return "move_m@drunk@verydrunk";
                }
                else
                {
                    return "move_m@drunk@slightlydrunk";
                }
            }
        }
        private float SwerveAtCurrentIntensity
        {
            get
            {
                if (CurrentIntensity < 1.5)
                {
                    return 0.1f;
                }
                else if (CurrentIntensity >= 2)
                {
                    return 0.5f;
                }
                else if (CurrentIntensity >= 3)
                {
                    return 0.75f;
                }
                else
                {
                    return 0.1f;
                }
            }
        }
        private uint SwerveMinLength
        {
            get
            {
                if (CurrentIntensity < 1.5)
                {
                    return 1000;
                }
                else if (CurrentIntensity >= 2)
                {
                    return 2000;
                }
                else if (CurrentIntensity >= 3)
                {
                    return 3000;
                }
                else
                {
                    return 1000;
                }
            }
        }
        private uint SwerveMaxLength
        {
            get
            {
                if (CurrentIntensity < 1.5)
                {
                    return 2000;
                }
                else if (CurrentIntensity >= 2)
                {
                    return 3500;
                }
                else if (CurrentIntensity >= 3)
                {
                    return 5000;
                }
                else
                {
                    return 2000;
                }
            }
        }
        private uint SwerveMinDelay
        {
            get
            {
                if (CurrentIntensity < 1.5)
                {
                    return 10000;
                }
                else if (CurrentIntensity >= 2)
                {
                    return 5000;
                }
                else if (CurrentIntensity >= 3)
                {
                    return 2500;
                }
                else
                {
                    return 10000;
                }
            }
        }
        private uint SwerveMaxDelay
        {
            get
            {
                if (CurrentIntensity < 1.5)
                {
                    return 15000;
                }
                else if (CurrentIntensity >= 2)
                {
                    return 10000;
                }
                else if (CurrentIntensity >= 3)
                {
                    return 5000;
                }
                else
                {
                    return 15000;
                }
            }
        }
        private uint HasBeenIntoxicatedFor => !Player.IsPerformingActivity ? 0 : Game.GameTime - GameTimeStartedIntoxicating;
        private uint HasBeenNoIntoxicatedFor => Player.IsPerformingActivity ? 0 : Game.GameTime - GameTimeStoppedIntoxicating;
        private uint TotalTimeIntoxicated => Player.IsPerformingActivity ? HasBeenIntoxicatedFor : GameTimeStoppedIntoxicating - GameTimeStartedIntoxicating;
        private uint TotalTimeSober => Player.IsPerformingActivity ? 0 : HasBeenNoIntoxicatedFor;
        public void Start()
        {
            GameTimeStartedIntoxicating = Game.GameTime;
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                ContinueIntoxication();
                BeginSoberingUp();
            }, "ScenarioWatcher");
        }
        private void BeginSoberingUp()
        {
            while (Player.IsIntoxicated)
            {
                UpdateDrunkStatus();
                GameFiber.Yield();
            }
        }
        private void ContinueIntoxication()
        {
            while (Player.IsPerformingActivity)
            {
                UpdateDrunkStatus();
                GameFiber.Yield();
            }
            Player.IsPerformingActivity = false;
            GameTimeStoppedIntoxicating = Game.GameTime;
        }
        private void SetIntoxicated()
        {
            Player.IsIntoxicated = true;
            CurrentClipset = ClipsetAtCurrentIntensity;
            NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, true);
            if(CurrentClipset != "NONE")
            {
                if (!NativeFunction.CallByName<bool>("HAS_ANIM_SET_LOADED", CurrentClipset))
                {
                    NativeFunction.CallByName<bool>("REQUEST_ANIM_SET", CurrentClipset);
                }
                NativeFunction.CallByName<bool>("SET_PED_MOVEMENT_CLIPSET", Game.LocalPlayer.Character, CurrentClipset, 0x3E800000);
            }

            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, true);
            NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER", OverLayEffect);
            NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", CurrentIntensity / 5.0f);
            NativeFunction.Natives.x80C8B1846639BB19(1);
            NativeFunction.CallByName<int>("SHAKE_GAMEPLAY_CAM", "DRUNK_SHAKE", CurrentIntensity);

            GameTimeUntilNextSwerve = Game.GameTime + RandomItems.GetRandomNumber(15000, 30000);


            //EntryPoint.WriteToConsole($"Player Made Drunk. Strength: {CurrentIntensity}");
        }
        private void SetSober(bool ResetClipset)
        {
            Player.IsIntoxicated = false;
            NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, false);
            if (ResetClipset)
            {
                NativeFunction.CallByName<bool>("RESET_PED_MOVEMENT_CLIPSET", Game.LocalPlayer.Character);
            }
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
            NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
            NativeFunction.Natives.x80C8B1846639BB19(0);
            NativeFunction.CallByName<int>("STOP_GAMEPLAY_CAM_SHAKING", true);
            //EntryPoint.WriteToConsole("Player Made Sober");
        }
        private void UpdateDrunkStatus()
        {
            if (!Player.IsIntoxicated && CurrentIntensity >= 0.25f)
            {
                SetIntoxicated();
            }
            else if (Player.IsIntoxicated && CurrentIntensity <= 0.25f)
            {
                SetSober(true);
            }
            if (Player.IsIntoxicated)
            {
                if (CurrentClipset != ClipsetAtCurrentIntensity && ClipsetAtCurrentIntensity != "NONE")
                {
                    CurrentClipset = ClipsetAtCurrentIntensity;
                    if (!NativeFunction.CallByName<bool>("HAS_ANIM_SET_LOADED", CurrentClipset))
                    {
                        NativeFunction.CallByName<bool>("REQUEST_ANIM_SET", CurrentClipset);
                    }
                    NativeFunction.CallByName<bool>("SET_PED_MOVEMENT_CLIPSET", Game.LocalPlayer.Character, CurrentClipset, 0x3E800000);
                }
                NativeFunction.CallByName<int>("SET_GAMEPLAY_CAM_SHAKE_AMPLITUDE", CurrentIntensity);
                NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", CurrentIntensity / 5.0f);
                Player.IntoxicatedIntensity = CurrentIntensity;
                UpdateSwerving();
            }
        }
        private void UpdateSwerving()
        {
            //SET_VEHICLE_STEER_BIAS
            if (Game.GameTime >= GameTimeUntilNextSwerve)
            {
                GameTimeUntilNextSwerve = Game.GameTime + RandomItems.GetRandomNumber(15000, 30000);
                if (!IsSwerving && Player.IsDriver)
                {
                    IsSwerving = true;
                    GameTimeStartedSwerving = Game.GameTime;
                    GameTimeToStopSwerving = Game.GameTime + RandomItems.GetRandomNumber(SwerveMinLength, SwerveMaxLength);
                    SteeringBias = RandomItems.GetRandomNumber(-1f * SwerveAtCurrentIntensity, SwerveAtCurrentIntensity);
                    //EntryPoint.WriteToConsole($"PLAYER EVENT: DRUNK SWERVE STARTED BIAS: {SwerveAtCurrentIntensity}", 3);
                }
            }
            if(IsSwerving && Game.GameTime > GameTimeToStopSwerving)
            {
                IsSwerving = false;
                SteeringBias = 0f;
                //EntryPoint.WriteToConsole($"PLAYER EVENT: DRUNK SWERVE ENDED", 3);
            }
            if (Player.IsDriver && IsSwerving)
            {
                NativeFunction.Natives.SET_VEHICLE_STEER_BIAS(Player.CurrentVehicle.Vehicle, SteeringBias);
            }
        }
    }
}