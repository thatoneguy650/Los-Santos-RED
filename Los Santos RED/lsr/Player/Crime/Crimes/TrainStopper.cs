using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TrainStopper
{
    private IPoliceRespondable Player;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private IEntityProvideable World;
    private uint GameTimeBetweenTrainStop;
    private uint GameTimeSawOnTrain;


    public bool IsStoppingTrains { get; private set; }
    public TrainStopper(IPoliceRespondable player, ISettingsProvideable settings, ITimeReportable time, IEntityProvideable world)
    {
        Player = player;
        Settings = settings;
        Time = time;
        World = world;
        GameTimeBetweenTrainStop = RandomItems.GetRandomNumber(Settings.SettingsManager.PoliceSettings.MinTimeToStopTrains, Settings.SettingsManager.PoliceSettings.MaxTimeToStopTrains);
    }

    public void Dispose()
    {
        IsStoppingTrains = false;
        GameTimeSawOnTrain = 0;
    }

    public void Reset()
    {
        IsStoppingTrains = false;
        GameTimeSawOnTrain = 0;
    }

    public void Update()
    {
        if(!Settings.SettingsManager.PoliceSettings.AllowStoppingTrains)
        {

        }
        if (Player.IsRidingOnTrain && Player.AnyPoliceRecentlySeenPlayer)
        {
            if (GameTimeSawOnTrain == 0)
            {
                EntryPoint.WriteToConsole("YOU WERE SEEN BY POLICE RIDING ON A TRAIN!");
                GameTimeSawOnTrain = Game.GameTime;
            }
            if (Game.GameTime - GameTimeSawOnTrain >= GameTimeBetweenTrainStop)
            {
                StopAllTrains();
                GameTimeBetweenTrainStop = RandomItems.GetRandomNumber(Settings.SettingsManager.PoliceSettings.MinTimeToStopTrains, Settings.SettingsManager.PoliceSettings.MaxTimeToStopTrains);
            }
        }
        else
        {
            GameTimeSawOnTrain = 0;
        }
    }
    public void StopAllTrains()
    {
        if(IsStoppingTrains)
        {
            return;
        }
        IsStoppingTrains = true;
        EntryPoint.WriteToConsole("STARTING STOPPING ALL TRAINS");
        Player.Scanner.OnStoppingTrains();
        GameFiber.StartNew(delegate
        {
            try
            {
                List<VehicleExt> TrainVehicles = new List<VehicleExt>();
                GetAllTrains(TrainVehicles);
                uint GameTimeLastGotAllTrains = Game.GameTime;
                while (IsStoppingTrains && EntryPoint.ModController.IsRunning && Player.IsWanted)
                {
                    TrainVehicles.RemoveAll(x => !x.Vehicle.Exists());
                    if(Game.GameTime - GameTimeLastGotAllTrains >= 3000)
                    {
                        GetAllTrains(TrainVehicles);
                        GameTimeLastGotAllTrains = Game.GameTime;
                        EntryPoint.WriteToConsole("STOP TRAINS< GOT NEW TRAAIN LIST");
                    }
                    if(!TrainVehicles.Any())
                    {
                        IsStoppingTrains = false;
                        EntryPoint.WriteToConsole("NO TRAINS AROUND, STOPPING THE STOP");
                        break;
                    }
                    foreach(VehicleExt vehicleExt in TrainVehicles)
                    {
                        if (vehicleExt.Vehicle.Exists())
                        {
                            float speed = vehicleExt.Vehicle.Speed;
                            float newSpeed = speed * 0.95f;
                            if(newSpeed <= 1.5f)
                            {
                                newSpeed = 0.0f;
                            }
                            NativeFunction.Natives.SET_TRAIN_SPEED(vehicleExt.Vehicle, newSpeed);
                        }
                    }
                    GameFiber.Yield();
                }
                IsStoppingTrains = false;
                EntryPoint.WriteToConsole("ENDING STOPPING ALL TRAINS");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("StopAllTrains" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "StopAllTrains");
    }
    private void GetAllTrains(List<VehicleExt> TrainVehicles)
    {
        foreach (VehicleExt vehicleExt in World.Vehicles.CivilianVehicles.ToList())
        {
            if (!vehicleExt.Vehicle.Exists())
            {
                continue;
            }
            if (NativeFunction.Natives.IS_THIS_MODEL_A_TRAIN<bool>(vehicleExt.Vehicle.Model.Hash))
            {
                if(!TrainVehicles.Any(x=> x.Handle == vehicleExt.Handle))
                {
                    TrainVehicles.Add(vehicleExt);
                }
            }
        }
    }
}

