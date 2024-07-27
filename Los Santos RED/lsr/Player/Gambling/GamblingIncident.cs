using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GamblingIncident
{
    public GamblingIncident()
    {

    }
    public GamblingIncident(int amountWon, DateTime dateTimeWon)
    {
        AmountWon = amountWon;
        DateTimeWon = dateTimeWon;
    }
    public int AmountWon { get; set; }
    public DateTime DateTimeWon { get; set; }
}

