using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GangTask : IPlayerTask
    {
        protected ITaskAssignable Player;

        protected ITimeReportable Time;
        protected IGangs Gangs;
        protected IPlacesOfInterest PlacesOfInterest;
        protected ISettingsProvideable Settings;
        protected IEntityProvideable World;
        protected ICrimes Crimes; 
        protected IWeapons Weapons;
        protected INameProvideable Names;
        protected IPedGroups PedGroups;
        protected IShopMenus ShopMenus;
        protected IModItems ModItems;

        protected GangTasks GangTasks;
        protected PlayerTask CurrentTask;
        protected PlayerTasks PlayerTasks;

        protected Gang HiringGang;
        protected PhoneContact HiringContact;

        protected int PaymentAmount;
        protected int RepOnCompletion;
        protected int DebtOnFail;
        protected int RepOnFail;
        protected int DaysToComplete;
        protected string DebugName;

        public GangTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IEntityProvideable world,ICrimes crimes, IWeapons weapons, INameProvideable names, IPedGroups pedGroups, 
            IShopMenus shopMenus, IModItems modItems, PlayerTasks playerTasks, GangTasks gangTasks, PhoneContact hiringContact, Gang hiringGang)
        {
            Player = player;

            Time = time;
            Gangs = gangs;  
            PlacesOfInterest = placesOfInterest;
            Settings = settings;
            World = world;
            Crimes = crimes;
            Weapons = weapons;
            Names = names;
            PedGroups = pedGroups;
            ShopMenus = shopMenus;
            ModItems = modItems;


            PlayerTasks = playerTasks;
            GangTasks = gangTasks;
            HiringContact = hiringContact;
            HiringGang = hiringGang;
        }
        public virtual void Setup()
        {

        }
        public virtual void Dispose()
        {

        }
        public virtual void Start()
        {
            if(!CanStartNewTask())
            {
                GangTasks.SendGenericTooSoonMessage(HiringContact);
                return;
            }
            if (!GetTaskData())
            {
                Game.DisplayHelp($"Error Setting Up Task for {HiringContact.Name}.");
                return;
            }
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
            }, "GangTaskLoop");
        }


        protected virtual bool CanStartNewTask()
        {
            if (!PlayerTasks.CanStartNewTask(HiringContact?.Name))
            {
                return false;
            }
            return true;
        }
        protected virtual bool GetTaskData()
        {
            //Get Dens, Locations, Spawn Vehicles, Etc.
            return true;
        }

        protected virtual void GetPayment()
        {
            //Set the money to receive and stuffo

        }
        protected virtual void SendInitialInstructionsMessage()
        {

        }
        protected virtual void AddTask()
        {
            PlayerTasks.AddTask(HiringGang.Contact, PaymentAmount, RepOnCompletion, DebtOnFail, RepOnFail, DaysToComplete, DebugName, false);
        }
        protected virtual void Loop()
        {

        }

        protected virtual void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                SetReadyToPickupMoney();
            }
            else if (CurrentTask != null && CurrentTask.IsActive)
            {
                SetFailed();
            }
            else
            {
                Dispose();
            }
        }
        protected virtual void SetReadyToPickupMoney()
        {
            OnTaskCompletedOrFailed();
        }
        protected virtual void SetFailed()
        {
            OnTaskCompletedOrFailed();
            GangTasks.SendGenericFailMessage(HiringContact);
            PlayerTasks.FailTask(HiringGang.Contact);
        }
        protected virtual void OnTaskCompletedOrFailed()
        {

        }
    }
}

