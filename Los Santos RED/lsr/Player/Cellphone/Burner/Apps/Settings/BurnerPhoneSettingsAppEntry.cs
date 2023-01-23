using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


public class BurnerPhoneSettingsAppEntry
{
   // protected bool IsDisplayingSetting;
    protected BurnerPhoneSettingsApp BurnerPhoneSettingsApp;
    protected int CurrentRow;
    protected ISettingsProvideable Settings;
    protected int TotalItems = 2;
    public BurnerPhoneSettingsAppEntry(BurnerPhoneSettingsApp burnerPhoneSettingsApp, ISettingsProvideable settings, string name, int index, int icon)
    {
        BurnerPhoneSettingsApp = burnerPhoneSettingsApp;
        Settings = settings;
        Text = name;
        Index = index;
        Icon = icon;
    }

    public string Text { get; set; } = "Setting";
    public int Index { get; set; } = 0;
    public int Icon { get; set; } = -1;

    public int SelectedItemIcon { get; set; } = 48;
    public int NonSelectedItemIcon { get; set; } = 0;

    public void DrawSettingOnMainPage()
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhoneSettingsApp.BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT");
        NativeFunction.Natives.xC3D0841A0CC546A6(22);//2
        NativeFunction.Natives.xC3D0841A0CC546A6(Index);
        NativeFunction.Natives.xC3D0841A0CC546A6(Icon);

        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Text);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }
    public virtual void Open(bool Reset)
    {
        BurnerPhoneSettingsApp.BurnerPhone.SetHeader(Text);
        if (Reset)
        {
            CurrentRow = 0;
        }
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhoneSettingsApp.BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT_EMPTY");
        NativeFunction.Natives.xC3D0841A0CC546A6(22);//2
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

        DrawSettingsItem(NonSelectedItemIcon, 0, "Setting 1");
        DrawSettingsItem(SelectedItemIcon, 1, "Setting 2");

        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhoneSettingsApp.BurnerPhone.GlobalScaleformID, "DISPLAY_VIEW");
        NativeFunction.Natives.xC3D0841A0CC546A6(22);
        NativeFunction.Natives.xC3D0841A0CC546A6(CurrentRow);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }

    public virtual void HandleInput()
    {
        HandleIndex();
        HandleSelection();
        HandleBack();
        SetSoftKeys();
    }
    protected void HandleSelection()
    {
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176))
        {
            BurnerPhoneSettingsApp.BurnerPhone.MoveFinger(5);
            BurnerPhoneSettingsApp.BurnerPhone.PlayAcceptedSound();       
        }
    }
    protected void HandleBack()
    {
        if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(3, 177))
        {
            BurnerPhoneSettingsApp.BurnerPhone.PlayBackSound();
            BurnerPhoneSettingsApp.Open(false);
        }
    }
    protected void HandleIndex()
    {
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 172))//UP
        {
            BurnerPhoneSettingsApp.BurnerPhone.MoveFinger(1);
            BurnerPhoneSettingsApp.BurnerPhone.NavigateMenu(1);
            CurrentRow = CurrentRow - 1;
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 173))//DOWN
        {
            BurnerPhoneSettingsApp.BurnerPhone.MoveFinger(2);
            BurnerPhoneSettingsApp.BurnerPhone.NavigateMenu(3);
            CurrentRow = CurrentRow + 1;
        }
        if (CurrentRow > TotalItems - 1)
        {
            CurrentRow = 0;
        }
        if (CurrentRow < 0)
        {
            CurrentRow = TotalItems - 1;
        }
    }
    protected void SetSoftKeys()
    {
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Left, SoftKeyIcon.Blank, Color.Red);
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Middle, SoftKeyIcon.Select, Color.LightGreen);
        BurnerPhoneSettingsApp.BurnerPhone.SetSoftKey((int)SoftKey.Right, SoftKeyIcon.Back, Color.Purple);
    }

    protected void DrawSettingsItem(int Icon, int Index, string Name)
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhoneSettingsApp.BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT");
        NativeFunction.Natives.xC3D0841A0CC546A6(22);//2
        NativeFunction.Natives.xC3D0841A0CC546A6(Index);
        NativeFunction.Natives.xC3D0841A0CC546A6(Icon);

        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Name);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }


}

