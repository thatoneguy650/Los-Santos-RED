using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class OfficerFriendlyRelationship
{
    private IGunDealerRelateable Player;
    private IPlacesOfInterest PlacesOfInterest;
    private int reputationLevel = 200;
    public readonly int DefaultRepAmount = 200;
    public readonly int RepMaximum = 2000;
    public readonly int RepMinimum = -2000;

    public int TotalMoneySpentOnBribes { get; private set; } = 0;
    public int PlayerDebt { get; private set; } = 0;
    public int ReputationLevel => reputationLevel;
    public OfficerFriendlyRelationship(IGunDealerRelateable player, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {
        Reset(false);
    }
    public void Reset(bool sendText)
    {
        SetReputation(DefaultRepAmount, sendText);
        PlayerDebt = 0;
    }
    public void SetReputation(int value, bool sendText)
    {
        if (reputationLevel != value)
        {
            if (value > RepMaximum)
            {
                reputationLevel = RepMaximum;
            }
            else if (value < RepMinimum)
            {
                reputationLevel = RepMinimum;
            }
            else
            {
                reputationLevel = value;
            }
            OnReputationChanged(sendText);
        }
    }
    public void ChangeReputation(int Amount, bool sendText)
    {
        SetReputation(ReputationLevel + Amount, sendText);
    }
    private void OnReputationChanged(bool sendText)
    {

    }
    public void AddDebt(int Amount)
    {
        PlayerDebt += Amount;
    }
    public void SetDebt(int Amount)
    {
        PlayerDebt = Amount;
    }
    public void AddMoneySpent(int Amount)
    {
        TotalMoneySpentOnBribes += Amount;
        EntryPoint.WriteToConsole($"You spent {Amount} for a total of {TotalMoneySpentOnBribes}");

    }
    public void SetMoneySpent(int Amount, bool sendNotification)
    {
        TotalMoneySpentOnBribes = Amount;
    }
}

