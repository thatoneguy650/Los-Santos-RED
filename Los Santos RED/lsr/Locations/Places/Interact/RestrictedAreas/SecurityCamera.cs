using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SecurityCamera
{
    private Rage.Object securityCameraObject;
    private bool isDestroyed;
    private bool ShowedDestroyed = false;
    public SecurityCamera()
    {

    }
    public SecurityCamera(uint modelHash, Vector3 position)
    {
        ModelHash = modelHash;
        Position = position;
    }
    public string Name { get; set; } = "Security Cam";
    public uint ModelHash { get; set; }
    public Vector3 Position { get; set; }
    public bool IsDestroyed => isDestroyed;
    private void GetObject()
    {
        if(securityCameraObject.Exists())
        {
            return;
        }
        securityCameraObject = NativeFunction.Natives.GET_CLOSEST_OBJECT_OF_TYPE<Rage.Object>(Position.X, Position.Y, Position.Z, 10f, ModelHash, false, false, false);
    }
    public void Update()
    {
        GetObject();
        if (!securityCameraObject.Exists())
        {
            isDestroyed = true;
            return;
        }
        if (securityCameraObject.HasBeenDamagedBy(Game.LocalPlayer.Character))
        {
            isDestroyed = true;
            if (!ShowedDestroyed)
            {
                EntryPoint.WriteToConsole($"{Name} DESTROYED");
                Game.DisplaySubtitle($"{Name} Destroyed");
                ShowedDestroyed = true;
            }
        }
        else
        {
            isDestroyed = false;
        }
    }

    public void Reset()
    {
        ShowedDestroyed = false;
        isDestroyed = false;
        securityCameraObject = null;
    }
}