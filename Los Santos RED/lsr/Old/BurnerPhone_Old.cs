//using LosSantosRED.lsr.Helper;
//using LosSantosRED.lsr.Interface;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using static BurnerPhone_Old;

////needs to be cleaned up into classes, contacts app, messages app, home app, controller class etc. main class should mostly handle the player interaction (take out phone, do tasks set flags etc.)
//public class BurnerPhone_Old
//{
//    private bool isDialActive;
//    private bool isBusyActive;
//    private int dialSoundID = -1;
//    private int busySoundID = -1;
//    private int callTimer;
//    private int busyTimer;
//    private ICellPhoneable Player;
//    private ITimeReportable Time;
//    private int globalScaleformID;
//    private bool isPhoneActive;
//    private int CurrentApp;
//    private int CurrentColumn;
//    private int CurrentRow;
//    private int CurrentIndex;
//    private bool isVanillaPhoneDisabled;
//    private int prevCurrentRow;
//    private int prevCurrentColumn;
//    private int prevCurrentApp;
//    private int prevCurrentIndex;
//    private bool IsDisplayingTextMessage = false;
//    private ISettingsProvideable Settings;
//    private bool IsDisplayingCall;
//    private PhoneContact LastCalledContact;
//    private IModItems ModItems;

//    public int GlobalScaleformID => globalScaleformID;
//    public bool IsActive => isPhoneActive;
//    public BurnerPhone_Old(ICellPhoneable player, ITimeReportable time, ISettingsProvideable settings, IModItems modItems)
//    {
//        Player = player;
//        Time = time;
//        Settings = settings;
//        ModItems = modItems;
//    }
//    public void Setup()
//    {
//        NativeFunction.Natives.DESTROY_MOBILE_PHONE();
//        globalScaleformID = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>(Settings.SettingsManager.CellphoneSettings.BurnerCellScaleformName);
//        while(!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(globalScaleformID))
//        {
//            GameFiber.Yield();
//        }
//        SetHomeScreen();
//    }
//    public void Update()
//    {
//        if(isPhoneActive)
//        {
//            DetectInput();
//            UpdatePhone();
//            DebugCheck();
//        }
//        UpdateMessagesApp();
//        UpdateContactsApp();
//    }
//    public void ClosePhone()
//    {
//        if(isPhoneActive)
//        {
//            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Put_Away", "Phone_SoundSet_Michael", 0);
//        }
//        OnLeftCall();
//        isPhoneActive = false;
//        NativeFunction.Natives.DESTROY_MOBILE_PHONE();
//        Game.DisableControlAction(0, GameControl.Sprint, false);
//        if (Settings.SettingsManager.CellphoneSettings.AllowTerminateVanillaCellphoneScripts && !Settings.SettingsManager.CellphoneSettings.TerminateVanillaCellphone)
//        {
//            NativeHelper.StartScript("cellphone_flashhand", 1424);
//            NativeHelper.StartScript("cellphone_controller", 1424);
//        }
//    }
//    public void OpenPhone()
//    {
//        if (Settings.SettingsManager.CellphoneSettings.AllowTerminateVanillaCellphoneScripts)
//        {
//            NativeFunction.Natives.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME("cellphone_flashhand");
//            NativeFunction.Natives.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME("cellphone_controller");
//        }
//        globalScaleformID = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>(Settings.SettingsManager.CellphoneSettings.BurnerCellScaleformName);
//        while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(globalScaleformID))
//        {
//            GameFiber.Yield();
//        }
//        SetHomeScreen();
//        SetHomeMenuApp(globalScaleformID, 0, 2, "Texts", Player.CellPhone.TextList.Where(x => !x.IsRead).Count(), 100);
//        SetHomeMenuApp(globalScaleformID, 1, 5, "Contacts", 0, 100);
//        SetHomeMenuApp(globalScaleformID, 2, 0, "Flashlight", 0, 100);

