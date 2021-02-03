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
    private IWeapons Weapons;
    private Mod.Player PlayerIntellisense;//only for intellisense for the interface (see what i have and how i named it)
    public Tasker(IEntityProvideable pedProvider, ITargetable player, IWeapons weapons)
    {
        PedProvider = pedProvider;
        Player = player;
        Weapons = weapons;
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
        int CivlianPedsUpdated = 0;
        foreach (PedExt Ped in PedProvider.CivilianList.Where(x => x.CurrentTask != null).OrderBy(x => x.CurrentTask.GameTimeLastRan))
        {
            if (CivlianPedsUpdated > 2)
            {
                return;
            }
            else
            {
                Ped.UpdateTask();
                CivlianPedsUpdated++;
            }
        }
    }
    public void Update()
    {
        UpdatePolice();
        UpdateCivilians();
    }
    private void UpdatePolice()
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
    private void UpdateCivilians()
    {
        foreach (PedExt Civilian in PedProvider.CivilianList.Where(x => x.Pedestrian.Exists()))
        {
            if (Civilian.DistanceToPlayer <= 50f)
            {
                if (Civilian.HasSeenPlayerCommitCrime && Civilian.WillCallPolice)
                {
                    if (Civilian.CurrentTask?.Name != "CallIn")
                    {
                        Civilian.CurrentTask = new CallIn(Civilian, Player); ;
                        Civilian.CurrentTask.Start();
                    }
                }
                else if ((Civilian.CrimesWitnessed.Any(x=> x.AngersCivilians) || Civilian.IsFedUpWithPlayer) && Civilian.WillFight)
                {
                    if (Civilian.CurrentTask?.Name != "Fight")
                    {
                        WeaponInformation ToIssue = Civilian.IsGangMember ? Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol) : Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
                        Civilian.CurrentTask = new Fight(Civilian, Player, ToIssue); ;
                        Civilian.CurrentTask.Start();
                    }
                }
            }
        }
    }
    private bool WithinTaskDistance(Cop cop)
    {
        return cop.DistanceToPlayer <= Player.ActiveDistance;
    }
}

