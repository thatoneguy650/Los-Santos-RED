using System.Collections.Generic;
using System.Linq;

public class ResidenceInterior : Interior
{
    protected Residence residence;
    public Residence Residence => residence;
    public List<RestInteract> RestInteracts { get; set; } = new List<RestInteract>();
    public ResidenceInterior()
    {
       
    }

    public ResidenceInterior(int iD, string name) : base(iD, name)
    {

    }
    public void SetResidence(Residence newResidence)
    {
        residence = newResidence;
        foreach (RestInteract test in RestInteracts)
        {
            test.RestableLocation = newResidence;
        }
    }
    public override void InsideLoopNew()
    {
        float closestDistanceTo = 999f;
        InteriorInteract closestInteriorInteract = null;
        foreach (RestInteract interiorInteract in RestInteracts)
        {
            interiorInteract.Update(Player, Settings, InteractableLocation, this, LocationInteractable);
            if (interiorInteract.DistanceTo <= closestDistanceTo)
            {
                closestDistanceTo = interiorInteract.DistanceTo;
                closestInteriorInteract = interiorInteract;
            }
        }
        foreach (RestInteract interiorInteract in RestInteracts)
        {
            if (interiorInteract == closestInteriorInteract && interiorInteract.CanAddPrompt)
            {
                interiorInteract.AddPrompt();
            }
            else
            {
                interiorInteract.RemovePrompt();
            }
        }
        base.InsideLoopNew();
    }
}

