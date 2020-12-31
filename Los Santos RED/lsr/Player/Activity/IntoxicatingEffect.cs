using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;

namespace LosSantosRED.lsr.Player
{
    public class IntoxicatingEffect
    {
        private string CurrentClipset;
        private uint GameTimeLastSetTimecycle;
        private uint GameTimeStartedIntoxicating;
        private uint GameTimeStoppedIntoxicating;
       // private float Intensity;
        private uint IntoxicatingIntervalTime;
        private float MaxEffectAllowed;
        private IConsumableIntoxicatable Player;
        private uint SoberingIntervalTime;
        public IntoxicatingEffect(IConsumableIntoxicatable player, float maxEffectAllowed, uint intoxicatingIntervalTime, uint soberingIntervalTime)
        {
            Player = player;
            MaxEffectAllowed = maxEffectAllowed;
            IntoxicatingIntervalTime = intoxicatingIntervalTime;
            SoberingIntervalTime = soberingIntervalTime;
        }
        public string ClipsetAtCurrentIntensity
        {
            get
            {
                if (CurrentIntensity < 3)
                {
                    return "move_m@drunk@slightlydrunk";
                }
                else if (CurrentIntensity >= 3)
                {
                    return "move_m@drunk@moderatedrunk";
                }
                else if (CurrentIntensity >= 5)
                {
                    return "move_m@drunk@verydrunk";
                }
                else
                {
                    return "move_m@drunk@slightlydrunk";
                }
            }
        }
        public float CurrentIntensity => (float)((float)TotalTimeIntoxicated / IntoxicatingIntervalTime - (float)TotalTimeSober / SoberingIntervalTime).Clamp(0.0f, MaxEffectAllowed);//(float)((float)TotalTimeDrank / 5000 - (float)TotalTimeSober / 60000).Clamp(0.0f, 5.0f);
        public string DebugString => $"IsIntox {Player.IsIntoxicated} IsBeingIntoxicated {Player.IsConsuming} I {CurrentIntensity} HBDF {HasBeenIntoxicatedFor} HBNDF {HasBeenNoIntoxicatedFor} TTD {TotalTimeIntoxicated} TTS {TotalTimeSober}";
        public uint HasBeenIntoxicatedFor => !Player.IsConsuming ? 0 : Game.GameTime - GameTimeStartedIntoxicating;
        public uint HasBeenNoIntoxicatedFor => Player.IsConsuming ? 0 : Game.GameTime - GameTimeStoppedIntoxicating;
        public uint TotalTimeIntoxicated => Player.IsConsuming ? HasBeenIntoxicatedFor : GameTimeStoppedIntoxicating - GameTimeStartedIntoxicating;
        public uint TotalTimeSober => Player.IsConsuming ? 0 : HasBeenNoIntoxicatedFor;
        public void SetSober(bool ResetClipset)
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
            Game.Console.Print("Player Made Sober");
        }
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
            while (Player.IsConsuming)
            {
                UpdateDrunkStatus();
                GameFiber.Yield();
            }
            Player.IsConsuming = false;
            GameTimeStoppedIntoxicating = Game.GameTime;
        }
        private void SetDrunk()
        {
            Player.IsIntoxicated = true;
            CurrentClipset = ClipsetAtCurrentIntensity;
            NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, true);
            if (!NativeFunction.CallByName<bool>("HAS_ANIM_SET_LOADED", CurrentClipset))
            {
                NativeFunction.CallByName<bool>("REQUEST_ANIM_SET", CurrentClipset);
            }
            NativeFunction.CallByName<bool>("SET_PED_MOVEMENT_CLIPSET", Game.LocalPlayer.Character, CurrentClipset, 0x3E800000);
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, true);
            NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER", "Drunk");
            NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", CurrentIntensity / 5.0f);
            NativeFunction.Natives.x80C8B1846639BB19(1);
            NativeFunction.CallByName<int>("SHAKE_GAMEPLAY_CAM", "DRUNK_SHAKE", CurrentIntensity);
            Game.Console.Print($"Player Made Drunk. Strength: {CurrentIntensity}");
        }
        private void UpdateDrunkStatus()
        {
            if (!Player.IsIntoxicated && CurrentIntensity >= 1.0f)
            {
                SetDrunk();
            }
            else if (Player.IsIntoxicated && CurrentIntensity <= 1.0f)
            {
                SetSober(true);
            }
            if (Player.IsIntoxicated)
            {
                if (CurrentClipset != ClipsetAtCurrentIntensity)
                {
                    CurrentClipset = ClipsetAtCurrentIntensity;
                    if (!NativeFunction.CallByName<bool>("HAS_ANIM_SET_LOADED", CurrentClipset))
                    {
                        NativeFunction.CallByName<bool>("REQUEST_ANIM_SET", CurrentClipset);
                    }
                    NativeFunction.CallByName<bool>("SET_PED_MOVEMENT_CLIPSET", Game.LocalPlayer.Character, CurrentClipset, 0x3E800000);
                }
                NativeFunction.CallByName<int>("SET_GAMEPLAY_CAM_SHAKE_AMPLITUDE", CurrentIntensity);

                if (Game.GameTime - GameTimeLastSetTimecycle >= 5000)
                {
                    GameTimeLastSetTimecycle = Game.GameTime;
                    NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", CurrentIntensity / 5.0f);
                }
                Player.IntoxicatedIntensity = CurrentIntensity;
            }
        }
    }
}