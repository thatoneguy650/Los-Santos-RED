using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public class DebugMenu : Menu
{

    private UIMenu Debug;
    private UIMenuListItem AutoSetRadioStation;
    private UIMenuItem GiveMoney;
    private UIMenuItem FillHealthAndArmor;
    private UIMenuItem KillPlayer;
    private UIMenuListItem GetRandomWeapon;
    private IActionable Player;
    private RadioStations RadioStations;
    private int RandomWeaponCategory;
    private IWeapons Weapons; 
    public DebugMenu(MenuPool menuPool, IActionable player, IWeapons weapons, RadioStations radioStations)
    {    
        Player = player;
        Weapons = weapons;
        RadioStations = radioStations;
        Debug = new UIMenu("Debug", "Debug Settings");
        menuPool.Add(Debug);
        Debug.OnItemSelect += DebugMenuSelect;
        Debug.OnListChange += OnListChange;
        CreateDebugMenu();
    }
    public override void Hide()
    {
        Debug.Visible = false;
    }
    public override void Show()
    {
        if (!Debug.Visible)
        {
            Debug.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!Debug.Visible)
        {
            Debug.Visible = true;
        }
        else
        {
            Debug.Visible = false;
        }
    }
    private void CreateDebugMenu()
    {

        KillPlayer = new UIMenuItem("Kill Player", "Immediatly die and ragdoll");
        GetRandomWeapon = new UIMenuListItem("Get Random Weapon", "Gives the Player a random weapon and ammo.", Enum.GetNames(typeof(WeaponCategory)).ToList());
        GiveMoney = new UIMenuItem("Get Money", "Give you some cash");
        FillHealthAndArmor = new UIMenuItem("Health and Armor", "Get loaded for bear");
        AutoSetRadioStation = new UIMenuListItem("Auto-Set Station", "Will auto set the station any time the radio is on", RadioStations.RadioStationList);
        Debug.AddItem(KillPlayer);
        Debug.AddItem(GetRandomWeapon);
        Debug.AddItem(GiveMoney);
        Debug.AddItem(FillHealthAndArmor);
        Debug.AddItem(AutoSetRadioStation);
    }
    private void DebugMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == KillPlayer)
        {
            Game.LocalPlayer.Character.Kill();
        }
        else if (selectedItem == GetRandomWeapon)
        {
            WeaponInformation myGun = Weapons.GetRandomRegularWeapon((WeaponCategory)RandomWeaponCategory);
            if (myGun != null)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
            }
        }
        else if (selectedItem == GiveMoney)
        {
            Player.GiveMoney(50000);
        }
        else if (selectedItem == FillHealthAndArmor)
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
            Game.LocalPlayer.Character.Armor = 100;
        }
        Debug.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (list == GetRandomWeapon)
        {
            RandomWeaponCategory = index;
        }
        if (list == AutoSetRadioStation)
        {
            Player.AutoTuneStation = RadioStations.RadioStationList[index].InternalName;
            //EntryPoint.WriteToConsole($"Debug AutoTune Station {Player.AutoTuneStation}");
        }
    }
}