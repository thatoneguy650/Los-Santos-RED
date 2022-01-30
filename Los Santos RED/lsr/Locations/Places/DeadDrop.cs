using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DeadDrop : InteractableLocation
{
    public DeadDrop() : base()
    {
        
    }
    public override BlipSprite MapIcon { get; set; } = BlipSprite.Dead;
    public override Color MapIconColor { get; set; } = Color.White;
    public override string ButtonPromptText { get; set; }
    public DeadDrop(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ButtonPromptText = $"Drop Money at {_Name}";
    }
    public override void OnInteract()
    {

        Game.DisplayHelp("You interacted with a dead drop");
        base.OnInteract();
    }
}