//        isPhoneActive = true;
//        CurrentApp = 1;
//        CurrentColumn = 0;
//        CurrentRow = 0;
//        CurrentIndex = 0;
//        IsDisplayingTextMessage = false;
//        NativeFunction.Natives.CREATE_MOBILE_PHONE(Settings.SettingsManager.CellphoneSettings.BurnerCellPhoneTypeID);
//        NativeFunction.Natives.SET_MOBILE_PHONE_POSITION(Settings.SettingsManager.CellphoneSettings.BurnerCellPositionX, Settings.SettingsManager.CellphoneSettings.BurnerCellPositionY, Settings.SettingsManager.CellphoneSettings.BurnerCellPositionZ);
//        NativeFunction.Natives.SET_MOBILE_PHONE_ROTATION(-90f, 0f, 0f);
//        NativeFunction.Natives.SET_MOBILE_PHONE_SCALE(Settings.SettingsManager.CellphoneSettings.BurnerCellScale);
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "DISPLAY_VIEW");
//        NativeFunction.Natives.xC3D0841A0CC546A6(1);
//        NativeFunction.Natives.xC3D0841A0CC546A6(0);
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Pull_Out", "Phone_SoundSet_Michael", 0);
//        isVanillaPhoneDisabled = true;
//    }

//    public void ActivateFlashlight()
//    {
//        ClosePhone();
//        GameFiber.Sleep(500);
//        string modItemName = "iFruit Cellphone";
//        if(Settings.SettingsManager.CellphoneSettings.BurnerCellPhoneTypeID == 1)
//        {
//            modItemName = "Facade Cellphone";
//        }
//        else if (Settings.SettingsManager.CellphoneSettings.BurnerCellPhoneTypeID == 2)
//        {
//            modItemName = "Badger Cellphone";
//        }
//        Player.ActivityManager.UseInventoryItem(ModItems.Get(modItemName), true);
//    }
//    private void UpdatePhone()
//    {
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_TITLEBAR_TIME");
//        NativeFunction.Natives.xC3D0841A0CC546A6(Time.CurrentHour);
//        NativeFunction.Natives.xC3D0841A0CC546A6(Time.CurrentMinute);

//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Time.CurrentDateTime.ToString("ddd"));
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_SIGNAL_STRENGTH");
//        NativeFunction.Natives.xC3D0841A0CC546A6(5);//1-5
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

//        unsafe
//        {
//            int lol = 0;
//            NativeFunction.CallByName<bool>("GET_MOBILE_PHONE_RENDER_ID", &lol);
//            NativeFunction.Natives.SET_TEXT_RENDER_ID(lol);
//        }

//        NativeFunction.Natives.DRAW_SCALEFORM_MOVIE(globalScaleformID, 0.1f, 0.18f, 0.2f, 0.35f, 255, 255, 255, 255, 0);
//        NativeFunction.Natives.SET_TEXT_RENDER_ID(1);
//    }
//    private void SetHomeMenuApp(int scaleform, int index, int icon, string name, int notifications, int opactiy)
//    {
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(scaleform, "SET_DATA_SLOT");
//        NativeFunction.Natives.xC3D0841A0CC546A6(1);

//        NativeFunction.Natives.xC3D0841A0CC546A6(index);

//        NativeFunction.Natives.xC3D0841A0CC546A6(icon);

//        NativeFunction.Natives.xC3D0841A0CC546A6(notifications);

//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(name);
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//        NativeFunction.Natives.xC3D0841A0CC546A6(opactiy);

