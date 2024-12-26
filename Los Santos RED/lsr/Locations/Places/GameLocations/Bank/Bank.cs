using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Bank : GameLocation
{
    //private string drawerStealPromptGroup = "BankSteal";
    //private string drawerStealPromptIdentity = "BankStealDrawer";
    //private string drawerStealPromptText = "Steal from Drawer";
    //private string drawerStealEmptyText = "Drawer Empty";
    private BankInteraction BankInteraction;
    private List<Teller> SpawnedTellers = new List<Teller>();
    //private BankDrawer ClosestBankDrawer;
    //private bool IsCancelled;
    protected readonly List<string> FallBackTellerModels = new List<string>() { "s_f_m_shop_high", "s_f_y_airhostess_01", "s_m_m_highsec_01" };
    //private List<BankDrawer> BankDrawers = new List<BankDrawer>();
    //private bool IsStealingFromDrawer;
    public Bank(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string shortName) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ShortName = shortName;
    }
    public Bank() : base()
    {

    }
    public override string TypeName { get; set; } = "Bank";
    public override int MapIcon { get; set; } = (int)BlipSprite.Devin;
    public bool HasTellers => VendorLocations != null && VendorLocations.Any();
    public int DrawerCashMin { get; set; } = 500;
    public int DrawerCashMax { get; set; } = 3000;
    public int DrawerCashGainedPerAnimation { get; set; } = 500;
    public float ExtaTellerSpawnPercentage { get; set; } = 70f;
    public override string VendorPersonnelID => "TellerPeds";
    public string ShortName { get; set; }




    public override int VendorMoneyMin { get; set; } = 45;
    public override int VendorMoneyMax { get; set; } = 650;

    public override float VendorFightPercentage { get; set; } = 1f;
    public override float VendorCallPolicePercentage { get; set; } = 95f;
    public override float VendorCallPoliceForSeriousCrimesPercentage { get; set; } = 100f;
    public override float VendorFightPolicePercentage { get; set; } = 0f;
    public override float VendorCowerPercentage { get; set; } = 85f;
    public override int RacketeeringAmountMin { get; set; } = 10000;
    public override int RacketeeringAmountMax { get; set; } = 20000;

    [XmlIgnore]
    public BankInterior BankInterior { get; set; }


    [XmlIgnore]
    public override List<PedExt> SpawnedVendors
    {
        get
        {
            List<PedExt> toreturn = new List<PedExt>() { };
            toreturn.AddRange(SpawnedTellers);
            return toreturn;
        }
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Bank At {Name}";
        return true;
    }
    public override void OnInteract()
    {
        if (IsLocationClosed())
        {
            return;
        }
        if (!CanInteract)
        {
            return;
        }


        if (BankInterior != null && BankInterior.IsTeleportEntry)
        {
            DoEntranceCamera(false);
            BankInterior.SetBank(this);
            BankInterior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, true);
                CreateInteractionMenu();
                BankInteraction = new BankInteraction(Player, this);
                BankInteraction.Start(MenuPool, InteractionMenu, isInside);
                while (IsAnyMenuVisible)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                BankInteraction.Dispose();
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                //StoreCamera.Dispose();
                Player.IsTransacting = false;
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "BankInteract");
    }
    public override void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
    {
        //BankDrawers.Clear();
        if(HasTellers)
        {
            CanInteract = false;
        }
        base.Activate(interiors, settings, crimes, weapons, time, world);
    }

    public override void AttemptVendorSpawn(bool isOpen, IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world, bool isInterior)
    {
        int TellersSpawned = 0;
        //BankDrawers.Clear();
        List<SpawnPlace> spawns = new List<SpawnPlace>();
        if (isInterior)
        {
            if (Interior != null && Interior.VendorLocations != null && Interior.VendorLocations.Any())
            {
                spawns = Interior.VendorLocations.ToList();
            }
        }
        else
        {
            spawns = VendorLocations;
        }
        foreach (SpawnPlace spawnPlace in spawns)
        {
            if (IsOpen(time.CurrentHour) && settings.SettingsManager.CivilianSettings.ManageDispatching && world.Pedestrians.TotalSpawnedServiceWorkers < settings.SettingsManager.CivilianSettings.TotalSpawnedServiceMembersLimit && (TellersSpawned == 0 || RandomItems.RandomPercent(ExtaTellerSpawnPercentage)))
            {
                if (SpawnTeller(spawnPlace))
                {
                    TellersSpawned++;
                }
            }
        }
    }


    protected bool SpawnTeller(SpawnPlace spawnPlace)
    {
        //Ped ped;
        DispatchablePerson VendorPersonType = null;
        if (VendorPersonnel != null && VendorPersonnel.Any())
        {
            VendorPersonType = VendorPersonnel.PickRandom();
        }
        if (VendorPersonType == null)
        {
            VendorPersonType = new DispatchablePerson(FallBackVendorModels.PickRandom(), 100, 100);
        }
        HandleVariableItems();
        EntryPoint.WriteToConsole($"ATTEMPTING TELLER AT {Name} {VendorPersonType.ModelName}");
        SpawnedTellers = new List<Teller>();
        SpawnLocation sl = new SpawnLocation(spawnPlace.Position) { Heading = spawnPlace.Heading };
        TellerSpawnTask merchantSpawnTask = new TellerSpawnTask(sl, null, VendorPersonType, false, false, true, Settings, Crimes, Weapons, Names, World, ModItems, ShopMenus, this);
        merchantSpawnTask.AllowAnySpawn = true;
        merchantSpawnTask.SpawnWithAllWeapons = true;
        merchantSpawnTask.AllowBuddySpawn = false;
        merchantSpawnTask.AttemptSpawn();



        SpawnedTellers.AddRange(merchantSpawnTask.SpawnedTellers);

        return merchantSpawnTask.CreatedPeople.Any();
    }

    public override void AttemptVendorDespawn()
    {
        foreach (PedExt pedExt in SpawnedTellers.ToList())
        {
            if (pedExt.Pedestrian.Exists())
            {
                pedExt.DeleteBlip();
                pedExt.Pedestrian.IsPersistent = false;
                EntryPoint.WriteToConsole($"AttemptVendorDespawn MADE NON PERSIST TELLER");
            }
        }
        SpawnedTellers.Clear();
        SpawnedVendors.Clear();
    }


    //private bool SpawnTeller(ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, SpawnPlace spawnPlace)
    //{
    //    //Ped ped;
    //    string ModelName;
    //    if (TellerModels != null && TellerModels.Any())
    //    {
    //        ModelName = TellerModels.PickRandom();
    //    }
    //    else
    //    {
    //        ModelName = FallBackTellerModels.PickRandom();
    //    }





    //    EntryPoint.WriteToConsole($"ATTEMPTING TELLER AT {Name} {ModelName}");
    //    NativeFunction.Natives.CLEAR_AREA(spawnPlace.Position.X, spawnPlace.Position.Y, spawnPlace.Position.Z, 2f, true, false, false, false);
    //    World.Pedestrians.CleanupAmbient();
    //    Ped ped = new Ped(ModelName, spawnPlace.Position, spawnPlace.Heading);
    //    GameFiber.Yield();
    //    if (!ped.Exists())
    //    {
    //        return false;
    //    }
    //    EntryPoint.SpawnedEntities.Add(ped);
    //    Teller teller = new Teller(ped, settings, "Teller", crimes, weapons, World);
    //    if (!World.Pedestrians.Tellers.Any(x => x.Handle == ped.Handle))
    //    {
    //        World.Pedestrians.Tellers.Add(teller);
    //    }
    //    teller.AssociatedBank = this;
    //    teller.SpawnPosition = spawnPlace.Position;
    //    teller.WasModSpawned = true;
    //    teller.WillCower = true;
    //    ped.IsPersistent = true;//THIS IS ON FOR NOW!
    //    ped.RandomizeVariation();
    //    teller.LocationTaskRequirements = new LocationTaskRequirements() { TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_STAND_IMPATIENT" } };
    //    GameFiber.Yield();
    //    if (!ped.Exists())
    //    {
    //        return false;
    //    }
    //    SpawnedTellers.Add(teller);
    //    EntryPoint.WriteToConsole($"SPAWNED WORKED TELLER AT {Name}");
    //    return true;
    //}
    public override void Deactivate(bool deleteBlip)
    {
        //foreach(Teller merchant in SpawnedTellers)//handled in location dispatcher now 
        //{
        //    if (merchant != null && merchant.Pedestrian.Exists())
        //    {
        //        merchant.DeleteBlip();
        //        merchant.Pedestrian.Delete();
        //    }
        //}
        base.Deactivate(deleteBlip);
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups, IEntityProvideable world, IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors, ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople)
    {
        if (HasInterior)
        {
            BankInterior = interiors.PossibleInteriors.BankInteriors.Where(x => x.LocalID == InteriorID).FirstOrDefault();
            interior = BankInterior;
            if (BankInterior != null)
            {
                BankInterior.SetBank(this);
            }
        }
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople);
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.Banks.Add(this);
        base.AddLocation(possibleLocations);
    }

}

