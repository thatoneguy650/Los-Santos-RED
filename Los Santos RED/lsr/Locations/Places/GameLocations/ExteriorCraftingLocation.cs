using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;

public class ExteriorCraftingLocation : GameLocation
{
    public ExteriorCraftingLocation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        OpenTime = 0;
        CloseTime = 24;
    }
    public ExteriorCraftingLocation() : base()
    {

    }
    public override string TypeName { get; set; } = "CraftingLocation";
    public override int MapIcon { get; set; } = 537;//873;//162;
    public string CraftingFlag { get; set; }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Interact With {Name}";
        return true;
    }
    public override void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        if (CanInteract)
        {
            Player.Crafting.CraftingMenu.Show(CraftingFlag);
            NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
        }
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.ExteriorCraftingLocations.Add(this);
        base.AddLocation(possibleLocations);
    }
}

