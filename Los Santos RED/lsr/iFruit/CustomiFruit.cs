using Rage;
using Rage.Native;
using System.Drawing;
using System.Linq;

namespace iFruitAddon2
{
    //public delegate void ContactSelectedEvent(iFruitContactCollection sender, iFruitContact selectedItem);
    public delegate void ContactAnsweredEvent(iFruitContact contact);

    public class CustomiFruit
    {
        private static CustomiFruit _instance;
        private bool _shouldDraw = true;
        private PhoneImage _wallpaper;
        private iFruitContactCollection _contacts;


        private bool IsScriptHashRunning = false;


        private iFruitTextCollection _texts;

        private int _mScriptHash;
        private int _timerClose = -1;


        public string DebugString { get; set; } = "";

        /// <summary>
        /// Left Button Color
        /// </summary>
        public Color LeftButtonColor { get; set; } = Color.Empty;

        /// <summary>
        /// Center Button Color
        /// </summary>
        public Color CenterButtonColor { get; set; } = Color.Empty;

        /// <summary>
        /// Right Button Color
        /// </summary>
        public Color RightButtonColor { get; set; } = Color.Empty;

        /// <summary>
        /// Left Button Icon
        /// </summary>
        public SoftKeyIcon LeftButtonIcon { get; set; } = SoftKeyIcon.Blank;

        /// <summary>
        /// Center Button Icon
        /// </summary>
        public SoftKeyIcon CenterButtonIcon { get; set; } = SoftKeyIcon.Blank;

        /// <summary>
        /// Right Button Icon
        /// </summary>
        public SoftKeyIcon RightButtonIcon { get; set; } = SoftKeyIcon.Blank;

        /// <summary>
        /// List of custom contacts in the phone
        /// </summary>
        public iFruitContactCollection Contacts
        {
            get { return _contacts; }
            set { _contacts = value; }
        }
        public iFruitTextCollection Texts
        {
            get { return _texts; }
            set { _texts = value; }
        }

        public CustomiFruit() : this(new iFruitContactCollection(), new iFruitTextCollection())
        { }

        /// <summary>
        /// Initialize the class.
        /// </summary>
        /// <param name="contacts"></param>
        public CustomiFruit(iFruitContactCollection contacts, iFruitTextCollection texts)
        {
            _instance = this;
            _contacts = contacts;
            _texts = texts;
            _mScriptHash = (int)Game.GetHashKey("cellphone_flashhand");
        }

