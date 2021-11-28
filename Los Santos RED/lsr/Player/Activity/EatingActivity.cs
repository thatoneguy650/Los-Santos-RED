using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class EatingActivity : DynamicActivity
    {
        private Rage.Object Food;
        private string PlayingAnim;
        private string PlayingDict;
        private EatingData Data;
        private IntoxicatingEffect IntoxicatingEffect;
        private bool IsAttachedToHand;
        private bool IsCancelled;
        private IIntoxicatable Player;
        private ISettingsProvideable Settings;
        private ModItem ModItem;
        public EatingActivity(IIntoxicatable consumable, ISettingsProvideable settings, ModItem modItem) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
        }
        public override string DebugString => $"Intox {Player.IsIntoxicated} Consum: {Player.IsPerformingActivity} I: {Player.IntoxicatedIntensity}";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
        }
        public override void Continue()
        {
        }
        public override void Start()
        {
            Setup();
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "DrinkingWatcher");
        }
        private void AttachFoodToHand()
        {
            CreateFood();
            if (Food.Exists() && !IsAttachedToHand)
            {
                Food.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, Data.HandBoneID), Data.HandOffset, Data.HandRotator);
                IsAttachedToHand = true;
            }
        }
        private void CreateFood()
        {
            if (!Food.Exists() && Data.PropModelName != "")
            {
                try
                {
                    //Vector3 position = Player.Character.GetOffsetPositionUp(50f);
                    //Model modelToCreate = new Model(Game.GetHashKey(Data.PropModelName));
                    //modelToCreate.LoadAndWait();
                    //Food = NativeFunction.Natives.CREATE_OBJECT<Rage.Object>(Game.GetHashKey(Data.PropModelName), position.X, position.Y, position.Z, 0f);
                    Food = new Rage.Object(Data.PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch(Exception e)
                {
                    Game.DisplayNotification($"Could Not Spawn Prop {Data.PropModelName}");
                }
                if (Food.Exists())
                {
                    Food.IsGravityDisabled = false;
                }
                else
                {
                    IsCancelled = true;
                }
            }
        }
        private void Enter()
        {
            Player.SetUnarmed();
            AttachFoodToHand();
            Player.IsPerformingActivity = true;
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);//-1
            Idle();
        }
        private void Exit()
        {
            if (Food.Exists())
            {
                Food.Detach();
            }
            //Player.Character.Tasks.Clear();
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            Player.IsPerformingActivity = false;
            GameFiber.Sleep(5000);
            if (Food.Exists())
            {
                Food.Delete();
            }
        }
        private void Idle()
        {
            bool HasMadeNoise = false;
            while (Player.CanPerformActivities && !IsCancelled)
            {
                Player.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                if (AnimationTime >= 0.9f)
                {
                    if (Food.Exists())
                    {
                        Food.Delete();
                        if(Game.LocalPlayer.Character.Health < Game.LocalPlayer.Character.MaxHealth)
                        {
                            int ToAdd = 10;
                            if(Game.LocalPlayer.Character.MaxHealth - Game.LocalPlayer.Character.Health < 10)
                            {
                                ToAdd = Game.LocalPlayer.Character.MaxHealth - Game.LocalPlayer.Character.Health;
                            }
                            Player.Character.Health += ToAdd;
                        }
                    }
                }
                if (AnimationTime >= 1.0f)
                {
                    break;
                }
                if(!HasMadeNoise && AnimationTime >= 0.2)
                {
                    HasMadeNoise = true;
                    SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_EAT" }, false);
                }
                GameFiber.Yield();
            }
            Exit();
        }
        private void Setup()
        {
            List<string> AnimIdle;
            string AnimEnter = "";
            string AnimEnterDictionary = "";
            string AnimExit = "";
            string AnimExitDictionary = "";
            string AnimIdleDictionary;
            int HandBoneID;
            Vector3 HandOffset;
            Rotator HandRotator;
            string PropModel;

            if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale)
            {
                AnimIdleDictionary = "amb@code_human_wander_eating_donut@male@idle_a";
                AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };
            }
            else
            {
                AnimIdleDictionary = "amb@code_human_wander_eating_donut@female@idle_a";
                AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };
            }
            HandBoneID = ModItem.PhysicalItem.AttachBoneIndex;
            HandOffset = ModItem.PhysicalItem.AttachOffset;
            HandRotator = ModItem.PhysicalItem.AttachRotation;
            PropModel = ModItem.PhysicalItem.ModelName;
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            Data = new EatingData(AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneID, HandOffset, HandRotator, PropModel);
        }
        private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
        {
            bool Spoke = false;
            foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()))
            {
                ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                GameFiber.Sleep(100);
                if (ToSpeak.IsAnySpeechPlaying)
                {
                    Spoke = true;
                }
                if (Spoke)
                {
                    break;
                }
            }
            GameFiber.Sleep(100);
            while (ToSpeak.IsAnySpeechPlaying && WaitForComplete)
            {
                Spoke = true;
                GameFiber.Yield();
            }
            if (!Spoke)
            {
                Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
            }
            return Spoke;
        }
    }
}