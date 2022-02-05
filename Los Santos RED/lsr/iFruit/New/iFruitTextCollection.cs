using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace iFruitAddon2
{
    public class iFruitTextCollection : List<iFruitText>
    {
        private bool _shouldDraw = true;
        private int _mScriptHash;
        private int timesShow;
        private int CurrentTextIndex;
        private bool HasLetGoOfSelect = false;

        public iFruitTextCollection()
        {
            _mScriptHash = (int)Game.GetHashKey("appTextMessage");
        }
        public bool IsDrawing => _shouldDraw;
        public bool IsScriptRunning { get; set; } = false;
        internal void Update(int handle)
        {
            int _selectedIndex = -1;
            // If we are in the Text menu
            if (NativeFunction.Natives.x2C83A9DA6BFFC4F9<int>(_mScriptHash) > 0)
            {
                IsScriptRunning = true;
                _shouldDraw = true;

                if(!HasLetGoOfSelect && !Game.IsControlPressed(2, GameControl.CellphoneSelect))
                {
                    HasLetGoOfSelect = true;
                }

                if (Game.IsControlPressed(2, GameControl.CellphoneSelect) && HasLetGoOfSelect)
                {
                    _selectedIndex = GetSelectedIndex(handle);  // We must use this function only when necessary since it contains Script.Wait(0)
                }
            }
            else
            {
                IsScriptRunning = false;
                HasLetGoOfSelect = false;
                timesShow = 0;
                _selectedIndex = -1;
            }

            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "SET_DATA_SLOT_EMPTY");
            NativeFunction.Natives.xC3D0841A0CC546A6(6);//2
            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

            int i = 0;
            // Browsing every added text
            foreach (iFruitText text in this.ToList().OrderByDescending(x=> x.TimeReceived))
            {
                if (_shouldDraw)
                {
                    text.Draw(handle);
                }
                if (_selectedIndex != -1 && _selectedIndex == text.Index)
                {
                    //Prevent original Text to be called
                    Tools.Scripts.TerminateScript("appTextMessage");
                    text.IsRead = true;
                    DisplayTextUI(handle, text, "CELL_211", text.Icon.Name.SetBold(text.Bold));
                    GameFiber.Wait(50);
                }
                i++;
            }
            if (IsScriptRunning && timesShow <= 5)
            {
                NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "DISPLAY_VIEW");
                NativeFunction.Natives.xC3D0841A0CC546A6(6);
                NativeFunction.Natives.xC3D0841A0CC546A6(0);
                NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
                timesShow++;
            }
            _shouldDraw = false;
        }
        public void DisplayTextUI(int handle, iFruitText text, string statusText = "CELL_211", string picName = "CELL_300")
        {
            IsScriptRunning = false;

            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "SET_DATA_SLOT");
            NativeFunction.Natives.xC3D0841A0CC546A6(7);
            NativeFunction.Natives.xC3D0841A0CC546A6(0);

            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text.Name);       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text.Message);       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("CHAR_BLANK_ENTRY");       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "DISPLAY_VIEW");
            NativeFunction.Natives.xC3D0841A0CC546A6(7);
            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

        }
        internal int GetSelectedIndex(int handle)
        {
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "GET_CURRENT_SELECTION");
            int num = NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE<int>();
            while (!NativeFunction.Natives.x768FF8961BA904D6<bool>(num))         //UI::_GET_SCALEFORM_MOVIE_FUNCTION_RETURN_BOOL
                GameFiber.Wait(0);
            int data = NativeFunction.Natives.x2DE7EFA66B906036<int>(num);       //UI::_GET_SCALEFORM_MOVIE_FUNCTION_RETURN_INT


            EntryPoint.WriteToConsole($"Text Message Selected Index {data}", 5);


            return data;
        }
        internal void RemoveActiveNotification()
        {
            NativeFunction.Natives.BEGIN_TEXT_COMMAND_THEFEED_POST("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("temp");
            int temp = NativeFunction.Natives.END_TEXT_COMMAND_THEFEED_POST_TICKER<int>(false, 1);
            NativeFunction.Natives.THEFEED_REMOVE_ITEM(temp);
            NativeFunction.Natives.THEFEED_REMOVE_ITEM(temp - 1);
        }
    }
}