//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//    }
//    private void SetHomeScreen()
//    {
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_THEME");
//        NativeFunction.Natives.xC3D0841A0CC546A6(Settings.SettingsManager.CellphoneSettings.DefaultBurnerCellThemeID);
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_BACKGROUND_IMAGE");
//        NativeFunction.Natives.xC3D0841A0CC546A6(Settings.SettingsManager.CellphoneSettings.DefaultBurnerCellBackgroundID);
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_SLEEP_MODE");
//        NativeFunction.Natives.xC3D0841A0CC546A6(0);
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//    }
//    private void DetectInput()
//    {
//        DisabledVanillaControls();
//        if (CurrentApp == 1)
//        {
//            IsDisplayingCall = false;
//            IsDisplayingTextMessage = false;
//            HandleHomeInput();
//        }
//        else if (CurrentApp == 2)
//        {
//            IsDisplayingCall = false;
//            HandleMessagesInput();
//        }
//        else if (CurrentApp == 3)
//        {
//            IsDisplayingTextMessage = false;
//            HandleContactsInput();
//        }
//        else if (CurrentApp == 4)
//        {
//            IsDisplayingCall = false;
//            IsDisplayingTextMessage = false;
//        }
//    }
//    private void DisabledVanillaControls()
//    {
//        Game.DisableControlAction(0, GameControl.Sprint, true);
//        Game.DisableControlAction(3, GameControl.CellphoneUp, true);
//        Game.DisableControlAction(0, GameControl.CellphoneUp, true);
//        Game.DisableControlAction(3, GameControl.CellphoneDown, true);
//        Game.DisableControlAction(3, GameControl.CellphoneLeft, true);
//        Game.DisableControlAction(3, GameControl.CellphoneRight, true);
//        Game.DisableControlAction(3, GameControl.CellphoneSelect, true);
//        Game.DisableControlAction(3, GameControl.CellphoneCancel, true);
//    }


//    private void HandleHomeInput()
//    {
//        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 172))//UP
//        {
//            EntryPoint.WriteToConsole("Burner Phone: Pressed UP");
//            NavigateMenu(1);
//            MoveFinger(1);
//            CurrentRow = CurrentRow - 1;
//            //CurrentIndex = GetSelectedIndex();
//        }
//        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 173))//DOWN
//        {
//            EntryPoint.WriteToConsole("Burner Phone: Pressed DOWN");
//            NavigateMenu(3);
//            MoveFinger(2);
//            CurrentRow = CurrentRow + 1;
//            //CurrentIndex = GetSelectedIndex();
//        }
//        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 174))//LEFT
//        {
//            EntryPoint.WriteToConsole("Burner Phone: Pressed LEFT");
//            NavigateMenu(4);
//            MoveFinger(4);
//            CurrentColumn = CurrentColumn - 1;
//            //CurrentIndex = GetSelectedIndex();
//        }
//        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 175))//UP
//        {
//            EntryPoint.WriteToConsole("Burner Phone: Pressed RIGHT");
//            NavigateMenu(2);
//            MoveFinger(4);
//            CurrentColumn = CurrentColumn + 1;
//            //CurrentIndex = GetSelectedIndex();
//        }


//        CurrentColumn = CurrentColumn > 2 ? 0 : CurrentColumn;// CurrentColumn % 2;
//        CurrentRow = 0;// CurrentRow % 1;
//        CurrentIndex = GetCurrentIndex(CurrentColumn + 1, CurrentRow + 1);


//        SetSoftKeyIcon((int)SoftKey.Left, SoftKeyIcon.Select);
//        SetSoftKeyColor((int)SoftKey.Left, Color.FromArgb(46, 204, 113));

//        SetSoftKeyIcon((int)SoftKey.Middle, SoftKeyIcon.Blank);
//        SetSoftKeyColor((int)SoftKey.Middle, Color.Black);

//        SetSoftKeyIcon((int)SoftKey.Right, SoftKeyIcon.Back);
//        SetSoftKeyColor((int)SoftKey.Right, Color.Purple);




//        EntryPoint.WriteToConsole($"Burner Phone: Pressed SELECT CurrentIndex {CurrentIndex} OpenApp {CurrentIndex + 1}");


//        //CurrentIndex = GetSelectedIndex();

