using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CivilianTasker
{
    private IEntityProvideable PedProvider;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Tasker Tasker;
    public CivilianTasker(Tasker tasker, IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings)
    {
        Tasker = tasker;
        PedProvider = pedProvider;
        Player = player;
        Weapons = weapons;
        Settings = settings;
    }

    public void Setup()
    {

    }
    public void Update()
    {
        if (Settings.SettingsManager.CivilianSettings.ManageCivilianTasking)
        {
            Tasker.ExpireSeatAssignments();
            foreach (PedExt civilian in PedProvider.Pedestrians.CivilianList.Where(x => x.Pedestrian.Exists()))
            {
                try
                {
                    if (civilian.CanBeTasked && civilian.CanBeAmbientTasked)
                    {
                        if (civilian.DistanceToPlayer >= 230f)
                        {
                            civilian.CurrentTask = null;
                            continue;
                        }
                        if (civilian.NeedsTaskAssignmentCheck)
                        {
                            if (civilian.DistanceToPlayer <= 200f)
                            {
                                UpdateCurrentTask(civilian);//has yields if it does anything
                            }
                            else if (civilian.CurrentTask != null)
                            {
                                civilian.CurrentTask = null;
                            }
                        }
                        if (civilian.CurrentTask != null && civilian.CurrentTask.ShouldUpdate)
                        {
                            civilian.UpdateTask(null);
                            GameFiber.Yield();
                        }
                    }
                    else if (civilian.IsBusted)
                    {
                        UpdateCurrentTask(civilian);
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
                }
            }
            foreach (Merchant merchant in PedProvider.Pedestrians.MerchantList.Where(x => x.Pedestrian.Exists()))
            {
                try
                {
                    if (merchant.CanBeTasked && merchant.CanBeAmbientTasked)
                    {
                        if (merchant.DistanceToPlayer >= 230f)
                        {
                            merchant.CurrentTask = null;
                            continue;
                        }
                        if (merchant.NeedsTaskAssignmentCheck)
                        {
                            if (merchant.DistanceToPlayer <= 200f)
                            {
                                UpdateCurrentTask(merchant);
                            }
                            else if (merchant.CurrentTask != null)
                            {
                                merchant.CurrentTask = null;
                            }
                        }
                        if (merchant.CurrentTask != null && merchant.CurrentTask.ShouldUpdate)
                        {
                            merchant.UpdateTask(null);
                            GameFiber.Yield();
                        }
                    }
                    else if (merchant.IsBusted)
                    {
                        UpdateCurrentTask(merchant);
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
                }
            }
        }
    }
    private void UpdateCurrentTask(PedExt Civilian)//this should be moved out?
    {
        if (Civilian.IsBusted)
        {
            if (Civilian.DistanceToPlayer <= 75f)
            {
                if (Civilian.CurrentTask?.Name != "GetArrested")
                {
                    Civilian.CurrentTask = new GetArrested(Civilian, Player, PedProvider, Tasker);
                    GameFiber.Yield();//TR Added back 7
                    Civilian.CurrentTask.Start();
                }
            }
        }
        else if (Civilian.DistanceToPlayer <= 75f)//50f
        {
            WitnessedCrime HighestPriority = Civilian.OtherCrimesWitnessed.OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
            bool SeenScaryCrime = Civilian.PlayerCrimesWitnessed.Any(x => x.ScaresCivilians && x.CanBeReportedByCivilians) || Civilian.OtherCrimesWitnessed.Any(x => x.Crime.ScaresCivilians && x.Crime.CanBeReportedByCivilians);
            bool SeenAngryCrime = Civilian.PlayerCrimesWitnessed.Any(x => x.AngersCivilians && x.CanBeReportedByCivilians) || Civilian.OtherCrimesWitnessed.Any(x => x.Crime.AngersCivilians && x.Crime.CanBeReportedByCivilians);
            bool SeenMundaneCrime = Civilian.PlayerCrimesWitnessed.Any(x => !x.AngersCivilians && !x.ScaresCivilians && x.CanBeReportedByCivilians) || Civilian.OtherCrimesWitnessed.Any(x => !x.Crime.AngersCivilians && !x.Crime.ScaresCivilians && x.Crime.CanBeReportedByCivilians);
            if (SeenScaryCrime || SeenAngryCrime)
            {
                if (Civilian.WillCallPolice)
                {
                    if (Civilian.CurrentTask?.Name != "ScaredCallIn")
                    {
                        Civilian.CurrentTask = new ScaredCallIn(Civilian, Player) { OtherTarget = HighestPriority?.Perpetrator };
                        GameFiber.Yield();//TR Added back 7
                        Civilian.CurrentTask.Start();

                    }
                }
                else if (Civilian.WillFight)
                {
                    if (SeenAngryCrime && Player.IsNotWanted)
                    {
                        if (Civilian.CurrentTask?.Name != "Fight")
                        {
                            Civilian.CurrentTask = new Fight(Civilian, Player, GetWeaponToIssue(Civilian.IsGangMember)) { OtherTarget = HighestPriority?.Perpetrator };
                            GameFiber.Yield();//TR Added back 7
                            Civilian.CurrentTask.Start();
                        }
                    }
                    else
                    {
                        if (Civilian.CurrentTask?.Name != "Flee")
                        {
                            Civilian.CurrentTask = new Flee(Civilian, Player) { OtherTarget = HighestPriority?.Perpetrator };
                            GameFiber.Yield();//TR Added back 7
                            Civilian.CurrentTask.Start();
                        }
                    }
                }
                else
                {
                    if (Civilian.CurrentTask?.Name != "Flee")
                    {
                        Civilian.CurrentTask = new Flee(Civilian, Player) { OtherTarget = HighestPriority?.Perpetrator };
                        GameFiber.Yield();//TR Added back 7
                        Civilian.CurrentTask.Start();
                    }
                }
            }
            else if (Civilian.CanAttackPlayer && Civilian.WillFight)// && !Civilian.IsGangMember )
            {
                if (Civilian.CurrentTask?.Name != "Fight")
                {
                    Civilian.CurrentTask = new Fight(Civilian, Player, GetWeaponToIssue(Civilian.IsGangMember)) { OtherTarget = HighestPriority?.Perpetrator };//gang memebrs already have guns
                    GameFiber.Yield();//TR Added back 7
                    Civilian.CurrentTask.Start();
                }
            }
            else if (SeenMundaneCrime)
            {
                if (Civilian.WillCallPolice)
                {
                    if (Civilian.CurrentTask?.Name != "CalmCallIn")
                    {
                        Civilian.CurrentTask = new CalmCallIn(Civilian, Player);//oither target not needed, they just call in all crimes
                        GameFiber.Yield();//TR Added back 7
                        Civilian.CurrentTask.Start();
                    }
                }
            }
        }
        Civilian.GameTimeLastUpdatedTask = Game.GameTime;
    }
    private WeaponInformation GetWeaponToIssue(bool IsGangMember)
    {
        WeaponInformation ToIssue;
        if (IsGangMember)
        {
            if (RandomItems.RandomPercent(70))
            {
                ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol);
            }
            else
            {
                ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
            }
        }
        else if (RandomItems.RandomPercent(40))
        {
            ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol);
        }
        else
        {
            if (RandomItems.RandomPercent(65))
            {
                ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
            }
            else
            {
                ToIssue = null;
            }
        }
        return ToIssue;
    }
}
