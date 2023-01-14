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
using System.Windows.Media;


public class CivilianTasker
{
    private IEntityProvideable PedProvider;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Tasker Tasker;
    private IPlacesOfInterest PlacesOfInterest;
    public CivilianTasker(Tasker tasker, IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Tasker = tasker;
        PedProvider = pedProvider;
        Player = player;
        Weapons = weapons;
        Settings = settings;
        PlacesOfInterest = placesOfInterest;
    }

    public void Setup()
    {

    }
    public void Update()
    {
        if (Settings.SettingsManager.CivilianSettings.ManageCivilianTasking)
        {
            PedProvider.Pedestrians.ExpireSeatAssignments();
            foreach (PedExt civilian in PedProvider.Pedestrians.CivilianList.Where(x => x.Pedestrian.Exists()))
            {
                try
                {
                    civilian.PedBrain.Update(Player, PlacesOfInterest);
                    //DoTaskUpdate(civilian);
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
                    merchant.PedBrain.Update(Player, PlacesOfInterest);
                    //DoTaskUpdate(merchant);          
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
                }
            }
            foreach (SecurityGuard securityGuard in PedProvider.Pedestrians.SecurityGuardList.Where(x => x.Pedestrian.Exists()))
            {
                try
                {
                    securityGuard.PedBrain.Update(Player, PlacesOfInterest);
                   // DoTaskUpdate(securityGuard);
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
                }
            }
        }
    }
    //private void DoTaskUpdate(PedExt civilian)
    //{
    //    if (civilian.CanBeTasked && civilian.CanBeAmbientTasked)
    //    {
    //        if (civilian.DistanceToPlayer >= 230f)
    //        {
    //            civilian.CurrentTask = null;
    //            return;
    //        }
    //        if (civilian.NeedsTaskAssignmentCheck)
    //        {
    //            if (civilian.DistanceToPlayer <= 200f)
    //            {
    //                UpdateCurrentTask(civilian);//has yields if it does anything
    //            }
    //            else if (civilian.CurrentTask != null)
    //            {
    //                civilian.CurrentTask = null;
    //            }
    //        }
    //        if (civilian.CurrentTask != null && civilian.CurrentTask.ShouldUpdate)
    //        {
    //            civilian.UpdateTask(null);
    //            GameFiber.Yield();
    //        }
    //    }
    //    else if (civilian.IsBusted || civilian.IsWanted)
    //    {
    //        UpdateCurrentTask(civilian);
    //        if (civilian.CurrentTask != null && civilian.CurrentTask.ShouldUpdate)
    //        {
    //            civilian.UpdateTask(null);
    //            GameFiber.Yield();
    //        }
    //    }
    //    else if (!civilian.IsBusted && !civilian.CanBeTasked)
    //    {
    //        if (civilian.CurrentTask != null)
    //        {
    //            civilian.CurrentTask = null;
    //        }
    //    }
    //}



