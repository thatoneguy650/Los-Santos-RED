using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Debug;

public class StreetNode
{
    private ISettingsProvideable Settings;
    private IStreets Streets;
    //private int globalScaleformID;
    private Street CurrentStreet;
    private Street CurrentCrossStreet;

    public StreetNode(Vector3 position, float heading, ISettingsProvideable settings, IStreets streets)
    {
        Position = position;
        Heading = heading;
        Settings = settings;
        Streets = streets;
    }

    public Vector3 Position { get; set; }
    public float Heading { get; set; }
    public eVehicleNodeProperties eVehicleNodeProperties { get; set; }
    public int Density { get; set; }
    public float DistanceToPlayer { get; set; } = 999f;
    public void Display(int globalScaleformID)
    {




        //string textToDisplay = "";
        //if (CurrentStreet != null)
        //{
        //    textToDisplay += CurrentStreet.ProperStreetName;
        //}
        //if (CurrentCrossStreet != null)
        //{
        //    textToDisplay += " - " + CurrentCrossStreet.ProperStreetName;
        //}



        
        //NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_ORGANISATION_NAME");

        //NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        //NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(textToDisplay);
        //NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

        //NativeFunction.Natives.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT(Settings.SettingsManager.DebugSettings.StreetDisplayStyleIndex);
        //NativeFunction.Natives.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT(Settings.SettingsManager.DebugSettings.StreetDisplayColorIndex);
        //NativeFunction.Natives.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT(Settings.SettingsManager.DebugSettings.StreetDisplayFontIndex);


        //NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

        //Vector3 rotationVector;
        //float realYaw = Heading;
        //realYaw = 360 - realYaw;
        //realYaw = realYaw - 180;
        //rotationVector = new Vector3(0f, 0f, realYaw);

        //Vector3 drawPos = Position;
        //drawPos += new Vector3(Settings.SettingsManager.DebugSettings.StreetDisplayOffsetX, Settings.SettingsManager.DebugSettings.StreetDisplayOffsetY, Settings.SettingsManager.DebugSettings.StreetDisplayOffsetZ);


        //    Render3D(globalScaleformID,
        //        drawPos,
        //         rotationVector,
        //         Settings.SettingsManager.DebugSettings.StreetDisplayScaleX * new Vector3(1.0f, 1.0f, 1.0f));
        










        //NativeFunction.Natives.SET_DRAW_ORIGIN(drawPos.X, drawPos.Y, drawPos.Z, 0);
        //NativeHelper.DisplayTextOnScreen(textToDisplay, 0f, 0f, Settings.SettingsManager.LSRHUDSettings.StreetScale, System.Drawing.Color.White, Settings.SettingsManager.LSRHUDSettings.StreetFont,
        //    (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.StreetJustificationID, false, 255);
        //NativeFunction.Natives.CLEAR_DRAW_ORIGIN();


        //Rage.Debug.DrawArrowDebug(drawPos, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Black);
    }
    public void Render3D(int handle, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        NativeFunction.Natives.DRAW_SCALEFORM_MOVIE_3D_SOLID(handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
    }
    public void GetStreetsAtPos()
    {
        int StreetHash = 0;
        int CrossingHash = 0;
        string CurrentStreetName;
        string CurrentCrossStreetName;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", Position.X, Position.Y, Position.Z, &StreetHash, &CrossingHash);
        }
        string StreetName = string.Empty;
        if (StreetHash != 0)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);
                StreetName = Marshal.PtrToStringAnsi(ptr);
            }
            CurrentStreetName = StreetName;
            //GameFiber.Yield();
        }
        else
        {
            CurrentStreetName = "";
        }

        string CrossStreetName = string.Empty;
        if (CrossingHash != 0)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", CrossingHash);
                CrossStreetName = Marshal.PtrToStringAnsi(ptr);
            }
            CurrentCrossStreetName = CrossStreetName;
            //GameFiber.Yield();
        }
        else
        {
            CurrentCrossStreetName = "";
        }

        CurrentStreet = Streets.GetStreet(CurrentStreetName);
        CurrentCrossStreet = Streets.GetStreet(CurrentCrossStreetName);
    }

    public void GetDistance()
    {
        DistanceToPlayer = Position.DistanceTo2D(Game.LocalPlayer.Character.Position);
    }
}

