using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Linq;

public class PickPocket : Interaction
{
    private readonly IInteractionable Player;
    private readonly ITargetable TargetPlayer;
    private readonly PedExt Target;
    private readonly ISettingsProvideable Settings;
    private readonly ICrimes Crimes;
    private readonly IEntityProvideable World;
    private readonly string AnimDict = "anim@scripted@player@freemode@tun_prep_grab_midd_ig3@male@";
    private readonly string AnimName = "tun_prep_grab_midd_ig3";
    private uint GameTimeStartedPickpocketing;
    private bool isSuccess;
    private bool canPickpocket;
    private bool isCleanedUp;
    private bool isStarting;
    private bool isDetected;

    public PickPocket(IInteractionable player, ITargetable targetPlayer, PedExt target, ISettingsProvideable settings, ICrimes crimes, IEntityProvideable world)
    {
        Player = player;
        TargetPlayer = targetPlayer;
        Target = target;
        Settings = settings;
        Crimes = crimes;
        World = world;

        canPickpocket = true;
        isCleanedUp = false;
        isStarting = false;
        isSuccess = false;
        //EntryPoint.WriteToConsole($"Pickpocket: Initialized for ped {Target.Pedestrian.Handle:X8}, MaxDistance={MaxDistance}, AllowPedPickPockets={Settings.SettingsManager.ActivitySettings.AllowPedPickPockets}, SuccessRate={SuccessRate:F2}, DetectionChance={DetectionChance:F2}, IsGangMember={Target.IsGangMember}, IsCop={Target.IsCop}, PedType={Target.GetType().Name}");
    }

