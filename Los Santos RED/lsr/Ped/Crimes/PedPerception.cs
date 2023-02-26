using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Linq;

public class PedPerception
{
    private PedExt PedExt;
    private bool EverCommittedCrime = false;
    //private List<WitnessedCrime> OtherPedCrimesObserved = new List<WitnessedCrime>();
    private ICrimes Crimes;
    private ISettingsProvideable Settings;
    private uint GameTimeLastWitnessedCivilianCrime;
    private IWeapons Weapons;
    private IEntityProvideable World;
    private List<PedExt> PedsToCheckCrimes;
    private PedExt CurrentCriminal;
    private float DistanceToCurrentCriminal;
    private Vector3 WitnessedLocation;
    private VehicleExt WitnessedVehicle;
    private WeaponInformation WitnessedWeapon;

    private bool IsNonCriminal => PedExt.PedViolations.IsNotWanted && !PedExt.PedViolations.IsCurrentlyViolatingAnyCrimes;
    public PedPerception(PedExt pedExt, ICrimes crimes, ISettingsProvideable settings, IWeapons weapons, IEntityProvideable world)
    {
        PedExt = pedExt;
        Crimes = crimes;
        Settings = settings;
        Weapons = weapons;
        World = world;
    }
    public List<WitnessedCrime> NPCCrimesWitnessed { get; set; } = new List<WitnessedCrime>();
    public void Reset()
    {
        NPCCrimesWitnessed.Clear();
    }
    public void Update()
    {
        if (Settings.SettingsManager.CivilianSettings.CheckCivilianCrimes && !PedExt.IsArrested && PedExt.PedViolations.WantedLevel == 0 && PedExt.PedViolations.CurrentlyViolatingWantedLevel == 0 && Settings.SettingsManager.CivilianSettings.AllowCivilinsToCallPoliceOnOtherCivilians)//used to have player.isnotwanted here
        {
            CheckOtherPedCrimes();
        }
    }
    private void CheckOtherPedCrimes()
    {
        if (IsNonCriminal)
        {
            RemoveExpiredWitnessedCrimes();
            UpdatePossibleCriminals();
            CurrentCriminal = PedsToCheckCrimes.Where(x => x.Pedestrian.Exists() && x.PedViolations.IsCurrentlyViolatingAnyCivilianReportableCrimes && x.Pedestrian.IsAlive && NativeHelper.IsNearby(PedExt.CellX, PedExt.CellY, x.CellX, x.CellY, 4)).OrderByDescending(x => x.CurrentlyViolatingWantedLevel).FirstOrDefault();
            if (CurrentCriminal == null || !CurrentCriminal.Pedestrian.Exists() || !PedExt.Pedestrian.Exists())
            {
                return;
            }
           // GameFiber.Yield();//TR TEST 28
            if (CurrentCriminal == null || !CurrentCriminal.Pedestrian.Exists() || !PedExt.Pedestrian.Exists())
            {
                return;
            }
            DistanceToCurrentCriminal = PedExt.Pedestrian.DistanceTo2D(CurrentCriminal.Pedestrian);
            GetWitnessInformation();
            if (CurrentCriminal == null || !CurrentCriminal.Pedestrian.Exists() || !PedExt.Pedestrian.Exists())
            {
                return;
            }
            AddObservedCrime();     
        }
    }
    private void GetWitnessInformation()
    {
        WitnessedVehicle = null;
        WitnessedWeapon = null;
        WitnessedLocation = PedExt.Pedestrian.Position;
        if (!PedExt.IsGangMember && (PedExt.WillCallPolice || (PedExt.WillCallPoliceIntense && CurrentCriminal.WantedLevel >= 3)))
        {
            uint VehicleWitnessed = 0;
            uint WeaponWitnessed = 0;
            WitnessedLocation = CurrentCriminal.Pedestrian.Position;
            if (DistanceToCurrentCriminal <= 60f)
            {
                Vehicle tryingToEnter = CurrentCriminal.Pedestrian.VehicleTryingToEnter;
                if (CurrentCriminal.Pedestrian.IsInAnyVehicle(false) && CurrentCriminal.Pedestrian.CurrentVehicle.Exists())
                {
                    VehicleWitnessed = CurrentCriminal.Pedestrian.CurrentVehicle.Handle;
                }
                else if (tryingToEnter.Exists())
                {
                    VehicleWitnessed = tryingToEnter.Handle;
                }
                uint currentWeapon;
                NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(CurrentCriminal.Pedestrian, out currentWeapon, true);
                if (currentWeapon != 2725352035 && currentWeapon != 0)
                {
                    WeaponWitnessed = currentWeapon;
                }
                WitnessedVehicle = World.Vehicles.GetVehicleExt(VehicleWitnessed);
                WitnessedWeapon = Weapons.GetWeapon((ulong)WeaponWitnessed);
                //GameFiber.Yield();//TR TEST 28
            }
        }
    }
    private void AddObservedCrime()
    {
        if (DistanceToCurrentCriminal <= 40f && CurrentCriminal.Pedestrian.IsThisPedInFrontOf(PedExt.Pedestrian) && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", PedExt.Pedestrian, CurrentCriminal.Pedestrian))//60f
        {
            foreach (Crime crime in CurrentCriminal.CrimesCurrentlyViolating.Where(x => x.CanBeReactedToByCivilians))
            {
                if (DistanceToCurrentCriminal <= crime.MaxReportingDistance)
                {
                    AddOtherPedObserved(crime, CurrentCriminal, WitnessedVehicle, WitnessedWeapon, WitnessedLocation);
                    GameTimeLastWitnessedCivilianCrime = Game.GameTime;
                }
            }
        }
        else if (DistanceToCurrentCriminal <= 100f)
        {
            foreach (Crime crime in CurrentCriminal.CrimesCurrentlyViolating.Where(x => x.CanBeReactedToByCivilians && x.CanReportBySound))
            {
                if (DistanceToCurrentCriminal <= crime.MaxReportingDistance)
                {
                    AddOtherPedObserved(crime, CurrentCriminal, WitnessedVehicle, WitnessedWeapon, WitnessedLocation);
                    GameTimeLastWitnessedCivilianCrime = Game.GameTime;
                }
            }
        }
    }
    private void UpdatePossibleCriminals()
    {
        PedsToCheckCrimes = new List<PedExt>();
        PedsToCheckCrimes.AddRange(World.Pedestrians.CivilianList);
        PedsToCheckCrimes.AddRange(World.Pedestrians.ZombieList);
        if (PedExt.GetType() == typeof(GangMember))
        {
            GangMember gangMember = (GangMember)PedExt;
            PedsToCheckCrimes.AddRange(World.Pedestrians.GangMemberList.Where(x => x.Gang?.ID != gangMember.Gang?.ID));
        }
        else
        {
            PedsToCheckCrimes.AddRange(World.Pedestrians.GangMemberList);
        }
    }
    private void RemoveExpiredWitnessedCrimes()
    {
        NPCCrimesWitnessed.RemoveAll(x => x.Perpetrator != null && x.Perpetrator.Pedestrian.Exists() && x.Perpetrator.Pedestrian.IsDead);
    }
    private void AddOtherPedObserved(Crime crime, PedExt perpetrator, VehicleExt vehicle, WeaponInformation weapon, Vector3 location)
    {
        if (crime != null && crime.Enabled)
        {
            WitnessedCrime ExistingEvent = NPCCrimesWitnessed.FirstOrDefault(x => x.Crime?.ID == crime.ID && x.Perpetrator.Handle == perpetrator.Handle);
            if (ExistingEvent == null)
            {
                NPCCrimesWitnessed.Add(new WitnessedCrime(crime, perpetrator, vehicle, weapon, location));
            }
            else
            {
                ExistingEvent.UpdateWitnessed(vehicle, weapon, location);
            }
        }
    }
}