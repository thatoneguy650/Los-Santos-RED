using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MoneyTheftInteract : TheftInteract
{
    private int CashAmount;
    private Rage.Object rightHandCashBundle;
    private Rage.Object leftHandCashBundle;
    public MoneyTheftInteract()
    {

    }

    public MoneyTheftInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }

    protected override bool CanInteract => CashAmount > 0;
    public int CashMinAmount { get; set; }
    public int CashMaxAmount { get; set; }
    public int CashGainedPerIncrement { get; set; }
    public bool SpawnPropLeftHand { get; set; } = true;
    public bool SpawnPropRightHand { get; set; } = true;
    public List<AnimationPoint> RightHandAnimationPoints { get; set; } = new List<AnimationPoint>();
    public List<AnimationPoint> LeftHandAnimationPoints { get; set; } = new List<AnimationPoint>();

    public override void Setup(IModItems modItems, IClothesNames clothesNames)
    {
        base.Setup(modItems, clothesNames);
        if (RightHandAnimationPoints == null || !RightHandAnimationPoints.Any())
        {
            RightHandAnimationPoints = new List<AnimationPoint>() { new AnimationPoint(0, 0.01f, true), new AnimationPoint(1, 0.4f, false), new AnimationPoint(2, 0.6f, true), new AnimationPoint(3, 0.8f, false) };
        }
        if (LeftHandAnimationPoints == null || !LeftHandAnimationPoints.Any())
        {
            LeftHandAnimationPoints = new List<AnimationPoint>() { new AnimationPoint(0, 0.1f, true), new AnimationPoint(1, 0.45f, false), new AnimationPoint(2, 0.7f, true), new AnimationPoint(3, 0.85f, false) };
        }
    }
    public override void OnInteriorLoaded()
    {
        if (!RandomItems.RandomPercent(SpawnPercent))
        {
            EntryPoint.WriteToConsole($"MoneyTheftInteract OnInteriorLoaded NOT SPAWNING (PERCENTAGE) ADDING NO CASH");
            CashAmount = 0;
            return;
        }
        CashAmount = RandomItems.GetRandomNumberInt(CashMinAmount, CashMaxAmount);
        base.OnInteriorLoaded();
    }
    protected override void GiveReward()
    {
        if (CashAmount <= CashGainedPerIncrement)
        {
            Player.BankAccounts.GiveMoney(CashAmount, false);
            NativeHelper.PlaySuccessSound();
            CashAmount = 0;
            EntryPoint.WriteToConsole($"BANK GAVE CASH 1 {CashAmount}");
        }
        else
        {
            Player.BankAccounts.GiveMoney(CashGainedPerIncrement, false);
            NativeHelper.PlaySuccessSound();
            CashAmount -= CashGainedPerIncrement;
            EntryPoint.WriteToConsole($"BANK GAVE CASH 2 {CashGainedPerIncrement}");
        }
        Game.DisplaySubtitle($"Remaining Cash: ${CashAmount}");
    }
    protected override void HandlePreLoop()
    {
        ModItem cashItem = ModItems?.Get("Cash Bundle");
        rightHandCashBundle = null;
        leftHandCashBundle = null;
        if (cashItem != null)
        {
            if (SpawnPropRightHand)
            {
                rightHandCashBundle = cashItem.SpawnAndAttachItem(Player, false, true);
            }
            if (SpawnPropLeftHand)
            {
                leftHandCashBundle = cashItem.SpawnAndAttachItem(Player, false, false);
            }
        }
        base.HandlePreLoop();
    }
    protected override void HandleLoop(float animationTime)
    {
        if (rightHandCashBundle.Exists())
        {
            foreach (AnimationPoint ap in RightHandAnimationPoints.OrderByDescending(x => x.Order))
            {
                if (animationTime >= ap.Position)
                {
                    if (ap.Visible)
                    {
                        if (!rightHandCashBundle.IsVisible)
                        {
                            rightHandCashBundle.IsVisible = true;
                        }
                    }
                    else
                    {
                        if (rightHandCashBundle.IsVisible)
                        {
                            rightHandCashBundle.IsVisible = false;
                        }
                    }

                    break;
                }
            }
        }
        if (leftHandCashBundle.Exists())
        {
            foreach (AnimationPoint ap in LeftHandAnimationPoints.OrderByDescending(x => x.Order))
            {
                if (animationTime >= ap.Position)
                {
                    if (ap.Visible)
                    {
                        if (!leftHandCashBundle.IsVisible)
                        {
                            leftHandCashBundle.IsVisible = true;
                        }
                    }
                    else
                    {
                        if (leftHandCashBundle.IsVisible)
                        {
                            leftHandCashBundle.IsVisible = false;
                        }
                    }

                    break;
                }
            }
        }
        base.HandleLoop(animationTime);
    }
    protected override void HandlePostLoop()
    {
        if (rightHandCashBundle.Exists())
        {
            rightHandCashBundle.Delete();
        }
        if (leftHandCashBundle.Exists())
        {
            leftHandCashBundle.Delete();
        }
        base.HandlePostLoop();
    }
}

