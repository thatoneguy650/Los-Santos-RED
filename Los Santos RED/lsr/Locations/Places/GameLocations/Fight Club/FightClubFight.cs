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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static RAGENativeUI.Elements.UIMenuStatsPanel;


public class FightClubFight
{
    private FightClubArena FightClubArena;
    private List<FightClubFighter> Fighters;
    private List<AIFightClubFighter> AIFighters;
    private List<PedExt> Spectators;
    private FightClub FightClub;
    private bool IsPlayerFight;
    private int TotalFighters;
    private IEntityProvideable World;
    private IFightClubable Player;
    private ITargetable Targetable;
    private IDances Dances;
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
    private bool IsCancelled;

    private AIFightClubFighter SelectedOrBetFighter;
    private AIFightClubFighter PreviousWinner;
    private List<DispatchablePerson> NonGangFightersGroup;
    private List<DispatchablePerson> SpectatorsPersonList;
    private UIMenu ActiveFightMenu;

    public bool IsFightActive { get;private set; }
    public bool IsEnded { get; private set; }   
    public bool AllowMeleeWeapons { get; set; }
    public bool AllowSidearms { get;set; }
    public bool AllowHeavyWeapons { get; set; }

    public bool IsWeaponFight => AllowMeleeWeapons || AllowSidearms || AllowHeavyWeapons;

    public int SpectatorsToAdd { get; set; } 

    public FightClubFight()
    {

    }

