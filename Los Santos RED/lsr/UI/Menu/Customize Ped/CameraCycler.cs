using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Windows.Media.Imaging;
using System.Threading;
public class CameraCycler
{

    private IPedSwappable Player;
    private PedCustomizer PedCustomizer;
    private List<CameraCyclerPosition> CameraCyclerPositions = new List<CameraCyclerPosition>();
    private int CurrentPositionIndex = 0;
    private Camera CharCam;
    public CameraCycler(Camera charCam, IPedSwappable player, PedCustomizer pedCustomizer, List<CameraCyclerPosition> cameraCyclerPositions)
    { 
        Player = player;
        PedCustomizer = pedCustomizer;
        CharCam = charCam;
        CameraCyclerPositions = cameraCyclerPositions;
    }



    public void Setup()
    {
        //CameraCyclerPositions.Add(new CameraCyclerPosition("Default", new Vector3(402.9301f, -998.267f, -98.51537f), new Vector3(0.004358141f, 0.9860916f, -0.1661458f), new Rotator(-9.5638f, -4.058472E-08f, -0.2532234f), 0));//new Vector3(402.8145f, -998.5043f, -98.29621f), new Vector3(-0.02121102f, 0.9286007f, -0.3704739f), new Rotator(-21.74485f, -5.170386E-07f, 1.308518f), 0));
        //CameraCyclerPositions.Add(new CameraCyclerPosition("Face", new Vector3(402.8708f, -997.5441f, -98.30454f), new Vector3(-0.005195593f, 0.9991391f, -0.04116036f), new Rotator(-2.358982f, 2.136245E-06f, 0.2979394f), 1));
        //CameraCyclerPositions.Add(new CameraCyclerPosition("Lower", new Vector3(402.9348f, -998.0379f, -99.38499f), new Vector3(0.02025275f, 0.9928887f, -0.117311f), new Rotator(-6.736939f, 1.611956E-07f, -1.168546f), 2));
        //CameraCyclerPositions.Add(new CameraCyclerPosition("Torso", new Vector3(402.9301f, -998.267f, -98.51537f), new Vector3(0.004358141f, 0.9860916f, -0.1661458f), new Rotator(-9.5638f, -4.058472E-08f, -0.2532234f), 3));
        //CameraCyclerPositions.Add(new CameraCyclerPosition("Hands", new Vector3(402.8127f, -997.4653f, -99.04851f), new Vector3(0.0355651f, 0.9914218f, -0.1257696f), new Rotator(-7.225204f, -5.647735E-07f, -2.054481f),4));
    }

    public void Cycle()
    {
        CurrentPositionIndex++;
        if(CurrentPositionIndex > CameraCyclerPositions.Count()-1)
        {
            CurrentPositionIndex = 0;
        }
        CameraCyclerPosition ccp = CameraCyclerPositions.Where(x => x.Order == CurrentPositionIndex).FirstOrDefault();
        ccp?.Move(CharCam);
        GameFiber.Sleep(100);
    }
    public void SetDefault()
    {
        CurrentPositionIndex = 0;
        CameraCyclerPositions.FirstOrDefault(x=> x.Order == 0)?.Move(CharCam);
        GameFiber.Sleep(100);
    }
    public void Set(string name)
    {
        CurrentPositionIndex = 0;
        CameraCyclerPosition ccp = CameraCyclerPositions.Where(x => x.Name == name).FirstOrDefault();
        ccp?.Move(CharCam);
        GameFiber.Sleep(100);
    }
}