    public override string DebugString => $"Pickpocketing {Target.Pedestrian.Handle:X8} Success={isSuccess} Detected={isDetected}";
    public override bool CanPerformActivities { get; set; } = true;
    public override void Dispose()
    {
        if (!isCleanedUp)
        {
            CleanUp();
        }
    }
    public override void Start()
    {
        if (isStarting)
        {
            EntryPoint.WriteToConsole($"Pickpocket: Blocked, already starting for ped {Target.Pedestrian.Handle:X8}");
            CleanUp();
            return;
        }
        isStarting = true;

        if (!CanStartPickpocketing())
        {
            CleanUp();
            return;
        }

        GameFiber.StartNew(() =>
        {
            try
            {
                Setup();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole($"Pickpocket: Error in Start - {ex.Message} {ex.StackTrace}");
                CleanUp();
            }
        }, $"PickpocketStart_{Target.Pedestrian.Handle:X8}");
    }
    private bool CanStartPickpocketing()
    {
        if(!Target.Pedestrian.Exists())
        {
            return false;
        }
        if (!canPickpocket || !Settings.SettingsManager.ActivitySettings.AllowPedPickPockets || !Player.ActivityManager.CanPickpocketLookedAtPed || Player.ActivityManager.IsPerformingActivity)
        {
            //EntryPoint.WriteToConsole($"Pickpocket: Blocked, CanPickpocket={canPickpocket}, AllowPedPickPockets={Settings.SettingsManager.ActivitySettings.AllowPedPickPockets}, CanPickpocketLookedAtPed={canPickpocketLookedAtPed}, IsPerformingActivity={isPlayerPerformingActivity}, PedExists={isPedValid}");
            return false;
        }
        if (Target.IsInVehicle || Target.DistanceToPlayer > Settings.SettingsManager.ActivitySettings.PickPocketDistance || Target.IsUnconscious || Target.IsDead || Target.HasBeenMugged)
        {
            Game.DisplayHelp("Cannot pickpocket: Invalid target state");
            //EntryPoint.WriteToConsole($"Pickpocket: Cannot start, Exists={isPedValid}, IsInVehicle={isPedInVehicle}, Distance={distanceToPed:F2}, MaxDistance={MaxDistance}, IsUnconscious={isPedUnconscious}, IsDead={isPedDead}, HasBeenMugged={hasBeenMugged}");
            return false;
        }
        return true;
    }
    private void Setup()
    {
        if (!Target.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"Pickpocket: Setup failed, ped {Target.Pedestrian.Handle:X8} does not exist");
            CleanUp();
            return;
        }
        //Target.HasBeenMugged = true;
        //EntryPoint.WriteToConsole($"Pickpocket: Attempt registered, future attempts blocked for ped {Target.Pedestrian.Handle:X8}");
        AnimationDictionary.RequestAnimationDictionay(AnimDict);
        GameTimeStartedPickpocketing = Game.GameTime;
        canPickpocket = false;
        Player.ActivityManager.IsPickPocketing = true;
        //EntryPoint.WriteToConsole($"Pickpocket: Setup for ped {Target.Pedestrian.Handle:X8}, Money={Target.Money}, IsGangMember={Target.IsGangMember}, IsCop={Target.IsCop}, TaskStatus={NativeFunction.Natives.GET_SCRIPT_TASK_STATUS<int>(Target.Pedestrian, 0x811455F8)}, CrimesWitnessed={Target.PlayerCrimesWitnessed.Count}, WillCallPolice={Target.WillCallPolice}, WillCallPoliceIntense={Target.WillCallPoliceIntense}, PedType={Target.GetType().Name}");
        PerformPickpocket();
    }
    private void PerformPickpocket()
    {
        try
        {
            if (!IsValidState())
            {
                Game.DisplayHelp("Pickpocket failed: Invalid state");
                //EntryPoint.WriteToConsole($"Pickpocket: Failed, Distance={Target.DistanceToPlayer:F2}, MaxDistance={MaxDistance}, IsAliveAndFree={TargetPlayer.IsAliveAndFree}, PedType={Target.GetType().Name}, CrimesViolating={string.Join(",", Player.Violations.CrimesViolating)}");
                CleanUp();
                return;
            }

            Crime pickpocketCrime = Crimes.GetCrime(StaticStrings.PickPocketingCrimeID);
            if (pickpocketCrime == null)
            {
                Game.DisplayHelp("Pickpocket failed: Crime not found");
                //EntryPoint.WriteToConsole($"Pickpocket: Crime {StaticStrings.PickPocketingCrimeID} not found, CrimesViolating={string.Join(",", Player.Violations.CrimesViolating)}");
                CleanUp();
                return;
            }

            DetermineOutcome();


            if (isDetected)
            {
                Player.Violations.SetContinuouslyViolating(pickpocketCrime.ID);
            }


            PlayAnimation();
            if (!WaitForAnimation())
            {
                Game.DisplayHelp("Pickpocket failed: Animation not loaded!");
                //EntryPoint.WriteToConsole($"Pickpocket: Animation {AnimDict}/{AnimName} failed to play for ped {Target.Pedestrian.Handle:X8}, CrimesViolating={string.Join(",", Player.Violations.CrimesViolating)}");
                CleanUp();
                return;
            }

            if (!IsValidState())
            {
                Game.DisplayHelp("Pickpocket failed: Invalid state");
                //EntryPoint.WriteToConsole($"Pickpocket: Failed post-animation, Distance={Target.DistanceToPlayer:F2}, MaxDistance={MaxDistance}, PedType={Target.GetType().Name}, CrimesViolating={string.Join(",", Player.Violations.CrimesViolating)}");
                CleanUp();
                return;
            }
            Player.Violations.StopContinuouslyViolating(pickpocketCrime.ID);
            
            if (isSuccess)
            {
                CreateMoneyDrop();
                Target.HasBeenMugged = true;
                //EntryPoint.WriteToConsole($"Pickpocket: Successful pickpocket of {(Target.IsGangMember ? $"gang member {(Target as GangMember)?.Gang?.ShortName ?? "Unknown"}" : $"non-gang member, IsCop={Target.IsCop}")}, ped {Target.Pedestrian.Handle:X8}, PedType={Target.GetType().Name}, CrimesViolating={string.Join(",", Player.Violations.CrimesViolating)}");
            }
            else if (isDetected)
            {
                Game.DisplayHelp("Pickpocketing failed! Detected!");
                //Player.PlaySpeech("GENERIC_CURSE_MED", false);
                Target.PlaySpeech("GENERIC_INSULT_MED", false);
                //Player.Violations.AddViolating(pickpocketCrime.ID);
                //Target.WillCallPolice = true;
                Target.OnPlayerFailedPickpocketing(Player);
                //EntryPoint.WriteToConsole($"Pickpocket: Failed and detected pickpocket of {(Target.IsGangMember ? $"gang member {(Target as GangMember)?.Gang?.ShortName ?? "Unknown"}" : $"non-gang member, IsCop={Target.IsCop}")}, ped {Target.Pedestrian.Handle:X8}, CrimesWitnessed={Target.PlayerCrimesWitnessed.Count}, HasSeenMundaneCrime={Target.PedReactions.HasSeenMundaneCrime}, CurrentTask={Target.CurrentTask?.Name}, PedType={Target.GetType().Name}, CrimesViolating={string.Join(",", Player.Violations.CrimesViolating)}");
            }
            else
            {
                Game.DisplayHelp("Pickpocketing failed! Not detected.");
                //Player.PlaySpeech("GENERIC_CURSE_MED", false);
                //EntryPoint.WriteToConsole($"Pickpocket: Failed but not detected pickpocket of {(Target.IsGangMember ? $"gang member {(Target as GangMember)?.Gang?.ShortName ?? "Unknown"}" : $"non-gang member, IsCop={Target.IsCop}")}, ped {Target.Pedestrian.Handle:X8}, PedType={Target.GetType().Name}, CrimesViolating={string.Join(",", Player.Violations.CrimesViolating)}");
            }
            CleanUp();
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Pickpocket: Error in PerformPickpocket - {ex.Message} {ex.StackTrace}, CrimesViolating={string.Join(",", Player.Violations.CrimesViolating)}");
            CleanUp();
        }
    }
    private void DetermineOutcome()
    {
        isSuccess = RandomItems.RandomPercent(Settings.SettingsManager.ActivitySettings.PickPocketSuccessRate);
        EntryPoint.WriteToConsole($"isSuccess {isSuccess} PickPocketSuccessRate{Settings.SettingsManager.ActivitySettings.PickPocketSuccessRate}");
        isDetected = false;
        if (isSuccess)
        {     
            return;
        }
        isDetected = RandomItems.RandomPercent(Settings.SettingsManager.ActivitySettings.PickPocketBaseDetectionChance * Target.PickpocketDetectionMultiplier);
    }
    private bool IsValidState()
    {
        return Target.Pedestrian.Exists() && Target.DistanceToPlayer <= Settings.SettingsManager.ActivitySettings.PickPocketDistance && TargetPlayer.IsAliveAndFree;
    }
    private void PlayAnimation()
    {
        Vector3 position = TargetPlayer.Character.Position;
        Vector3 rotation = NativeFunction.Natives.GET_ENTITY_ROTATION<Vector3>(TargetPlayer.Character, 2);
        NativeFunction.Natives.TASK_PLAY_ANIM_ADVANCED(TargetPlayer.Character, AnimDict, AnimName, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 4.0f, -4.0f, 800, 49, 0.0f, 0.0f, false, false);
    }
    private bool WaitForAnimation()
    {
        uint animationStartTime = Game.GameTime;
        while (!NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(TargetPlayer.Character, AnimDict, AnimName, 3) && Game.GameTime - animationStartTime < 500)
        {
            GameFiber.Yield();
        }
        if (!NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(TargetPlayer.Character, AnimDict, AnimName, 3))
        {
            return false;
        }
        while (Game.GameTime - animationStartTime < 800 && IsValidState())
        {
            GameFiber.Wait(100);
            GameFiber.Yield();
        }
        return true;
    }
    private void CreateMoneyDrop()
    {
        try
        {
            int money = Target.Money;
            if (money <= 0)
            {
                Game.DisplayHelp("Pickpocket failed: Target has no money!");
                EntryPoint.WriteToConsole($"Pickpocket: No money to drop for ped {Target.Pedestrian.Handle:X8}, Money={money}");
                return;
            }
            Target.Money = 0;
            if (!IsValidState())
            {
                Game.DisplayHelp("Pickpocket failed: Invalid state!");
                EntryPoint.WriteToConsole($"Pickpocket: Pickup creation skipped, PedExists={Target.Pedestrian.Exists()}, PlayerExists={true}");
                return;
            }

            NativeFunction.Natives.SET_PED_MONEY(Target.Pedestrian, 0);
            uint modelHash = Game.GetHashKey("PICKUP_MONEY_WALLET");
            Vector3 moneyPos = Target.Pedestrian.Position.Around2D(0.5f, 1.5f);

            if (Target.IsGangMember)
            {
                modelHash = Game.GetHashKey("PICKUP_MONEY_VARIABLE");
                int moneyPickupCreated = 0;
                int pickupsToCreate = Math.DivRem(money, 500, out int remainder);
                if (remainder > 0)
                {
                    pickupsToCreate++;
                }
                for (int i = 0; i < pickupsToCreate; i++)
                {
                    int moneyToDrop = money - moneyPickupCreated;
                    if (moneyToDrop > 500)
                    {
                        moneyToDrop = 500;
                    }
                    moneyPos = Target.Pedestrian.Position.Around2D(0.5f, 1.5f);
                    NativeFunction.Natives.CREATE_AMBIENT_PICKUP(modelHash, moneyPos.X, moneyPos.Y, moneyPos.Z, 0, moneyToDrop, 1, false, true);
                    moneyPickupCreated += moneyToDrop;
                }
            }
            else
            {
                if (Target.IsMerchant)
                {
                    modelHash = Game.GetHashKey("PICKUP_MONEY_DEP_BAG");
                }
                NativeFunction.Natives.CREATE_AMBIENT_PICKUP(modelHash, moneyPos.X, moneyPos.Y, moneyPos.Z, 0, money, 1, false, true);
            }

            string description = $"Cash Stolen:~n~~g~${money}~s~";
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~r~Ped Pickpocketed", $"~y~{Target.Name}", description);
            //EntryPoint.WriteToConsole($"Pickpocket: Created money drop for ped {Target.Pedestrian.Handle:X8}, Money={money}, TaskStatus={NativeFunction.Natives.GET_SCRIPT_TASK_STATUS<int>(Target.Pedestrian, 0x811455F8)}, WillCallPolice={Target.WillCallPolice}, HasBeenMugged={Target.HasBeenMugged}, CrimesWitnessed={Target.PlayerCrimesWitnessed.Count}, PedType={Target.GetType().Name}");
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Pickpocket: Error in CreateMoneyDrop - {ex.Message} {ex.StackTrace}");
            CleanUp();
        }
    }
    private void CleanUp()
    {
        if (isCleanedUp)
        {
            return;
        }
        isCleanedUp = true;

        if (Target.Pedestrian.Exists())
        {
            Target.CanBeTasked = true;
            Target.CanBeAmbientTasked = true;
            if (isSuccess || !isDetected)
            {
                if (Target.CurrentTask != null)
                {
                    Target.CurrentTask.Stop();
                    Target.CurrentTask = null;
                }
                //int taskStatus = NativeFunction.Natives.GET_SCRIPT_TASK_STATUS<int>(Target.Pedestrian, 0x811455F8);
                //if (taskStatus == 7)
                //{
                //    EntryPoint.WriteToConsole($"Pickpocket: WARNING: Ped {Target.Pedestrian.Handle:X8} has no active task after cleanup, IsGangMember={Target.IsGangMember}, IsCop={Target.IsCop}");
                //}
                //EntryPoint.WriteToConsole($"Pickpocket: Restored tasking for ped {Target.Pedestrian.Handle:X8}, IsGangMember={Target.IsGangMember}, IsCop={Target.IsCop}, TaskStatus={taskStatus}, WillCallPolice={Target.WillCallPolice}, HasBeenMugged={Target.HasBeenMugged}, CrimesWitnessed={Target.PlayerCrimesWitnessed.Count}");
            }
        }

        if (Player.ActivityManager != null)
        {
            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.IsPickPocketing = false;
            Player.ButtonPrompts.RemovePrompts("Pickpocket");
        }
        Player.Violations.StopContinuouslyViolating(StaticStrings.PickPocketingCrimeID);
        canPickpocket = true;
        isStarting = false;
        EntryPoint.WriteToConsole($"Pickpocket: Cleaned up for ped {Target.Pedestrian.Handle:X8}, CrimesViolating={string.Join(",", Player.Violations.CrimesViolating)}");
    }
}