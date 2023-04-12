using LosSantosRED.lsr.Interface;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;

public class BurnerPhoneApp
{
    //protected BurnerPhone BurnerPhone;
   // protected ICellPhoneable Player;
    protected ITimeReportable Time;
    protected ISettingsProvideable Settings;
    public int Index { get; set; } = 0;
    public int Icon { get; set; } = 0;
    public string Name { get; set; } = "App";
    public int Notifications { get; set; } = 0;
    public ICellPhoneable Player { get; private set; }
    public BurnerPhone BurnerPhone { get; private set; }

    public BurnerPhoneApp(BurnerPhone burnerPhone, ICellPhoneable player, ITimeReportable time, ISettingsProvideable settings, int index, string name, int icon)
    {
        BurnerPhone = burnerPhone;
        Player = player;
        Time = time;
        Settings = settings;
        Index = index;
        Name = name;
        Icon = icon;
    }
    public virtual void Open(bool Reset)
    {
        //EntryPoint.WriteToConsoleTestLong("BurnerPhoneApp OPEN");
        if(Reset)
        {

        }
        BurnerPhone.ReturnHome(Index);
    }
    public virtual void Update()
    {

    }

    public virtual void HandleInput()
    {

    }
    public virtual void Setup()
    {

    }
    public virtual void SetHomeMenu()
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(BurnerPhone.GlobalScaleformID, "SET_DATA_SLOT");
        NativeFunction.Natives.xC3D0841A0CC546A6(1);
        NativeFunction.Natives.xC3D0841A0CC546A6(Index);
        NativeFunction.Natives.xC3D0841A0CC546A6(Icon);
        NativeFunction.Natives.xC3D0841A0CC546A6(Notifications);
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Name);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();
        NativeFunction.Natives.xC3D0841A0CC546A6(100);
        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }
}

