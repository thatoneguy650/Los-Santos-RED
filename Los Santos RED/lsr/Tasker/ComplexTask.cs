using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class ComplexTask
{
    protected ITargetable Player;
    protected IComplexTaskable Cop;
    protected ComplexTask(ITargetable player, IComplexTaskable cop)
    {
        Player = player;
        Cop = cop;
    }
    public string Name { get; set; }
    public AIDynamic CurrentDynamic
    {
        get
        {
            if (Player.IsInVehicle)
            {
                if (Cop.IsInVehicle)
                {
                    return AIDynamic.Cop_InVehicle_Player_InVehicle;
                }
                else
                {
                    return AIDynamic.Cop_OnFoot_Player_InVehicle;
                }
            }
            else
            {
                if (Cop.IsInVehicle)
                {
                    return AIDynamic.Cop_InVehicle_Player_OnFoot;
                }
                else
                {
                    return AIDynamic.Cop_OnFoot_Player_OnFoot;
                }
            }
        }
    }
    public abstract void Start();
    public abstract void Update();
    public abstract void Stop();

}

