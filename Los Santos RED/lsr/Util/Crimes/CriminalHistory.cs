using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;

public class CriminalHistory
{
    private IPoliceRespondable Player;
    //public int MaxWantedLevel = 0;
    public List<CrimeEvent> CrimesObserved = new List<CrimeEvent>();
    public List<CrimeEvent> CrimesReported = new List<CrimeEvent>();
    public List<LicensePlate> WantedPlates = new List<LicensePlate>();
    public uint GameTimeWantedStarted;
    public uint GameTimeWantedEnded;
    public bool PlayerSeenDuringWanted = false;
    public int ObservedMaxWantedLevel
    {
        get
        {
            return CrimesObserved.Max(x => x.AssociatedCrime.ResultingWantedLevel);
        }
    }
    public bool PoliceHaveDescription { get; private set; }
    public Vector3 PlaceLastReportedCrime { get; private set; }
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

    public CriminalHistory(IPoliceRespondable currentPlayer)
    {
        Player = currentPlayer;
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
    public int InstancesOfCrime(string CrimeID)
    {
        CrimeEvent MyStuff = CrimesObserved.Where(x => x.AssociatedCrime.ID == CrimeID).FirstOrDefault();
        if (MyStuff == null)
            return 0;
        else
            return MyStuff.Instances;
    }
    public int InstancesOfCrime(Crime ToCheck)
    {
        CrimeEvent MyStuff = CrimesObserved.Where(x => x.AssociatedCrime == ToCheck).FirstOrDefault();
        if (MyStuff == null)
            return 0;
        else
            return MyStuff.Instances;
    }
    public void AddCrime(Crime CrimeInstance, bool ByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription)
    {
        if (Player.IsAliveAndFree)// && !CurrentPlayer.RecentlyBribedPolice)
        {
            if (HaveDescription)
            {
                PoliceHaveDescription = HaveDescription;
            }
            PlaceLastReportedCrime = Location;
            CrimeEvent PreviousViolation;
            if (ByPolice)
            {
                PreviousViolation = CrimesObserved.FirstOrDefault(x => x.AssociatedCrime.Name == CrimeInstance.Name);
            }
            else
            {
                PreviousViolation = CrimesReported.FirstOrDefault(x => x.AssociatedCrime.Name == CrimeInstance.Name);
            }

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
                    CrimesObserved.Add(new CrimeEvent(CrimeInstance, new PoliceScannerCallIn(!Player.IsInVehicle, ByPolice, Location, HaveDescription) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved, Speed = Game.LocalPlayer.Character.Speed, InstancesObserved = CurrentInstances }));
                }
                else
                {
                    //Game.Console.Print(string.Format("Crime Reported: {0}", CrimeInstance.Name));
                    CrimesReported.Add(new CrimeEvent(CrimeInstance, new PoliceScannerCallIn(!Player.IsInVehicle, ByPolice, Location, HaveDescription) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved, Speed = Game.LocalPlayer.Character.Speed, InstancesObserved = CurrentInstances }));
                }
            }
            if (ByPolice && Player.WantedLevel != CrimeInstance.ResultingWantedLevel)
            {
                Player.CurrentPoliceResponse.SetWantedLevel(CrimeInstance.ResultingWantedLevel, CrimeInstance.Name, true);
            }
        }
    }
}