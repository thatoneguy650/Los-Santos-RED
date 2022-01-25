using Rage;
using Rage.Native;
using System;
using System.IO;



namespace iFruitAddon2
{
    public class iFruitText
    {
        private bool _dialActive, _busyActive;
        private int _dialSoundID = -1;
        private int _busySoundID = -1;
        private int _callTimer, _busyTimer;

        /// <summary>
        /// Fired when the contact picks up the phone.
        /// </summary>
        public event ContactAnsweredEvent Answered;
        protected virtual void OnAnswered(iFruitContact sender) { return; }// Answered?.Invoke(this); }

        /// <summary>
        /// The name of the contact.
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// The message sent
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// The hour the message was sent
        /// </summary>
        /// 
        public int HourSent { get; set; } = 1;
        /// <summary>
        /// The minute the message was sent
        /// </summary>
        public int MinuteSent { get; set; } = 0;
        /// <summary>
        /// The minute the message was sent
        /// </summary>
        public bool IsRead { get; set; } = false;
        /// <summary>
        /// The index where we should draw the item.
        /// </summary>
        public int Index { get; private set; } = 0;

        /// <summary>
        /// Status representing the outcome when the contact is called. 
        /// Contact will answer when true.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Milliseconds timeout before the contact picks up. 
        /// Set this to 0 if you want the contact to answer instantly.
        /// </summary>
        public int DialTimeout { get; set; } = 0;

        /// <summary>
        /// The icon to associate with this contact.
        /// </summary>
        public ContactIcon Icon { get; set; } = ContactIcon.Generic;

        /// <summary>
        /// Set the contact text in bold.
        /// </summary>
        public bool Bold { get; set; } = false;

        public iFruitText(string name, int index, string message, int hourSent, int minuteSent)
        {
            //UpdateContactIndex();
            Name = name;
            Index = index;
            Message = message;
            HourSent = hourSent;
            MinuteSent = minuteSent;
        }
        internal void Draw(int handle)
        {
            NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(handle, "SET_DATA_SLOT");
            NativeFunction.Natives.xC3D0841A0CC546A6(6);//2
            NativeFunction.Natives.xC3D0841A0CC546A6(Index);
            NativeFunction.Natives.xC3D0841A0CC546A6(HourSent);
            NativeFunction.Natives.xC3D0841A0CC546A6(MinuteSent);

            if(IsRead)
            {
                NativeFunction.Natives.xC3D0841A0CC546A6(34);
            }
            else
            {
                NativeFunction.Natives.xC3D0841A0CC546A6(33);
            }


            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Name);
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Message);
            NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();


            NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
        }

        internal void Update()
        {
            // Contact was busy and busytimer has ended
            if (_busyActive && Game.GameTime > _busyTimer)
            {
                //Game.LocalPlayer.Character.Task.PutAwayMobilePhone();
                NativeFunction.Natives.TASK_USE_MOBILE_PHONE(Game.LocalPlayer.Character, false);
                NativeFunction.Natives.STOP_SOUND(_busySoundID);
                NativeFunction.Natives.RELEASE_SOUND_ID(_busySoundID);
                _busySoundID = -1;
                _busyActive = false;
            }

            // We are calling the contact
            if (_dialActive && Game.GameTime > _callTimer)
            {
                NativeFunction.Natives.STOP_SOUND(_dialSoundID);
                NativeFunction.Natives.RELEASE_SOUND_ID(_dialSoundID);
                _dialSoundID = -1;

                //if (!Active)
                //{
                //    // Contact is busy, play the busy sound until the busytimer runs off
                //    iFruitContactCollection.DisplayCallUI(CustomiFruit.GetCurrentInstance().Handle, Name, "CELL_220", Icon.Name.SetBold(Bold)); // Displays "BUSY"
                //    _busySoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
                //    NativeFunction.Natives.PLAY_SOUND_FRONTEND(_busySoundID, "Remote_Engaged", "Phone_SoundSet_Default", 1);
                //    _busyTimer = (int)Game.GameTime + 5000;
                //    _busyActive = true;
                //}
                //else
                //{
                //    iFruitContactCollection.DisplayCallUI(CustomiFruit.GetCurrentInstance().Handle, Name, "CELL_219", Icon.Name.SetBold(Bold)); // Displays "CONNECTED"
                //    OnAnswered(this); // Answer the phone
                //}

                _dialActive = false;
            }
        }

        /// <summary>
        /// Call this contact.
        /// If DialTimeout less or equal than 0, the contact will pickup instantly.
        /// </summary>
        public void Call()
        {
            // Cannot call if already on call or contact is busy (Active == false)
            if (_dialActive || _busyActive)
                return;


            //Game.LocalPlayer.Character.tas.UseMobilePhone();
            NativeFunction.Natives.TASK_USE_MOBILE_PHONE(Game.LocalPlayer.Character, true);


            // Do we have to wait before the contact pickup the phone?
            //if (DialTimeout > 0)
            //{
            //    // Play the Dial sound
            //    iFruitContactCollection.DisplayCallUI(CustomiFruit.GetCurrentInstance().Handle, Name, "CELL_220", Icon.Name.SetBold(Bold)); // Displays "BUSY"
            //    _dialSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
            //    NativeFunction.Natives.PLAY_SOUND_FRONTEND(_dialSoundID, "Dial_and_Remote_Ring", "Phone_SoundSet_Default", 1);
            //    _callTimer = (int)Game.GameTime + DialTimeout;
            //    _dialActive = true;
            //}
            //else
            //{
            //    iFruitContactCollection.DisplayCallUI(CustomiFruit.GetCurrentInstance().Handle, Name, "CELL_219", Icon.Name.SetBold(Bold)); // Displays "CONNECTED"
            //    OnAnswered(this); // Answer the phone instantly
            //}
        }

        /// <summary>
        /// Stop and release phone sounds.
        /// </summary>
        public void EndCall()
        {
            if (_dialActive)
            {
                NativeFunction.Natives.STOP_SOUND(_dialSoundID);
                NativeFunction.Natives.RELEASE_SOUND_ID(_dialSoundID);
                _dialSoundID = -1;
                _dialActive = false;
            }

            if (_busyActive)
            {
                NativeFunction.Natives.STOP_SOUND(_busySoundID);
                NativeFunction.Natives.RELEASE_SOUND_ID(_busySoundID);
                _busySoundID = -1;
                _busyActive = false;
            }
        }

        private void UpdateContactIndex()
        {
            // Warning: new iFruitContact(...) can be called before iFruitAddon2 is initialized.
            // That's why it is important not to rely on static variables inside the iFruitAddon2 class.
            string tempFile = "";// iFruitAddon2.GetTempFilePath();

            //if (File.Exists(tempFile))
            //{
            //    // Not the first launch
            //    bool written = false;
            //    while (!written)
            //    {
            //        try
            //        {
            //            // We need to check if the file is unlocked and then get the value and write the new one.
            //            int index;

            //            StreamReader sr = new StreamReader(tempFile);
            //            if (int.TryParse(sr.ReadLine().Trim(new char[] { '\r', '\n', ' ' }), out index))
            //                iFruitAddon2.ContactIndex = index;
            //            sr.Close();

            //            StreamWriter file = new StreamWriter(tempFile);
            //            file.WriteLine(iFruitAddon2.ContactIndex + 1);
            //            file.Close();
            //            written = true;
            //        }
            //        catch (IOException)
            //        {
            //            // The file is locked when StreamReader or StreamWriter has opend it.
            //            // The current instance of iFruitAddon2 must wait until the file is released.
            //        }
            //        catch (Exception ex)
            //        {
            //            // Unknown error occured
            //            //Logger.Log(ex.Message);
            //            Game.Console.Print(ex.Message);
            //            written = true;
            //        }
            //    }
            //}
            //else
            //{
            //    // First launch. We create the file
            //    StreamWriter file = new StreamWriter(tempFile);
            //    file.Write(iFruitAddon2.ContactIndex + 1);
            //    file.Close();
            //}
        }

    }
}