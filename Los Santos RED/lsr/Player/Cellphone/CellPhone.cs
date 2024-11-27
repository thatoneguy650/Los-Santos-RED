using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static DispatchScannerFiles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

public class CellPhone
{
    private bool IsDisposed = false;
    private ICellPhoneable Player;
    private int ContactIndex = 40;
    private int BurnerContactIndex = 0;
    private int TextIndex = 0;
    private MenuPool MenuPool;
    private IJurisdictions Jurisdictions;
    private List<PhoneContact> AddedContacts = new List<PhoneContact>();


    private IAgencies Agencies;
    private IContacts Contacts;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private IGangs Gangs;
    private List<PhoneText> AddedTexts = new List<PhoneText>();
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

    private IEntityProvideable World;
    private ICrimes Crimes;
    private IModItems ModItems;
    private uint GameTimeLastCheckedScheduledItems;
    private uint GameTimeBetweenCheckScheduledItems = 1000;
    private NAudioPlayer phoneAudioPlayer;
    private IWeapons Weapons;
    private INameProvideable Names;
    private IShopMenus ShopMenus;


    private ICellphones Cellphones;
    public CellphoneData CurrentCellphoneData { get; private set; }

    public BurnerPhone BurnerPhone { get; private set; }

    public string RingTone => !string.IsNullOrEmpty(CustomRingtone) ? CustomRingtone : Settings.SettingsManager.CellphoneSettings.DefaultCustomRingtoneNameNew;
    public string TextTone => !string.IsNullOrEmpty(CustomTextTone) ? CustomTextTone : Settings.SettingsManager.CellphoneSettings.DefaultCustomTexttoneNameNew;
    public int Theme => CustomTheme != -1 ? CustomTheme : Settings.SettingsManager.CellphoneSettings.DefaultBurnerCellThemeID;
    public int Background => CustomBackground != -1 ? CustomBackground : Settings.SettingsManager.CellphoneSettings.DefaultBurnerCellBackgroundID;
    public float Volume => CustomVolume != -1.0f ? CustomVolume : Settings.SettingsManager.CellphoneSettings.DefaultCustomToneVolume;
    public bool SleepMode { get; set; } = false;
    public int PhoneType => CustomPhoneType != -1 ? CustomPhoneType : CurrentCellphoneData != null ? CurrentCellphoneData.PhoneType : Settings.SettingsManager.CellphoneSettings.BurnerCellPhoneTypeID;
    public string PhoneOS => CustomPhoneOS != "" ? CustomPhoneOS : CurrentCellphoneData != null ? CurrentCellphoneData.PhoneOS : Settings.SettingsManager.CellphoneSettings.BurnerCellScaleformName;
    public string CustomRingtone { get; set; } = "";
    public string CustomTextTone { get; set; } = "";
    public int CustomTheme { get; set; } = -1;
    public int CustomBackground { get; set; } = -1;
    public float CustomVolume { get; set; } = -1.0f;
    public int CustomPhoneType { get; set; } = -1;
    public string CustomPhoneOS { get; set; } = "";
    private bool ShouldCheckScheduledItems => GameTimeLastCheckedScheduledItems == 0 || Game.GameTime - GameTimeLastCheckedScheduledItems >= GameTimeBetweenCheckScheduledItems;
    public bool IsActive => BurnerPhone?.IsActive == true;
    public List<PhoneText> TextList => AddedTexts;
    public List<PhoneContact> ContactList => AddedContacts;
    public List<PhoneResponse> PhoneResponseList => PhoneResponses;
    public CellPhone(ICellPhoneable player, IContactInteractable gangInteractable, IJurisdictions jurisdictions, ISettingsProvideable settings, ITimeReportable time, IGangs gangs, IPlacesOfInterest placesOfInterest, IZones zones, IStreets streets,
        IGangTerritories gangTerritories, ICrimes crimes, IEntityProvideable world, IModItems modItems, IWeapons weapons, INameProvideable names, IShopMenus shopMenus, ICellphones cellphones, IContacts contacts, IAgencies agencies)
    {
        Player = player;
        MenuPool = new MenuPool();
        Jurisdictions = jurisdictions;
        Settings = settings;
        Time = time;
        Gangs = gangs;
        Zones = zones;
        Streets = streets;
        ModItems = modItems;
        ContactIndex = 0;
        PlacesOfInterest = placesOfInterest;
        GangTerritories = gangTerritories;
        ContactInteractable = gangInteractable;
        Crimes = crimes;
        World = world;
        Weapons = weapons;
        Names = names;
        ShopMenus = shopMenus;
        Contacts = contacts;
        Agencies = agencies;
        BurnerPhone = new BurnerPhone(Player, Time, Settings, modItems, Contacts);
        phoneAudioPlayer = new NAudioPlayer(Settings);
        Cellphones = cellphones;
        CurrentCellphoneData = Cellphones.GetDefault();
    }
    public void Setup()
    {
        IsDisposed = false;
        //AddContact(new EmergencyServicesContact(StaticStrings.EmergencyServicesContactName, "CHAR_CALL911"), false);
        foreach (PhoneContact phoneContact in Contacts.GetDefaultContacts())
        {
            AddContact(phoneContact, false);
        }
        BurnerPhone.Setup();
        phoneAudioPlayer.Setup();
    }
    public void ContactAnswered(PhoneContact contact)
    {
        if (!BurnerPhone.IsActive)
        {

        }
        else
        {
            isRunningForcedMobileTask = false;
        }
        contact.OnAnswered(ContactInteractable, this, Gangs, PlacesOfInterest, Settings, Jurisdictions, Crimes, World, ModItems, Weapons, Names, ShopMenus, Agencies);
    }
    public void DeleteText(PhoneText text)
    {
        AddedTexts.Remove(text);
        ReIndexTexts();
    }
    public void DeletePhoneRespone(PhoneResponse phoneResponse)
    {
        PhoneResponses.Remove(phoneResponse);
    }
    private void Update()
    {
        CheckScheduledItems();
        MenuPool.ProcessMenus();
        foreach (PhoneContact phoneContact in ContactList)
        {
            phoneContact.MenuInteraction?.Update();
        }
    }
    public void Reset()
    {
        CustomRingtone = "";
        CustomTextTone = "";
        CustomTheme = -1;
        CustomBackground = -1;
        CustomVolume = -1.0f;
        ContactIndex = 0;
        CurrentCellphoneData = Cellphones.GetDefault();
        AddedTexts = new List<PhoneText>();
        AddedContacts = new List<PhoneContact>();
        PhoneResponses = new List<PhoneResponse>();
        ScheduledContacts = new List<ScheduledContact>();
        ScheduledTexts = new List<ScheduledText>();
        foreach (PhoneContact phoneContact in Contacts.GetDefaultContacts())
        {
            AddContact(phoneContact, false);
        }
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
        NativeHelper.StartScript("cellphone_flashhand", 1424);
        NativeHelper.StartScript("cellphone_controller", 1424);
        IsDisposed = true;
    }
    public void Close(int time)
    {
        if (isRunningForcedMobileTask)
        {
            NativeFunction.Natives.DESTROY_MOBILE_PHONE();
        }
        isRunningForcedMobileTask = false;
        if (Settings.SettingsManager.CellphoneSettings.AllowBurnerPhone && IsActive)
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
    public void AddScamText()
    {
        List<string> ScammerNames = new List<string>() {
        "American Freedom Institute",
        "Lifestyle Unlimited",
        "NAMB Products",
        "Turner Investments",
        "L.F. Fields",
        "Jambog Ltd.",


        };
        List<string> ScammerMessages = new List<string>() {
        "Beautiful weekend coming up. Wanna go out? Sophie gave me your number. Check out my profile here: virus.link/safe",
        "Your IBS tax refund is pending acceptance. Must accept within 24 hours http://asdas.sdf.sdf//asdasd",
        "We've been trying to reach you concerning your vehicle's ~r~extended warranty~s~. You should've received a notice in the mail about your car's extended warranty eligibility.",
        "Verify your Lifeinvader ID. Use Code: DVWB@55",
        "You've won a prize! Go to securelink.safe.biz.ug to claim your ~r~$500~s~ gift card.",
        "Dear customer, Fleeca Bank is closing your bank accounts. Please confirm your pin at fleecascam.ytmd/theft to keep your account activated",
        "URGENT! Your grandson was arrested last night in New Armadillo. Need bail money immediately! Wire Eastern Confederacy at econfed.utg/legit",

        };
        Player.CellPhone.AddScheduledText(new PhoneContact(ScammerNames.PickRandom(), "CHAR_BLANK_ENTRY"), ScammerMessages.PickRandom(), 0, true);
        //CheckScheduledTexts();     

    }
    public void RandomizeSettings()
    {
        var dir = new DirectoryInfo("Plugins\\LosSantosRED\\audio\\tones");
        List<FileInfo> files = dir.GetFiles().ToList();
        if (files != null)
        {
            CustomRingtone = files.PickRandom()?.Name;
            CustomTextTone = files.PickRandom()?.Name;
        }
        else
        {
            CustomRingtone = "";
            CustomTextTone = "";
        }
        CurrentCellphoneData = Cellphones.GetRandomRegular();
        if (CurrentCellphoneData != null)
        {
            CustomTheme = CurrentCellphoneData.GetRandomTheme();
            CustomBackground = CurrentCellphoneData.GetRandomBackground();
            CustomPhoneType = CurrentCellphoneData.PhoneType;// - 1;
            CustomPhoneOS = CurrentCellphoneData.PhoneOS;// "";
        }
        else
        {
            CustomTheme = RandomItems.GetRandomNumberInt(1, 8);
            CustomBackground = new List<int>() { 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }.PickRandom();
            CustomPhoneType = RandomItems.GetRandomNumberInt(0, 3);
            CustomPhoneOS = CustomPhoneType == 0 ? "cellphone_ifruit" : CustomPhoneType == 1 ? "cellphone_facade" : CustomPhoneType == 2 ? "cellphone_badger" : "cellphone_ifruit";// new List<string>() { "cellphone_ifruit", "cellphone_facade", "cellphone_badger" }.PickRandom();
        }
    }
    private void CheckScheduledItems()
    {
        if (ShouldCheckScheduledItems)
        {
            bool hasDisplayed = CheckScheduledTexts();
            if (!hasDisplayed)
            {
                CheckScheduledContacts();
            }
            GameTimeLastCheckedScheduledItems = Game.GameTime;
        }
    }
    private bool CheckScheduledTexts()
    {
        foreach(ScheduledText scheduledText in ScheduledTexts)
        {
            EntryPoint.WriteToConsole($"I HAVE SCHEDULES TEXTS FROM {scheduledText.ContactName} {scheduledText.Message}");
        }
        for (int i = ScheduledTexts.Count - 1; i >= 0; i--)
        {
            
            ScheduledText sc = ScheduledTexts[i];

            EntryPoint.WriteToConsole($"{sc.ContactName} Text Stuff: {DateTime.Compare(Time.CurrentDateTime, sc.TimeToSend)} Currently:{Time.CurrentDateTime} TimeToSend:{sc.TimeToSend} GameTimeDiff{Game.GameTime - sc.GameTimeSent}");

            if (DateTime.Compare(Time.CurrentDateTime, sc.TimeToSend) >= 0 && (sc.SendImmediately || Game.GameTime - sc.GameTimeSent >= 5000))
            {
                if (!AddedTexts.Any(x => x.ContactName == sc.ContactName && x.Message == sc.Message && sc.TimeToSend.Hour == x.HourSent && sc.TimeToSend.Minute == x.MinuteSent))
                {
                    AddText(sc.ContactName, sc.IconName, sc.Message, Time.CurrentHour, Time.CurrentMinute, false, sc.CustomPicture);

                    if (!string.IsNullOrEmpty(sc.CustomPicture))
                    {
                        EntryPoint.WriteToConsole($"CUSTOM PICTURE SENT {sc.CustomPicture}");
                        NativeHelper.DisplayNotificationCustom(sc.CustomPicture, sc.CustomPicture, sc.ContactName, "~g~Text Received~s~", sc.Message, NotificationIconTypes.ChatBox, false);
                    }
                    else
                    {
                        NativeHelper.DisplayNotificationCustom(sc.IconName, sc.IconName, sc.ContactName, "~g~Text Received~s~", sc.Message, NotificationIconTypes.ChatBox, false);
                    }
                    PlayTexttone();
                    AddContact(sc.PhoneContact, true);
                }
                ScheduledTexts.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    private void CheckScheduledContacts()
    {
        for (int i = ScheduledContacts.Count - 1; i >= 0; i--)
        {
            ScheduledContact sc = ScheduledContacts[i];
            if (DateTime.Compare(Time.CurrentDateTime, sc.TimeToSend) >= 0 && (sc.SendImmediately || Game.GameTime - sc.GameTimeSent >= 7000))
            {
                if (!AddedContacts.Any(x => x.Name == sc.ContactName))
                {
                    AddContact(sc.PhoneContact, true);
                    PlayTexttone();
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
    public void AddContact(PhoneContact phoneContact, bool displayNotification)
    {
        if(phoneContact == null)
        {
            EntryPoint.WriteToConsole("AddContact PHONECONTACT IS NULL");

        }

        if (!AddedContacts.Any(x => x.Name == phoneContact.Name))
        {
            phoneContact.Index = ContactIndex;
            ContactIndex++;
            AddedContacts.Add(phoneContact);
            if (displayNotification)
            {
                NativeHelper.DisplayNotificationCustom(phoneContact.IconName, phoneContact.IconName, "New Contact", phoneContact.Name, NotificationIconTypes.AddFriendRequest, true);
                PlayTexttone();
            }
        }
    }



    public void AddScheduledText(PhoneContact phoneContact, string MessageToSend, int minutesToWait, bool sendImmediately)
    {
        AddScheduledText(phoneContact, MessageToSend, Time.CurrentDateTime.AddMinutes(minutesToWait), sendImmediately);
    }
    public void AddScheduledText(PhoneContact phoneContact, string MessageToSend, DateTime timeToAdd, bool sendImmediately)
    {
        EntryPoint.WriteToConsole("AddScheduledText");
        if (phoneContact == null)
        {
            return;
        }
        if (!ScheduledTexts.Any(x => x.ContactName == phoneContact.Name && x.Message == MessageToSend && x.TimeToSend == timeToAdd))
        {
            EntryPoint.WriteToConsole($"AddScheduledText ADD phoneContact{phoneContact.Name} timeToAdd:{timeToAdd} MessageToSend{MessageToSend}");
            ScheduledTexts.Add(new ScheduledText(timeToAdd, phoneContact, MessageToSend) { SendImmediately = sendImmediately });
        }
    }
    public void AddCustomScheduledText(PhoneContact phoneContact, string MessageToSend, DateTime timeToAdd, string customPicture, bool sendImmediately)
    {
        if (phoneContact == null)
        {
            return;
        }
        if (!ScheduledTexts.Any(x => x.ContactName == phoneContact.Name && x.Message == MessageToSend && x.TimeToSend == timeToAdd))
        {
            EntryPoint.WriteToConsole($"CUSTOM PICTURE SENT {customPicture}");
            EntryPoint.WriteToConsole($"AddScheduledText ADD phoneContact{phoneContact.Name} timeToAdd:{timeToAdd} MessageToSend{MessageToSend}");
            ScheduledTexts.Add(new ScheduledText(timeToAdd, phoneContact, MessageToSend) { CustomPicture = customPicture, SendImmediately = sendImmediately });
        }
    }
    public void AddText(string Name, string IconName, string message, int hourSent, int minuteSent, bool isRead, string customPicture)
    {
        if (!AddedTexts.Any(x => x.ContactName == Name && x.Message == message && x.HourSent == hourSent && x.MinuteSent == minuteSent))
        {
            PhoneText textA = new PhoneText(Name, TextIndex, message, hourSent, minuteSent) { CustomPicture = customPicture };
            textA.IconName = IconName;
            textA.IsRead = isRead;
            textA.TimeReceived = Time.CurrentDateTime;
            TextIndex++;
            AddedTexts.Add(textA);
            ReIndexTexts();

        }
    }
    public void ReIndexTexts()
    {
        int NewTextIndex = 0;
        foreach (PhoneText ifta in TextList.OrderByDescending(x => x.TimeReceived))
        {
            ifta.Index = NewTextIndex;
            NewTextIndex++;
        }
    }
    public void AddPhoneResponse(string Name, string IconName, string Message)
    {
        PhoneResponses.Add(new PhoneResponse(Name, IconName, Message, Time.CurrentDateTime));
        NativeHelper.DisplayNotificationCustom(IconName, IconName, Name, "~o~Response", Message, NotificationIconTypes.RightJumpingArrow, false);
        PlayPhoneResponseSound();
    }
    public void AddPhoneResponse(string Name, string Message)
    {
        string IconName = ContactList.FirstOrDefault(x => x.Name.ToLower() == Name.ToLower())?.IconName;
        PhoneResponses.Add(new PhoneResponse(Name, IconName, Message, Time.CurrentDateTime));
        NativeHelper.DisplayNotificationCustom(IconName, IconName, Name, "~o~Response", Message, NotificationIconTypes.RightJumpingArrow, false);
        PlayPhoneResponseSound();
    }
    public void DisableContact(string Name)
    {
        PhoneContact myContact = AddedContacts.FirstOrDefault(x => x.Name == Name);
        if (myContact != null)
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
    public void StopAudio()
    {
        if (!phoneAudioPlayer.IsAudioPlaying)
        {
            return;
        }
        phoneAudioPlayer.Abort();
    }
    public void PlayRingtone()
    {
        if (SleepMode)
        {
            return;
        }
        float volumeToUse = Volume.Clamp(0.0f, 1.0f);
        string ringToneToUse = Settings.SettingsManager.CellphoneSettings.DefaultCustomRingtoneNameNew;
        if (!string.IsNullOrEmpty(CustomRingtone))
        {
            ringToneToUse = CustomRingtone;
        }
        if (Settings.SettingsManager.CellphoneSettings.UseCustomRingtone)
        {
            string AudioPath = $"tones\\{ringToneToUse}";
            if (!phoneAudioPlayer.IsAudioPlaying)
            {
                phoneAudioPlayer.Play(AudioPath, volumeToUse, false, false);
            }
        }
        else
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Text_Arrive_Tone", "Phone_SoundSet_Default", 0);
        }
    }
    public void PlayTexttone()
    {
        if (SleepMode)
        {
            return;
        }
        float volumeToUse = Volume.Clamp(0.0f, 1.0f);
        string textToneToUse = Settings.SettingsManager.CellphoneSettings.DefaultCustomTexttoneNameNew;
        if (!string.IsNullOrEmpty(CustomTextTone))
        {
            textToneToUse = CustomTextTone;
        }
        if (Settings.SettingsManager.CellphoneSettings.UseCustomTexttone)
        {
            string AudioPath = $"tones\\{textToneToUse}";
            if (!phoneAudioPlayer.IsAudioPlaying)
            {
                phoneAudioPlayer.Play(AudioPath, volumeToUse, false, false);
            }
        }
        else
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Text_Arrive_Tone", "Phone_SoundSet_Default", 0);
        }
    }
    public void PreviewTextSound()
    {
        GameFiber.StartNew(delegate
        {
            StopAudio();
            GameFiber.Sleep(100);
            if (!phoneAudioPlayer.IsAudioPlaying)
            {
                PlayTexttone();
            }
        }, "PreviewTextSound");
    }
    public void PreviewRingtoneSound()
    {
        GameFiber.StartNew(delegate
        {
            StopAudio();
            GameFiber.Sleep(100);
            if (!phoneAudioPlayer.IsAudioPlaying)
            {
                PlayRingtone();
            }
        }, "PreviewTextSound");
    }
    private void PlayPhoneResponseSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Hang_Up", "Phone_SoundSet_Default", 0);
    }
    public void Start()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (!IsDisposed)
                {
                    Update();
                    GameFiber.Yield();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "CellPhone");
        GameFiber.StartNew(delegate
        {
            try
            {
                while (!IsDisposed)
                {
                    if (Settings.SettingsManager.CellphoneSettings.AllowBurnerPhone)
                    {
                        BurnerPhone.Update();
                    }
                    GameFiber.Yield();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "BurnerPhone");
        if (Settings.SettingsManager.CellphoneSettings.TerminateVanillaCellphone)
        {
            NativeFunction.Natives.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME("cellphone_flashhand");
            NativeFunction.Natives.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME("cellphone_controller");
        }
    }
    public PhoneContact GetCorruptCopContact()
    {
        if (Contacts.PossibleContacts.CorruptCopContact == null)
        {
            return null;
        }
        return ContactList.FirstOrDefault(x => Contacts.PossibleContacts.CorruptCopContact.Name == x.Name);
    }
    public void ClearPendingTexts()
    {
        ScheduledTexts.Clear();
        ScheduledContacts.Clear();
    }
    public void ClearPendingGangTexts(Gang gang)
    {
        if(gang == null)
        {
            return;
        }
        ScheduledTexts.RemoveAll(x => x.ContactName == gang.ContactName);
        ScheduledContacts.RemoveAll(x => x.ContactName == gang.ContactName);
    }
    public bool HasPendingItems(PhoneContact phoneContact)
    {
        if (phoneContact == null)
        {
            return false;
        }
        return ScheduledTexts.Any(x => x.ContactName == phoneContact.Name) || ScheduledContacts.Any(x => x.ContactName == phoneContact.Name);
    }
    public void ClearPendingTexts(PhoneContact phoneContact)
    {
        if (phoneContact == null)
        {
            return;
        }
        ScheduledTexts.RemoveAll(x => x.ContactName == phoneContact.Name);
        ScheduledContacts.RemoveAll(x => x.ContactName == phoneContact.Name);
    }

    public CorruptCopContact DefaultCorruptCopContact
    {
        get
        {
            return Contacts.PossibleContacts.CorruptCopContact;
        }
    }
    private class ScheduledContact
    {
        public ScheduledContact(DateTime timeToSend, PhoneContact phoneContact, string message)
        {
            TimeToSend = timeToSend;
            ContactName = phoneContact.Name;
            Message = message;
            IconName = phoneContact.IconName;
            GameTimeSent = Game.GameTime;
            PhoneContact = phoneContact;
        }
        public DateTime TimeToSend { get; set; }
        public string ContactName { get; set; }
        public string Message { get; set; } = "We need to talk";
        public string IconName { get; set; } = "CHAR_BLANK_ENTRY";
        public uint GameTimeSent { get; set; }
        public PhoneContact PhoneContact { get; set; }
        public bool SendImmediately { get; set; } = false;
    }
    private class ScheduledText
    {
        public ScheduledText(DateTime timeToSend, PhoneContact phoneContact, string message)
        {
            TimeToSend = timeToSend;
            ContactName = phoneContact.Name;
            Message = message;
            IconName = phoneContact.IconName;
            GameTimeSent = Game.GameTime;
            PhoneContact = phoneContact;
        }
        public DateTime TimeToSend { get; set; }
        public string ContactName { get; set; }
        public string Message { get; set; } = "We need to talk";
        public string IconName { get; set; } = "CHAR_BLANK_ENTRY";
        public uint GameTimeSent { get; set; }
        public PhoneContact PhoneContact { get; set; }
        public string CustomPicture { get; set; }
        public bool SendImmediately { get; set; } = false;
    }

}