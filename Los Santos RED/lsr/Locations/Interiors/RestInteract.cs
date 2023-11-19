using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Xml.Serialization;

public class RestInteract : InteriorInteract
{ 
    [XmlIgnore]
    public IRestableLocation RestableLocation {get;set;}

    public RestInteract()
    {
    }

    public RestInteract(Vector3 position, float heading, string buttonPromptText) : base(position, heading, buttonPromptText)
    {

    }

    public override void OnInteract()
    {
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        Interior.IsMenuInteracting = true;
        RestableLocation?.OnRestInteract(this);
    }
    protected override void AddPrompt()
    {
        ButtonPromptIndetifierText = "RestInteriorLocation";
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AddPrompt(ButtonPromptIndetifierText, ButtonPromptText, ButtonPromptIndetifierText, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
}

