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
    public void RunTasks()
    {
        int PedsUpdated = 0;
        foreach (Cop Cop in PedProvider.PoliceList.Where(x => x.CurrentTask != null).OrderBy(x => x.CurrentTask.GameTimeLastRan))
        {
            if (PedsUpdated > 2)
            {
                return;
            }
            else
            {
                Cop.UpdateTask();
                PedsUpdated++;
            }
        }
    }
    public void Update()
    {
        foreach (Cop Cop in PedProvider.PoliceList.Where(x => x.Pedestrian.Exists() && x.HasBeenSpawnedFor >= 2000))
        {
            if (WithinTaskDistance(Cop) && !Cop.IsInHelicopter)//heli, dogs, boats come next?
            {
                if (Player.IsWanted)
                {
                    if (Player.IsInSearchMode)
                    {
                        if (Cop.CurrentTask?.Name != "Locate")
                        {
                            Cop.CurrentTask = new Locate(Cop, Player);
                            Cop.CurrentTask.Start();
                        }
                    }
                    else
                    {
                        if (Cop.DistanceToPlayer <= 200f)
                        {
                            if (Player.PoliceResponse.IsDeadlyChase)
                            {
                                if (Cop.CurrentTask?.Name != "Kill")
                                {
                                    Cop.CurrentTask = new Kill(Cop, Player);
                                    Cop.CurrentTask.Start();
                                }
                            }
                            else
                            {
                                if (Cop.CurrentTask?.Name != "Chase")
                                {
                                    Cop.CurrentTask = new Chase(Cop, Player);
                                    Cop.CurrentTask.Start();
                                }
                            }
                        }
                        else if (Cop.DistanceToPlayer <= 1000f)
                        {
                            if (Cop.CurrentTask?.Name != "Locate")
                            {
                                Cop.CurrentTask = new Locate(Cop, Player);
                                Cop.CurrentTask.Start();
                            }
                        }
                    }
                }
                else if (Player.Investigation.IsActive)
                {
                    if (Cop.CurrentTask?.Name != "Investigate")
                    {
                        Cop.CurrentTask = new Investigate(Cop, Player);
                        Cop.CurrentTask.Start();
                    }
                }
                else
                {
                    if (Cop.CurrentTask?.Name != "Idle")
                    {
                        Cop.CurrentTask = new Idle(Cop, Player);
                        Cop.CurrentTask.Start();
                    }
                }
            }
            else
            {
                if (Cop.CurrentTask?.Name != "Idle")
                {
                    Cop.CurrentTask = new Idle(Cop, Player);
                    Cop.CurrentTask.Start();
                }
            }
        }
    }
    private bool WithinTaskDistance(Cop cop)
    {
        return cop.DistanceToPlayer <= Player.ActiveDistance;
    }
}

