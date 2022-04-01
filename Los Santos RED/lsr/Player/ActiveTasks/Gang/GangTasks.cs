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

    public RivalGangHitTask RivalGangHit { get; private set; }
    public PayoffGangTask PayoffGangTask { get; private set; }
    public RivalGangTheftTask RivalGangTheftTask { get; private set; }
    public GangPickupTask GangPickupTask { get; private set; }
    public GangDeliveryTask GangDeliveryTask { get; private set; }
    public GangWheelmanTask GangWheelmanTask { get; private set; }
    public GangPizzaDeliveryTask GangPizzaDeliveryTask { get; private set; }

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
        RivalGangHit = new RivalGangHitTask(Player,Time,Gangs,PlayerTasks,PlacesOfInterest,ActiveDrops,Settings,World,Crimes);
        PayoffGangTask = new PayoffGangTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes);
        RivalGangTheftTask = new RivalGangTheftTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes);
        GangPickupTask = new GangPickupTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes);
        GangDeliveryTask = new GangDeliveryTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes, ModItems, shopMenus);
        GangWheelmanTask = new GangWheelmanTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes, weapons,names, pedGroups, shopMenus);
        GangPizzaDeliveryTask = new GangPizzaDeliveryTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes, ModItems, shopMenus);
    }
    public void Setup()
    {
        RivalGangHit.Setup();
        PayoffGangTask.Setup();
        RivalGangTheftTask.Setup();
        GangPickupTask.Setup();
        GangDeliveryTask.Setup();
        GangWheelmanTask.Setup();
        GangPizzaDeliveryTask.Setup();
    }
    public void Dispose()
    {
        RivalGangHit.Dispose();
        PayoffGangTask.Dispose();
        RivalGangTheftTask.Dispose();
        GangPickupTask.Dispose();
        GangDeliveryTask.Dispose();
        GangWheelmanTask.Dispose();
        GangPizzaDeliveryTask.Dispose();
    }
}

