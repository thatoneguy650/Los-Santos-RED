using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[XmlInclude(typeof(OfficerFriendlyRelationship))]
[XmlInclude(typeof(GunDealerRelationship))]
public class ContactRelationship
{
    protected IContactRelateable Player;
    protected IPlacesOfInterest PlacesOfInterest;
   // protected int reputationLevel = 200;
    public readonly int DefaultRepAmount = 200;
    public readonly int RepMaximum = 2000;
    public readonly int RepMinimum = -2000;
    public int TotalMoneySpent { get; set; } = 0;
    public int PlayerDebt { get; set; } = 0;
    public int ReputationLevel { get; set; } = 200;//=> reputationLevel;
    public string ContactName { get; set; }
    public ContactRelationship()
    {

    }
    public ContactRelationship(string contactName)
    {
        ContactName = contactName; 
    }
    public void Setup(IContactRelateable player, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
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
        if (ReputationLevel != value)
        {
            if (value > RepMaximum)
            {
                ReputationLevel = RepMaximum;
            }
            else if (value < RepMinimum)
            {
                ReputationLevel = RepMinimum;
            }
            else
            {
                ReputationLevel = value;
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
    public virtual void AddMoneySpent(int Amount)
    {
        TotalMoneySpent += Amount;
    }
    public virtual void SetMoneySpent(int Amount, bool sendNotification)
    {
        TotalMoneySpent = Amount;
    }
}

