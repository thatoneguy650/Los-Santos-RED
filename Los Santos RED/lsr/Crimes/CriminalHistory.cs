using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;

public class CriminalHistory
{
    public int MaxWantedLevel = 0;
    public List<CrimeEvent> CrimesObserved = new List<CrimeEvent>();
    public List<CrimeEvent> CrimesReported = new List<CrimeEvent>();
    public List<LicensePlate> WantedPlates = new List<LicensePlate>();
    public uint GameTimeWantedStarted;
    public uint GameTimeWantedEnded;
    public bool PlayerSeenDuringWanted = false;
    public bool LethalForceAuthorized
    {
        get
        {
            return CrimesObserved.Any(x => x.AssociatedCrime.ResultsInLethalForce);
        }
    }
    public bool CommittedAnyCrimes
    {
        get
        {
            return CrimesObserved.Any();
        }
    }
    public CriminalHistory()
    {

    }
    public string PrintCrimes()
    {
        string CrimeString = "";
        foreach (CrimeEvent MyCrime in CrimesObserved.Where(x => x.Instances > 0).OrderBy(x => x.AssociatedCrime.Priority).Take(3))
        {
            CrimeString += string.Format("~n~{0} ({1})~s~", MyCrime.AssociatedCrime.Name, MyCrime.Instances);
        }
        return CrimeString;
    }
    public string DebugPrintCrimes()
    {
        string CrimeString = "";
        foreach (CrimeEvent MyCrime in CrimesObserved)
        {
            CrimeString += Environment.NewLine + string.Format("{0} ({1})", MyCrime.AssociatedCrime.Name, MyCrime.Instances);
        }
        return CrimeString;
    }
    public int InstancesOfCrime(Crime ToCheck)
    {
        CrimeEvent MyStuff = CrimesObserved.Where(x => x.AssociatedCrime == ToCheck).FirstOrDefault();
        if (MyStuff == null)
            return 0;
        else
            return MyStuff.Instances;
    }
    public void AddCrime(Crime CrimeInstance, bool ByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved)
    {
        CrimeEvent PreviousViolation;
        if (ByPolice)
            PreviousViolation = CrimesObserved.FirstOrDefault(x => x.AssociatedCrime.Name == CrimeInstance.Name);
        else
            PreviousViolation = CrimesReported.FirstOrDefault(x => x.AssociatedCrime.Name == CrimeInstance.Name);

        int CurrentInstances = 1;
        if (PreviousViolation != null)
        {
            PreviousViolation.AddInstance();
            CurrentInstances = PreviousViolation.Instances;
        }
        else
        {
            if (ByPolice)
            {
                CrimesObserved.Add(new CrimeEvent(CrimeInstance));
            }
            else
            {
                Debugging.WriteToLog("Crimes", string.Format("Crime Reported: {0}", CrimeInstance.Name));
                CrimesReported.Add(new CrimeEvent(CrimeInstance));
            }
        }
        if (ByPolice && Mod.Player.WantedLevel != CrimeInstance.ResultingWantedLevel)
        {
            WantedLevelManager.SetWantedLevel(CrimeInstance.ResultingWantedLevel, CrimeInstance.Name, true);
        }
        ScannerManager.AnnounceCrime(CrimeInstance, new PoliceScannerCallIn(!Mod.Player.IsInVehicle, ByPolice, Location) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved, Speed = Game.LocalPlayer.Character.Speed,InstancesObserved = CurrentInstances });

    }

}