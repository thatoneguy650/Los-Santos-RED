
using ExtensionsMethods;
using LosSantosRED.lsr;
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
    private readonly List<string> AmbientSpeech = new List<string> { "WON_DISPUTE" };
    private readonly List<string> RegularChaseSpeech = new List<string> { "SUSPECT_KILLED", "WON_DISPUTE", "SUSPECT_KILLED_REPORT" };
    private uint GameTimeLastSpoke;
    private uint GameTimeLastRadioed;

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
                return false;
            else if (!IsInVehicle && !Pedestrian.IsSwimming && !Pedestrian.IsInCover && !Pedestrian.IsGoingIntoCover && !Pedestrian.IsShooting && !Pedestrian.IsInWrithe)
                return true;
            else
                return false;
        }
    }

    public bool WasModSpawned { get; set; }
    public bool WasSpawnedAsDriver { get; set; }
    public bool ShouldAutoSetWeaponState { get; set; } = true;
    public Agency AssignedAgency { get; set; } = new Agency();
    public float DistanceToInvestigationPosition
    {
        get
        {
            return Pedestrian.DistanceTo2D(Mod.Player.Investigations.InvestigationPosition);
        }
    }
    public uint HasBeenSpawnedFor
    {
        get
        {
            return Game.GameTime - GameTimeSpawned;
        }
    }
    public int CountNearbyCops
    {
        get
        {
            return Mod.World.Pedestrians.Cops.Count(x => Pedestrian.Exists() && x.Pedestrian.Exists() && Pedestrian.Handle != x.Pedestrian.Handle && x.Pedestrian.DistanceTo2D(Pedestrian) >= 3f && x.Pedestrian.DistanceTo2D(Pedestrian) <= 50f);
        }
    }
    public bool ShouldBustPlayer
    {
        get
        {
            if (Mod.Player.IsBusted)
            {
                return false;
            }
            else if (!Mod.Player.IsBustable)
            {
                return false;
            }
            else if (IsInVehicle)
            {
                return false;
            }
            else if (DistanceToPlayer < 0.1f) //weird cases where they are my same position
            {
                return false;
            }
            else if (Mod.Player.HandsAreUp && DistanceToPlayer <= 5f)
            {
                return true;
            }
            if (Mod.Player.IsInVehicle)
            {
                if (Mod.Player.IsStationary && DistanceToPlayer <= 1f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll) && DistanceToPlayer <= 3f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    public Loadout Loadout { get; set; }
    public Cop(Ped pedestrian, int health, Agency agency) : base(pedestrian)
    {
        IsCop = true;
        Health = health;
        AssignedAgency = agency;

        Pedestrian.VisionRange = 90f;//55F
        Pedestrian.HearingRange = 55;//25
        if (Mod.DataMart.Settings.SettingsManager.Police.OverridePoliceAccuracy)
            Pedestrian.Accuracy = Mod.DataMart.Settings.SettingsManager.Police.PoliceGeneralAccuracy;

        Loadout = new Loadout(this);
    }
    public void Speak()
    {
        if (CanSpeak)
        {
            if (Mod.Player.IsBusted && DistanceToPlayer <= 20f)
            {
                Pedestrian.PlayAmbientSpeech("ARREST_PLAYER");
            }
            else if (Mod.World.PedDamage.RecentlyKilledCop)
            {
                Pedestrian.PlayAmbientSpeech("OFFICER_DOWN");
            }
            else if (Mod.Player.IsWanted && !Mod.Player.CurrentPoliceResponse.IsDeadlyChase)
            {
                Pedestrian.PlayAmbientSpeech(RegularChaseSpeech.PickRandom());
            }
            else if (Mod.Player.IsNotWanted && Mod.Player.Respawning.RecentlyBribedPolice)
            {
                Pedestrian.PlayAmbientSpeech(AmbientSpeech.PickRandom());
            }
            else if (Mod.Player.CurrentPoliceResponse.IsDeadlyChase)
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
    public void RadioIn()
    {
        if (CanRadioIn)
        {
            string AnimationToPlay = "generic_radio_enter";
            WeaponInformation CurrentGun = Mod.DataMart.Weapons.GetCurrentWeapon(Pedestrian);
            if (CurrentGun != null && CurrentGun.IsOneHanded)
                AnimationToPlay = "radio_enter";

            Speak();

            AnimationDictionary AnimDictionary = new AnimationDictionary("random@arrests");
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Pedestrian, "random@arrests", AnimationToPlay, 2.0f, -2.0f, -1, 52, 0, false, false, false);
            GameTimeLastRadioed = Game.GameTime;
        }

    }
}

