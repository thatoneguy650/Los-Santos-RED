using LosSantosRED.lsr.Interface;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using ExtensionsMethods;

public class BurnerPhoneContactsApp : BurnerPhoneApp
{

    private bool isDialActive;
    private bool isBusyActive;
    private int dialSoundID = -1;
    private int busySoundID = -1;
    private int callTimer;
    private int busyTimer;

    private bool IsDisplayingNumpad;
    private int CurrentRow;
    private int CurrentIndex;
    private bool IsDisplayingCall;
    private int CurrentColumn;
    private int ColMax = 3;
    private string NumpadString;
    private IContacts Contacts;

    public BurnerPhoneContactsApp(BurnerPhone burnerPhone, ICellPhoneable player, ITimeReportable time, ISettingsProvideable settings, int index, IContacts contacts) : base(burnerPhone, player, time, settings, index, "Contacts", 5)
    {
        Contacts = contacts;
    }
    public override void Open(bool Reset)
    {
        IsDisplayingCall = false;
        IsDisplayingNumpad = false;
        NumpadString = "";
        BurnerPhone.SetHeader(Name);
        if(Reset)
        {
            CurrentRow = 0;
            CurrentColumn = 0;
         } 
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT_EMPTY");
        NativeFunction.Natives.xC3D0841A0CC546A6(2);//2
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        foreach (PhoneContact contact in Player.CellPhone.ContactList.OrderBy(x => x.Index))
        {
            DrawContact(contact);
        }
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "DISPLAY_VIEW");
        NativeFunction.Natives.xC3D0841A0CC546A6(2);
        NativeFunction.Natives.xC3D0841A0CC546A6(0);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }
    public override void HandleInput()
    {
        if(IsDisplayingNumpad)
        {
            HandleNumpadInput();
        }
        //else if (IsDisplayingCall)
        //{
        //    HandleOnCallInput();
        //}
        else if(!IsDisplayingCall)
        {
            HandleMainInput();
        }
        if (IsDisplayingNumpad)
        {
            BurnerPhone.SetSoftKey((int)SoftKey.Left, SoftKeyIcon.Call, Color.LightBlue);
            BurnerPhone.SetSoftKey((int)SoftKey.Middle, SoftKeyIcon.Select, Color.LightGreen);
            BurnerPhone.SetSoftKey((int)SoftKey.Right, SoftKeyIcon.Back, Color.Red);
        }
        else
        {
            BurnerPhone.SetSoftKey((int)SoftKey.Left, SoftKeyIcon.Keypad, Color.LightBlue);
            BurnerPhone.SetSoftKey((int)SoftKey.Middle, SoftKeyIcon.Call, Color.LightGreen);
            BurnerPhone.SetSoftKey((int)SoftKey.Right, SoftKeyIcon.Back, Color.Red);
        }
    }
    private void HandleMainInput()
    {
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 172))//UP
        {
            BurnerPhone.MoveFinger(1);
            BurnerPhone.NavigateMenu(1);
            CurrentRow = CurrentRow - 1;
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 173))//DOWN
        {
            BurnerPhone.MoveFinger(2);
            BurnerPhone.NavigateMenu(3);
            CurrentRow = CurrentRow + 1;
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176))//SELECT
        {
            BurnerPhone.MoveFinger(5);
            BurnerPhone.PlayAcceptedSound();
            PhoneContact contact = Player.CellPhone.ContactList.Where(x => x.Index == CurrentRow).FirstOrDefault();
            if (contact != null)
            {
                Call(contact);
            }
        }
        else if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(3, 177))//CLOSE
        {
            BurnerPhone.PlayBackSound();
            IsDisplayingNumpad = false;
            BurnerPhone.ReturnHome(Index);
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 179))//EXTRA OPTION
        {
            BurnerPhone.MoveFinger(5);
            BurnerPhone.PlayAcceptedSound();
            IsDisplayingNumpad = true;
            BurnerPhone.SetHeader("");
            DisplayNumpadUI();
        }
        int TotalContacts = Player.CellPhone.ContactList.Count();
        if (TotalContacts > 0)
        {
            if (CurrentRow > TotalContacts - 1)
            {
                CurrentRow = 0;
            }
        }
        if (CurrentRow < 0)
        {
            CurrentRow = TotalContacts - 1;
        }
    }
    private void HandleNumpadInput()
    {
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 172))//UP
        {
            BurnerPhone.MoveFinger(1);
            BurnerPhone.NavigateMenu(1);
            EntryPoint.WriteToConsole($"UP BEFORE Col:{CurrentColumn} Row:{CurrentRow}");
            CurrentRow = CurrentRow - 1;
            if(CurrentRow < 0)
            {
                CurrentRow = 3;
            }
            if(CurrentRow > 3)
            {
                CurrentRow = 0;
            }
            EntryPoint.WriteToConsole($"UP AFTER Col:{CurrentColumn} Row:{CurrentRow}");
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 173))//DOWN
        {
            BurnerPhone.MoveFinger(2);
            BurnerPhone.NavigateMenu(3);
            EntryPoint.WriteToConsole($"DOWN BEFORE Col:{CurrentColumn} Row:{CurrentRow}");
            CurrentRow = CurrentRow + 1;
            if(CurrentRow < 0)
            {
                CurrentRow = 3;
            }
            if (CurrentRow > 3)
            {
                CurrentRow = 0;
            }
            EntryPoint.WriteToConsole($"DOWN AFTER Col:{CurrentColumn} Row:{CurrentRow}");
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 174))//LEFT
        {
            BurnerPhone.NavigateMenu(4);
            BurnerPhone.MoveFinger(4);
            EntryPoint.WriteToConsole($"LEFT BEFORE Col:{CurrentColumn} Row:{CurrentRow}");
            CurrentColumn = CurrentColumn - 1;
            if (CurrentColumn < 0)
            {
                CurrentColumn = 2;
            }
            if (CurrentColumn > 2)
            {
                CurrentColumn = 0;
            }
            EntryPoint.WriteToConsole($"LEFT AFTER Col:{CurrentColumn} Row:{CurrentRow}");
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 175))//RIGHT
        {
            BurnerPhone.NavigateMenu(2);
            BurnerPhone.MoveFinger(4);
            EntryPoint.WriteToConsole($"RIGHT BEFORE Col:{CurrentColumn} Row:{CurrentRow}");
            CurrentColumn = CurrentColumn + 1;
            if (CurrentColumn < 0)
            {
                CurrentColumn = 2;
            }
            if (CurrentColumn > 2)
            {
                CurrentColumn = 0;
            }
            EntryPoint.WriteToConsole($"RIGHT AFTER Col:{CurrentColumn} Row:{CurrentRow}");
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176))//SELECT
        {
            BurnerPhone.MoveFinger(5);
            BurnerPhone.PlayAcceptedSound();
            NumpadString += GetCurrentNumpad();
            //Game.DisplaySubtitle(NumpadString);
            BurnerPhone.SetHeader(FormatPhoneNumber(NumpadString));
        }
        else if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(3, 177))//CLOSE
        {
            BurnerPhone.PlayBackSound();
            IsDisplayingNumpad = false;
            Open(true);
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 179))//EXTRA OPTION
        {
            BurnerPhone.MoveFinger(5);
            BurnerPhone.PlayAcceptedSound();
            PhoneContact tocall = Contacts.GetContactByNumber(NumpadString);
            if (tocall == null)
            {
                tocall = new PhoneContact(NumpadString) { Active = false };
            }
            else
            {
                Player.CellPhone.AddContact(tocall, false);
            }
            IsDisplayingNumpad = false;
            Call(tocall);
        }        
    }
    public string FormatPhoneNumber(string phoneNumber)
    {
        if(phoneNumber.Length <= 2)
        {
            return phoneNumber;
        }
        if(phoneNumber.Length == 3)
        {
            return $"({phoneNumber})";
        }
        if (phoneNumber.Length == 4)
        {
            return $"({phoneNumber.Left(3)}) {phoneNumber.Substring(3,1)}";
        }
        if (phoneNumber.Length == 5)
        {
            return $"({phoneNumber.Left(3)}) {phoneNumber.Substring(3, 2)}";
        }
        if (phoneNumber.Length == 6)
        {
            return $"({phoneNumber.Left(3)}) {phoneNumber.Substring(3, 3)}";
        }
        if (phoneNumber.Length == 7)
        {
            return $"({phoneNumber.Left(3)}) {phoneNumber.Substring(3, 3)}-{phoneNumber.Substring(6, 1)}";
        }
        if (phoneNumber.Length == 8)
        {
            return $"({phoneNumber.Left(3)}) {phoneNumber.Substring(3, 3)}-{phoneNumber.Substring(6, 2)}";
        }
        if (phoneNumber.Length == 9)
        {
            return $"({phoneNumber.Left(3)}) {phoneNumber.Substring(3, 3)}-{phoneNumber.Substring(6, 3)}";
        }
        if (phoneNumber.Length == 10)
        {
            return $"({phoneNumber.Left(3)}) {phoneNumber.Substring(3, 3)}-{phoneNumber.Substring(6, 4)}";
        }
        if (phoneNumber.Length > 10)
        {
            return $"({phoneNumber.Left(3)}) {phoneNumber.Substring(3, 3)}-{phoneNumber.Substring(6, phoneNumber.Length-6)}";
        }
        return phoneNumber;
        //string originalValue = phoneNumber;

        //phoneNumber = new System.Text.RegularExpressions.Regex(@"\D")
        //    .Replace(phoneNumber, string.Empty);

        //value = value.TrimStart('1');

        //if (phoneNumber.Length == 7)

        //    return Convert.ToInt64(value).ToString("###-####");
        //if (phoneNumber.Length == 9)

        //    return Convert.ToInt64(originalValue).ToString("###-###-####");
        //if (phoneNumber.Length == 10)

        //    return Convert.ToInt64(value).ToString("###-###-####");

        //if (phoneNumber.Length > 10)
        //    return Convert.ToInt64(phoneNumber)
        //        .ToString("###-###-#### " + new String('#', (phoneNumber.Length - 10)));

        //return phoneNumber;
    }
    private string GetCurrentNumpad()
    {
        if(CurrentRow == 0)
        {
            if (CurrentColumn == 0)
            {
                return "1";
            }
            else if (CurrentColumn == 1)
            {
                return "2";
            }
            else
            {
                return "3";
            }
        }
        else if(CurrentRow == 1)
        {
            if (CurrentColumn == 0)
            {
                return "4";
            }
            else if (CurrentColumn == 1)
            {
                return "5";
            }
            else
            {
                return "6";
            }
        }
        else if (CurrentRow == 2)
        {
            if (CurrentColumn == 0)
            {
                return "7";
            }
            else if (CurrentColumn == 1)
            {
                return "8";
            }
            else
            {
                return "9";
            }
        }
        else if (CurrentRow == 3)
        {
            if(CurrentColumn == 0)
            {
                return "*";
            }
            else if (CurrentColumn == 1)
            {
                return "0";
            }
            else
            {
                return "#";
            }
        }
        return "";
    }
    public override void Update()
    {
        if (BurnerPhone.LastCalledContact != null)
        {
            UpdateContact(BurnerPhone.LastCalledContact);
        }
    }
    public void OnLeftCall()
    {
        BurnerPhone.LastCalledContact = null;
        isDialActive = false;
        isBusyActive = false;
        NativeFunction.Natives.STOP_SOUND(busySoundID);
        NativeFunction.Natives.RELEASE_SOUND_ID(busySoundID);

        NativeFunction.Natives.STOP_SOUND(dialSoundID);
        NativeFunction.Natives.RELEASE_SOUND_ID(dialSoundID);
    }
    private void DrawContact(PhoneContact contact)
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT");
        NativeFunction.Natives.xC3D0841A0CC546A6(2);
        NativeFunction.Natives.xC3D0841A0CC546A6(contact.Index);
        NativeFunction.Natives.xC3D0841A0CC546A6(0);
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(contact.Name);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("CELL_999");
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("CELL_2000");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(contact.IconName);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }
    private void UpdateContact(PhoneContact contact)
    {
        // Contact was busy and busytimer has ended
        if (isBusyActive && Game.GameTime > busyTimer)
        {
            //Game.LocalPlayer.Character.Task.PutAwayMobilePhone();
            NativeFunction.Natives.TASK_USE_MOBILE_PHONE(Game.LocalPlayer.Character, false);
            NativeFunction.Natives.STOP_SOUND(busySoundID);
            NativeFunction.Natives.RELEASE_SOUND_ID(busySoundID);
            busySoundID = -1;
            isBusyActive = false;
            BurnerPhone.ReturnHome(Index);
        }
        // We are calling the contact
        if (isDialActive && Game.GameTime > callTimer)
        {
            NativeFunction.Natives.STOP_SOUND(dialSoundID);
            NativeFunction.Natives.RELEASE_SOUND_ID(dialSoundID);
            dialSoundID = -1;

            if (!contact.Active)
            {
                // Contact is busy, play the busy sound until the busytimer runs off
                DisplayCallUI(contact.Name, "CELL_220", contact.IconName.ToUpper()); // Displays "BUSY"
                busySoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(busySoundID, "Remote_Engaged", "Phone_SoundSet_Default", 1);
                busyTimer = (int)Game.GameTime + 1000;
                isBusyActive = true;
            }
            else
            {
                if (BurnerPhone.IsActive)
                {
                    DisplayCallUI(contact.Name, "CELL_219", contact.IconName.ToUpper()); // Displays "CONNECTED"
                    Player.CellPhone.ContactAnswered(contact);
                    //EntryPoint.WriteToConsole("BURNER PHONE UPDATE CALL CALLED!!!!");
                }
            }
            isDialActive = false;
        }
    }
    public void Call(PhoneContact contact)
    {
        if(contact == null)
        {
            return;
        }
        BurnerPhone.LastCalledContact = contact;
        if (contact.RandomizeDialTimeout)
        {
            contact.RandomizedDialTimeout = RandomItems.GetRandomNumberInt(1000, 5000);
        }
        IsDisplayingCall = true;


        // Cannot call if already on call or contact is busy (Active == false)
        if (isDialActive || isBusyActive)
        {
            return;
        }
        NativeFunction.Natives.TASK_USE_MOBILE_PHONE(Game.LocalPlayer.Character, true);
        // Do we have to wait before the contact pickup the phone?
        if (contact.CurrentDialTimeout > 0)
        {
            // Play the Dial sound
            DisplayCallUI(contact.Name, "CELL_220", contact.IconName.ToUpper()); // Displays "BUSY"
            dialSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(dialSoundID, "Dial_and_Remote_Ring", "Phone_SoundSet_Default", 1);
            callTimer = (int)Game.GameTime + contact.CurrentDialTimeout;
            isDialActive = true;
            //EntryPoint.WriteToConsole("BURNER PHONE CALL CALLED!!!!");
        }
        else
        {
            DisplayCallUI(contact.Name, "CELL_219", contact.IconName.ToUpper()); // Displays "CONNECTED"
            Player.CellPhone.ContactAnswered(contact);
            //OnAnswered(this); // Answer the phone instantly
        }


        DisplayCallUI(contact.Name, "CELL_211", contact.IconName.ToUpper());

    }
    public void DisplayCallUI(string contactName, string statusText = "CELL_211", string picName = "CELL_300")
    {
        string dialText;// = Game.GetGXTEntry(statusText); // "DIALING..." translated in current game's language
        unsafe
        {
            IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, statusText);
            dialText = Marshal.PtrToStringAnsi(ptr2);
        }

        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT");
        NativeFunction.Natives.xC3D0841A0CC546A6(4);
        NativeFunction.Natives.xC3D0841A0CC546A6(0);
        NativeFunction.Natives.xC3D0841A0CC546A6(3);

        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.x761B77454205A61D(contactName, -1);       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("CELL_2000");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(picName);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.x761B77454205A61D(dialText, -1);      //UI::_ADD_TEXT_COMPONENT_APP_TITLE
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "DISPLAY_VIEW");
        NativeFunction.Natives.xC3D0841A0CC546A6(4);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }


    public void DisplayNumpadUI()
    {
        CurrentRow = 0;
        CurrentColumn = 0;
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT_EMPTY");
        NativeFunction.Natives.xC3D0841A0CC546A6(11);

        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

        List<string> NumpadItems = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "*", "0", "#" };
        int i = 0;
        foreach(string item in NumpadItems)
        {
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT");
            NativeFunction.Natives.xC3D0841A0CC546A6(11);
            NativeFunction.Natives.xC3D0841A0CC546A6(i);
            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(item);
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
            i++;
        }



 

        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "DISPLAY_VIEW");
        NativeFunction.Natives.xC3D0841A0CC546A6(11);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();      
    }

}

