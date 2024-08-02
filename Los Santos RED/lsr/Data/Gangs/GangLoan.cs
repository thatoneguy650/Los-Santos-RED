using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;


public class GangLoan
{
    private bool HasSentWarning;
    private IGangRelateable Player;
    private Gang Gang;
    private ITimeReportable Time;
    private LoanParameter LoanParameter;
    private int LoanAmount;
    public DateTime DueDate { get; private set; }
    public int DueAmount { get; private set; }
    public int MissedPeriods { get; private set; }
    public int VigAmount { get; private set; }
    private bool IsPassedDueDate => DateTime.Compare(Time.CurrentDateTime, DueDate) >= 0;//is past the due date
    private bool ShouldSendWarning => !HasSentWarning && DateTime.Compare(DueDate.AddDays(-1), Time.CurrentDateTime) < 0;//Is within 1 day 
    public GangLoan(IGangRelateable player, Gang gang, ITimeReportable time, LoanParameter loanParameter, int loanAmount)
    {
        Player = player;
        Gang = gang;
        Time = time;
        LoanParameter = loanParameter;
        LoanAmount = loanAmount;
    }
    public void Setup()
    {

    }
    public void Update()
    {
        if (IsPassedDueDate && DueAmount > 0)
        {
            SetMissedPayment();
        }
        else if (ShouldSendWarning && DueAmount > 0)
        {
            SendWarningMessage();
        }
    }
    public void MakeVigPayment()
    {
        HasSentWarning = false;
        DateTime NextDueDate = DueDate.AddDays(7);
        DueDate = new DateTime(NextDueDate.Year, NextDueDate.Month, NextDueDate.Day, 12, 0, 0);
        SendVigMessage();
    }
    public void Dispose()
    {
        Reset();
    }
    public void PayLoan()
    {
        Reset();
        SendPaymentMessage();
    }
    public void Start(bool sendMessage)
    {
        Reset();
        DateTime NextDueDate = Time.CurrentDateTime.AddDays(7);
        DueDate = new DateTime(NextDueDate.Year, NextDueDate.Month, NextDueDate.Day, 12, 0, 0);
        int LoanExtra = (int)Math.Floor(LoanParameter.Rate * LoanAmount);
        VigAmount = LoanExtra;
        DueAmount = LoanAmount + LoanExtra;
        if (sendMessage)
        {
            SendStartMessage();
        }
    }
    public void RestartFromSaved(int dueAmount, int vigAmount, int missedPeriods, DateTime dueDate, LoanParameter loanParameter)
    {
        Reset();
        VigAmount = vigAmount;
        DueAmount = dueAmount;
        MissedPeriods = missedPeriods;
        DueDate = dueDate;
        LoanParameter = loanParameter;
    }
    public void Reset()
    {
        MissedPeriods = 0;
        HasSentWarning = false;
        DueAmount = 0;
        VigAmount = 0;
    }
    private void SetMissedPayment()
    {
        if (MissedPeriods >= LoanParameter.MaxPeriods)
        {
            int test = DueAmount;
            Reset();
            Player.RelationshipManager.GangRelationships.ResetGang(false);
            Player.RelationshipManager.GangRelationships.SetDebt(Gang, test);
            Player.RelationshipManager.GangRelationships.SetReputation(Gang, -1 * test, true);
        }
        else
        {
            MissedPeriods++;
            int toAdd = (int)Math.Floor(LoanParameter.Rate * DueAmount);
            DueAmount += toAdd;
            VigAmount += toAdd;
            HasSentWarning = false;
            DateTime NextDueDate = DueDate.AddDays(7);
            DueDate = new DateTime(NextDueDate.Year, NextDueDate.Month, NextDueDate.Day, 12, 0, 0);
            SendMissedMessage();
        }
    }
    private void SendStartMessage()
    {
        List<string> StartMessages = new List<string>() {
            $"Expecting ${DueAmount} by {DueDate}. You pay an additional {LoanParameter.Rate * 100f} points each week. You've got {LoanParameter.MaxPeriods} weeks to pay up.",
        };
        Player.CellPhone.AddScheduledText(Gang.Contact, StartMessages.PickRandom(), 2, false);
    }
    private void SendVigMessage()
    {
        List<string> StartMessages = new List<string>() {
            $"This will do for now.",
        };
        NativeHelper.DisplayNotificationCustom(Gang.Contact.IconName, Gang.Contact.IconName, Gang.ContactName, "~g~Response", StartMessages.PickRandom(), NotificationIconTypes.DollarSign, false);
    }
    private void SendPaymentMessage()
    {
        List<string> StartMessages = new List<string>() {
            $"About time I got this. Your debt of ${DueAmount} is cleared.",
        };
        NativeHelper.DisplayNotificationCustom(Gang.Contact.IconName, Gang.Contact.IconName, Gang.ContactName, "~g~Response", StartMessages.PickRandom(), NotificationIconTypes.DollarSign, false);
    }
    private void SendWarningMessage()
    {
        List<string> WarningMessages = new List<string>() {
            $"Dont forget your obligations. ${DueAmount} by {DueDate:g} or the points add up",
            $"Here's a warning. ${DueAmount} by {DueDate:g}.",
            $"Helpful reminder. ${DueAmount} by {DueDate:g}.",

        };
        Player.CellPhone.AddScheduledText(Gang.Contact, WarningMessages.PickRandom(), 2, false);
        HasSentWarning = true;
    }
    private void SendMissedMessage()
    {
        List<string> MissedMessages = new List<string>() {
        $"Thats a missed paymnent. Now I'm expecting ${DueAmount} by {DueDate:g}.",
        $"Are you stupid or something? I better get ${DueAmount} by {DueDate:g}.",
        $"Fuck you, pay me ${DueAmount} by {DueDate:g}. Don't make me come looking.",
        };
        Player.CellPhone.AddScheduledText(Gang.Contact, MissedMessages.PickRandom(), 2, false);
    }
    public override string ToString()
    {
        return $"Loan: ~r~${DueAmount}~s~ by {DueDate:g}~n~Days: {(DueDate.Date - Time.CurrentDateTime.Date).Days} ~n~Vig: ${VigAmount} ~n~Currently: {Time?.CurrentDateTime:g}";
    }
}

