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
    public class IngestActivity : DynamicActivity
    {
        private Rage.Object Item;
        private string PlayingAnim;
        private string PlayingDict;
        private EatingData Data;
        private IntoxicatingEffect IntoxicatingEffect;
        private bool IsAttachedToHand;
        private bool IsCancelled;
        private IIntoxicatable Player;
        private ISettingsProvideable Settings;
        private ModItem ModItem;
        private IIntoxicants Intoxicants;
        private Intoxicant CurrentIntoxicant;
        public IngestActivity(IIntoxicatable consumable, ISettingsProvideable settings, ModItem modItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
            Intoxicants = intoxicants;
        }
        public override string DebugString => $"Intox {Player.IsIntoxicated} Consum: {Player.IsPerformingActivity} I: {Player.IntoxicatedIntensity}";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
            Player.StopIngesting(CurrentIntoxicant);
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
        private void AttachItemToHand()
        {
            CreateItem();
            if (Item.Exists() && !IsAttachedToHand)
            {
                Item.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, Data.HandBoneID), Data.HandOffset, Data.HandRotator);
                IsAttachedToHand = true;
            }
        }
        private void CreateItem()
        {
            if (!Item.Exists() && Data.PropModelName != "")
            {
                try
                {
                    Item = new Rage.Object(Data.PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception e)
                {
                    Game.DisplayNotification($"Could Not Spawn Prop {Data.PropModelName}");
                }
                if (Item.Exists())
                {
                    Item.IsGravityDisabled = false;
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
            AttachItemToHand();
            Player.IsPerformingActivity = true;
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);//-1
            Idle();
        }
        private void Exit()
        {
            if (Item.Exists())
            {
                Item.Detach();
            }
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            Player.IsPerformingActivity = false;
            Player.StopIngesting(CurrentIntoxicant);
            GameFiber.Sleep(5000);
            if (Item.Exists())
            {
                Item.Delete();
            }
        }
        private void Idle()
        {
            while (Player.CanPerformActivities && !IsCancelled)
            {
                Player.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                if (AnimationTime >= 0.25f)
                {
                    if (Item.Exists())
                    {
                        Item.Delete();
                    }
                }
                if (AnimationTime >= 0.35f)
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                    break;
                }
                GameFiber.Yield();
            }
            GameFiber.Sleep(5000);//wait for it to take effect!
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
            int HandBoneID = 57005;
            Vector3 HandOffset = Vector3.Zero;
            Rotator HandRotator = Rotator.Zero;
            string PropModel = "";

            //if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale)
            //{
            //    AnimIdleDictionary = "amb@code_human_wander_eating_donut@male@idle_a";
            //    AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };
            //}
            //else
            //{
            //    AnimIdleDictionary = "amb@code_human_wander_eating_donut@female@idle_a";
            //    AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };
            //}
            if (ModItem != null && ModItem.ModelItem != null)
            {
                HandBoneID = ModItem.ModelItem.AttachBoneIndex;
                HandOffset = ModItem.ModelItem.AttachOffset;
                HandRotator = ModItem.ModelItem.AttachRotation;
                PropModel = ModItem.ModelItem.ModelName;
            }


            AnimIdleDictionary = "mp_suicide";
            AnimIdle = new List<string>() { "pill" };



            if(ModItem != null && ModItem.IsIntoxicating)
            {
                CurrentIntoxicant = Intoxicants.Get(ModItem.IntoxicantName);
                Player.StartIngesting(CurrentIntoxicant);
            }


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