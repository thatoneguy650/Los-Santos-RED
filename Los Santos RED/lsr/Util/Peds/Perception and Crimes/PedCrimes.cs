using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;

public class PedCrimes
{
    private bool IsShootingCheckerActive = false;
    private PedExt PedExt;
    private List<Crime> CrimesViolating = new List<Crime>();
    private List<Crime> CrimesObserved = new List<Crime>();
    private ICrimes Crimes;
    private bool IsShooting = false;
    private ISettingsProvideable Settings;
    private Vector3 LastWitnessedCivilianCrimePosition;
    private Crime WorstCivilianCrimeWitnessed;
    private uint GameTimeLastWitnessedCivilianCrime;
    private PedExt LastWitnessedCivilianCrimePed;

    private bool ShouldCheck => PedExt.PedGroup != null && PedExt.PedGroup?.InternalName != "SECURITY_GUARD" && PedExt.PedGroup?.InternalName != "PRIVATE_SECURITY" && PedExt.PedGroup?.InternalName != "FIREMAN" && PedExt.PedGroup?.InternalName != "MEDIC";
    public PedCrimes(PedExt pedExt, ICrimes crimes, ISettingsProvideable settings)
    {
        PedExt = pedExt;
        Crimes = crimes;
        Settings = settings;
    }
    public int WantedLevel { get; private set; } = 0;
    public bool IsWanted => WantedLevel > 0;
    public bool IsNotWanted => WantedLevel == 0;
    public bool IsDeadlyChase => CrimesObserved.Any(x => x.ResultsInLethalForce);
    public int CurrentlyViolatingWantedLevel => CrimesViolating.Any() ? CrimesViolating.Max(x => x.ResultingWantedLevel) : 0;
    public string CurrentlyViolatingWantedLevelReason => CrimesViolating.OrderBy(x=> x.Priority).FirstOrDefault()?.Name;
    public List<Crime> CrimesCurrentlyViolating => CrimesViolating;
    public bool IsCurrentlyViolatingAnyCrimes => CrimesViolating.Any();
    public List<Crime> CrimesObservedViolating => CrimesObserved;
    public void OnPedSeenByPolice()
    {
        int WantedLevelToAssign = 0;
        foreach (Crime crime in CrimesViolating)
        {
            if (crime.ResultingWantedLevel > WantedLevelToAssign)
            {
                WantedLevelToAssign = crime.ResultingWantedLevel;
            }
            AddObserved(crime);
        }
        if (WantedLevelToAssign > WantedLevel)
        {
            WantedLevel = WantedLevelToAssign;
        }
    }
    public void OnPedHeardByPolice()
    {
        int WantedLevelToAssign = 0;
        foreach(Crime crime in CrimesViolating.Where(x=> x.CanReportBySound))
        {
            if(crime.ResultingWantedLevel > WantedLevelToAssign)
            {
                WantedLevelToAssign = crime.ResultingWantedLevel;
            }
            AddObserved(crime);
        }
        if (WantedLevelToAssign > WantedLevel)
        {
            WantedLevel = WantedLevelToAssign;
        }  
    }
    public void ShootingChecker()
    {
        if (!IsShootingCheckerActive)
        {
            GameFiber.StartNew(delegate
            {
                IsShootingCheckerActive = true;
                EntryPoint.WriteToConsole($"        Ped {PedExt.Pedestrian.Handle} IsShootingCheckerActive {IsShootingCheckerActive}", 5);
                uint GameTimeLastShot = 0;
                while (PedExt.Pedestrian.Exists() && IsShootingCheckerActive)// && CarryingWeapon && IsShootingCheckerActive && ObservedWantedLevel < 3)
                {
                    if (PedExt.Pedestrian.IsShooting)
                    {
                        IsShooting = true;
                        GameTimeLastShot = Game.GameTime;
                    }
                    else if (Game.GameTime - GameTimeLastShot >= 5000)
                    {
                        IsShooting = false;
                    }
                    GameFiber.Yield();
                }
                IsShootingCheckerActive = false;
            }, "Ped Shooting Checker");
        }
    }
    public void Update(IEntityProvideable world, IPoliceRespondable player)
    {
        if (ShouldCheck)
        {
            ResetViolations();
            CheckCrimes(world);
            if (WantedLevel > 0)
            {
                OnPedSeenByPolice();
            }
            else
            {
                CheckPoliceSight(world);
            }
            if(player.IsNotWanted)
            {
                CheckIfNeedsToCallPolice(world, player);
            }
        }
    }
    private void CheckPoliceSight(IEntityProvideable world)
    {
        foreach (Cop cop in world.PoliceList)
        {
            if (cop.Pedestrian.Exists())
            {
                float DistanceTo = cop.Pedestrian.DistanceTo2D(PedExt.Pedestrian.Position);
                if (DistanceTo <= 0.1f)
                {
                    DistanceTo = 999f;
                }
                if (DistanceTo <= Settings.SettingsManager.PoliceSettings.SightDistance && IsThisPedInFrontOf(cop.Pedestrian) && !cop.Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", cop.Pedestrian, PedExt.Pedestrian))//55f
                {
                    OnPedSeenByPolice();
                    OnPedHeardByPolice();
                    return;
                }
                if (DistanceTo <= Settings.SettingsManager.PoliceSettings.GunshotHearingDistance)
                {
                    OnPedHeardByPolice();
                }
            }
        }
    }
    private bool IsThisPedInFrontOf(Ped ToCheck)
    {
        float Result = GetDotVectorResult(ToCheck, PedExt.Pedestrian);
        if (Result > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private float GetDotVectorResult(Entity source, Entity target)
    {
        if (source.Exists() && target.Exists())
        {
            Vector3 dir = (target.Position - source.Position).ToNormalized();
            return Vector3.Dot(dir, source.ForwardVector);
        }
        else return -1.0f;
    }
    private void CheckIfNeedsToCallPolice(IEntityProvideable world, IPoliceRespondable playerToCheck)
    {
        if (IsNotWanted && !IsCurrentlyViolatingAnyCrimes && PedExt.WillCallPolice)
        {
            foreach (PedExt criminal in world.CivilianList.Where(x => x.Pedestrian.Exists() && x.IsCurrentlyViolatingAnyCrimes))
            {
                if (!PedExt.Pedestrian.Exists())
                {
                    break;
                }
                else
                {
                    Crime ToCallIn = null;
                    float distanceToCriminal = PedExt.Pedestrian.DistanceTo2D(criminal.Pedestrian);
                    if (distanceToCriminal <= 50f && criminal.Pedestrian.IsThisPedInFrontOf(PedExt.Pedestrian))
                    {
                        ToCallIn = criminal.CrimesCurrentlyViolating.Where(x => x.CanBeReportedByCivilians).OrderByDescending(x => x.ResultingWantedLevel).ThenBy(x => x.Priority).FirstOrDefault();
                    }
                    else if (distanceToCriminal <= 100f)
                    {
                        ToCallIn = criminal.CrimesCurrentlyViolating.Where(x => x.CanBeReportedByCivilians && x.CanReportBySound).OrderByDescending(x => x.ResultingWantedLevel).ThenBy(x => x.Priority).FirstOrDefault();
                    }
                    if (ToCallIn != null && (WorstCivilianCrimeWitnessed == null || (WorstCivilianCrimeWitnessed != null && ToCallIn.ID != WorstCivilianCrimeWitnessed.ID)))
                    {
                        LastWitnessedCivilianCrimePosition = criminal.Pedestrian.Position;
                        WorstCivilianCrimeWitnessed = ToCallIn;
                        GameTimeLastWitnessedCivilianCrime = Game.GameTime;
                        LastWitnessedCivilianCrimePed = criminal;
                        EntryPoint.WriteToConsole($"Handle {PedExt.Pedestrian.Handle} Add Other Civ Crime {WorstCivilianCrimeWitnessed.Name} for: {LastWitnessedCivilianCrimePed.Handle}", 5);
                    }
                }
            }
            //check if i should call the police on anyone else!
        }
        if (LastWitnessedCivilianCrimePed != null && LastWitnessedCivilianCrimePed.Pedestrian.Exists() && LastWitnessedCivilianCrimePed.Pedestrian.IsDead)
        {
            GameTimeLastWitnessedCivilianCrime = 0;
            WorstCivilianCrimeWitnessed = null;
            LastWitnessedCivilianCrimePosition = Vector3.Zero;
            LastWitnessedCivilianCrimePed = null;
        }
        if (WorstCivilianCrimeWitnessed != null && GameTimeLastWitnessedCivilianCrime != 0 && Game.GameTime - GameTimeLastWitnessedCivilianCrime > 3000)//12000
        {
            playerToCheck.AddCrime(WorstCivilianCrimeWitnessed, false, LastWitnessedCivilianCrimePosition, null, null, false, true, true);

            EntryPoint.WriteToConsole($"Handle {PedExt.Pedestrian.Handle} Call IN Other Civ Crime {WorstCivilianCrimeWitnessed.Name} for: {LastWitnessedCivilianCrimePed.Handle}", 5);
            WorstCivilianCrimeWitnessed = null;
        }

    }
    private void CheckCrimes(IEntityProvideable world)
    {
        bool isVisiblyArmed = IsVisiblyArmed();
        if (isVisiblyArmed)
        {
            if (!PedExt.IsInVehicle)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "BrandishingWeapon"));
            }
        }
        if (isVisiblyArmed)
        {
            ShootingChecker();
            if (IsShooting)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "FiringWeapon"));
                if (world.PoliceList.Any(x => x.DistanceToPlayer <= 60f))//maybe store and do the actual one?
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "FiringWeaponNearPolice"));
                }
            }
        }
        else
        {
            IsShootingCheckerActive = false;
        }

        if (PedExt.Pedestrian.IsInCombat || PedExt.Pedestrian.IsInMeleeCombat)
        {
            AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "AssaultingCivilians"));
            if(isVisiblyArmed)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "AssaultingWithDeadlyWeapon"));  
            }
            foreach (Cop cop in world.PoliceList)
            {
                if (cop.Pedestrian.Exists())
                {
                    if (PedExt.Pedestrian.CombatTarget.Exists() && PedExt.Pedestrian.CombatTarget.Handle == cop.Pedestrian.Handle)
                    {
                        AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "HurtingPolice"));
                        break;
                    }
                }
            }
        }
    }
    private bool IsVisiblyArmed()
    {
        WeaponDescriptor CurrentWeapon = PedExt.Pedestrian.Inventory.EquippedWeapon;
        if (CurrentWeapon == null)
        {
            return false;
        }
        else if (CurrentWeapon.Hash == (WeaponHash)2725352035
            || CurrentWeapon.Hash == (WeaponHash)966099553
            || CurrentWeapon.Hash == (WeaponHash)0x787F0BB//weapon_snowball
            || CurrentWeapon.Hash == (WeaponHash)0x060EC506//weapon_fireextinguisher
            || CurrentWeapon.Hash == (WeaponHash)0x34A67B97//weapon_petrolcan
            || CurrentWeapon.Hash == (WeaponHash)0xBA536372//weapon_hazardcan
            || CurrentWeapon.Hash == (WeaponHash)0x8BB05FD7//weapon_flashlight
            || CurrentWeapon.Hash == (WeaponHash)0x23C9F95C)//weapon_ball
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void AddViolating(Crime crime)
    {
        if (crime != null && crime.Enabled && !CrimesViolating.Any(x=> x.ID == crime.ID))
        {
            CrimesViolating.Add(crime);
        }
    }
    private void AddObserved(Crime crime)
    {
        if (crime != null && crime.Enabled && !CrimesObserved.Any(x => x.ID == crime.ID))
        {
            CrimesObserved.Add(crime);
        }
    }
    private void ResetViolations()
    {
        CrimesViolating.Clear();
    }
}
/*
 * using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedCrimes
{
    private bool IsShootingCheckerActive = false;
    private PedExt PedExt;
    private List<Crime> CrimesViolating = new List<Crime>();
    private List<Crime> CrimesObserved = new List<Crime>();
    public PedCrimes(PedExt pedExt)
    {
        PedExt = pedExt;
    }
    public bool CarryingWeapon { get; set; } = false;
    public int CommittingWantedLevel { get; set; } = 0;
    public string CommittingWantedLevelReason { get; set; } = "";
    public bool Fighting { get; set; } = false;
    public bool FightingPolice { get; set; } = false;
    
    public int ObservedWantedLevel { get; set; } = 0;
    public bool Shooting { get; set; } = false;
    public void SetHeard()
    {
        if (Shooting)
        {
            CommittingWantedLevelReason = "Shooting";
            CommittingWantedLevel = 3;
            if (CommittingWantedLevel > ObservedWantedLevel)//only goes up for peds!
            {
                ObservedWantedLevel = CommittingWantedLevel;
            }
        }

    }
    public void SetObserved()
    {
        if (CommittingWantedLevel > ObservedWantedLevel)//only goes up for peds!
        {
            ObservedWantedLevel = CommittingWantedLevel;
        }
    }
    public void ShootingChecker()
    {
        if (!IsShootingCheckerActive)
        {
            GameFiber.StartNew(delegate
            {
                IsShootingCheckerActive = true;
                EntryPoint.WriteToConsole($"        Ped {PedExt.Pedestrian.Handle} IsShootingCheckerActive {IsShootingCheckerActive}", 5);
                uint GameTimeLastShot = 0;
                while (PedExt.Pedestrian.Exists())// && CarryingWeapon && IsShootingCheckerActive && ObservedWantedLevel < 3)
                {
                    if (PedExt.Pedestrian.IsShooting)
                    {
                        Shooting = true;
                        GameTimeLastShot = Game.GameTime;
                    }
                    else if (Game.GameTime - GameTimeLastShot >= 5000)
                    {
                        Shooting = false;
                    }
                    GameFiber.Yield();
                }
                IsShootingCheckerActive = false;
            }, "Ped Shooting Checker");
        }
    }
    public void Update(IEntityProvideable world)
    {
        ResetViolations();





        if (IsVisiblyArmed())
        {
            CarryingWeapon = true;
        }
        else
        {
            CarryingWeapon = false;
        }
        if(CarryingWeapon)
        {
            ShootingChecker();
        }

        if(PedExt.IsInVehicle)
        {
            CarryingWeapon = false;
        }
        if(PedExt.Pedestrian.IsInCombat || PedExt.Pedestrian.IsInMeleeCombat)
        {
            Fighting = true;
            FightingPolice = false;
            foreach (Cop cop in world.PoliceList)
            {
                if (cop.Pedestrian.Exists())
                {
                    if(PedExt.Pedestrian.CombatTarget.Exists() && PedExt.Pedestrian.CombatTarget.Handle == cop.Pedestrian.Handle)
                    {
                        FightingPolice = true;
                        break;
                    }
                }
            }
        }
        else
        {
            Fighting = false;
        }


        if(Shooting)
        {
            CommittingWantedLevelReason = "Shooting";
            CommittingWantedLevel = 3;
        }
        if (FightingPolice)
        {
            CommittingWantedLevelReason = "FightingPolice";
            CommittingWantedLevel = 3;
        }
        else if (CarryingWeapon)
        {
            CommittingWantedLevelReason = "CarryingWeapon";
            CommittingWantedLevel = 2;
        }
        else if (Fighting)
        {
            CommittingWantedLevelReason = "InCombat";
            CommittingWantedLevel = 1;
        }
        else
        {
            CommittingWantedLevelReason = "";
            CommittingWantedLevel = 0;
        }

        if(ObservedWantedLevel > 0)
        {
            SetHeard();
            SetObserved();
        }

    }
    private bool IsVisiblyArmed()
    {
        WeaponDescriptor CurrentWeapon = PedExt.Pedestrian.Inventory.EquippedWeapon;
        if (CurrentWeapon == null)
        {
            return false;
        }
        else if (CurrentWeapon.Hash == (WeaponHash)2725352035
            || CurrentWeapon.Hash == (WeaponHash)966099553
            || CurrentWeapon.Hash == (WeaponHash)0x787F0BB//weapon_snowball
            || CurrentWeapon.Hash == (WeaponHash)0x060EC506//weapon_fireextinguisher
            || CurrentWeapon.Hash == (WeaponHash)0x34A67B97//weapon_petrolcan
            || CurrentWeapon.Hash == (WeaponHash)0xBA536372//weapon_hazardcan
            || CurrentWeapon.Hash == (WeaponHash)0x8BB05FD7//weapon_flashlight
            || CurrentWeapon.Hash == (WeaponHash)0x23C9F95C)//weapon_ball
        {
            return false;
        }
        else
        {
            return true;
        }
    }




    private void AddViolating(Crime crime)
    {
        if (crime != null && crime.Enabled)
        {
            CrimesViolating.Add(crime);
        }
    }
    private void ResetViolations()
    {
        CrimesViolating.RemoveAll(x => !x.IsTrafficViolation);
    }



}


*/