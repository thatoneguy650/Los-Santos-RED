using LosSantosRED.lsr.Interface;
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

    private Blip AttachedBlip;
    private bool IsCreated = false;

    public SecurityCamera()
    {

    }
    public SecurityCamera(uint modelHash, Vector3 position, float heading)
    {
        ModelHash = modelHash;
        Position = position;
        Heading = heading;
    }
    public string Name { get; set; } = "Security Cam";
    public uint ModelHash { get; set; }
    public Vector3 Position { get; set; }
    public float Heading { get; set; }
    public bool IsDestroyed => isDestroyed;
    public void Activate(IEntityProvideable world)
    {
        CreateBlip(world);
        ShowedDestroyed = false;
        isDestroyed = false;
        IsCreated = false;
        EntryPoint.WriteToConsole("Security Camera ACTIVATED");
    }
    public void Deactivate()
    {
        DeleteBlip();
        ShowedDestroyed = false;
        isDestroyed = false;
        securityCameraObject = null;
        IsCreated = false;
        EntryPoint.WriteToConsole($"{Name} DEACTIVATED");
    }
    private void GetObject()
    {
        if(securityCameraObject.Exists())
        {
            return;
        }
        securityCameraObject = NativeFunction.Natives.GET_CLOSEST_OBJECT_OF_TYPE<Rage.Object>(Position.X, Position.Y, Position.Z, 15f, ModelHash, false, false, false);
        if (!securityCameraObject.Exists())
        {
            IsCreated = false;

            return;
        }
        //securityCameraObject.IsPersistent = false;
        IsCreated = true;
        //EntryPoint.WriteToConsole($"{Name} Created");
    }
    public void Update()
    {
        GetObject();
        UpdateBlip();
        if (!IsCreated || IsDestroyed)
        {
            return;
        }
        UpdateCameraStatus();
    }
    private void UpdateCameraStatus()
    {
        if (!securityCameraObject.Exists())
        {
            OnCameraDeleted();
            return;
        }
        if (securityCameraObject.HasBeenDamagedBy(Game.LocalPlayer.Character))
        {
            OnCameraDestroyed();
        }
    }
    private void OnCameraDestroyed()
    {
        isDestroyed = true;
        if (!ShowedDestroyed)
        {         
            Game.DisplayHelp($"{Name} Destroyed");
            ShowedDestroyed = true;
        }
        EntryPoint.WriteToConsole($"{Name} DESTROYED");
    }
    private void OnCameraDeleted()
    {
        IsCreated = false;
        //EntryPoint.WriteToConsole($"{Name} DELETED?");
    }
    private void CreateBlip(IEntityProvideable world)
    {
        if(AttachedBlip.Exists())
        {
            return;
        }
        AttachedBlip = new Blip(Position) { Name = Name, Color = EntryPoint.LSRedColor, Sprite = (BlipSprite)629,Angle = (int)Heading,Scale = 0.55f };

        EntryPoint.WriteToConsole($"SECURITY CAMERA BLIP CREATED");
        if (!AttachedBlip.Exists())
        {
            return;
        }
        AttachedBlip.Color = EntryPoint.LSRedColor;
        AttachedBlip.Scale = 0.55f;
        NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)AttachedBlip.Handle, true);     
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("CCTV");
        NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(AttachedBlip);
        world.AddBlip(AttachedBlip);
        EntryPoint.WriteToConsole($"{Name} BLIP CREATED");
    }
    private void UpdateBlip()
    {
        if (!AttachedBlip.Exists())
        {
            return;
        }
        AttachedBlip.Alpha = IsDestroyed || !securityCameraObject.Exists() ? 0.25f : 1.0f;
    }
    private void DeleteBlip()
    {
        if (!AttachedBlip.Exists())
        {
            AttachedBlip = null;
            return;
        }
        AttachedBlip.Delete();
        AttachedBlip = null;
        EntryPoint.WriteToConsole($"{Name} BLIP DELETED");
    }
}