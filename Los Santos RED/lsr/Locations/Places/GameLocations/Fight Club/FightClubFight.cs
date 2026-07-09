using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;


public class FightClubFight
{
    private FightClubArena FightClubArena;
    private List<FightClubFighter> Fighters;
    private List<AIFightClubFighter> AIFighters;
    private FightClub FightClub;
    private bool IsPlayerFight;
    private int TotalFighters;
    private IEntityProvideable World;
    private IFightClubable Player;
    private ITargetable Targetable;
    private bool HasFoundWinner;
    private PlayerFightClubFighter FightClubFighterPlayer;
    private Gang GangToFight;
    private uint GameTimeStartedCountdown;
    private BigMessageThread BigMessage;
    private MenuPool MenuPool;
    private bool FinishedBetting;
    private UIMenu BetMenu;
    private int PlayerBetAmount;
    private string PlayerBetName;

    public bool IsEnded { get; private set; }   
    public FightClubFight()
    {

    }

    public FightClubFight(FightClubArena fightClubArena,  FightClub fightClub, bool isPlayerFight, int totalFighters, IEntityProvideable world, ITargetable targetable, IFightClubable player)
    {
        FightClubArena = fightClubArena;
        FightClub = fightClub;
        IsPlayerFight = isPlayerFight;
        World = world;
        TotalFighters = totalFighters;
        Targetable = targetable;
        Player = player;
    }
    public FightClubFight(FightClubArena fightClubArena, FightClub fightClub, bool isPlayerFight, int totalFighters, IEntityProvideable world, ITargetable targetable, IFightClubable player, Gang gang)
    {
        FightClubArena = fightClubArena;
        FightClub = fightClub;
        IsPlayerFight = isPlayerFight;
        World = world;
        TotalFighters = totalFighters;
        Targetable = targetable;
        Player = player;
        GangToFight = gang;
    }

    public void Setup()
    {
        BigMessage = new BigMessageThread(true);
        MenuPool = new MenuPool();
    }
    public void Update()
    {
        foreach (FightClubFighter fighter in Fighters)
        {
            fighter.Update();
        }

        DetermineWinner();
        
        //EntryPoint.WriteToConsole("FIGHT CLUB FIGHT UPDATE RAN ");
    }



    public void Begin()
    {
        SpawnRingItems();
        AnnounceFighters();
        TakeBets();
        DoCountdown();
        StartFight();
    }



    private void DetermineWinner()
    {




        bool isFightOver = false;// Fighters.Where(x => !x.HasLost).Count() == 1 || FightClubFighterPlayer.HasLost;

        if(IsPlayerFight)
        {
            isFightOver = Fighters.Where(x => !x.HasLost).Count() == 1 || FightClubFighterPlayer.HasLost;
        }
        else
        {
            isFightOver = AIFighters.Where(x => !x.HasLost).Count() == 1 || AIFighters.All(x=> !x.PedExt.Pedestrian.Exists());
        }


        if(isFightOver && !HasFoundWinner)
        {
            IsEnded = true;
            HasFoundWinner = true;
            //FightClubFighter playerFighter = Fighters.Where(x => !x.HasLost && x.IsPlayer).FirstOrDefault();


            if(IsPlayerFight)
            {
                bool allAILost = AIFighters.All(x => x.HasLost);
                bool PlayerLost = FightClubFighterPlayer.HasLost;
                if (allAILost)
                {
                    OnPlayerWon();
                }
                else if (PlayerLost)
                {
                    OnPlayerLost();
                }
            }
            else
            {
                OnAIWon();
            }

            OnFightEnded();


            EntryPoint.WriteToConsole("Fight Ended");
        }
    }