//        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176))//SELECT
//        {
//            EntryPoint.WriteToConsole($"Burner Phone: Pressed SELECT CurrentIndex {CurrentIndex} OpenApp {CurrentIndex + 1}");
//            MoveFinger(5);
//            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Accept", "Phone_SoundSet_Michael", 0);
//            OpenApp(CurrentIndex + 1);
//        }
//        if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(3, 177))//CLOSE
//        {
//            EntryPoint.WriteToConsole("Burner Phone: Pressed CLOSE");
//            ClosePhone();
//        }
//    }
//    private void OpenApp(int Index)
//    {
//        CurrentApp = Index;
//        if(Index == 2)//Messages
//        {
//            OpenMessagesApp();
//        }
//        else if (Index == 3)//Contacts
//        {
//            OpenContactsApp();
//        }
//        else if (Index == 4)//Contacts
//        {
//            ActivateFlashlight();
//        }
//    }




//    private void UpdateMessagesApp()
//    {

//    }
//    private void OpenMessagesApp()
//    {
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_DATA_SLOT_EMPTY");
//        NativeFunction.Natives.xC3D0841A0CC546A6(6);//2
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//        foreach (PhoneText text in Player.CellPhone.TextList.OrderBy(x => x.Index))
//        {
//            EntryPoint.WriteToConsole($"Adding Message {text.ContactName} {text.Index}");
//            DrawMessage(text);
//        }
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "DISPLAY_VIEW");
//        NativeFunction.Natives.xC3D0841A0CC546A6(6);
//        NativeFunction.Natives.xC3D0841A0CC546A6(CurrentRow);
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//    }
//    private void HandleMessagesInput()
//    {
//        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 172) && !IsDisplayingTextMessage)//UP
//        {
//            EntryPoint.WriteToConsole("Burner Phone: Pressed UP APP MESSAGES");
//            MoveFinger(1);
//            NavigateMenu(1);
//            //CurrentIndex = GetSelectedIndex();
//            CurrentRow = CurrentRow - 1;
//        }
//        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 173) && !IsDisplayingTextMessage)//DOWN
//        {
//            EntryPoint.WriteToConsole("Burner Phone: Pressed DOWN APP MESSAGES");
//            MoveFinger(2);
//            NavigateMenu(3);
//            //CurrentIndex = GetSelectedIndex();
//            CurrentRow = CurrentRow + 1;
//        }

//        int TotalMessages = Player.CellPhone.TextList.Count();
//        if (TotalMessages > 0)
//        {
//            if (CurrentRow > TotalMessages - 1)
//            {
//                CurrentRow = 0;
//            }
//        }
//        if (CurrentRow < 0)
//        {
//            CurrentRow = 0;
//        }
//        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176) && !IsDisplayingTextMessage)//SELECT
//        {
//            EntryPoint.WriteToConsole($"Burner Phone: Pressed MESSAGES CurrentIndex {CurrentIndex} OpenApp {CurrentIndex + 1}");
//            MoveFinger(5);
//            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Accept", "Phone_SoundSet_Michael", 0);
//            IsDisplayingTextMessage = true;


//            DisplayTextUI(Player.CellPhone.TextList.Where(x => x.Index == CurrentRow).FirstOrDefault());




//            //Game.DisplaySubtitle($"SELECTED {CurrentRow}");
//        }
//        if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(3, 177))//CLOSE
//        {

//            if (IsDisplayingTextMessage)
//            {
//                EntryPoint.WriteToConsole("Burner Phone: Pressed CLOSE APP READING MESSAGES");
//                NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Back", "Phone_SoundSet_Michael", 0);
//                IsDisplayingTextMessage = false;
//                OpenApp(2);
//            }
//            else
//            {
//                EntryPoint.WriteToConsole("Burner Phone: Pressed CLOSE APP MESSAGES");

//                NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Back", "Phone_SoundSet_Michael", 0);


//                NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "DISPLAY_VIEW");
//                NativeFunction.Natives.xC3D0841A0CC546A6(1);
//                NativeFunction.Natives.xC3D0841A0CC546A6(0);
//                NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();


