using LosSantosRED.lsr.Interface;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class BurnerPhoneSettingsApp : BurnerPhoneApp
{
    private int CurrentRow;
    private int CurrentIndex;
    private BurnerPhoneSettingsToneEntry RingtoneSetting;
    private BurnerPhoneSettingsToneEntry TexttoneSetting;
    private BurnerPhoneSettingsAppEntry ProfileSetting;
    private BurnerPhoneSettingsVolumeEntry VolumeSetting;
    private BurnerPhoneSettingsThemeEntry ThemeSetting;
    private BurnerPhoneSettingsBackgroundEntry BackgroundSetting;
    private List<BurnerPhoneSettingsAppEntry> SettingsEntries;
    private BurnerPhoneSettingsAppEntry CurrentActiveSetting;

    private enum SettingsIcon
    {
        Attachment = 10,
        ToDoList = 12,
        Ringtone = 18,
        TextTone = 19,
        VibrateOn = 20,
        VibrateOff = 21,
        Volume = 22,
        Edit = 23,
        Profile = 25,
        SleepMode = 26,
        Checklist = 39,
        Ticked = 48,
        Silent = 51
    }
    public BurnerPhoneSettingsApp(BurnerPhone burnerPhone, ICellPhoneable player, ITimeReportable time, ISettingsProvideable settings, int index) : base(burnerPhone, player, time, settings, index, "Settings", 24)
    {
        RingtoneSetting = new BurnerPhoneSettingsToneEntry(this, settings, "Ringtone", 0, 18, true);
        TexttoneSetting = new BurnerPhoneSettingsToneEntry(this, settings, "TextTone", 1, 19, false);
        VolumeSetting = new BurnerPhoneSettingsVolumeEntry(this, settings, "Volume", 2, 22);
        ProfileSetting = new BurnerPhoneSettingsProfileEntry(this, settings, "Profile", 3, 25);   
        ThemeSetting = new BurnerPhoneSettingsThemeEntry(this, settings, "Theme", 4, 23);
        BackgroundSetting = new BurnerPhoneSettingsBackgroundEntry(this, settings, "Background", 5, 23);
        SettingsEntries = new List<BurnerPhoneSettingsAppEntry>() { RingtoneSetting, TexttoneSetting, ProfileSetting, VolumeSetting, ThemeSetting, BackgroundSetting };
    }
    public override void Open(bool Reset)
    {
        //EntryPoint.WriteToConsoleTestLong("BurnerPhoneSettingsApp OPEN");
        if (Reset)
        {
            CurrentRow = 0;
        }
        BurnerPhone.SetHeader(Name);
        CurrentActiveSetting = null;
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT_EMPTY");
        NativeFunction.Natives.xC3D0841A0CC546A6(22);//2
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        foreach (BurnerPhoneSettingsAppEntry burnerPhoneSettingsAppEntry in SettingsEntries.OrderBy(x => x.Index))
        {
            burnerPhoneSettingsAppEntry.DrawSettingOnMainPage();
        }
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "DISPLAY_VIEW");
        NativeFunction.Natives.xC3D0841A0CC546A6(22);
        NativeFunction.Natives.xC3D0841A0CC546A6(CurrentRow);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }
    public override void HandleInput()
    {
        if(CurrentActiveSetting != null)
        {
            CurrentActiveSetting.HandleInput();
            return;
        }
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
        if(CurrentRow > SettingsEntries.Count() - 1)
        {
            CurrentRow = 0;
        }
        if(CurrentRow < 0)
        {
            CurrentRow = SettingsEntries.Count() - 1;
        }
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176))//SELECT
        {
            BurnerPhone.MoveFinger(5);
            BurnerPhone.PlayAcceptedSound();
            CurrentActiveSetting = SettingsEntries.FirstOrDefault(x => x.Index == CurrentRow);
            CurrentActiveSetting?.Open(true);
        }
        if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(3, 177))//CLOSE
        {
            BurnerPhone.PlayBackSound();
            BurnerPhone.ReturnHome(Index);
        }
        BurnerPhone.SetSoftKey((int)SoftKey.Left, SoftKeyIcon.Blank, Color.Red);
        BurnerPhone.SetSoftKey((int)SoftKey.Middle, SoftKeyIcon.Select, Color.Green);
        BurnerPhone.SetSoftKey((int)SoftKey.Right, SoftKeyIcon.Back, Color.Red);
 
    }


}

