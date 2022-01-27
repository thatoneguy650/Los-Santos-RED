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
        //public static int _currentIndex = 40;
        private bool _shouldDraw = true;
        private int _mScriptHash;
        private bool shownOnce;
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

            // If we are in the Contacts menu
            if (NativeFunction.Natives.x2C83A9DA6BFFC4F9<int>(_mScriptHash) > 0)
            {
                IsScriptRunning = true;
                _shouldDraw = true;
                //GameFiber.Wait(50);

                if(!HasLetGoOfSelect && !Game.IsControlPressed(2, GameControl.CellphoneSelect))
                {
                    HasLetGoOfSelect = true;
                }


                if (Game.IsControlPressed(2, GameControl.CellphoneSelect) && HasLetGoOfSelect)
                    _selectedIndex = GetSelectedIndex(handle);  // We must use this function only when necessary since it contains Script.Wait(0)


                //NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "DISPLAY_VIEW");
                //NativeFunction.Natives.xC3D0841A0CC546A6(6);//2
                //NativeFunction.Natives.xC3D0841A0CC546A6(CurrentTextIndex);//2
                //NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
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
            // Browsing every added contacts
            foreach (iFruitText text in this.ToList().OrderByDescending(x => x.Index))
            {

                if (_shouldDraw)
                    text.Draw(handle);

                if (_selectedIndex != -1 && _selectedIndex == text.Index)
                {
                    // Prevent original contact to be called
                     //Tools.Scripts.TerminateScript("appTextMessage");

                    text.IsRead = true;

                    //text.Call();
                    DisplayTextUI(handle, text, "CELL_211", text.Icon.Name.SetBold(text.Bold));

                     GameFiber.Wait(50);

                    // RemoveActiveNotification();

                }
                i++;




            }



            if (IsScriptRunning && timesShow <= 5)
            {
                NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "DISPLAY_VIEW");
                NativeFunction.Natives.xC3D0841A0CC546A6(6);//2
                NativeFunction.Natives.xC3D0841A0CC546A6(0);//2
                NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
                //EntryPoint.WriteToConsole($"Text Message Selected Index {_selectedIndex} _shouldDraw {_shouldDraw}", 5);
                timesShow++;


                //shownOnce = true;
                //Tools.Scripts.TerminateScript("appTextMessage");
            }

            _shouldDraw = false;
        }
        public void DisplayTextUI(int handle, iFruitText text, string statusText = "CELL_211", string picName = "CELL_300")
        {
            IsScriptRunning = false;
            string dialText;// = Game.GetGXTEntry(statusText); // "DIALING..." translated in current game's language
            unsafe
            {
                IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, statusText);
                dialText = Marshal.PtrToStringAnsi(ptr2);
            }
            CurrentTextIndex = text.Index;

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


            GameFiber.StartNew(delegate
            {
               // GameFiber.Sleep(5000);
               //
               // NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "DISPLAY_VIEW");
                //NativeFunction.Natives.xC3D0841A0CC546A6(6);//2
               // NativeFunction.Natives.xC3D0841A0CC546A6(CurrentTextIndex);//2
               // NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

                //   // NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 177, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 181, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 174, true);


                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 172, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 173, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 174, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 175, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 176, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 177, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 178, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 179, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 180, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 181, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 182, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 183, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 184, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 185, true);
                //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 186, true);




                //    while(!NativeFunction.Natives.IS_CONTROL_JUST_PRESSED<bool>(0, (int)GameControl.CellphoneCancel))
                //    {
                //        if (Game.IsKeyDown(System.Windows.Forms.Keys.F8))
                //        {
                //            break;
                //        }
                //        GameFiber.Yield();
                //    }



                //    //while (!NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(0, (int)GameControl.CellphoneCancel) || !NativeFunction.Natives.IS_CONTROL_JUST_PRESSED<bool>(0, (int)GameControl.CellphoneScrollBackward) || !NativeFunction.Natives.IS_CONTROL_JUST_PRESSED<bool>(0, (int)GameControl.CellphoneLeft)) //while (!NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(2, (int)GameControl.CellphoneLeft)) //while (!NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(2, (int)GameControl.CellphoneCancel) || !NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(2, (int)GameControl.CellphoneScrollBackward))
                //    //{

                //    //    if(Game.IsKeyDown(System.Windows.Forms.Keys.F8))
                //    //    {
                //    //        break;
                //    //    }

                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 177, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 177, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 181, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 174, true);


                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 172, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 173, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 174, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 175, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 176, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 177, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 178, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 179, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 180, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 181, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 182, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 183, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 184, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 185, true);
                //    //    //NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 186, true);

                //    //    GameFiber.Yield();
                //    //}
                //    timesShow = 0;
                //    if (timesShow <= 5)
                //    {
                //        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "DISPLAY_VIEW");
                //        NativeFunction.Natives.xC3D0841A0CC546A6(6);//2
                //        NativeFunction.Natives.xC3D0841A0CC546A6(CurrentTextIndex);//2
                //        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
                //        timesShow++;
                //    }
                //    EntryPoint.WriteToConsole("TEXT MESSAGE WENT BACK!", 5);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 177, true);
                //   // GameFiber.Sleep(500);


                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 177, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 181, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 174, true);




                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 172, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 173, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 174, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 175, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 176, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 177, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 178, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 179, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 180, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 181, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 182, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 183, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 184, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 185, true);
                //    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 186, true);


            }, "Run Debug Logic");



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
            int data = NativeFunction.Natives.x2DE7EFA66B906036<int>(num);       //UI::_GET_SCALEFORM_MOVIE_FUNCTION_RETURN_INT


            EntryPoint.WriteToConsole($"Text Message Selected Index {data}", 5);


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