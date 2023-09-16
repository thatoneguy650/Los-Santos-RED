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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

public class BurnerPhoneSettingsBackgroundEntry : BurnerPhoneSettingsAppEntry
{
    private List<BurnerPhoneSettingTracker> BurnerPhoneSettingTrackers;
    private List<CellphoneIDLookup> Backgrounds = new List<CellphoneIDLookup>();
    public BurnerPhoneSettingsBackgroundEntry(BurnerPhoneSettingsApp burnerPhoneSettingsApp, ISettingsProvideable settings, string name, int index, int icon) : base(burnerPhoneSettingsApp, settings, name, index, icon)
    {
        SelectedItemIcon = (int)BurnerPhoneSettingsIcon.Ticked;// 39;
        NonSelectedItemIcon = (int)BurnerPhoneSettingsIcon.Edit; //0;
    }

    private void GetBackgrounds()
    {
        Backgrounds.Clear();
        if (BurnerPhoneSettingsApp.Player.CellPhone.CurrentCellphoneData == null)
        {
            Backgrounds.Add(new CellphoneIDLookup(0, "Default"));
            Backgrounds.Add(new CellphoneIDLookup(10, "Blue Angles"));
            Backgrounds.Add(new CellphoneIDLookup(11, "Blue Shards"));
            Backgrounds.Add(new CellphoneIDLookup(12, "Blue Circles"));
            Backgrounds.Add(new CellphoneIDLookup(13, "Diamonds"));
            Backgrounds.Add(new CellphoneIDLookup(14, "Green Glow"));
            Backgrounds.Add(new CellphoneIDLookup(9, "Green Shards"));
            Backgrounds.Add(new CellphoneIDLookup(5, "Green Squares"));

            Backgrounds.Add(new CellphoneIDLookup(8, "Green Triangles"));
            Backgrounds.Add(new CellphoneIDLookup(15, "Orange 8-Bit"));
            Backgrounds.Add(new CellphoneIDLookup(7, "Orange Halftone"));
            Backgrounds.Add(new CellphoneIDLookup(6, "Orange Herringbone"));
            Backgrounds.Add(new CellphoneIDLookup(16, "Orange Triangles"));
            Backgrounds.Add(new CellphoneIDLookup(4, "Purple Glow"));
            Backgrounds.Add(new CellphoneIDLookup(17, "Purple Tartan"));
            //Backgrounds.Add(new CellphoneIDLookup(15, "Background 15"));
            // Backgrounds.Add(new CellphoneIDLookup(16, "Background 16"));
            //Backgrounds.Add(new CellphoneIDLookup(17, "Background 17"));
        }
        else
        {
            Backgrounds.AddRange(BurnerPhoneSettingsApp.Player.CellPhone.CurrentCellphoneData.Backgrounds);
        }
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

        DisplayBackgrounds();

        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhoneSettingsApp.BurnerPhone.GlobalScaleformID, "DISPLAY_VIEW");
        NativeFunction.Natives.xC3D0841A0CC546A6(22);
        NativeFunction.Natives.xC3D0841A0CC546A6(CurrentRow);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }

    public override void HandleInput()
    {
        HandleIndex();
        HandleBackgroundSelection();
        HandleBack();
        SetRingtoneSoftKeys();
    }
    private void DisplayBackgrounds()
    {
        BurnerPhoneSettingTrackers = new List<BurnerPhoneSettingTracker>();
        GetBackgrounds();
        int Index = 0;
        foreach(CellphoneIDLookup thingo in Backgrounds.OrderBy(x=> x.ID))
        {
            BurnerPhoneSettingTracker burnerPhoneSettingTracker = new BurnerPhoneSettingTracker(Index, thingo.Name) { IntegerValue = thingo.ID };
            if (BurnerPhoneSettingsApp.Player.CellPhone.Background == thingo.ID)
            {
                burnerPhoneSettingTracker.IsSelected = true;
            }
            BurnerPhoneSettingTrackers.Add(burnerPhoneSettingTracker);
            DrawSettingsItem(burnerPhoneSettingTracker.IsSelected ? SelectedItemIcon : NonSelectedItemIcon, burnerPhoneSettingTracker.Index, burnerPhoneSettingTracker.Name);
            Index++;
        }
        TotalItems = Backgrounds.Count();
    }
    private void HandleBackgroundSelection()
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
            BurnerPhoneSettingsApp.Player.CellPhone.CustomBackground = selectedItem.IntegerValue;
            //EntryPoint.WriteToConsoleTestLong($"SETTING BACKGROUND TO {selectedItem.IntegerValue} {BurnerPhoneSettingsApp.Player.CellPhone.CustomBackground}");

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

