//using LosSantosRED.lsr.Helper;
//using LosSantosRED.lsr.Interface;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//public class FakeCustomizeInterior
//{
//    private Rage.Object chairProp;
//    private Vector3 ChairPosition;
//    private float ChairHeading;


//    private Vector3 PlayerTelePosition;
//    private float PlayerTeleHeading;

//    private Vector3 BarberPosition;
//    private float BarberHeading;

//    private BarberShop BarberShop;
//    private PedExt SpawnedBarber;

//    private Vector3 AnimEnterPosition;
//    private Vector3 AnimEnterRotation;
//    private Vector3 PreEnterPlayerPosition;
//    private float PreEnterPlayerHeading;

//    private Vector3 CameraPosition;
//    private Vector3 CameraDirection;
//    private Rotator CameraRotation;

//    private ILocationInteractable Player;
//    private ISettingsProvideable Settings;
//    private IModItems ModItems;
//    private IClothesNames ClothesNames;
//    private IEntityProvideable World;


//    public FakeCustomizeInterior(ILocationInteractable player,IEntityProvideable world, ISettingsProvideable settings,IModItems modItems, IClothesNames clothesNames,BarberShop barberShop,Vector3 animEnterPosition,Vector3 animEnterRotation, 
//        Vector3 chairPosition, float chairHeading, 
//        Vector3 playerTelePosition, float playerTeleHeading, 
//        Vector3 barberPosition, float barberHeading,
//        Vector3 cameraPosition, Vector3 cameraDirection, Rotator cameraRotation
//        )
//    {
//        Player = player;
//        Settings = settings;
//        ChairPosition = chairPosition;
//        ChairHeading = chairHeading;
//        PlayerTelePosition = playerTelePosition;
//        PlayerTeleHeading = playerTeleHeading;
//        BarberPosition = barberPosition;
//        BarberHeading = barberHeading;
//        ModItems = modItems;
//        ClothesNames = clothesNames;
//        BarberShop = barberShop;
//        AnimEnterPosition = animEnterPosition;
//        AnimEnterRotation = animEnterRotation;
//        CameraPosition = cameraPosition;
//        CameraDirection = cameraDirection;
//        CameraRotation = cameraRotation;
//        World = world;
//    }

//    public void Start()
//    {
//        PreEnterPlayerPosition = Player.Position;
//        PreEnterPlayerHeading = Player.Character.Heading;
//        Game.FadeScreenOut(1000, true);
//        SetPlayerAtLocation();
//        SpawnBarber();
//        SpawnChair();
//        HaircutInteract haircutInteract = new HaircutInteract("generatedHaircut1", ChairPosition, ChairHeading, "Get Haircut");
//        haircutInteract.AnimEnterPosition = AnimEnterPosition;
//        haircutInteract.AnimEnterRotation = AnimEnterRotation;
//        haircutInteract.CameraPosition = CameraPosition;
//        haircutInteract.CameraDirection = CameraDirection;
//        haircutInteract.CameraRotation = CameraRotation;
//        haircutInteract.BarberShop = BarberShop;
//        haircutInteract.SetupFake(Player,Settings,BarberShop,Player);
//        haircutInteract.Setup(ModItems, ClothesNames);
//        haircutInteract.SetStylist(SpawnedBarber);
//        GameFiber.Sleep(2000);
//        //Game.FadeScreenIn(500, false);
//        Player.ActivityManager.IsInteractingWithLocation = true;
//        Player.IsTransacting = true;
//        haircutInteract.WaitForCameraReturn = false;
//        haircutInteract.OnInteract();
//        Exit();



//    }

//    private void SpawnBarber()
//    {
//        Ped ped = new Ped("s_f_m_fembarber", BarberPosition, BarberHeading);
//        GameFiber.Yield();
//        if (ped.Exists())
//        {
//            ped.IsPersistent = true;
//            SpawnedBarber = new Merchant(ped, Settings, "Barber", null, null, World);
//            World.Pedestrians.AddEntity(SpawnedBarber);
//            SpawnedBarber.WasEverSetPersistent = true;
//            SpawnedBarber.CanBeAmbientTasked = false;
//            SpawnedBarber.CanBeTasked = false;
//            SpawnedBarber.WasModSpawned = true;
//            SpawnedBarber.IsManuallyDeleted = true;
//            EntryPoint.WriteToConsole("BARBER SPAWNED!");
//        }
//    }

//    public void Exit()
//    {
//        Player.ActivityManager.IsInteractingWithLocation = false;
//        Player.IsTransacting = false;
//        EntryPoint.WriteToConsole("EXIT START!");
//        Game.FadeScreenOut(1000, true);
//        SetPlayerAtPreEnter();
//        if(SpawnedBarber != null && SpawnedBarber.Pedestrian.Exists())
//        {
//            SpawnedBarber.Pedestrian.Delete();
//        }
//        if (chairProp.Exists())
//        {
//            chairProp.Delete();
//        }
//        GameFiber.Sleep(1000);
//        Game.FadeScreenIn(1000, false);
//    }
//    private void SpawnChair()
//    {
//        if (chairProp.Exists())
//        {
//            chairProp.Delete();
//        }
//        chairProp = new Rage.Object("vw_prop_casino_track_chair_01", ChairPosition, ChairHeading);// 239.2449f);
//        if (chairProp.Exists())
//        {
//            NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(chairProp);
//        }
//    }
//    private void SetPlayerAtLocation()
//    {
//        Player.Character.Position = PlayerTelePosition;
//        Player.Character.Heading = PlayerTeleHeading;
//    }
//    private void SetPlayerAtPreEnter()
//    {
//        Player.Character.Position = PreEnterPlayerPosition;
//        Player.Character.Heading = PreEnterPlayerHeading;
//    }
//}

