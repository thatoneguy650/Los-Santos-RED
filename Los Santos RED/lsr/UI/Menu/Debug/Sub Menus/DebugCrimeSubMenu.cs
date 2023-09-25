using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class DebugCrimeSubMenu : DebugSubMenu
{
    private ISettingsProvideable Settings;
    private ICrimes Crimes;
    private ITaskerable Tasker;
    private IEntityProvideable World;
    private IWeapons Weapons;
    public DebugCrimeSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, ISettingsProvideable settings, ICrimes crimes, ITaskerable tasker, IEntityProvideable world, IWeapons weapons) : base(debug, menuPool, player)
    {
        Settings = settings;
        Crimes = crimes;
        Tasker = tasker;
        World = world;
        Weapons = weapons;
    }
    public override void AddItems()
    {
        UIMenu CrimeItemsMenu = MenuPool.AddSubMenu(Debug, "Crime Menu");
        CrimeItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various crime items.";

        UIMenuListScrollerItem<int> SetWantedLevel = new UIMenuListScrollerItem<int>("Set Wanted Level", "Set wanted at the desired level", new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        SetWantedLevel.Activated += (menu, item) =>
        {
            if (SetWantedLevel.SelectedItem <= Settings.SettingsManager.PoliceSettings.MaxWantedLevel)
            {
                Player.SetWantedLevel(SetWantedLevel.SelectedItem, "Debug Menu", true);
                menu.Visible = false;
            }
        };
        UIMenuItem ToggleInvestigation = new UIMenuItem("Toggle Investigation", "Start or stop an investigation.");
        ToggleInvestigation.Activated += (menu, item) =>
        {
            if (Player.Investigation.IsActive)
            {
                Player.Investigation.Start(Player.Character.Position, false, true, false, false);
            }
            else
            {
                Player.Investigation.Expire();
            }
            menu.Visible = false;
        };


        UIMenuItem CallInCrime = new UIMenuItem("Call Cops On Yourself", "Call the cops on yourself with description.");
        CallInCrime.Activated += (menu, item) =>
        {
            //CrimeSceneDescription description = new CrimeSceneDescription(!Player.IsInVehicle, false, Player.Character.Position, true);
            //Player.PoliceResponse.AddCrime(Crimes.CrimeList?.FirstOrDefault(x => x.ID == StaticStrings.ArmedRobberyCrimeID), description, true);


            Crime crimeObserved = Crimes.GetCrime(StaticStrings.ArmedRobberyCrimeID);
            CrimeSceneDescription description = new CrimeSceneDescription(!Player.IsInVehicle, false, Player.Character.Position, true);
            Player.PoliceResponse.AddCrime(crimeObserved, description, false);
            Player.Investigation.Start(Player.Character.Position, true, true, false, false);


            menu.Visible = false;
        };


        UIMenuItem SpawnGunAttackersMenu = new UIMenuItem("Spawn Gun Attacker", "spawns some peds with guns that will attack you");
        SpawnGunAttackersMenu.Activated += (menu, item) =>
        {
            SpawnGunAttackers();
            menu.Visible = false;
        };
        UIMenuItem SpawnNoGunAttackersMenu = new UIMenuItem("Spawn No Gun Attackers", "spawns some peds without guins that will attack you");
        SpawnNoGunAttackersMenu.Activated += (menu, item) =>
        {
            SpawnNoGunAttackers();
            menu.Visible = false;
        };
        UIMenuItem StartRandomCrime = new UIMenuItem("Start Random Crime", "Trigger a random crime around the map.");
        StartRandomCrime.Activated += (menu, item) =>
        {
            Tasker.CreateCrime();
            menu.Visible = false;
        };


        UIMenuItem GiveClosesetGun = new UIMenuItem("Give Gun", "Give a gun to the closest ped");
        GiveClosesetGun.Activated += (menu, item) =>
        {
            GiveClosestGun();
            menu.Visible = false;
        };

        UIMenuItem SetNearestWanted = new UIMenuItem("Set Nearest Wanted", "Set the nearest ped wanted");
        SetNearestWanted.Activated += (menu, item) =>
        {
            SetNearestPedWanted();
            menu.Visible = false;
        };


        UIMenuItem ToggleCopTasking = new UIMenuItem("Toggle Cop Tasking", "Toggle player cop as taskable or not");
        ToggleCopTasking.Activated += (menu, item) =>
        {
            Player.ToggleCopTaskable();
            menu.Visible = false;
        };




        UIMenuListScrollerItem<string> SetDistantSirens = new UIMenuListScrollerItem<string>("Set Distant Sirens", "Set distance sirens play or stop", new List<string>() { "Start", "Stop" });
        SetDistantSirens.Activated += (menu, item) =>
        {
            bool value = SetDistantSirens.SelectedItem == "Start" ? true : false;
            NativeFunction.Natives.DISTANT_COP_CAR_SIRENS(value);
            Game.DisplaySubtitle($"DISTANT_COP_CAR_SIRENS SET {value}");

        };



        CrimeItemsMenu.AddItem(SetWantedLevel);
        CrimeItemsMenu.AddItem(ToggleInvestigation);
        CrimeItemsMenu.AddItem(SpawnGunAttackersMenu);
        CrimeItemsMenu.AddItem(SpawnNoGunAttackersMenu);
        CrimeItemsMenu.AddItem(StartRandomCrime);
        CrimeItemsMenu.AddItem(GiveClosesetGun);
        CrimeItemsMenu.AddItem(SetNearestWanted);
        CrimeItemsMenu.AddItem(ToggleCopTasking);
        CrimeItemsMenu.AddItem(SetDistantSirens);
        CrimeItemsMenu.AddItem(CallInCrime);
    }
    private void SpawnGunAttackers()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                bool isInvince = true;
                Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionRight(10f).Around2D(10f));
                GameFiber.Yield();
                if (coolguy.Exists())
                {
                    coolguy.BlockPermanentEvents = true;
                    coolguy.KeepTasks = true;

                    coolguy.Inventory.GiveNewWeapon(WeaponHash.Pistol, 50, true);
                    //coolguy.IsInvincible = true;
                    //if (RandomItems.RandomPercent(30))
                    //{
                    //    coolguy.Inventory.GiveNewWeapon(WeaponHash.Pistol, 50, true);
                    //}
                    //else if (RandomItems.RandomPercent(30))
                    //{
                    //    coolguy.Inventory.GiveNewWeapon(WeaponHash.Bat, 1, true);
                    //}
                    coolguy.Tasks.FightAgainstClosestHatedTarget(250f, -1);
                    PedExt pedExt = new PedExt(coolguy, Settings, Crimes, Weapons, "Test1", "CRIMINAL", World);
                    pedExt.WasEverSetPersistent = true;
                    pedExt.WillFight = true;
                    pedExt.WillFightPolice = true;
                    pedExt.WillAlwaysFightPolice = true;
                    World.Pedestrians.AddEntity(pedExt);
                }
                while (coolguy.Exists() && !Game.IsKeyDownRightNow(Keys.P))
                {
                    Game.DisplayHelp($"Attackers Spawned! ~n~P to Delete ~n~O to Flee~n~L to Toggle Invincible");
                    if (Game.IsKeyDownRightNow(Keys.L))
                    {
                        isInvince = !isInvince;
                        coolguy.IsInvincible = isInvince;
                        Game.DisplaySubtitle($"isInvince {isInvince}");
                    }
                    if (Game.IsKeyDownRightNow(Keys.O))
                    {
                        coolguy.Tasks.Clear();
                        coolguy.Tasks.Flee(Game.LocalPlayer.Character, 1000f, -1);
                    }
                    GameFiber.Sleep(25);
                }
                if (coolguy.Exists())
                {
                    coolguy.Delete();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Run Debug Logic");
    }
    private void GiveClosestGun()
    {
        PedExt toChoose = World.Pedestrians.PedExts.OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        if (toChoose != null && toChoose.Pedestrian.Exists())
        {
            toChoose.Pedestrian.Inventory.GiveNewWeapon(WeaponHash.Pistol, 50, true);
            //EntryPoint.WriteToConsoleTestLong($"Gave {toChoose.Pedestrian.Handle} Weapon");
        }

    }
    private void SetNearestPedWanted()
    {
        PedExt toChoose = World.Pedestrians.PedExts.Where(x => x.Handle != Player.Handle).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        if (toChoose != null && toChoose.Pedestrian.Exists())
        {
            toChoose.SetWantedLevel(3);
            //EntryPoint.WriteToConsoleTestLong($"Gave {toChoose.Pedestrian.Handle} Weapon");
        }

    }
    private void SpawnNoGunAttackers()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(10f).Around2D(10f));
                GameFiber.Yield();
                if (coolguy.Exists())
                {
                    coolguy.BlockPermanentEvents = true;
                    coolguy.KeepTasks = true;
                    //coolguy.IsInvincible = true;
                    PedExt pedExt = new PedExt(coolguy, Settings, Crimes, Weapons, "Test1", "CRIMINAL", World);
                    pedExt.WillFight = true;
                    pedExt.WillFightPolice = true;
                    pedExt.WillAlwaysFightPolice = true;
                    pedExt.WasEverSetPersistent = true;
                    World.Pedestrians.AddEntity(pedExt);
                    NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", coolguy, 281, true);//Can Writhe
                    NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", coolguy, false);

                    if (RandomItems.RandomPercent(30))
                    {
                        coolguy.Inventory.GiveNewWeapon(WeaponHash.Bat, 1, true);
                    }
                    else if (RandomItems.RandomPercent(30))
                    {
                        coolguy.Inventory.GiveNewWeapon(WeaponHash.Knife, 1, true);
                    }
                    //coolguy.Tasks.FightAgainstClosestHatedTarget(250f, -1);
                    coolguy.Tasks.FightAgainst(Game.LocalPlayer.Character);
                }
                while (coolguy.Exists() && !Game.IsKeyDownRightNow(Keys.P))
                {
                    Game.DisplayHelp($"Attackers Spawned! Press P to Delete O to Flee");


                    if (Game.IsKeyDownRightNow(Keys.O))
                    {
                        coolguy.Tasks.Clear();
                        coolguy.Tasks.Flee(Game.LocalPlayer.Character, 1000f, -1);
                    }
                    GameFiber.Sleep(25);
                }
                if (coolguy.Exists())
                {
                    coolguy.Delete();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Run Debug Logic");
    }
}

