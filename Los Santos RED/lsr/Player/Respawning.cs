using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Respawning// : IRespawning
{
    private int BailFee;
    private int BailFeePastDue;
    private IRespawnable CurrentPlayer;
    private uint GameTimeLastBribedPolice;
    private uint GameTimeLastPaidFine;
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
    private List<string> BribedCopResponses;
    private List<string> CitationCopResponses;
    private int RequiredBribeAmount;
    private int BailDuration;
    private DateTime BailPostingTime;
    private string BailReport;
    private DateTime HospitalDischargeDate;
    private string HospitalStayReport;
    private int HospitalFee;

    public Respawning(ITimeControllable time, IEntityProvideable world, IRespawnable currentPlayer, IWeapons weapons, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Time = time;
        World = world;
        CurrentPlayer = currentPlayer;
        Weapons = weapons;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
    }
    public bool RecentlyRespawned => GameTimeLastRespawned != 0 && Game.GameTime - GameTimeLastRespawned <= Settings.SettingsManager.RespawnSettings.RecentlyRespawnedTime;
    public bool RecentlyResistedArrest => GameTimeLastResistedArrest != 0 && Game.GameTime - GameTimeLastResistedArrest <= Settings.SettingsManager.RespawnSettings.RecentlyResistedArrestTime;



    public bool RecentlyBribedPolice => GameTimeLastBribedPolice != 0 && Game.GameTime - GameTimeLastBribedPolice <= 30000;
    public bool RecentlyPaidFine => GameTimeLastPaidFine != 0 && Game.GameTime - GameTimeLastPaidFine <= 30000;
    public bool CanUndie => TimesDied < Settings.SettingsManager.RespawnSettings.UndieLimit || Settings.SettingsManager.RespawnSettings.UndieLimit == 0;
    public int TimesDied { get; private set; }
    public void Reset()
    {
        TimesDied = 0;
    }
    public void Setup()
    {
        BribedCopResponses = new List<string>()
        { 
            "Thanks for the cash, you've got ~r~30 seconds~s~ to get lost.",
            "If I can see you in ~r~30 seconds~s~ you will regret it.",
            "I'll give you ~r~30 seconds~s~ to get the fuck outta here.",
            "Make like a tree and get outta here. You've got ~r~30 seconds~s~.",
            "Fuck off punk. T-Minus ~r~30 seconds~s~ to an ass beating.",
            "You wanna go to jail or you wanna go home? You've got ~r~30 seconds~s~ to decide.",
            "Pleasure doing business douchebag. You've got ~r~30 seconds~s~ to fuck off.",
        };

    }
    public bool BribePolice(int Amount)
    {
        CalculateBribe();
        if (CurrentPlayer.Money < Amount)
        {
            Game.DisplayNotification("CHAR_BANK_FLEECA", "CHAR_BANK_FLEECA", "FLEECA Bank", "Overdrawn Notice", string.Format("Current transaction would overdraw account. Denied.", Amount));
            return false;
        }
        else if (Amount < RequiredBribeAmount)//(CurrentPlayer.WantedLevel * Settings.SettingsManager.RespawnSettings.PoliceBribeWantedLevelScale))
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", EntryPoint.OfficerFriendlyContactName, "Expedited Service Fee", string.Format("Thats it? ~r~${0}~s~?", Amount));
            if (Settings.SettingsManager.RespawnSettings.DeductMoneyOnFailedBribe)
            {
                CurrentPlayer.GiveMoney(-1 * Amount);
            }
            return false;
        }
        else
        {
            ResetPlayer(true, false, false, false, true, false, false, false, false, false, false, false, false);
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", EntryPoint.OfficerFriendlyContactName, "~r~Expedited Service Fee", BribedCopResponses.PickRandom());
            CurrentPlayer.GiveMoney(-1 * Amount);
            GameTimeLastBribedPolice = Game.GameTime;
            CurrentPlayer.CellPhone.AddScheduledContact(EntryPoint.OfficerFriendlyContactName, "CHAR_BLANK_ENTRY", "", Time.CurrentDateTime.AddMinutes(2));            
            return true;
        }
    }
    public void PayoffPolice()
    {
        GameTimeLastBribedPolice = Game.GameTime;
    }
    public bool PayFine()
    {
        int FineAmount = CurrentPlayer.FineAmount();
        if (CurrentPlayer.Money < FineAmount)
        {
            Game.DisplayNotification("CHAR_BANK_FLEECA", "CHAR_BANK_FLEECA", "FLEECA Bank", "Overdrawn Notice", string.Format("Current transaction would overdraw account. Denied.", FineAmount));
            return false;
        }
        else
        {
            ResetPlayer(true, false, false, false, true, false, false, false, false, false, false, false, false);


            CitationCopResponses = new List<string>()
                {
                    $"Thank you for paying the citation amount of ~r~${FineAmount}~s~. Fuck off before you regret it.",
                    $"You have paid the citation amount of ~r~${FineAmount}~s~, now fuck off.",
                    $"Citation of ~r~${FineAmount}~s~ paid. Move along."
                };


            Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", EntryPoint.OfficerFriendlyContactName, "~o~Citation", CitationCopResponses.PickRandom());
            CurrentPlayer.GiveMoney(-1 * FineAmount);
            GameTimeLastPaidFine = Game.GameTime;
            return true;
        }
    }
    public void ResistArrest()
    {

        ResetPlayer(false, false, false, false, false, false, false, false, false, false, false, false, false);
        GameTimeLastResistedArrest = Game.GameTime;
    }
    public void RespawnAtCurrentLocation(bool withInvicibility, bool resetWanted, bool clearCriminalHistory, bool clearInventory)
    {
        if (CanUndie)
        {
            Respawn(resetWanted, true, false, false, clearCriminalHistory, clearInventory, false, false, false, false, false, false, false);
            CurrentPlayer.SetWantedLevel(CurrentPlayer.MaxWantedLastLife, "RespawnAtCurrentLocation", true);
            if (withInvicibility & Settings.SettingsManager.RespawnSettings.InvincibilityOnRespawn)
            {
               // CurrentPlayer.BigMessage.ShowMissionPassedMessage("Invincible", Settings.SettingsManager.RespawnSettings.RespawnInvincibilityTime);
                Game.LocalPlayer.Character.IsInvincible = true;
                GameFiber.StartNew(delegate
                {
                    GameFiber.Sleep(Settings.SettingsManager.RespawnSettings.RespawnInvincibilityTime);
                    Game.LocalPlayer.Character.IsInvincible = false;
                });
            }
            GameTimeLastUndied = Game.GameTime;
        }
    }
    public void RespawnAtHospital(Hospital PlaceToSpawn)
    {
        FadeOut();
        if (Settings.SettingsManager.RespawnSettings.RemoveWeaponsOnDeath)
        {
            CheckWeapons();
        }
        CalculateHospitalStay();
        Respawn(true, true, true, false, true, false, true, false, false, false, false, false, true);//we are already removing the weapons above, done need to do it twice with the old bug
        CurrentPlayer.PlayerTasks.OnStandardRespawn();
        if (PlaceToSpawn == null)
        {
            PlaceToSpawn = PlacesOfInterest.PossibleLocations.Hospitals.OrderBy(x => Game.LocalPlayer.Character.Position.DistanceTo2D(x.EntrancePosition)).FirstOrDefault();
        }
        SetPlayerAtLocation(PlaceToSpawn);
        if (Settings.SettingsManager.RespawnSettings.ClearIllicitInventoryOnDeath)
        {
            RemoveIllicitInventoryItems();
        }
        World.ClearSpawned();
        Time.SetDateTime(HospitalDischargeDate);
        GameFiber.Sleep(2000);
        FadeIn();
        if (Settings.SettingsManager.RespawnSettings.DeductHospitalFee)
        {
            SetHospitalFee(PlaceToSpawn.Name);
        }
        GameTimeLastDischargedFromHospital = Game.GameTime;
    }
    public void SurrenderToPolice(PoliceStation PoliceStation)
    {
        FadeOut();
        if (Settings.SettingsManager.RespawnSettings.RemoveWeaponsOnSurrender)
        {
            CheckWeapons();
        }
        CalculateBail();
        CurrentPlayer.RaiseHands();
        ResetPlayer(true, true, false, false, true, false, true,false, false, false, false, false,true);//if you pass clear weapons here it will just remover everything anwyays
        CurrentPlayer.PlayerTasks.OnStandardRespawn();
        if (PoliceStation == null)
        {
            PoliceStation = PlacesOfInterest.PossibleLocations.PoliceStations.OrderBy(x => Game.LocalPlayer.Character.Position.DistanceTo2D(x.EntrancePosition)).FirstOrDefault();
        }
        SetPlayerAtLocation(PoliceStation);
        if (Settings.SettingsManager.RespawnSettings.ClearIllicitInventoryOnSurrender)
        {
            RemoveIllicitInventoryItems();
        }
        World.ClearSpawned();
        Time.SetDateTime(BailPostingTime);
        GameFiber.Sleep(2000);
        FadeIn();
        if (Settings.SettingsManager.RespawnSettings.DeductBailFee)
        {
            SetBailFee(PoliceStation.Name, BailFee);
        }
        GameTimeLastSurrenderedToPolice = Game.GameTime;
    }
    private void CheckWeapons()
    {
        if (CurrentPlayer.Licenses.HasCCWLicense && CurrentPlayer.Licenses.CCWLicense.IsValid(Time))//need to add something like this back
        {
            RemoveIllegalWeapons();
        }
        else
        {
            Game.LocalPlayer.Character.Inventory.Weapons.Clear();//ResetPlayer is also doing this already......, if you add the above need to stop that from clearing everything anyways (this was that old bug lol), removed from doing that
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
    private void RemoveIllicitInventoryItems()
    {
        foreach(InventoryItem ii in CurrentPlayer.Inventory.Items.ToList())
        {
            if(ii.ModItem != null && ii.ModItem.IsPossessionIllicit)
            {
                CurrentPlayer.Inventory.Remove(ii.ModItem);
            }
        }
    }
    private void CalculateHospitalStay()
    {
        int HighestWantedLevel = CurrentPlayer.WantedLevel;
        HospitalFee = Settings.SettingsManager.RespawnSettings.HospitalFee * (1 + HighestWantedLevel);
        int DaysToStay = RandomItems.GetRandomNumberInt(Settings.SettingsManager.RespawnSettings.HospitalStayMinDays, Settings.SettingsManager.RespawnSettings.HospitalStayMaxDays);
        HospitalDischargeDate = Time.CurrentDateTime.AddDays(DaysToStay);
        HospitalStayReport = $"~s~Hospitalized Days: ~g~{DaysToStay}~s~~n~Released: {HospitalDischargeDate:g}~s~";
        EntryPoint.WriteToConsole($"CalculateHospitalStay(): HighestWantedLevel {HighestWantedLevel} HospitalFee {HospitalFee} HospitalDischargeDate {HospitalDischargeDate:g}");
    }
    private void CalculateBail()
    {
        int PoliceKilled = CurrentPlayer.PoliceResponse.PoliceKilled;
        int PoliceInjured = CurrentPlayer.PoliceResponse.PoliceHurt;
        int CiviliansKilled = CurrentPlayer.PoliceResponse.CiviliansKilled;
        int HighestWantedLevel = CurrentPlayer.WantedLevel;

        BailFee = HighestWantedLevel * Settings.SettingsManager.RespawnSettings.PoliceBailWantedLevelScale;//max wanted last life wil get reset when calling resetplayer
        BailFee += PoliceKilled * Settings.SettingsManager.RespawnSettings.PoliceBailWantedLevelScale;
        BailFee += PoliceInjured * Settings.SettingsManager.RespawnSettings.PoliceBailPoliceInjuredMultiplier;
        BailFee += CiviliansKilled * Settings.SettingsManager.RespawnSettings.PoliceBailCiviliansKilledMultiplier;


        BailDuration = HighestWantedLevel * Settings.SettingsManager.RespawnSettings.PoliceBailDurationWantedLevelScale;//max wanted last life wil get reset when calling resetplayer
        BailDuration += PoliceKilled * Settings.SettingsManager.RespawnSettings.PoliceBailDurationWantedLevelScale;
        BailDuration += PoliceInjured * Settings.SettingsManager.RespawnSettings.PoliceBailDurationPoliceInjuredMultiplier;
        BailDuration += CiviliansKilled * Settings.SettingsManager.RespawnSettings.PoliceBailDurationCiviliansKilledMultiplier;

        BailPostingTime = Time.CurrentDateTime.AddDays(BailDuration);
        BailPostingTime = new DateTime(BailPostingTime.Year, BailPostingTime.Month, BailPostingTime.Day, 9, 0, 0);

        BailReport = $"~s~Incarcerated Days: ~r~{BailDuration}~s~~n~Released: {BailPostingTime:g}~s~";
        EntryPoint.WriteToConsole($"CalculateBail(): HighestWantedLevel {HighestWantedLevel} PoliceKilled {PoliceKilled} PoliceInjured {PoliceInjured} CiviliansKilled {CiviliansKilled} BailFee {BailFee} BailDuration {BailDuration} BailPostingTime {BailPostingTime:g}");
    }
    private void CalculateBribe()
    {
        int PoliceKilled = CurrentPlayer.PoliceResponse.PoliceKilled;
        int PoliceInjured = CurrentPlayer.PoliceResponse.PoliceHurt;
        int HighestWantedLevel = CurrentPlayer.WantedLevel;

        RequiredBribeAmount = HighestWantedLevel * Settings.SettingsManager.RespawnSettings.PoliceBribeWantedLevelScale;//max wanted last life wil get reset when calling resetplayer
        RequiredBribeAmount += PoliceKilled * Settings.SettingsManager.RespawnSettings.PoliceBribePoliceKilledMultiplier;
        RequiredBribeAmount += PoliceInjured * Settings.SettingsManager.RespawnSettings.PoliceBribePoliceInjuredMultiplier;
    }
    private void RemoveIllegalWeapons()
    {
        //Needed cuz for some reason the other weapon list just forgets your last gun in in there and it isnt applied, so until I can find it i can only remove all
        //Make a list of my old guns
        List<StoredWeapon> MyOldGuns = new List<StoredWeapon>();
        WeaponDescriptorCollection CurrentWeapons = Game.LocalPlayer.Character.Inventory.Weapons;
        foreach (WeaponDescriptor Weapon in CurrentWeapons)
        {
            WeaponVariation DroppedGunVariation = Weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)Weapon.Hash);
            StoredWeapon MyGun = new StoredWeapon((uint)Weapon.Hash, Vector3.Zero, DroppedGunVariation, Weapon.Ammo);
            MyOldGuns.Add(MyGun);
        }
        //Totally clear our guns
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        //Add out guns back with variations
        foreach (StoredWeapon MyNewGun in MyOldGuns)
        {
            WeaponInformation MyGTANewGun = Weapons.GetWeapon((ulong)MyNewGun.WeaponHash);
            if (MyGTANewGun == null || MyGTANewGun.IsLegal)//or its an addon gun
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(MyNewGun.WeaponHash, (short)MyNewGun.Ammo, false);
                MyGTANewGun.ApplyWeaponVariation(Game.LocalPlayer.Character, MyNewGun.Variation);
                NativeFunction.CallByName<bool>("ADD_AMMO_TO_PED", Game.LocalPlayer.Character, (uint)MyNewGun.WeaponHash, MyNewGun.Ammo + 1);
            }
        }
    }
    private void ResetPlayer(bool resetWanted, bool resetHealth, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory, bool clearInventory, bool clearIntoxication, bool resetGangRelationships, bool clearVehicleOwnership, bool resetCellphone, bool clearActiveTasks, bool clearProperties, bool resetNeeds)
    {
        CurrentPlayer.Reset(resetWanted, resetTimesDied, clearWeapons, clearCriminalHistory, clearInventory, clearIntoxication, resetGangRelationships, clearVehicleOwnership, resetCellphone, clearActiveTasks, clearProperties, resetHealth, resetNeeds);
       // CurrentPlayer.UnSetArrestedAnimation();
        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.CallByName<uint>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        NativeFunction.Natives.xC0AA53F866B3134D();//FORCE_GAME_STATE_PLAYING
        if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnDeath)
        {
            Game.TimeScale = 1f;
        }
        if (clearIntoxication)
        {
            NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
            NativeFunction.Natives.x80C8B1846639BB19(0);//_SET_CAM_EFFECT (0 = cancelled)

            //new for drunk stuff
            NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
            NativeFunction.CallByName<int>("STOP_GAMEPLAY_CAM_SHAKING", true);
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
            NativeFunction.CallByName<bool>("RESET_PED_MOVEMENT_CLIPSET", Game.LocalPlayer.Character);
            NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, false);
        }

        NativeFunction.CallByName<bool>("RESET_HUD_COMPONENT_VALUES", 0);
        NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
        NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 0f);


        CurrentPlayer.UnSetArrestedAnimation();


    }
    private void Respawn(bool resetWanted, bool resetHealth, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory, bool clearInventory, bool clearIntoxication, bool resetGangRelationships, bool clearOwnedVehicles, bool resetCellphone, bool clearActiveTasks, bool clearProperties, bool resetNeeds)
    {
        try
        {
            ResurrectPlayer(resetTimesDied);
            ResetPlayer(resetWanted, resetHealth, resetTimesDied, clearWeapons, clearCriminalHistory, clearInventory, clearIntoxication, resetGangRelationships, clearOwnedVehicles, resetCellphone, clearActiveTasks, clearProperties, resetNeeds);
            Game.HandleRespawn();
            Time.UnPauseTime();
            GameTimeLastRespawned = Game.GameTime;
        }
        catch (Exception e)
        {
            EntryPoint.WriteToConsole("RespawnInPlace" + e.Message + e.StackTrace, 0);
        }
    }
    private void ResurrectPlayer(bool resetTimesDied)
    {
        if (!resetTimesDied)
        {
            ++TimesDied;
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
        Game.DisplayNotification("CHAR_BANK_FLEECA", "CHAR_BANK_FLEECA", HospitalName, "Hospital Fees", string.Format("Todays Bill: ~r~${0}~s~~n~Payment Today: ~g~${1}~s~~n~Outstanding: ~r~${2}~s~ ~n~{3}", HospitalFee, TodaysPayment, HospitalBillPastDue, HospitalStayReport));
        CurrentPlayer.GiveMoney(-1 * TodaysPayment);
    }
    private void SetPlayerAtLocation(BasicLocation ToSet)
    {
        Game.LocalPlayer.Character.Position = ToSet.EntrancePosition;
        Game.LocalPlayer.Character.Heading = ToSet.EntranceHeading;
        EntryPoint.FocusCellX = (int)(ToSet.EntrancePosition.X / EntryPoint.CellSize);
        EntryPoint.FocusCellY = (int)(ToSet.EntrancePosition.Y / EntryPoint.CellSize);
        if (ToSet.HasInterior)
        {
            World.Places.StaticPlaces.ActivateBasicLocation(ToSet);
        }
        NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Game.LocalPlayer.Character);
    }
    private void SetBailFee(string PoliceStationName, int BailFee)
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
        bool LesterHelp = RandomItems.RandomPercent(1);
        if (!LesterHelp)
        {
            Game.DisplayNotification("CHAR_BANK_FLEECA", "CHAR_BANK_FLEECA", PoliceStationName, "Bail Fees", string.Format("Todays Bill: ~r~${0}~s~~n~Payment Today: ~g~${1}~s~~n~Outstanding: ~r~${2}~s~ ~n~{3}", BailFee, TodaysPayment, BailFeePastDue, BailReport));
            CurrentPlayer.GiveMoney(-1 * TodaysPayment);
        }
        else
        {
            Game.DisplayNotification("CHAR_LESTER", "CHAR_LESTER", PoliceStationName, "Bail Fees", string.Format("~g~${0} ~s~", 0));
        }
    }

}