    //private void UpdateCurrentTask(PedExt Civilian)//this should be moved out?
    //{
    //    if (Civilian.IsBusted)
    //    {
    //        if (Civilian.DistanceToPlayer <= 175f)//75f
    //        {
    //            SetArrested(Civilian);
    //        }
    //    }
    //    else if (Civilian.IsWanted)
    //    {
    //        if(Civilian.WillFightPolice)
    //        {
    //            SetFight(Civilian);
    //        }
    //        else
    //        {
    //            SetFlee(Civilian);
    //        }
    //    }
    //    else if (Civilian.DistanceToPlayer <= 75f)//50f
    //    {
    //        Civilian.PedReactions.Update();
    //        if (Civilian.PedReactions.HasSeenScaryCrime || Civilian.PedReactions.HasSeenAngryCrime)
    //        {
    //            if (Civilian.WillCallPolice || (Civilian.WillCallPoliceIntense && Civilian.PedReactions.HasSeenIntenseCrime))
    //            {
    //                SetScaredCallIn(Civilian);
    //            }
    //            else if (Civilian.WillFight)
    //            {
    //                if (Civilian.PedReactions.HasSeenAngryCrime && Player.IsNotWanted)
    //                {
    //                    SetFight(Civilian);
    //                }
    //                else
    //                {
    //                    SetFlee(Civilian);
    //                }
    //            }
    //            else
    //            {
    //                SetFlee(Civilian);
    //            }
    //        }
    //        else if (Civilian.CanAttackPlayer && Civilian.WillFight)// && !Civilian.IsGangMember )
    //        {
    //            SetFight(Civilian);
    //        }
    //        else if (Civilian.PedReactions.HasSeenMundaneCrime && Civilian.WillCallPolice)
    //        {
    //            SetCalmCallIn(Civilian); 
    //        }
    //        else if(Civilian.WasModSpawned && Civilian.CurrentTask == null)
    //        {
    //            SetIdle(Civilian);
    //        }
    //    }
    //    Civilian.GameTimeLastUpdatedTask = Game.GameTime;
    //}
    //private void SetArrested(PedExt ped)
    //{
    //    if (ped.CurrentTask?.Name == "GetArrested")
    //    {
    //        return;
    //    }
    //    ped.CurrentTask = new GetArrested(ped, Player, PedProvider);
    //    GameFiber.Yield();//TR Added back 7
    //    ped.CurrentTask?.Start();
    //}
    //private void SetFlee(PedExt ped)
    //{
    //    if (ped.CurrentTask?.Name == "Flee")
    //    {
    //        return;
    //    }
    //    ped.CurrentTask = new Flee(ped, Player) { OtherTarget = ped.PedReactions.HighestPriorityCrime?.Perpetrator };
    //    GameFiber.Yield();//TR Added back 7
    //    ped.CurrentTask?.Start();
    //}
    //private void SetFight(PedExt ped)
    //{
    //    if (ped.CurrentTask?.Name == "Fight")
    //    {
    //        return;
    //    }
    //    ped.CurrentTask = new Fight(ped, Player, GetWeaponToIssue(ped.IsGangMember)) { OtherTarget = ped.PedReactions.HighestPriorityCrime?.Perpetrator };//gang memebrs already have guns
    //    GameFiber.Yield();//TR Added back 7
    //    ped.CurrentTask?.Start();
    //}
    //private void SetScaredCallIn(PedExt ped)
    //{
    //    if (ped.CurrentTask?.Name == "ScaredCallIn")
    //    {
    //        return;
    //    }
    //    ped.CurrentTask = new ScaredCallIn(ped, Player) { OtherTarget = ped.PedReactions.HighestPriorityCrime?.Perpetrator };
    //    GameFiber.Yield();//TR Added back 7
    //    ped.CurrentTask?.Start();
    //}
    //private void SetCalmCallIn(PedExt ped)
    //{
    //    if (ped.CurrentTask?.Name == "CalmCallIn")
    //    {
    //        return;
    //    }
    //    ped.CurrentTask = new CalmCallIn(ped, Player);
    //    GameFiber.Yield();//TR Added back 4
    //    ped.CurrentTask.Start();
    //}
    //private void SetIdle(PedExt ped)
    //{
    //    if (ped.CurrentTask?.Name == "GangIdle")
    //    {
    //        return;
    //    }
    //    ped.CurrentTask = new GangIdle(ped, Player, PedProvider, PlacesOfInterest);
    //    GameFiber.Yield();//TR Added back 4
    //    ped.CurrentTask.Start();
    //}

    //private WeaponInformation GetWeaponToIssue(bool IsGangMember)
    //{
    //    WeaponInformation ToIssue;
    //    if (IsGangMember)
    //    {
    //        if (RandomItems.RandomPercent(70))
    //        {
    //            ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol);
    //        }
    //        else
    //        {
    //            ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
    //        }
    //    }
    //    else if (RandomItems.RandomPercent(40))
    //    {
    //        ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol);
    //    }
    //    else
    //    {
    //        if (RandomItems.RandomPercent(65))
    //        {
    //            ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
    //        }
    //        else
    //        {
    //            ToIssue = null;
    //        }
    //    }
    //    return ToIssue;
    //}
}

//old merchant update
//if (merchant.CanBeTasked && merchant.CanBeAmbientTasked)
//{
//    if (merchant.DistanceToPlayer >= 230f)
//    {
//        merchant.CurrentTask = null;
//        continue;
//    }
//    if (merchant.NeedsTaskAssignmentCheck)
//    {
//        if (merchant.DistanceToPlayer <= 200f)
//        {
//            UpdateCurrentTask(merchant);
//        }
//        else if (merchant.CurrentTask != null)
//        {
//            merchant.CurrentTask = null;
//        }
//    }
//    if (merchant.CurrentTask != null && merchant.CurrentTask.ShouldUpdate)
//    {
//        merchant.UpdateTask(null);
//        GameFiber.Yield();
//    }
//}
//else if (merchant.IsBusted || merchant.IsWanted)
//{
//    UpdateCurrentTask(merchant);
//    if (merchant.CurrentTask != null && merchant.CurrentTask.ShouldUpdate)
//    {
//        merchant.UpdateTask(null);
//        GameFiber.Yield();
//    }
//}
//else if (!merchant.IsBusted && !merchant.CanBeTasked)
//{
//    if (merchant.CurrentTask != null)
//    {
//        merchant.CurrentTask = null;
//    }
//}