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

    private ReactionTier prevReactionTier;


    private List<PedReaction> PedReactionList = new List<PedReaction>();

    private PedExt Civilian;
    public PedReactions(PedExt civilian)
    {
        Civilian = civilian;
    }


    public PedReaction PrimaryPedReaction { get; private set; }
    public ReactionTier ReactionTier { get; private set; }


    //OLD
    private uint GameTimeLastSeenAngryCrime;
    private uint GameTimeLastSeenScaryCrime;
    private uint GameTimeLastSeenMundaneCrime;
    private uint GameTimeLastSeenIntenseCrime;
    public bool HasSeenAngryCrime { get; set; }
    public bool HasSeenScaryCrime { get; set; }
    public bool HasSeenIntenseCrime { get; set; }
    public bool HasSeenMundaneCrime { get; set; }
    public bool RecentlySeenAngryCrime => GameTimeLastSeenAngryCrime > 0 && Game.GameTime - GameTimeLastSeenAngryCrime <= 30000;
    public bool RecentlySeenScaryCrime => GameTimeLastSeenScaryCrime > 0 && Game.GameTime - GameTimeLastSeenScaryCrime <= 30000;
    public bool RecentlySeenIntenseCrime => GameTimeLastSeenIntenseCrime > 0 && Game.GameTime - GameTimeLastSeenIntenseCrime <= 30000;
    public bool RecentlySeenMundaneCrime => GameTimeLastSeenMundaneCrime > 0 && Game.GameTime - GameTimeLastSeenMundaneCrime <= 30000;
    public WitnessedCrime HighestPriorityCrime { get; set; }
    public bool IncludeUnconsciousAsMundane { get; set; } = true;

    public void Update(ITargetable Player)
    {
        ResetCrimeStatus();
        UpdateCrimes(Player);
        UpdateReactions();
    }
    public void Reset()
    {
        PedReactionList.Clear();
        UpdateReactions();
    }
    private void ResetCrimeStatus()
    {
        HasSeenScaryCrime = false;
        HasSeenAngryCrime = false;
        HasSeenMundaneCrime = false;
        HasSeenIntenseCrime = false;
    }
    private void UpdateCrimes(ITargetable Player)
    {       
        foreach (WitnessedCrime witnessedCrime in Civilian.CrimesWitnessed.Where(x => x.Crime.CanBeReactedToByCivilians && !x.HasBeenReactedTo))
        {
            if(witnessedCrime.IsPlayerWitnessedCrime && (!Player.IsAliveAndFree || Player.WantedLevel >= 3))
            {
                continue;
            }
            if(!witnessedCrime.IsPlayerWitnessedCrime && (witnessedCrime.Perpetrator.IsBusted || witnessedCrime.Perpetrator.IsUnconscious || witnessedCrime.Perpetrator.IsDead))
            {
                continue;
            }
            AddReaction(witnessedCrime, Player);
            OldUpdate(witnessedCrime);
            witnessedCrime.SetReactedTo();
        }
        OldDistressedUpdate();
        if(prevReactionTier != ReactionTier)
        {
            //EntryPoint.WriteToConsoleTestLong($"Ped Reaction {Civilian.Handle} Reaction Changed from {prevReactionTier} to {ReactionTier}");
            prevReactionTier = ReactionTier;
        }
    }

    private void AddReaction(WitnessedCrime witnessed, ITargetable Player)
    {
        PedReaction ExistingReaction;
        if (witnessed.IsPlayerWitnessedCrime)
        {
            ExistingReaction = PedReactionList.FirstOrDefault(x => x.ReactingToPed == null);
        }
        else
        {
            ExistingReaction = PedReactionList.FirstOrDefault(x => x.ReactingToPed != null && x.ReactingToPed.Handle == witnessed.Perpetrator.Handle);
        }
        if (ExistingReaction == null)
        {
            if(witnessed.IsPlayerWitnessedCrime)
            {
                PedReactionList.Add(new PedReaction(Player, Civilian, witnessed.GameTimeLastWitnessed, witnessed.Crime.ReactionTier));
            }
            else
            {
                PedReactionList.Add(new PedReaction(Player, Civilian, witnessed.Perpetrator, witnessed.GameTimeLastWitnessed, witnessed.Crime.ReactionTier));
            }
            //EntryPoint.WriteToConsoleTestLong($"Added New Ped Reaction {Civilian.Handle} for {witnessed.Crime.Name} to Perpetrator {witnessed.Perpetrator?.Handle}");
        }
        else
        {
            ExistingReaction.PlaceLastReacted = witnessed.Location;
            if (ExistingReaction.ReactionTier < witnessed.Crime.ReactionTier)
            {
                ExistingReaction.ReactionTier = witnessed.Crime.ReactionTier;
                //EntryPoint.WriteToConsoleTestLong($"Updated Existing Ped Reaction {Civilian.Handle} for {witnessed.Crime.Name} to Perpetrator {witnessed.Perpetrator?.Handle} ReactionTier {ExistingReaction.ReactionTier}");
            }
            ExistingReaction.GameTimeLastReacted = witnessed.GameTimeLastWitnessed;
            //EntryPoint.WriteToConsole($"Updated Existing Ped Reaction {Civilian.Handle} for {witnessed.Crime.Name} to Perpetrator {witnessed.Perpetrator?.Handle}");
        }
    }
    private void UpdateReactions()
    {
        ReactionTier = ReactionTier.None;
        PrimaryPedReaction = null;
        foreach (PedReaction pedReaction in PedReactionList)
        {
            pedReaction.Update();
            if(ReactionTier < pedReaction.ReactionTier)
            {
                ReactionTier = pedReaction.ReactionTier;
                PrimaryPedReaction = pedReaction;
            }
        }
        PedReactionList.RemoveAll(x => x.IsExpired);
    }
    private void OldUpdate(WitnessedCrime witnessedCrime)
    {
        if (witnessedCrime.Crime.IsAngerInducing)
        {
            GameTimeLastSeenAngryCrime = witnessedCrime.GameTimeLastWitnessed;
            HasSeenAngryCrime = true;
        }
        if (witnessedCrime.Crime.IsScary)
        {
            GameTimeLastSeenScaryCrime = witnessedCrime.GameTimeLastWitnessed;
            HasSeenScaryCrime = true;
        }
        if(witnessedCrime.Crime.IsMundane)
        {
            GameTimeLastSeenMundaneCrime = witnessedCrime.GameTimeLastWitnessed;
            HasSeenMundaneCrime = true;
        }
        if (witnessedCrime.Crime.IsIntense)
        {
            GameTimeLastSeenIntenseCrime = witnessedCrime.GameTimeLastWitnessed;
            HasSeenIntenseCrime = true;
        }
        if (HighestPriorityCrime == null || witnessedCrime.Crime.Priority < HighestPriorityCrime?.Crime.Priority || (witnessedCrime.Crime.Priority == HighestPriorityCrime?.Crime.Priority && witnessedCrime.IsPlayerWitnessedCrime))
        {
            HighestPriorityCrime = witnessedCrime;
        }
    }
    private void OldDistressedUpdate()
    {
        if (!HasSeenAngryCrime && !HasSeenScaryCrime && Civilian.PedAlerts.HasCrimeToReport && IncludeUnconsciousAsMundane)
        {
            GameTimeLastSeenMundaneCrime = Game.GameTime;
            HasSeenMundaneCrime = true;
        }
    }
}

