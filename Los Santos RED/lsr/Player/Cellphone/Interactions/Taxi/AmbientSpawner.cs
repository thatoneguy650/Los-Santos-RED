//using LSR.Vehicles;
//using Rage.Native;
//using Rage;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using LosSantosRED.lsr.Interface;

//public class AmbientSpawner
//{
//    private Vector3 Position;
//    private ISettingsProvideable Settings;
//    private ICrimes Crimes;
//    private IWeapons Weapons;
//    private INameProvideable Names;
//    private IEntityProvideable World;
//    private IModItems ModItems;
//    private IShopMenus ShopMenus;
//    private DispatchableVehicle DispatchableVehicle;
//    private DispatchablePerson DispatchablePerson;
//    private PedExt spawnedDriver;
//    private VehicleExt spawnedVehicle;
//    private SpawnLocation spawnPosition;
//    private SpawnLocation hirePosition;
//    public bool SetPersistent { get; set; } = false;
//    public bool SpawnedItems { get; private set; } = false;

//    public AmbientSpawner(DispatchableVehicle dispatchableVehicle, DispatchablePerson dispatchablePerson, Vector3 position, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, INameProvideable names, IEntityProvideable world, IModItems modItems, IShopMenus shopMenus)
//    {
//        DispatchableVehicle = dispatchableVehicle;
//        DispatchablePerson = dispatchablePerson;
//        Position = position;
//        Settings = settings;
//        Crimes = crimes;
//        Weapons = weapons;
//        Names = names;
//        World = world;
//        ModItems = modItems;
//        ShopMenus = shopMenus;
//    }
//    public void Start()
//    {
//        GetSpawnPosition();
//        if (spawnPosition.StreetPosition == null)
//        {
//            return;
//        }
//        SpawnItems();
//        TaskPeds();
//        SetUnpersistentTimed();
//    }
//    private void GetSpawnPosition()
//    {
//        hirePosition = new SpawnLocation(Position);
//        hirePosition.GetClosestStreet(true);
//        spawnPosition = new SpawnLocation(Position.Around2D(250f, 550f));
//        spawnPosition.GetClosestStreet(true);
//    }
//    private void SpawnItems()
//    {
//        CivilianSpawnTask civilianSpawnTask = new CivilianSpawnTask(spawnPosition, DispatchableVehicle, DispatchablePerson, false, false, SetPersistent, Settings, Crimes, Weapons, Names, World, ModItems, ShopMenus);
//        civilianSpawnTask.AllowAnySpawn = true;
//        civilianSpawnTask.AllowBuddySpawn = false;
//        civilianSpawnTask.AttemptSpawn();
//        civilianSpawnTask.CreatedPeople.ForEach(x => World.Pedestrians.AddEntity(x));
//        civilianSpawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));
//        spawnedDriver = civilianSpawnTask.CreatedPeople.FirstOrDefault();
//        spawnedVehicle = civilianSpawnTask.CreatedVehicles.FirstOrDefault();
//        spawnedVehicle?.AddBlip();
//        if(spawnedDriver != null && spawnedDriver.Pedestrian.Exists() && spawnedVehicle != null && spawnedVehicle.Vehicle.Exists())
//        {
//            SpawnedItems = true;
//        }
//    }
//    private void TaskPeds()
//    {
//        if (spawnedDriver != null && spawnedDriver.Pedestrian.Exists() && spawnedDriver.Pedestrian.CurrentVehicle.Exists())
//        {
//            spawnedDriver.CanBeTasked = true;
//            spawnedDriver.CanBeAmbientTasked = true;
//            //NativeFunction.Natives.TASK_VEHICLE_DRIVE_WANDER(spawnedDriver.Pedestrian, spawnedDriver.Pedestrian.CurrentVehicle, 10f, (int)eCustomDrivingStyles.RegularDriving, 10f);
//            unsafe
//            {
//                int lol = 0;
//                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", 0, spawnedDriver.Pedestrian.CurrentVehicle, hirePosition.StreetPosition.X, hirePosition.StreetPosition.Y, hirePosition.StreetPosition.Z, 10f, (int)eCustomDrivingStyles.RegularDriving, 10f);
//                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(8000, 15000));
//                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, spawnedDriver.Pedestrian.CurrentVehicle, 10f, (int)eCustomDrivingStyles.RegularDriving, 10f);
//                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
//                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", spawnedDriver.Pedestrian, lol);
//                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//            }
//            //NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(spawnedDriver.Pedestrian, spawnedDriver.Pedestrian.CurrentVehicle, hirePosition.StreetPosition.X, hirePosition.StreetPosition.Y, hirePosition.StreetPosition.Z, 10f, (int)eCustomDrivingStyles.RegularDriving, 10f); //30f speed
//        }
//    }
//    private void SetUnpersistentTimed()
//    {
//        if(!SetPersistent)
//        {
//            return;
//        }
//        GameFiber.StartNew(delegate
//        {
//            try
//            {
//                while(true)
//                {
//                    if (spawnedDriver == null || !spawnedDriver.Pedestrian.Exists() || spawnedVehicle == null || !spawnedVehicle.Vehicle.Exists())
//                    {
//                        EntryPoint.WriteToConsole("Spawned Driver Disappeared");
//                        break;
//                    }
//                    if(spawnedDriver.RecentlyUpdated && spawnedDriver.ClosestDistanceToPlayer <= 30f && spawnedDriver.DistanceToPlayer >= 150f)
//                    {
//                        EntryPoint.WriteToConsole("Spawned Driver Got Close then far");
//                        break;
//                    }
//                    if(spawnedDriver.RecentlyUpdated && spawnedDriver.DistanceToPlayer >= 600f)
//                    {
//                        EntryPoint.WriteToConsole("Spawned Driver IS FAR");
//                        break;
//                    }
//                    GameFiber.Sleep(1000);
//                }
//               // GameFiber.Sleep(15000);
//                if (spawnedDriver != null && spawnedDriver.Pedestrian.Exists())
//                {
//                    spawnedDriver.Pedestrian.IsPersistent = false;
//                }
//                if (spawnedVehicle != null && spawnedVehicle.Vehicle.Exists())
//                {
//                    spawnedVehicle.RemoveBlip();
//                    spawnedVehicle.Vehicle.IsPersistent = false;
//                }
//                EntryPoint.WriteToConsole("SET TAXI NON PERSIST");
//            }
//            catch (Exception ex)
//            {
//                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
//                EntryPoint.ModController.CrashUnload();
//            }
//        }, "AmbientSpawnerInteract");
//    }
//}
