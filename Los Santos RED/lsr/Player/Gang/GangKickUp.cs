using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangKickUp
{

    private bool HasSentWarning;
    private IGangRelateable Player;
    private Gang Gang;
    private ITimeReportable Time;

    public DateTime DueDate { get; private set; }
    public int DueAmount { get; private set; }
    public int MissedPeriods { get; private set; }
    public int MissedAmount { get; private set; }
    private bool IsPassedDueDate => DateTime.Compare(Time.CurrentDateTime, DueDate) >= 0;//is past the due date
    private bool ShouldSendWarning => !HasSentWarning && DateTime.Compare(DueDate.AddDays(-1), Time.CurrentDateTime) < 0;//Is within 1 day 
    public GangKickUp(IGangRelateable player, Gang gang, ITimeReportable time)
    {
        Player = player;
        Gang = gang;
        Time = time;

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
    public void Dispose()
    {
        Reset();
    }
    public void PayDue()
    {
        Reset();
        SetNewDueDate(true);
        SendPaymentMessage();
        EntryPoint.WriteToConsole("PAYED GANG KICK UP");
    }
    public void Start(bool sendMessage)
    {
        Reset();
        SetNewDueDate(false);
        if (sendMessage)
        {
            SendStartMessage();
        }
    }
    public void Restart(DateTime dueDate, int missedPeriods, int missedAmout)
    {
        Reset();
        DueDate = dueDate;
        MissedPeriods = missedPeriods;
        MissedAmount = missedAmout;
        DueAmount = Gang.MemberKickUpAmount + MissedAmount;
    }
    private void SetNewDueDate(bool fromCurrentDue)
    {
        DateTime NextDueDate;
        if (fromCurrentDue && DueDate != null)
        {
            NextDueDate = DueDate.AddDays(Gang.MemberKickUpDays);
        }
        else
        {
            NextDueDate = Time.CurrentDateTime.AddDays(Gang.MemberKickUpDays);
        }
        DueDate = new DateTime(NextDueDate.Year, NextDueDate.Month, NextDueDate.Day, 12, 0, 0);
        DueAmount = Gang.MemberKickUpAmount + MissedAmount;
    }
    private void Reset()
    {
        MissedPeriods = 0;
        HasSentWarning = false;
        MissedAmount = 0;
        DueAmount = 0;
    }
    private void SetMissedPayment()
    {
        if(MissedPeriods >= Gang.MemberKickUpMissLimit)
        {
            int test = DueAmount;
            Reset();
            Player.GangRelationships.ResetGang(false);
            Player.GangRelationships.SetDebt(Gang, test);
            Player.GangRelationships.SetReputation(Gang, -500, true);
        }
        else
        {
            MissedAmount = DueAmount;
            MissedPeriods++;
            HasSentWarning = false;
            SetNewDueDate(true);
            SendMissedMessage();
        }
        EntryPoint.WriteToConsole("SET MISSED KICK UP PAYMENT");
    }
    private void SendStartMessage()
    {
        List<string> StartMessages = new List<string>() { 
            $"The kick up is ${DueAmount} every {Gang.MemberKickUpDays} days.",
            $"I expect ${DueAmount} every {Gang.MemberKickUpDays} days.",
            $"Every {Gang.MemberKickUpDays} days I expect ${DueAmount}.",
            $"You need to kick up ${DueAmount} every {Gang.MemberKickUpDays} days.",
            $"This ain't free, ${DueAmount} every {Gang.MemberKickUpDays} days.",
        };
        Player.CellPhone.AddScheduledText(Gang.ContactName, Gang.ContactIcon, StartMessages.PickRandom());
    }
    private void SendPaymentMessage()
    {
        List<string> StartMessages = new List<string>() { 
            $"Thanks for the donation. See you in {Gang.MemberKickUpDays} days for ${DueAmount}.",
            $"Good work, next time is ${DueAmount} in {Gang.MemberKickUpDays} days.",
        };
        NativeHelper.DisplayNotificationCustom(Gang.ContactIcon, Gang.ContactIcon, Gang.ContactName, "~g~Response", StartMessages.PickRandom(), NotificationIconTypes.DollarSign, false);
        //Player.CellPhone.AddPhoneResponse(Gang.ContactName, Gang.ContactIcon, StartMessages.PickRandom());
    }
    private void SendWarningMessage()
    {
        List<string> WarningMessages = new List<string>() { 
            $"Dont forget. ${DueAmount} by {DueDate:g}.",
            $"Here's a warning. ${DueAmount} by {DueDate:g}.",
            $"Helpful reminder. ${DueAmount} by {DueDate:g}.",
            $"Cutting it a little close? ${DueAmount} by {DueDate:g}.",

        };
        Player.CellPhone.AddScheduledText(Gang.ContactName, Gang.ContactIcon, WarningMessages.PickRandom());
        HasSentWarning = true;
        EntryPoint.WriteToConsole("SENT WARNING FOR GANG KICK UP");
    }
    private void SendMissedMessage()
    {
        List<string> MissedMessages = new List<string>() { $"You trying to hide money from me prick? DO NOT MISS THE KICK UP. Expecting ${DueAmount} by {DueDate:g}.",
        $"${DueAmount} by {DueDate:g}. Miss it and they won't miss you.",
        $"You are playing a dangerous game. I better get ${DueAmount} by {DueDate:g}.",
        $"Owing people like me money isn't a good idea. I better get ${DueAmount} by {DueDate:g}.",

        };
        Player.CellPhone.AddScheduledText(Gang.ContactName, Gang.ContactIcon, MissedMessages.PickRandom());
    }
    public override string ToString()
    {
        return $"Gang Dues: ${DueAmount} by {DueDate:g}";
    }
}

