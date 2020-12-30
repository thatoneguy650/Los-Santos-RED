using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;

namespace LosSantosRED.lsr.Player
{
    public class IntoxicatingActivity
    {
        private IPlayerIntoxicationState Player;
        private float Intensity;
        private uint GameTimeStartedImbibing;
        private uint GameTimeStoppedImbibing;
        private uint GameTimeLastSetTimecycle;
        private string CurrentClipset;
        private string ScenarionName;
        private uint IntoxicatingIntervalTime;
        private uint SoberingIntervalTime;
        private float MaxEffectAllowed; 

        public IntoxicatingActivity(IPlayerIntoxicationState player, string ActivityToStart, float maxEffectAllowed, uint intoxicatingIntervalTime, uint soberingIntervalTime)
        {
            ScenarionName = ActivityToStart;
            Player = player;
            MaxEffectAllowed = maxEffectAllowed;
            IntoxicatingIntervalTime = intoxicatingIntervalTime;
            SoberingIntervalTime = soberingIntervalTime;
        }
        public string DebugString => $"IsIntox {Player.IsDrunk} IsBeingIntoxicated {Player.IsImbibing} I {CurrentIntensity} HBDF {HasBeenIntoxicatedFor} HBNDF {HasBeenNoIntoxicatedFor} TTD {TotalTimeIntoxicated} TTS {TotalTimeSober}";
        public uint TotalTimeIntoxicated => Player.IsImbibing ? HasBeenIntoxicatedFor : GameTimeStoppedImbibing - GameTimeStartedImbibing;
        public uint TotalTimeSober => Player.IsImbibing ? 0 : HasBeenNoIntoxicatedFor;
        public float CurrentIntensity => (float)((float)TotalTimeIntoxicated / IntoxicatingIntervalTime - (float)TotalTimeSober / SoberingIntervalTime).Clamp(0.0f, MaxEffectAllowed);//(float)((float)TotalTimeDrank / 5000 - (float)TotalTimeSober / 60000).Clamp(0.0f, 5.0f);
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
        public uint HasBeenIntoxicatedFor => !Player.IsImbibing ? 0 : Game.GameTime - GameTimeStartedImbibing;
        public uint HasBeenNoIntoxicatedFor => Player.IsImbibing ? 0 : Game.GameTime - GameTimeStoppedImbibing;
        public void Start()
        {
            GameTimeStartedImbibing = Game.GameTime;
            NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", Game.LocalPlayer.Character, ScenarionName, 0, true);      
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                ContinueIntoxication();
                BeginSoberingUp();        
            }, "ScenarioWatcher");
        }
        private void ContinueIntoxication()
        {
            while (!EntryPoint.IsMoveControlPressed)
            {
                UpdateDrunkStatus();
                GameFiber.Yield();
            }
            Game.LocalPlayer.Character.Tasks.Clear();
            Player.IsImbibing = false;
            GameTimeStoppedImbibing = Game.GameTime;
        }
        private void BeginSoberingUp()
        {
            while (Player.IsDrunk)
            {
                UpdateDrunkStatus();
                GameFiber.Yield();
            }
        }
        private void UpdateDrunkStatus()
        {
            if(!Player.IsDrunk && CurrentIntensity >= 1.0f)
            {
                SetDrunk();
            }
            else if (Player.IsDrunk && CurrentIntensity <= 0.0f)
            {
                SetSober(true);
            }
            if (Player.IsDrunk)
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

                if(Game.GameTime - GameTimeLastSetTimecycle >= 5000)
                {
                    GameTimeLastSetTimecycle = Game.GameTime;
                    NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", CurrentIntensity / 5.0f);
                }  
                Intensity = CurrentIntensity;
            }
        }
        private void SetDrunk()
        {
            Player.IsDrunk = true;
            CurrentClipset = ClipsetAtCurrentIntensity;
            NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, true);
            if (!NativeFunction.CallByName<bool>("HAS_ANIM_SET_LOADED", CurrentClipset))
            {
                NativeFunction.CallByName<bool>("REQUEST_ANIM_SET", CurrentClipset);
            }
            NativeFunction.CallByName<bool>("SET_PED_MOVEMENT_CLIPSET", Game.LocalPlayer.Character, CurrentClipset, 0x3E800000);
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, true);
            NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER", "Drunk");
            NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", CurrentIntensity/5.0f);
            NativeFunction.Natives.x80C8B1846639BB19(1);
            NativeFunction.CallByName<int>("SHAKE_GAMEPLAY_CAM", "DRUNK_SHAKE", CurrentIntensity);
            Game.Console.Print($"Player Made Drunk. Strength: {CurrentIntensity}");
        }
        public void SetSober(bool ResetClipset)
        {
            Player.IsDrunk = false;
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
    }
}