using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;

public class HeliEngage
{
    private IComplexTaskable Ped;
    private IPlayerChaseable Cop;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private bool IsAssignedCircle;
    private uint GameTimeAssignedCircle;
    private bool IsAssignedRappel;
    private uint GameTimeAssignedRappel;
    private uint GameTimeStartedRappel;
    private ITargetable Player;
    private VehicleExt Helicopter;
    private bool CanHelicopterRappel;
    private uint StoredHeliHandle;
    private bool HasStartedRappel;
    private bool HasFinishedRappel;
    private bool ShouldCircle => Player.CurrentLocation.StationaryTime >= Settings.SettingsManager.PoliceTaskSettings.CircleStationaryTime;// Player.CurrentLocation.IsMostlyStationary;
    private bool ShouldRappel => Settings.SettingsManager.PoliceTaskSettings.AllowRappelling 
        //&& !HasFinishedRappel
        && CanHelicopterRappel
        //&& ShouldCircle 
        && Player.CurrentLocation.StationaryTime >= Settings.SettingsManager.PoliceTaskSettings.RappellingStationaryTime
        && GameTimeAssignedCircle != 0 
        && Game.GameTime - GameTimeAssignedCircle >= Settings.SettingsManager.PoliceTaskSettings.MinCircleTimeToStartRappelling 
        //&& (!Settings.SettingsManager.PoliceTaskSettings.RappellingRequiresLethalForce || Player.PoliceResponse.LethalForceAuthorized) 

        && IsAbleToEngage
        && Cop.PlayerPerception.HeightToTarget <= 65f


        //&& (!Settings.SettingsManager.PoliceTaskSettings.RappellingRequiresWeaponsFree || Player.PoliceResponse.IsWeaponsFree)
        && Helicopter != null
        && Helicopter.Vehicle.Exists()
        && ((HasStartedRappel && !HasFinishedRappel) || Helicopter.Vehicle.PassengerCount >= 2);

    private bool IsAbleToEngage
    {
        get
        {
            if(Settings.SettingsManager.PoliceTaskSettings.RappellingRequiresWeaponsFree)
            {
                return Player.PoliceResponse.IsWeaponsFree;
            }
            else if (Settings.SettingsManager.PoliceTaskSettings.RappellingRequiresLethalForce)
            {
                return Player.PoliceResponse.LethalForceAuthorized;
            }
            return true;
        }
    }

    public HeliEngage(IComplexTaskable ped, IPlayerChaseable cop, IEntityProvideable world, ISettingsProvideable settings, ITargetable player)
    {
        Ped = ped;
        Cop = cop;
        World = world;
        Settings = settings;
        Player = player;
    }

