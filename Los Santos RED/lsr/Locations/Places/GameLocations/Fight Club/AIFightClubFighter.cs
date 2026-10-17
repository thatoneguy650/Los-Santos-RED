using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AIFightClubFighter : FightClubFighter
{

    private ITargetable Targetable;
    public override bool IsPlayer => false;
    public PedExt PedExt { get; private set; }
    public GangMember GangMember { get; private set; }
    public bool WasPreviousWinner { get; set; }
    public Gang Gang { get; set; }

    public void SpawnInRing(FightClubArena fightClubArena, IEntityProvideable world, IFightClubable player, SpawnPlace spawnPlace, FightClub fightClub, DispatchablePerson dispatchablePerson,
        ITargetable targetable, int spawnedOrder)
    {
        FightClubArena = fightClubArena;
        Player = player;
        Targetable = targetable;
        SpawnPlace = spawnPlace;
        SpawnLocation spawnLocation = new SpawnLocation(spawnPlace.Position, spawnPlace.Heading);
        if(Gang == null)
        {
            PedExt = Player.Dispatcher.SpawnCivilian(spawnLocation, null, dispatchablePerson, fightClub, null);
        }
        else
        {
            GangMember = Player.Dispatcher.GangDispatcher.SpawnGangMember(spawnLocation, Gang,true, true,null, dispatchablePerson, fightClub, true);
            PedExt = GangMember;
        }   
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        PedExt.CanBeAmbientTasked = false;
        PedExt.CanBeTasked = false;
        PedExt.CanBeIdleTasked = false;
        PedExt.WillFight = true;
        PedExt.WillCallPolice = false;
        PedExt.WillCower = false;


        PedExt.DisableCrimeCheck = true;
        PedExt.DisableUnconsciousBodyAlerts = true;
        PedExt.IsManuallyDeleted = true;
        PedExt.Pedestrian.IsPersistent = true;
        PedExt.Pedestrian.RelationshipGroup = new RelationshipGroup($"FIGHTER{spawnedOrder}");
        PedExt.Pedestrian.BlockPermanentEvents = true;
        PedExt.Pedestrian.KeepTasks = true;
        //PedExt.Pedestrian.Tasks.StandStill(1500000);




        PutInMeleeAnimation();


        //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedExt.Pedestrian, "melee@unarmed@base", "melee_intro_ai", 4.0f, -4.0f, -1, 1, 0, false, false, false);//-1
        //NativeFunction.Natives.TASK_PUT_PED_DIRECTLY_INTO_MELEE(PedExt.Pedestrian,)


    }
    private void PutInMeleeAnimation()
    {
        if(PedExt != null && PedExt.IsAnimal)
        {
            return;
        }
        AnimationDictionary.RequestAnimationDictionay("melee@unarmed@base");
        AnimationDictionary.RequestAnimationDictionay("melee@unarmed@streamed_background");
        //
        GameFiber.Sleep(100);
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "melee@unarmed@base", "melee_intro_ai", 4.0f, -4.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "melee@unarmed@streamed_background", "idle", 4.0f, -4.0f, -1, 1, 0, false, false, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedExt.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
    }
    public override void Setup()
    {
        
    }
    public override void StartFight(FightClubFight fightClubFight)
    {
        PedExt.WillFight = true;
        PedExt.WillFightPolice = true;
        PedExt.CanBeTasked = false;
        PedExt.WillCower = false;
        PedExt.CurrentTask = new GeneralFight(PedExt, PedExt, Targetable, fightClubFight);
        PedExt.CurrentTask.Start();
    }
    public override void Update()
    {
        if(PedExt == null || HasLost)
        {
            return;
        }
        if(!PedExt.Pedestrian.Exists())
        {
            HasLost = true;
            return;
        }
        PedExt.CurrentTask?.Update();
        CheckLoseConditions(PedExt.Pedestrian.DistanceTo(FightClubArena.ArenaCenter));
        base.Update();
    }
    public void RestoreForNewFight(int fighterNumber)
    {
        HasLost = false;
        IsOutsideRing = false;
        GameTimeLeftRing = 0;
        if (PedExt == null)
        {
            return;
        }
        if (!PedExt.Pedestrian.Exists())
        {
            HasLost = true;
            return;
        }
        PedExt.Pedestrian.Health = PedExt.Pedestrian.MaxHealth;
        PedExt.Pedestrian.IsPersistent = true;
        PedExt.Pedestrian.RelationshipGroup = new RelationshipGroup($"FIGHTER{fighterNumber}");
        if( SpawnPlace == null)
        {
            return;
        }
        PedExt.Pedestrian.Position = SpawnPlace.Position;
        PedExt.Pedestrian.Heading = SpawnPlace.Heading;
        PutInMeleeAnimation();
        //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedExt.Pedestrian, "melee@unarmed@base", "melee_intro_ai", 4.0f, -4.0f, -1, (int)(eAnimationFlags.), 0, false, false, false);//-1

    }
    public void CheckLoseConditions(float distance)
    {
        if(HasLost)
        {
            return;
        }
        if(!PedExt.Pedestrian.Exists() || PedExt.IsDead || PedExt.IsUnconscious)
        {
            HasLost = true;
            EntryPoint.WriteToConsole("FIGHTER LOST SINCE THEY ARE DEAD GONE OR UNCONSCIOUS");
            return;
        }
        if (distance >= 20f)
        {
            if (!IsOutsideRing)
            {
                IsOutsideRing = true;
                GameTimeLeftRing = Game.GameTime;
                EntryPoint.WriteToConsole("FIGHTER LEFT THE RING");
            }
        }
        else
        {
            if (IsOutsideRing)
            {
                IsOutsideRing = false;
                GameTimeLeftRing = 0;
                EntryPoint.WriteToConsole("FIGHTER WENT BACK INTO THE RING");
            }
        }
        if (GameTimeOutsideRing >= 5000 && !HasLost)
        {
            HasLost = true;
            EntryPoint.WriteToConsole("FIGHTER LEFT THE RING TOO LONG, FAIL");
        }
    }
    public override void Dispose()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(PedExt.Pedestrian);
        PedExt.Pedestrian.IsPersistent = false;
        PedExt.CanBeAmbientTasked = true;
        PedExt.CanBeTasked = true;
        PedExt.CanBeIdleTasked = true;
        PedExt.IsManuallyDeleted = false;
        PedExt.PedAlerts.Reset();
        PedExt.PedViolations.Reset();
        PedExt.PedPerception.Reset();
        PedExt.PlayerPerception.Reset();
        
        if (GangMember != null && Gang != null)
        {
            PedExt.Pedestrian.RelationshipGroup = new RelationshipGroup(Gang.ID);
        }
        else
        {
            PedExt.Pedestrian.RelationshipGroup = new RelationshipGroup("CIVMALE");
        }
        PedExt.ClearTasks(true);
        PedExt.Pedestrian.BlockPermanentEvents = true;
        PedExt.PedBrain.AssignIdleTask();
        PedExt.Pedestrian.BlockPermanentEvents = true;
        PedExt.DisableCrimeCheck = false;
        
        base.Dispose();
        EntryPoint.WriteToConsole($"AI FIGHTER DISPOSE RAN FOR {PedExt.Name}");
    }

    public void SetPassive()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        if (GangMember != null && Gang != null)
        {
            PedExt.Pedestrian.RelationshipGroup = new RelationshipGroup(Gang.ID);
        }
        else
        {
            PedExt.Pedestrian.RelationshipGroup = new RelationshipGroup("CIVMALE");
        }
        PedExt.ClearTasks(true);
        PedExt.CurrentTask = null;
        PedExt.Pedestrian.IsPersistent = true;
        PedExt.WillFight = false;
        PedExt.WillCower = false;
        PedExt.Pedestrian.BlockPermanentEvents = true;
        PedExt.Pedestrian.KeepTasks = true;

        PedExt.PedAlerts.Reset();
        PedExt.PedViolations.Reset();
        PedExt.PedPerception.Reset();
        PedExt.PlayerPerception.Reset();

        if (SpawnPlace == null)
        {
            PedExt.Pedestrian.Tasks.StandStill(1500000);
        }
        else
        {
            PedExt.Pedestrian.Tasks.GoStraightToPosition(SpawnPlace.Position, 1.0f, SpawnPlace.Heading, 1.0f, 5000);
        }    
        //PedExt.PedBrain.AssignIdleTask();
        EntryPoint.WriteToConsole($"SET PASSIVE RAN FOR {PedExt.Name}");
    }
}

