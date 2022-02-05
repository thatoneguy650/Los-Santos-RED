using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class InteractableLocation : BasicLocation
{
    public bool HasCustomCamera => CameraPosition != Vector3.Zero;
    public Vector3 CameraPosition { get; set; } = Vector3.Zero;
    public Vector3 CameraDirection { get; set; } = Vector3.Zero;
    public Rotator CameraRotation { get; set; }
    public bool CanInteract { get; set; } = true;
    public InteractableLocation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ButtonPromptText = $"Interact with {_Name}";
    }
    public InteractableLocation() : base()
    {
    }
    public virtual void OnInteract(IActivityPerformable Player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        EntryPoint.WriteToConsole("InteractableLocation OnInteract 2");
    }
}