    public void AssignTask()
    {
        if(!Ped.Pedestrian.Exists())
        {
            return;
        }
        IsAssignedCircle = false;
        NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, Player.Character, RandomItems.RandomPercent(50) ? -35f : 35f, RandomItems.RandomPercent(50) ? -35f : 35f, RandomItems.GetRandomNumber(35f, 50f));
        UpdateAssociatedHelicopter();
    }
    private void UpdateAssociatedHelicopter()
    {
        if (!Ped.Pedestrian.Exists() || !Ped.Pedestrian.CurrentVehicle.Exists())
        {
            Helicopter = null;
            StoredHeliHandle = 0;
            return;
        }
        uint handleToSearch = Ped.Pedestrian.CurrentVehicle.Handle;
        if(handleToSearch != StoredHeliHandle)
        {
            if (Ped.AssignedVehicle != null && Ped.AssignedVehicle.Handle == handleToSearch)
            {
                Helicopter = Ped.AssignedVehicle;
            }
            else
            {
                Helicopter = World.Vehicles.AllVehicleList.Where(x => x.Handle == handleToSearch).FirstOrDefault();
                if (Helicopter == null)
                {
                    EntryPoint.WriteToConsole("Assigned Heli Engage Task but unknown helicopter NO RAPPEL");

                }
            }
            if (Helicopter != null)
            {
                CanHelicopterRappel = NativeFunction.Natives.DOES_VEHICLE_ALLOW_RAPPEL<bool>(Ped.Pedestrian.CurrentVehicle);
            }
            EntryPoint.WriteToConsole($"HeliEngage AssignedHelicopter Updated to {handleToSearch} from {StoredHeliHandle} HasHeli {Helicopter != null} CanHelicopterRappel {CanHelicopterRappel}");
            StoredHeliHandle = handleToSearch;
        }   
    }
    public void UpdateTask()
    {
        if(!Ped.IsInHelicopter || !Ped.Pedestrian.Exists() || !Ped.Pedestrian.CurrentVehicle.Exists())
        {
            return;
        }
        UpdateAssociatedHelicopter();
        if (Ped.IsDriver)
        {
            UpdatePilotTask();
        }
        else
        {
            UpdatePassengerTask();//NOT CALLED? MAYBE
        }
        
    }
    private void UpdatePilotTask()
    {
//# if DEBUG
//        EntryPoint.WriteToConsole($"ShouldRappel:{ShouldRappel} ShouldCircle:{ShouldCircle} StationaryTime{Player.CurrentLocation.StationaryTime} GameTimeAssignedCircle{GameTimeAssignedCircle} CircleTime{Game.GameTime - GameTimeAssignedCircle} HasStartedRappel{HasStartedRappel} HasFinishedRappel{HasFinishedRappel} CanHelicopterRappel{CanHelicopterRappel}");
//        if(Helicopter != null && Helicopter.Vehicle.Exists())
//        {
//            EntryPoint.WriteToConsole($"PassengerCount:{Helicopter.Vehicle.PassengerCount}");
//        }
//#endif
        if (ShouldRappel)
        {
            PilotReppelTask();
        }
        else if (ShouldCircle)//Player.CurrentLocation.IsMostlyStationary)
        {
            PilotCircleTask();
        }
        else
        {
            PilotChaseTask();
        }
    }

    private void PilotChaseTask()
    {
        if (IsAssignedCircle)
        {
            NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, Player.Character, RandomItems.RandomPercent(50) ? -35f : 35f, RandomItems.RandomPercent(50) ? -35f : 35f, RandomItems.GetRandomNumber(35f, 50f));
            IsAssignedCircle = false;
            GameTimeAssignedCircle = 0;
            IsAssignedRappel = false;
            GameTimeAssignedRappel = 0;
            HasStartedRappel = false;
            EntryPoint.WriteToConsole($"IsAssignedCircle CHANGED TO {IsAssignedCircle}");
        }
    }

    private void PilotReppelTask()
    {
        if (!IsAssignedRappel)
        {
            NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, Player.Character, 0f, 0f, 0f, Settings.SettingsManager.PoliceTaskSettings.HeliMission,
            0f,//20f, //Cruise SPeed
                45f, // Target Reached DIst
                -1f, //Heli Orientation
                30, //flight height
                30,//min hiehg tabove terrain
                -1, //slowdown distance
                0);//HELIMODE heli flags, 0 is none
                   //IsAssignedCircle = false;
                   //GameTimeAssignedCircle = 0;
            IsAssignedRappel = true;
            GameTimeAssignedRappel = Game.GameTime;
            GameTimeStartedRappel = 0;
            HasFinishedRappel = false;
            EntryPoint.WriteToConsole("Heli Engage Assigned Hover Rappel Task");
        }
        else
        {
            EntryPoint.WriteToConsole($"HasStartedRappel {HasStartedRappel} TimeStartedRappel:{Game.GameTime - GameTimeStartedRappel} GameTimeStartedRappel:{GameTimeStartedRappel} HasFinishedRappel{HasFinishedRappel}");

            if (Game.GameTime - GameTimeAssignedRappel >= 5000 && !Ped.IsMovingFast && !HasStartedRappel && !HasFinishedRappel)
            {
                EntryPoint.WriteToConsole("Heli Engage Rappel Start Ran");
                StartRappell();
            }

            if (HasStartedRappel && GameTimeStartedRappel > 0 && Game.GameTime - GameTimeStartedRappel >= 3000)
            {
                EntryPoint.WriteToConsole("Heli Engage Rappel Started Waiting For END");
                if (Helicopter == null)
                {
                    ResetItems();
                    EntryPoint.WriteToConsole($"Heli Engage HasStartedRappel {HasStartedRappel} BAD");
                }
                else
                {
                    bool hasAnyRappel = NativeFunction.Natives.IS_ANY_PED_RAPPELLING_FROM_HELI<bool>(Helicopter.Vehicle);
                    if (!hasAnyRappel)
                    {
                        ResetItems();
                        EntryPoint.WriteToConsole($"Heli Engage HasStartedRappel {HasStartedRappel} NO MORE PEDS RAPPELLING");
                    }
                }
            }

        }
    }
    private void ResetItems()
    {
        HasStartedRappel = false;
        HasFinishedRappel = true;
        IsAssignedCircle = false;
        GameTimeAssignedCircle = 0;
        IsAssignedRappel = false;
        GameTimeAssignedRappel = 0;
        HasStartedRappel = false;
    }
    private void PilotCircleTask()
    {
        if (!IsAssignedCircle)
        {
            NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, Player.Character, 0f, 0f, 0f, Settings.SettingsManager.PoliceTaskSettings.HeliMission,
            Settings.SettingsManager.PoliceTaskSettings.HeliMissionCruiseSpeed,//20f, //Cruise SPeed
                45f, // Target Reached DIst
                -1f, //Heli Orientation
                30, //flight height
                30,//min hiehg tabove terrain
                -1, //slowdown distance
                0);//HELIMODE heli flags, 0 is none
            IsAssignedCircle = true;
            GameTimeAssignedCircle = Game.GameTime;
            IsAssignedRappel = false;
            GameTimeAssignedRappel = 0;
            HasStartedRappel = false;
            //        NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle,
            //0, Player.Character,//target vehicle and ped
            //0f, 0f, 0f,//coordinated, shouldnt be needed
            //Settings.SettingsManager.DebugSettings.HeliMission,//MISSION
            //100f,//Cruise SPeed
            //50f,//Target Reached DIst
            //-1f,//Heli Orientation
            //50,//flight height
            //50, //min hiehg tabove terrain
            //-1.0f,//slowdown distance
            //0//HELIMODE heli flags, 0 is none
            //);
            EntryPoint.WriteToConsole($"IsAssignedCircle CHANGED TO {IsAssignedCircle}");
        }
    }
    private void UpdatePassengerTask()
    {

    }
    private void StartRappell()
    {
        if (!Ped.Pedestrian.Exists() || !Ped.Pedestrian.CurrentVehicle.Exists() || !CanHelicopterRappel || Helicopter == null || !Helicopter.Vehicle.Exists())
        {
            return;
        }
        uint heliHandle = Helicopter.Handle;// Ped.Pedestrian.CurrentVehicle.Handle;
        List<Cop> Passengers = World.Pedestrians.PoliceList.Where(x => x.Handle != Ped.Handle && !x.IsDriver && x.IsInVehicle && x.Pedestrian.Exists() && x.Pedestrian.CurrentVehicle.Exists() && x.Pedestrian.CurrentVehicle.Handle == heliHandle).ToList();
        foreach (Cop c in Passengers)
        {
            EntryPoint.WriteToConsole($"RAPPEL START FOR {c.Handle}");
            int seatIndex = -2;
            if (c.Pedestrian.CurrentVehicle.Exists())
            {
                seatIndex = c.Pedestrian.SeatIndex;
            }
            if (seatIndex != 1 && seatIndex != 2)
            {
                continue;
            }
            //EntryPoint.WriteToConsole($"RAPPEL FINISH FOR {c.Handle}");
            int DoorID = seatIndex == 1 ? 2 : 3;
            NativeFunction.Natives.TASK_RAPPEL_FROM_HELI(c.Pedestrian, 10f);
            HasStartedRappel = true;
            Helicopter.AddRappelled(c,seatIndex);
        }
        GameTimeStartedRappel = Game.GameTime;
    }
}

