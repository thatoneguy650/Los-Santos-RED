using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ThirstNeed : HumanNeed
{
    private IHumanStateable Player;
    private float MinChangeValue = -0.005f;
    
    public ThirstNeed(string name, float minValue, float maxValue, IHumanStateable humanStateable) : base(name, minValue, maxValue, humanStateable)
    {
        Player = humanStateable;
    }

    public override void OnMaximum()
    {

    }

    public override void OnMinimum()
    {

    }

    public override void Update()
    {
        if(NeedsUpdate)
        {
            if(Player.IsAlive)
            {
                if(Player.IsInVehicle)
                {
                    Change(MinChangeValue);
                }
                else
                {
                    float Multiplier = 1.0f;
                    if(Player.FootSpeed >= 1.0f)
                    {
                        Multiplier = Player.FootSpeed/5.0f; 
                    }
                    if(Multiplier <= 1.0f)
                    {
                        Multiplier = 1.0f;
                    }
                    Change(MinChangeValue * Multiplier);
                }
            }
        }
    }
}

