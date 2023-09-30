using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using static BurnerPhone_Old;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

public class BurnerPhone
{
    private int MaxColumns = 3;
    private int MaxRows = 1;
    private int ColMax => MaxColumns - 1;
    private int RowMax => MaxRows - 1;
    private ICellPhoneable Player;
    private ITimeReportable Time;
    private int globalScaleformID;
    private bool isPhoneActive;
    private int CurrentApp;
    private int CurrentColumn;
    private int CurrentRow;
    private bool isVanillaPhoneDisabled;
    private int prevCurrentRow;
    private int prevCurrentColumn;
    private int prevCurrentApp;
    private int prevCurrentIndex;
    private ISettingsProvideable Settings;
    private IContacts Contacts;
    //private bool IsDisplayingCall;

    private IModItems ModItems;

    private List<BurnerPhoneApp> PhoneApps = new List<BurnerPhoneApp>();
    public BurnerPhoneMessagesApp MessagesApp { get; private set; }
    public BurnerPhoneContactsApp ContactsApp { get; private set; }
    public BurnerPhoneFlashlightApp FlashlightApp { get; private set; }
    public BurnerPhoneSettingsApp SettingsApp { get; private set; }
    private BurnerPhoneApp CurrentBurnerApp;
    private bool pressedDirection;

    public PhoneContact LastCalledContact { get; set; }

