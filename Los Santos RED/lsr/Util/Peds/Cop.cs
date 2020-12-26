
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
    public Loadout Loadout { get; set; }
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
        if (DataMart.Instance.Settings.SettingsManager.Police.OverridePoliceAccuracy)
        {
            Pedestrian.Accuracy = DataMart.Instance.Settings.SettingsManager.Police.PoliceGeneralAccuracy;
        }

        Loadout = new Loadout(this);
    }
    public void CheckSpeech(IPlayer currentPlayer)
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
            WeaponInformation CurrentGun = DataMart.Instance.Weapons.GetCurrentWeapon(Pedestrian);
            if (CurrentGun != null && CurrentGun.IsOneHanded)
                AnimationToPlay = "radio_enter";

            Speak(currentPlayer);

            AnimationDictionary AnimDictionary = new AnimationDictionary("random@arrests");
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Pedestrian, "random@arrests", AnimationToPlay, 2.0f, -2.0f, -1, 52, 0, false, false, false);
            GameTimeLastRadioed = Game.GameTime;
        }

    }
}

