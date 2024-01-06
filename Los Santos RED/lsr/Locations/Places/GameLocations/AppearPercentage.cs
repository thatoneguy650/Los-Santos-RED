using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AppearPercentage
{
    public AppearPercentage()
    {
    }

    public AppearPercentage(int hour, int percentage)
    {
        Hour = hour;
        Percentage = percentage;
    }

    public int Hour { get; set; }
    public int Percentage { get; set; }

}

