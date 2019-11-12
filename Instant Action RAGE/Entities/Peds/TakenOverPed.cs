using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TakenOverPed
{
    public Ped Pedestrian { get; set; }
    public PoolHandle OriginalHandle { get; set; }
    public PedVariation Variation { get; set; }
    public Model OriginalModel { get; set; }
    public uint GameTimeTakenover { get; set; }
    public TakenOverPed(Ped _Pedestrian, PoolHandle _OriginalHandle)
    {
        Pedestrian = _Pedestrian;
        OriginalHandle = _OriginalHandle;
    }
    public TakenOverPed(Ped _Pedestrian, PoolHandle _OriginalHandle,PedVariation _Variation, Model _OriginalModel,uint _GameTimeTakenover)
    {
        Pedestrian = _Pedestrian;
        OriginalHandle = _OriginalHandle;
        Variation = _Variation;
        OriginalModel = _OriginalModel;
        GameTimeTakenover = _GameTimeTakenover;
    }
}