//                GameFiber.Sleep(200);
//                CurrentColumn = 0;
//                CurrentRow = 0;
//                CurrentIndex = 0;
//                CurrentApp = 1;
//            }
//        }
//        if (!IsDisplayingTextMessage)
//        {
//            SetSoftKeyIcon((int)SoftKey.Left, SoftKeyIcon.Select);
//            SetSoftKeyColor((int)SoftKey.Left, Color.LightBlue);

//            SetSoftKeyIcon((int)SoftKey.Middle, SoftKeyIcon.Blank);
//            SetSoftKeyColor((int)SoftKey.Middle, Color.Black);

//            SetSoftKeyIcon((int)SoftKey.Right, SoftKeyIcon.Back);
//            SetSoftKeyColor((int)SoftKey.Right, Color.Purple);
//        }
//        else
//        {
//            SetSoftKeyIcon((int)SoftKey.Left, SoftKeyIcon.Delete);
//            SetSoftKeyColor((int)SoftKey.Left, Color.Red);

//            SetSoftKeyIcon((int)SoftKey.Middle, SoftKeyIcon.Call);
//            SetSoftKeyColor((int)SoftKey.Middle, Color.LightBlue);

//            SetSoftKeyIcon((int)SoftKey.Right, SoftKeyIcon.Back);
//            SetSoftKeyColor((int)SoftKey.Right, Color.Purple);
//        }
//    }
//    private void DrawMessage(PhoneText text)
//    {

//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_DATA_SLOT");
//        NativeFunction.Natives.xC3D0841A0CC546A6(6);//2
//        NativeFunction.Natives.xC3D0841A0CC546A6(text.Index);
//        NativeFunction.Natives.xC3D0841A0CC546A6(text.HourSent);
//        NativeFunction.Natives.xC3D0841A0CC546A6(text.MinuteSent);

//        if (text.IsRead)
//        {
//            NativeFunction.Natives.xC3D0841A0CC546A6(34);
//        }
//        else
//        {
//            NativeFunction.Natives.xC3D0841A0CC546A6(33);
//        }

//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text.ContactName);
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text.Message);
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

//    }




//    public void DisplayTextUI(PhoneText text)
//    {
//        if (text != null)
//        {
//            text.IsRead = true;
//            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_DATA_SLOT");
//            NativeFunction.Natives.xC3D0841A0CC546A6(7);
//            NativeFunction.Natives.xC3D0841A0CC546A6(0);

//            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text.ContactName);       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
//            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text.Message);       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
//            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("CHAR_BLANK_ENTRY");       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
//            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

//            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "DISPLAY_VIEW");
//            NativeFunction.Natives.xC3D0841A0CC546A6(7);
//            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

//            SetHomeMenuApp(globalScaleformID, 0, 2, "Texts", Player.CellPhone.TextList.Where(x => !x.IsRead).Count(), 100);
//        }
//    }
//    public void SetOffScreen()
//    {
//        NativeFunction.Natives.SET_MOBILE_PHONE_POSITION(0f, 0f, 0f);
//        NativeFunction.Natives.SET_MOBILE_PHONE_ROTATION(-90f, 0f, 0f);
//        NativeFunction.Natives.SET_MOBILE_PHONE_SCALE(Settings.SettingsManager.CellphoneSettings.BurnerCellScale);
//    }
//    private void OnLeftCall()
//    {
//        LastCalledContact = null;
//        isDialActive = false;
//        isBusyActive = false;
//        NativeFunction.Natives.STOP_SOUND(busySoundID);
//        NativeFunction.Natives.RELEASE_SOUND_ID(busySoundID);

