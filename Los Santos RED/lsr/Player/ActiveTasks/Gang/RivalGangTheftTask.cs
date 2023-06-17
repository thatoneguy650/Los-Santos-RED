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
    public class RivalGangTheftTask
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
        private DispatchableVehicle VehicleToSteal;
        private string VehicleToStealMakeName;
        private string VehicleToStealModelName;
        private PhoneContact PhoneContact;
        private GangTasks GangTasks;

        private bool HasTargetGangVehicleAndHiringDen => TargetGang != null && HiringGangDen != null && VehicleToSteal != null;
        private bool IsInStolenGangCar => Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.Vehicle.Model.Name.ToLower() == VehicleToSteal.ModelName.ToLower() && Player.CurrentVehicle.WasModSpawned && Player.CurrentVehicle.AssociatedGang != null && Player.CurrentVehicle.AssociatedGang.ID == TargetGang.ID;
        public RivalGangTheftTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes,
            PhoneContact phoneContact, GangTasks gangTasks)
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
                GameFiber.Sleep(RandomItems.GetRandomNumberInt(5000, 15000));
                GangTasks.SendGenericPickupMoneyMessage(PhoneContact, HiringGang.DenName, HiringGangDen, MoneyToRecieve);
            }
            else
            {
                Dispose();
            }
        }
        private void GetTargetGang()
        {
            TargetGang = null;
            VehicleToSteal = null;
            VehicleToStealMakeName = "";
            VehicleToStealModelName = "";
            if (HiringGang.EnemyGangs != null && HiringGang.EnemyGangs.Any())
            {
                TargetGang = Gangs.GetGang(HiringGang.EnemyGangs.PickRandom());
            }
            if (TargetGang == null)
            {
                TargetGang = Gangs.GetAllGangs().Where(x => x.ID != HiringGang.ID).PickRandom();
            }
            if (TargetGang != null)
            {
                VehicleToSteal = TargetGang.GetRandomVehicle(0, false, false, true,"", Settings);
                if (VehicleToSteal != null)
                {
                    VehicleToStealMakeName = NativeHelper.VehicleMakeName(Game.GetHashKey(VehicleToSteal.ModelName));
                    VehicleToStealModelName = NativeHelper.VehicleModelName(Game.GetHashKey(VehicleToSteal.ModelName));
                }
            }
        }
        private void GetHiringDen()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded);
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
            PlayerTasks.AddTask(HiringGang.ContactName, MoneyToRecieve, 1000, 0, -500, 5, "Auto Theft for Gang");
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                    $"Go steal a ~p~{VehicleToStealMakeName} {VehicleToStealModelName}~s~ from those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ assholes. Once you are done come back to {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. ${MoneyToRecieve} on completion",
                    $"Go get me a ~p~{VehicleToStealMakeName} {VehicleToStealModelName}~s~ with {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ gang colors. Bring it back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. Payment ${MoneyToRecieve}",
                    };
            Player.CellPhone.AddPhoneResponse(HiringGang.ContactName, HiringGang.ContactIcon, Replies.PickRandom());
        }
    }
}
