using LosSantosRED.lsr.Interface;
using System.Collections.Generic;


public class VanillaRestrictedArea
{
    private bool isPlayerViolating;
    public bool IsPlayerViolating => isPlayerViolating;
    public List<AngledRestrictedArea> AngledRestrictedAreas { get; set; }
    public void Update(ILocationInteractable player)
    {
        isPlayerViolating = false;
        foreach(AngledRestrictedArea angledRestrictedArea in AngledRestrictedAreas)
        {
            if(angledRestrictedArea.CheckInside(player.Position))
            {
                isPlayerViolating = true;
                EntryPoint.WriteToConsole("PLAYER IS INSIDE ANGLED AREA VIOLATIONS!");
                return;
            }
        }
    }
}

