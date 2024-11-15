//using LSR.Vehicles;
//using Rage.Native;
//using Rage;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using LosSantosRED.lsr.Interface;

//public class TaxiDropOff
//{
//    private Vector3 Position;
//    private ISettingsProvideable Settings;
//    private ICrimes Crimes;
//    private IWeapons Weapons;
//    private INameProvideable Names;
//    private IEntityProvideable World;
//    private IModItems ModItems;
//    private IShopMenus ShopMenus;
//    public TaxiDropOff(Vector3 position, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, INameProvideable names, IEntityProvideable world, IModItems modItems, IShopMenus shopMenus)
//    {
//        Position = position;
//        Settings = settings;
//        Crimes = crimes;    
//        Weapons = weapons;  
//        Names = names;
//        World = world;
//        ModItems = modItems;
//        ShopMenus = shopMenus;
//    }
//    public void Setup()
//    {

//    }
//    public void Start()
//    {
//        SpawnLocation taxiSpawn = new SpawnLocation(Position);
//        taxiSpawn.GetClosestStreet(true);
//        DispatchableVehicle taxiVehicle = new DispatchableVehicle("taxi", 100, 100);
//        DispatchablePerson taxiPed = new DispatchablePerson("a_m_m_socenlat_01", 100, 100);
//        if (taxiSpawn.StreetPosition != null)
//        {
//            CivilianSpawnTask civilianSpawnTask = new CivilianSpawnTask(taxiSpawn, taxiVehicle, taxiPed, false, false, true, Settings, Crimes, Weapons, Names, World, ModItems, ShopMenus);
//            civilianSpawnTask.AllowAnySpawn = true;
//            civilianSpawnTask.AllowBuddySpawn = false;
//            civilianSpawnTask.AttemptSpawn();
//            civilianSpawnTask.CreatedPeople.ForEach(x => World.Pedestrians.AddEntity(x));
//            civilianSpawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.None));
//            PedExt spawnedDriver = civilianSpawnTask.CreatedPeople.FirstOrDefault();
//            VehicleExt spawnedTaxi = civilianSpawnTask.CreatedVehicles.FirstOrDefault();
//            if (spawnedDriver != null && spawnedDriver.Pedestrian.Exists() && spawnedDriver.Pedestrian.CurrentVehicle.Exists())
//            {
//                spawnedDriver.CanBeTasked = true;
//                spawnedDriver.CanBeAmbientTasked = true;
//                NativeFunction.Natives.TASK_VEHICLE_DRIVE_WANDER(spawnedDriver.Pedestrian, spawnedDriver.Pedestrian.CurrentVehicle, 10f, (int)eCustomDrivingStyles.RegularDriving, 10f);
//            }
//            GameFiber.StartNew(delegate
//            {
//                try
//                {
//                    GameFiber.Sleep(15000);
//                    if (spawnedDriver != null && spawnedDriver.Pedestrian.Exists())
//                    {
//                        spawnedDriver.Pedestrian.IsPersistent = false;
//                    }
//                    if (spawnedTaxi != null && spawnedTaxi.Vehicle.Exists())
//                    {
//                        spawnedTaxi.Vehicle.IsPersistent = false;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
//                    EntryPoint.ModController.CrashUnload();
//                }
//            }, "HotelInteract");
//        }
//    }
//}

