using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Respawning
{
    private int BailFee;
    private int BailFeePastDue;
    private uint GameTimeLastBribedPolice;
    private uint GameTimeLastDischargedFromHospital;
    private uint GameTimeLastResistedArrest;
    private uint GameTimeLastRespawned;
    private uint GameTimeLastSurrenderedToPolice;
    private uint GameTimeLastUndied;
    private int HospitalBillPastDue;
    public bool RecentlyBribedPolice
    {
        get
        {
            if (GameTimeLastBribedPolice == 0)
                return false;
            else if (Game.GameTime - GameTimeLastBribedPolice <= 10000)
                return true;
            else
                return false;
        }
    }
    public bool RecentlyDischargedFromHospital
    {
        get
        {
            if (GameTimeLastDischargedFromHospital == 0)
                return false;
            else if (Game.GameTime - GameTimeLastDischargedFromHospital <= 5000)
                return true;
            else
                return false;
        }
    }
    public bool RecentlyResistedArrest
    {
        get
        {
            if (GameTimeLastResistedArrest == 0)
                return false;
            else if (Game.GameTime - GameTimeLastResistedArrest <= 5000)
                return true;
            else
                return false;
        }
    }
    public bool RecentlyRespawned
    {
        get
        {
            if (GameTimeLastRespawned == 0)
                return false;
            else if (Game.GameTime - GameTimeLastRespawned <= 1000)
                return true;
            else
                return false;
        }
    }
    public bool RecentlySurrenderedToPolice
    {
        get
        {
            if (GameTimeLastSurrenderedToPolice == 0)
                return false;
            else if (Game.GameTime - GameTimeLastSurrenderedToPolice <= 5000)
                return true;
            else
                return false;
        }
    }
    public bool RecentlyUndied
    {
        get
        {
            if (GameTimeLastUndied == 0)
                return false;
            else if (Game.GameTime - GameTimeLastUndied <= 5000)
                return true;
            else
                return false;
        }
    }
    public void BribePolice(int Amount)
    {
        if (Mod.Player.Instance.Money < Amount)
        {
            Game.DisplayNotification("CHAR_BANK_FLEECA", "CHAR_BANK_FLEECA", "FLEECA Bank", "Overdrawn Notice", string.Format("Current transaction would overdraw account. Denied.", Amount));
        }
        else if (Amount < (Mod.Player.Instance.WantedLevel * DataMart.Instance.Settings.SettingsManager.Police.PoliceBribeWantedLevelScale))
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "Officer Friendly", "Expedited Service Fee", string.Format("Thats it? ${0}?", Amount));
            Mod.Player.Instance.GiveMoney(-1 * Amount);
        }
        else
        {       
            ResetPlayer(true, false, false, false);
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "Officer Friendly", "Expedited Service Fee", "Thanks for the cash, now beat it.");
            Mod.Player.Instance.GiveMoney(-1 * Amount);
            GameTimeLastBribedPolice = Game.GameTime;
        }
    }
    public void ResistArrest()
    {
        ResetPlayer(false, false, false, false);
        GameTimeLastResistedArrest = Game.GameTime;
    }
    public void RespawnAtGrave()
    {
        FadeOut();
        Respawn(true, true, true, true);
        GameLocation PlaceToSpawn = DataMart.Instance.Places.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Grave);
        SetPlayerAtLocation(PlaceToSpawn);     
        Mod.World.Instance.ClearPolice();
        Game.LocalPlayer.Character.IsRagdoll = true;
        FadeIn();
        Game.LocalPlayer.Character.IsRagdoll = false;
        GameTimeLastDischargedFromHospital = Game.GameTime;
    }
    public void RespawnAtHospital(GameLocation PlaceToSpawn)
    {
        FadeOut();
        Respawn(true, true, true, true);
        if (PlaceToSpawn == null)
        {
            PlaceToSpawn = DataMart.Instance.Places.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Hospital);
        }
        SetPlayerAtLocation(PlaceToSpawn);   
        Mod.World.Instance.ClearPolice(); 
        FadeIn();
        SetHospitalFee(PlaceToSpawn.Name);
        GameTimeLastDischargedFromHospital = Game.GameTime;
    }
    public void RespawnAtCurrentLocation(bool withInvicibility, bool resetWanted)
    {
        Respawn(resetWanted, true, false, false);
        Mod.Player.Instance.CurrentPoliceResponse.SetWantedLevel(Mod.Player.Instance.MaxWantedLastLife, "RespawnAtCurrentLocation", true);
        if (withInvicibility)
        {
            Game.LocalPlayer.Character.IsInvincible = true;
            GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(5000);
                Game.LocalPlayer.Character.IsInvincible = false;
            });
        }
        GameTimeLastUndied = Game.GameTime;
    }
    public void SurrenderToPolice(GameLocation PoliceStation)
    {
        FadeOut();
        CheckWeapons();
        BailFee = Mod.Player.Instance.MaxWantedLastLife * DataMart.Instance.Settings.SettingsManager.Police.PoliceBailWantedLevelScale;//max wanted last life wil get reset when calling resetplayer
        Mod.Player.Instance.RaiseHands();
        ResetPlayer(true, true,false,true);
        if (PoliceStation == null)
        {
            PoliceStation = DataMart.Instance.Places.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Police);
        }
        SetPlayerAtLocation(PoliceStation);
        Mod.World.Instance.ClearPolice();
        FadeIn();
        SetPoliceFee(PoliceStation.Name, BailFee);
        GameTimeLastSurrenderedToPolice = Game.GameTime;
    }
    private void CheckWeapons()
    {
        if (!Mod.Player.Instance.KilledAnyCops)
        {
            RemoveIllegalWeapons();
        }
        else
        {
            Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        }
    }
    private void FadeIn()
    {
        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);
    }
    private void FadeOut()
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);
    }
    private void RemoveIllegalWeapons()
    {
        //Needed cuz for some reason the other weapon list just forgets your last gun in in there and it isnt applied, so until I can find it i can only remove all
        //Make a list of my old guns
        List<DroppedWeapon> MyOldGuns = new List<DroppedWeapon>();
        WeaponDescriptorCollection CurrentWeapons = Game.LocalPlayer.Character.Inventory.Weapons;
        foreach (WeaponDescriptor Weapon in CurrentWeapons)
        {
            WeaponVariation DroppedGunVariation = DataMart.Instance.Weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)Weapon.Hash);
            DroppedWeapon MyGun = new DroppedWeapon(Weapon, Vector3.Zero, DroppedGunVariation, Weapon.Ammo);
            MyOldGuns.Add(MyGun);
        }
        //Totally clear our guns
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        //Add out guns back with variations
        foreach (DroppedWeapon MyNewGun in MyOldGuns)
        {
            WeaponInformation MyGTANewGun = DataMart.Instance.Weapons.GetWeapon((ulong)MyNewGun.Weapon.Hash);
            if (MyGTANewGun == null || MyGTANewGun.IsLegal)//or its an addon gun
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(MyNewGun.Weapon.Hash, (short)MyNewGun.Ammo, false);
                MyNewGun.Variation.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)MyNewGun.Weapon.Hash);
                NativeFunction.CallByName<bool>("ADD_AMMO_TO_PED", Game.LocalPlayer.Character, (uint)MyNewGun.Weapon.Hash, MyNewGun.Ammo + 1);
            }
        }
    }
    private void ResetPlayer(bool resetWanted, bool resetHealth, bool resetTimesDied, bool clearWeapons)
    {
        Mod.Player.Instance.Reset(resetWanted, resetTimesDied, clearWeapons);
        Mod.Player.Instance.UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.CallByName<uint>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        NativeFunction.Natives.xC0AA53F866B3134D();
        Game.TimeScale = 1f;
        NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
        NativeFunction.Natives.x80C8B1846639BB19(0);
        if (resetHealth)
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        }
        NativeFunction.CallByName<bool>("RESET_HUD_COMPONENT_VALUES", 0);
        NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
        NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 0f);
        //Audio.Instance.Abort();//moved into the scanner abort function
        Mod.World.Instance.AbortScanner();
    }
    private void Respawn(bool resetWanted, bool resetHealth, bool resetTimesDied, bool clearWeapons)
    {
        try
        {
            ResurrectPlayer(resetTimesDied);
            ResetPlayer(resetWanted, resetHealth, resetTimesDied, clearWeapons);    
            Game.HandleRespawn();
            Mod.World.Instance.UnPauseTime();
            GameTimeLastRespawned = Game.GameTime;
        }
        catch (Exception e)
        {
            Debug.Instance.WriteToLog("RespawnInPlace", e.Message);
        }
    }
    private void ResurrectPlayer(bool resetTimesDied)
    {
        if (!resetTimesDied)
        {
            ++Mod.Player.Instance.TimesDied;
        }
        NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);//"NETWORK_REQUEST_CONTROL_OF_ENTITY" 
        NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
        if (Mod.Player.Instance.DiedInVehicle)
        {
            NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X + 10f, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);//"NETWORK_RESURRECT_LOCAL_PLAYER"
            if (Game.LocalPlayer.Character.LastVehicle.Exists() && Game.LocalPlayer.Character.LastVehicle.IsDriveable)
            {
                Game.LocalPlayer.Character.WarpIntoVehicle(Game.LocalPlayer.Character.LastVehicle, -1);
            }
        }
        else
        {
            NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);//"NETWORK_RESURRECT_LOCAL_PLAYER"
        }
    }
    private void SetHospitalFee(string HospitalName)
    {
        int HospitalFee = DataMart.Instance.Settings.SettingsManager.Police.HospitalFee * (1 + Mod.Player.Instance.MaxWantedLastLife);
        int CurrentCash = Mod.Player.Instance.Money;
        int TodaysPayment = 0;

        int TotalNeededPayment = HospitalFee + HospitalBillPastDue;

        if (TotalNeededPayment > CurrentCash)
        {
            HospitalBillPastDue = TotalNeededPayment - CurrentCash;
            TodaysPayment = CurrentCash;
        }
        else
        {
            HospitalBillPastDue = 0;
            TodaysPayment = TotalNeededPayment;
        }

        Game.DisplayNotification("CHAR_BANK_FLEECA", "CHAR_BANK_FLEECA", HospitalName, "Hospital Fees", string.Format("Todays Bill: ~r~${0}~s~~n~Payment Today: ~g~${1}~s~~n~Outstanding: ~r~${2}", HospitalFee, TodaysPayment, HospitalBillPastDue));

        Mod.Player.Instance.GiveMoney(-1 * TodaysPayment);
    }
    private void SetPlayerAtLocation(GameLocation ToSet)
    {
        Game.LocalPlayer.Character.Position = ToSet.LocationPosition;
        Game.LocalPlayer.Character.Heading = ToSet.Heading;
        if (ToSet.Type == LocationType.Grave)
        {
            Game.LocalPlayer.Character.IsRagdoll = true;
        }
        Game.LocalPlayer.Character.Tasks.ClearImmediately();
    }
    private void SetPoliceFee(string PoliceStationName, int BailFee)
    {
        int CurrentCash = Mod.Player.Instance.Money;
        int TodaysPayment = 0;

        int TotalNeededPayment = BailFee + BailFeePastDue;

        if (TotalNeededPayment > CurrentCash)
        {
            BailFeePastDue = TotalNeededPayment - CurrentCash;
            TodaysPayment = CurrentCash;
        }
        else
        {
            BailFeePastDue = 0;
            TodaysPayment = TotalNeededPayment;
        }

        bool LesterHelp = RandomItems.RandomPercent(20);
        if (!LesterHelp)
        {
            Game.DisplayNotification("CHAR_BANK_FLEECA", "CHAR_BANK_FLEECA", PoliceStationName, "Bail Fees", string.Format("Todays Bill: ~r~${0}~s~~n~Payment Today: ~g~${1}~s~~n~Outstanding: ~r~${2}", BailFee, TodaysPayment, BailFeePastDue));
            Mod.Player.Instance.GiveMoney(-1 * TodaysPayment);
        }
        else
        {
            Game.DisplayNotification("CHAR_LESTER", "CHAR_LESTER", PoliceStationName, "Bail Fees", string.Format("~g~${0} ~s~", 0));
        }
    }
}


