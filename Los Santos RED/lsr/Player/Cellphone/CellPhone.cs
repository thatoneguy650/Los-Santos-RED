using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using static DispatchScannerFiles;

public class CellPhone
{
    private ICellPhoneable Player;
    private int ContactIndex = 40;
    private int BurnerContactIndex = 0;
    private int TextIndex = 0;
    private MenuPool MenuPool;
    private IJurisdictions Jurisdictions;
    private List<PhoneContact> AddedContacts = new List<PhoneContact>();




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
    private BurnerPhone BurnerPhone;
    private IEntityProvideable World;
    private ICrimes Crimes;
    private uint GameTimeLastCheckedScheduledItems;
    private uint GameTimeBetweenCheckScheduledItems = 15000;
    private NAudioPlayer phoneAudioPlayer;

    private bool ShouldCheckScheduledItems => GameTimeLastCheckedScheduledItems == 0 || Game.GameTime - GameTimeLastCheckedScheduledItems >= GameTimeBetweenCheckScheduledItems;
    public bool IsActive => BurnerPhone?.IsActive == true;
    public List<PhoneText> TextList => AddedTexts;
    public List<PhoneContact> ContactList => AddedContacts;
    public List<PhoneResponse> PhoneResponseList => PhoneResponses;
    public CellPhone(ICellPhoneable player, IContactInteractable gangInteractable, IJurisdictions jurisdictions, ISettingsProvideable settings, ITimeReportable time, IGangs gangs, IPlacesOfInterest placesOfInterest, IZones zones, IStreets streets, IGangTerritories gangTerritories, ICrimes crimes, IEntityProvideable world, IModItems modItems)
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
        Crimes = crimes;
        World = world;
        BurnerPhone = new BurnerPhone(Player, Time, Settings, modItems);
        phoneAudioPlayer = new NAudioPlayer(Settings);
    }
    public void Setup()
    {
        AddContact(new EmergencyServicesContact(StaticStrings.EmergencyServicesContactName, "CHAR_CALL911"), false);
        BurnerPhone.Setup();
    }
    public void ContactAnswered(PhoneContact contact)
    {
        if (!BurnerPhone.IsActive)
        {
            //NativeFunction.Natives.CREATE_MOBILE_PHONE(Settings.SettingsManager.CellphoneSettings.BurnerCellPhoneTypeID);
            //isRunningForcedMobileTask = true;
            //NativeFunction.Natives.TASK_USE_MOBILE_PHONE(Game.LocalPlayer.Character, true);
            //Player.Character.KeepTasks = true;
            //if (Settings.SettingsManager.CellphoneSettings.AllowBurnerPhone)
            //{
            //    BurnerPhone.DisplayCallUI(contact.Name, "CELL_219", contact.IconName.ToUpper());
            //}
            //else
            //{
            //    BurnerPhone.SetOffScreen();
            //}
        }
        else
        {
            isRunningForcedMobileTask = false;
        }
        contact.OnAnswered(ContactInteractable, this, Gangs, PlacesOfInterest, Settings, Jurisdictions, Crimes, World);
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
        foreach(PhoneContact phoneContact in ContactList)
        {
            phoneContact.MenuInteraction?.Update();
        }
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
        AddContact(new EmergencyServicesContact(StaticStrings.EmergencyServicesContactName, "CHAR_CALL911"), false);
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
        //}
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

        Player.CellPhone.AddScheduledText(new PhoneContact(ScammerNames.PickRandom(), "CHAR_BLANK_ENTRY"), ScammerMessages.PickRandom(),0);
        CheckScheduledTexts();
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
            GameTimeBetweenCheckScheduledItems = RandomItems.GetRandomNumber(15000, 25000);
            GameTimeLastCheckedScheduledItems = Game.GameTime;
        }
    }
    private bool CheckScheduledTexts()
    {
        for (int i = ScheduledTexts.Count - 1; i >= 0; i--)
        {
            ScheduledText sc = ScheduledTexts[i];
            if (DateTime.Compare(Time.CurrentDateTime, sc.TimeToSend) >= 0 && Game.GameTime - sc.GameTimeSent >= 15000)
            {
                if (!AddedTexts.Any(x => x.ContactName == sc.ContactName && x.Message == sc.Message))
                {
                    AddText(sc.ContactName, sc.IconName, sc.Message, Time.CurrentHour, Time.CurrentMinute, false);
                    NativeHelper.DisplayNotificationCustom(sc.IconName, sc.IconName, sc.ContactName, "~g~Text Received~s~", sc.Message, NotificationIconTypes.ChatBox, false);
                    PlayTextReceivedSound();
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
            if (DateTime.Compare(Time.CurrentDateTime, sc.TimeToSend) >= 0 && Game.GameTime - sc.GameTimeSent >= 15000)
            {
                if (!AddedContacts.Any(x => x.Name == sc.ContactName))
                {
                    AddContact(sc.PhoneContact, true);
                    PlayTextReceivedSound();
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
        if (!AddedContacts.Any(x=> x.Name == phoneContact.Name))
        {
            phoneContact.Index = ContactIndex;
            ContactIndex++;
            AddedContacts.Add(phoneContact);
            if (displayNotification)
            {
                NativeHelper.DisplayNotificationCustom(phoneContact.IconName, phoneContact.IconName, "New Contact", phoneContact.Name, NotificationIconTypes.AddFriendRequest, true);
                PlayTextReceivedSound();
            }
        }
    }
    public void AddScheduledText(PhoneContact phoneContact, string MessageToSend, int minutesToWait)
    {
        AddScheduledText(phoneContact, MessageToSend, Time.CurrentDateTime.AddMinutes(minutesToWait));
    }
    public void AddScheduledText(PhoneContact phoneContact, string MessageToSend)
    {
        AddScheduledText(phoneContact, MessageToSend, 0);
    }
    public void AddScheduledText(PhoneContact phoneContact, string MessageToSend, DateTime timeToAdd)
    {
        if (!AddedTexts.Any(x => x.ContactName == phoneContact.Name && x.Message == MessageToSend))
        {
            ScheduledTexts.Add(new ScheduledText(timeToAdd, phoneContact, MessageToSend));
        }
    }
    public void AddText(string Name, string IconName, string message, int hourSent, int minuteSent, bool isRead)
    {
        if (!AddedTexts.Any(x => x.ContactName == Name && x.Message == message && x.HourSent == hourSent && x.MinuteSent == minuteSent))
        {
            PhoneText textA = new PhoneText(Name, TextIndex, message, hourSent, minuteSent);
            textA.IconName = IconName;
            textA.IsRead = isRead;
            textA.TimeReceived = Time.CurrentDateTime;
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
    private void PlayTextReceivedSound()
    {

        if (Settings.SettingsManager.CellphoneSettings.UseCustomRingtone)
        {
            string AudioPath = $"ringtones\\{Settings.SettingsManager.CellphoneSettings.CustomRingtoneName}";
            if (!phoneAudioPlayer.IsAudioPlaying)
            {
                if (Settings.SettingsManager.CellphoneSettings.SetCustomRingtoneVolume)
                {
                    phoneAudioPlayer.Play(AudioPath, Settings.SettingsManager.CellphoneSettings.CustomRingtoneVolume, false, false);
                }
                else
                {
                    phoneAudioPlayer.Play(AudioPath, 0.5f, false, false);
                }
            }
            //NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Text_Arrive_Tone", "Phone_SoundSet_Default", 0);
        }
        else
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Text_Arrive_Tone", "Phone_SoundSet_Default", 0);
        }
    }
    private void PlayPhoneResponseSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Hang_Up", "Phone_SoundSet_Default", 0);
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
    }

}

//public void AddGangText(PhoneContact phoneContact, Gang gang, bool isPositive)
//{
//    if (gang != null)
//    {
//        List<string> Replies = new List<string>();
//        if (isPositive)
//        {
//            Replies.AddRange(new List<string>() {
//                $"Heard some good things about you, come see us sometime.",
//                $"Call us soon to discuss business.",
//                $"Might have some business opportunites for you soon, give us a call.",
//                $"You've been making some impressive moves, call us to discuss.",
//                $"Give us a call soon.",
//                $"We may have some opportunites for you.",
//                $"My guys tell me you are legit, hit us up sometime.",
//                $"Looking for people I can trust, if so give us a call.",
//                $"Word has gotten around about you, mostly positive, give us a call soon.",
//                $"Always looking for help with some 'items'. Call us if you think you can handle it.",
//            });
//        }
//        else
//        {
//            Replies.AddRange(new List<string>() {
//                $"Watch your back",
//                $"Dead man walking",
//                $"ur fucking dead",
//                $"You just fucked with the wrong people asshole",
//                $"We're gonna fuck you up buddy",
//                $"My boys are gonna skin you alive prick.",
//                $"You will die slowly.",
//                $"I'll take pleasure in guttin you boy.",
//                $"Better leave LS while you can...",
//                $"We'll be waiting for you asshole.",
//                $"You're gonna wish you were dead motherfucker.",
//                $"Got some 'associates' out looking for you prick. Where you at?",


//                $"We'll be seeing you soon",
//                $"{Player.PlayerName}? Better watch out.",
//                $"You'll never hear us coming",
//                $"You are a dead man",
//                $"You're gonna find out what happens when you fuck with us asshole.",
//                $"When my boys find you...",
//            });
//        }

//        List<ZoneJurisdiction> myGangTerritories = GangTerritories.GetGangTerritory(gang.ID);
//        ZoneJurisdiction mainTerritory = myGangTerritories.OrderBy(x => x.Priority).FirstOrDefault();

//        if (mainTerritory != null)
//        {
//            Zone mainGangZone = Zones.GetZone(mainTerritory.ZoneInternalGameName);
//            if (mainGangZone != null)
//            {
//                if (isPositive)
//                {
//                    Replies.AddRange(new List<string>() {
//                        $"Heard some good things about you, come see us sometime in ~p~{mainGangZone.DisplayName}~s~ to discuss some business",
//                        $"Call us soon to discuss business in ~p~{mainGangZone.DisplayName}~s~.",
//                        $"Might have some business opportunites for you soon in ~p~{mainGangZone.DisplayName}~s~, give us a call.",
//                        $"You've been making some impressive moves, call us to discuss.",
//                    });
//                }
//                else
//                {
//                    Replies.AddRange(new List<string>() {
//                        $"Watch your back next time you are in ~p~{mainGangZone.DisplayName}~s~ motherfucker",
//                        $"You are dead next time we see you in ~p~{mainGangZone.DisplayName}~s~",
//                        $"Better stay out of ~p~{mainGangZone.DisplayName}~s~ cocksucker",
//                    });
//                }
//            }
//        }
//        string MessageToSend;
//        MessageToSend = Replies.PickRandom();
//        AddScheduledText(phoneContact, MessageToSend);
//    }
//}