using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FightClub : GameLocation
{
    public FightClub() : base()
    {

    }
    public override string TypeName { get; set; } = "Fight Club";
    public override int MapIcon { get; set; } = (int)BlipSprite.Rampage;
    public override string ButtonPromptText { get; set; }
}
