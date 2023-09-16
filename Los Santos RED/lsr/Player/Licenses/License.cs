using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class License
{
    public License()
    {
    }

    public virtual DateTime ExpirationDate { get; set; }
    public virtual DateTime IssueDate { get; set; }
    public virtual string IssueStateID { get; set; }
    public virtual void IssueLicense(ITimeReportable time, int months, string stateID)
    {
        if(IsValid(time))
        {
            ExpirationDate = ExpirationDate.AddMonths(months);
        }
        else
        {
            IssueDate = time.CurrentDateTime;
            ExpirationDate = time.CurrentDateTime.AddMonths(months);
        }
    }
    public virtual bool IsValid(ITimeReportable time)
    {
        if (DateTime.Compare(time.CurrentDateTime, ExpirationDate) >= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public virtual string ExpirationDescription(ITimeReportable time)
    {
        if (DateTime.Compare(time.CurrentDateTime, ExpirationDate) >= 0)
        {
            return $"~n~Issue Date: {IssueDate:d}~n~Expiration Date: ~r~{ExpirationDate:d}~s~";
        }
        else
        {
            return $"~n~Issue Date: {IssueDate:d}~n~Expiration Date: ~g~{ExpirationDate:d}~s~";
        }
    }
}

