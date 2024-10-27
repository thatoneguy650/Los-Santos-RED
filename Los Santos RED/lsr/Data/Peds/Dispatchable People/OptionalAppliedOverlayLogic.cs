using ExtensionsMethods;
using Rage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class OptionalAppliedOverlayLogic
{
    public List<OptionalAppliedOverlay> OptionalAppliedOverlays { get; set; } = new List<OptionalAppliedOverlay>();
    public List<AppliedOverlayZonePercentage> AppliedOverlayZonePercentages { get; set; } = new List<AppliedOverlayZonePercentage>();
    public OptionalAppliedOverlayLogic()
    {

    }
    public void Setup()
    {
        foreach(OptionalAppliedOverlay optionalAppliedOverlay in OptionalAppliedOverlays)
        {
            optionalAppliedOverlay.Setup();
        }
    }
    public void ApplyToPed(Ped ped, PedVariation variationToSet)
    {
        if(!ped.Exists())
        {
            return;
        }
        if(OptionalAppliedOverlays == null || !OptionalAppliedOverlays.Any())
        {
            return;
        }
        if(AppliedOverlayZonePercentages == null || !AppliedOverlayZonePercentages.Any())
        {
            AppliedOverlayZonePercentages = new List<AppliedOverlayZonePercentage>
            {
                new AppliedOverlayZonePercentage("ZONE_HEAD", 40f, 1),
                new AppliedOverlayZonePercentage("ZONE_TORSO", 40f, 1),
                new AppliedOverlayZonePercentage("ZONE_RIGHT_ARM", 40f, 1),
                new AppliedOverlayZonePercentage("ZONE_LEFT_ARM", 40f, 1),
                new AppliedOverlayZonePercentage("ZONE_LEFT_LEG", 40f, 1),
                new AppliedOverlayZonePercentage("ZONE_RIGHT_LEG", 40f, 1),
            };
        }
        foreach(AppliedOverlayZonePercentage appliedOverlayZonePercentage in AppliedOverlayZonePercentages)
        {
            for (int i = 0; i < appliedOverlayZonePercentage.Limit; i++)
            {
                if (RandomItems.RandomPercent(appliedOverlayZonePercentage.Percentage))//Do Check Each Zone Each Time
                {
                    OptionalAppliedOverlay oao = OptionalAppliedOverlays.Where(x => x.ZoneName == appliedOverlayZonePercentage.ZoneName).PickRandom();
                    if(oao != null)
                    {
                        oao.Apply(ped, variationToSet);
                    }
                }
            }
        }            
    }
}

