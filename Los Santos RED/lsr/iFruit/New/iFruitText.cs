using Rage;
using Rage.Native;
using System;
using System.IO;



namespace iFruitAddon2
{
    public class iFruitText
    {
        public string Name { get; set; } = "";
        public string Message { get; set; } = "";
        public int HourSent { get; set; } = 1;
        public int MinuteSent { get; set; } = 0;
        public bool IsRead { get; set; } = false;
        public int Index { get; set; } = 0;
        public ContactIcon Icon { get; set; } = ContactIcon.Blank;
        public string IconName { get; set; } = "";
        public bool Bold { get; set; } = false;
        public DateTime TimeReceived { get; set; }
        public iFruitText()
        {

        }
        public iFruitText(string name, int index, string message, int hourSent, int minuteSent)
        {
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