//        NativeFunction.Natives.STOP_SOUND(dialSoundID);
//        NativeFunction.Natives.RELEASE_SOUND_ID(dialSoundID);
//    }
//    private void UpdateContactsApp()
//    {
//        if (LastCalledContact != null)
//        {
//            UpdateContact(LastCalledContact);
//        }
//    }
//    private void OpenContactsApp()
//    {
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_DATA_SLOT_EMPTY");
//        NativeFunction.Natives.xC3D0841A0CC546A6(2);//2
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//        foreach (PhoneContact contact in Player.CellPhone.ContactList.OrderBy(x=>x.Index))
//        {
//            EntryPoint.WriteToConsole($"Adding Contact {contact.Name} {contact.Index}");
//            DrawContact(contact);
//        }
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "DISPLAY_VIEW");
//        NativeFunction.Natives.xC3D0841A0CC546A6(2);
//        NativeFunction.Natives.xC3D0841A0CC546A6(0);
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//    }
//    private void HandleContactsInput()
//    {
//        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 172) && !IsDisplayingCall)//UP
//        {
//            EntryPoint.WriteToConsole("Burner Phone: Pressed UP APP CONTACTS");
//            MoveFinger(1);
//            NavigateMenu(1);
//            //CurrentIndex = GetSelectedIndex();
//            CurrentRow = CurrentRow - 1;
//        }
//        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 173) && !IsDisplayingCall)//DOWN
//        {
//            EntryPoint.WriteToConsole("Burner Phone: Pressed DOWN APP CONTACTS");
//            MoveFinger(2);
//            NavigateMenu(3);
//            //CurrentIndex = GetSelectedIndex();
//            CurrentRow = CurrentRow + 1;
//        }
//        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176) && !IsDisplayingCall)//SELECT
//        {
//            EntryPoint.WriteToConsole($"Burner Phone: Pressed SELECT CurrentIndex {CurrentIndex} OpenApp {CurrentIndex + 1}");
//            MoveFinger(5);
//            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Accept", "Phone_SoundSet_Michael", 0);
//            //Game.DisplaySubtitle($"SELECTED {CurrentRow}");
//            PhoneContact contact = Player.CellPhone.ContactList.Where(x => x.Index == CurrentRow).FirstOrDefault();
//            if (contact != null)
//            {
//                LastCalledContact = contact;
//                if (contact.RandomizeDialTimeout)
//                {
//                    contact.DialTimeout = RandomItems.GetRandomNumberInt(1000, 5000);
//                }
                



//                IsDisplayingCall = true;
//                Call(contact);
//                DisplayCallUI(contact.Name, "CELL_211", contact.IconName.ToUpper());
//            }
//        }
//        int TotalContacts = Player.CellPhone.ContactList.Count();
//        if (TotalContacts > 0)
//        {
//            if (CurrentRow > TotalContacts - 1)
//            {
//                CurrentRow = 0;
//            }
//        }
//        if (CurrentRow < 0)
//        {
//            CurrentRow = TotalContacts - 1;
//        }
//        if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(3, 177))//CLOSE
//        {
//            EntryPoint.WriteToConsole("Burner Phone: Pressed CLOSE APP CONTACTS");
//            if (IsDisplayingCall)
//            {
//                NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Back", "Phone_SoundSet_Michael", 0);
//                IsDisplayingCall = false;
//                NativeFunction.Natives.TASK_USE_MOBILE_PHONE(Game.LocalPlayer.Character, false);
//            }
//            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Back", "Phone_SoundSet_Michael", 0);
//            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "DISPLAY_VIEW");
//            NativeFunction.Natives.xC3D0841A0CC546A6(1);
//            NativeFunction.Natives.xC3D0841A0CC546A6(1);
//            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//            OnLeftCall();
//            GameFiber.Sleep(200);
//            CurrentColumn = 1;
//            CurrentRow = 0;
//            CurrentIndex = 1;
//            CurrentApp = 1;
//        }
//        SetSoftKeyIcon((int)SoftKey.Left, SoftKeyIcon.Call);
//        SetSoftKeyColor((int)SoftKey.Left, Color.LightBlue);

//        SetSoftKeyIcon((int)SoftKey.Middle, SoftKeyIcon.Blank);
//        SetSoftKeyColor((int)SoftKey.Middle, Color.Black);

