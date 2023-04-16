//using LosSantosRED.lsr.Interface;
//using LSR.Vehicles;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class GeneralLocate_Old : ComplexTask, ILocationReachable
//{
//    private Cop Cop;
//    private IComplexTaskable complexTaskablePed;
//    private PedExt PedGeneral;
//    private IEntityProvideable World;
//    private IPlacesOfInterest PlacesOfInterest;
//    private SeatAssigner SeatAssigner;
//    private TaskState CurrentTaskState;
//    private ISettingsProvideable Settings;
//    private bool BlockPermanentEvents = false;
//    private bool CheckSiren = false;
//    private bool SetDriverSkill = false;
//    private bool HasSixthSense;
//    private bool AllowEnteringVehicle = true;

//    private bool LocationsChanged = false;


//    private Vector3 PlaceToDriveTo;
//    private Vector3 PlaceToWalkTo;
//    private Vector3 prevPlaceToDriveTo;
//    private Vector3 prevPlaceToWalkTo;
//    public bool HasReachedLocatePosition { get; set; } = false;


//    protected virtual bool ShouldInvestigateOnFoot => !Ped.IsInHelicopter && Player.IsOnFoot && Player.PoliceLastSeenOnFoot && Player.IsNearbyPlacePoliceShouldSearchForPlayer;

//    public GeneralLocate_Old(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents, bool checkSiren,
//        bool setDriverSkill, Cop cop) : base(player, ped, 1500)//1500
//    {
//        PedGeneral = pedGeneral;
//        complexTaskablePed = ped;
//        Cop = cop;
//        Name = "Locate";
//        SubTaskName = "";
//        World = world;
//        PlacesOfInterest = placesOfInterest;
//        Settings = settings;
//        BlockPermanentEvents = blockPermanentEvents;
//        CheckSiren = checkSiren;
//        SetDriverSkill = setDriverSkill;






//        PlaceToDriveTo = HasSixthSense ? Player.StreetPlacePoliceShouldSearchForPlayer : Player.StreetPlacePoliceLastSeenPlayer;// Player.StreetPlacePoliceShouldSearchForPlayer;
//        PlaceToWalkTo = HasSixthSense ? Player.PlacePoliceShouldSearchForPlayer : Player.PlacePoliceLastSeenPlayer; //Player.PlacePoliceShouldSearchForPlayer;
//        HasSixthSense = RandomItems.RandomPercent(Ped.IsInHelicopter ? Settings.SettingsManager.PoliceTaskSettings.SixthSenseHelicopterPercentage : Settings.SettingsManager.PoliceTaskSettings.SixthSensePercentage);
//        if (!HasSixthSense && Ped.DistanceToPlayer <= 40f && RandomItems.RandomPercent(Settings.SettingsManager.PoliceTaskSettings.SixthSensePercentageClose))
//        {
//            HasSixthSense = true;
//        }




//        SeatAssigner = new SeatAssigner(Ped, World, possibleVehicles);
//    }
//    public override void ReTask()
//    {
//        Start();
//    }
//    public override void Start()
//    {
//        CurrentTaskState?.Stop();
//        GetNewTaskState();
//        CurrentTaskState?.Start();
//        //EntryPoint.WriteToConsole($"{PedGeneral.Handle} STARTING Task{CurrentTaskState?.DebugName}");
//    }
//    public override void Stop()
//    {
//        CurrentTaskState?.Stop();
//    }
//    public override void Update()
//    {
//        GetLocations();
//        CheckLocationChanged();
//        if (LocationsChanged)
//        {
//            Start();
//        }
//        else
//        {
//            StandardUpdate();
//        }
//        UpdateVehicleState();
//    }
//    private void StandardUpdate()
//    {
//        if (CurrentTaskState == null || !CurrentTaskState.IsValid)
//        {
//            Start();
//        }
//        else
//        {
//            SubTaskName = CurrentTaskState.DebugName;
//            CurrentTaskState.Update();
//        }
//    }
//    private void GetNewTaskState()
//    {
//        if (!HasReachedLocatePosition)
//        {
//            if (Ped.IsInVehicle)
//            {
//                CurrentTaskState = new GoToInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToDriveTo, this);
//            }
//            else
//            {
//                CurrentTaskState = new GoToOnFootTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToWalkTo, this);
//            }
//        }
//        else if (ShouldInvestigateOnFoot)
//        {
//            if (Cop != null)
//            {
//                Cop.WeaponInventory.Reset();
//                Cop.WeaponInventory.SetDeadly(true);
//            }
//            CurrentTaskState = new SearchLocationOnFootTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToWalkTo, this, Cop);
//        }
//        else
//        {
//            CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest, Settings, BlockPermanentEvents);
//        }
//    }
//    protected virtual void UpdateVehicleState()
//    {
//        if (!Ped.IsInVehicle || !Ped.Pedestrian.Exists())
//        {
//            return;
//        }
//        if (CheckSiren && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && !Ped.Pedestrian.CurrentVehicle.IsSirenOn)
//        {
//            Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
//            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
//        }
//        if (SetDriverSkill)
//        {
//            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAbility);
//            NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAggressiveness);
//            if (Settings.SettingsManager.PoliceTaskSettings.DriverRacing > 0f)
//            {
//                NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverRacing);
//            }
//        }
//    }
//    private void CheckLocationChanged()
//    {
//        if (prevPlaceToDriveTo.DistanceTo(PlaceToDriveTo) <= 5.0f && prevPlaceToWalkTo.DistanceTo(PlaceToWalkTo) <= 5.0f)
//        {
//            LocationsChanged = false;
//            return;
//        }
//        HasReachedLocatePosition = false;
//        prevPlaceToDriveTo = PlaceToDriveTo;
//        prevPlaceToWalkTo = PlaceToWalkTo;
//        LocationsChanged = true;
//        EntryPoint.WriteToConsole($"{PedGeneral.Handle} General Locate, Search Place Changed");
//    }

//    protected virtual void GetLocations()
//    {
//        PlaceToDriveTo = HasSixthSense ? Player.StreetPlacePoliceShouldSearchForPlayer : Player.StreetPlacePoliceLastSeenPlayer;// Player.StreetPlacePoliceShouldSearchForPlayer;
//        PlaceToWalkTo = HasSixthSense ? Player.PlacePoliceShouldSearchForPlayer : Player.PlacePoliceLastSeenPlayer; //Player.PlacePoliceShouldSearchForPlayer;
//    }
//}

