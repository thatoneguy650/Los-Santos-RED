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

public class BurnerPhoneContactsApp : BurnerPhoneApp
{

    private bool isDialActive;
    private bool isBusyActive;
    private int dialSoundID = -1;
    private int busySoundID = -1;
    private int callTimer;
    private int busyTimer;

    private bool IsDisplayingTextMessage;
    private int CurrentRow;
    private int CurrentIndex;
    private bool IsDisplayingCall;

    public BurnerPhoneContactsApp(BurnerPhone burnerPhone, ICellPhoneable player, ITimeReportable time, ISettingsProvideable settings, int index) : base(burnerPhone, player, time, settings, index, "Contacts", 5)
    {
    }
    public override void Open(bool Reset)
    {
        BurnerPhone.SetHeader(Name);
        if(Reset)
        {
            CurrentRow = 0;
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
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 172) && !IsDisplayingCall)//UP
        {
            BurnerPhone.MoveFinger(1);
            BurnerPhone.NavigateMenu(1);
            CurrentRow = CurrentRow - 1;
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 173) && !IsDisplayingCall)//DOWN
        {
            BurnerPhone.MoveFinger(2);
            BurnerPhone.NavigateMenu(3);
            CurrentRow = CurrentRow + 1;
        }
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176) && !IsDisplayingCall)//SELECT
        {
            BurnerPhone.MoveFinger(5);
            BurnerPhone.PlayAcceptedSound();
            PhoneContact contact = Player.CellPhone.ContactList.Where(x => x.Index == CurrentRow).FirstOrDefault();
            if (contact != null)
            {
                BurnerPhone.LastCalledContact = contact;
                if (contact.RandomizeDialTimeout)
                {
                    contact.DialTimeout = RandomItems.GetRandomNumberInt(1000, 5000);
                }
                IsDisplayingCall = true;
                Call(contact);
                DisplayCallUI(contact.Name, "CELL_211", contact.IconName.ToUpper());
            }
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
        if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(3, 177))//CLOSE
        {
            BurnerPhone.PlayBackSound();
            if (IsDisplayingCall)
            {
                IsDisplayingCall = false;
                NativeFunction.Natives.TASK_USE_MOBILE_PHONE(Game.LocalPlayer.Character, false);
            }
            OnLeftCall();
            //GameFiber.Sleep(200);
            BurnerPhone.ReturnHome(Index);
        }
        BurnerPhone.SetSoftKey((int)SoftKey.Left, SoftKeyIcon.Call, Color.LightBlue);
        BurnerPhone.SetSoftKey((int)SoftKey.Middle, SoftKeyIcon.Blank, Color.Black);
        BurnerPhone.SetSoftKey((int)SoftKey.Right, SoftKeyIcon.Back, Color.Purple);
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
                busyTimer = (int)Game.GameTime + 5000;
                isBusyActive = true;
            }
            else
            {
                if (BurnerPhone.IsActive)
                {
                    DisplayCallUI(contact.Name, "CELL_219", contact.IconName.ToUpper()); // Displays "CONNECTED"
                    Player.CellPhone.ContactAnswered(contact);
                    EntryPoint.WriteToConsole("BURNER PHONE UPDATE CALL CALLED!!!!");
                }
            }
            isDialActive = false;
        }
    }
    public void Call(PhoneContact contact)
    {
        // Cannot call if already on call or contact is busy (Active == false)
        if (isDialActive || isBusyActive)
        {
            return;
        }
        NativeFunction.Natives.TASK_USE_MOBILE_PHONE(Game.LocalPlayer.Character, true);
        // Do we have to wait before the contact pickup the phone?
        if (contact.DialTimeout > 0)
        {
            // Play the Dial sound
            DisplayCallUI(contact.Name, "CELL_220", contact.IconName.ToUpper()); // Displays "BUSY"
            dialSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(dialSoundID, "Dial_and_Remote_Ring", "Phone_SoundSet_Default", 1);
            callTimer = (int)Game.GameTime + contact.DialTimeout;
            isDialActive = true;
            EntryPoint.WriteToConsole("BURNER PHONE CALL CALLED!!!!");
        }
        else
        {
            DisplayCallUI(contact.Name, "CELL_219", contact.IconName.ToUpper()); // Displays "CONNECTED"
            Player.CellPhone.ContactAnswered(contact);
            //OnAnswered(this); // Answer the phone instantly
        }
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
}

