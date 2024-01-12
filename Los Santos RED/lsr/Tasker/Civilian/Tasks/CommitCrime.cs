using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class CommitCrime : ComplexTask
{
    private bool NeedsVictim;
    private int NewTargets = 0;
    private uint PreviousTargetHandle;
    private string SelectedCrime;
    private Ped Target;
    private WeaponInformation ToIssue;
    private IWeapons Weapons;
    private IEntityProvideable World;
    private uint GameTimeTimeStartedDealingDrugs;
    private uint GameTimeTimeStartedHarassing;

    public CommitCrime(IComplexTaskable ped, ITargetable player, IWeapons weapons, IEntityProvideable world) : base(player, ped, 2000)
    {
        Name = "CommitCrime";
        SubTaskName = "";
        Weapons = weapons;
        World = world;

    }
    public bool IsTrafficOnly { get; set; } = false;
    private bool IsPlayerTarget => Target.Exists() && Player.Character.Exists() && Target.Handle == Player.Character.Handle;
    public override void Start()
    {
        DetermineCrime();
        IssueWeapon();
        StartCrimeTask();
        GameTimeLastRan = Game.GameTime;
    }
    public override void Stop()
    {
    }
    public override void Update()
    {
        if (NeedsVictim && (!Target.Exists() || (Target.Exists() && Target.IsDead)) && NewTargets < 3)
        {
            StartCrimeTask();
        }
        if(GameTimeTimeStartedDealingDrugs != 0 && Game.GameTime - GameTimeTimeStartedDealingDrugs >= 5000)
        {
            Ped.IsDealingDrugs = false;
            GameTimeTimeStartedDealingDrugs = 0;
        }
        if (GameTimeTimeStartedHarassing != 0 && Game.GameTime - GameTimeTimeStartedHarassing >= 5000)
        {
            Ped.IsSuspicious = false;
            GameTimeTimeStartedHarassing = 0;
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void ReTask()
    {

    }
    private void AttackTarget()
    {
        if (Ped.Pedestrian.Exists() && Target.Exists())
        {
            if (!IsPlayerTarget)
            {
                NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Target, (int)eCombatAttributes.BF_AlwaysFight, true);
                NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Target, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
                NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(Target, 0, false);
            }
            if (Target.IsInAnyVehicle(false) && Target.CurrentVehicle.Exists())
            {
                Vehicle TargetVehicle = Target.CurrentVehicle;
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, TargetVehicle, -1, -1, 15.0f, 9);
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, TargetVehicle, 25f, (int)eCustomDrivingStyles.Code3, 25f);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
            else
            {
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Target, 0, 16);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
            GameTimeLastRan = Game.GameTime;
        }
    }
    private void DealDrugsToTarget()
    {
        if (Ped.Pedestrian.Exists() && Target.Exists())
        {
            Target.Tasks.StandStill(10000);
            GameTimeTimeStartedDealingDrugs = Game.GameTime;
            Ped.IsDealingDrugs = true;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Target, -1, 5f, 1.2f, 2f, 0); //Original and works ok//7f
                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Target, 2000);
                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Target, -1, 0, 2);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void DetermineCrime()
    {
        List<string> PossibleCrimes = new List<string>();
        if (Ped != null)
        {
            if (Ped.IsInVehicle || IsTrafficOnly)
            {
                PossibleCrimes.AddRange(new List<string>() { "FelonySpeeding", "HitCarWithCar", "DrunkDriving" });
            }
            else
            {
                PossibleCrimes.AddRange(new List<string>() { "AssaultingCivilians", "BrandishingWeapon", "FiringWeapon", "AssaultingWithDeadlyWeapon", "PublicIntoxication", "GrandTheftAuto", "Harassment", "AttemptingSuicide", "KillingCivilians", "TerroristActivity", "DealingDrugs" });
            }
        }
        SelectedCrime = PossibleCrimes.PickRandom();
        //EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Crime Picked {SelectedCrime}", 5);
    }
    private void DriveAroundDrunk()
    {
        if (Ped.Pedestrian.Exists())
        {
            Ped.IsDrunk = true;
            string CurrentClipset = "move_m@drunk@verydrunk";
            NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Ped.Pedestrian, true);
            if (CurrentClipset != "NONE")
            {
                if (!NativeFunction.CallByName<bool>("HAS_ANIM_SET_LOADED", CurrentClipset))
                {
                    NativeFunction.CallByName<bool>("REQUEST_ANIM_SET", CurrentClipset);
                }
                NativeFunction.CallByName<bool>("SET_PED_MOVEMENT_CLIPSET", Ped.Pedestrian, CurrentClipset, 0x3E800000);
            }
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Ped.Pedestrian, (int)PedConfigFlags.PED_FLAG_DRUNK, true);
            if (Ped.Pedestrian.CurrentVehicle.Exists())
            {
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 35f, (int)eCustomDrivingStyles.DrunkDriving, 10f);
            }
        }
    }
    private void DriveRecklessly()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists())
        {
            Ped.IsDrivingRecklessly= true;
            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 90f, (int)eCustomDrivingStyles.RecklessDriving, 10f);
        }
    }
    private void DriveAtSpeed()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists())
        {
            Ped.IsSpeeding = true;
            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 42f, (int)eCustomDrivingStyles.SpeedingDriving, 10f);
        }
    }
    private void GetNewVictim()
    {
        if (Ped.Pedestrian.Exists())
        {
            float closestDistance = 999f;
            PedExt closestTarget = null;
            Target = null;
            foreach (PedExt possibletarget in World.Pedestrians.CivilianList)
            {
                if (possibletarget.Pedestrian.Exists() && possibletarget.Pedestrian.IsAlive && possibletarget.Pedestrian.Handle != Ped.Pedestrian.Handle && PreviousTargetHandle != possibletarget.Pedestrian.Handle && possibletarget.DistanceToPlayer <= 85 && possibletarget.CanBeAmbientTasked && possibletarget.Pedestrian.Speed <= 2.0f && !possibletarget.IsGangMember && possibletarget.Pedestrian.IsAlive)
                {
                    float distanceToPossibleTarget = possibletarget.Pedestrian.DistanceTo2D(Ped.Pedestrian);
                    if (distanceToPossibleTarget <= closestDistance)
                    {
                        closestDistance = distanceToPossibleTarget;
                        closestTarget = possibletarget;
                    }
                }
            }
            if (closestTarget != null && closestTarget.Pedestrian.Exists())
            {
                if (closestDistance <= Ped.DistanceToPlayer)
                {
                    Target = closestTarget.Pedestrian;
                }
                else
                {
                    Target = Player.Character;
                }
            }
            if (Target.Exists())
            {
                PreviousTargetHandle = Target.Handle;
                NewTargets++;
                //EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Got New Target {Target.Handle}", 5);
            }
        }
    }
    private void HarassTarget()
    {
        if (Ped.Pedestrian.Exists() && Target.Exists())
        {
            GameTimeTimeStartedHarassing = Game.GameTime;
            Ped.IsSuspicious = true;
            Target.Tasks.StandStill(10000);
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Target, -1, 5f, 1.2f, 2f, 0); //Original and works ok//7f
                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Target, 2000);
                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Target, -1, 0, 2);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void IssueWeapon()
    {
        List<string> WeaponCrimes = new List<string>() { "FiringWeapon", "AssaultingWithDeadlyWeapon", "KillingCivilians", "TerroristActivity", "AttemptingSuicide" };
        bool equipNow = false;
        if (WeaponCrimes.Contains(SelectedCrime))
        {
            ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol);
            equipNow = true;
        }
        else
        {
            if (RandomItems.RandomPercent(30))
            {
                ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol);
                equipNow = false;
            }
            else
            {
                if (RandomItems.RandomPercent(30))
                {
                    ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
                    equipNow = false;
                }
            }
        }
        if (ToIssue != null)
        {
            Ped.Pedestrian.Inventory.GiveNewWeapon(ToIssue.Hash, ToIssue.AmmoAmount, equipNow);
            //EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Issued Weapon {ToIssue.ModelName}", 5);
        }
    }
    private void ShootYourself()
    {
        Ped.IsSuicidal = true;
        if (Ped.Pedestrian.Exists())
        {
            if (ToIssue != null)
            {
                Ped.Pedestrian.Inventory.GiveNewWeapon(ToIssue.Hash, ToIssue.AmmoAmount, true);
            }
            NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
        }
    }
    private void StartCrimeTask()
    {
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;

            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
            NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(Ped.Pedestrian, 0, false);
        }
        List<string> VictimCrimes = new List<string>() { "AssaultingCivilians", "FiringWeapon", "AssaultingWithDeadlyWeapon", "GrandTheftAuto", "Harassment", "KillingCivilians", "TerroristActivity", "DealingDrugs" };
        if (VictimCrimes.Contains(SelectedCrime))
        {
            NeedsVictim = true;
            GetNewVictim();
            if (Target.Exists())
            {
                List<string> AttackCrimes = new List<string>() { "AssaultingCivilians", "FiringWeapon", "AssaultingWithDeadlyWeapon", "GrandTheftAuto", "KillingCivilians", "TerroristActivity" };
                if (AttackCrimes.Contains(SelectedCrime))
                {
                    EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Crime Picked {SelectedCrime} AttackTarget Ran", 5);
                    AttackTarget();
                }
                else if (SelectedCrime == "DealingDrugs")
                {
                    EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Crime Picked {SelectedCrime} DealDrugsToTarget Ran", 5);
                    DealDrugsToTarget();
                }
                else if (SelectedCrime == "Harassment")
                {
                    EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Crime Picked {SelectedCrime} HarassTarget Ran", 5);
                    HarassTarget();
                }
            }
        }
        else
        {
            if (SelectedCrime == "PublicIntoxication")
            {
                EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Crime Picked {SelectedCrime} WalkAroundDrunk Ran", 5);
                WalkAroundDrunk();
            }
            else if (SelectedCrime == "DrunkDriving")
            {
                EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Crime Picked {SelectedCrime} DrunkDriving Ran", 5);
                DriveAroundDrunk();
            }
            else if (SelectedCrime == "HitCarWithCar")
            {
                EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Crime Picked {SelectedCrime} DriveRecklessly Ran", 5);
                DriveRecklessly();
            }
            else if (SelectedCrime == "FelonySpeeding")
            {
                EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Crime Picked {SelectedCrime} DriveAtSpeed Ran", 5);
                DriveAtSpeed();
            }
            else if (SelectedCrime == "AttemptingSuicide")
            {
                EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Crime Picked {SelectedCrime} ShootYourself Ran", 5);
                ShootYourself();
            }
            else if (SelectedCrime == "BrandishingWeapon")
            {
                EntryPoint.WriteToConsole($"CommitCrime: {Ped.Pedestrian} Crime Picked {SelectedCrime} WalkAroundWithGun Ran", 5);
                WalkAroundWithGun();
            }
        }
    }
    private void WalkAroundDrunk()
    {
        Ped.IsDrunk = true;
        string CurrentClipset = "move_m@drunk@verydrunk";
        NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Ped.Pedestrian, true);
        if (!NativeFunction.CallByName<bool>("HAS_ANIM_SET_LOADED", CurrentClipset))
        {
            NativeFunction.CallByName<bool>("REQUEST_ANIM_SET", CurrentClipset);
        }
        NativeFunction.CallByName<bool>("SET_PED_MOVEMENT_CLIPSET", Ped.Pedestrian, CurrentClipset, 0x3E800000);    
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Ped.Pedestrian, (int)PedConfigFlags.PED_FLAG_DRUNK, true);
        NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
    }
    private void WalkAroundWithGun()
    {
        if (Ped.Pedestrian.Exists())
        {
            if (ToIssue != null)
            {
                Ped.Pedestrian.Inventory.GiveNewWeapon(ToIssue.Hash, ToIssue.AmmoAmount, true);
            }
            NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
        }
    }
}