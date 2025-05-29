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
    private ILocationInteractable Player;

    private Vector3 entityLocation;
    private bool isInputPressed;
    private float XNormalValue;
    private float YNormalValue;
    private float angleZ = 0f;
    private float angleY = 0f;//170f;
    private float Radius = 7f;
    private float RadiusIncrementLow = 0.25f;
    private float RadiusIncrement = 1.0f;
    private float RadiusIncrementHigh = 5.0f;
    private float initialHeading = 0f;
    private bool _isInputPressed;

    public OrbitCamera(ILocationInteractable player, Entity orbitingEntity, Camera existingCamera, ISettingsProvideable settings, MenuPool menuPool)
    {
        Player = player;
        OrbitingEntity = orbitingEntity;
        MainCamera = existingCamera;
        Settings = settings;
        MenuPool = menuPool;
    }
    public bool IsRunning { get; private set; } = false;
    public void Setup()
    {
        if (!OrbitingEntity.Exists())
        {
            return;
        }
        IsRunning = true;
        initialHeading = OrbitingEntity.Heading;
        angleZ = (initialHeading + Settings.SettingsManager.PlayerOtherSettings.OrbitCameraInitialHorizontalOffset) * 10.0f;
        angleY = Settings.SettingsManager.PlayerOtherSettings.OrbitCameraInitialVerticalOffset;
        if (!MainCamera.Exists())
        {
            MainCamera = new Camera(true);
            MainCamera.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        }
        Player.ButtonPrompts.AttemptAddPrompt("orbitCamera", "Select", "orbitCameraselect", GameControl.FrontendAccept, 4);
        Player.ButtonPrompts.AttemptAddPrompt("orbitCamera", "Back", "orbitCameraback", GameControl.FrontendCancel, 3);



        Player.ButtonPrompts.AttemptAddPrompt("orbitCameraExtra", "Orbit Camera", "orbitCameramovecameraStart", GameControl.Attack, 2);


        //EntryPoint.WriteToConsole($"ORBIT CAMERA SETUP RAN angleZ {angleZ} initialHeading {initialHeading}");
        GameFiber.StartNew(delegate
        {
            while (IsRunning)
            {
                Update();
                GameFiber.Yield();
            }
        }, "Run Orbit Camera");
    }
    public void Dispose()
    {
        EntryPoint.WriteToConsole("ORBIT CAMERA DISPOSE RAN");
        IsRunning = false;
        if(MainCamera.Exists())
        {
            MainCamera.Delete();
        }
        Player.ButtonPrompts.RemovePrompts("orbitCamera");
        Player.ButtonPrompts.RemovePrompts("orbitCameraExtra");
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
        GetControlValues();
        DisableControlsWhilePanning();
        CalculateFinalPosition();
    }
    private void DisableControlsWhilePanning()
    {
        NativeFunction.Natives.SET_MOUSE_CURSOR_THIS_FRAME();  
        if (MenuPool == null)
        {
            return;//no menu pool do not handle any controls
        }
        MenuPool.DisableInstructionalButtons = true;
        if (isInputPressed != _isInputPressed)
        {
            if(isInputPressed)
            {
                OnPressedMouseDown();
            }
            else
            {
                OnReleasedMouseDown();
            }
            _isInputPressed = isInputPressed;
        }
        if (isInputPressed)
        {
            MenuPool.Draw();
        }
        else
        {
            MenuPool.ProcessMenus();
        }
    }

    private void OnReleasedMouseDown()
    {
        NativeFunction.Natives.SET_MOUSE_CURSOR_STYLE(3);
        Player.ButtonPrompts.RemovePrompts("orbitCamera");
        Player.ButtonPrompts.RemovePrompts("orbitCameraExtra");
        Player.ButtonPrompts.AttemptAddPrompt("orbitCamera", "Select", "orbitCameraselect", GameControl.FrontendAccept, 4);
        Player.ButtonPrompts.AttemptAddPrompt("orbitCamera", "Back", "orbitCameraback", GameControl.FrontendCancel, 3);
        Player.ButtonPrompts.AttemptAddPrompt("orbitCameraExtra", "Orbit Camera", "orbitCameramovecameraStart", GameControl.Attack, 2);
    }

    private void OnPressedMouseDown()
    {
        NativeFunction.Natives.SET_MOUSE_CURSOR_STYLE(4);
        Player.ButtonPrompts.RemovePrompts("orbitCamera");
        Player.ButtonPrompts.RemovePrompts("orbitCameraExtra");
        Player.ButtonPrompts.AttemptAddPrompt("orbitCameraExtra", "Move Camera", "orbitCameramovecamera", GameControl.LookLeftRight, 2);
        Player.ButtonPrompts.AttemptAddPrompt("orbitCameraExtra", "Zoom", "orbitCamerazoom", GameControl.WeaponWheelNext, 1);
    }

    private void CalculateFinalPosition()
    {
        //All vector magic from https://github.com/Jorn08/dragcam/blob/master/client.lua
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
        // EntryPoint.WriteToConsole($"XNormalValue:{XNormalValue} YNormalValue:{YNormalValue} angleZ{angleZ} angleY{angleY} InitalHeading {initialHeading} ModANgle{angleZ % 360f} AngleZRadians{Extensions.ToRadians(angleZ)}");
        Vector3 camPos = new Vector3(entityLocation.X + offset.X, entityLocation.Y + offset.Y, entityLocation.Z + offset.Z);
        MainCamera.Position = camPos;
        MainCamera.PointAtEntity(OrbitingEntity, Vector3.Zero, false);
    }
    private void GetControlValues()
    {
        isInputPressed = NativeFunction.Natives.IS_CONTROL_PRESSED<bool>(0, 24) || NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, 24);
        if (isInputPressed)
        {
            XNormalValue = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, 1) * 40.0f * Settings.SettingsManager.PlayerOtherSettings.OrbitCameraSensitivity;
            YNormalValue = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, 2) * 40.0f * Settings.SettingsManager.PlayerOtherSettings.OrbitCameraSensitivity;
        }
        else
        {
            XNormalValue = 0f;
            YNormalValue = 0f;
        }
        if (isInputPressed && (NativeFunction.Natives.IS_CONTROL_JUST_RELEASED<bool>(0, 14) || NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_RELEASED<bool>(0, 14)))
        {
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
    }
}

