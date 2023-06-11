using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BodyWatcher
{
    public BodyWatcher(PedExt deadBody)
    {
        PedBody = deadBody;
        GameTimeFirstSeen = Game.GameTime;
    }

    public PedExt PedBody { get; set; }
    public uint GameTimeFirstSeen { get; set; }
    public void DisableAlerts()
    {
        if(PedBody == null || !PedBody.GeneratesBodyAlerts)
        {
            return;
        }
        PedBody.GeneratesBodyAlerts = false;
        EntryPoint.WriteToConsole($"{PedBody.Handle} I HAVE BEEN SEEN, DISABLING ADDITIONAL ALERTS");
    }
}

