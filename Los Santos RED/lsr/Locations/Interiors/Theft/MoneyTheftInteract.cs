using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MoneyTheftInteract : TheftInteract
{
    private int CashAmount;



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
    public override void OnInteriorLoaded()
    {
        CashAmount = RandomItems.GetRandomNumberInt(CashMinAmount, CashMaxAmount);
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
}