    public FightClubFight(FightClubArena fightClubArena,  FightClub fightClub, bool isPlayerFight, int totalFighters, IEntityProvideable world, ISettingsProvideable settings, ITargetable targetable, 
        IFightClubable player, List<DispatchablePerson> nonGangFightersGroup, IDances dances, List<DispatchablePerson> spectatorsPersonList)
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
        Dances = dances;
        SpectatorsPersonList = spectatorsPersonList;
    }

    public FightClubFight(FightClubArena fightClubArena, FightClub fightClub, bool isPlayerFight, int totalFighters, IEntityProvideable world, ISettingsProvideable settings, ITargetable targetable, 
        IFightClubable player, Gang gang, IDances dances, List<DispatchablePerson> spectatorsPersonList)
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
        Dances = dances;
        SpectatorsPersonList = spectatorsPersonList;
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
        Player.ButtonPrompts.RemovePrompt("FightMenu");

        foreach (FightClubFighter fightClubFighter in Fighters)
        {
            fightClubFighter.Dispose();
        }


        foreach (PedExt spectator in Spectators)
        {
            DisposeSpectator(spectator);
        }


        //GameFiber.Sleep(1000);
        BigMessage.Fiber?.Abort();
        if (BigMessage != null && BigMessage.MessageInstance != null)
        {
            BigMessage.MessageInstance.Dispose();
        }
        Player.ActivityManager.IsInteractingWithLocation = false;
    }

    private void DisposeSpectator(PedExt PedExt)
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        PedExt.Pedestrian.IsPersistent = false;
        PedExt.CanBeAmbientTasked = true;
        PedExt.CanBeTasked = true;
        PedExt.CanBeIdleTasked = true;
        PedExt.WillCallPolice = false;
        PedExt.WillFight = false;
        PedExt.IsManuallyDeleted = false;
        PedExt.PedAlerts.Reset();
        PedExt.PedViolations.Reset();
        PedExt.PedPerception.Reset();
        PedExt.PlayerPerception.Reset();
        PedExt.ClearTasks(true);
        PedExt.PedBrain.AssignIdleTask();
    }

    public void Update()
    {
        foreach (FightClubFighter fighter in Fighters)
        {
            fighter.Update();
        }
        CheckEndingConditions();
        if(!EntryPoint.ModController.IsRunning)
        {
            Dispose();
        }
        //if(IsPlayerFight)
        //{
            //if (IsFightActive)
            //{
            //    Player.ActivityManager.IsInteractingWithLocation = false;
            //}
            //else if(!IsCancelled)
            //{
            //    Player.ActivityManager.IsInteractingWithLocation = true;
            //}
        //}
        if (IsFightActive)
        {
            if(IsPlayerFight)
            {
                HighlightAllOpponents();
                
            }
            else
            {
                HighlightSelectedOrBetFighter();
            }
            if (!IsWeaponFight)
            {
                Player.WeaponEquipment.SetUnarmed();
            }
        }
        if (!IsPlayerFight && IsFightActive)
        {
            NativeHelper.DisablePlayerMovementControl();
        }


        //button shit below
        if (!IsPlayerFight && IsFightActive)
        {
            if (Player.ActivityManager.IsCheering || IsShowingMenu)
            {
                Player.ButtonPrompts.RemovePrompt("StartCheer");
            }
            else
            {
                Player.ButtonPrompts.AttemptAddPrompt("StartCheer", "Start Cheering", "StartCheer", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 999);

                if (Player.ButtonPrompts.IsPressed("StartCheer") && !Player.ActivityManager.IsCheering)
                {
                    Player.ActivityManager.Cheer();
                }
            }
        }
        if (!IsPlayerFight && !IsFightActive)
        {
            Player.ButtonPrompts.RemovePrompt("StartCheer");
        }


        if (IsFightActive)
        {
            if (!IsShowingMenu && (ActiveFightMenu == null || !ActiveFightMenu.Visible))
            {
                Player.ButtonPrompts.AttemptAddPrompt("FightMenu", "Fight Options", "FightMenu", Settings.SettingsManager.KeySettings.InteractStart, 999);
            }
            else
            {
                Player.ButtonPrompts.RemovePrompt("FightMenu");
            }
            if (Player.ButtonPrompts.IsPressed("FightMenu") && (ActiveFightMenu == null || !ActiveFightMenu.Visible))
            {
                EntryPoint.WriteToConsole("FightMenu RAN BUTTON PRESS");
                ShowActiveFightMenu();
            }
        }
        else
        {
            Player.ButtonPrompts.RemovePrompt("FightMenu");
        }


    }

    private void ShowActiveFightMenu()
    {
        EntryPoint.WriteToConsole("ShowActiveFightMenu RAN");
        MenuPool.Clear();

        ActiveFightMenu = new UIMenu("Fight Options", "Select fight options");
        ActiveFightMenu.SetBannerType(EntryPoint.LSRedColor);
        MenuPool.Add(ActiveFightMenu);

        UIMenuItem endFightMenu = new UIMenuItem("End Fight","Select to end the fight");
        endFightMenu.Activated += (sender, e) =>
        {
            if(IsFightActive)
            {
                ActiveFightMenu.Visible = false;
                IsCancelled = true;
                OnFightEnded();              
            }
        };
        ActiveFightMenu.AddItem(endFightMenu);
        ActiveFightMenu.Visible = true;



        IsShowingMenu = true;

        Player.ActivityManager.StopDynamicActivity();

        Player.ButtonPrompts.RemovePrompt("FightMenu");
        //Player.ButtonPrompts.RemovePrompts("CheerActivity");
        while (MenuPool.IsAnyMenuOpen())
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
        IsShowingMenu = false;
        EntryPoint.WriteToConsole("ShowActiveFightMenu RAN");
    }



    public void BeginFirstFight()
    {
        SpawnRingItems();
        SetGameplayCameraHint();
        GameFiber.Sleep(1000);
        Game.FadeScreenIn(1000, true);
        AnnounceFighters();
        TakeBets();
        if (IsCancelled)
        {
            Dispose();
            return;
        }
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

        if(IsCancelled)
        {
            Dispose();
            return;
        }
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

        if (!IsCancelled)
        {
            PayoutBetsToPlayer();
        }
        foreach (AIFightClubFighter fightClubFighter in AIFighters)
        {
            fightClubFighter.SetPassive();
        }
        if (!IsPlayerFight)
        {
            Player.ActivityManager.CancelCurrentActivity();
        }
        if(Player.IsDead || !EntryPoint.ModController.IsRunning || IsCancelled)
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
        ContinueMenu.SetBannerType(EntryPoint.LSRedColor);

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
            if (Player.IsAliveAndFree)
            {
                ShowMessage($"You Have Won", $"The fight has been won by {Player.PlayerName}");
            }
        }
        AIFightClubFighter winningPed = AIFighters.Where(x => !x.HasLost).FirstOrDefault();
        if (winningPed != null)
        {
            if (PlayerBetName == winningPed.PedExt.Name)
            {
                Player.BankAccounts.GiveMoney(2 * PlayerBetAmount, false);
                wonMoney = true;
            }
            if (Player.IsAliveAndFree)
            {
                ShowMessage($"{winningPed.PedExt.Name} Has Won", $"The fight has been won by {winningPed.PedExt.Name}");
            }
        }
        if (wonMoney && Player.IsAliveAndFree)
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


        NativeFunction.Natives.CLEAR_AREA_OF_PEDS(FightClubArena.ArenaCenter, 5.0f, false);

        bool createdFighters = CreateFighters();
        if(!createdFighters)
        {
            IsEnded = true;
            Game.FadeScreenIn(0, false);
            Dispose();
            Game.DisplayHelp("COULD NOT START FIGHT");
        }
        SetupFighters();

        CreateSpectators();

        //Game.FadeScreenIn(1000, true);
    }

    private void CreateSpectators()
    {
        if (RoundNumber > 0 || SpectatorsToAdd <= 0)
        {
            return;
        }
        EntryPoint.WriteToConsole($"CreateSpectators SpectatorsToAdd{SpectatorsToAdd}");
        Spectators = new List<PedExt>();
        List<SpawnPlace> spawnLocations = new List<SpawnPlace>();
        spawnLocations.AddRange(FightClubArena.SpectatorLocations);
        if (!IsPlayerFight)
        {   
            SpawnPlace spectatorSpot = spawnLocations.PickRandom();
            if (spectatorSpot == null)
            {
                return;
            }
            Player.Character.Position = spectatorSpot.Position;
            Player.Character.Heading = spectatorSpot.Heading;
            spawnLocations.Remove(spectatorSpot);
        }
        int spectatorsAdded = 0;
        foreach(SpawnPlace spawnPlace in spawnLocations)
        {
            CreateSpectator(spawnPlace);
            spectatorsAdded++;
            if(spectatorsAdded >= SpectatorsToAdd)
            {
                break;
            }
        }
    }

    private void CreateSpectator(SpawnPlace spawnPlace)
    {
        SpawnLocation spawnLocation = new SpawnLocation(spawnPlace.Position, spawnPlace.Heading);

        DispatchablePerson dispatchablePersonToCreate = new DispatchablePerson("a_m_y_methhead_01", 0, 0);

        if(SpectatorsPersonList != null && SpectatorsPersonList.Any())
        {
            dispatchablePersonToCreate = SpectatorsPersonList.RandomElementByWeight(x => x.AmbientSpawnChance);

        }
        else if (NonGangFightersGroup != null && NonGangFightersGroup.Any())
        {
            dispatchablePersonToCreate = NonGangFightersGroup.RandomElementByWeight(x => x.AmbientSpawnChance);
        }

        PedExt createdSpectator = Player.Dispatcher.SpawnCivilian(spawnLocation, null, dispatchablePersonToCreate, FightClub, null);

        if (createdSpectator == null || !createdSpectator.Pedestrian.Exists())
        {
            return;
        }
        createdSpectator.CanBeAmbientTasked = false;
        createdSpectator.CanBeTasked = false;
        createdSpectator.CanBeIdleTasked = false;
        createdSpectator.WillFight = false;
        createdSpectator.WillCallPolice = false;
        createdSpectator.WillCower = false;
        createdSpectator.IsManuallyDeleted = true;
        createdSpectator.Pedestrian.IsPersistent = true;
        createdSpectator.Pedestrian.RelationshipGroup = new RelationshipGroup($"SPECTATOR");
        createdSpectator.Pedestrian.BlockPermanentEvents = true;
        createdSpectator.Pedestrian.KeepTasks = true;


        AssignSpectatorCheer(createdSpectator);

        //createdSpectator.Pedestrian.Tasks.StandStill(1500000);
        Spectators.Add(createdSpectator);
    }
    private void AssignSpectatorCheer(PedExt createdSpectator)
    {
        if (createdSpectator == null || !createdSpectator.Pedestrian.Exists())
        {
            return;
        }
        if (Dances == null || !Dances.DanceLookups.Any(x => x.IsCheer && !string.IsNullOrEmpty(x.AnimationIdle)))
        {
            return;
        }
        if (createdSpectator != null && createdSpectator.IsAnimal)
        {
            createdSpectator.Pedestrian.Tasks.StandStill(1500000);
            return;
        }
        DanceData danceData = Dances.DanceLookups.Where(x => x.IsCheer && !string.IsNullOrEmpty(x.AnimationIdle)).PickRandom();
        if(danceData == null)
        {
            return;
        }
        EntryPoint.WriteToConsole($"AssignSpectatorCheer DICT:{danceData.AnimationDictionary}");
        bool isAvailable = AnimationDictionary.RequestAnimationDictionayResult(danceData.AnimationDictionary);

        GameFiber.Sleep(100);//let it load or it dont worko

        if(!isAvailable)
        {
            EntryPoint.WriteToConsole("AssignSpectatorCheer FAILED");
            createdSpectator.Pedestrian.Tasks.StandStill(1500000);
            return;
        }
        
        EntryPoint.WriteToConsole($"AssignSpectatorCheer RAN {danceData.AnimationIdle}");
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", createdSpectator.Pedestrian, danceData.AnimationDictionary, danceData.AnimationIdle, 4.0f, -4.0f, -1, 1, 0, false, false, false);//-1

        //NativeFunction.Natives.TASK_PLAY_ANIM(createdSpectator.Pedestrian, danceData.AnimationDictionary, danceData.AnimationIdle, 4.0f, -4.0f, -1, 1, 0, false, false, false);//-1
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
        MenuPool.Clear();


        BetMenu = new UIMenu("Bet", "Bet on the fighter to win");


        BetMenu.SetBannerType(EntryPoint.LSRedColor);

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



        UIMenuItem cancelFight = new UIMenuItem("Cancel Fight", "Select to cancel the current fight.");
        cancelFight.Activated += (sender, e) =>
        {
            FinishedBetting = true;
            SelectedOrBetFighter = null;
            PlayerBetAmount = 0;
            PlayerBetName = "";
            IsCancelled = true;
        };
        BetMenu.AddItem(cancelFight);


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
    private void HighlightAllOpponents()
    {
        foreach(AIFightClubFighter aIFightClubFighter in AIFighters)
        {
            if (aIFightClubFighter.PedExt != null && aIFightClubFighter.PedExt.Pedestrian.Exists())
            {
                Rage.Debug.DrawArrowDebug(aIFightClubFighter.PedExt.Pedestrian.Position + new Vector3(0f, 0f, 2f), Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Red);
            }
        }
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
            if (FightClubFighterPlayer != null && FightClubFighterPlayer.SpawnPlace != null)
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
        //else if(RoundNumber == 0)
        //{
        //    SpawnPlace spectatorSpot = FightClubArena.SpectatorLocations.PickRandom();
        //    if (spectatorSpot == null)
        //    {
        //        return false;
        //    }
        //    Player.Character.Position = spectatorSpot.Position;
        //    Player.Character.Heading = spectatorSpot.Heading;
        //}
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
        if(FightClubArena == null || FightClubArena.ArenaCenter == Vector3.Zero)
        {
            return;
        }
        NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(FightClubArena.ArenaCenter.X, FightClubArena.ArenaCenter.Y, FightClubArena.ArenaCenter.Z, 1000, 2000, 2000);
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