        /// <summary>
        /// Handle of the current scaleform.
        /// </summary>
        public static CustomiFruit GetCurrentInstance() { return _instance; }
        public int Handle
        {
            get
            {
                int h = 0;
                switch ((uint)Game.LocalPlayer.Character.Model.Hash)
                {
                    case (uint)225514697:
                        h = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("cellphone_ifruit");
                        while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>( h))
                            GameFiber.Yield();
                        return h;
                    case (uint)2602752943:
                        h = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("cellphone_badger");
                        while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>( h))
                            GameFiber.Yield();
                        return h;
                    case (uint)2608926626:
                        h = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("cellphone_facade");
                        while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>( h))
                            GameFiber.Yield();
                        return h;
                    default:
                        h = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("cellphone_ifruit");
                        while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>( h))
                            GameFiber.Yield();
                        return h;
                }
            }
        }

        /// <summary>
        /// Set text displayed at the top of the phone interface. Must be called every update!
        /// </summary>
        /// <param name="text"></param>
        public void SetTextHeader(string text)
        {
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD( Handle, "SET_HEADER");
            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING( "STRING");
            NativeFunction.Natives.x761B77454205A61D( text, -1);
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        }

        /// <summary>
        /// Set icon of the soft key buttons directly.
        /// </summary>
        /// <param name="buttonID">The button index</param>
        /// <param name="icon">Supplied icon</param>
        public void SetSoftKeyIcon(int buttonID, SoftKeyIcon icon)
        {
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(Handle, "SET_SOFT_KEYS");
            NativeFunction.Natives.xC3D0841A0CC546A6(buttonID);
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD_PARAMETER_BOOL(true);
            NativeFunction.Natives.xC3D0841A0CC546A6((int)icon);
            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        }

        /// <summary>
        /// Set the color of the soft key buttons directly.
        /// </summary>
        /// <param name="buttonID">The button index</param>
        /// <param name="color">Supplied color</param>
        public void SetSoftKeyColor(int buttonID, Color color)
        {
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(Handle, "SET_SOFT_KEYS_COLOUR");
            NativeFunction.Natives.xC3D0841A0CC546A6(buttonID);
            NativeFunction.Natives.xC3D0841A0CC546A6(color.R);
            NativeFunction.Natives.xC3D0841A0CC546A6(color.G);
            NativeFunction.Natives.xC3D0841A0CC546A6(color.B);
            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        }

        internal void SetWallpaperTXD(string textureDict)
        {
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(Handle, "SET_BACKGROUND_CREW_IMAGE");
            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("CELL_2000");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(textureDict);
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        }

        /// <summary>
        /// Set the wallpaper of the phone.
        /// </summary>
        /// <param name="wallpaper"></param>
        public void SetWallpaper(Wallpaper wallpaper)
        {
            _wallpaper = wallpaper;
        }
        public void SetWallpaper(ContactIcon icon)
        {
            _wallpaper = icon;
        }
        public void SetWallpaper(string textureDict)
        {
            _wallpaper = new Wallpaper(textureDict);
        }

        public void Update()
        {



            if (NativeFunction.Natives.x2C83A9DA6BFFC4F9<int>(_mScriptHash) > 0)
            {
                IsScriptHashRunning = true;
                if (_shouldDraw)
                {
                    //Script.Wait(0);

                    if (LeftButtonColor != Color.Empty)
                        SetSoftKeyColor(1, LeftButtonColor);
                    if (CenterButtonColor != Color.Empty)
                        SetSoftKeyColor(2, CenterButtonColor);
                    if (RightButtonColor != Color.Empty)
                        SetSoftKeyColor(3, RightButtonColor);

                    //Script.Wait(0);

                    if (LeftButtonIcon != SoftKeyIcon.Blank)
                        SetSoftKeyIcon(1, LeftButtonIcon);
                    if (CenterButtonIcon != SoftKeyIcon.Blank)
                        SetSoftKeyIcon(2, CenterButtonIcon);
                    if (RightButtonIcon != SoftKeyIcon.Blank)
                        SetSoftKeyIcon(3, RightButtonIcon);

                    if (_wallpaper != null)
                        SetWallpaperTXD(_wallpaper.Name);

                    

                //    SetUnread();



                    _shouldDraw = !_shouldDraw;
                }




            }
            else
            {
                IsScriptHashRunning = false;
                _shouldDraw = true;
            }


            if (_timerClose != -1)
            {
                if (_timerClose <= Game.GameTime)
                {
                    Close();
                    _timerClose = -1;
                }
            }


            _contacts.Update(Handle);


            if (IsScriptHashRunning && !_contacts.IsScriptRunning && !_texts.IsScriptRunning)
            {
                SetUnread();
            }
            _texts.Update(Handle);

            //if(_texts.IsDrawing)
            //{
            //    LeftButtonIcon = SoftKeyIcon.Back;
            //    RightButtonIcon = SoftKeyIcon.Delete;
            //    RightButtonColor = Color.Red;
            //    LeftButtonColor = Color.White;
            //}
            //else
            //{
            //    LeftButtonIcon = SoftKeyIcon.Blank;
            //    RightButtonIcon = SoftKeyIcon.Blank;
            //    RightButtonColor = Color.Empty;
            //    LeftButtonColor = Color.Empty;
            //}


            //int selectedindexmain = GetSelectedIndex();
            //EntryPoint.WriteToConsole($"OVERALL selectedindexmain {selectedindexmain}", 5);

            DebugString = $"Main: {IsScriptHashRunning} Texts: {_texts.IsScriptRunning} Contacts: {_contacts.IsScriptRunning}";


        }
        private void SetUnread()
        {
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(Handle, "SET_DATA_SLOT");
            NativeFunction.Natives.xC3D0841A0CC546A6(1);//is always 1?

            NativeFunction.Natives.xC3D0841A0CC546A6(1);//second index of homescreen starting at 0?

            int TotalNotifcations = _texts.Where(x => !x.IsRead).Count();

            NativeFunction.Natives.xC3D0841A0CC546A6(2);//iconid?
            NativeFunction.Natives.xC3D0841A0CC546A6(TotalNotifcations);//notifications
            //NativeFunction.Natives.xC3D0841A0CC546A6(99);

            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("Text");//appname
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

            NativeFunction.Natives.xC3D0841A0CC546A6(100);//opacity

            NativeFunction.Natives.xC3D0841A0CC546A6(100);
            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        }


        internal int GetSelectedIndex()
        {
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(Handle, "GET_CURRENT_SELECTION");
            int num = NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE<int>();
            while (!NativeFunction.Natives.x768FF8961BA904D6<bool>(num))         //UI::_GET_SCALEFORM_MOVIE_FUNCTION_RETURN_BOOL
                GameFiber.Wait(0);
            int data = NativeFunction.Natives.x2DE7EFA66B906036<int>(num);       //UI::_GET_SCALEFORM_MOVIE_FUNCTION_RETURN_INT
            return data;
        }



        /// <summary>
        /// Closes the phone.
        /// </summary>
        /// <param name="timer">Thread safe timer waiting before closing the phone. Time in ms.</param>
        public void Close(int timer = 0)
        {
            if (timer == 0)
                Close();
            else
                _timerClose = (int)(Game.GameTime + timer);
        }
        private void Close()
        {
            if (NativeFunction.Natives.x2C83A9DA6BFFC4F9< int>(_mScriptHash) > 0)
            {
                NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(Handle, "SHUTDOWN_MOVIE");
                NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

                GameFiber.Yield();

                //Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, CustomiFruit.GetCurrentInstance().Handle);
                Tools.Scripts.DestroyPhone(Handle);
                Tools.Scripts.TerminateScript("cellphone_flashhand");
                Tools.Scripts.TerminateScript("cellphone_controller");

                GameFiber.Yield();

                Tools.Scripts.StartScript("cellphone_flashhand", 1424);
                Tools.Scripts.StartScript("cellphone_controller", 1424);
            }
        }



    }


    public enum SoftKeyIcon
    {
        Blank = 1,
        Select = 2,
        Pages = 3,
        Back = 4,
        Call = 5,
        Hangup = 6,
        HangupHuman = 7,
        Week = 8,
        Keypad = 9,
        Open = 10,
        Reply = 11,
        Delete = 12,
        Yes = 13,
        No = 14,
        Sort = 15,
        Website = 16,
        Police = 17,
        Ambulance = 18,
        Fire = 19,
        Pages2 = 20
    }
}