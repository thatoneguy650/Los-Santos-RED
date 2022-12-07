using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Windows.Media.Imaging;

public class CameraCycler
{

    private IPedSwappable Player;


    private List<CameraCyclerPosition> CameraCyclerPositions = new List<CameraCyclerPosition>();
    private int CurrentPositionIndex = 0;

    private Vector3 DefaultCameraPosition;
    private Vector3 DefaultCameraLookAtPosition;




    public CameraCycler(Camera charCam, IPedSwappable player, PedExt modelPed, Vector3 initialPosition, Vector3 initialPosition2)
    { 
        Player = player;
        DefaultCameraPosition = initialPosition;
        DefaultCameraLookAtPosition = initialPosition2;
    }
    public void Setup()
    {
        CameraCyclerPositions.Add(new CameraCyclerPosition("Root", 0, 2.0f, 0));
        CameraCyclerPositions.Add(new CameraCyclerPosition("LHand", 18905, 2.0f, 1));
        CameraCyclerPositions.Add(new CameraCyclerPosition("RHand", 57005, 2.0f, 2));
        CameraCyclerPositions.Add(new CameraCyclerPosition("RKnee", 16335, 2.0f, 3));

    }

    public void Cycle(Camera charCam, PedExt modelPed)
    {
        CurrentPositionIndex++;
        if(CurrentPositionIndex >= CameraCyclerPositions.Count())
        {
            CurrentPositionIndex = 0;
        }
        CameraCyclerPosition ccp = CameraCyclerPositions.Where(x => x.Order == CurrentPositionIndex).FirstOrDefault();
        ccp?.MoveToPosition(charCam, modelPed);
    }

}

