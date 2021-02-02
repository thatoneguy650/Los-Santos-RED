using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class Cop : PedExt
{

    private readonly List<string> DeadlyChaseSpeech = new List<string> { "DRAW_GUN", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL", "GET_HIM", "REQUEST_BACKUP", "REQUEST_NOOSE", "SHOOTOUT_OPEN_FIRE" };
    private readonly List<string> SuspectBusted = new List<string> { "WON_DISPUTE", "ARREST_PLAYER" };
    private readonly List<string> SuspectDown = new List<string> { "SUSPECT_KILLED", "SUSPECT_KILLED_REPORT" };
    private readonly List<string> UnarmedChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "SUSPECT_SPOTTED" };
    private readonly List<string> IdleSpeech = new List<string> { "CHAT_STATE", "CHAT_RESP" };
    private readonly List<string> AngrySpeech = new List<string> { "CHALLENGE_THREATEN", "COMBAT_TAUNT", "FIGHT", "GENERIC_SHOCKED_HIGH", "GENERIC_WAR_CRY", "PINNED_DOWN", "GENERIC_INSULT_HIGH", "GET_HIM" };

    private uint GameTimeLastRadioed;
    private uint GameTimeLastSpoke;
    private uint GameTimeLastWeaponCheck;
    private uint GameTimeSpawned;
    private bool IsSetDeadly;
    private bool IsSetLessLethal;
    private bool IsSetUnarmed;
    private IssuableWeapon LongGun;
    private IssuableWeapon Sidearm;
    private bool HasHeavyWeaponOnPerson;
    public Cop(Ped pedestrian, int health, Agency agency, bool wasModSpawned) : base(pedestrian)
    {
        IsCop = true;
        Health = health;
        AssignedAgency = agency;
        WasModSpawned = wasModSpawned;
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
        Pedestrian.VisionRange = 90f;//55F
        Pedestrian.HearingRange = 55;//25
    }
    public Agency AssignedAgency { get; set; } = new Agency();
    public bool CanRadioIn => !IsRadioTimedOut && DistanceToPlayer <= 50f && !IsInVehicle && !RecentlyGotOutOfVehicle && !Pedestrian.IsSwimming && !Pedestrian.IsInCover && !Pedestrian.IsGoingIntoCover && !Pedestrian.IsShooting && !Pedestrian.IsInWrithe && !Pedestrian.IsGettingIntoVehicle && !Pedestrian.IsInAnyVehicle(true) && !Pedestrian.IsInAnyVehicle(false);
    public bool CanSpeak => !IsSpeechTimedOut && DistanceToPlayer <= 50f;
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool HasPistol => Sidearm != null;
    public bool IsRadioTimedOut => GameTimeLastRadioed != 0 && Game.GameTime - GameTimeLastRadioed < 60000;
    public bool IsSpeechTimedOut => GameTimeLastSpoke != 0 && Game.GameTime - GameTimeLastSpoke < 25000;
    public bool NeedsWeaponCheck => GameTimeLastWeaponCheck == 0 || Game.GameTime > GameTimeLastWeaponCheck + 750;
    public bool ShouldAutoSetWeaponState { get; set; } = true;
    public bool ShouldBustPlayer => !IsInVehicle && DistanceToPlayer > 0.1f && DistanceToPlayer <= 5f;
    public bool WasModSpawned { get; private set; }
    public void IssueWeapons()
    {
        Sidearm = AssignedAgency.GetRandomWeapon(true);
        //Game.Console.Print($"Issued: {Sidearm.ModelName} Variation: {string.Join(",", Sidearm.Variation.Components.Select(x => x.Name))}");
        LongGun = AssignedAgency.GetRandomWeapon(false);
        //Game.Console.Print($"Issued: {LongGun.ModelName} Variation: {string.Join(",", LongGun.Variation.Components.Select(x => x.Name))}");
    }
    public void RadioIn(IPoliceRespondable currentPlayer)
    {
        //if (CanRadioIn && currentPlayer.IsWanted)
        //{
        //    string AnimationToPlay = "generic_radio_enter";
        //    //WeaponInformation CurrentGun = DataMart.Instance.Weapons.GetCurrentWeapon(Pedestrian);
        //    //if (CurrentGun != null && CurrentGun.IsOneHanded)
        //    //    AnimationToPlay = "radio_enter";

        //    Speak(currentPlayer);

        //    AnimationDictionary.RequestAnimationDictionay("random@arrests");
        //    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Pedestrian, "random@arrests", AnimationToPlay, 2.0f, -2.0f, -1, 52, 0, false, false, false);
        //    GameTimeLastRadioed = Game.GameTime;
        //}
    }
    public void Speak(IPoliceRespondable currentPlayer)
    {
        //if (CanSpeak)
        //{
        //    if (currentPlayer.IsWanted)
        //    {
        //        if (currentPlayer.IsBusted)
        //        {
        //            Pedestrian.PlayAmbientSpeech(SuspectBusted.PickRandom());
        //        }
        //        else if (currentPlayer.IsDead)
        //        {
        //            Pedestrian.PlayAmbientSpeech(SuspectDown.PickRandom());
        //        }
        //        else
        //        {
        //            if (currentPlayer.PoliceResponse.IsDeadlyChase)
        //            {
        //                if (currentPlayer.PoliceResponse.IsWeaponsFree)
        //                {
        //                    Pedestrian.PlayAmbientSpeech(AngrySpeech.PickRandom());
        //                }
        //                else
        //                {
        //                    Pedestrian.PlayAmbientSpeech(DeadlyChaseSpeech.PickRandom());
        //                }
        //            }
        //            else
        //            {
        //                Pedestrian.PlayAmbientSpeech(UnarmedChaseSpeech.PickRandom());
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Pedestrian.PlayAmbientSpeech(IdleSpeech.PickRandom());
        //    }
        //    GameTimeLastSpoke = Game.GameTime;
        //}
    }
    public void UpdateDrivingFlags()
    {
        NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Pedestrian, 100f);
        NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Pedestrian, 8f);
        NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 32, true);
    }
    public void UpdateLoadout(bool IsDeadlyChase, int WantedLevel)
    {
        if (ShouldAutoSetWeaponState)
        {
            if (IsInVehicle && IsDeadlyChase)
            {
                HasHeavyWeaponOnPerson = true;
            }
            if (IsDeadlyChase)
            {
                if (IsInVehicle && WantedLevel < 4)
                {
                    SetUnarmed();
                }
                else
                {
                    SetDeadly();
                }
            }
            else
            {
                if (WantedLevel != 0)
                {
                    if (IsInVehicle)
                    {
                        SetUnarmed();
                    }
                    else
                    {
                        SetLessLethal();
                    }
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, true);//for idle,
                }
            }
        }
    }
    public void UpdateSpeech(IPoliceRespondable currentPlayer)
    {
        Speak(currentPlayer);
        RadioIn(currentPlayer);
    }
    private void SetDeadly()
    {
        if (Pedestrian.Exists() && Pedestrian.IsAlive && (!IsSetDeadly || NeedsWeaponCheck))
        {
            if (Pedestrian.Inventory != null && !Pedestrian.Inventory.Weapons.Contains(Sidearm.ModelName))
            {
                Pedestrian.Inventory.GiveNewWeapon(Sidearm.ModelName, -1, true);
                Sidearm.ApplyVariation(Pedestrian);
            }
            if (Pedestrian.Inventory != null && !Pedestrian.Inventory.Weapons.Contains(LongGun.ModelName))
            {
                Pedestrian.Inventory.GiveNewWeapon(LongGun.ModelName, -1, true);
                LongGun.ApplyVariation(Pedestrian);
            }
            if (LongGun != null && HasHeavyWeaponOnPerson && !IsInVehicle)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, LongGun.GetHash(), true);
            }
            else
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, Sidearm.GetHash(), true);
            }
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
            //NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 1, true);//can use vehicle in combat
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 2, true);//can do drivebys       
            IsSetLessLethal = false;
            IsSetUnarmed = false;
            IsSetDeadly = true;
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
    private void SetLessLethal()
    {
        if (Pedestrian.Exists() && Pedestrian.IsAlive && (!IsSetLessLethal || NeedsWeaponCheck))
        {
            if (Pedestrian.Inventory != null && !Pedestrian.Inventory.Weapons.Contains(WeaponHash.StunGun))
            {
                Pedestrian.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
            }
            else if (Pedestrian.Inventory != null && Pedestrian.Inventory.EquippedWeapon != WeaponHash.StunGun)
            {
                Pedestrian.Inventory.EquippedWeapon = WeaponHash.StunGun;
            }
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
            //NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 1, false);//cant use vehicle in combat
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 2, false);//cant do drivebys
            IsSetLessLethal = true;
            IsSetUnarmed = false;
            IsSetDeadly = false;
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
    private void SetUnarmed()
    {
        if (Pedestrian.Exists() && Pedestrian.IsAlive && (!IsSetUnarmed || NeedsWeaponCheck))
        {
            if (Pedestrian.Inventory != null && Pedestrian.Inventory.EquippedWeapon != null)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, 2725352035, true);
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
            }
           // NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 1, false);//cant use vehicle in combat
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 2, false);//cant do drivebys
            IsSetLessLethal = false;
            IsSetUnarmed = true;
            IsSetDeadly = false;
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
}