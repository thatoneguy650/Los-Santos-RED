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
    private INameProvideable Names;

    private List<IPlayerTask> AllTasks = new List<IPlayerTask>();

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
        Names = names;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {
        AllTasks.ForEach(x => x.Dispose());
        AllTasks.Clear();
    }
    public void StartWitnessEliminationTask(CorruptCopContact contact)
    {
        WitnessEliminationTask WitnessEliminationTask = new WitnessEliminationTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes, Names, Weapons, ShopMenus);
        AllTasks.Add(WitnessEliminationTask);
        WitnessEliminationTask.Setup();
        WitnessEliminationTask.Start(contact);
    }
    public void StartCopHitTask(CorruptCopContact contact)
    {
        CopHitTask CopHitTask = new CopHitTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes, Names, Weapons);
        AllTasks.Add(CopHitTask);
        CopHitTask.Setup();
        CopHitTask.Start(contact);
    }
    public void StartCopGangHitTask(CorruptCopContact contact)
    {
        CopGangHitTask CopGangHitTask = new CopGangHitTask(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes);
        AllTasks.Add(CopGangHitTask);
        CopGangHitTask.Setup();
        CopGangHitTask.Start(contact);
    }
}

