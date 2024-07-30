using LosSantosRED.lsr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RouletteGameRules
{
    public RouletteGameRules()
    {

    }

    public RouletteGameRules(string dealerName)
    {
        DealerName = dealerName;
    }

    public RouletteGameRules(string gameName, string dealerName, int minBet, int maxBet, bool isRestrictedToFriendly, bool isRestrictedToMember)
    {
        GameName = gameName;
        DealerName = dealerName;
        MinBet = minBet;
        MaxBet = maxBet;
        IsRestrictedToFriendly = isRestrictedToFriendly;
        IsRestrictedToMember = isRestrictedToMember;
    }
    public string GameName { get; set; } = "Roulette";
    public string DealerName { get; set; } = "Dealer";
    public int MinBet { get; set; } = 5;
    public int MaxBet { get; set; } = 250;
    public bool IsRestrictedToFriendly { get; set; } = false;
    public bool IsRestrictedToMember { get; set; } = false;
}