    public int GlobalScaleformID => globalScaleformID;
    public bool IsActive => isPhoneActive;
    private int CurrentIndex => (3 * CurrentRow) + CurrentColumn;
    public BurnerPhone(ICellPhoneable player, ITimeReportable time, ISettingsProvideable settings, IModItems modItems, IContacts contacts)
    {
        Player = player;
        Time = time;
        Settings = settings;
        ModItems = modItems;
        Contacts = contacts;
    }
    public void Setup()
    {
        NativeFunction.Natives.DESTROY_MOBILE_PHONE();
        PhoneApps = new List<BurnerPhoneApp>();
        MessagesApp = new BurnerPhoneMessagesApp(this, Player, Time, Settings, 0);
        ContactsApp = new BurnerPhoneContactsApp(this, Player, Time, Settings, 1, Contacts);
        FlashlightApp = new BurnerPhoneFlashlightApp(this, Player, Time, Settings, 2, ModItems);
        SettingsApp = new BurnerPhoneSettingsApp(this, Player, Time, Settings, 3);

        PhoneApps.Add(MessagesApp);
        PhoneApps.Add(ContactsApp);
        if (Player.CellPhone.CurrentCellphoneData?.HasFlashlight == true)
        {
            PhoneApps.Add(FlashlightApp);
        }
        PhoneApps.Add(SettingsApp);

        MaxColumns = 3;//hardcoded to the phone
        MaxRows = 1 + (PhoneApps.Count() / 3);

        foreach (BurnerPhoneApp burnerPhoneApp in PhoneApps)
        {
            burnerPhoneApp.Setup();
        }
    }
    public void Update()
    {
        if (isPhoneActive)
        {
            DisabledVanillaControls();
            if (CurrentBurnerApp == null)
            {
                UpdateHomeScreen();
            }
            else
            {
                CurrentBurnerApp.HandleInput();
            }
            UpdatePhone();
        }
        foreach (BurnerPhoneApp bpa in PhoneApps)
        {
            bpa.Update();
        }
    }
    public void ClosePhone()
    {
        if (isPhoneActive)
        {
            PlayPutAwaySound();
        }
        ContactsApp.OnLeftCall();
        isPhoneActive = false;
        NativeFunction.Natives.DESTROY_MOBILE_PHONE();
        Game.DisableControlAction(0, GameControl.Sprint, false);
        if (Settings.SettingsManager.CellphoneSettings.AllowTerminateVanillaCellphoneScripts && !Settings.SettingsManager.CellphoneSettings.TerminateVanillaCellphone)
        {
            NativeHelper.StartScript("cellphone_flashhand", 1424);
            NativeHelper.StartScript("cellphone_controller", 1424);
        }
    }
    public void OpenPhone()
    {
        if (Settings.SettingsManager.CellphoneSettings.AllowTerminateVanillaCellphoneScripts)
        {
            NativeFunction.Natives.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME("cellphone_flashhand");
            NativeFunction.Natives.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME("cellphone_controller");
        }
        RequestScaleform();
        SetHomeScreen();
        int Index = 0;
        foreach (BurnerPhoneApp bpa in PhoneApps)
        {
            bpa.SetHomeMenu();
            Index++;
        }
        isPhoneActive = true;
        CurrentBurnerApp = null;
        CurrentApp = 0;
        CurrentColumn = 0;
        CurrentRow = 0;
        NativeFunction.Natives.CREATE_MOBILE_PHONE(Player.CellPhone.PhoneType);
        NativeFunction.Natives.SET_MOBILE_PHONE_POSITION(Settings.SettingsManager.CellphoneSettings.BurnerCellPositionX, Settings.SettingsManager.CellphoneSettings.BurnerCellPositionY, Settings.SettingsManager.CellphoneSettings.BurnerCellPositionZ);
        NativeFunction.Natives.SET_MOBILE_PHONE_ROTATION(-90f, 0f, 0f);
        NativeFunction.Natives.SET_MOBILE_PHONE_SCALE(Settings.SettingsManager.CellphoneSettings.BurnerCellScale);
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "DISPLAY_VIEW");
        NativeFunction.Natives.xC3D0841A0CC546A6(1);
        NativeFunction.Natives.xC3D0841A0CC546A6(0);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        PlayPullOutSound();
        isVanillaPhoneDisabled = true;
    }
    public void ReturnHome(int Index)
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(GlobalScaleformID, "DISPLAY_VIEW");
        NativeFunction.Natives.xC3D0841A0CC546A6(1);
        NativeFunction.Natives.xC3D0841A0CC546A6(Index);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

        //int CurrentSelectedIndex = GetSelectedIndex();
        //CurrentRow = CurrentSelectedIndex / MaxColumns;
        //CurrentColumn = CurrentSelectedIndex % MaxColumns;
        //EntryPoint.WriteToConsole($"ReturnHome CurrentSelectedIndex:{CurrentSelectedIndex} CurrentRow {CurrentRow} CurrentColumn {CurrentColumn}");
        CurrentBurnerApp = null;
    }
    public void SetHeader(string Header)
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(GlobalScaleformID, "SET_HEADER");
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Header);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }
    public void PlayAcceptedSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Accept", "Phone_SoundSet_Michael", 0);
    }
    public void UpdateThemeItems()
    {
        SetHomeScreen();
    }
    public void PlayBackSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Back", "Phone_SoundSet_Michael", 0);
    }
    public void PlayPutAwaySound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Put_Away", "Phone_SoundSet_Michael", 0);
    }
    public void PlayPullOutSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Pull_Out", "Phone_SoundSet_Michael", 0);
    }
    public void PlayNavigateSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Navigate", "Phone_SoundSet_Michael", 0);
    }
    private void UpdatePhone()
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_TITLEBAR_TIME");
        NativeFunction.Natives.xC3D0841A0CC546A6(Time.CurrentHour);
        NativeFunction.Natives.xC3D0841A0CC546A6(Time.CurrentMinute);
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Time.CurrentDateTime.ToString("ddd"));
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_SIGNAL_STRENGTH");
        NativeFunction.Natives.xC3D0841A0CC546A6(5);//1-5
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("GET_MOBILE_PHONE_RENDER_ID", &lol);
            NativeFunction.Natives.SET_TEXT_RENDER_ID(lol);
        }
        NativeFunction.Natives.DRAW_SCALEFORM_MOVIE(globalScaleformID, 0.1f, 0.18f, 0.2f, 0.35f, 255, 255, 255, 255, 0);
        NativeFunction.Natives.SET_TEXT_RENDER_ID(1);
    }
    private void SetHomeScreen()
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_THEME");
        NativeFunction.Natives.xC3D0841A0CC546A6(Player.CellPhone.Theme);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_BACKGROUND_IMAGE");
        NativeFunction.Natives.xC3D0841A0CC546A6(Player.CellPhone.Background);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_SLEEP_MODE");
        NativeFunction.Natives.xC3D0841A0CC546A6(Player.CellPhone.SleepMode ? 1 : 0);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }
    private void DisabledVanillaControls()
    {
        Game.DisableControlAction(0, GameControl.Sprint, true);
        Game.DisableControlAction(3, GameControl.CellphoneUp, true);
        Game.DisableControlAction(0, GameControl.CellphoneUp, true);
        Game.DisableControlAction(3, GameControl.CellphoneDown, true);
        Game.DisableControlAction(3, GameControl.CellphoneLeft, true);
        Game.DisableControlAction(3, GameControl.CellphoneRight, true);
        Game.DisableControlAction(3, GameControl.CellphoneSelect, true);
        Game.DisableControlAction(3, GameControl.CellphoneCancel, true);


        Game.DisableControlAction(0, GameControl.Attack, true);
        Game.DisableControlAction(0, GameControl.Attack2, true);
        Game.DisableControlAction(0, GameControl.VehicleAttack, true);
        Game.DisableControlAction(0, GameControl.VehicleAttack2, true);

        if (Player.IsInVehicle)
        {
            Game.DisableControlAction(0, GameControl.VehicleDuck, true);
            Game.DisableControlAction(0, GameControl.VehicleCinCam, true);
            Game.DisableControlAction(0, GameControl.VehicleSelectNextWeapon, true);
            Game.DisableControlAction(0, GameControl.VehicleSelectPrevWeapon, true);

            Game.DisableControlAction(0, GameControl.VehicleHeadlight, true);
        }
        else
        {
            Game.DisableControlAction(0, GameControl.Sprint, true);
            Game.DisableControlAction(0, GameControl.Jump, true);

        }



    }
    private void UpdateSelectedIndex()
    {
        pressedDirection = false;
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 172))//UP
        {
            PressedUp();
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 173))//DOWN
        {
            PressedDown();
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 174))//LEFT
        {
            PressedLeft();
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 175))//RIGHT
        {
            PressedRight();
        }
    
        if(pressedDirection)
        {
            //EntryPoint.WriteToConsoleTestLong($"Row:{CurrentRow} Column:{CurrentColumn} Index:{CurrentIndex}");
        }
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176))//SELECT
        {
            PressedSelect();
        }
        if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(3, 177))//CLOSE
        {
            PressedBack();
        }
    }

    private void PressedUp()
    {
        //EntryPoint.WriteToConsoleTestLong("Burner Phone: Pressed UP");
        NavigateMenu(1);
        MoveFinger(1);
        int prevRow = CurrentRow;
        CurrentRow = CurrentRow - 1;
        if (CurrentRow < 0)
        {
            CurrentRow = 0;
        }
        if(!PhoneApps.Any(x=> x.Index == CurrentIndex))
        {
            CurrentRow = prevRow;
            //EntryPoint.WriteToConsoleTestLong("NO APP, RESETTING");
        }
        pressedDirection = true;
    }
    private void PressedDown()
    {
        //EntryPoint.WriteToConsoleTestLong("Burner Phone: Pressed DOWN");
        NavigateMenu(3);
        MoveFinger(2);
        int prevRow = CurrentRow;
        CurrentRow = CurrentRow + 1;
        if (CurrentRow > RowMax)
        {
            CurrentRow = RowMax;
        }
        if (!PhoneApps.Any(x => x.Index == CurrentIndex))
        {
            CurrentRow = prevRow;
            //EntryPoint.WriteToConsoleTestLong("NO APP, RESETTING");
        }
        pressedDirection = true;
    }
    private void PressedLeft()
    {
        //EntryPoint.WriteToConsoleTestLong("Burner Phone: Pressed LEFT");
        NavigateMenu(4);
        MoveFinger(4);
        int prevColumn = CurrentColumn;
        CurrentColumn = CurrentColumn - 1;
        if (CurrentColumn < 0)
        {
            CurrentColumn = ColMax;
        }
        if (!PhoneApps.Any(x => x.Index == CurrentIndex))
        {
            CurrentColumn = prevColumn;
            //EntryPoint.WriteToConsoleTestLong("NO APP, RESETTING");
        }
        pressedDirection = true;
    }
    private void PressedRight()
    {
        //EntryPoint.WriteToConsoleTestLong("Burner Phone: Pressed RIGHT");
        NavigateMenu(2);
        MoveFinger(4);
        int prevColumn = CurrentColumn;
        CurrentColumn = CurrentColumn + 1;
        if (CurrentColumn > ColMax)
        {
            CurrentColumn = 0;
        }
        if (!PhoneApps.Any(x => x.Index == CurrentIndex))
        {
            CurrentColumn = prevColumn;
            //EntryPoint.WriteToConsoleTestLong("NO APP, RESETTING");
        }
        pressedDirection = true;
    }
    private void PressedSelect()
    {
        //EntryPoint.WriteToConsoleTestLong($"Burner Phone: Pressed SELECT Row:{CurrentRow} Column:{CurrentColumn} Index:{CurrentIndex}");
        MoveFinger(5);
        PlayAcceptedSound();
        OpenApp(CurrentIndex);
    }
    private void PressedBack()
    {
        //EntryPoint.WriteToConsoleTestLong($"Burner Phone: Pressed CLOSE Row:{CurrentRow} Column:{CurrentColumn} Index:{CurrentIndex}");
        ClosePhone();
    }
    private void SetDefaultSoftKeys()
    {
        SetSoftKeyIcon((int)SoftKey.Left, SoftKeyIcon.Blank);
        SetSoftKeyColor((int)SoftKey.Left, Color.Black);

        SetSoftKeyIcon((int)SoftKey.Middle, SoftKeyIcon.Select);
        SetSoftKeyColor((int)SoftKey.Middle, Color.LightGreen);

        SetSoftKeyIcon((int)SoftKey.Right, SoftKeyIcon.Back);
        SetSoftKeyColor((int)SoftKey.Right, Color.Red);
    }
    private void UpdateHomeScreen()
    {
        SetDefaultSoftKeys();
        UpdateSelectedIndex();
    }
    private void OpenApp(int Index)
    {
        CurrentBurnerApp = PhoneApps.FirstOrDefault(x => x.Index == Index);
        CurrentBurnerApp?.Open(true);
    }
    public void NavigateMenu(int index)
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_INPUT_EVENT");
        NativeFunction.Natives.xC3D0841A0CC546A6(index);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        PlayNavigateSound();
    }
    public void MoveFinger(int index)
    {
        NativeFunction.Natives.x95C9E72F3D7DEC9B(index);
    }
    public void SetSoftKey(int buttonID, SoftKeyIcon icon, Color color)
    {
        SetSoftKeyIcon(buttonID, icon);
        SetSoftKeyColor(buttonID, color);
    }
    private void SetSoftKeyIcon(int buttonID, SoftKeyIcon icon)
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_SOFT_KEYS");
        NativeFunction.Natives.xC3D0841A0CC546A6(buttonID);
        NativeFunction.Natives.xC58424BA936EB458(true);
        NativeFunction.Natives.xC3D0841A0CC546A6((int)icon);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }
    private void SetSoftKeyColor(int buttonID, Color color)
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_SOFT_KEYS_COLOUR");
        NativeFunction.Natives.xC3D0841A0CC546A6(buttonID);
        NativeFunction.Natives.xC3D0841A0CC546A6(color.R);
        NativeFunction.Natives.xC3D0841A0CC546A6(color.G);
        NativeFunction.Natives.xC3D0841A0CC546A6(color.B);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }

    private void RequestScaleform()
    {
        globalScaleformID = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>(Player.CellPhone.PhoneOS);
        while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(globalScaleformID))
        {
            GameFiber.Yield();
        }
    }
}

