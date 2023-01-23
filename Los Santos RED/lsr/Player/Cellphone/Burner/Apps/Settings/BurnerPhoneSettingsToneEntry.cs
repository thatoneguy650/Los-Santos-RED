using LosSantosRED.lsr.Interface;
using Rage.Native;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

public class BurnerPhoneSettingsToneEntry : BurnerPhoneSettingsAppEntry
{
    private bool IsRingtone;
    private List<BurnerPhoneSettingTracker> BurnerPhoneSettingTrackers;

    private string DirectoryName => "Plugins\\LosSantosRED\\audio\\tones";

    public BurnerPhoneSettingsToneEntry(BurnerPhoneSettingsApp burnerPhoneSettingsApp, ISettingsProvideable settings, string name, int index, int icon, bool isRingtone) : base(burnerPhoneSettingsApp, settings, name, index, icon)
    {
        IsRingtone = isRingtone;
        SelectedItemIcon = 48;
        NonSelectedItemIcon = 20;

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
        HandleToneSelection();
        HandleBack();
        SetToneSoftKeys();
    }
    private void DisplayTones()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo(DirectoryName);
        List<FileInfo> ToneFiles = LSRDirectory.GetFiles("*.*").OrderBy(x => x.Name).ToList();
        BurnerPhoneSettingTrackers = new List<BurnerPhoneSettingTracker>();
        int Index = 0;
        foreach(FileInfo fileInfo in ToneFiles)
        {
            BurnerPhoneSettingTracker burnerPhoneSettingTracker = new BurnerPhoneSettingTracker(Index, fileInfo.Name);

            if(IsRingtone)
            {
                if (BurnerPhoneSettingsApp.Player.CellPhone.CustomRingtone == fileInfo.Name)// if(Settings.SettingsManager.CellphoneSettings.DefaultCustomRingtoneName == fileInfo.Name)
                {
                    burnerPhoneSettingTracker.IsSelected = true;
                }
            }
            else
            {
                if (BurnerPhoneSettingsApp.Player.CellPhone.CustomTextTone == fileInfo.Name)// if(Settings.SettingsManager.CellphoneSettings.DefaultCustomRingtoneName == fileInfo.Name)
                {
                    burnerPhoneSettingTracker.IsSelected = true;
                }
            }



            BurnerPhoneSettingTrackers.Add(burnerPhoneSettingTracker);
            DrawSettingsItem(burnerPhoneSettingTracker.IsSelected ? SelectedItemIcon : NonSelectedItemIcon, Index,fileInfo.Name);
            Index++;
        }
        TotalItems = Index;
    }
    private void HandleToneSelection()
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
           

            if (IsRingtone)
            {
                BurnerPhoneSettingsApp.Player.CellPhone.CustomRingtone = selectedItem.Name;
                BurnerPhoneSettingsApp.Player.CellPhone.PreviewRingtoneSound();
            }
            else
            {
                BurnerPhoneSettingsApp.Player.CellPhone.CustomTextTone = selectedItem.Name;
                BurnerPhoneSettingsApp.Player.CellPhone.PreviewTextSound();
            }
            Open(false);
        }
    }
    protected void SetToneSoftKeys()
    {
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Left, SoftKeyIcon.Blank, Color.Red);
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Middle, SoftKeyIcon.Select, Color.LightGreen);
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Right, SoftKeyIcon.Back, Color.Red);
    }
}

