using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class GangLoanSave
{
    public string GangID { get; set; }
    public int DueAmount { get; set; }
    public int MissedPeriods { get; set; }
    public DateTime DueDate { get; set; }
    public GangLoanSave()
    {
    }

    public GangLoanSave(string gangID, int dueAmount, int missedPeriods, DateTime dueDate)
    {
        GangID = gangID;
        DueAmount = dueAmount;
        MissedPeriods = missedPeriods;
        DueDate = dueDate;
    }
}