    private void OnFightEnded()
    {
        bool wonMoney = false;
        if(FightClubFighterPlayer != null && !FightClubFighterPlayer.HasLost)
        {
            if (PlayerBetName == Player.PlayerName)
            {
                Player.BankAccounts.GiveMoney(2 * PlayerBetAmount, false);
                wonMoney = true;
            }
            ShowMessage($"You Have Won",$"The fight has been won by {Player.PlayerName}");
        }
        AIFightClubFighter winningPed = AIFighters.Where(x => !x.HasLost).FirstOrDefault();
        if(winningPed != null)
        {
            if(PlayerBetName == winningPed.PedExt.Name)
            {
                Player.BankAccounts.GiveMoney(2 * PlayerBetAmount, false);
                wonMoney = true;
            }
            ShowMessage($"{winningPed.PedExt.Name} Has Won", $"The fight has been won by {winningPed.PedExt.Name}");
        }
        if(wonMoney)
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", FightClub.Name, "Winner", $"You have won ~g~${2 * PlayerBetAmount}~s~");
        }
    }

    private void OnAIWon()
    {
        AIFightClubFighter winner = AIFighters.Where(x => !x.HasLost).FirstOrDefault();
        if(winner != null)
        {
            Game.DisplayHelp($"{winner.PedExt.Name} Won the Fight");
            EntryPoint.WriteToConsole($"{winner.PedExt.Name} WON THE FIGHT");
        }
        else
        {
            Game.DisplayHelp($"Nobody Won the Fight");
            EntryPoint.WriteToConsole($"nobody WON THE FIGHT");
        }
        Dispose();
    }

    private void OnPlayerWon()
    {
       // Game.DisplayHelp("You Won the Fight");
        EntryPoint.WriteToConsole("PLAYER WON THE FIGHT");
        Dispose();
    }
    private void OnPlayerLost()
    {
        //Game.DisplayHelp("You Lost the Fight");
        EntryPoint.WriteToConsole("PLAYER LOST THE FIGHT");
        Dispose();
    }
    public void Dispose()
    {
        foreach(FightClubFighter fightClubFighter in Fighters)
        {
            fightClubFighter.Dispose();
        }

        GameFiber.Sleep(1000);

        BigMessage.Fiber?.Abort();
        if (BigMessage != null && BigMessage.MessageInstance != null)
        {
            BigMessage.MessageInstance.Dispose();
        }
    }



    private void SpawnRingItems()
    {
        bool createdFighters = CreateFighters();
        if(!createdFighters)
        {
            IsEnded = true;
            Dispose();
            Game.DisplayHelp("COULD NOT START FIGHT");
        }
        SetupFighters();
    }
    private void AnnounceFighters()
    {

    }
    private void TakeBets()
    {
        ShowBettingMenu();
    }

    private void ShowBettingMenu()
    {
        FinishedBetting = false;
        BetMenu = new UIMenu("Bet", "Bet on the fighter to win");
        MenuPool.Add(BetMenu);
        int MaxBet = FightClub == null ? 5000 : FightClub.MaxBet;
        if (Player.BankAccounts.GetMoney(false) < MaxBet)
        {
            MaxBet = Player.BankAccounts.GetMoney(false);
        }

        UIMenuNumericScrollerItem<int>  AmountBetScroller = new UIMenuNumericScrollerItem<int>("Amount", "Set The Amount you wish to bet", 0, MaxBet, 1);
        BetMenu.AddItem(AmountBetScroller);


        List<string> FightersToBetOn = new List<string>();
        if(IsPlayerFight)
        {
            FightersToBetOn.Add(Player.PlayerName);
        }
        foreach (AIFightClubFighter aIFightClubFighter in AIFighters)
        {
            FightersToBetOn.Add(aIFightClubFighter.PedExt.Name);

        }


        UIMenuListScrollerItem<string> FighterPickedMenu = new UIMenuListScrollerItem<string>("Fighter", "Select the fighter you want to win", FightersToBetOn);
        BetMenu.AddItem(FighterPickedMenu);


        UIMenuItem startBet = new UIMenuItem("Confirm Bets", "Select to confirm your bet and start the fight.");
        startBet.Activated += (sender, e) =>
        {
            FinishedBetting = true;
            if(AmountBetScroller.Value > 0)
            {

                Player.BankAccounts.GiveMoney(-1 * AmountBetScroller.Value, false);
                PlayerBetAmount = AmountBetScroller.Value;
                PlayerBetName = FighterPickedMenu.SelectedItem.ToString();
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", FightClub.Name, "Placed Bet", $"You have placed a bet of ${PlayerBetAmount} on {PlayerBetName}");
            }
        };
        BetMenu.AddItem(startBet);

        BetMenu.Visible = true;

        while (MenuPool.IsAnyMenuOpen() && !FinishedBetting)
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
    }


    private void DoCountdown()
    {


        GameTimeStartedCountdown = Game.GameTime;
        string showing = "";

        while (Game.GameTime - GameTimeStartedCountdown <= 3000)
        {
            string toShow = "";
            HudColor toShowColor = HudColor.RedDark;

            long elapsed = Game.GameTime - GameTimeStartedCountdown;

            if (elapsed < 1000) { toShow = "3"; toShowColor = HudColor.RedDark; }
            else if (elapsed < 2000) { toShow = "2"; toShowColor = HudColor.Red; }
            else { toShow = "1"; toShowColor = HudColor.OrangeDark; }

            if (toShow != showing)
            {
                ShowMessage(toShow, "", HudColor.Black, toShowColor, 1000);
                showing = toShow;
            }

            GameFiber.Yield(); // CRITICAL: Keeps the game from freezing
        }
    }
    private void StartFight()
    {
        foreach (FightClubFighter fighter in Fighters)
        {
            fighter.StartFight();
        }

        if (IsPlayerFight)
        {
            
            //Game.DisplayHelp("Fight Started, stay in the ring until you cant");
        }
        else
        {
            //Game.DisplayHelp("Fight Started, cheer on your guy");
        }
        ShowMessage("Fight Started", "");
    }






    private void SetupFighters()
    {
        foreach(FightClubFighter fighter in Fighters)
        {
            fighter.Setup();
        }
        List<RelationshipGroup> relationshipGroups = new List<RelationshipGroup>();
        foreach(AIFightClubFighter aiFighter in AIFighters)
        {
            relationshipGroups.Add(aiFighter.PedExt.Pedestrian.RelationshipGroup);
        }
        foreach(RelationshipGroup relationshipGroup in relationshipGroups)
        {
            if (IsPlayerFight)
            {
                EntryPoint.WriteToConsole($"{relationshipGroup.Name} SET HATE WITH {Player.Character.RelationshipGroup.Name}");
                relationshipGroup.SetRelationshipWith(Player.Character.RelationshipGroup, Relationship.Hate);
                Player.Character.RelationshipGroup.SetRelationshipWith(relationshipGroup, Relationship.Hate);
            }
            else
            {
                relationshipGroup.SetRelationshipWith(Player.Character.RelationshipGroup, Relationship.Respect);
                Player.Character.RelationshipGroup.SetRelationshipWith(relationshipGroup, Relationship.Respect);
            }
            foreach (RelationshipGroup relationshipGroupOther in relationshipGroups.Where(x=> x.Name != relationshipGroup.Name))
            {
                relationshipGroup.SetRelationshipWith(relationshipGroupOther, Relationship.Hate);
                relationshipGroupOther.SetRelationshipWith(relationshipGroup, Relationship.Hate);
                EntryPoint.WriteToConsole($"{relationshipGroup.Name} SET HATE WITH {relationshipGroupOther.Name}");
            }
        }
    }

    private bool CreateFighters()
    {
        Fighters = new List<FightClubFighter>();
        AIFighters = new List<AIFightClubFighter>();
        int FightersToSpawn = IsPlayerFight ? TotalFighters-1 : TotalFighters;
        EntryPoint.WriteToConsole($"FightersToSpawn {FightersToSpawn}");
        List<SpawnPlace> spawnLocations = new List<SpawnPlace>();
        spawnLocations.AddRange(FightClubArena.FighterSpawnLocations);
        if (IsPlayerFight)
        {
            SpawnPlace toPick = spawnLocations.PickRandom();
            if (toPick == null)
            {
                return false;
            }
            spawnLocations.Remove(toPick);
            FightClubFighterPlayer = new PlayerFightClubFighter();
            FightClubFighterPlayer.MoveToRing(FightClubArena, World, toPick, Player);
            Fighters.Add(FightClubFighterPlayer);
        }     
        else
        {
            SpawnPlace spectatorSpot = FightClubArena.SpectatorLocations.PickRandom();
            if (spectatorSpot == null)
            {
                return false;
            }
            Player.Character.Position = spectatorSpot.Position;
            Player.Character.Heading = spectatorSpot.Heading;
        }
        bool CreatedAny = false;
        for (int fightersSpawned = 0; fightersSpawned < FightersToSpawn; fightersSpawned++)
        {
            bool spawnedPed = false;
            SpawnPlace toPick = spawnLocations.PickRandom();
            if (toPick == null)
            {
                return false;
            }
            spawnLocations.Remove(toPick);
            if (GangToFight == null)
            {
                spawnedPed = CreateCivilianFighter(toPick, fightersSpawned);
            }
            else
            {
                spawnedPed = CreateGangFighter(toPick,GangToFight, fightersSpawned);
            }

            if(spawnedPed)
            {
                CreatedAny = true;
            }
            EntryPoint.WriteToConsole($"SPAWNED FIGHTER fightersSpawned{fightersSpawned} FightersToSpawn{FightersToSpawn}");
        }
        return CreatedAny;
    }
    private bool CreateCivilianFighter(SpawnPlace spawnLocation, int order)
    {
        AIFightClubFighter aiFighter = new AIFightClubFighter();
        
        aiFighter.SpawnInRing(FightClubArena, World, Player, spawnLocation, FightClub, new DispatchablePerson("a_m_y_methhead_01", 0, 0), Targetable, order);//use the built in stuff here, get gans or other stuffo
        Fighters.Add(aiFighter);
        AIFighters.Add(aiFighter);
        return true;
    }
    private bool CreateGangFighter(SpawnPlace spawnLocation,Gang gang, int order)
    {
        if(gang == null)
        {
            return false;
        }
        DispatchablePerson dispatchablePerson = gang.GetRandomPed(0,"");
        if(dispatchablePerson == null)
        {
            return false;
        }
        AIFightClubFighter aiFighter = new AIFightClubFighter();
        aiFighter.SpawnInRing(FightClubArena, World, Player, spawnLocation, FightClub, dispatchablePerson, Targetable, order);//use the built in stuff here, get gans or other stuffo
        Fighters.Add(aiFighter);
        AIFighters.Add(aiFighter);
        return true;
    }


    public void ShowMessage(string v, string v1, HudColor color1, HudColor color2, int time)
    {
        BigMessage.MessageInstance.ShowColoredShard(v, v1, color1, color2, time);
    }
    public void ShowMessage(string v, string v1)
    {
        BigMessage.MessageInstance.ShowColoredShard(v, v1, HudColor.Black, HudColor.GreenDark, 1000);
    }

}

