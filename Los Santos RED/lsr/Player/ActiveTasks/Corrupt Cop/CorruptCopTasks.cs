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


public class CorruptCopTasks : IPlayerTaskGroup
{

    private ITaskAssignable Player;
    private ITimeReportable Time;
    private IGangs Gangs;
    private PlayerTasks PlayerTasks;
    private IPlacesOfInterest PlacesOfInterest;
    private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
    private ISettingsProvideable Settings;
    private PlayerTask CurrentTask;
    private IEntityProvideable World;
    private ICrimes Crimes;
    private IWeapons Weapons;
    private IShopMenus ShopMenus;
    public WitnessEliminationTask WitnessEliminationTask { get; private set; }
    public CopGangHitTask CopGangHitTask { get; private set; }
    public CopHitTask CopHitTask { get; private set; }
    public CorruptCopTasks(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, INameProvideable names, IWeapons weapons, IShopMenus shopMenus)
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
        Weapons = weapons;
        ShopMenus = shopMenus;
        WitnessEliminationTask = new WitnessEliminationTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes, names, Weapons, ShopMenus);
        CopGangHitTask = new CopGangHitTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes);
        CopHitTask = new CopHitTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes, names, Weapons);
    }
    public void Setup()
    {
        WitnessEliminationTask.Setup();
        CopGangHitTask.Setup();
        CopHitTask.Setup();
    }
    public void Dispose()
    {
        WitnessEliminationTask.Dispose();
        CopGangHitTask.Dispose();
        CopHitTask.Dispose();
    }

}

