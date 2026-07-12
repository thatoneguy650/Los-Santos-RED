using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static RAGENativeUI.Elements.UIMenuStatsPanel;


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
    private bool PlayerWon;
    private ISettingsProvideable Settings;
    private int RoundNumber;
    private bool IsShowingMenu;

    private AIFightClubFighter SelectedOrBetFighter;
    private AIFightClubFighter PreviousWinner;
    private List<DispatchablePerson> NonGangFightersGroup;

    public bool IsFightActive { get;private set; }
    public bool IsEnded { get; private set; }   
    public bool AllowMeleeWeapons { get; set; }
    public bool AllowSidearms { get;set; }
    public bool AllowHeavyWeapons { get; set; }

    public bool IsWeaponFight => AllowMeleeWeapons || AllowSidearms || AllowHeavyWeapons;
    public FightClubFight()
    {

    }

    public FightClubFight(FightClubArena fightClubArena,  FightClub fightClub, bool isPlayerFight, int totalFighters, IEntityProvideable world, ISettingsProvideable settings, ITargetable targetable, 
        IFightClubable player, List<DispatchablePerson> nonGangFightersGroup)
    {
        FightClubArena = fightClubArena;
        FightClub = fightClub;
        IsPlayerFight = isPlayerFight;
        World = world;
        Settings = settings;
        TotalFighters = totalFighters;
        Targetable = targetable;
        Player = player;
        NonGangFightersGroup = nonGangFightersGroup;
    }

    public FightClubFight(FightClubArena fightClubArena, FightClub fightClub, bool isPlayerFight, int totalFighters, IEntityProvideable world, ISettingsProvideable settings, ITargetable targetable, 
        IFightClubable player, Gang gang)
    {
        FightClubArena = fightClubArena;
        FightClub = fightClub;
        IsPlayerFight = isPlayerFight;
        World = world;
        Settings = settings;
        TotalFighters = totalFighters;
        Targetable = targetable;
        Player = player;
        GangToFight = gang;
    }

    public void Setup()
    {
        BigMessage = new BigMessageThread(true);
        MenuPool = new MenuPool();
        Player.ActivityManager.IsInteractingWithLocation = true;
        Player.DisableMainMenu = true;



    }
    public void Dispose()
    {
        IsEnded = true;
        EntryPoint.WriteToConsole("DISPOSE RAN FOR FIGHT CLUB");
        Player.DisableMainMenu = false;
        Player.ButtonPrompts.RemovePrompt("StartCheer");
        foreach (FightClubFighter fightClubFighter in Fighters)
        {
            fightClubFighter.Dispose();
        }

        //GameFiber.Sleep(1000);
        BigMessage.Fiber?.Abort();
        if (BigMessage != null && BigMessage.MessageInstance != null)
        {
            BigMessage.MessageInstance.Dispose();
        }
        Player.ActivityManager.IsInteractingWithLocation = false;
    }
    public void Update()
    {
        foreach (FightClubFighter fighter in Fighters)
        {
            fighter.Update();
        }
        CheckEndingConditions();
        if(!IsPlayerFight && IsFightActive)
        {
            if(Player.ActivityManager.IsCheering && !IsShowingMenu)
            {
                Player.ButtonPrompts.RemovePrompt("StartCheer");
            }
            else
            {
                Player.ButtonPrompts.AttemptAddPrompt("StartCheer", "Start Cheering", "StartCheer", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 999);

                if(Player.ButtonPrompts.IsPressed("StartCheer") && !Player.ActivityManager.IsCheering)
                {
                    Player.ActivityManager.Cheer();
                }
            }
            NativeHelper.DisablePlayerMovementControl();
        }
        if(!IsPlayerFight && !IsFightActive)
        {
            Player.ButtonPrompts.RemovePrompt("StartCheer");
        }
        if(!EntryPoint.ModController.IsRunning)
        {
            Dispose();
        }

        if(IsPlayerFight )
        {
            if (IsFightActive)
            {
                Player.ActivityManager.IsInteractingWithLocation = false;
            }
            else
            {
                Player.ActivityManager.IsInteractingWithLocation = true;
            }
        }

        if(IsFightActive)
        {
            HighlightSelectedOrBetFighter();
        }
    }
    public void BeginFirstFight()
    {
        SpawnRingItems();
        SetGameplayCameraHint();
        GameFiber.Sleep(1000);
        Game.FadeScreenIn(1000, true);
        AnnounceFighters();
        TakeBets();
        DoCountdown();
        StartFight();
    }
    private void BeginLaterFight()
    {
        RoundNumber++;
        Game.FadeScreenOut(1000, true);
        Reset();
        CleanupNonWinners();
        SpawnRingItems();
        GameFiber.Sleep(1000);
        Game.FadeScreenIn(1000, true);
        SetGameplayCameraHint();
        AnnounceFighters();
        TakeBets();
        DoCountdown();
        StartFight();
    }



    private void CheckEndingConditions()
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
        IsFightActive = false;
        PayoutBetsToPlayer();
        foreach (AIFightClubFighter fightClubFighter in AIFighters)
        {
            fightClubFighter.SetPassive();
        }
        if (!IsPlayerFight)
        {
            Player.ActivityManager.CancelCurrentActivity();
        }
        if(Player.IsDead || !EntryPoint.ModController.IsRunning)
        {
            Dispose();
            return;        
        }
        ShowContinueFightingMenu();
    }
    private void DisplayInsufficientFundsMessage()
    {
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", FightClub.Name, "~r~Insufficient Funds~s~", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
    }
    private void ShowContinueFightingMenu()
    {
        bool finishedWithMenu = false;
        bool startNewFight = false;
        MenuPool.Clear();

        IsShowingMenu = true;






        UIMenu ContinueMenu = new UIMenu("Continue Fighting", "Select an option");
        MenuPool.Add(ContinueMenu);


        if (IsPlayerFight)
        {
            int ReliefCost = FightClub == null ? 500 : FightClub.PostRoundReliefPrice;
            int ReliefHealthGained = FightClub == null ? 25 : FightClub.ReliefHealthGained;
            UIMenuItem reliefBuyMenu = new UIMenuItem("Buy Some Relief", $"Select to purchase some relief. Gives you ~g~ +{ReliefHealthGained} HP~s~") { RightLabel = $"~r~${ReliefCost}" };
            reliefBuyMenu.Activated += (sender, e) =>
            {
                if (Player.BankAccounts.GetMoney(false) >= ReliefCost)
                {
                    Player.BankAccounts.GiveMoney(-1 * ReliefCost, false);
                    Player.HealthManager.ChangeHealth(ReliefHealthGained);
                    reliefBuyMenu.Enabled = false;
                }
                else
                {
                    DisplayInsufficientFundsMessage();
                }
                //ContinueMenu.Visible = false;
            };
            ContinueMenu.AddItem(reliefBuyMenu);

        }

        UIMenuItem startFight = new UIMenuItem("Continue", "Select to continue fighting.");
        startFight.Activated += (sender, e) =>
        {
            finishedWithMenu = true;
            startNewFight = true;
            ContinueMenu.Visible = false;
        };
        ContinueMenu.AddItem(startFight);





        UIMenuItem stopFighting = new UIMenuItem("Stop", "Select to stop fighting.");
        stopFighting.Activated += (sender, e) =>
        {
            finishedWithMenu = true;
            ContinueMenu.Visible = false;
        };
        ContinueMenu.AddItem(stopFighting);

        ContinueMenu.Visible = true;
        while (MenuPool.IsAnyMenuOpen() && !finishedWithMenu)
        {
            NativeHelper.DisablePlayerMovementControl();
            HighlightSelectedOrBetFighter();
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
        ContinueMenu.Visible = false;
        IsShowingMenu = false;
        if (startNewFight && EntryPoint.ModController.IsRunning)
        {
            BeginLaterFight();
        }
        else
        {
            Dispose();
        }
    }


    private void StartFight()
    {
        IsFightActive = true;
        foreach (FightClubFighter fighter in Fighters)
        {
            fighter.StartFight(this);
        }
        string FightStartedMessage = $"";
        if(PreviousWinner != null && PreviousWinner.PedExt != null)
        {
            FightStartedMessage = $"Last Rounds Winner {PreviousWinner.PedExt.Name}";
        }
        ShowMessage("Fight Started", FightStartedMessage);
    }
    private void Reset()
    {
        HasFoundWinner = false;
        IsEnded = false;
        PlayerWon = false;
    }
    private void CleanupNonWinners()
    {
        foreach (AIFightClubFighter fightClubFighter in AIFighters)
        {
            if(fightClubFighter.PedExt != null && fightClubFighter.PedExt.Pedestrian.Exists())
            {
                if (!fightClubFighter.HasLost && !fightClubFighter.PedExt.IsDead && !fightClubFighter.PedExt.IsUnconscious && fightClubFighter.PedExt.Pedestrian.Exists() && !fightClubFighter.PedExt.Pedestrian.IsDead)
                {
                    fightClubFighter.WasPreviousWinner = true;
                    EntryPoint.WriteToConsole("YOU HAD A PREVIOUS WINNER OF THE FIGHT");
                }
                else
                {
                    fightClubFighter.PedExt.FullyDelete();
                }
            }
        }
    }

    private void PayoutBetsToPlayer()
    {
        bool wonMoney = false;
        if (PlayerWon)//FightClubFighterPlayer != null && !FightClubFighterPlayer.HasLost)
        {
            if (PlayerBetName == Player.PlayerName)
            {
                Player.BankAccounts.GiveMoney(2 * PlayerBetAmount, false);
                wonMoney = true;
            }
            ShowMessage($"You Have Won", $"The fight has been won by {Player.PlayerName}");
        }
        AIFightClubFighter winningPed = AIFighters.Where(x => !x.HasLost).FirstOrDefault();
        if (winningPed != null)
        {
            if (PlayerBetName == winningPed.PedExt.Name)
            {
                Player.BankAccounts.GiveMoney(2 * PlayerBetAmount, false);
                wonMoney = true;
            }
            ShowMessage($"{winningPed.PedExt.Name} Has Won", $"The fight has been won by {winningPed.PedExt.Name}");
        }
        if (wonMoney)
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", FightClub.Name, "Winner", $"You have won ~g~${2 * PlayerBetAmount}~s~");
        }
    }
    private void OnAIWon()
    {
        PlayerWon = false;
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
    }
    private void OnPlayerWon()
    {
        PlayerWon = true;
        EntryPoint.WriteToConsole("PLAYER WON THE FIGHT");
    }
    private void OnPlayerLost()
    {
        PlayerWon = false;
        EntryPoint.WriteToConsole("PLAYER LOST THE FIGHT");
    }
    private void SpawnRingItems()
    {
        //Game.FadeScreenOut(1000, true);

        bool createdFighters = CreateFighters();
        if(!createdFighters)
        {
            IsEnded = true;
            Game.FadeScreenIn(0, false);
            Dispose();
            Game.DisplayHelp("COULD NOT START FIGHT");
        }
        SetupFighters();
        //Game.FadeScreenIn(1000, true);
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
        int MinBet = FightClub == null ? 100 : FightClub.MinBet;
        if (Player.BankAccounts.GetMoney(false) < MaxBet)
        {
            MaxBet = Player.BankAccounts.GetMoney(false);
        }

        UIMenuNumericScrollerItem<int>  AmountBetScroller = new UIMenuNumericScrollerItem<int>("Amount", "Set The Amount you wish to bet", MinBet, MaxBet, 100);
        AmountBetScroller.Value = MinBet;
        AmountBetScroller.Activated += (sender, e) =>
        {
            if (int.TryParse(NativeHelper.GetKeyboardInput(AmountBetScroller.Value.ToString()), out int enteredAmount))
            {
                if (enteredAmount <= MaxBet && enteredAmount > MinBet)
                {
                    AmountBetScroller.Value = enteredAmount;
                }
            }
        };


        BetMenu.AddItem(AmountBetScroller);


        List<string> FightersToBetOn = new List<string>();
        if (IsPlayerFight)
        {
            FightersToBetOn.Add(Player.PlayerName);
        }
        else
        {
            foreach (AIFightClubFighter aIFightClubFighter in AIFighters)
            {

                FightersToBetOn.Add(aIFightClubFighter.PedExt.Name);

            }
        }

        UIMenuListScrollerItem<string> FighterPickedMenu = new UIMenuListScrollerItem<string>("Fighter", "Select the fighter you want to win", FightersToBetOn);


        SelectedOrBetFighter = AIFighters.Where(x => x.PedExt != null && FighterPickedMenu.SelectedItem != null && x.PedExt.Name == FighterPickedMenu.SelectedItem.ToString()).FirstOrDefault();



        FighterPickedMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SelectedOrBetFighter = AIFighters.Where(x => x.PedExt != null && x.PedExt.Name == FighterPickedMenu.SelectedItem.ToString()).FirstOrDefault();

            //if(SelectedOrBetFighter != null && SelectedOrBetFighter.PedExt != null && SelectedOrBetFighter.PedExt.Pedestrian.Exists())
            //{
            //    NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(SelectedOrBetFighter.PedExt.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
            //}
            //ApplyItems(player, ClothingPurchaseMenu.WorkingVariation, false);
        };

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

                SelectedOrBetFighter = AIFighters.Where(x => x.PedExt != null && x.PedExt.Name == FighterPickedMenu.SelectedItem.ToString()).FirstOrDefault();

                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", FightClub.Name, "Placed Bet", $"You have placed a bet of ${PlayerBetAmount} on {PlayerBetName}");
            }
        };
        BetMenu.AddItem(startBet);

        UIMenuItem noBet = new UIMenuItem("No Bet", "Select to watch the match without a wager.");
        noBet.Activated += (sender, e) =>
        {
            FinishedBetting = true;
            SelectedOrBetFighter = null;
            PlayerBetAmount = 0;
            PlayerBetName = "";
        };
        BetMenu.AddItem(noBet);


        BetMenu.Visible = true;
        IsShowingMenu = true;
        while (MenuPool.IsAnyMenuOpen() && !FinishedBetting)
        {
            NativeHelper.DisablePlayerMovementControl();

            HighlightSelectedOrBetFighter();

            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
        IsShowingMenu = false;
    }
    private void HighlightSelectedOrBetFighter()
    {
        if(SelectedOrBetFighter == null)
        {
            return;
        }
        if(SelectedOrBetFighter.PedExt == null || !SelectedOrBetFighter.PedExt.Pedestrian.Exists())
        {
            return;
        }
        Rage.Debug.DrawArrowDebug(SelectedOrBetFighter.PedExt.Pedestrian.Position + new Vector3(0f, 0f, 2f), Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Red);
    }
    private void DoCountdown()
    {
        IsShowingMenu = true;
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
            HighlightSelectedOrBetFighter();
            NativeHelper.DisablePlayerMovementControl();
            GameFiber.Yield(); // CRITICAL: Keeps the game from freezing
        }
        IsShowingMenu = false;
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
            if(aiFighter.PedExt == null)
            {
                EntryPoint.WriteToConsole("SETUP FIGHTS AI FIGHTER HAS NO PEDEXTY");
            }
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
        if (AIFighters != null)
        {
            PreviousWinner = AIFighters.Where(x => !x.HasLost && x.PedExt != null && !x.PedExt.IsDead && !x.PedExt.IsUnconscious && x.PedExt.Pedestrian.Exists() && !x.PedExt.Pedestrian.IsDead).FirstOrDefault();
        }
        bool hasPreviousWinner = PreviousWinner != null;
        Fighters = new List<FightClubFighter>();
        AIFighters = new List<AIFightClubFighter>();
        int FightersToSpawn = IsPlayerFight ? TotalFighters-1 : TotalFighters;
        EntryPoint.WriteToConsole($"FightersToSpawn {FightersToSpawn}");
        List<SpawnPlace> spawnLocations = new List<SpawnPlace>();
        spawnLocations.AddRange(FightClubArena.FighterSpawnLocations);
        if (IsPlayerFight)
        {
            SpawnPlace toPick = null;
            if (FightClubFighterPlayer.SpawnPlace != null)
            {
                toPick = FightClubFighterPlayer.SpawnPlace;
            }
            else
            {
                toPick = spawnLocations.PickRandom();
            }
            if (toPick == null)
            {
                return false;
            }
            spawnLocations.Remove(toPick);
            FightClubFighterPlayer = new PlayerFightClubFighter();
            FightClubFighterPlayer.MoveToRing(FightClubArena, World, toPick, Player);
            Fighters.Add(FightClubFighterPlayer);
        }     
        else if(RoundNumber == 0)
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

            if (hasPreviousWinner && fightersSpawned == 0)
            {
                spawnedPed = true;
                EntryPoint.WriteToConsole("RESTORED FIGHTER FROM PREVIOUS FIGHT");
                PreviousWinner.RestoreForNewFight(fightersSpawned);
                spawnLocations.Remove(PreviousWinner.SpawnPlace);
                Fighters.Add(PreviousWinner);
                AIFighters.Add(PreviousWinner);
            }
            else
            {
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
                    spawnedPed = CreateGangFighter(toPick, GangToFight, fightersSpawned);
                }
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
        DispatchablePerson dispatchablePersonToCreate = new DispatchablePerson("a_m_y_methhead_01", 0, 0);
        if (NonGangFightersGroup != null && NonGangFightersGroup.Any())
        {
            dispatchablePersonToCreate = NonGangFightersGroup.RandomElementByWeight(x => x.AmbientSpawnChance);
        }
        aiFighter.SpawnInRing(FightClubArena, World, Player, spawnLocation, FightClub, dispatchablePersonToCreate, Targetable, order);//use the built in stuff here, get gans or other stuffo
        Fighters.Add(aiFighter);
        AIFighters.Add(aiFighter);
        return aiFighter.PedExt != null || aiFighter.PedExt.Pedestrian.Exists();
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
        aiFighter.Gang = gang;
        aiFighter.SpawnInRing(FightClubArena, World, Player, spawnLocation, FightClub, dispatchablePerson, Targetable, order);//use the built in stuff here, get gans or other stuffo
        Fighters.Add(aiFighter);
        AIFighters.Add(aiFighter);
        return aiFighter.PedExt != null || aiFighter.PedExt.Pedestrian.Exists();
    }
    private void SetGameplayCameraHint()
    {
        //NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(FightClubArena.ArenaCenter.X, FightClubArena.ArenaCenter.Y, FightClubArena.ArenaCenter.Z, 2000, 2000, 0);
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

