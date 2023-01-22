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

public class BurnerPhoneSettingsRingtoneEntry : BurnerPhoneSettingsAppEntry
{
    private List<BurnerPhoneSettingTracker> BurnerPhoneSettingTrackers;

    public BurnerPhoneSettingsRingtoneEntry(BurnerPhoneSettingsApp burnerPhoneSettingsApp, ISettingsProvideable settings, string name, int index, int icon) : base(burnerPhoneSettingsApp, settings, name, index, icon)
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

        DisplayTones();

        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhoneSettingsApp.BurnerPhone.GlobalScaleformID, "DISPLAY_VIEW");
        NativeFunction.Natives.xC3D0841A0CC546A6(22);
        NativeFunction.Natives.xC3D0841A0CC546A6(CurrentRow);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }

    public override void HandleInput()
    {
        HandleIndex();
        HandleRingtoneSelection();
        HandleBack();
        SetRingtoneSoftKeys();
    }
    private void DisplayTones()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED\\audio\\ringtones");
        List<FileInfo> RingtoneFiles = LSRDirectory.GetFiles("*.*").OrderBy(x => x.Name).ToList();
        BurnerPhoneSettingTrackers = new List<BurnerPhoneSettingTracker>();
        int Index = 0;
        foreach(FileInfo fileInfo in RingtoneFiles)
        {
            BurnerPhoneSettingTracker burnerPhoneSettingTracker = new BurnerPhoneSettingTracker(Index, fileInfo.Name);
            if(Settings.SettingsManager.CellphoneSettings.CustomRingtoneName == fileInfo.Name)
            {
                burnerPhoneSettingTracker.IsSelected = true;
            }
            BurnerPhoneSettingTrackers.Add(burnerPhoneSettingTracker);
            DrawSettingsItem(burnerPhoneSettingTracker.IsSelected ? 48 : 20,Index,fileInfo.Name);
            Index++;
        }
        TotalItems = Index;
    }
    private void HandleRingtoneSelection()
    {
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176))//SELECT
        {
            BurnerPhoneSettingsApp.BurnerPhone.MoveFinger(5);
            BurnerPhoneSettingsApp.BurnerPhone.PlayAcceptedSound();
            BurnerPhoneSettingTracker selectedItem = BurnerPhoneSettingTrackers.FirstOrDefault(x => x.Index == CurrentRow);
            if(selectedItem == null)
            {
                return;
            }
            BurnerPhoneSettingTracker oldSelected = BurnerPhoneSettingTrackers.FirstOrDefault(x => x.IsSelected);
            if(oldSelected != null)
            {
                oldSelected.IsSelected = false;
            }
            selectedItem.IsSelected = true;
            Settings.SettingsManager.CellphoneSettings.CustomRingtoneName = selectedItem.Name;
            BurnerPhoneSettingsApp.BurnerPhone.PlayRingtone();
            Open(false);
        }
    }
    protected void SetRingtoneSoftKeys()
    {
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Left, SoftKeyIcon.Blank, Color.Red);
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Middle, SoftKeyIcon.Open, Color.LightGreen);
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Right, SoftKeyIcon.Back, Color.Red);
    }
}

