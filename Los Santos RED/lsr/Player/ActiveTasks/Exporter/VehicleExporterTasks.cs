using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.ActiveTasks;
using RAGENativeUI;
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
    private IModItems ModItems;

    private List<IPlayerTask> AllTasks = new List<IPlayerTask>();
    public VehicleExporterTasks(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, IModItems modItems)
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
    }
    public void Setup()
    {

    }
    public void Dispose()
    {
        AllTasks.ForEach(x => x.Dispose());
        AllTasks.Clear();
    }
    public void StartTansferStolenCarTask(VehicleExporterContact contact)
    {
        TansferStolenCar tansferStolenCar = new TansferStolenCar(Player, Time, Gangs, PlayerTasks, PlacesOfInterest, ActiveDrops, Settings, World, Crimes, ModItems, contact);
        AllTasks.Add(tansferStolenCar);
        tansferStolenCar.Setup();
        tansferStolenCar.Start();
    }

    public void OnInteractionMenuCreated(GameLocation gameLocation, MenuPool menuPool, UIMenu interactionMenu)
    {

    }
}