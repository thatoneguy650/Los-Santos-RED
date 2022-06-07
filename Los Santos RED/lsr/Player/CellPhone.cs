using ExtensionsMethods;
using iFruitAddon2;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CellPhone
{
    private ICellPhoneable Player;
    private int ContactIndex = 40;
    private int BurnerContactIndex = 0;
    private int TextIndex = 0;
    private MenuPool MenuPool;
    private IJurisdictions Jurisdictions;
    private List<PhoneContact> AddedContacts = new List<PhoneContact>();
    private List<ContactLookup> ContactLookups = new List<ContactLookup>();
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private IGangs Gangs;
    private List<PhoneText> AddedTexts = new List<PhoneText>();
    private Gang ActiveGang;
    private IPlacesOfInterest PlacesOfInterest;
    private IZones Zones;
    private IStreets Streets;
    private List<ScheduledContact> ScheduledContacts = new List<ScheduledContact>();
    private List<ScheduledText> ScheduledTexts = new List<ScheduledText>();
    private IGangTerritories GangTerritories;
    private int TextSound;
    private List<PhoneResponse> PhoneResponses = new List<PhoneResponse>();
    private GunDealerInteraction GunDealerInteraction;
    private GangInteraction GangInteraction;
    private IContactInteractable ContactInteractable;
    private CorruptCopInteraction CorruptCopInteraction;
    private EmergencyServicesInteraction EmergencyServicesInteraction;
    private bool isRunningForcedMobileTask;
    private BurnerPhone BurnerPhone;
    public bool IsActive => BurnerPhone?.IsActive == true;
   // public CustomiFruit CustomiFruit { get; private set; }
    public List<PhoneText> TextList => AddedTexts;
    public List<PhoneContact> ContactList => AddedContacts;
    public List<PhoneResponse> PhoneResponseList => PhoneResponses;
    public CellPhone(ICellPhoneable player, IContactInteractable gangInteractable, IJurisdictions jurisdictions, ISettingsProvideable settings, ITimeReportable time, IGangs gangs, IPlacesOfInterest placesOfInterest, IZones zones, IStreets streets, IGangTerritories gangTerritories)
    {
        Player = player;
        MenuPool = new MenuPool();
        Jurisdictions = jurisdictions;
        Settings = settings;
        Time = time;
        Gangs = gangs;
        Zones = zones;
        Streets = streets;
        ContactIndex = 0;
        PlacesOfInterest = placesOfInterest;
        GangTerritories = gangTerritories;
        ContactInteractable = gangInteractable;
        BurnerPhone = new BurnerPhone(Player, Time, Settings);
    }
    public void Setup()
    {
        //if (Settings.SettingsManager.CellphoneSettings.OverwriteVanillaEmergencyServicesContact)
        //{
            AddEmergencyServicesCustomContact(false);
       // }
        //ContactLookups = new List<ContactLookup>()
        //{
        //     new ContactLookup(ContactIcon.Generic,"CHAR_DEFAULT"),
        //     new ContactLookup(ContactIcon.Abigail,"CHAR_ABIGAIL"),
        //     new ContactLookup(ContactIcon.AllCharacters,"CHAR_ALL_PLAYERS_CONF"),
        //     new ContactLookup(ContactIcon.Amanda,"CHAR_AMANDA"),
        //     new ContactLookup(ContactIcon.Ammunation,"CHAR_AMMUNATION"),
        //     new ContactLookup(ContactIcon.Andreas,"CHAR_ANDREAS"),
        //     new ContactLookup(ContactIcon.Antonia,"CHAR_ANTONIA"),
        //     new ContactLookup(ContactIcon.Arthur,"CHAR_ARTHUR"),
        //     new ContactLookup(ContactIcon.Ashley,"CHAR_ASHLEY"),
        //     new ContactLookup(ContactIcon.BankOfLiberty,"CHAR_BANK_BOL"),
        //     new ContactLookup(ContactIcon.FleecaBank,"CHAR_BANK_FLEECA"),
        //     new ContactLookup(ContactIcon.MazeBank,"CHAR_BANK_MAZE"),
        //     new ContactLookup(ContactIcon.Barry,"CHAR_BARRY"),
        //     new ContactLookup(ContactIcon.Beverly,"CHAR_BEVERLY"),
        //     new ContactLookup(ContactIcon.Bikesite,"CHAR_BIKESITE"),
        //     new ContactLookup(ContactIcon.Blank,"CHAR_BLANK_ENTRY"),
        //     new ContactLookup(ContactIcon.Blimp,"CHAR_BLIMP"),
        //     new ContactLookup(ContactIcon.Blocked,"CHAR_BLOCKED"),
        //     new ContactLookup(ContactIcon.Boatsite,"CHAR_BOATSITE"),
        //     new ContactLookup(ContactIcon.BrokenDownGirl,"CHAR_BROKEN_DOWN_GIRL"),
        //     new ContactLookup(ContactIcon.Bugstars,"CHAR_BUGSTARS"),
        //     new ContactLookup(ContactIcon.Emergency,"CHAR_CALL911"),
        //     new ContactLookup(ContactIcon.LegendaryMotorsport,"CHAR_CARSITE"),
        //     new ContactLookup(ContactIcon.SSASuperAutos,"CHAR_CARSITE2"),
        //     new ContactLookup(ContactIcon.Castro,"CHAR_CASTRO"),
        //     new ContactLookup(ContactIcon.ChatIcon,"CHAR_CHAT_CALL"),
        //     new ContactLookup(ContactIcon.Chef,"CHAR_CHEF"),
        //     new ContactLookup(ContactIcon.Cheng,"CHAR_CHENG"),
        //     new ContactLookup(ContactIcon.ChengSr,"CHAR_CHENGSR"),
        //     new ContactLookup(ContactIcon.Chop,"CHAR_CHOP"),
        //     new ContactLookup(ContactIcon.CreatorPortraits,"CHAR_CREATOR_PORTRAITS"),
        //     new ContactLookup(ContactIcon.Cris,"CHAR_CRIS"),
        //     new ContactLookup(ContactIcon.Dave,"CHAR_DAVE"),
        //     new ContactLookup(ContactIcon.Denise,"CHAR_DENISE"),
        //     new ContactLookup(ContactIcon.DetonateBomb,"CHAR_DETONATEBOMB"),
        //     new ContactLookup(ContactIcon.DetonatePhone,"CHAR_DETONATEPHONE"),
        //     new ContactLookup(ContactIcon.Devin,"CHAR_DEVIN"),
        //     new ContactLookup(ContactIcon.DialASub,"CHAR_DIAL_A_SUB"),
        //     new ContactLookup(ContactIcon.Dom,"CHAR_DOM"),
        //     new ContactLookup(ContactIcon.DomesticGirl,"CHAR_DOMESTIC_GIRL"),
        //     new ContactLookup(ContactIcon.Dreyfuss,"CHAR_DREYFUSS"),
        //     new ContactLookup(ContactIcon.DrFriedlander,"CHAR_DR_FRIEDLANDER"),
        //     new ContactLookup(ContactIcon.Epsilon,"CHAR_EPSILON"),
        //     new ContactLookup(ContactIcon.EstateAgent,"CHAR_ESTATE_AGENT"),
        //     new ContactLookup(ContactIcon.Facebook,"CHAR_FACEBOOK"),
        //     new ContactLookup(ContactIcon.Filmnoir,"CHAR_FILMNOIR"),
        //     new ContactLookup(ContactIcon.Floyd,"CHAR_FLOYD"),
        //     new ContactLookup(ContactIcon.Franklin,"CHAR_FRANKLIN"),
        //     new ContactLookup(ContactIcon.FranklinTrevor,"CHAR_FRANK_TREV_CONF"),
        //     new ContactLookup(ContactIcon.GayMilitary,"CHAR_GAYMILITARY"),
        //     new ContactLookup(ContactIcon.Hao,"CHAR_HAO"),
        //     new ContactLookup(ContactIcon.HitcherGirl,"CHAR_HITCHER_GIRL"),
        //     new ContactLookup(ContactIcon.Human,"CHAR_HUMANDEFAULT"),
        //     new ContactLookup(ContactIcon.Hunter,"CHAR_HUNTER"),
        //     new ContactLookup(ContactIcon.Jimmy,"CHAR_JIMMY"),
        //     new ContactLookup(ContactIcon.JimmyBoston,"CHAR_JIMMY_BOSTON"),
        //     new ContactLookup(ContactIcon.Joe,"CHAR_JOE"),
        //     new ContactLookup(ContactIcon.Josef,"CHAR_JOSEF"),
        //     new ContactLookup(ContactIcon.Josh,"CHAR_JOSH"),
        //     new ContactLookup(ContactIcon.Lamar,"CHAR_LAMAR"),
        //     new ContactLookup(ContactIcon.Lazlow,"CHAR_LAZLOW"),
        //     new ContactLookup(ContactIcon.Lester,"CHAR_LESTER"),
        //     new ContactLookup(ContactIcon.Skull,"CHAR_LESTER_DEATHWISH"),
        //     new ContactLookup(ContactIcon.LesterFranklin,"CHAR_LEST_FRANK_CONF"),
        //     new ContactLookup(ContactIcon.LesterMichael,"CHAR_LEST_MIKE_CONF"),
        //     new ContactLookup(ContactIcon.Lifeinvader,"CHAR_LIFEINVADER"),
        //     new ContactLookup(ContactIcon.LSCustoms,"CHAR_LS_CUSTOMS"),
        //     new ContactLookup(ContactIcon.LSTouristBoard,"CHAR_LS_TOURIST_BOARD"),
        //     new ContactLookup(ContactIcon.Manuel,"CHAR_MANUEL"),
        //     new ContactLookup(ContactIcon.Marnie,"CHAR_MARNIE"),
        //     new ContactLookup(ContactIcon.Martin,"CHAR_MARTIN"),
        //     new ContactLookup(ContactIcon.MaryAnn,"CHAR_MARY_ANN"),
        //     new ContactLookup(ContactIcon.Maude,"CHAR_MAUDE"),
        //     new ContactLookup(ContactIcon.Mechanic,"CHAR_MECHANIC"),
        //     new ContactLookup(ContactIcon.Michael,"CHAR_MICHAEL"),
        //     new ContactLookup(ContactIcon.MichaelFranklin,"CHAR_MIKE_FRANK_CONF"),
        //     new ContactLookup(ContactIcon.MichaelTrevor,"CHAR_MIKE_TREV_CONF"),
        //     new ContactLookup(ContactIcon.Milsite,"CHAR_MILSITE"),
        //     new ContactLookup(ContactIcon.Minotaur,"CHAR_MINOTAUR"),
        //     new ContactLookup(ContactIcon.Molly,"CHAR_MOLLY"),
        //     new ContactLookup(ContactIcon.MP_ArmyContact,"CHAR_MP_ARMY_CONTACT"),
        //     new ContactLookup(ContactIcon.MP_BikerBoss,"CHAR_MP_BIKER_BOSS"),
        //     new ContactLookup(ContactIcon.MP_BikerMechanic,"CHAR_MP_BIKER_MECHANIC"),
        //     new ContactLookup(ContactIcon.MP_Brucie,"CHAR_MP_BRUCIE"),
        //     new ContactLookup(ContactIcon.MP_Detonatephone,"CHAR_MP_DETONATEPHONE"),
        //     new ContactLookup(ContactIcon.MP_FamBoss,"CHAR_MP_FAM_BOSS"),
        //     new ContactLookup(ContactIcon.MP_FibContact,"CHAR_MP_FIB_CONTACT"),
        //     new ContactLookup(ContactIcon.MP_FmContact,"CHAR_MP_FM_CONTACT"),
        //     new ContactLookup(ContactIcon.MP_Gerald,"CHAR_MP_GERALD"),
        //     new ContactLookup(ContactIcon.MP_Julio,"CHAR_MP_JULIO"),
        //     new ContactLookup(ContactIcon.MP_Mechanic,"CHAR_MP_MECHANIC"),
        //     new ContactLookup(ContactIcon.MP_Merryweather,"CHAR_MP_MERRYWEATHER"),
        //     new ContactLookup(ContactIcon.MP_MexBoss,"CHAR_MP_MEX_BOSS"),
        //     new ContactLookup(ContactIcon.MP_MexDocks,"CHAR_MP_MEX_DOCKS"),
        //     new ContactLookup(ContactIcon.MP_MexLt,"CHAR_MP_MEX_LT"),
        //     new ContactLookup(ContactIcon.MP_MorsMutual,"CHAR_MP_MORS_MUTUAL"),
        //     new ContactLookup(ContactIcon.MP_ProfBoss,"CHAR_MP_PROF_BOSS"),
        //     new ContactLookup(ContactIcon.MP_RayLavoy,"CHAR_MP_RAY_LAVOY"),
        //     new ContactLookup(ContactIcon.MP_Roberto,"CHAR_MP_ROBERTO"),
        //     new ContactLookup(ContactIcon.MP_Snitch,"CHAR_MP_SNITCH"),
        //     new ContactLookup(ContactIcon.MP_Stretch,"CHAR_MP_STRETCH"),
        //     new ContactLookup(ContactIcon.MP_StripclubPr,"CHAR_MP_STRIPCLUB_PR"),
        //     new ContactLookup(ContactIcon.MrsThornhill,"CHAR_MRS_THORNHILL"),
        //     new ContactLookup(ContactIcon.Multiplayer,"CHAR_MULTIPLAYER"),
        //     new ContactLookup(ContactIcon.Nigel,"CHAR_NIGEL"),
        //     new ContactLookup(ContactIcon.Omega,"CHAR_OMEGA"),
        //     new ContactLookup(ContactIcon.Oneil,"CHAR_ONEIL"),
        //     new ContactLookup(ContactIcon.Ortega,"CHAR_ORTEGA"),
        //     new ContactLookup(ContactIcon.Oscar,"CHAR_OSCAR"),
        //     new ContactLookup(ContactIcon.Patricia,"CHAR_PATRICIA"),
        //     new ContactLookup(ContactIcon.Pegasus,"CHAR_PEGASUS_DELIVERY"),
        //     new ContactLookup(ContactIcon.Planesite,"CHAR_PLANESITE"),
        //     new ContactLookup(ContactIcon.Property_ArmsTrafficking,"CHAR_PROPERTY_ARMS_TRAFFICKING"),
        //     new ContactLookup(ContactIcon.Property_BarAirport,"CHAR_PROPERTY_BAR_AIRPORT"),
        //     new ContactLookup(ContactIcon.Property_BarBayview,"CHAR_PROPERTY_BAR_BAYVIEW"),
        //     new ContactLookup(ContactIcon.Property_BarCafeRojo,"CHAR_PROPERTY_BAR_CAFE_ROJO"),
        //     new ContactLookup(ContactIcon.Property_BarCockotoos,"CHAR_PROPERTY_BAR_COCKOTOOS"),
        //     new ContactLookup(ContactIcon.Property_BarEclipse,"CHAR_PROPERTY_BAR_ECLIPSE"),
        //     new ContactLookup(ContactIcon.Property_BarFes,"CHAR_PROPERTY_BAR_FES"),
        //     new ContactLookup(ContactIcon.Property_BarHenHouse,"CHAR_PROPERTY_BAR_HEN_HOUSE"),
        //     new ContactLookup(ContactIcon.Property_BarHiMen,"CHAR_PROPERTY_BAR_HI_MEN"),
        //     new ContactLookup(ContactIcon.Property_BarHookies,"CHAR_PROPERTY_BAR_HOOKIES"),
        //     new ContactLookup(ContactIcon.Property_BarIrish,"CHAR_PROPERTY_BAR_IRISH"),
        //     new ContactLookup(ContactIcon.Property_BarLesBianco,"CHAR_PROPERTY_BAR_LES_BIANCO"),
        //     new ContactLookup(ContactIcon.Property_BarMirrorPark,"CHAR_PROPERTY_BAR_MIRROR_PARK"),
        //     new ContactLookup(ContactIcon.Property_BarPitchers,"CHAR_PROPERTY_BAR_PITCHERS"),
        //     new ContactLookup(ContactIcon.Property_BarSingletons,"CHAR_PROPERTY_BAR_SINGLETONS"),
        //     new ContactLookup(ContactIcon.Property_BarTequilala,"CHAR_PROPERTY_BAR_TEQUILALA"),
        //     new ContactLookup(ContactIcon.Property_BarUnbranded,"CHAR_PROPERTY_BAR_UNBRANDED"),
        //     new ContactLookup(ContactIcon.Property_CarModShop,"CHAR_PROPERTY_CAR_MOD_SHOP"),
        //     new ContactLookup(ContactIcon.Property_CarScrapYard,"CHAR_PROPERTY_CAR_SCRAP_YARD"),
        //     new ContactLookup(ContactIcon.Property_CinemaDowntown,"CHAR_PROPERTY_CINEMA_DOWNTOWN"),
        //     new ContactLookup(ContactIcon.Property_CinemaMorningwood,"CHAR_PROPERTY_CINEMA_MORNINGWOOD"),
        //     new ContactLookup(ContactIcon.Property_CinemaVinewood,"CHAR_PROPERTY_CINEMA_VINEWOOD"),
        //     new ContactLookup(ContactIcon.Property_GolfClub,"CHAR_PROPERTY_GOLF_CLUB"),
        //     new ContactLookup(ContactIcon.Property_PlaneScrapYard,"CHAR_PROPERTY_PLANE_SCRAP_YARD"),
        //     new ContactLookup(ContactIcon.Property_SonarCollections,"CHAR_PROPERTY_SONAR_COLLECTIONS"),
        //     new ContactLookup(ContactIcon.Property_TaxiLot,"CHAR_PROPERTY_TAXI_LOT"),
        //     new ContactLookup(ContactIcon.Property_TowingImpound,"CHAR_PROPERTY_TOWING_IMPOUND"),
        //     new ContactLookup(ContactIcon.Property_WeedShop,"CHAR_PROPERTY_WEED_SHOP"),
        //     new ContactLookup(ContactIcon.Ron,"CHAR_RON"),
        //     new ContactLookup(ContactIcon.Saeeda,"CHAR_SAEEDA"),
        //     new ContactLookup(ContactIcon.Sasquatch,"CHAR_SASQUATCH"),
        //     new ContactLookup(ContactIcon.Simeon,"CHAR_SIMEON"),
        //     new ContactLookup(ContactIcon.SocialClub,"CHAR_SOCIAL_CLUB"),
        //     new ContactLookup(ContactIcon.Solomon,"CHAR_SOLOMON"),
        //     new ContactLookup(ContactIcon.Steve,"CHAR_STEVE"),
        //     new ContactLookup(ContactIcon.SteveMichael,"CHAR_STEVE_MIKE_CONF"),
        //     new ContactLookup(ContactIcon.SteveTrevor,"CHAR_STEVE_TREV_CONF"),
        //     new ContactLookup(ContactIcon.Stretch,"CHAR_STRETCH"),
        //     new ContactLookup(ContactIcon.StripperChastity,"CHAR_STRIPPER_CHASTITY"),
        //     new ContactLookup(ContactIcon.StripperCheetah,"CHAR_STRIPPER_CHEETAH"),
        //     new ContactLookup(ContactIcon.StripperFufu,"CHAR_STRIPPER_FUFU"),
        //     new ContactLookup(ContactIcon.StripperInfernus,"CHAR_STRIPPER_INFERNUS"),
        //     new ContactLookup(ContactIcon.StripperJuliet,"CHAR_STRIPPER_JULIET"),
        //     new ContactLookup(ContactIcon.StripperNikki,"CHAR_STRIPPER_NIKKI"),
        //     new ContactLookup(ContactIcon.StripperPeach,"CHAR_STRIPPER_PEACH"),
        //     new ContactLookup(ContactIcon.StripperSapphire,"CHAR_STRIPPER_SAPPHIRE"),
        //     new ContactLookup(ContactIcon.Tanisha,"CHAR_TANISHA"),
        //     new ContactLookup(ContactIcon.Taxi,"CHAR_TAXI"),
        //     new ContactLookup(ContactIcon.TaxiLiz,"CHAR_TAXI_LIZ"),
        //     new ContactLookup(ContactIcon.TennisCoach,"CHAR_TENNIS_COACH"),
        //     new ContactLookup(ContactIcon.Tonya,"CHAR_TOW_TONYA"),
        //     new ContactLookup(ContactIcon.Tracey,"CHAR_TRACEY"),
        //     new ContactLookup(ContactIcon.Trevor,"CHAR_TREVOR"),
        //     new ContactLookup(ContactIcon.Wade,"CHAR_WADE"),
        //     new ContactLookup(ContactIcon.Youtube,"CHAR_YOUTUBE"),
        //};

        BurnerPhone.Setup();
    }
    public void ContactAnswered(PhoneContact contact)
    {
        //if(!NativeFunction.Natives.IS_PED_RUNNING_MOBILE_PHONE_TASK<bool>(Player.Character))
        //{
        if (!BurnerPhone.IsActive)
        {
            NativeFunction.Natives.CREATE_MOBILE_PHONE(Settings.SettingsManager.CellphoneSettings.BurnerCellPhoneTypeID);
            isRunningForcedMobileTask = true;
            NativeFunction.Natives.TASK_USE_MOBILE_PHONE(Game.LocalPlayer.Character, true);
            Player.Character.KeepTasks = true;
            if (Settings.SettingsManager.CellphoneSettings.AllowBurnerPhone)
            {
                BurnerPhone.DisplayCallUI(contact.Name, "CELL_219", contact.IconName.ToUpper());
            }
            else
            {
                BurnerPhone.SetOffScreen();
            }
        }
        else
        {
            isRunningForcedMobileTask = false;
        }
        //}
        //else
        //{
        //    isRunningForcedMobileTask = false;
        //}
        Gang myGang = Gangs.GetAllGangs().FirstOrDefault(x => x.ContactName == contact.Name);
        if (myGang != null)
        {
            ActiveGang = myGang;
            GangInteraction = new GangInteraction(ContactInteractable, Gangs, PlacesOfInterest);
            GangInteraction.Start(myGang);
        }
        else if (contact.Name == EntryPoint.OfficerFriendlyContactName)
        {
            CorruptCopInteraction = new CorruptCopInteraction(ContactInteractable, Gangs, PlacesOfInterest, Settings);
            CorruptCopInteraction.Start(contact);
        }
        else if (contact.Name == EntryPoint.UndergroundGunsContactName)
        {
            GunDealerInteraction = new GunDealerInteraction(ContactInteractable, Gangs, PlacesOfInterest, Settings);
            GunDealerInteraction.Start(contact);
        }
        else if (contact.Name == EntryPoint.EmergencyServicesContactName)
        {
            EmergencyServicesInteraction = new EmergencyServicesInteraction(ContactInteractable, Gangs, PlacesOfInterest, Jurisdictions);
            EmergencyServicesInteraction.Start(contact);
        }
        else
        {
            Close(0);
        }

    }
    public void DeleteText(PhoneText text)
    {
        AddedTexts.Remove(text);
    }
    public void DeletePhoneRespone(PhoneResponse phoneResponse)
    {
        PhoneResponses.Remove(phoneResponse);
    }
    public void Update()
    {
        CheckScheduledItems();
        MenuPool.ProcessMenus();
        GangInteraction?.Update();
        GunDealerInteraction?.Update();
        CorruptCopInteraction?.Update();
        EmergencyServicesInteraction?.Update();
        if (Settings.SettingsManager.CellphoneSettings.AllowBurnerPhone)
        {
            BurnerPhone.Update();
        }
    }
    public void Reset()
    {
        ContactIndex = 0;
        AddedTexts = new List<PhoneText>();
        AddedContacts = new List<PhoneContact>();
        PhoneResponses = new List<PhoneResponse>();
        ScheduledContacts = new List<ScheduledContact>();
        ScheduledTexts = new List<ScheduledText>();
        AddEmergencyServicesCustomContact(false);
    }
    public void ClearTextMessages()
    {
        AddedTexts = new List<PhoneText>();
    }
    public void ClearContacts()
    {
        ContactIndex = 0;
        AddedContacts = new List<PhoneContact>();
    }
    public void ClearPhoneResponses()
    {
        PhoneResponses = new List<PhoneResponse>();
    }
    public void Dispose()
    {
        if (Settings.SettingsManager.CellphoneSettings.AllowBurnerPhone)
        {
            BurnerPhone.ClosePhone();
        }


        if(!Settings.SettingsManager.CellphoneSettings.TerminateVanillaCellphone)
        {
            Tools.Scripts.StartScript("cellphone_flashhand", 1424);
            Tools.Scripts.StartScript("cellphone_controller", 1424);
        }
    }
    public void Close(int time)
    {
        EntryPoint.WriteToConsole("Mobile Phone Closed");
        if (isRunningForcedMobileTask)
        {
            NativeFunction.Natives.DESTROY_MOBILE_PHONE();
            //NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        }
        isRunningForcedMobileTask = false;
        //CustomiFruit.Close(time);

        if (Settings.SettingsManager.CellphoneSettings.AllowBurnerPhone)
        {
            BurnerPhone.ClosePhone();
        }
        else if (isRunningForcedMobileTask)
        {
            NativeFunction.Natives.DESTROY_MOBILE_PHONE();
        }

    }
    public void OpenBurner()
    {
        if (Settings.SettingsManager.CellphoneSettings.AllowBurnerPhone)
        {
            BurnerPhone.OpenPhone();
        }
    }
    public void CloseBurner()
    {
        if (Settings.SettingsManager.CellphoneSettings.AllowBurnerPhone)
        {
            BurnerPhone.ClosePhone();
        }
    }
    private void CheckScheduledItems()
    {
        CheckScheduledTexts();
        CheckScheduledContacts();
    }
    private void CheckScheduledTexts()
    {
        for (int i = ScheduledTexts.Count - 1; i >= 0; i--)
        {
            ScheduledText sc = ScheduledTexts[i];
            if (DateTime.Compare(Time.CurrentDateTime, sc.TimeToSend) >= 0)
            {
                if (!AddedTexts.Any(x => x.Name == sc.ContactName && x.Message == sc.Message))
                {
                    AddText(sc.ContactName, sc.IconName, sc.Message, Time.CurrentHour, Time.CurrentMinute, false);
                    NativeHelper.DisplayNotificationCustom(sc.IconName, sc.IconName, sc.ContactName, "~g~Text Received~s~", sc.Message, NotificationIconTypes.ChatBox, false);
                    TextSound = NativeFunction.Natives.GET_SOUND_ID<int>();
                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(TextSound, "Phone_Generic_Key_01", "HUD_MINIGAME_SOUNDSET", 0);


                    if (!AddedContacts.Any(x => x.Name == sc.ContactName))
                    {
                        Gang relatedGang = Gangs.GetGangByContact(sc.ContactName);
                        if (relatedGang != null)
                        {
                            AddContact(relatedGang, true);
                        }
                        else if (sc.ContactName == EntryPoint.OfficerFriendlyContactName)
                        {
                            AddCopContact(sc.ContactName, sc.IconName, true);
                        }
                        else
                        {
                            AddContact(sc.ContactName, sc.IconName, true);
                        }

                    }
                }
                ScheduledTexts.RemoveAt(i);
            }
        }
    }
    private void CheckScheduledContacts()
    {
        for (int i = ScheduledContacts.Count - 1; i >= 0; i--)
        {
            ScheduledContact sc = ScheduledContacts[i];
            if (DateTime.Compare(Time.CurrentDateTime, sc.TimeToSend) >= 0)
            {
                if (!AddedContacts.Any(x => x.Name == sc.ContactName))
                {
                    Gang relatedGang = Gangs.GetGangByContact(sc.ContactName);
                    if (relatedGang != null)
                    {
                        AddContact(relatedGang, true);
                    }
                    else if (sc.ContactName == EntryPoint.OfficerFriendlyContactName)
                    {
                        AddCopContact(sc.ContactName,sc.IconName, true);
                    }
                    else if (sc.ContactName == EntryPoint.UndergroundGunsContactName)
                    {
                        AddGunDealerContact(true);
                    }
                    else
                    {
                        AddContact(sc.ContactName, sc.IconName, true);
                    }
                    TextSound = NativeFunction.Natives.GET_SOUND_ID<int>();
                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(TextSound, "Phone_Generic_Key_01", "HUD_MINIGAME_SOUNDSET", 0);
                    if (sc.Message == "")
                    {
                        NativeHelper.DisplayNotificationCustom(sc.IconName, sc.IconName, "New Contact", sc.ContactName, NotificationIconTypes.AddFriendRequest, true);
                    }
                    else
                    {
                        NativeHelper.DisplayNotificationCustom(sc.IconName, sc.IconName, "New Contact", sc.ContactName, sc.Message, NotificationIconTypes.AddFriendRequest, false);
                    }
                }
                ScheduledContacts.RemoveAt(i);
            }
        }
    }
    public void AddScheduledContact(string Name, string IconName, string MessageToSend, DateTime timeToAdd)
    {
        if(!AddedContacts.Any(x=> x.Name == Name))
        {
            ScheduledContacts.Add(new ScheduledContact(timeToAdd, Name, MessageToSend, IconName));
        }
    }
    public void AddContact(string Name, string IconName, bool displayNotification)
    {
        if (!AddedContacts.Any(x => x.Name == Name))
        {
            PhoneContact contactA = new PhoneContact(Name, ContactIndex);
            //contactA.Answered += ContactAnswered;
            contactA.Active = false;
            contactA.DialTimeout = 4000;
            contactA.RandomizeDialTimeout = true;
            //contactA.Icon = GetIconFromString(IconName);
            contactA.IconName = IconName;
            //CustomiFruit.Contacts.Add(contactA);
            ContactIndex++;
            AddedContacts.Add(contactA);

            if (displayNotification)
            {
                NativeHelper.DisplayNotificationCustom(IconName, IconName, "New Contact", Name, NotificationIconTypes.AddFriendRequest, true);
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(TextSound, "Phone_Generic_Key_01", "HUD_MINIGAME_SOUNDSET", 0);
            }
        }
    }
    public void AddContact(Gang gang, bool displayNotification)
    {
        if (!AddedContacts.Any(x => x.Name == gang.ContactName))
        {
            PhoneContact contactA = new PhoneContact(gang.ContactName, ContactIndex);
            //contactA.Answered += ContactAnswered;
            contactA.Active = true;
            contactA.DialTimeout = 4000;
            contactA.RandomizeDialTimeout = true;
            //contactA.Icon = GetIconFromString(gang.ContactIcon);
            contactA.IconName = gang.ContactIcon;
            //CustomiFruit.Contacts.Add(contactA);
            ContactIndex++;
            AddedContacts.Add(contactA);

            if (displayNotification)
            {
                NativeHelper.DisplayNotificationCustom(gang.ContactIcon, gang.ContactIcon, "New Contact", gang.ContactName, NotificationIconTypes.AddFriendRequest, true);
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(TextSound, "Phone_Generic_Key_01", "HUD_MINIGAME_SOUNDSET", 0);
            }
        }
    }
    public void AddCopContact(string Name, string IconName, bool displayNotification)
    {
        if (!AddedContacts.Any(x => x.Name == Name))
        {
            PhoneContact contactA = new PhoneContact(Name, ContactIndex);
            //contactA.Answered += ContactAnswered;
            contactA.Active = true;
            contactA.DialTimeout = 4000;
            contactA.RandomizeDialTimeout = true;
            //contactA.Icon = GetIconFromString(IconName);
            contactA.IconName = IconName;
            //CustomiFruit.Contacts.Add(contactA);
            ContactIndex++;
            AddedContacts.Add(contactA);

            if (displayNotification)
            {
                NativeHelper.DisplayNotificationCustom(IconName, IconName, "New Contact", Name, NotificationIconTypes.AddFriendRequest, true);
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(TextSound, "Phone_Generic_Key_01", "HUD_MINIGAME_SOUNDSET", 0);
            }
        }
    }
    public void AddGunDealerContact(bool displayNotification)
    {
        string Name = EntryPoint.UndergroundGunsContactName;
        string IconName = "CHAR_BLANK_ENTRY";
        if (!AddedContacts.Any(x => x.Name == Name))
        {
            PhoneContact contactA = new PhoneContact(Name, ContactIndex);
           // contactA.Answered += ContactAnswered;
            contactA.Active = true;
            contactA.DialTimeout = 4000;
            contactA.RandomizeDialTimeout = true;
            //contactA.Icon = GetIconFromString(IconName);
            contactA.IconName = IconName;
            //CustomiFruit.Contacts.Add(contactA);
            ContactIndex++;
            AddedContacts.Add(contactA);

            if (displayNotification)
            {
                NativeHelper.DisplayNotificationCustom(IconName, IconName, "New Contact", Name, NotificationIconTypes.AddFriendRequest, true);
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(TextSound, "Phone_Generic_Key_01", "HUD_MINIGAME_SOUNDSET", 0);
            }
        }
    }
    public void AddEmergencyServicesCustomContact(bool displayNotification)
    {
        string Name = EntryPoint.UndergroundGunsContactName;
        string IconName = "CHAR_CALL911";
        if (!AddedContacts.Any(x => x.Name == EntryPoint.EmergencyServicesContactName))
        {
            PhoneContact contactA = new PhoneContact(EntryPoint.EmergencyServicesContactName, ContactIndex);
            //contactA.Answered += ContactAnswered;
            contactA.DialTimeout = 3000;
            contactA.Active = true;
            contactA.IconName = IconName;
            //contactA.Icon = ContactIcon.Emergency;
            //CustomiFruit.Contacts.Add(contactA);

            ContactIndex++;
            AddedContacts.Add(contactA);
            if (displayNotification)
            {
                NativeHelper.DisplayNotificationCustom(IconName, IconName, "New Contact", Name, NotificationIconTypes.AddFriendRequest, true);
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(TextSound, "Phone_Generic_Key_01", "HUD_MINIGAME_SOUNDSET", 0);
            }
        }
    }
    public void AddGangText(Gang gang, bool isPositive)
    {
        if (gang != null)
        {
            List<string> Replies = new List<string>();
            if (isPositive)
            {
                Replies.AddRange(new List<string>() {
                    $"Heard some good things about you, come see us sometime.",
                    $"Call us soon to discuss business.",
                    $"Might have some business opportunites for you soon, give us a call.",
                    $"You've been making some impressive moves, call us to discuss.",
                    $"Give us a call soon.",
                    $"We may have some opportunites for you.",
                    $"My guys tell me you are legit, hit us up sometime.",
                    $"Looking for people I can trust, if so give us a call.",
                    $"Word has gotten around about you, mostly positive, give us a call soon.",
                    $"Always looking for help with some 'items'. Call us if you think you can handle it.",
                });
            }
            else
            {
                Replies.AddRange(new List<string>() {
                    $"Watch your back",
                    $"Dead man walking",
                    $"ur fucking dead",
                    $"You just fucked with the wrong people asshole",
                    $"We're gonna fuck you up buddy",
                    $"My boys are gonna skin you alive prick.",
                    $"You will die slowly.",
                    $"I'll take pleasure in guttin you boy.",
                    $"Better leave LS while you can...",
                    $"We'll be waiting for you asshole.",
                    $"You're gonna wish you were dead motherfucker.",
                    $"Got some 'associates' out looking for you prick. Where you at?",


                    $"We'll be seeing you soon",
                    $"{Player.PlayerName}? Better watch out.",
                    $"You'll never hear us coming",
                    $"You are a dead man",
                    $"You're gonna find out what happens when you fuck with us asshole.",
                    $"When my boys find you...",
                });
            }

            List<ZoneJurisdiction> myGangTerritories = GangTerritories.GetGangTerritory(gang.ID);
            ZoneJurisdiction mainTerritory = myGangTerritories.OrderBy(x => x.Priority).FirstOrDefault();

            if (mainTerritory != null)
            {
                Zone mainGangZone = Zones.GetZone(mainTerritory.ZoneInternalGameName);
                if (mainGangZone != null)
                {
                    if (isPositive)
                    {
                        Replies.AddRange(new List<string>() {
                            $"Heard some good things about you, come see us sometime in ~p~{mainGangZone.DisplayName}~s~ to discuss some business",
                            $"Call us soon to discuss business in ~p~{mainGangZone.DisplayName}~s~.",
                            $"Might have some business opportunites for you soon in ~p~{mainGangZone.DisplayName}~s~, give us a call.",
                            $"You've been making some impressive moves, call us to discuss.",
                        });
                    }
                    else
                    {
                        Replies.AddRange(new List<string>() {
                            $"Watch your back next time you are in ~p~{mainGangZone.DisplayName}~s~ motherfucker",
                            $"You are dead next time we see you in ~p~{mainGangZone.DisplayName}~s~",
                            $"Better stay out of ~p~{mainGangZone.DisplayName}~s~ cocksucker",
                        });
                    }
                }
            }
            string MessageToSend;
            MessageToSend = Replies.PickRandom();
            AddScheduledText(gang.ContactName,gang.ContactIcon, MessageToSend);
        }
    }
    public void AddScheduledText(string Name, string IconName, string MessageToSend, int minutesToWait)
    {
        AddScheduledText(Name, IconName, MessageToSend, Time.CurrentDateTime.AddMinutes(minutesToWait));
    }
    public void AddScheduledText(string Name, string IconName, string MessageToSend)
    {
        AddScheduledText(Name, IconName, MessageToSend, Time.CurrentDateTime.AddMinutes(3));
    }
    public void AddScheduledText(string Name, string IconName, string MessageToSend, DateTime timeToAdd)
    {
        if (!AddedTexts.Any(x => x.Name == Name && x.Message == MessageToSend))
        {
            ScheduledTexts.Add(new ScheduledText(timeToAdd, Name, MessageToSend, IconName));
        }
    }
    public void AddText(string Name, string IconName, string message, int hourSent, int minuteSent, bool isRead)
    {
        if (!AddedTexts.Any(x => x.Name == Name && x.Message == message && x.HourSent == hourSent && x.MinuteSent == minuteSent))
        {
            PhoneText textA = new PhoneText(Name, TextIndex, message, hourSent, minuteSent);
            //textA.Icon = GetIconFromString(IconName);      // Contact's icon
            textA.IconName = IconName;
            textA.IsRead = isRead;
            textA.TimeReceived = Time.CurrentDateTime;
            //CustomiFruit.Texts.Add(textA);         // Add the contact to the phone
            TextIndex++;
            AddedTexts.Add(textA);

            int NewTextIndex = 0;
            foreach(PhoneText ifta in TextList.OrderByDescending(x=> x.TimeReceived))
            {
                ifta.Index = NewTextIndex;
                NewTextIndex++;
            }
        }
    }
    public void AddPhoneResponse(string Name, string IconName, string Message)
    {
        PhoneResponses.Add(new PhoneResponse(Name, IconName, Message,Time.CurrentDateTime));
        Game.DisplayNotification(IconName, IconName, Name, "~o~Response", Message);
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(TextSound, "Phone_Generic_Key_02", "HUD_MINIGAME_SOUNDSET", 0);
    }
    public void AddPhoneResponse(string Name, string Message)
    {
        string IconName = ContactList.FirstOrDefault(x => x.Name.ToLower() == Name.ToLower())?.IconName;
        PhoneResponses.Add(new PhoneResponse(Name, IconName, Message, Time.CurrentDateTime));
        Game.DisplayNotification(IconName, IconName, Name, "~o~Response", Message);
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(TextSound, "Phone_Generic_Key_02", "HUD_MINIGAME_SOUNDSET", 0);
    }
    public void DisableContact(string Name)
    {
        PhoneContact myContact = AddedContacts.FirstOrDefault(x => x.Name == Name);
        if(myContact != null)
        {
            myContact.Active = false;
        }
    }
    public bool IsContactEnabled(string contactName)
    {
        PhoneContact myContact = AddedContacts.FirstOrDefault(x => x.Name == contactName);
        if (myContact != null)
        {
            return myContact.Active;
        }
        return false;
    }
    private class ContactLookup
    {
        public ContactLookup(ContactIcon contactIcon, string iconText)
        {
            ContactIcon = contactIcon;
            IconText = iconText;
        }

        public ContactIcon ContactIcon { get; set; }
        public string IconText { get; set; }
    }
    private class ScheduledContact
    {
        public ScheduledContact(DateTime timeToSend, string contactName, string message, string iconName)
        {
            TimeToSend = timeToSend;
            ContactName = contactName;
            Message = message;
            IconName = iconName;
        }

        public DateTime TimeToSend { get; set; }
        public string ContactName { get; set; }
        public string Message { get; set; } = "We need to talk";
        public string IconName { get; set; } = "CHAR_DEFAULT";
    }
    private class ScheduledCall
    {
        public DateTime TimeToSend { get; set; }
        public iFruitContact ContactToCall { get; set; }
    }
    private class ScheduledText
    {
        public ScheduledText(DateTime timeToSend, string contactName, string message, string iconName)
        {
            TimeToSend = timeToSend;
            ContactName = contactName;
            Message = message;
            IconName = iconName;
        }
        public DateTime TimeToSend { get; set; }
        public string ContactName { get; set; }
        public string Message { get; set; } = "We need to talk";
        public string IconName { get; set; } = "CHAR_DEFAULT";
    }

}