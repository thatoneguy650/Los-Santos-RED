using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class LoanParameter
{
    public GangRespect ResepectLevel { get; set; }
    public float Rate { get; set; }
    public int MaxPeriods { get; set; }
    public int MinAmount { get; set; }
    public int MaxAmount { get; set; }
    public LoanParameter()
    {
    }

    public LoanParameter(GangRespect resepectLevel, float rate, int maxDurationDays, int minAmount, int maxAmount)
    {
        ResepectLevel = resepectLevel;
        Rate = rate;
        MaxPeriods = maxDurationDays;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
    }
}

