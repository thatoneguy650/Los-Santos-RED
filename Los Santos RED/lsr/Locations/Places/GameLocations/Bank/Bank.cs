using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Bank : GameLocation
{
    private BankInteraction BankInteraction;
    private List<Teller> SpawnedTellers = new List<Teller>();
    protected readonly List<string> FallBackTellerModels = new List<string>() { "s_f_m_shop_high", "s_f_y_airhostess_01", "s_m_m_highsec_01" };
    public Bank(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public Bank() : base()
    {

    }
    public override string TypeName { get; set; } = "Bank";
    public override int MapIcon { get; set; } = (int)BlipSprite.Devin;
    public List<SpawnPlace> TellerLocations { get; set; } = new List<SpawnPlace>();
    public List<string> TellerModels { get; set; } = new List<string>();
    public bool HasTellers => TellerLocations != null && TellerLocations.Any();
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Bank At {Name}";
        return true;
    }

    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
        if (IsLocationClosed())
        {
            return;
        }
        if (!CanInteract)
        {
            return;
        }
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                StoreCamera = new LocationCamera(this, Player, Settings);
                StoreCamera.Setup();
                CreateInteractionMenu();
                BankInteraction = new BankInteraction(Player, this);
                BankInteraction.Start(MenuPool, InteractionMenu);
                while (IsAnyMenuVisible)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                DisposeInteractionMenu();
                StoreCamera.Dispose();
                Player.IsTransacting = false;
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "BarInteract");
    }



    public override void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
    {
        foreach(SpawnPlace spawnPlace in TellerLocations)
        {
            SpawnTeller(settings,crimes,weapons,spawnPlace);
        }
        if(HasTellers)
        {
            CanInteract = false;
        }
        base.Activate(interiors, settings, crimes, weapons, time, world);
    }
    protected void SpawnTeller(ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, SpawnPlace spawnPlace)
    {
        Ped ped;
        string ModelName;
        if (TellerModels != null && TellerModels.Any())
        {
            ModelName = TellerModels.PickRandom();
        }
        else
        {
            ModelName = FallBackTellerModels.PickRandom();
        }
        NativeFunction.Natives.CLEAR_AREA(spawnPlace.Position.X, spawnPlace.Position.Y, spawnPlace.Position.Z, 2f, true, false, false, false);
        Model modelToCreate = new Model(Game.GetHashKey(ModelName));
        modelToCreate.LoadAndWait();
        ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), spawnPlace.Position.X, spawnPlace.Position.Y, spawnPlace.Position.Z, spawnPlace.Heading, false, false);//ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), VendorPosition.X, VendorPosition.Y, VendorPosition.Z + 1f, VendorHeading, false, false);
        GameFiber.Yield();
        if (!ped.Exists())
        {
            return;
        }
        ped.IsPersistent = true;//THIS IS ON FOR NOW!
        ped.RandomizeVariation();
        NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", ped, "WORLD_HUMAN_STAND_IMPATIENT", 0, true);
        ped.KeepTasks = true;
        EntryPoint.SpawnedEntities.Add(ped);
        GameFiber.Yield();
        if (!ped.Exists())
        {
            return;
        }
        Teller teller = new Teller(ped, settings, "Teller", crimes, weapons, World);
        teller.AssociatedBank = this;
        teller.SpawnPosition = spawnPlace.Position;
        teller.WasModSpawned = true;

        float resultArg = ped.Position.Z;
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(ped.Position.X, ped.Position.Y, ped.Position.Z + 1f, out resultArg, false))
        {
            ped.Position = new Vector3(ped.Position.X, ped.Position.Y, resultArg);
        }



        World.Pedestrians.AddEntity(teller);
        SpawnedTellers.Add(teller);
    }
    public override void Deactivate(bool deleteBlip)
    {
        foreach(Teller merchant in SpawnedTellers)
        {
            if (merchant != null && merchant.Pedestrian.Exists())
            {
                merchant.Pedestrian.Delete();
            }
        }
        base.Deactivate(deleteBlip);
    }
}

