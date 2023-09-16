using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PossibleBribe
{
    public int Amount { get; set; }
    public float Percentage { get; set; }
    public PossibleBribe()
    {
    }

    public PossibleBribe(int amount, float percentage)
    {
        Amount = amount;
        Percentage = percentage;
    }
    public override string ToString()
    {
        if (Percentage >= 100f)
        {
            return $"~r~${Amount}";
        }
        return $"~r~${Amount} ~s~({Percentage}%)";
    }
    public bool AttemptBribe() => RandomItems.RandomPercent(Percentage);

}

