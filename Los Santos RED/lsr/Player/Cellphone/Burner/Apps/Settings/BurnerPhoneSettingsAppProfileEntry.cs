using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class BurnerPhoneSettingsProfileEntry : BurnerPhoneSettingsAppEntry
{
    private List<BurnerPhoneSettingTracker> BurnerPhoneSettingTrackers;
    private List<Tuple<int, string>> Backgrounds = new List<Tuple<int, string>>();
    public BurnerPhoneSettingsProfileEntry(BurnerPhoneSettingsApp burnerPhoneSettingsApp, ISettingsProvideable settings, string name, int index, int icon) : base(burnerPhoneSettingsApp, settings, name, index, icon)
    {

    }
    public override void Open(bool Reset)
    {
        BurnerPhoneSettingsApp.BurnerPhone.SetHeader(Text);
        if (Reset)
        {
            CurrentRow = 0;
        }
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhoneSettingsApp.BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT_EMPTY");
        NativeFunction.Natives.xC3D0841A0CC546A6(22);//2
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

        DisplayVolume();

        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhoneSettingsApp.BurnerPhone.GlobalScaleformID, "DISPLAY_VIEW");
        NativeFunction.Natives.xC3D0841A0CC546A6(22);
        NativeFunction.Natives.xC3D0841A0CC546A6(CurrentRow);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }

    public override void HandleInput()
    {
        HandleIndex();
        HandleThemeSelection();
        HandleBack();
        SetRingtoneSoftKeys();
    }
    private void DisplayVolume()
    {
        BurnerPhoneSettingTrackers = new List<BurnerPhoneSettingTracker>();

        BurnerPhoneSettingTracker NormalMode = new BurnerPhoneSettingTracker(0, "Normal Mode") { IntegerValue = 0, IsSelected = !BurnerPhoneSettingsApp.Player.CellPhone.SleepMode };
        BurnerPhoneSettingTracker SleepMode = new BurnerPhoneSettingTracker(1, "Sleep Mode") { IntegerValue = 1, IsSelected = BurnerPhoneSettingsApp.Player.CellPhone.SleepMode };
        BurnerPhoneSettingTrackers.Add(NormalMode);
        BurnerPhoneSettingTrackers.Add(SleepMode);
        DrawSettingsItem(NormalMode.IsSelected ? (int)BurnerPhoneSettingsIcon.Ticked : (int)BurnerPhoneSettingsIcon.Profile, NormalMode.Index, NormalMode.Name);
        DrawSettingsItem(SleepMode.IsSelected ? (int)BurnerPhoneSettingsIcon.Ticked : (int)BurnerPhoneSettingsIcon.SleepMode, SleepMode.Index, SleepMode.Name);
        TotalItems = 2;
    }
    private void HandleThemeSelection()
    {
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176))//SELECT
        {
            BurnerPhoneSettingsApp.BurnerPhone.MoveFinger(5);
            BurnerPhoneSettingsApp.BurnerPhone.PlayAcceptedSound();
            BurnerPhoneSettingTracker selectedItem = BurnerPhoneSettingTrackers.FirstOrDefault(x => x.Index == CurrentRow);
            if (selectedItem == null)
            {
                return;
            }
            BurnerPhoneSettingTracker oldSelected = BurnerPhoneSettingTrackers.FirstOrDefault(x => x.IsSelected);
            if (oldSelected != null)
            {
                oldSelected.IsSelected = false;
            }
            selectedItem.IsSelected = true;
            BurnerPhoneSettingsApp.Player.CellPhone.SleepMode = selectedItem.IntegerValue == 1 ? true : false;
            BurnerPhoneSettingsApp.BurnerPhone.UpdateThemeItems();
            Open(false);
        }
    }
    protected void SetRingtoneSoftKeys()
    {
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Left, SoftKeyIcon.Blank, Color.Red);
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Middle, SoftKeyIcon.Select, Color.LightGreen);
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Right, SoftKeyIcon.Back, Color.Red);
    }
}

