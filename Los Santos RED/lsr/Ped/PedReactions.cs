using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PedReactions
{

    private PedExt Civilian;

    public PedReactions(PedExt civilian)
    {
        Civilian = civilian;
    }


    public bool HasSeenAngryCrime { get; set; }
    public bool HasSeenScaryCrime { get; set; }
    public bool HasSeenIntenseCrime { get; set; }
    public bool HasSeenMundaneCrime { get; set; }
    public WitnessedCrime HighestPriorityCrime { get; set; }

    public void Update()
    {

        HasSeenScaryCrime = false;
        HasSeenAngryCrime = false;
        HasSeenMundaneCrime = false;
        int PlayerCrimePriority = 99;
        int PlayerCrimeWantedLevel = 0;
        HighestPriorityCrime = Civilian.OtherCrimesWitnessed.OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
        if (HighestPriorityCrime != null && HighestPriorityCrime.Crime != null)
        {
            if (HighestPriorityCrime.Crime.CanBeReportedByCivilians && HighestPriorityCrime.Crime.AngersCivilians)
            {
                HasSeenAngryCrime = true;
            }
            if (HighestPriorityCrime.Crime.CanBeReportedByCivilians && HighestPriorityCrime.Crime.ScaresCivilians)
            {
                HasSeenScaryCrime = true;
            }
            if (HighestPriorityCrime.Crime.CanBeReportedByCivilians && !HighestPriorityCrime.Crime.AngersCivilians && !HighestPriorityCrime.Crime.ScaresCivilians)
            {
                HasSeenMundaneCrime = true;
            }
        }

        foreach (Crime crime in Civilian.PlayerCrimesWitnessed.Where(x => x.CanBeReportedByCivilians))
        {
            if (crime.AngersCivilians)
            {
                HasSeenAngryCrime = true;
            }
            if (crime.ScaresCivilians)
            {
                HasSeenScaryCrime = true;
            }
            if (!crime.ScaresCivilians && !crime.AngersCivilians)
            {
                HasSeenMundaneCrime = true;
            }
            if (crime.Priority < PlayerCrimePriority)
            {
                PlayerCrimePriority = crime.Priority;
            }
            if (crime.ResultingWantedLevel >= PlayerCrimeWantedLevel)
            {
                PlayerCrimeWantedLevel = crime.ResultingWantedLevel;
            }
        }

        if (PlayerCrimePriority < HighestPriorityCrime?.Crime?.Priority)
        {
            HighestPriorityCrime = null;
        }
        else if (PlayerCrimePriority == HighestPriorityCrime?.Crime?.Priority && Civilian.DistanceToPlayer <= 30f)
        {
            HighestPriorityCrime = null;
        }

        HasSeenIntenseCrime = false;
        if (HighestPriorityCrime?.Crime?.ResultingWantedLevel >= 3 || PlayerCrimeWantedLevel >= 3)
        {
            HasSeenIntenseCrime = true;
        }

        if (!HasSeenAngryCrime && !HasSeenScaryCrime && Civilian.HasSeenDistressedPed)
        {
            HasSeenMundaneCrime = true;
        }
    }
}