//        SetSoftKeyIcon((int)SoftKey.Right, SoftKeyIcon.Back);
//        SetSoftKeyColor((int)SoftKey.Right, Color.Purple);
//    }



//    public void SetSoftKeyIcon(int buttonID, SoftKeyIcon icon)
//    {
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_SOFT_KEYS");
//        NativeFunction.Natives.xC3D0841A0CC546A6(buttonID);
//        NativeFunction.Natives.xC58424BA936EB458(true);
//        NativeFunction.Natives.xC3D0841A0CC546A6((int)icon);
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//    }
//    public void SetSoftKeyColor(int buttonID, Color color)
//    {
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_SOFT_KEYS_COLOUR");
//        NativeFunction.Natives.xC3D0841A0CC546A6(buttonID);
//        NativeFunction.Natives.xC3D0841A0CC546A6(color.R);
//        NativeFunction.Natives.xC3D0841A0CC546A6(color.G);
//        NativeFunction.Natives.xC3D0841A0CC546A6(color.B);
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//    }



//    private void DrawContact(PhoneContact contact)
//    {
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_DATA_SLOT");
//        NativeFunction.Natives.xC3D0841A0CC546A6(2);
//        NativeFunction.Natives.xC3D0841A0CC546A6(contact.Index);
//        NativeFunction.Natives.xC3D0841A0CC546A6(0);
//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(contact.Name);
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("CELL_999");
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("CELL_2000");
//        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(contact.IconName);
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();    
//    }
//    private void UpdateContact(PhoneContact contact)
//    {
//        // Contact was busy and busytimer has ended
//        if (isBusyActive && Game.GameTime > busyTimer)
//        {
//            //Game.LocalPlayer.Character.Task.PutAwayMobilePhone();
//            NativeFunction.Natives.TASK_USE_MOBILE_PHONE(Game.LocalPlayer.Character, false);
//            NativeFunction.Natives.STOP_SOUND(busySoundID);
//            NativeFunction.Natives.RELEASE_SOUND_ID(busySoundID);
//            busySoundID = -1;
//            isBusyActive = false;
//        }

//        // We are calling the contact
//        if (isDialActive && Game.GameTime > callTimer)
//        {
//            NativeFunction.Natives.STOP_SOUND(dialSoundID);
//            NativeFunction.Natives.RELEASE_SOUND_ID(dialSoundID);
//            dialSoundID = -1;

//            if (!contact.Active)
//            {
//                // Contact is busy, play the busy sound until the busytimer runs off
//                DisplayCallUI(contact.Name, "CELL_220", contact.IconName.ToUpper()); // Displays "BUSY"
//                busySoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
//                NativeFunction.Natives.PLAY_SOUND_FRONTEND(busySoundID, "Remote_Engaged", "Phone_SoundSet_Default", 1);
//                busyTimer = (int)Game.GameTime + 5000;
//                isBusyActive = true;
//            }
//            else
//            {
//                if (isPhoneActive && CurrentApp == 3)
//                {
//                    DisplayCallUI(contact.Name, "CELL_219", contact.IconName.ToUpper()); // Displays "CONNECTED"
//                    Player.CellPhone.ContactAnswered(contact);
//                    EntryPoint.WriteToConsole("BURNER PHONE UPDATE CALL CALLED!!!!");
//                }
//                //OnAnswered(this); // Answer the phone
//            }

//            isDialActive = false;
//        }
//    }
//    public void Call(PhoneContact contact)
//    {
//        // Cannot call if already on call or contact is busy (Active == false)
//        if (isDialActive || isBusyActive)
//        {
//            return;
//        }

