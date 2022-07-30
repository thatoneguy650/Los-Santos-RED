using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ActivityManager
{
    private IActivityManageable Player;

    public ActivityManager(IActivityManageable player)
    {
        Player = player;
    }
}


