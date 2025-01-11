using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class RivalGangVehicleTheftTask : IPlayerTask
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
        private Gang HiringGang;
        private GangDen HiringGangDen;
        private Gang TargetGang;
        private PlayerTask CurrentTask;
        private int MoneyToRecieve;
        private PhoneContact PhoneContact;
        private GangTasks GangTasks;
        private uint VehicleToStealHash;
        private string VehicleModelName;
        private string VehicleDisplayName;
        private bool HasTargetGangVehicleAndHiringDen => TargetGang != null && HiringGangDen != null && VehicleToStealHash != 0;
        private bool IsInStolenGangCar => Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.Vehicle.Model.Hash == VehicleToStealHash && Player.CurrentVehicle.WasModSpawned && Player.CurrentVehicle.AssociatedGang != null && Player.CurrentVehicle.AssociatedGang.ID == TargetGang.ID;
        public RivalGangVehicleTheftTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes,
            PhoneContact phoneContact, GangTasks gangTasks, Gang targetGang, string vehicleModelName, string vehicleDisplayName)
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
            PhoneContact = phoneContact;
            GangTasks = gangTasks;
            TargetGang = targetGang;
            VehicleModelName = vehicleModelName;
            VehicleDisplayName = vehicleDisplayName;
        }
        public void Setup()
        {
            
        }
        public void Dispose()
        {

        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            if (PlayerTasks.CanStartNewTask(ActiveGang?.ContactName))
            {
                GetTargetGang();
                GetHiringDen();
                if (HasTargetGangVehicleAndHiringDen)
                {
                    GetPayment();
                    SendInitialInstructionsMessage();
                    AddTask();
                    GameFiber PayoffFiber = GameFiber.StartNew(delegate
                    {
                        try
                        {
                            Loop();
                            FinishTask();
                        }
                        catch (Exception ex)
                        {
                            EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                            EntryPoint.ModController.CrashUnload();
                        }
                    }, "PayoffFiber");
                }
                else
                {
                    GangTasks.SendGenericTooSoonMessage(PhoneContact);
                }
            }
        }
        private void Loop()
        {
            while (true)
            {
                CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
                if (CurrentTask == null || !CurrentTask.IsActive)
                {
                    break;
                }
                if (IsInStolenGangCar)
                {
                    CurrentTask.OnReadyForPayment(true);
                    break;
                }
                GameFiber.Sleep(1000);
            }
        }
        private void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                //GameFiber.Sleep(RandomItems.GetRandomNumberInt(5000, 15000));
                GangTasks.SendGenericPickupMoneyMessage(PhoneContact, HiringGang.DenName, HiringGangDen, MoneyToRecieve);
            }
        }
        private void GetTargetGang()
        {
            VehicleToStealHash = 0;
            VehicleToStealHash = Game.GetHashKey(VehicleModelName);
            EntryPoint.WriteToConsole($"VehicleSteal {VehicleDisplayName} {VehicleModelName} {VehicleToStealHash} {TargetGang?.ID}");
        }
        private void GetHiringDen()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded, Player.CurrentLocation);
        }
        private void GetPayment()
        {
            MoneyToRecieve = RandomItems.GetRandomNumberInt(HiringGang.TheftPaymentMin, HiringGang.TheftPaymentMax).Round(500);
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
        }
        private void AddTask()
        {
            PlayerTasks.AddTask(HiringGang.Contact, MoneyToRecieve, 1000, 0, -500, 5, "Auto Theft for Gang");
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                    $"Go steal a ~p~{VehicleDisplayName}~s~ from those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ assholes. Once you are done come back to {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. ${MoneyToRecieve} on completion",
                    $"Go get me a ~p~{VehicleDisplayName}~s~ with {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ gang colors. Bring it back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. Payment ${MoneyToRecieve}",
                    };
            Player.CellPhone.AddPhoneResponse(HiringGang.Contact.Name, HiringGang.Contact.IconName, Replies.PickRandom());
        }
    }
}
