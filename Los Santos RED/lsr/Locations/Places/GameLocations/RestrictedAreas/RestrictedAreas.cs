using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RestrictedAreas
{

    public RestrictedAreas()
    {

    }
    public List<RestrictedArea> RestrictedAreasList { get; set; } = new List<RestrictedArea>();
    public List<VanillaRestrictedArea> VanillaRestrictedAreas { get; set; } = new List<VanillaRestrictedArea>();
    public void Activate(IEntityProvideable world)
    {
        foreach (RestrictedArea restrictedArea in RestrictedAreasList)
        {
            restrictedArea.Activate(world);
        }
    }
    public void Deactivate()
    {
        foreach (RestrictedArea restrictedArea in RestrictedAreasList)
        {
            restrictedArea.Deactivate();
        }
    }
    public void Update(ILocationInteractable player, IEntityProvideable world)
    {
        foreach(RestrictedArea restrictedArea in RestrictedAreasList)
        {
            restrictedArea.Update(player, world);
        }
    }
    public void UpdateVanilla(ILocationInteractable player, IEntityProvideable world)
    {
        foreach (VanillaRestrictedArea restrictedArea in VanillaRestrictedAreas)
        {
            restrictedArea.Update(player);
        }
    }
    public void RemoveImpoundRestrictions()
    {
        foreach (RestrictedArea restrictedArea in RestrictedAreasList.Where(x=> x.RestrictedAreaType == RestrictedAreaType.ImpoundLot))
        {
            restrictedArea.RemoveRestriction();
        }
    }
    public void AddDistanceOffset(Vector3 offsetToAdd)
    {
        foreach (RestrictedArea restrictedArea in RestrictedAreasList)
        {
            restrictedArea.AddDistanceOffset(offsetToAdd);
        }
    }
    public bool IsPlayerViolating() => RestrictedAreasList.Any(x => x.IsPlayerViolating);
    public bool IsPlayerViolatingVanilla() => VanillaRestrictedAreas.Any(x => x.IsPlayerViolating);
}

