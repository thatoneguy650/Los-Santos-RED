using LosSantosRED.lsr.Interface;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class BurnerPhoneMessagesApp : BurnerPhoneApp
{
    private bool IsDisplayingTextMessage;
    private int CurrentRow;
    private int CurrentIndex;

    public BurnerPhoneMessagesApp(BurnerPhone burnerPhone, ICellPhoneable player, ITimeReportable time, ISettingsProvideable settings, int index) : base(burnerPhone, player, time, settings, index, "Messages", 2)
    {
    }
    public override void Open()
    {
        EntryPoint.WriteToConsole("BurnerPhoneMessagesApp OPEN");
        IsDisplayingTextMessage = false;
        CurrentRow = 0;
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT_EMPTY");
        NativeFunction.Natives.xC3D0841A0CC546A6(6);//2
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        foreach (PhoneText text in Player.CellPhone.TextList.OrderBy(x => x.Index))
        {
            EntryPoint.WriteToConsole($"Adding Message {text.ContactName} {text.Index}");
            DrawMessage(text);
        }
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "DISPLAY_VIEW");
        NativeFunction.Natives.xC3D0841A0CC546A6(6);
        NativeFunction.Natives.xC3D0841A0CC546A6(CurrentRow);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }
    public override void HandleInput()
    {
        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 172) && !IsDisplayingTextMessage)//UP
        {
            EntryPoint.WriteToConsole("Burner Phone: Pressed UP APP MESSAGES");
            BurnerPhone.MoveFinger(1);
            BurnerPhone.NavigateMenu(1);
            //CurrentIndex = GetSelectedIndex();
            CurrentRow = CurrentRow - 1;
        }
        else if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 173) && !IsDisplayingTextMessage)//DOWN
        {
            EntryPoint.WriteToConsole("Burner Phone: Pressed DOWN APP MESSAGES");
            BurnerPhone.MoveFinger(2);
            BurnerPhone.NavigateMenu(3);
            //CurrentIndex = GetSelectedIndex();
            CurrentRow = CurrentRow + 1;
        }
        int TotalMessages = Player.CellPhone.TextList.Count();
        if (TotalMessages > 0)
        {
            if (CurrentRow > TotalMessages - 1)
            {
                CurrentRow = 0;
            }
        }
        if (CurrentRow < 0)
        {
            CurrentRow = 0;
        }

        if (NativeFunction.Natives.x91AEF906BCA88877<bool>(3, 176) && !IsDisplayingTextMessage)//SELECT
        {
            EntryPoint.WriteToConsole($"Burner Phone: Pressed MESSAGES CurrentIndex {CurrentIndex} OpenApp {CurrentIndex + 1}");
            BurnerPhone.MoveFinger(5);
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Accept", "Phone_SoundSet_Michael", 0);
            IsDisplayingTextMessage = true;
            DisplayTextUI(Player.CellPhone.TextList.Where(x => x.Index == CurrentRow).FirstOrDefault());
        }
        if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(3, 177))//CLOSE
        {
            if (IsDisplayingTextMessage)
            {
                EntryPoint.WriteToConsole("Burner Phone: Pressed CLOSE APP READING MESSAGES");
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Back", "Phone_SoundSet_Michael", 0);
                IsDisplayingTextMessage = false;
                Open();
            }
            else
            {
                EntryPoint.WriteToConsole("Burner Phone: Pressed CLOSE APP MESSAGES");
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Back", "Phone_SoundSet_Michael", 0);
                NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "DISPLAY_VIEW");
                NativeFunction.Natives.xC3D0841A0CC546A6(1);
                NativeFunction.Natives.xC3D0841A0CC546A6(0);
                NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
                GameFiber.Sleep(200);
                BurnerPhone.ReturnHome();
            }
        }
        if (!IsDisplayingTextMessage)
        {
            BurnerPhone.SetSoftKeyIcon((int)SoftKey.Left, SoftKeyIcon.Select);
            BurnerPhone.SetSoftKeyColor((int)SoftKey.Left, Color.LightBlue);

            BurnerPhone.SetSoftKeyIcon((int)SoftKey.Middle, SoftKeyIcon.Blank);
            BurnerPhone.SetSoftKeyColor((int)SoftKey.Middle, Color.Black);

            BurnerPhone.SetSoftKeyIcon((int)SoftKey.Right, SoftKeyIcon.Back);
            BurnerPhone.SetSoftKeyColor((int)SoftKey.Right, Color.Purple);
        }
        else
        {
            BurnerPhone.SetSoftKeyIcon((int)SoftKey.Left, SoftKeyIcon.Delete);
            BurnerPhone.SetSoftKeyColor((int)SoftKey.Left, Color.Red);

            BurnerPhone.SetSoftKeyIcon((int)SoftKey.Middle, SoftKeyIcon.Call);
            BurnerPhone.SetSoftKeyColor((int)SoftKey.Middle, Color.LightBlue);

            BurnerPhone.SetSoftKeyIcon((int)SoftKey.Right, SoftKeyIcon.Back);
            BurnerPhone.SetSoftKeyColor((int)SoftKey.Right, Color.Purple);
        }
    }
    private void DrawMessage(PhoneText text)
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT");
        NativeFunction.Natives.xC3D0841A0CC546A6(6);//2
        NativeFunction.Natives.xC3D0841A0CC546A6(text.Index);
        NativeFunction.Natives.xC3D0841A0CC546A6(text.HourSent);
        NativeFunction.Natives.xC3D0841A0CC546A6(text.MinuteSent);
        if (text.IsRead)
        {
            NativeFunction.Natives.xC3D0841A0CC546A6(34);
        }
        else
        {
            NativeFunction.Natives.xC3D0841A0CC546A6(33);
        }
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text.ContactName);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text.Message);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }
    public void DisplayTextUI(PhoneText text)
    {
        if (text != null)
        {
            text.IsRead = true;
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT");
            NativeFunction.Natives.xC3D0841A0CC546A6(7);
            NativeFunction.Natives.xC3D0841A0CC546A6(0);

            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text.ContactName);       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text.Message);       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("CHAR_BLANK_ENTRY");       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "DISPLAY_VIEW");
            NativeFunction.Natives.xC3D0841A0CC546A6(7);
            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

            SetTextApp();
        }
    }
    private void SetTextApp()
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT");
        NativeFunction.Natives.xC3D0841A0CC546A6(1);
        NativeFunction.Natives.xC3D0841A0CC546A6(0);
        NativeFunction.Natives.xC3D0841A0CC546A6(2);
        NativeFunction.Natives.xC3D0841A0CC546A6(Player.CellPhone.TextList.Where(x => !x.IsRead).Count());
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("Texts");
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
        NativeFunction.Natives.xC3D0841A0CC546A6(100);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }
}

