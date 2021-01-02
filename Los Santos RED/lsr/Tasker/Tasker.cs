using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Tasker
{
    private IEntityProvideable PedProvider;
    private ITargetable Player;
    private Mod.Player PlayerIntellisense;//only for intellisense for the interface (see what i have and how i named it)
    public Tasker(IEntityProvideable pedProvider, ITargetable player)
    {
        PedProvider = pedProvider;
        Player = player;
    }
    public void Update()
    {
        foreach (Cop Cop in PedProvider.PoliceList.Where(x => x.Pedestrian.Exists()))
        {
            if (Player.IsWanted && WithinTaskDistance(Cop))
            {
                if (Player.IsInSearchMode)
                {
                    //GoToLast Seen?
                }
                else
                {
                    if (Cop.DistanceToPlayer <= 200f)
                    {
                        if (Player.CurrentPoliceResponse.IsDeadlyChase)
                        {
                            //Kill?
                        }
                        else
                        {
                            if(Cop.CurrentTask.Name != "Chase")
                            {
                                Cop.CurrentTask = new Chase(Cop, Player);
                                Cop.CurrentTask.Start();
                            }
                        }
                    }
                    else if (Cop.DistanceToPlayer <= 1000f)
                    {
                        //GoToLast Seen?
                    }
                }
            }
            else
            {
                //idle?
            }
        }
    }
    private bool WithinTaskDistance(Cop cop)
    {
        return cop.DistanceToPlayer <= Player.ActiveDistance;
    }
}

