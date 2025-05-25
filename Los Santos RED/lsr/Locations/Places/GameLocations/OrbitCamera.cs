using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class OrbitCamera
{
    private Entity OrbitingEntity;
    private Camera MainCamera;
    private ISettingsProvideable Settings;
    private MenuPool MenuPool;


    private Vector3 entityLocation;
    private float XNormalValue;
    private float YNormalValue;
    private float angleZ = 0f;
    private float angleY = 0f;//170f;
    private float Radius = 7f;
    private float RadiusIncrementLow = 0.25f;
    private float RadiusIncrement = 1.0f;
    private float RadiusIncrementHigh = 5.0f;
    private bool hasMoved = false;
    

    public OrbitCamera(Entity orbitingEntity, Camera existingCamera, ISettingsProvideable settings, MenuPool menuPool)
    {
        OrbitingEntity = orbitingEntity;
        MainCamera = existingCamera;
        Settings = settings;
        MenuPool = menuPool;
    }
    public bool IsRunning { get; private set; } = false;

    public void Setup()
    {
        IsRunning = true;


        angleZ = Settings.SettingsManager.DebugSettings.AngleZ;
        angleY = Settings.SettingsManager.DebugSettings.AngleY;
        if (!MainCamera.Exists())
        {
            MainCamera = new Camera(true);
            MainCamera.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        }


        EntryPoint.WriteToConsole("ORBIT CAMERA SETUP RAN");
        GameFiber.StartNew(delegate
        {
            while (IsRunning)
            {
                Update();
                GameFiber.Yield();
            }
        }, "Run Orbit Camera");
    }

    internal void Dispose()
    {
        EntryPoint.WriteToConsole("ORBIT CAMERA DISPOSE RAN");
        IsRunning = false;
        if(MainCamera.Exists())
        {
            MainCamera.Delete();
        }
    }





    private void Update()
    {
        //EntryPoint.WriteToConsole("ORBIT CAMERA UPDATE RAN");
        if (!OrbitingEntity.Exists())
        {
            EntryPoint.WriteToConsole("NO ORIBITNG ENTITY");
            return;
        }
        if(!MainCamera.Exists())
        {
            EntryPoint.WriteToConsole("NO MAIN CAMERA");
            return;
        }
        entityLocation = OrbitingEntity.Position;



        bool isInputPressed = NativeFunction.Natives.IS_CONTROL_PRESSED<bool>(0, 24) || NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, 24);
        if (isInputPressed)
        {
            //MenuPool.ControlDisablingEnabled = true;


            hasMoved = true;
            XNormalValue = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, 1) * 40.0f * Settings.SettingsManager.PlayerOtherSettings.OrbitCameraSensitivity;
            YNormalValue = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, 2) * 40.0f * Settings.SettingsManager.PlayerOtherSettings.OrbitCameraSensitivity;
        }
        else
        {
            //MenuPool.ControlDisablingEnabled = false;
            XNormalValue = 0f;
            YNormalValue = 0f;
        }
        if (isInputPressed && (NativeFunction.Natives.IS_CONTROL_JUST_RELEASED<bool>(0, 14) || NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_RELEASED<bool>(0, 14)))
        {
            hasMoved = true;

            if (Radius <= 3.0f)
            {
                Radius += RadiusIncrementLow;
            }
            else if (Radius >= 20f)
            {
                Radius += RadiusIncrementHigh;
            }
            else
            {
                Radius += RadiusIncrement;
            }
        }
        else if (isInputPressed && (NativeFunction.Natives.IS_CONTROL_JUST_RELEASED<bool>(0, 15) || NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_RELEASED<bool>(0, 15)))
        {
            hasMoved = true;
            Radius -= RadiusIncrement;


            if (Radius <= 3.0f)
            {
                Radius -= RadiusIncrementLow;
            }
            else if (Radius >= 25f)
            {
                Radius -= RadiusIncrementHigh;
            }
            else
            {
                Radius -= RadiusIncrement;
            }

        }
        Radius = Extensions.Clamp(Radius, 1.0f, 100f);


        if(isInputPressed)
        {
            MenuPool.DisableInstructionalButtons = true;
            MenuPool.Draw();
        }
        else
        {
            MenuPool.DisableInstructionalButtons = false;
            MenuPool.ProcessMenus();
        }

            //XNormalValue = NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, (int)GameControl.CursorX);
            //YNormalValue = NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, (int)GameControl.CursorY);

            // EntryPoint.WriteToConsole($"XNormalValue:{XNormalValue} YNormalValue:{YNormalValue}");

            angleZ = angleZ - XNormalValue;// left / right
            angleY = angleY + YNormalValue;// up / down
            angleY = Extensions.Clamp(angleY, 0.0f, 330f);// 89.0f);


            float cosAngleZ = (float)Math.Cos(Extensions.ToRadians(angleZ));
            float cosAngleY = (float)Math.Cos(Extensions.ToRadians(angleY));
            float sinAngleZ = (float)Math.Sin(Extensions.ToRadians(angleZ));
            float sinAngleY = (float)Math.Sin(Extensions.ToRadians(angleY));

            Vector3 offset = new Vector3(
                ((cosAngleZ * cosAngleY) + (cosAngleY * cosAngleZ)) / 2 * Radius,
                ((sinAngleZ * cosAngleY) + (cosAngleY * sinAngleZ)) / 2 * Radius,
                ((sinAngleY)) * Radius
            );


            Vector3 camPos = new Vector3(entityLocation.X + offset.X, entityLocation.Y + offset.Y, entityLocation.Z + offset.Z);
            MainCamera.Position = camPos;
            MainCamera.PointAtEntity(OrbitingEntity, Vector3.Zero, false);
        
    }


}

