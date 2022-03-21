using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerTask
{
    public PlayerTask(string contactName, bool isActive)
    {
        ContactName = contactName;
        IsActive = isActive;
    }

    public string ContactName { get; set; }
    public bool IsActive { get; set; } = false;
    public bool CanExpire { get; set; } = false;
    public DateTime ExpireTime { get; set; }
    public bool IsReadyForPayment { get; set; } = false;
    public int PaymentAmountOnCompletion { get; set; } = 0;
    public int RepAmountOnCompletion { get; set; } = 0;
    public int DebtAmountOnFail { get; set; } = 0;
    public int RepAmountOnFail { get; set; } = -200;


    public bool WasCompleted { get; set; } = false;
    public bool WasFailed { get; set; } = false;
    public DateTime CompletionTime { get; set; }
    public DateTime FailedTime { get; set; }


    //public uint GameTimeCompleted { get; set; }
    //public uint GameTimeFailed { get; set; }
}

