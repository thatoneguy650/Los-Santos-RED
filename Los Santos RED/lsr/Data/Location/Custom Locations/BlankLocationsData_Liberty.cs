using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BlankLocationsData_Liberty
{
    private float defaultSpawnPercentage = 60f;
    public BlankLocationsData_Liberty()
    {

    }
    public List<BlankLocation> BlankLocationPlaces { get; set; } = new List<BlankLocation>();
    public void DefaultConfig()
    {
        SpeedTraps();
        Checkpoints();
        RooftopSnipers();
        OtherCops();
    }
    private void RooftopSnipers()
    {
        float sniperSpawnPercentage = 65f;
        List<BlankLocation> blankLocationPlaces = new List<BlankLocation>() {
            //LC Snipers
            new BlankLocation(new Vector3(-148.9657f, 1058.107f, 40.98241f), 318.6407f, "LCRoofTopSniper1", "LCRooftop Sniper 1")
            {  //middlepark overlook
                ActivateDistance = 300f,
                ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-148.9657f, 1058.107f, 40.98241f), 318.6407f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle, LongGunAlwaysEquipped = true }, },
                },
        };
        BlankLocationPlaces.AddRange(blankLocationPlaces);
    }
    private void Checkpoints()
    {
       
    }
    private void SpeedTraps()
    {
       
    }
    private void OtherCops()
    {
      
    }    
}

