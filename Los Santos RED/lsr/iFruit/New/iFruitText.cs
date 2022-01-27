using Rage;
using Rage.Native;
using System;
using System.IO;



namespace iFruitAddon2
{
    public class iFruitText
    {

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
    }
}