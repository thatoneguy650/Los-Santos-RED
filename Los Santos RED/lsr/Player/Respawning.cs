using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Respawning : IRespawning
{
    private int BailFee;
    private int BailFeePastDue;
    private IRespawnable CurrentPlayer;
    private uint GameTimeLastBribedPolice;
    private uint GameTimeLastDischargedFromHospital;
    private uint GameTimeLastResistedArrest;
    private uint GameTimeLastRespawned;
    private uint GameTimeLastSurrenderedToPolice;
    private uint GameTimeLastUndied;
    private int HospitalBillPastDue;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private IWeapons Weapons;
    private IEntityProvideable World;
    public Respawning(ITimeControllable time, IEntityProvideable world, IRespawnable currentPlayer, IWeapons weapons, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Time = time;
        World = world;
        CurrentPlayer = currentPlayer;
        Weapons = weapons;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
    }
    public bool RecentlyBribedPolice => GameTimeLastBribedPolice != 0 && Game.GameTime - GameTimeLastBribedPolice <= 10000;
    public bool RecentlyRespawned => GameTimeLastRespawned != 0 && Game.GameTime - GameTimeLastRespawned <= 1000;
    public bool RecentlySurrenderedToPolice => GameTimeLastSurrenderedToPolice != 0 && Game.GameTime - GameTimeLastSurrenderedToPolice <= 5000;
    public void BribePolice(int Amount)
    {
        if (CurrentPlayer.Money < Amount)
        {
            Game.DisplayNotification("CHAR_BANK_FLEECA", "CHAR_BANK_FLEECA", "FLEECA Bank", "Overdrawn Notice", string.Format("Current transaction would overdraw account. Denied.", Amount));
        }
        else if (Amount < (CurrentPlayer.WantedLevel * Settings.SettingsManager.Police.PoliceBribeWantedLevelScale))
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "Officer Friendly", "Expedited Service Fee", string.Format("Thats it? ${0}?", Amount));
            CurrentPlayer.GiveMoney(-1 * Amount);
        }
        else
        {
            ResetPlayer(true, false, false, false, true);
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "Officer Friendly", "Expedited Service Fee", "Thanks for the cash, now beat it.");
            CurrentPlayer.GiveMoney(-1 * Amount);
            GameTimeLastBribedPolice = Game.GameTime;
        }
    }
    public void ResistArrest()
    {
        ResetPlayer(false, false, false, false, false);
        GameTimeLastResistedArrest = Game.GameTime;
    }
    public void RespawnAtCurrentLocation(bool withInvicibility, bool resetWanted, bool clearCriminalHistory)
    {
        if (CurrentPlayer.CanUndie)
        {
            Respawn(resetWanted, true, false, false, clearCriminalHistory);
            CurrentPlayer.PoliceResponse.SetWantedLevel(CurrentPlayer.MaxWantedLastLife, "RespawnAtCurrentLocation", true);
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
    }
    public void RespawnAtGrave()
    {
        FadeOut();
        Respawn(true, true, true, true, true);
        GameLocation PlaceToSpawn = PlacesOfInterest.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Grave);
        SetPlayerAtLocation(PlaceToSpawn);
        World.ClearPolice();
        Game.LocalPlayer.Character.IsRagdoll = true;
        FadeIn();
        Game.LocalPlayer.Character.IsRagdoll = false;
        GameTimeLastDischargedFromHospital = Game.GameTime;
    }
    public void RespawnAtHospital(GameLocation PlaceToSpawn)
    {
        FadeOut();
        Respawn(true, true, true, true, true);
        if (PlaceToSpawn == null)
        {
            PlaceToSpawn = PlacesOfInterest.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Hospital);
        }
        SetPlayerAtLocation(PlaceToSpawn);
        World.ClearPolice();
        FadeIn();
        SetHospitalFee(PlaceToSpawn.Name);
        GameTimeLastDischargedFromHospital = Game.GameTime;
    }
    public void SurrenderToPolice(GameLocation PoliceStation)
    {
        FadeOut();
        CheckWeapons();
        BailFee = CurrentPlayer.MaxWantedLastLife * Settings.SettingsManager.Police.PoliceBailWantedLevelScale;//max wanted last life wil get reset when calling resetplayer
        CurrentPlayer.RaiseHands();
        ResetPlayer(true, true, false, true, true);
        if (PoliceStation == null)
        {
            PoliceStation = PlacesOfInterest.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Police);
        }
        SetPlayerAtLocation(PoliceStation);
        World.ClearPolice();
        FadeIn();
        SetPoliceFee(PoliceStation.Name, BailFee);
        GameTimeLastSurrenderedToPolice = Game.GameTime;
    }
    private void CheckWeapons()
    {
        //if (!CurrentPlayer.KilledAnyCops)//need to add something like this back
        //{
        //    RemoveIllegalWeapons();
        //}
        //else
        //{
            Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        //}
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
            WeaponVariation DroppedGunVariation = Weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)Weapon.Hash);
            DroppedWeapon MyGun = new DroppedWeapon(Weapon, Vector3.Zero, DroppedGunVariation, Weapon.Ammo);
            MyOldGuns.Add(MyGun);
        }
        //Totally clear our guns
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        //Add out guns back with variations
        foreach (DroppedWeapon MyNewGun in MyOldGuns)
        {
            WeaponInformation MyGTANewGun = Weapons.GetWeapon((ulong)MyNewGun.Weapon.Hash);
            if (MyGTANewGun == null || MyGTANewGun.IsLegal)//or its an addon gun
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(MyNewGun.Weapon.Hash, (short)MyNewGun.Ammo, false);
                MyGTANewGun.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)MyNewGun.Weapon.Hash, MyNewGun.Variation);
                NativeFunction.CallByName<bool>("ADD_AMMO_TO_PED", Game.LocalPlayer.Character, (uint)MyNewGun.Weapon.Hash, MyNewGun.Ammo + 1);
            }
        }
    }
    private void ResetPlayer(bool resetWanted, bool resetHealth, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory)
    {
        CurrentPlayer.Reset(resetWanted, resetTimesDied, clearWeapons, clearCriminalHistory);
        CurrentPlayer.UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.CallByName<uint>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        NativeFunction.Natives.xC0AA53F866B3134D();
        Game.TimeScale = 1f;
        NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
        NativeFunction.Natives.x80C8B1846639BB19(0);

        //new for drunk stuff
        NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
        NativeFunction.CallByName<int>("STOP_GAMEPLAY_CAM_SHAKING", true);
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
        NativeFunction.CallByName<bool>("RESET_PED_MOVEMENT_CLIPSET", Game.LocalPlayer.Character);
        NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, false);

        if (resetHealth)
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        }
        NativeFunction.CallByName<bool>("RESET_HUD_COMPONENT_VALUES", 0);
        NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
        NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 0f);
    }
    private void Respawn(bool resetWanted, bool resetHealth, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory)
    {
        try
        {
            ResurrectPlayer(resetTimesDied);
            ResetPlayer(resetWanted, resetHealth, resetTimesDied, clearWeapons, clearCriminalHistory);
            Game.HandleRespawn();
            Time.UnPauseTime();
            GameTimeLastRespawned = Game.GameTime;
        }
        catch (Exception e)
        {
            Game.Console.Print("RespawnInPlace" + e.Message + e.StackTrace);
        }
    }
    private void ResurrectPlayer(bool resetTimesDied)
    {
        if (!resetTimesDied)
        {
            ++CurrentPlayer.TimesDied;
        }
        NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);//"NETWORK_REQUEST_CONTROL_OF_ENTITY" 
        NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
        if (CurrentPlayer.DiedInVehicle)
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
        int HospitalFee = Settings.SettingsManager.Police.HospitalFee * (1 + CurrentPlayer.MaxWantedLastLife);
        int CurrentCash = CurrentPlayer.Money;
        int TotalNeededPayment = HospitalFee + HospitalBillPastDue;
        int TodaysPayment;
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
        CurrentPlayer.GiveMoney(-1 * TodaysPayment);
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
        int CurrentCash = CurrentPlayer.Money;
        int TotalNeededPayment = BailFee + BailFeePastDue;
        int TodaysPayment;
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
            CurrentPlayer.GiveMoney(-1 * TodaysPayment);
        }
        else
        {
            Game.DisplayNotification("CHAR_LESTER", "CHAR_LESTER", PoliceStationName, "Bail Fees", string.Format("~g~${0} ~s~", 0));
        }
    }
}


