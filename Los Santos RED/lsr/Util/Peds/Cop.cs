
using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Linq;


public class Cop : PedExt
{
    private readonly List<string> DeadlyChaseSpeech = new List<string> { "CHALLENGE_THREATEN", "COMBAT_TAUNT", "FIGHT", "GENERIC_INSULT", "GENERIC_WAR_CRY", "GET_HIM", "REQUEST_BACKUP", "REQUEST_NOOSE", "SHOOTOUT_OPEN_FIRE" };
    private readonly List<string> UnarmedChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "GET_HIM", "SUSPECT_SPOTTED" };
    private readonly List<string> CautiousChaseSpeech = new List<string> { "DRAW_GUN", "GET_HIM", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL" };
    private readonly List<string> ArrestedWaitSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "GET_HIM", "SUSPECT_SPOTTED", "DRAW_GUN", "GET_HIM", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL" };
    private readonly List<string> PlayerDeadSpeech = new List<string> { "CHAT_STATE", "CHAT_RESP" };
    private readonly List<string> SuspectBusted = new List<string> { "WON_DISPUTE" };
    private readonly List<string> SuspectDown = new List<string> { "SUSPECT_KILLED", "WON_DISPUTE", "SUSPECT_KILLED_REPORT" };
    private uint GameTimeLastSpoke;
    private uint GameTimeLastRadioed;
    private uint GameTimeSpawned;
    private bool IsSetLessLethal;
    private bool IsSetUnarmed;
    private bool IsSetDeadly;
    private uint GameTimeLastWeaponCheck;
    private WeaponInformation IssuedPistol;
    private WeaponInformation IssuedHeavy;
    private WeaponVariation IssuedPistolVariation;
    private WeaponVariation IssuedHeavyVariation;
    public bool NeedsWeaponCheck
    {
        get
        {
            if (GameTimeLastWeaponCheck == 0)
            {
                return true;
            }
            else if (Game.GameTime > GameTimeLastWeaponCheck + 750)//500
            {
                return true;
            }
            else
                return false;
        }
    }
    public bool HasPistol
    {
        get
        {
            if (IssuedPistol != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool HasHeavyWeapon
    {
        get
        {
            if (IssuedHeavy != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool IsSpeechTimedOut
    {
        get
        {
            if (GameTimeLastSpoke == 0)
                return false;
            else if (Game.GameTime - GameTimeLastSpoke >= 15000)
                return false;
            else
                return true;
        }
    }
    public bool CanSpeak
    {
        get
        {
            if (IsSpeechTimedOut)
                return false;
            else
                return true;
        }
    }
    public bool IsRadioTimedOut
    {
        get
        {
            if (GameTimeLastRadioed == 0)
                return false;
            else if (Game.GameTime - GameTimeLastRadioed >= 45000)
                return false;
            else
                return true;
        }
    }
    public bool CanRadioIn
    {
        get
        {
            if (IsRadioTimedOut)
            {
                return false;
            }
            else if (!IsInVehicle && !Pedestrian.IsSwimming && !Pedestrian.IsInCover && !Pedestrian.IsGoingIntoCover && !Pedestrian.IsShooting && !Pedestrian.IsInWrithe && !Pedestrian.IsGettingIntoVehicle && !Pedestrian.IsInAnyVehicle(true) && !Pedestrian.IsInAnyVehicle(false))
            {//simplify this, testing seems to make them be deleted?
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool WasModSpawned { get; private set; }
    public bool WasSpawnedAsDriver { get; set; }
    public bool ShouldAutoSetWeaponState { get; set; } = true;
    public Agency AssignedAgency { get; set; } = new Agency();
    public float DistanceToInvestigationPosition(Vector3 Position)
    {
        return Pedestrian.DistanceTo2D(Position);
    }
    public uint HasBeenSpawnedFor
    {
        get
        {
            return Game.GameTime - GameTimeSpawned;
        }
    }
    public bool ShouldBustPlayer
    {
        get
        {
            if (DistanceToPlayer < 0.1f) //weird cases where they are my same position
            {
                return false;
            }
            else if (DistanceToPlayer <= 5f)
            {
                return true;
            }
            return false;
        }
    }
    public Cop(Ped pedestrian, int health, Agency agency, bool wasModSpawned, WeaponInformation pistolToIssue, WeaponVariation pistolVariation, WeaponInformation heavyToIssue, WeaponVariation heavyVaritaion) : base(pedestrian)
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
        IssuedPistol = pistolToIssue;
        IssuedPistolVariation = pistolVariation;
        IssuedHeavy = heavyToIssue;
        IssuedHeavyVariation = heavyVaritaion;
    }
    public void UpdateSpeech(IPlayer currentPlayer)
    {
        Speak(currentPlayer);
        RadioIn(currentPlayer);
    }
    public void Speak(IPlayer currentPlayer)
    {
        if (CanSpeak)
        {
            if (!currentPlayer.IsBusted && DistanceToPlayer <= 20f)
            {
                Pedestrian.PlayAmbientSpeech("ARREST_PLAYER");
            }
            else if (currentPlayer.RecentlyKilledCop)
            {
                Pedestrian.PlayAmbientSpeech("OFFICER_DOWN");
            }
            else if (currentPlayer.IsWanted && !currentPlayer.CurrentPoliceResponse.IsDeadlyChase)
            {
                Pedestrian.PlayAmbientSpeech(UnarmedChaseSpeech.PickRandom());
            }
            else if (currentPlayer.IsNotWanted && currentPlayer.IsBusted)
            {
                Pedestrian.PlayAmbientSpeech(SuspectBusted.PickRandom());
            }
            else if (currentPlayer.CurrentPoliceResponse.IsDeadlyChase)
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
    public void RadioIn(IPlayer currentPlayer)
    {
        if (CanRadioIn)
        {
            string AnimationToPlay = "generic_radio_enter";
            //WeaponInformation CurrentGun = DataMart.Instance.Weapons.GetCurrentWeapon(Pedestrian);
            //if (CurrentGun != null && CurrentGun.IsOneHanded)
            //    AnimationToPlay = "radio_enter";

            Speak(currentPlayer);

            AnimationDictionary AnimDictionary = new AnimationDictionary("random@arrests");
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Pedestrian, "random@arrests", AnimationToPlay, 2.0f, -2.0f, -1, 52, 0, false, false, false);
            GameTimeLastRadioed = Game.GameTime;
        }

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
                if (WantedLevel > 0)
                {
                    SetLessLethal();
                }
                else if (IsInVehicle)
                {
                    SetUnarmed();
                }
                else if (WantedLevel == 0)
                {
                    NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, true);//for idle, 
                }
                else
                {
                    SetUnarmed();
                }
            }
        }
    }
    private void SetUnarmed()
    {
        if (Pedestrian.Exists() && Pedestrian.IsAlive && (!IsSetUnarmed || NeedsWeaponCheck))
        {
            if (Pedestrian.Inventory.EquippedWeapon != null)
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
    private void SetDeadly()
    {
        if (Pedestrian.Exists() && Pedestrian.IsAlive && (!IsSetDeadly || NeedsWeaponCheck))
        {
            Pedestrian.Accuracy = 10;
            if (!Pedestrian.Inventory.Weapons.Contains(IssuedPistol.ModelName))
            {
                Pedestrian.Inventory.GiveNewWeapon(IssuedPistol.ModelName, -1, true);
                IssuedPistol.ApplyWeaponVariation(Pedestrian, (uint)IssuedPistol.Hash, IssuedPistolVariation);
            }
            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Pedestrian, 50);//30
            if (IssuedHeavy != null)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, IssuedHeavy.Hash, true);
            }
            else
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, IssuedPistol.Hash, true);
            }
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
            if (!Pedestrian.Inventory.Weapons.Contains(WeaponHash.StunGun))
            {
                Pedestrian.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
            }
            else if (Pedestrian.Inventory.EquippedWeapon != WeaponHash.StunGun)
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

}

