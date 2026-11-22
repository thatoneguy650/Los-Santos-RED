using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;

/// <summary>
/// The crew kicking up to you.
///
/// WHY THIS IS A WAGER AND NOT A COLLECTION. A button that simply banks what your men have
/// earned is a button you press; it asks nothing and it teaches nothing. Upstream already
/// models the other half of this relationship - Gang.MemberKickUpDays/Amount/MissLimit, the
/// dues YOU owe the gang on a timer - so the player already understands kicking up as an
/// obligation with consequences. Pointing it downward keeps that vocabulary: your men have
/// their own rackets, you take a share, and putting their money to work is a decision rather
/// than a formality.
///
/// THE FLOOR IS THE DESIGN. Every tier pays something even when it fails. A man who went out
/// and tried does not come back empty handed, and the player is never punished for having
/// spent a resource he could not get back. What the tiers actually trade is variance: low
/// risk is a near certainty of a modest return, high risk is mostly a bad night with the
/// occasional score.
///
/// NOTE ON THE ODDS. As tuned, expected value RISES with risk (2.85 / 3.40 / 3.45 per point
/// staked). That is deliberate - the swing is what makes the choice interesting - but it does
/// mean a patient player is right to take the high tier. If that flattens the decision in
/// play, raise the low tier's rates rather than nerfing the high one; a floor that feels mean
/// is worse than a ceiling that feels generous.
///
/// Owned by GangCrewManager rather than registered on the player, so it costs no new
/// declaration on any of the player interfaces.
/// </summary>
public class GangKickUpManager
{
    public enum RiskTier
    {
        Low = 0,
        Medium = 1,
        High = 2,
    }

    private readonly GangCrewManager Crew;
    private readonly Mod.Player Player;
    private readonly ISettingsProvideable Settings;

    public GangKickUpManager(GangCrewManager crew, Mod.Player player, ISettingsProvideable settings)
    {
        Crew = crew;
        Player = player;
        Settings = settings;
    }

    private GangProgressionSettings Config => Settings?.SettingsManager?.GangProgressionSettings;

    public bool IsEnabled => Crew != null && Crew.TributeEnabled;

    public int MinimumStake => Config == null || Config.CrewKickUpMinimumStake <= 0 ? 100 : Config.CrewKickUpMinimumStake;

    private static readonly int[] DefaultChances = new int[] { 85, 60, 35 };
    private static readonly int[] DefaultSuccessRates = new int[] { 3, 5, 8 };
    private static readonly int[] DefaultFailureRates = new int[] { 2, 1, 1 };

    public int ChanceFor(RiskTier tier) => TierValue(Config?.CrewKickUpSuccessChanceByTier, DefaultChances, tier);

    public int SuccessRateFor(RiskTier tier) => TierValue(Config?.CrewKickUpSuccessRateByTier, DefaultSuccessRates, tier);

    public int FailureRateFor(RiskTier tier) => TierValue(Config?.CrewKickUpFailureRateByTier, DefaultFailureRates, tier);

    public string NameFor(RiskTier tier)
    {
        switch (tier)
        {
            case RiskTier.Low: return "Keep it quiet";
            case RiskTier.Medium: return "Push it a little";
            default: return "Let him swing";
        }
    }

    public string BlurbFor(RiskTier tier)
    {
        return $"Usually works out. Pays about ~g~{SuccessRateFor(tier)} to one~s~, ~o~{FailureRateFor(tier)} to one~s~ if it doesn't.";
    }

    /// <summary>
    /// Put a man's money to work. Returns the line to show the player, and never throws -
    /// this runs inside a location's menu callback, where an exception takes the whole
    /// interaction down rather than just this item.
    /// </summary>
    public string Attempt(GangCrewMember member, int stake, RiskTier tier)
    {
        try
        {
            if (!IsEnabled)
            {
                return "That's not how we do things.";
            }
            if (member == null)
            {
                return "Who?";
            }
            if (stake < MinimumStake)
            {
                return "Not worth anybody's time.";
            }
            int taken = Crew.TakeTribute(member, stake);
            if (taken <= 0)
            {
                return $"{member.Name} hasn't got that on him.";
            }

            bool success = RandomItems.GetRandomNumberInt(1, 100) <= ChanceFor(tier);
            int rate = success ? SuccessRateFor(tier) : FailureRateFor(tier);
            int payout = taken * rate;
            if (success)
            {
                payout += VolumeBonus(payout, taken);
            }
            if (payout < 1)
            {
                payout = 1; // the floor is a promise; never hand back nothing
            }

            Player?.BankAccounts?.GiveMoney(payout, false);
            Crew.RecordEvent(member, success ? $"Brought you ${payout}" : "It went bad on him", success ? 5 : 1, 0);

            Log($"{member.Name} staked {taken} at {tier} - {(success ? "paid" : "flopped")} ${payout}");

            return success
                ? $"~g~{member.Name} came through.~s~ He's back with ${payout}."
                : $"~o~It went bad.~s~ {member.Name} salvaged ${payout} of it.";
        }
        catch (Exception e)
        {
            EntryPoint.WriteToConsole($"GangKickUp: attempt failed {e.Message}", 0);
            return "Something went wrong out there.";
        }
    }

    /// <summary>
    /// A bigger stake buys a better rate, capped. Without the cap a player who hoards a
    /// veteran's earnings for an hour turns one press of a button into the whole economy.
    /// </summary>
    private int VolumeBonus(int payout, int taken)
    {
        int perStep = Config == null ? 4 : Config.CrewKickUpVolumeBonusPercent;
        if (perStep <= 0)
        {
            return 0;
        }
        int steps = (taken / MinimumStake) - 1;
        if (steps <= 0)
        {
            return 0;
        }
        int bonusPercent = steps * perStep;
        int cap = Config == null || Config.CrewKickUpVolumeBonusMaximum <= 0 ? 60 : Config.CrewKickUpVolumeBonusMaximum;
        if (bonusPercent > cap)
        {
            bonusPercent = cap;
        }
        return (int)Math.Round(payout * bonusPercent / 100.0);
    }

    private void Log(string message)
    {
        if (Config != null && Config.LogStandingChanges)
        {
            EntryPoint.WriteToConsole($"GangKickUp: {message}", 5);
        }
    }

    public List<RiskTier> AllTiers => new List<RiskTier> { RiskTier.Low, RiskTier.Medium, RiskTier.High };

    private static int TierValue(string configured, int[] fallback, RiskTier tier)
    {
        int[] table = fallback;
        if (!string.IsNullOrWhiteSpace(configured))
        {
            List<int> parsed = new List<int>();
            bool ok = true;
            foreach (string piece in configured.Split(','))
            {
                int value;
                if (!int.TryParse(piece.Trim(), out value) || value < 0)
                {
                    ok = false;
                    break;
                }
                parsed.Add(value);
            }
            if (ok && parsed.Count == fallback.Length)
            {
                table = parsed.ToArray();
            }
        }
        int index = (int)tier;
        if (index < 0) { index = 0; }
        if (index >= table.Length) { index = table.Length - 1; }
        return table[index];
    }
}
