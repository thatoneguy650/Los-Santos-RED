using System;

public class GangKickSave
{
    public GangKickSave()
    {
    }

    public GangKickSave(string gangID, DateTime kickDueDate, int kickMissedPeriods, int kickMissedAmount)
    {
        GangID = gangID;
        KickDueDate = kickDueDate;
        KickMissedPeriods = kickMissedPeriods;
        KickMissedAmount = kickMissedAmount;
    }

    public string GangID { get; set; }
    public DateTime KickDueDate { get; set; }
    public int KickMissedPeriods { get; set; }
    public int KickMissedAmount { get; set; }
}
