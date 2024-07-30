using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BlackJackGameRules
{
    public BlackJackGameRules()
    {

    }
    public BlackJackGameRules(string gameName, string dealerName, int minBet, int maxBet, bool canSurrender, bool canSplit, bool isRestrictedToFriendly, bool isRestrictedToMember)
    {
        GameName = gameName;
        DealerName = dealerName;
        MinBet = minBet;
        MaxBet = maxBet;
        CanSurrender = canSurrender;
        CanSplit = canSplit;
        IsRestrictedToFriendly = isRestrictedToFriendly;
        IsRestrictedToMember = isRestrictedToMember;
    }
    public string GameName { get; set; } = "Blackjack";
    public string DealerName { get; set; } = "Dealer";
    public int MinBet { get; set; } = 5;
    public int MaxBet { get; set; } = 250;
    public bool CanSurrender { get; set; } = true;
    public bool CanSplit { get; set; } = true;
    public bool IsRestrictedToFriendly { get; set; } = false;
    public bool IsRestrictedToMember { get; set; } = false;

}

