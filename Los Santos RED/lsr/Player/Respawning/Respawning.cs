using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Respawning// : IRespawning
{
    private readonly string LesterContactPicture = "CHAR_LESTER";
    private readonly string BankContactPicture = "CHAR_BANK_FLEECA";
    private readonly string PoliceContactPicture = "CHAR_CALL911";
    private readonly string BlankContactPicture = "CHAR_BLANK_ENTRY";
    private readonly string BribeFailedResponse = "Thats it? ~r~${0}~s~?";


    private IRespawnable Player;
    private uint GameTimeLastBribedPolice;
    private uint GameTimeLastPaidFine;
    private uint GameTimeLastDischargedFromHospital;
    private uint GameTimeLastResistedArrest;
    private uint GameTimeLastRespawned;
    private uint GameTimeLastSurrenderedToPolice;
    private uint GameTimeLastUndied;

    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private IWeapons Weapons;
    private IEntityProvideable World;
    private IModItems ModItems;
    private List<string> BribedCopResponses;
    private List<string> CitationCopResponses;
    

    private string BailReport;
    private DateTime HospitalDischargeDate;
    private string HospitalStayReport;

    private uint GameTimeLastPlacedAtLocation;
    private IPoliceRespondable PoliceRespondable;
    private ISeatAssignable SeatAssignable;
    private GTANotification ImpoundNotification;
    private int TodaysPayment;
    private bool HasIllegalWeapons;
    private bool HasIllegalItems;

    public Respawning(ITimeControllable time, IEntityProvideable world, IRespawnable currentPlayer, IWeapons weapons, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IPoliceRespondable policeRespondable, ISeatAssignable seatAssignable, IModItems modItems)
    {
        Time = time;
        World = world;
        Player = currentPlayer;
        Weapons = weapons;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        PoliceRespondable = policeRespondable;
        SeatAssignable = seatAssignable;
        ModItems = modItems;
    }
    public bool RecentlyRespawned => GameTimeLastRespawned != 0 && Game.GameTime - GameTimeLastRespawned <= Settings.SettingsManager.RespawnSettings.RecentlyRespawnedTime;
    public bool RecentlyResistedArrest => GameTimeLastResistedArrest != 0 && Game.GameTime - GameTimeLastResistedArrest <= Settings.SettingsManager.RespawnSettings.RecentlyResistedArrestTime;
    public bool WasRecentlyTeleported => GameTimeLastPlacedAtLocation != 0 && Game.GameTime - GameTimeLastPlacedAtLocation <= 5000;
    public bool RecentlyBribedPolice => GameTimeLastBribedPolice != 0 && Game.GameTime - GameTimeLastBribedPolice <= 30000;
    public bool RecentlyPaidFine => GameTimeLastPaidFine != 0 && Game.GameTime - GameTimeLastPaidFine <= 30000;
    public bool CanUndie => TimesDied < Settings.SettingsManager.RespawnSettings.UndieLimit || Settings.SettingsManager.RespawnSettings.UndieLimit == 0;
    public int TimesDied { get; private set; }
    public int TimesTalked { get; private set; }
    public int RequiredBribeAmount { get; private set; }
    public int PastDueBailFees => BailFeePastDue;
    public int BailDuration { get; private set; }
    public DateTime BailPostingTime { get; private set; }
    public int HospitalDuration { get; private set; }
    public int HospitalFee { get; private set; }
    public int HospitalBillPastDue { get; private set; }
    public int BailFee { get; private set; }
    public int BailFeePastDue { get; private set; }
    public void PayPastDueBail()
    {
        BailFeePastDue = 0;
    }
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
    public bool BribePolice(ModUIMenu menu, PossibleBribe possibleBribe)
    {
        CalculateBribe();
        if (Player.BankAccounts.GetMoney(false) < possibleBribe.Amount)
        {
            Game.DisplayNotification(BlankContactPicture, BlankContactPicture, StaticStrings.OfficerFriendlyContactName, "~r~Cash Only", "You do not have enough cash on hand.");
            menu?.Show();
            NativeHelper.PlayErrorSound();
            return false;
        }


        bool isFailedBribe = possibleBribe.Amount < RequiredBribeAmount && !possibleBribe.AttemptBribe();
        BribeActivity bribeActivity = new BribeActivity(Player, World, Settings, ModItems) { IsFailed = isFailedBribe };
        bribeActivity.Setup();
        bribeActivity.Start();

        if (isFailedBribe)//(CurrentPlayer.WantedLevel * Settings.SettingsManager.RespawnSettings.PoliceBribeWantedLevelScale))
        {
            Game.DisplayNotification(BlankContactPicture, BlankContactPicture, StaticStrings.OfficerFriendlyContactName, "Expedited Service Fee", string.Format(BribeFailedResponse, possibleBribe.Amount));
            if (Settings.SettingsManager.RespawnSettings.DeductMoneyOnFailedBribe)
            {
                Player.BankAccounts.GiveMoney(-1 * possibleBribe.Amount, false);
            }
            NativeHelper.PlayErrorSound();
            menu?.Show();
            return false;
        }
        else
        {
            ResetPlayer(true, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
            Game.DisplayNotification(BlankContactPicture, BlankContactPicture, StaticStrings.OfficerFriendlyContactName, "~r~Expedited Service Fee", BribedCopResponses.PickRandom());
            Player.BankAccounts.GiveMoney(-1 * possibleBribe.Amount, true);
            GameTimeLastBribedPolice = Game.GameTime;
            NativeHelper.PlaySuccessSound();
            List<string> OfficerFriendlyResponses = new List<string>() { 
            "Thanks for the donation, give me a call if you are in a jam with the cops.",
            "Pleasure doing business. Hit me up when you've got issues with johnny law.",
            "Thanks for the cash. Give me a ring when the cops are crawling up your ass.",
            "Always nice to help out a friend. Remember this number if you've got cop problems in the future.",
            "As long as you've got the cash, I can take care of the cops.",

            };
            Player.Scanner.OnBribedPolice();
            if (Player.CellPhone.GetCorruptCopContact() != null)
            {
                return true;
            }
            CorruptCopContact toSend = Player.CellPhone.DefaultCorruptCopContact;
            if(toSend != null && !Player.CellPhone.HasPendingItems(toSend))
            {
                Player.CellPhone.AddScheduledText(toSend, OfficerFriendlyResponses.PickRandom(), 1, false);
            }
            return true;
        }
    }
    public void PayoffPolice()
    {
        GameTimeLastBribedPolice = Game.GameTime;
        Player.Scanner.OnBribedPolice();
    }
    public void PayFine()
    {
        int FineAmount = Player.FineAmount();
        ResetPlayer(true, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
        if (Player.BankAccounts.GetMoney(true) < FineAmount)
        {
            BailFeePastDue += FineAmount;       
            Game.DisplayNotification(PoliceContactPicture, PoliceContactPicture, "Summary", "~o~Citation", $"Citation of ~r~${FineAmount}~s~ has been added to your debt.");
        }
        else
        {
            CitationCopResponses = new List<string>()
                {
                    $"Thank you for paying the citation amount of ~r~${FineAmount}~s~. Fuck off before you regret it.",
                    $"You have paid the citation amount of ~r~${FineAmount}~s~, now fuck off.",
                    $"Citation of ~r~${FineAmount}~s~ paid. Move along."
                };
            Game.DisplayNotification(PoliceContactPicture, PoliceContactPicture, "Summary", "~o~Citation", CitationCopResponses.PickRandom());
            Player.BankAccounts.GiveMoney(-1 * FineAmount, true);
        }
        GameTimeLastPaidFine = Game.GameTime;
        Player.Scanner.OnPaidFine();
    }
    public void GetBooked(ILocationRespawnable respawnableLocation)
    {
        Player.IsBeingBooked = true;
        while (Player.IsIncapacitated)
        {
            GameFiber.Sleep(500);
        }
        EntryPoint.WriteToConsole("GetBooked Start");
        VehicleExt vehicleToSearch = GetVehicleToSearch();
        SearchActivity searchActivity = new SearchActivity(Player, World, PoliceRespondable, SeatAssignable, Settings, Time, ModItems, vehicleToSearch, Weapons);
        searchActivity.Setup();
        searchActivity.Start();
        if (!searchActivity.IsActive)
        {
            EntryPoint.WriteToConsole("GetBooked Search Didnt start, ending");
            SkipBooking(respawnableLocation);
            return;
        }
        GameFiber.StartNew(delegate
        {
            try
            {
                EntryPoint.WriteToConsole("GetBooked Waiting for search acitivty start");
                Player.ButtonPrompts.AddPrompt("GetBooked", "Skip To Booking", "SkipBooking", GameControl.Attack, 15);
                bool shouldSkip = false;
                while (searchActivity.IsActive)
                {
                    if(Player.ButtonPrompts.IsPressed("SkipBooking"))
                    {
                        shouldSkip = true;
                        break;
                    }
                    GameFiber.Yield();
                }
                if(shouldSkip)
                {
                    EntryPoint.WriteToConsole("GetBooked skip triggered 1");
                    SkipBooking(respawnableLocation);
                    return;
                }
                EntryPoint.WriteToConsole("GetBooked Search Ended");
                if (!searchActivity.CompletedSearch)
                {
                    EntryPoint.WriteToConsole("GetBooked search didnt complete, ending with skip");
                    SkipBooking(respawnableLocation);
                    return;
                }
                BookingActivity bookingActivity = new BookingActivity(Player, World, PoliceRespondable, respawnableLocation, SeatAssignable, Settings);
                bookingActivity.Setup();
                bookingActivity.Start();
                EntryPoint.WriteToConsole("GetBooked Bookingactivty started");
                while (bookingActivity.IsActive)
                {
                    if (Player.ButtonPrompts.IsPressed("SkipBooking"))
                    {
                        shouldSkip = true;
                        break;
                    }
                    GameFiber.Yield();
                }
                if (shouldSkip)
                {
                    EntryPoint.WriteToConsole("GetBooked skip triggered 2");
                    SkipBooking(respawnableLocation);
                    return;
                }
                while (Player.IsArrested && EntryPoint.ModController.IsRunning && Player.IsInVehicle)
                {
                    if (Player.ButtonPrompts.IsPressed("SkipBooking"))
                    {
                        shouldSkip = true;
                        break;
                    }
                    GameFiber.Yield();
                }
                if (shouldSkip)
                {
                    EntryPoint.WriteToConsole("GetBooked skip triggered 2");
                    SkipBooking(respawnableLocation);
                    return;
                }
                //Finally just fade out as we cant escape yet

                if (Player.IsArrested)
                {
                    SkipBooking(respawnableLocation);
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Searching");
    }
    private void SkipBooking(ILocationRespawnable respawnableLocation)
    {
        SurrenderToPolice(respawnableLocation);
        Player.IsBeingBooked = false;
        Player.ButtonPrompts.RemovePrompts("GetBooked");
    }

    public void AskAboutCrimes()
    {
        List<string> AttemptTalkOut = new List<string>()
            {
                //$"What seems to be the problem officer?",
                $"What did I ~y~allegedly~s~ do?",
                $"What did you ~y~bust~s~ me for?",
                $"What is the ~y~reason for the stop~s~?",
                $"Why did you ~y~stop me~s~?",
                $"Why was I ~y~singled out~s~?",
            };
        Game.DisplaySubtitle("You: ~s~" + AttemptTalkOut.PickRandom());
        GameFiber.Sleep(4000);
        string CrimeDisplay = Player.PoliceResponse.PrintCrimes(false);
        List<string> TalkOutResponsePositive = new List<string>()
            {
                "{0}",
            };
        Game.DisplaySubtitle("~g~Cop: ~s~" + string.Format(TalkOutResponsePositive.PickRandom(), CrimeDisplay));
        GameFiber.Sleep(4000);
    }
    public bool TalkOutOfTicket(ModUIMenu menu)
    {
        TimesTalked++;
        List<string> AttemptTalkOut = new List<string>()
                {
                    $"It wasn't me officer, it was the one-armed man!",
                    $"I pay your salary just so you know.",
                    $"I don't know what you're talking about.",
                    $"I plead the fifth.",
                    $"I am a law abiding citizen and I will not take this harassment!",
                    $"That stuff? It isn't mine. I also didn't do it. Whatever it is.",
                    $"What seems to be the problem officer?",
                    $"Are you sure this is your jurisdiction?",
                    $"He went that way.",
                    $"Call my lawyer.",
                    $"I ain't saying nothing!",
                    $"...",
                    $"Am I going to be on ~y~The Underbelly Of Paradise~s~?",
                };
        Game.DisplaySubtitle("You: ~s~" + AttemptTalkOut.PickRandom());
        GameFiber.Sleep(4000);
        if (RandomItems.RandomPercent(Player.SpeechSkill))
        {
            List<string> TalkOutResponsePositive = new List<string>()
                {
                    $"I don't care enough for this shit. I'm outta here.",
                    $"I need to go tongue a ~p~Rusty Brown~s~ Ring Donut anyways.",
                    $"It's almost happy hour at ~p~Wigwam~s~, get outta here.",
                    $"I need me a bleeder burger at ~p~Burger Shot~s~ anyways. Get outta my sight.",
                    $"I've got enough paperwork already. Don't let me catch you again",
                    $"Whatever, ~y~Republican Space Rangers~s~ is almost on. Fuck off.",
                    $"It's your lucky day, I must have forgot my drop gun. Beat it.",
                    $"Whatever prick.",
                    $"You aren't worth my time.",
                };
            Game.DisplaySubtitle("~g~Cop: ~s~" + TalkOutResponsePositive.PickRandom());
            GameFiber.Sleep(4000);
            ResetPlayer(true, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
            Player.Scanner.OnTalkedOutOfTicket();
            return true;
        }
        else
        {
            List<string> TalkOutResponseNegative = new List<string>()
                {
                    $"Shut the fuck up prick.",
                    $"A most persuasive argument, too bad I don't give a fuck.",
                    $"Try having great tits next time.",
                    $"Next time you speak, you are getting a beating.",
                    $"Wanna try me again?",
                    $"This isn't Alderny prick.",
                    $"Are you trying to really piss me off?",
                    $"Next time bring cash.",
                    $"Does this work wherever the fuck you are from?",
                    $"You trying to sweet talk me asshole?",
                    $"Keep being smart with me and you'll be eating the pavement.",
                };
            Game.DisplaySubtitle("~r~Cop: ~s~" + TalkOutResponseNegative.PickRandom());
            menu?.Show();
            return false;
        }     
    }
    public void ResistArrest()
    {
        ResetPlayer(false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false);
        GameTimeLastResistedArrest = Game.GameTime;
    }
    public void RespawnAtCurrentLocation(bool withInvicibility, bool resetWanted, bool clearCriminalHistory, bool clearInventory)
    {
        if (CanUndie)
        {
            int wantedLevel = Player.WantedLevel;
            Respawn(resetWanted, true, false, false, clearCriminalHistory, clearInventory, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
            Player.SetWantedLevel(wantedLevel, "RespawnAtCurrentLocation", true);
            if (withInvicibility & Settings.SettingsManager.RespawnSettings.InvincibilityOnRespawn)
            {
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
    public void RespawnAtHospital(ILocationRespawnable respawnableLocation)
    {
        FadeOut();
        if (respawnableLocation == null)
        {
            respawnableLocation = PlacesOfInterest.HospitalRespawnLocations().ToList().OrderBy(x => Game.LocalPlayer.Character.Position.DistanceTo2D(x.EntrancePosition)).FirstOrDefault();
        }
        if (Settings.SettingsManager.RespawnSettings.RemoveWeaponsOnDeath)
        {
            RemoveIllegalWeapons();
        }
        CalculateHospitalStay();
        if(Player.IsWanted)
        {
            ImpoundNotification = ImpoundVehicles();
        }
        Respawn(true, true, true, false, true, false, true, false, false, false, false, false, true,true, false, true, true, false, false, false, true, false);//we are already removing the weapons above, done need to do it twice with the old bug
        Player.PlayerTasks.OnStandardRespawn();

        SetPlayerAtLocation(respawnableLocation);
        if (Settings.SettingsManager.RespawnSettings.ClearIllicitInventoryOnDeath)
        {
            Player.Inventory.RemoveIllicitInventoryItems();
        }
        EntryPoint.WriteToConsole($"PRE 1: {Time.CurrentDateTime} {HospitalDuration} {HospitalDischargeDate}");



        GameFiber.Sleep(4000);
        Time.SetDateTime(HospitalDischargeDate);
        Player.HumanState.SetRandom();
        FadeIn();
        if (Settings.SettingsManager.RespawnSettings.DeductHospitalFee)
        {
            SetHospitalFee(respawnableLocation.Name);
        }
        if(Settings.SettingsManager.RespawnSettings.RemoveOnHandCashOnDeath)
        {
            LoseOnHandCash();
        }
        ShowImpoundDisplay();
        GameTimeLastDischargedFromHospital = Game.GameTime;
        EntryPoint.WriteToConsole($"POST 1: {Time.CurrentDateTime} {HospitalDuration} {HospitalDischargeDate}");
    }
    private void LoseOnHandCash()
    {
        int CurrentCash = Player.BankAccounts.GetMoney(false);
        int PercentToRemove = RandomItems.GetRandomNumberInt(Settings.SettingsManager.RespawnSettings.RemoveOnHandCashOnDeathPercentageMin, Settings.SettingsManager.RespawnSettings.RemoveOnHandCashOnDeathPercentageMax);
        int CashToRemove = (int)Math.Floor(CurrentCash * ((float)PercentToRemove/100f));
        Player.BankAccounts.GiveMoney(-1 * CashToRemove, false);
    }
    public void SurrenderToPolice(ILocationRespawnable respawnableLocation)
    {
        FadeOut();
        if (respawnableLocation == null)
        {
            respawnableLocation = PlacesOfInterest.BustedRespawnLocations().ToList().OrderBy(x => Game.LocalPlayer.Character.Position.DistanceTo2D(x.EntrancePosition)).FirstOrDefault();
        }
        HasIllegalWeapons = false;
        if (Settings.SettingsManager.RespawnSettings.RemoveWeaponsOnSurrender)
        {
            HasIllegalWeapons = Player.WeaponEquipment.GetIllegalWeapons(Player.Licenses.HasValidCCWLicense(Time))?.Any() == true;
            RemoveIllegalWeapons();
        }    
        ImpoundNotification = ImpoundVehicles();
        ResetPlayer(true, true, false, false, true, false, true,false, false, false, false, false,true, true, false, true, true, false,false, false, true, false);//if you pass clear weapons here it will just remover everything anwyays
        Player.PlayerTasks.OnStandardRespawn();
        SetPlayerAtLocation(respawnableLocation);
        HasIllegalItems = false;
        if (Settings.SettingsManager.RespawnSettings.ClearIllicitInventoryOnSurrender)
        {
            HasIllegalItems = Player.Inventory.GetIllicitItems()?.Any() == true;
            Player.Inventory.RemoveIllicitInventoryItems();
        }
        EntryPoint.WriteToConsole($"PRE 1: {Time.CurrentDateTime} {BailDuration}");
        Time.SetDateTime(BailPostingTime);
        GameFiber.Sleep(4000);
        Player.HumanState.SetRandom();
        FadeIn();
        if (Settings.SettingsManager.RespawnSettings.DeductBailFee)
        {
            GenerateTotalBailFee();
        }
        DisplayBailNotification(respawnableLocation.Name);
        ShowImpoundDisplay();
        GameTimeLastSurrenderedToPolice = Game.GameTime;
        EntryPoint.WriteToConsole($"POST 1: {Time.CurrentDateTime} {BailDuration}");
    }
    private void ShowImpoundDisplay()
    {
        if(ImpoundNotification == null)
        {
            return;
        }
        ImpoundNotification.Display();
    }
    private GTANotification ImpoundVehicles()
    {
        if (!Settings.SettingsManager.RespawnSettings.ImpoundVehicles)
        {
            EntryPoint.WriteToConsole("IMPOUND VEHICLE FAIL NO SETTING");
            return null;
        }
        ILocationImpoundable impoundLocation = PlacesOfInterest.VehicleImpoundLocations().Where(x=> x.HasImpoundLot).ToList().OrderBy(x => Game.LocalPlayer.Character.Position.DistanceTo2D(x.EntrancePosition)).FirstOrDefault();     
        if(impoundLocation == null || impoundLocation.VehicleImpoundLot == null)
        {
            EntryPoint.WriteToConsole("IMPOUND VEHICLE FAIL NO LOCATION 1");
            return null;
        }
        VehicleExt vehicleToImpound = Player.VehicleOwnership.OwnedVehicles.Where(x => x.Vehicle.Exists()).OrderBy(x => x.Vehicle.DistanceTo2D(Player.Character)).FirstOrDefault();  
        if(vehicleToImpound == null || !vehicleToImpound.Vehicle.Exists() || vehicleToImpound.Vehicle.DistanceTo2D(Player.Character) >= 50f)
        {
            EntryPoint.WriteToConsole("IMPOUND VEHICLE FAIL NO VEHICLE TO IMPOUND");
            return null;
        }
        if(impoundLocation.VehicleImpoundLot.ImpoundVehicle(vehicleToImpound, Time, Player.Licenses.HasValidCCWLicense(Time), Weapons))
        {
            return new GTANotification(impoundLocation.Name, "~o~Vehicle Impounded~s~",vehicleToImpound.GetRegularDescription(true));
        }
        EntryPoint.WriteToConsole("IMPOUND VEHICLE FAIL CAN NOT IMPOUND");
        return null;
    }
    private void RemoveIllegalWeapons()
    {
        Player.WeaponEquipment.RemoveIllegalWeapons(Player.Licenses.HasValidCCWLicense(Time));     
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
    public void CalculateHospitalStay()
    {
        HospitalDuration = RandomItems.GetRandomNumberInt(Settings.SettingsManager.RespawnSettings.HospitalStayMinDays, Settings.SettingsManager.RespawnSettings.HospitalStayMaxDays);
        HospitalFee = Settings.SettingsManager.RespawnSettings.HospitalStayDailyFee * HospitalDuration;  
        //HospitalDischargeDate = Time.CurrentDateTime.AddDays(HospitalDuration);


        HospitalDischargeDate = Time.CurrentDateTime.AddDays(HospitalDuration);
        HospitalDischargeDate = new DateTime(HospitalDischargeDate.Year, HospitalDischargeDate.Month, HospitalDischargeDate.Day, 9, 0, 0);


        HospitalStayReport = $"~s~Hospitalized Days: ~g~{HospitalDuration}~s~~n~Released: {HospitalDischargeDate:g}~s~";
    }
    public void CalculateBailDurationAndFees()
    {
        int PoliceKilled = Player.PoliceResponse.PoliceKilled;
        int PoliceInjured = Player.PoliceResponse.PoliceHurt;
        int CiviliansKilled = Player.PoliceResponse.CiviliansKilled;
        int HighestWantedLevel = Player.WantedLevel;

        BailFee = HighestWantedLevel * Settings.SettingsManager.RespawnSettings.PoliceBailWantedLevelScale;//max wanted last life wil get reset when calling resetplayer
        BailFee += PoliceKilled * Settings.SettingsManager.RespawnSettings.PoliceBailPoliceKilledMultiplier;
        BailFee += PoliceInjured * Settings.SettingsManager.RespawnSettings.PoliceBailPoliceInjuredMultiplier;
        BailFee += CiviliansKilled * Settings.SettingsManager.RespawnSettings.PoliceBailCiviliansKilledMultiplier;

        BailDuration = HighestWantedLevel * Settings.SettingsManager.RespawnSettings.PoliceBailDurationWantedLevelScale;//max wanted last life wil get reset when calling resetplayer
        BailDuration += PoliceKilled * Settings.SettingsManager.RespawnSettings.PoliceBailDurationPoliceKilledMultiplier;
        BailDuration += PoliceInjured * Settings.SettingsManager.RespawnSettings.PoliceBailDurationPoliceInjuredMultiplier;
        BailDuration += CiviliansKilled * Settings.SettingsManager.RespawnSettings.PoliceBailDurationCiviliansKilledMultiplier;

        BailPostingTime = Time.CurrentDateTime.AddDays(BailDuration);
        BailPostingTime = new DateTime(BailPostingTime.Year, BailPostingTime.Month, BailPostingTime.Day, 9, 0, 0);
    }
    public void CalculateBribe()
    {
        int PoliceKilled = Player.PoliceResponse.PoliceKilled;
        int PoliceInjured = Player.PoliceResponse.PoliceHurt;
        int HighestWantedLevel = Player.WantedLevel;

        //RequiredBribeAmount = Settings.SettingsManager.RespawnSettings.PoliceBribeBase;
        RequiredBribeAmount = 0;
        RequiredBribeAmount += HighestWantedLevel * Settings.SettingsManager.RespawnSettings.PoliceBribeWantedLevelScale;//max wanted last life wil get reset when calling resetplayer
        RequiredBribeAmount += PoliceKilled * Settings.SettingsManager.RespawnSettings.PoliceBribePoliceKilledMultiplier;
        RequiredBribeAmount += PoliceInjured * Settings.SettingsManager.RespawnSettings.PoliceBribePoliceInjuredMultiplier;
    }
    private void ResetPlayer(bool resetWanted, bool resetHealth, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory, bool clearInventory, bool clearIntoxication, bool resetGangRelationships, 
        bool clearVehicleOwnership,bool resetCellphone, bool clearActiveTasks, bool clearProperties, bool resetNeeds, bool resetGroup, bool resetLicenses, bool resetActivites, bool resetGracePeriod, 
        bool resetBankAccounts, bool resetSavedGame, bool resetPendingMessages, bool resetInteriors, bool resetGambling)
    {
        Player.Reset(resetWanted, resetTimesDied, clearWeapons, clearCriminalHistory, clearInventory, clearIntoxication, resetGangRelationships, clearVehicleOwnership, resetCellphone, clearActiveTasks, clearProperties, resetHealth, resetNeeds,
            resetGroup, resetLicenses, resetActivites, resetGracePeriod, resetBankAccounts, resetSavedGame, resetPendingMessages, resetInteriors, resetGambling, false);
        // CurrentPlayer.UnSetArrestedAnimation();

        NativeFunction.Natives.SET_ENABLE_HANDCUFFS(Game.LocalPlayer.Character, false);

        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.CallByName<uint>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        NativeFunction.Natives.xC0AA53F866B3134D();//FORCE_GAME_STATE_PLAYING
        if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnDeath)
        {
            Game.TimeScale = 1f;
        }
        if(Settings.SettingsManager.PlayerOtherSettings.SetCutToBlackDeath && Game.IsScreenFadedOut)
        {
            Game.FadeScreenIn(0, false);
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
        //NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 0f);
        Player.Surrendering.UnSetArrestedAnimation();
    }
    private void Respawn(bool resetWanted, bool resetHealth, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory, bool clearInventory, bool clearIntoxication, bool resetGangRelationships, bool clearOwnedVehicles, bool resetCellphone,
        bool clearActiveTasks, bool clearProperties, bool resetNeeds, bool resetGroup, bool resetLicenses, bool resetActivites, bool resetGracePeriod, bool resetBankAccounts, bool resetSavedGame, bool resetPendingMessages, bool resetInteriors, bool resetGambling)
    {
        try
        {
            ResurrectPlayer(resetTimesDied);
            ResetPlayer(resetWanted, resetHealth, resetTimesDied, clearWeapons, clearCriminalHistory, clearInventory, clearIntoxication, resetGangRelationships, clearOwnedVehicles, resetCellphone, clearActiveTasks, clearProperties, resetNeeds, resetGroup, resetLicenses, resetActivites, resetGracePeriod, resetBankAccounts, resetSavedGame, resetPendingMessages, resetInteriors, resetGambling);
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
        if (Player.DiedInVehicle)
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
        int CurrentCash = Player.BankAccounts.GetMoney(true);
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
        Game.DisplayNotification(BankContactPicture, BankContactPicture, HospitalName, "Hospital Fees", string.Format("Todays Bill: ~r~${0}~s~~n~Payment Today: ~g~${1}~s~~n~Outstanding: ~r~${2}~s~ ~n~{3}", HospitalFee, TodaysPayment, HospitalBillPastDue, HospitalStayReport));
        Player.BankAccounts.GiveMoney(-1 * TodaysPayment, true);
    }
    private void SetPlayerAtLocation(ILocationRespawnable ToSet)
    {
        if (ToSet.RespawnLocation == Vector3.Zero)
        {
            Game.LocalPlayer.Character.Position = ToSet.EntrancePosition;
            Game.LocalPlayer.Character.Heading = ToSet.EntranceHeading;
            EntryPoint.FocusCellX = (int)(ToSet.EntrancePosition.X / EntryPoint.CellSize);
            EntryPoint.FocusCellY = (int)(ToSet.EntrancePosition.Y / EntryPoint.CellSize);
            EntryPoint.FocusPosition = ToSet.EntrancePosition;
        }
        else
        {
            Game.LocalPlayer.Character.Position = ToSet.RespawnLocation;
            Game.LocalPlayer.Character.Heading = ToSet.RespawnHeading;
            EntryPoint.FocusCellX = (int)(ToSet.RespawnLocation.X / EntryPoint.CellSize);
            EntryPoint.FocusCellY = (int)(ToSet.RespawnLocation.Y / EntryPoint.CellSize);
            EntryPoint.FocusPosition = ToSet.RespawnLocation;
        }
        World.ClearSpawned(false);
        Player.Dispatcher.LocationDispatcher.Reset();
        World.Places.StaticPlaces.ActivateLocation(ToSet);
        GameTimeLastPlacedAtLocation = Game.GameTime;
        NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Game.LocalPlayer.Character);
    }
    private void GenerateTotalBailFee()
    {
        int CurrentCash = Player.BankAccounts.GetMoney(true);
        int TotalNeededPayment = BailFee + BailFeePastDue;
        TodaysPayment = 0;
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
    }
    private void DisplayBailNotification(string PoliceStationName)
    {
        BailReport = $"~s~Incarcerated Days: ~r~{BailDuration}~s~~n~Released: {BailPostingTime:g}~s~";


        if(HasIllegalWeapons && HasIllegalItems)
        {
            BailReport += $"~n~~r~Illicit Weapons & Items Removed~s~";
        }
        else if (HasIllegalItems)
        {
            BailReport += $"~n~~r~Illicit Items Removed~s~";
        }
        else if (HasIllegalWeapons)
        {
            BailReport += $"~n~~r~Illicit Weapons Removed~s~";
        }

        bool LesterHelp = RandomItems.RandomPercent(Settings.SettingsManager.RespawnSettings.LesterBailHelpPercent);
        if (!LesterHelp)
        {
            Game.DisplayNotification(BankContactPicture, BankContactPicture, PoliceStationName, "Bail Fees", $"Todays Bill: ~r~${BailFee}~s~~n~Payment Today: ~g~${TodaysPayment}~s~~n~Outstanding: ~r~${BailFeePastDue}~s~ ~n~{BailReport}");// string.Format("Todays Bill: ~r~${0}~s~~n~Payment Today: ~g~${1}~s~~n~Outstanding: ~r~${2}~s~ ~n~{3}", BailFee, TodaysPayment, BailFeePastDue, BailReport));
            Player.BankAccounts.GiveMoney(-1 * TodaysPayment, true);
        }
        else
        {
            Game.DisplayNotification(LesterContactPicture, LesterContactPicture, PoliceStationName, "Bail Fees", "~g~$0 ~s~");
        }
    }
    public void OnPlayerBusted()
    {
        TimesTalked = 0;
    } 
    public void ConsentToSearch(ModUIMenu menu)
    {
        VehicleExt vehicleToSearch = GetVehicleToSearch();
        SearchActivity searchActivity = new SearchActivity(Player, World, PoliceRespondable, SeatAssignable, Settings, Time, ModItems, vehicleToSearch, Weapons);
        searchActivity.Setup();
        searchActivity.Start();
        if(!searchActivity.IsActive)
        {
            Game.DisplayHelp("Search Failed");
            if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnBusted)
            {
                Game.TimeScale = Settings.SettingsManager.PlayerOtherSettings.SlowMoOnBustedSpeed;
            }
            menu?.Show();
            return;
        }

        GameFiber.StartNew(delegate
        {
            try
            {
                while (searchActivity.IsActive)
                {
                    GameFiber.Yield();
                }
                CrimeEvent highestPriorityCrimeEvent = Player.PoliceResponse.CrimesObserved.OrderBy(x => x.AssociatedCrime.Priority).FirstOrDefault();
                if (searchActivity.CompletedSearch && !searchActivity.FoundIllegalItems && (highestPriorityCrimeEvent == null || highestPriorityCrimeEvent.AssociatedCrime.CanReleaseOnCleanSearch))
                {
                    ResetPlayer(true, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Player.Scanner.OnTalkedOutOfTicket();
                }
                else
                {
                    if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnBusted)
                    {
                        Game.TimeScale = Settings.SettingsManager.PlayerOtherSettings.SlowMoOnBustedSpeed;
                    }
                    menu?.Show();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Searching");
    }
    private VehicleExt GetVehicleToSearch()
    {
        VehicleExt vehicleToSearch = World.Vehicles.NonPoliceList.Where(x => (x.HasBeenEnteredByPlayer || x.IsOwnedByPlayer) && x.Vehicle.Exists()).OrderBy(x => x.Vehicle.DistanceTo2D(Player.Character)).FirstOrDefault();// CurrentPlayer.VehicleOwnership.OwnedVehicles.Where(x => x.Vehicle.Exists()).OrderBy(x => x.Vehicle.DistanceTo2D(CurrentPlayer.Character)).FirstOrDefault();
        if (vehicleToSearch == null || !vehicleToSearch.Vehicle.Exists() || vehicleToSearch.Vehicle.DistanceTo2D(Player.Character) >= 35f)
        {
            vehicleToSearch = null;
        }

        return vehicleToSearch;
    }
}


