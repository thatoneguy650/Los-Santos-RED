using LosSantosRED.lsr.Interface;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;

public class BribeActivity
{
    private ISettingsProvideable Settings;
    private IRespawnable Player;
    private IEntityProvideable World;
    private IModItems ModItems;
    private Cop Cop;
    private Vector3 CopTargetPosition;
    private float CopTargetHeading;
    private bool isCopInPosition;
    private bool IsActive;
    private AnimationWatcher AnimationWatcher;
    private PedPlayerInteract PedInteractSetup;
    private string TakeCashDictionary;
    private string TakeCashAnimation;
    private string OfferCashDictionary;
    private string OfferCashAnimation;
    private Rage.Object MoneyProp;
    public BribeActivity(IRespawnable player, IEntityProvideable world, ISettingsProvideable settings, IModItems modItems)
    {
        Player = player;
        World = world;
        Settings = settings;
        ModItems = modItems;
    }
    public bool HasFinishedAnimation { get; private set; }
    public bool CanContinueBribe => EntryPoint.ModController.IsRunning && (Player.IsBusted || Player.IsArrested) && !Player.IsIncapacitated && Player.IsAlive && Cop != null && Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead && !Cop.IsInWrithe && !Cop.IsUnconscious;
    public bool IsFailed { get; set; } = false;
    public void Setup()
    {
        OfferCashDictionary = "cellphone@self";
        OfferCashAnimation = "selfie_in";
        TakeCashDictionary = "mp_common_miss";
        TakeCashAnimation = "card_swipe";
        AnimationWatcher = new AnimationWatcher();
        AnimationDictionary.RequestAnimationDictionay(TakeCashDictionary);
        AnimationDictionary.RequestAnimationDictionay(OfferCashDictionary);
    }
    public void Dispose()
    {
        ReleaseCop();
        if (MoneyProp.Exists())
        {
            MoneyProp.Delete();
        }
    }
    public void Start()
    {
        bool IsCancelled = false;
        uint GameTimeStarted = Game.GameTime;
        while(Player.IsBusted && Player.IsAlive && Game.GameTime - GameTimeStarted <= 20000 && (!Player.Surrendering.HasPlayedSurrenderActivity || Player.Character.IsRagdoll || Player.Character.IsStunned))
        {
            //Game.DisplaySubtitle($"IS WAITING TO START BRIBE {Game.GameTime}");
            if(Player.IsMoveControlPressed)
            {
                IsCancelled = true;
                break;
            }
            GameFiber.Yield();
        }
        if(IsCancelled)
        {
            Dispose();
            return;
        }    
        GetCop();
        if (Cop == null || !Cop.Pedestrian.Exists())
        {
            Dispose();
            return;
        }
        SetupCop();
        SetupWorld();
        if (!CanContinueBribe)
        {
            Dispose();
            return;
        }
        OfferBribe();
        PedInteractSetup = new PedPlayerInteract(Player, Cop, -0.9f);
        PedInteractSetup.Start();
        if(CanContinueBribe && PedInteractSetup.IsInPosition)
        {
            PlayBribeAnimations();
        }
        Dispose();
    }
    private void SetupCop()
    {
        if (Cop == null)
        {
            return;
        }
        Cop.CanBeAmbientTasked = false;
        Cop.CanBeTasked = false;
    }
    private void SetupWorld()
    {
        Game.TimeScale = 1.0f;
    }
    private void GetCop()
    {
        Cop = World.Pedestrians.PoliceList.Where(x => x.DistanceToPlayer <= 20f && x.HeightToPlayer <= 5f && !x.IsInVehicle && !x.IsUnconscious && !x.IsInWrithe && !x.IsDead && !x.Pedestrian.IsRagdoll).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
    }
    private void ReleaseCop()
    {
        if (Cop == null)
        {
            return;
        }
        Cop.CanBeTasked = true;
        Cop.CanBeAmbientTasked = true;
    } 
    private void OfferBribe()
    {
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, OfferCashDictionary, OfferCashAnimation, 2.0f, -2.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_UPPERBODY | eAnimationFlags.AF_SECONDARY), 0, false, false, false);
        GameFiber.Sleep(1000);
        CreateAndAttachCash();
        NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
        Player.PlaySpeech(new List<string>() { "GENERIC_HOWS_IT_GOING", "APOLOGY_NO_TROUBLE", "CHAT_STATE" }.PickRandom(), false);
        GameFiber.Sleep(1000);
    }
    private void PlayBribeAnimations()
    {
        if (Cop == null || !CanContinueBribe)
        {
            return;
        }
        Cop.WeaponInventory.ShouldAutoSetWeaponState = false;
        Cop.WeaponInventory.SetUnarmed();

        NativeFunction.Natives.TASK_PLAY_ANIM(Cop.Pedestrian, TakeCashDictionary, TakeCashAnimation, 2.0f, -2.0f, -1, 0, 0, false, false, false);

        if (IsFailed)
        {
            Cop.PlaySpeech(new List<string>() { "GENERIC_INSULT_HIGH", "CHALLENGE_THREATEN", "GENERIC_WHATEVER" }.PickRandom(), false);
        }
        else
        {
            Cop.PlaySpeech(new List<string>() { "WON_DISPUTE", "GENERIC_THANKS", "GENERIC_BYE" }.PickRandom(), false);
        }
        bool endLoop = false;
        while (Cop.Pedestrian.Exists() && !endLoop && CanContinueBribe)
        {
            float animTime =  NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Cop.Pedestrian, TakeCashDictionary, TakeCashAnimation);
            bool isAnimRunning = AnimationWatcher.IsAnimationRunning(animTime);
            if (!isAnimRunning)
            {
                //EntryPoint.WriteToConsoleTestLong("Cop Animation on Search Not Running");
                endLoop = true;
            }
            if (animTime >= 0.5f)
            {
                if (MoneyProp.Exists())
                {
                    MoneyProp.Delete();
                }
                endLoop = true;
            }
            if (animTime >= 1.0f)
            {
                if(MoneyProp.Exists())
                {
                    MoneyProp.Delete();
                }
                endLoop = true;
            }
            GameFiber.Yield();
        }
        Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
        Cop.WeaponInventory.RemoveHeavyWeapon();
        if (MoneyProp.Exists())
        {
            MoneyProp.Delete();
        }
    }
    private void CreateAndAttachCash()
    {
        ModItem cashItem = ModItems.Get("Cash Bundle");
        if(cashItem == null)
        {
            return;
        }
        string HandBoneName = "BONETAG_R_PH_HAND";
        Vector3 HandOffset = Vector3.Zero;
        Rotator HandRotator = Rotator.Zero;
        string modelName = "";
        bool HasProp = false;
        PropAttachment finalAttachment = null;
        if (cashItem.ModelItem != null && cashItem.ModelItem.ModelName != "")
        {
            modelName = cashItem.ModelItem.ModelName;
            HasProp = true;
            finalAttachment = cashItem.ModelItem?.Attachments?.FirstOrDefault(x => x.Name == "RightHandPass" && (x.Gender == "U" || x.Gender == Player.Gender));
            if (finalAttachment == null)
            {
                finalAttachment = cashItem.ModelItem?.Attachments?.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
            }
        }
        if (finalAttachment != null)
        {
            HandOffset = finalAttachment.Attachment;
            HandRotator = finalAttachment.Rotation;
            HandBoneName = finalAttachment.BoneName;
        }
        MoneyProp = null;
        if (HasProp && modelName != "")
        {
            try
            {
                MoneyProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));// new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
            }
            catch (Exception ex)
            {

            }
            GameFiber.Yield();
            if (MoneyProp.Exists())
            {
                MoneyProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
            }
        }
    }
}

