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
        private uint IntoxicatingIntervalTime;
        private float MaxEffectAllowed;
        private IConsumableIntoxicatable Player;
        private uint SoberingIntervalTime;
        private string OverLayEffect;
        public IntoxicatingEffect(IConsumableIntoxicatable player, float maxEffectAllowed, uint intoxicatingIntervalTime, uint soberingIntervalTime, string overlay)
        {
            Player = player;
            MaxEffectAllowed = maxEffectAllowed;
            IntoxicatingIntervalTime = intoxicatingIntervalTime;
            SoberingIntervalTime = soberingIntervalTime;
            OverLayEffect = overlay;
        }
        public float CurrentIntensity => (float)((float)TotalTimeIntoxicated / IntoxicatingIntervalTime - (float)TotalTimeSober / SoberingIntervalTime).Clamp(0.0f, MaxEffectAllowed);
        public string DebugString => $"IsIntox {Player.IsIntoxicated} IsBeingIntoxicated {Player.IsConsuming} I {CurrentIntensity} HBDF {HasBeenIntoxicatedFor} HBNDF {HasBeenNoIntoxicatedFor} TTD {TotalTimeIntoxicated} TTS {TotalTimeSober}";
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
        private uint HasBeenIntoxicatedFor => !Player.IsConsuming ? 0 : Game.GameTime - GameTimeStartedIntoxicating;
        private uint HasBeenNoIntoxicatedFor => Player.IsConsuming ? 0 : Game.GameTime - GameTimeStoppedIntoxicating;
        private uint TotalTimeIntoxicated => Player.IsConsuming ? HasBeenIntoxicatedFor : GameTimeStoppedIntoxicating - GameTimeStartedIntoxicating;
        private uint TotalTimeSober => Player.IsConsuming ? 0 : HasBeenNoIntoxicatedFor;
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
            //Game.Console.Print($"Player Made Drunk. Strength: {CurrentIntensity}");
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
            //Game.Console.Print("Player Made Sober");
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

               // if (Game.GameTime - GameTimeLastSetTimecycle >= 5000)
               // {
                   // GameTimeLastSetTimecycle = Game.GameTime;
                    NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", CurrentIntensity / 5.0f);
               // }
                Player.IntoxicatedIntensity = CurrentIntensity;
            }
        }
    }
}