using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TransactableLocation : InteractableLocation
{
    public ShopMenu Menu { get; set; }
    public bool HasCustomItemPostion => ItemPreviewPosition != Vector3.Zero;
    public Vector3 ItemPreviewPosition { get; set; } = Vector3.Zero;
    public float ItemPreviewHeading { get; set; } = 0f;
    public Vector3 ItemDeliveryPosition { get; set; } = Vector3.Zero;
    public float ItemDeliveryHeading { get; set; } = 0f;

    public TransactableLocation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, ShopMenu shopMenu) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        Menu = shopMenu;
        ButtonPromptText = $"Transact with {_Name}";
    }
    public TransactableLocation() : base()
    {
    }
    public override void OnInteract(IActivityPerformable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        EntryPoint.WriteToConsole("InteractableLocation OnInteract");
    }
}
