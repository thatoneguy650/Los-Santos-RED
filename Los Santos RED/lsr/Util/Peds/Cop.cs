using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class Cop : PedExt
{
    private readonly List<string> ArrestedWaitSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "GET_HIM", "SUSPECT_SPOTTED", "DRAW_GUN", "GET_HIM", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL" };
    private readonly List<string> CautiousChaseSpeech = new List<string> { "DRAW_GUN", "GET_HIM", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL" };
    private readonly List<string> DeadlyChaseSpeech = new List<string> { "CHALLENGE_THREATEN", "COMBAT_TAUNT", "FIGHT", "GENERIC_INSULT", "GENERIC_WAR_CRY", "GET_HIM", "REQUEST_BACKUP", "REQUEST_NOOSE", "SHOOTOUT_OPEN_FIRE" };
    private readonly List<string> PlayerDeadSpeech = new List<string> { "CHAT_STATE", "CHAT_RESP" };
    private readonly List<string> SuspectBusted = new List<string> { "WON_DISPUTE" };
    private readonly List<string> SuspectDown = new List<string> { "SUSPECT_KILLED", "WON_DISPUTE", "SUSPECT_KILLED_REPORT" };
    private readonly List<string> UnarmedChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "GET_HIM", "SUSPECT_SPOTTED" };
    private uint GameTimeLastRadioed;
    private uint GameTimeLastSpoke;
    private uint GameTimeLastWeaponCheck;
    private uint GameTimeSpawned;
    private bool IsSetDeadly;
    private bool IsSetLessLethal;
    private bool IsSetUnarmed;
    private IssuableWeapon LongGun;
    private IssuableWeapon Sidearm;
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
    public bool CanRadioIn
    {
        get
        {
            if (IsRadioTimedOut)
            {
                return false;
            }
            else if (!IsInVehicle && !Pedestrian.IsSwimming && !Pedestrian.IsInCover && !Pedestrian.IsGoingIntoCover && !Pedestrian.IsShooting && !Pedestrian.IsInWrithe && !Pedestrian.IsGettingIntoVehicle && !Pedestrian.IsInAnyVehicle(true) && !Pedestrian.IsInAnyVehicle(false))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool CanSpeak => !IsSpeechTimedOut;
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool HasPistol => Sidearm != null;
    public bool IsRadioTimedOut => GameTimeLastRadioed != 0 && Game.GameTime - GameTimeLastRadioed < 60000;
    public bool IsSpeechTimedOut => GameTimeLastSpoke != 0 && Game.GameTime - GameTimeLastSpoke < 25000;
    public bool NeedsWeaponCheck => GameTimeLastWeaponCheck == 0 || Game.GameTime > GameTimeLastWeaponCheck + 750;
    public bool ShouldAutoSetWeaponState { get; set; } = true;
    public bool ShouldBustPlayer => !IsInVehicle && DistanceToPlayer > 0.1f && DistanceToPlayer <= 5f;
    public bool WasModSpawned { get; private set; }
    public float DistanceToInvestigationPosition(Vector3 Position)//whut lol
    {
        return Pedestrian.DistanceTo2D(Position);
    }
    public void IssueWeapons()
    {
        Sidearm = AssignedAgency.GetRandomWeapon(true);
        //Game.Console.Print($"Issued: {Sidearm.ModelName} Variation: {string.Join(",", Sidearm.Variation.Components.Select(x => x.Name))}");
        LongGun = AssignedAgency.GetRandomWeapon(false);
        //Game.Console.Print($"Issued: {LongGun.ModelName} Variation: {string.Join(",", LongGun.Variation.Components.Select(x => x.Name))}");
    }
    public void RadioIn(IPoliceRespondable currentPlayer)
    {
        if (CanRadioIn && currentPlayer.IsWanted)
        {
            string AnimationToPlay = "generic_radio_enter";
            //WeaponInformation CurrentGun = DataMart.Instance.Weapons.GetCurrentWeapon(Pedestrian);
            //if (CurrentGun != null && CurrentGun.IsOneHanded)
            //    AnimationToPlay = "radio_enter";

            Speak(currentPlayer);

            AnimationDictionary.RequestAnimationDictionay("random@arrests");
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Pedestrian, "random@arrests", AnimationToPlay, 2.0f, -2.0f, -1, 52, 0, false, false, false);
            GameTimeLastRadioed = Game.GameTime;
        }
    }
    public void Speak(IPoliceRespondable currentPlayer)
    {
        if (CanSpeak && currentPlayer.IsWanted)
        {
            if (!currentPlayer.IsBusted && DistanceToPlayer <= 20f)
            {
                Pedestrian.PlayAmbientSpeech("ARREST_PLAYER");
            }
            //else if (currentPlayer.RecentlyKilledCop)
            //{
            //    Pedestrian.PlayAmbientSpeech("OFFICER_DOWN");
            //}
            else if (currentPlayer.IsWanted && !currentPlayer.PoliceResponse.IsDeadlyChase)
            {
                Pedestrian.PlayAmbientSpeech(UnarmedChaseSpeech.PickRandom());
            }
            else if (currentPlayer.IsNotWanted && currentPlayer.IsBusted)
            {
                Pedestrian.PlayAmbientSpeech(SuspectBusted.PickRandom());
            }
            else if (currentPlayer.PoliceResponse.IsDeadlyChase)
            {
                Pedestrian.PlayAmbientSpeech(DeadlyChaseSpeech.PickRandom());
            }
            else //Normal State
            {
                if (DistanceToPlayer <= 4f)
                {
                    Pedestrian.PlayAmbientSpeech("CRIMINAL_WARNING");
                }
            }
            GameTimeLastSpoke = Game.GameTime;
        }
    }
    public void UpdateDrivingFlags() 
    {
        NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Pedestrian, 100f);
        NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Pedestrian, 8f);
    }
    public void UpdateLoadout(bool IsDeadlyChase, int WantedLevel)
    {
        if (ShouldAutoSetWeaponState)
        {
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
                    //this isnt normall here
                    SetLessLethal();

                    //temp off for testing!!!
                    // NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, true);//for idle,

                    ///temp off for testing!!!
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
            Pedestrian.Accuracy = 10;
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
            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Pedestrian, 100);//30
            if (LongGun != null)// && HasHeavyWeaponOnPerson)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, LongGun.GetHash(), true);
            }
            else
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, Sidearm.GetHash(), true);
            }
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
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
            Pedestrian.Accuracy = 30;
            if (Pedestrian.Inventory != null && !Pedestrian.Inventory.Weapons.Contains(WeaponHash.StunGun))
            {
                Pedestrian.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
            }
            else if (Pedestrian.Inventory != null && Pedestrian.Inventory.EquippedWeapon != WeaponHash.StunGun)
            {
                Pedestrian.Inventory.EquippedWeapon = WeaponHash.StunGun;
            }
            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Pedestrian, 100);
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
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
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, 2725352035, true); //Unequip weapon so you don't get shot
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
            }
            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Pedestrian, 0);
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 2, false);//cant do drivebys
            IsSetLessLethal = false;
            IsSetUnarmed = true;
            IsSetDeadly = false;
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
}