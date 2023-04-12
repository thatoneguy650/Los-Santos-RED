using LSR.Vehicles;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using System.Windows.Forms;
using LosSantosRED.lsr.Interface;
using RAGENativeUI.Elements;

public class VehicleShowcase
{

    private Camera StoreCam;
    private Vector3 _direction;
    private Camera CameraTo;
    private Vector3 InitialCameraPosition;
    private List<HighlightPosition> OffsetVectorList;
    private int MaxPositions;
    private int CurrentPosition = 0;
    private bool IsBig;
    private bool IsSmall;
    private bool IsRunning = false;
    private ISettingsProvideable Settings;
    private bool hidelsrUI;
    private bool hideRadar;
    private float distanceAwayX = 4.0f;
    private float distanceAwayY = 4.0f;
    private float distanceAwayZ = 1.5f;
    private ITimeControllable Time;
    private int LiveryToSet = 0;
    public VehicleShowcase(VehicleExt toShow, ISettingsProvideable settings, ITimeControllable time)
    {
        HighlightedVehicle = toShow;
        Settings = settings;
        Time = time;
    }

    public VehicleExt HighlightedVehicle { get; set; }

    public void Start()
    {
        if (HighlightedVehicle == null || !HighlightedVehicle.Vehicle.Exists())
        {
            //EntryPoint.WriteToConsoleTestLong("VehicleShowcase Failed 1");
            return;
        }
        Time.PauseTime();
        SetupPositions();
        IsRunning = true;
        hidelsrUI = Settings.SettingsManager.UIGeneralSettings.HideLSRUIUnlessActionWheelActive;
        hideRadar = Settings.SettingsManager.UIGeneralSettings.HideRadarUnlessActionWheelActive;
        Settings.SettingsManager.UIGeneralSettings.HideLSRUIUnlessActionWheelActive = true;
        Settings.SettingsManager.UIGeneralSettings.HideRadarUnlessActionWheelActive = true;
        GameFiber.StartNew(delegate
        {
            while (IsRunning)
            {
                DisableControls();
                NativeFunction.CallByName<bool>("DISPLAY_RADAR", false);
                GameFiber.Yield();
            }
        }, "VehicleShowcase");
        HighlightedVehicle.Vehicle.Wash();
        GameFiber.StartNew(delegate
        {
            try
            {
                Game.LocalPlayer.Character.IsVisible = false;
                HighlightEntity(false);
                while (!Game.IsKeyDownRightNow(Keys.RButton))
                {
                    if (Game.IsKeyDownRightNow(Keys.LButton))
                    {
                        HighlightEntity(true);
                    }



                    if (Game.IsKeyDownRightNow(Keys.O))
                    {
                        distanceAwayZ += 0.5f;
                        Game.DisplaySubtitle($"distanceAwayZ {distanceAwayZ}");
                        GameFiber.Sleep(100);
                        HighlightEntity(false);
                    }
                    if (Game.IsKeyDownRightNow(Keys.L))
                    {
                        distanceAwayZ -= 0.5f;
                        Game.DisplaySubtitle($"distanceAwayZ {distanceAwayZ}");
                        GameFiber.Sleep(100);
                        HighlightEntity(false);
                    }
                    if (Game.IsKeyDownRightNow(Keys.U))
                    {
                        distanceAwayX += 0.5f;
                        Game.DisplaySubtitle($"distanceAwayX {distanceAwayX}");
                        GameFiber.Sleep(100);
                        HighlightEntity(false);
                    }
                    if (Game.IsKeyDownRightNow(Keys.J))
                    {
                        distanceAwayX -= 0.5f;
                        Game.DisplaySubtitle($"distanceAwayX {distanceAwayX}");
                        GameFiber.Sleep(100);
                        HighlightEntity(false);
                    }

                    if (Game.IsKeyDownRightNow(Keys.I))
                    {
                        distanceAwayY += 0.5f;
                        Game.DisplaySubtitle($"distanceAwayY {distanceAwayY}");
                        GameFiber.Sleep(100);
                        HighlightEntity(false);
                    }
                    if (Game.IsKeyDownRightNow(Keys.K))
                    {
                        distanceAwayY -= 0.5f;
                        Game.DisplaySubtitle($"distanceAwayY {distanceAwayY}");
                        GameFiber.Sleep(100);
                        HighlightEntity(false);
                    }
                    if (Game.IsKeyDownRightNow(Keys.OemSemicolon))
                    {
                        SetNewLivery();
                        GameFiber.Sleep(100);
                    }
                    GameFiber.Yield();
                }
                StopImmediately();
                IsRunning = false;
                Time.UnPauseTime();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "VehicleShowcase");
    }

    private void SetNewLivery()
    {
        int Total = NativeFunction.Natives.GET_VEHICLE_LIVERY_COUNT<int>(HighlightedVehicle.Vehicle);
        if (Total == -1)
        {
            return;
        }
        if (HighlightedVehicle != null && HighlightedVehicle.Vehicle.Exists())
        {
            NativeFunction.Natives.SET_VEHICLE_LIVERY(HighlightedVehicle.Vehicle, LiveryToSet);
            //Game.DisplaySubtitle($"SET LIVERY {LiveryToSet}");
            LiveryToSet++;
            if(LiveryToSet > Total)
            {
                LiveryToSet = 0;
            }
        }
    }

    public void StopImmediately()
    {
        IsRunning = false;
        NativeFunction.Natives.SET_EVERYONE_IGNORE_PLAYER(Game.LocalPlayer, false);
        Game.LocalPlayer.Character.IsVisible = true;
        NativeFunction.Natives.CLEAR_FOCUS();
        if (StoreCam.Exists())
        {
            StoreCam.Delete();
        }
        if (CameraTo.Exists())
        {
            CameraTo.Delete();
        }
        EnableControls();
        Game.LocalPlayer.Character.Tasks.Clear();
        Settings.SettingsManager.UIGeneralSettings.HideLSRUIUnlessActionWheelActive = hidelsrUI;
        Settings.SettingsManager.UIGeneralSettings.HideRadarUnlessActionWheelActive = hideRadar;
    }
    private void EnableControls()
    {
        Game.DisableControlAction(0, GameControl.Attack, false);
        Game.DisableControlAction(0, GameControl.Attack2, false);
        Game.DisableControlAction(0, GameControl.Aim, false);
        Game.LocalPlayer.HasControl = true;
    }
    private void DisableControls()
    {
        NativeHelper.DisablePlayerControl();
        Game.DisableControlAction(0, GameControl.Attack, true);
        Game.DisableControlAction(0, GameControl.Attack2, true);
        Game.DisableControlAction(0, GameControl.Aim, true);
    }
    public void HighlightEntity(bool getNewOffset)
    {
        if(HighlightedVehicle == null || !HighlightedVehicle.Vehicle.Exists())
        {
            //EntryPoint.WriteToConsoleTestLong("VehicleShowcase Failed 2");
            return;
        }
        NativeFunction.Natives.SET_VEHICLE_DIRT_LEVEL(HighlightedVehicle.Vehicle, 0.0f);
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }

        GetNewOffset(getNewOffset);
        
        StoreCam.Position = InitialCameraPosition;
        Vector3 ToLookAt = new Vector3(HighlightedVehicle.Vehicle.Position.X, HighlightedVehicle.Vehicle.Position.Y, HighlightedVehicle.Vehicle.Position.Z + 0.5f);
        _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
        StoreCam.Direction = _direction;
        StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        if (!CameraTo.Exists())
        {
            CameraTo = new Camera(false);
        }
        if (Camera.RenderingCamera != null)
        {
            CameraTo.Position = Camera.RenderingCamera.Position;
            CameraTo.FOV = Camera.RenderingCamera.FOV;
            CameraTo.Rotation = Camera.RenderingCamera.Rotation;
        }
        else
        {
            CameraTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            CameraTo.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            CameraTo.Rotation = new Rotator(r.X, r.Y, r.Z);
            CameraTo.Active = true;
        }
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(StoreCam, CameraTo, 500, true, true);
        GameFiber.Sleep(500);
    }


    private void GetNewOffset(bool increase)
    {

        HighlightPosition hp = OffsetVectorList.FirstOrDefault(x => x.PositionNumber == CurrentPosition);
        float cool = hp.OffsetVector.X;
        float width = HighlightedVehicle.Vehicle.Model.Dimensions.X;
        float length = HighlightedVehicle.Vehicle.Model.Dimensions.Y;
        float height = HighlightedVehicle.Vehicle.Model.Dimensions.Z;
        Vector3 ToShowVector = new Vector3(hp.OffsetVector.X * distanceAwayX, hp.OffsetVector.Y * distanceAwayY, hp.OffsetVector.Z * distanceAwayZ);

        //EntryPoint.WriteToConsoleTestLong($"CurrentPosition {CurrentPosition} ToShowVector {ToShowVector}");
        InitialCameraPosition = HighlightedVehicle.Vehicle.GetOffsetPosition(ToShowVector);

        if (increase)
        {
            CurrentPosition++;
            if (CurrentPosition > MaxPositions)
            {
                CurrentPosition = 0;
            }
        }
    }
    private void SetupPositions()
    {
        OffsetVectorList = new List<HighlightPosition>() 
        { 

            new HighlightPosition(0, new Vector3(1f, 1f, 1f)),//FL
            new HighlightPosition(1, new Vector3(1f, -1f, 1f)),//FR
            new HighlightPosition(2, new Vector3(-1f, 1f, 1f)),//RL
            new HighlightPosition(3, new Vector3(-1f, -1f, 1f)),//RR

            new HighlightPosition(4, new Vector3(1f, 0, 1f)),//FRONT
            new HighlightPosition(5, new Vector3(-1f, 0, 1f)),//REAR
            new HighlightPosition(6, new Vector3(0, 1f, 1f)),//LEFT
            new HighlightPosition(7, new Vector3(0, -1f, 1f)),//RIGHT

        };
        MaxPositions = OffsetVectorList.Max(x => x.PositionNumber);
    }
    private class HighlightPosition
    {
        public HighlightPosition(int positionNumber, Vector3 offsetVector)
        {
            PositionNumber = positionNumber;
            OffsetVector = offsetVector;
        }

        public int PositionNumber { get; set; }
        public Vector3 OffsetVector { get; set; }
    }
}