//        NativeFunction.Natives.TASK_USE_MOBILE_PHONE(Game.LocalPlayer.Character, true);
//        // Do we have to wait before the contact pickup the phone?
//        if (contact.DialTimeout > 0)
//        {
//            // Play the Dial sound
//            DisplayCallUI(contact.Name, "CELL_220", contact.IconName.ToUpper()); // Displays "BUSY"
//            dialSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
//            NativeFunction.Natives.PLAY_SOUND_FRONTEND(dialSoundID, "Dial_and_Remote_Ring", "Phone_SoundSet_Default", 1);
//            callTimer = (int)Game.GameTime + contact.DialTimeout;
//            isDialActive = true;


//            EntryPoint.WriteToConsole("BURNER PHONE CALL CALLED!!!!");
//        }
//        else
//        {
//            DisplayCallUI(contact.Name, "CELL_219", contact.IconName.ToUpper()); // Displays "CONNECTED"
//            Player.CellPhone.ContactAnswered(contact);
//            //OnAnswered(this); // Answer the phone instantly
//        }
//    }
//    public void DisplayCallUI(string contactName, string statusText = "CELL_211", string picName = "CELL_300")
//    {
//        string dialText;// = Game.GetGXTEntry(statusText); // "DIALING..." translated in current game's language
//        unsafe
//        {
//            IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, statusText);
//            dialText = Marshal.PtrToStringAnsi(ptr2);
//        }

//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_DATA_SLOT");
//        NativeFunction.Natives.xC3D0841A0CC546A6(4);
//        NativeFunction.Natives.xC3D0841A0CC546A6(0);
//        NativeFunction.Natives.xC3D0841A0CC546A6(3);

//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//        NativeFunction.Natives.x761B77454205A61D(contactName, -1);       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("CELL_2000");
//        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(picName);
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//        NativeFunction.Natives.x761B77454205A61D(dialText, -1);      //UI::_ADD_TEXT_COMPONENT_APP_TITLE
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "DISPLAY_VIEW");
//        NativeFunction.Natives.xC3D0841A0CC546A6(4);
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//    }


//    private int GetCurrentIndex(int column, int row)
//    {
//        if (row == 1 && column == 1)
//            return 1;
//        else if(row == 1 && column == 2)
//            return 2;
//        else if (row == 1 && column == 3)
//            return 3;
//        //else if(row == 2 && column == 1)
//        //    return 4;
//        //else if(row == 2 && column == 2)
//        //    return 5;
//        //else if(row == 2 && column == 3)
//        //    return 6;
//        //else if(row == 3 && column == 1)
//        //    return 7;
//        //else if(row == 3 && column == 2)
//        //    return 8;
//        //else if(row == 3 && column == 3)
//        //    return 9;
//        else
//            return 1;
//    }

//    private void NavigateMenu(int index)
//    {
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_INPUT_EVENT");
//        NativeFunction.Natives.xC3D0841A0CC546A6(index);
//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Navigate", "Phone_SoundSet_Michael", 0);
//    }
//    private void MoveFinger(int index)
//    {
//        NativeFunction.Natives.x95C9E72F3D7DEC9B(index);
//    }


//    private void DebugCheck()
//    {
//        if (prevCurrentRow != CurrentRow)
//        {
//            EntryPoint.WriteToConsole($"CurrentRow Changed from {prevCurrentRow} to {CurrentRow}");
//            prevCurrentRow = CurrentRow;
//        }
//        if (prevCurrentColumn != CurrentColumn)
//        {
//            EntryPoint.WriteToConsole($"CurrentColumn Changed from {prevCurrentColumn} to {CurrentColumn}");
//            prevCurrentColumn = CurrentColumn;
//        }
//        if (prevCurrentApp != CurrentApp)
//        {
//            EntryPoint.WriteToConsole($"CurrentApp Changed from {prevCurrentApp} to {CurrentApp}");
//            prevCurrentApp = CurrentApp;
//        }
//        if (prevCurrentIndex != CurrentIndex)
//        {
//            EntryPoint.WriteToConsole($"CurrentIndex Changed from {prevCurrentIndex} to {CurrentIndex}");
//            prevCurrentIndex = CurrentIndex;
//        }
//    }
//}

