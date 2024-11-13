using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerTask
{
    private ISettingsProvideable Settings;
    public PlayerTask(string contactName, bool isActive, ISettingsProvideable settings)
    {
        ContactName = contactName;
        IsActive = isActive;
        Settings = settings;
    }
    public string Name { get; set; } = "";
    public int DaysToCompleted => CanExpire ? (ExpireTime - StartTime).Days : 0;
    public string ContactName { get; set; }
    public bool IsActive { get; set; } = false;
    public DateTime StartTime { get; set; }
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
    public bool FailOnStandardRespawn { get; set; } = false;
    public bool HasSentExpiringSoon { get; set; } = false;
    public bool JoinGangOnComplete { get; set; } = false;
    public PhoneContact PhoneContact { get; set; }
    public void OnReadyForPayment(bool displayHelp, string helpText)
    {
        //EntryPoint.WriteToConsoleTestLong($"OnReadyForPayment {ContactName} displayHelp {displayHelp} helpText {helpText}");
        IsReadyForPayment = true;
        if (!displayHelp || !Settings.SettingsManager.TaskSettings.DisplayHelpPrompts)
        {
            return;
        }
        if (helpText == "")
        {
            Game.DisplayHelp($"{ContactName} Ready for Payment");
        }
        else
        {
            Game.DisplayHelp(helpText);
        }       
    }
    public void OnReadyForPayment(bool displayHelp) => OnReadyForPayment(displayHelp, "");
    public virtual void OnCompleted()
    {

    }

}

