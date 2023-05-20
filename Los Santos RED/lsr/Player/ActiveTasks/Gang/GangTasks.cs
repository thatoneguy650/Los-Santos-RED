using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.ActiveTasks;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangTasks
{

    private ITaskAssignable Player;
    private ITimeControllable Time;
    private IGangs Gangs;
    private PlayerTasks PlayerTasks;
    private IPlacesOfInterest PlacesOfInterest;
    private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
    private ISettingsProvideable Settings;
    private IEntityProvideable World;
    private ICrimes Crimes;
    private IModItems ModItems;
    private IShopMenus ShopMenus;
    private INameProvideable Names;
    private IWeapons Weapons;
    private IPedGroups PedGroups;

    private List<RivalGangHitTask> RivalGangHits = new List<RivalGangHitTask>();
    private List<PayoffGangTask> PayoffGangTasks = new List<PayoffGangTask>();
    private List<RivalGangTheftTask> RivalGangTheftTasks = new List<RivalGangTheftTask>();
    private List<GangPickupTask> GangPickupTasks = new List<GangPickupTask>();
    private List<GangDeliveryTask> GangDeliveryTasks = new List<GangDeliveryTask>();
    private List<GangWheelmanTask> GangWheelmanTasks = new List<GangWheelmanTask>();
    private List<GangPizzaDeliveryTask> GangPizzaDeliveryTasks = new List<GangPizzaDeliveryTask>();
    private List<GangProveWorthTask> GangProveWorthTasks = new List<GangProveWorthTask>();

    public GangTasks(ITaskAssignable player, ITimeControllable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, IModItems modItems, IShopMenus shopMenus, IWeapons weapons, INameProvideable names, IPedGroups pedGroups)
    {
        Player = player;
        Time = time;
        Gangs = gangs;
        PlayerTasks = playerTasks;
        PlacesOfInterest = placesOfInterest;
        ActiveDrops = activeDrops;
        Settings = settings;
        World = world;
        Crimes = crimes;
        ModItems = modItems;
        ShopMenus = shopMenus;
        Names = names;
        Weapons = weapons;
        PedGroups = pedGroups;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {    
        RivalGangHits.ForEach(x=> x.Dispose());
        PayoffGangTasks.ForEach(x => x.Dispose());
        RivalGangTheftTasks.ForEach(x => x.Dispose());
        GangPickupTasks.ForEach(x => x.Dispose());
        GangDeliveryTasks.ForEach(x => x.Dispose());
        GangWheelmanTasks.ForEach(x => x.Dispose());
        GangPizzaDeliveryTasks.ForEach(x => x.Dispose());
        GangProveWorthTasks.ForEach(x => x.Dispose());

        RivalGangHits.Clear();
        PayoffGangTasks.Clear();
        RivalGangTheftTasks.Clear();
        GangPickupTasks.Clear();
        GangDeliveryTasks.Clear();
        GangWheelmanTasks.Clear();
        GangPizzaDeliveryTasks.Clear();
        GangProveWorthTasks.Clear();
    }
    public void StartGangProveWorth(Gang gang, int killRequirement)
    {
        GangProveWorthTask newTask = new GangProveWorthTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes);
        newTask.KillRequirement = killRequirement;
        newTask.JoinGangOnComplete = true;
        GangProveWorthTasks.Add(newTask);
        newTask.Setup();
        newTask.Start(gang);
    }
    public void StartGangHit(Gang gang, int killRequirement)
    {
        RivalGangHitTask newTask = new RivalGangHitTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes);
        newTask.KillRequirement = killRequirement;
        RivalGangHits.Add(newTask);
        newTask.Setup();
        newTask.Start(gang);
    }
    public void StartPayoffGang(Gang gang)
    {
        PayoffGangTask newTask = new PayoffGangTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes);
        PayoffGangTasks.Add(newTask);
        newTask.Setup();
        newTask.Start(gang);
    }
    public void StartGangTheft(Gang gang)
    {
        RivalGangTheftTask newTask = new RivalGangTheftTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes);
        RivalGangTheftTasks.Add(newTask);
        newTask.Setup();
        newTask.Start(gang);
    }
    public void StartGangPickup(Gang gang)
    {
        GangPickupTask newTask = new GangPickupTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes);
        GangPickupTasks.Add(newTask);
        newTask.Setup();
        newTask.Start(gang);
    }
    public void StartGangDelivery(Gang gang)
    {
        GangDeliveryTask newTask = new GangDeliveryTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes, ModItems, ShopMenus);
        GangDeliveryTasks.Add(newTask);
        newTask.Setup();
        newTask.Start(gang);
    }
    public void StartGangWheelman(Gang gang)
    {
        GangWheelmanTask newTask = new GangWheelmanTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes, Weapons, Names, PedGroups, ShopMenus, ModItems);
        GangWheelmanTasks.Add(newTask);
        newTask.Setup();
        newTask.Start(gang);
    }
    public void StartGangPizza(Gang gang)
    {
        GangPizzaDeliveryTask newDelivery = new GangPizzaDeliveryTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes, ModItems, ShopMenus);
        GangPizzaDeliveryTasks.Add(newDelivery);
        newDelivery.Setup();
        newDelivery.Start(gang);
    }
}

