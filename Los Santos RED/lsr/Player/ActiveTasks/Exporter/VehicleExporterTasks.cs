using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.ActiveTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleExporterTasks : IPlayerTaskGroup
{

    private ITaskAssignable Player;
    private ITimeReportable Time;
    private IGangs Gangs;
    private PlayerTasks PlayerTasks;
    private IPlacesOfInterest PlacesOfInterest;
    private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
    private ISettingsProvideable Settings;
    private IEntityProvideable World;
    private ICrimes Crimes;
    public TansferStolenCar TansferStolenCar { get; private set; }
    private List<IPlayerTask> PlayerTaskList = new List<IPlayerTask>();
    public VehicleExporterTasks(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes)
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
        TansferStolenCar = new TansferStolenCar(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes);
        PlayerTaskList = new List<IPlayerTask>
        {
            TansferStolenCar
        };
    }
    public void Setup()
    {
        foreach(IPlayerTask playerTask in PlayerTaskList)
        {
            playerTask.Setup();
        }
    }
    public void Dispose()
    {
        foreach (IPlayerTask playerTask in PlayerTaskList)
        {
            playerTask.Dispose();
        }
    }

}