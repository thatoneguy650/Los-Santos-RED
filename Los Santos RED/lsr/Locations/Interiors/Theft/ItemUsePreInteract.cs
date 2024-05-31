using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[XmlInclude(typeof(DrillUsePreInteract))]
public class ItemUsePreInteract
{
    protected IInteractionable Player;
    protected ILocationInteractable LocationInteractable;
    protected ISettingsProvideable Settings;
    protected IModItems ModItems;
    public ItemUsePreInteract()
    {

    }
    public virtual void Start(IInteractionable player, ILocationInteractable locationInteractable, ISettingsProvideable settings, IModItems modItems, TheftInteract theftInteract)
    {
        Player = player;
        LocationInteractable = locationInteractable;
        Settings = settings;
        ModItems = modItems;
        if(theftInteract != null)
        {
            theftInteract.SetUnlocked();
        }
    }
    
}
