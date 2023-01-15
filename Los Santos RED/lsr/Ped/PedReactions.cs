using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

public class PedReactions
{

    private uint GameTimeLastSeenAngryCrime;
    private uint GameTimeLastSeenScaryCrime;
    private uint GameTimeLastSeenMundaneCrime;
    private uint GameTimeLastSeenIntenseCrime;

    private PedExt Civilian;
    public PedReactions(PedExt civilian)
    {
        Civilian = civilian;
    }
    public bool HasSeenAngryCrime { get; set; }
    public bool HasSeenScaryCrime { get; set; }
    public bool HasSeenIntenseCrime { get; set; }
    public bool HasSeenMundaneCrime { get; set; }



    public bool RecentlySeenAngryCrime => GameTimeLastSeenAngryCrime > 0 && Game.GameTime - GameTimeLastSeenAngryCrime <= 30000;
    public bool RecentlySeenScaryCrime => GameTimeLastSeenScaryCrime > 0 && Game.GameTime - GameTimeLastSeenScaryCrime <= 30000;
    public bool RecentlySeenIntenseCrime => GameTimeLastSeenIntenseCrime > 0 && Game.GameTime - GameTimeLastSeenIntenseCrime <= 30000;
    public bool RecentlySeenMundaneCrime => GameTimeLastSeenMundaneCrime > 0 && Game.GameTime - GameTimeLastSeenMundaneCrime <= 30000;





    public WitnessedCrime HighestPriorityCrime { get; set; }

    public bool IsReactingToPlayer => HighestPriorityCrime != null && HighestPriorityCrime.IsPlayerWitnessedCrime;
    public bool IsReactingToNPC => HighestPriorityCrime != null && !HighestPriorityCrime.IsPlayerWitnessedCrime;

    public void Update(ITargetable Player)
    {
        Reset();
        UpdateCrimes(Player);
        DetermineReaction();
    }
    private void Reset()
    {
        HasSeenScaryCrime = false;
        HasSeenAngryCrime = false;
        HasSeenMundaneCrime = false;
        HasSeenIntenseCrime = false;
    }
    private void UpdateCrimes(ITargetable Player)
    {       
        foreach (WitnessedCrime witnessedCrime in Civilian.CrimesWitnessed.Where(x => x.Crime.CanBeReportedByCivilians))
        {





            if(witnessedCrime.IsPlayerWitnessedCrime && (!Player.IsAliveAndFree || Player.WantedLevel >= 3))
            {
                continue;
            }
            if(!witnessedCrime.IsPlayerWitnessedCrime && (witnessedCrime.Perpetrator.IsBusted || witnessedCrime.Perpetrator.IsUnconscious || witnessedCrime.Perpetrator.IsDead))
            {
                continue;
            }

            //how do i expire a crime that i saw? i might be in combat with them, but it has been 30 seconds since 
            //if i am in combat i should be seeing crimes methinks

            //how do I remove crimes? On Target Busted, Unconscious, or Dead, remove all witnessed crimes?
            //If they are > 150f from you remove all crimes?
            //maybe same as cops, if they havent seen you in a while, it just goes back to whatever
            //so your crimes get wiped or moved from active if they havent seen you in 45 seconds or so?maybe just ped crimes, player what if you hide, then show up again?




            if (witnessedCrime.Crime.AngersCivilians)
            {
                GameTimeLastSeenAngryCrime = witnessedCrime.GameTimeLastWitnessed;
                HasSeenAngryCrime = true;
            }
            if (witnessedCrime.Crime.ScaresCivilians)
            {
                GameTimeLastSeenScaryCrime = witnessedCrime.GameTimeLastWitnessed;
                HasSeenScaryCrime = true;
            }
            if (!witnessedCrime.Crime.ScaresCivilians && !witnessedCrime.Crime.AngersCivilians)
            {
                GameTimeLastSeenMundaneCrime = witnessedCrime.GameTimeLastWitnessed;
                HasSeenMundaneCrime = true;
            }
            if (witnessedCrime.Crime.ResultingWantedLevel >= 3 || witnessedCrime.Crime.ResultsInLethalForce || witnessedCrime.Crime.Priority <= 10)
            {
                GameTimeLastSeenIntenseCrime = witnessedCrime.GameTimeLastWitnessed;
                HasSeenIntenseCrime = true;
            }
            if (HighestPriorityCrime == null || witnessedCrime.Crime.Priority < HighestPriorityCrime?.Crime.Priority || (witnessedCrime.Crime.Priority == HighestPriorityCrime?.Crime.Priority && witnessedCrime.IsPlayerWitnessedCrime))
            {
                HighestPriorityCrime = witnessedCrime;
            }
        }
        if (!HasSeenAngryCrime && !HasSeenScaryCrime && Civilian.HasSeenDistressedPed)
        {
            GameTimeLastSeenMundaneCrime = Game.GameTime;
            HasSeenMundaneCrime = true;
        }
    }
    private void DetermineReaction()
    {

    }
}

