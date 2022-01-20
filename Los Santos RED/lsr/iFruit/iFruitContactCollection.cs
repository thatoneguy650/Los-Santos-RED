using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace iFruitAddon2
{
    public class iFruitContactCollection : List<iFruitContact>
    {
        public static int _currentIndex = 40;
        private bool _shouldDraw = true;
        private int _mScriptHash;

        public iFruitContactCollection()
        {
            _mScriptHash = (int)Game.GetHashKey("appcontacts");
        }


        internal void Update(int handle)
        {
            int _selectedIndex = 0;

            // If we are in the Contacts menu
            if (NativeFunction.Natives.x2C83A9DA6BFFC4F9<int>(_mScriptHash) > 0)
            {
                _shouldDraw = true;

                if (Game.IsControlPressed(2, GameControl.CellphoneSelect))
                    _selectedIndex = GetSelectedIndex(handle);  // We must use this function only when necessary since it contains Script.Wait(0)
            }
            else
                _selectedIndex = -1;

            // Browsing every added contacts
            foreach (iFruitContact contact in this)
            {
                contact.Update(); // Update sounds or Answer call when _callTimer has ended.

                if (_shouldDraw)
                    contact.Draw(handle);

                if (_selectedIndex != -1 && _selectedIndex == contact.Index)
                {
                    // Prevent original contact to be called
                    Tools.Scripts.TerminateScript("appcontacts");

                    contact.Call();
                    DisplayCallUI(handle, contact.Name, "CELL_211", contact.Icon.Name.SetBold(contact.Bold));

                    GameFiber.Wait(10);

                    RemoveActiveNotification();

                }

            }
            _shouldDraw = false;
        }


        /// <summary>
        /// Display the current call on the phone.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="contactName"></param>
        /// <param name="statusText">CELL_211 = "DIALING..." / CELL_219 = "CONNECTED"</param>
        /// <param name="picName"></param>
        public static void DisplayCallUI(int handle, string contactName, string statusText = "CELL_211", string picName = "CELL_300")
        {
            string dialText;// = Game.GetGXTEntry(statusText); // "DIALING..." translated in current game's language
            unsafe
            {
                IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, statusText);
                dialText = Marshal.PtrToStringAnsi(ptr2);
            }


            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "SET_DATA_SLOT");
            NativeFunction.Natives.xC3D0841A0CC546A6(4);
            NativeFunction.Natives.xC3D0841A0CC546A6(0);
            NativeFunction.Natives.xC3D0841A0CC546A6(3);

            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.x761B77454205A61D(contactName, -1);       //UI::_ADD_TEXT_COMPONENT_APP_TITLE
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("CELL_2000");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(picName);
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.x761B77454205A61D(dialText, -1);      //UI::_ADD_TEXT_COMPONENT_APP_TITLE
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "DISPLAY_VIEW");
            NativeFunction.Natives.xC3D0841A0CC546A6(4);
            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        }

        /// <summary>
        /// Get the index of the current highlighted contact.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal int GetSelectedIndex(int handle)
        {
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "GET_CURRENT_SELECTION");
            int num = NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE<int>();
            while (!NativeFunction.Natives.x768FF8961BA904D6<bool>(num))         //UI::_GET_SCALEFORM_MOVIE_FUNCTION_RETURN_BOOL
                GameFiber.Wait(0);
            int data = NativeFunction.Natives.x2DE7EFA66B906036< int>( num);       //UI::_GET_SCALEFORM_MOVIE_FUNCTION_RETURN_INT
            return data;
        }

        /// <summary>
        /// Remove the current notification.
        /// Useful to remove "The selected contact is no longer available" when you try to call a contact that shouldn't exist (ie: contacts added by iFruitAddon).
        /// </summary>
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