using System.Collections.Generic;


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
        foreach (RestInteract interiorInteract in RestInteracts)
        {
            interiorInteract.Update(Player, Settings, InteractableLocation, this);
        }
        base.InsideLoopNew();
    }
}

