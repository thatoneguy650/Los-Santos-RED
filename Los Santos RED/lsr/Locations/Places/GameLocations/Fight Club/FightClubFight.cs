using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
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
    private FightClubFighterPlayer FightClubFighterPlayer;
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

    public void Setup()
    {

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


            EntryPoint.WriteToConsole("Fight Ended");
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
        Game.DisplayHelp("You Won the Fight");
        EntryPoint.WriteToConsole("PLAYER WON THE FIGHT");
        Dispose();
    }
    private void OnPlayerLost()
    {
        Game.DisplayHelp("You Lost the Fight");
        EntryPoint.WriteToConsole("PLAYER LOST THE FIGHT");
        Dispose();
    }
    public void Dispose()
    {
        foreach(FightClubFighter fightClubFighter in Fighters)
        {
            fightClubFighter.Dispose();
        }
    }
    public void StartFight()
    {
        bool createdFighters = CreateFighters();
        if(!createdFighters)
        {
            IsEnded = true;
            Dispose();
            Game.DisplayHelp("COULD NOT START FIGHT");
        }

        SetupFighters();
        foreach (FightClubFighter fighter in Fighters)
        {
            fighter.StartFight();
        }

        if (IsPlayerFight)
        {
            Game.DisplayHelp("Fight Started, stay in the ring until you cant");
        }
        else
        {
            Game.DisplayHelp("Fight Started, cheer on your guy");
        }
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
        List<SpawnLocation> spawnLocations = new List<SpawnLocation>();
        spawnLocations.AddRange(FightClubArena.FighterSpawnLocations);
        if (IsPlayerFight)
        {
            SpawnLocation toPick = spawnLocations.PickRandom();
            if (toPick == null)
            {
                return false;
            }
            spawnLocations.Remove(toPick);
            FightClubFighterPlayer = new FightClubFighterPlayer();
            FightClubFighterPlayer.MoveToRing(FightClubArena, World, toPick, Player);
            Fighters.Add(FightClubFighterPlayer);
        }     
        else
        {
            SpawnLocation spectatorSpot = FightClubArena.SpectatorLocations.PickRandom();
            if (spectatorSpot == null)
            {
                return false;
            }
            Player.Character.Position = spectatorSpot.FinalPosition;
            Player.Character.Heading = spectatorSpot.Heading;
        }
        for (int fightersSpawned = 0; fightersSpawned < FightersToSpawn; fightersSpawned++)
        {
            SpawnLocation toPick = spawnLocations.PickRandom();
            if(toPick == null)
            {
                return false;
            }
            spawnLocations.Remove(toPick);
            AIFightClubFighter aiFighter = new AIFightClubFighter();
            aiFighter.SpawnInRing(FightClubArena, World,Player, toPick,FightClub,new DispatchablePerson("a_m_y_methhead_01", 0,0), Targetable, fightersSpawned);//use the built in stuff here, get gans or other stuffo
            Fighters.Add(aiFighter);
            AIFighters.Add(aiFighter);
            EntryPoint.WriteToConsole($"SPAWNED FIGHTER fightersSpawned{fightersSpawned} FightersToSpawn{FightersToSpawn}");
        }
        return true;
    }
}

