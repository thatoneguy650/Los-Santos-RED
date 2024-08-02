using Blackjack;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GamblingManager
{
    private ICasinoGamePlayable Player;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private List<LocationGamblingStatus> locationGamblingWins = new List<LocationGamblingStatus>();
    public bool HasSetupSharedTextures { get; private set; }
    public Texture UnknownCardTexture { get; set; }
    public List<Tuple<Card, Texture>> CardIconList { get; set; } = new List<Tuple<Card, Texture>>();
    public GamblingManager(ICasinoGamePlayable player, ISettingsProvideable settings, ITimeReportable time)
    {
        Player = player;
        Settings = settings;
        Time = time;
    }
    public void Setup()
    {
        locationGamblingWins = new List<LocationGamblingStatus>();
    }
    public void Dipsose()
    {
        locationGamblingWins.Clear();
    }
    public void Reset()
    {
        locationGamblingWins.Clear();
        //Reset win limit shit?
    }
    public void SetupSharedTextures()
    {
        if (HasSetupSharedTextures)
        {
            return;
        }
        HasSetupSharedTextures = true;
        UnknownCardTexture = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\cards\\unknown.png");
        CardIconList = new List<Tuple<Card, Texture>>();
        List<string> suits = new List<string>() { "diamonds", "spade", "hearts", "club" };
        foreach (string suitName in suits)
        {
            for (int faceValue = 2; faceValue <= 14; faceValue++)
            {
                Suit chosenSuit = suitName == "hearts" ? Suit.Hearts : suitName == "diamonds" ? Suit.Diamonds : suitName == "spade" ? Suit.Spades : suitName == "club" ? Suit.Clubs : Suit.Hearts;
                Face choseFace = Face.Two;
                string filePrefix = faceValue.ToString();
                if (faceValue == 2)
                {
                    choseFace = Face.Two;
                    filePrefix = "2";
                }
                else if (faceValue == 3)
                {
                    choseFace = Face.Three;
                    filePrefix = "3";
                }
                else if (faceValue == 4)
                {
                    choseFace = Face.Four;
                    filePrefix = "4";
                }
                else if (faceValue == 5)
                {
                    choseFace = Face.Five;
                    filePrefix = "5";
                }
                else if (faceValue == 6)
                {
                    choseFace = Face.Six;
                    filePrefix = "6";
                }
                else if (faceValue == 7)
                {
                    choseFace = Face.Seven;
                    filePrefix = "7";
                }
                else if (faceValue == 8)
                {
                    choseFace = Face.Eight;
                    filePrefix = "8";
                }
                else if (faceValue == 9)
                {
                    choseFace = Face.Nine;
                    filePrefix = "9";
                }
                else if (faceValue == 10)
                {
                    choseFace = Face.Ten;
                    filePrefix = "10";
                }
                else if (faceValue == 11)
                {
                    choseFace = Face.Jack;
                    filePrefix = "J";
                }
                else if (faceValue == 12)
                {
                    choseFace = Face.Queen;
                    filePrefix = "Q";
                }
                else if (faceValue == 13)
                {
                    choseFace = Face.King;
                    filePrefix = "K";
                }
                else if (faceValue == 14)
                {
                    choseFace = Face.Ace;
                    filePrefix = "A";
                }
                string fileSuffix = suitName;
                Card myCard = new Card(chosenSuit, choseFace);
                CardIconList.Add(new Tuple<Card, Texture>(myCard, Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\cards\\{filePrefix}_{fileSuffix}.png")));
            }
        }
    }
    public string GetStats(GamblingDen gameLocation)
    {
        if(gameLocation == null)
        {
            return "";
        }
        string ToReturn = "";
        int totalWon = TotalWon(gameLocation);
        ToReturn = $"Past {gameLocation.WinLimitResetHours} hours: Win Total: ${(totalWon >= 0 ? "~g~" : "~r~")}{totalWon}~s~ Limit ${gameLocation.WinLimit}";
        return ToReturn;
    }
    public int TotalWon(GameLocation gameLocation)
    {
        LocationGamblingStatus locationGamblingWin = locationGamblingWins.Where(x => x.GameLocation.Name == gameLocation.Name).FirstOrDefault();
        if (locationGamblingWin == null)
        {
            return 0;
        }
        locationGamblingWin.UpdateTime(Time);
        return locationGamblingWin.TotalWon;
    }
    public bool IsWinBanned(GamblingDen gameLocation)
    {
        if(TotalWon(gameLocation) > gameLocation.WinLimit)
        {
            return true;
        }
        return false;
    }
    public void OnMoneyWon(GamblingDen gameLocation, int totalMoneyWon)
    {
        if(gameLocation == null)
        {
            return;
        }
        LocationGamblingStatus locationGamblingWin = locationGamblingWins.Where(x=> x.GameLocation.Name == gameLocation.Name).FirstOrDefault();
        if(locationGamblingWin == null)
        {
            locationGamblingWins.Add(new LocationGamblingStatus(gameLocation,  new List<GamblingIncident> { new GamblingIncident(totalMoneyWon, Time.CurrentDateTime) }));
        }
        else
        {
            locationGamblingWin.GamblingIncidents.Add(new GamblingIncident(totalMoneyWon, Time.CurrentDateTime));
        }
        if(gameLocation.AssociatedGang != null)
        {
            Player.RelationshipManager.GangRelationships.OnMoneyWon(gameLocation.AssociatedGang,totalMoneyWon);
        }
# if DEBUG
        EntryPoint.WriteToConsole($"OnMoneyWon: {gameLocation.Name} ADDING: {totalMoneyWon} CurrentTotal {TotalWon(gameLocation)}");
#endif
    }
}

