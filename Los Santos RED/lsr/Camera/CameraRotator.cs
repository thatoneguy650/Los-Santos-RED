using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Policy;
using LosSantosRED.lsr.Helper;

public class CameraRotator
{
    private float prevXPercent;
    private float prevYPercent;
    private Camera CurrentCamera;
    private float XDifference;
    private float YDifference;
    private bool DoMovement = false;
    private Vector3 CenterPoint;
    private Quaternion currentRot;
    private Vector3 dir;
    private float XPercent;
    private float YPercent;
    private float _polarAngleDeg;
    private float _azimuthAngleDeg;
    private float _radius = 5.0f;
    private PointF _dragOffset;
    private CameraClamp2 CameraClamp;

    public CameraRotator(Camera currentCamera, Vector3 centerPoint)
    {
        CurrentCamera = currentCamera;
        CenterPoint = centerPoint;
        CameraClamp = new CameraClamp2() { MaxVerticalValue = -40f, MinVerticalValue = -3f };
    }

    public void RotateCameraByMouse()
    {
        dir = new Vector3(0, 0, 5f);//assign value to the distance between the maincamera and the target
        GameFiber.StartNew(delegate
        {
            while (!Game.IsKeyDownRightNow(Keys.Z))
            {
                CheckMouseMovement();
                MoveCamera();
                

                //if(Game.IsKeyDownRightNow(Keys.H))
                //{
                //    DoMovement = !DoMovement;
                //    GameFiber.Sleep(500);
                //}
                GameFiber.Yield();
            }
        }, "Run Debug Logic");
        GameFiber.Sleep(1000);
    }
    private void CheckMouseMovement()
    {
        XPercent = NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, (int)GameControl.CursorX);
        YPercent = NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, (int)GameControl.CursorY);


        XDifference = XPercent - prevXPercent;
        YDifference = YPercent - prevYPercent;
        

        prevXPercent = XPercent;
        prevYPercent = YPercent;


    }
    private void MoveCamera()
    {
        if(!CurrentCamera.Exists())// || !Game.IsKeyDownRightNow(Keys.XButton1))
        {
            return;
        }

        // var xMagnitude = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.LookLeftRight);// (int)GTA.Control.LookLeftRight);
        // var yMagnitude = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.LookUpDown);

        // _polarAngleDeg += (xMagnitude * 10);

        // if (_polarAngleDeg >= 360)
        //     _polarAngleDeg = 0;

        //// _azimuthAngleDeg += (yMagnitude * 10);

        // if (_azimuthAngleDeg >= 360)
        //     _azimuthAngleDeg = 0;

        // var nextCamLocation = polar3DToWorld3D(CenterPoint, _radius, _polarAngleDeg, _azimuthAngleDeg);
        // CurrentCamera.Position = new Vector3(nextCamLocation.X, nextCamLocation.Y, nextCamLocation.Z);

        // Vector3 ToLookAt1 = new Vector3(CenterPoint.X, CenterPoint.Y, CenterPoint.Z);
        // CurrentCamera.Direction = (ToLookAt1 - CurrentCamera.Position).ToNormalized();
        // Game.DisplaySubtitle($"Z to Exit {XDifference} {YDifference} {_radius} {_polarAngleDeg} {_azimuthAngleDeg}");

        //CurrentCamera.po(CenterPoint);



        //Vector3 tempV = new Vector3(XDifference, YDifference, 0);
        //Quaternion targetRot = Quaternion.Euler(tempV);

        //currentRot = Quaternion.Slerp(currentRot, targetRot, 50);

        //CurrentCamera.Position = CenterPoint + currentRot.ToVector() * dir;
        //CurrentCamera.Rotation = new Rotator(YDifference, 0f, XDifference);



        //if (_isDragging)
        //{
        //    Function.Call(Hash._SHOW_CURSOR_THIS_FRAME);
        var dir = RotationToDirection(CurrentCamera.Rotation.ToVector());

        var len = (CenterPoint - CurrentCamera.Position).Length();



        var rotLeft = CurrentCamera.Rotation.ToVector() + new Vector3(0, 0, -10);
        var rotRight = CurrentCamera.Rotation.ToVector() + new Vector3(0, 0, 10);
        var right = RotationToDirection(rotRight) -
                    RotationToDirection(rotLeft);

        var rotUp = CurrentCamera.Rotation.ToVector() + new Vector3(-20, 0, 0);
        var rotDown = CurrentCamera.Rotation.ToVector() + new Vector3(20, 0, 0);
        var up = RotationToDirection(rotUp) - RotationToDirection(rotDown);


        var mouseX = XPercent;
        var mouseY = YPercent;

        mouseX = (mouseX * 2) - 1;
        mouseY = (mouseY * 2) - 1;

        Vector3 rotation = new Vector3();

        if (!IsCameraClamped(true, mouseX - _dragOffset.X))
            rotation += right * 15 * (mouseX - _dragOffset.X);
        if (!IsCameraClamped(false, mouseY - _dragOffset.Y))
            rotation += up * -((mouseY - _dragOffset.Y) * 15);
        rotation += dir * (len - 5f);

        CurrentCamera.Position += rotation;

        _dragOffset = new PointF(mouseX, mouseY);


         Vector3 ToLookAt1 = new Vector3(CenterPoint.X, CenterPoint.Y, CenterPoint.Z);
         CurrentCamera.Direction = (ToLookAt1 - CurrentCamera.Position).ToNormalized();

        //}
    }


    private Vector3 RotationToDirection(Vector3 Rotation)
    {
        float z = Rotation.Z;
        float num = z * 0.0174532924f;
        float x = Rotation.X;
        float num2 = x * 0.0174532924f;
        float num3 = Math.Abs((float)Math.Cos((double)num2));
        return new Vector3
        {
            X = (float)((double)((float)(-(float)Math.Sin((double)num))) * (double)num3),
            Y = (float)((double)((float)Math.Cos((double)num)) * (double)num3),
            Z = (float)Math.Sin((double)num2)
        };
    }

    public bool IsCameraClamped(bool horizontally, float delta)
    {
        if (horizontally)
        {
            var goingLeft = delta < 0;
            if (goingLeft)// && !CameraClamp.LeftHorizontalValue.HasValue)
                return false;
            if (!goingLeft)// && !CameraClamp.RightHorizontalValue.HasValue)
                return false;


            if (CameraClamp.LeftHorizontalValue > 180f)
                CameraClamp.LeftHorizontalValue = CameraClamp.LeftHorizontalValue - (360 * ((int)(CameraClamp.LeftHorizontalValue / 360)) + 1);

            if (CameraClamp.RightHorizontalValue > 180f)
                CameraClamp.RightHorizontalValue = CameraClamp.RightHorizontalValue - (360 * ((int)(CameraClamp.RightHorizontalValue / 360)) + 1);

            var sameHemisphereLeft = (CurrentCamera.Rotation.ToVector().Z > 0f && CameraClamp.LeftHorizontalValue > 0f) ||
                                 (CurrentCamera.Rotation.ToVector().Z < 0f && CameraClamp.LeftHorizontalValue < 0f);

            var sameHemisphereRight = (CurrentCamera.Rotation.ToVector().Z > 0f && CameraClamp.RightHorizontalValue > 0f) ||
                                 (CurrentCamera.Rotation.ToVector().Z < 0f && CameraClamp.RightHorizontalValue < 0f);

            if (goingLeft && CurrentCamera.Rotation.ToVector().Z > CameraClamp.RightHorizontalValue && sameHemisphereRight)
                return true;
            if (!goingLeft && CurrentCamera.Rotation.ToVector().Z < CameraClamp.LeftHorizontalValue && sameHemisphereLeft)
                return true;
            return false;
        }
        else
        {
            var goingDown = delta < 0;
            if (goingDown)// && !CameraClamp.MinVerticalValue.HasValue)
                return false;
            if (!goingDown)// && !CameraClamp.MaxVerticalValue.HasValue)
                return false;
            if (goingDown && CurrentCamera.Rotation.ToVector().X > CameraClamp.MinVerticalValue)
                return true;
            if (!goingDown && CurrentCamera.Rotation.ToVector().X < CameraClamp.MaxVerticalValue)
                return true;
            return false;
        }
    }

    public float Clamp(float value, float min, float max)
    {
        if (value > max) return max;
        if (value < min) return min;
        return value;
    }

    private Vector3 polar3DToWorld3D(Vector3 entityPosition, float radius, float polarAngleDeg, float azimuthAngleDeg)
    {
        // convert degrees to radians
        var polarAngleRad = polarAngleDeg * Math.PI / 180.0;
        var azimuthAngleRad = azimuthAngleDeg * Math.PI / 180.0;

        float x = entityPosition.X + radius * (float)(Math.Sin(azimuthAngleRad) * Math.Cos(polarAngleRad));
        float y = entityPosition.Y - radius * (float)(Math.Sin(azimuthAngleRad) * Math.Sin(polarAngleRad));
        float z = entityPosition.Z - radius * (float)(Math.Cos(azimuthAngleRad));

        return new Vector3(x, y, z);
    }

    private class CameraClamp2
    {
        public CameraClamp2()
        {
        }

        public float MaxVerticalValue { get; set; }
        public float MinVerticalValue { get; set; }
        public float LeftHorizontalValue { get; set; }
        public float RightHorizontalValue { get; set; }
    }
}

