using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedAlerts
{

    private GunshotAlert GunshotAlert;
    private UnconsciousBodyAlert UnconsciousBodyAlert;
    private DeadBodyAlert DeadBodyAlert;
    private HelpCryAlert HelpCryAlert;

    private PedExt PedExt;
    private ISettingsProvideable Settings;
    private List<PedAlert> PedAlertList = new List<PedAlert>();
    public bool IsAlerted => CurrentPrirorityAlert != null && CurrentPrirorityAlert.IsActive;
    public PedAlert CurrentPrirorityAlert { get; private set; }
    public Vector3 AlertedPoint => IsAlerted ? CurrentPrirorityAlert.Position : Vector3.Zero;
    public PedExt AlertedPed => CurrentPrirorityAlert.AlertTarget;
    public bool HasCrimeToReport => UnconsciousBodyAlert.HasSeenUnconsciousPed;



    public PedAlerts(PedExt pedExt, ISettingsProvideable settings)
    {
        PedExt = pedExt;
        Settings = settings;
        GunshotAlert = new GunshotAlert(PedExt, Settings);
        UnconsciousBodyAlert = new UnconsciousBodyAlert(PedExt, Settings); 
        DeadBodyAlert = new DeadBodyAlert(PedExt, Settings);
        HelpCryAlert = new HelpCryAlert(PedExt, Settings);
        PedAlertList.Add(GunshotAlert);
        PedAlertList.Add(UnconsciousBodyAlert);
        PedAlertList.Add(DeadBodyAlert);
        PedAlertList.Add(HelpCryAlert);
    }
    public void Update(IPoliceRespondable policeRespondable, IEntityProvideable world)
    {
        foreach (PedAlert pedAlert in PedAlertList)
        {
            pedAlert.Update(policeRespondable, world);
        }
        CurrentPrirorityAlert = PedAlertList.Where(x => x.IsActive).OrderBy(x=> x.Priority).FirstOrDefault();
    }
    public void OnReportedCrime(IPoliceRespondable policeRespondable)
    {
        foreach (PedAlert pedAlert in PedAlertList)
        {
            pedAlert.OnReportedCrime(policeRespondable);
        }
    }
}




//using ExtensionsMethods;
//using LosSantosRED.lsr.Helper;
//using LosSantosRED.lsr.Interface;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class PedAlerts
//{

//    private GunshotAlert GunshotAlert;


//    private PedExt PedExt;
//    private ISettingsProvideable Settings;
//    private bool IsHelpCryAlerted => GameTimeLastHeardHelpCry > 0 && Game.GameTime - GameTimeLastHeardHelpCry <= Settings.SettingsManager.WorldSettings.HelpCryAlertTimeout;
//    private bool IsBodyAlerted => BodiesSeen.Any() && Game.GameTime - GameTimeLastSeenDeadBody <= Settings.SettingsManager.WorldSettings.DeadBodyAlertTimeout;
//    private bool IsGunshotAlerted => GameTimeLastHeardGunshots> 0 && Game.GameTime - GameTimeLastHeardGunshots <= Settings.SettingsManager.WorldSettings.GunshotAlertTimeout;
//    public List<PedExt> BodiesSeen { get; private set; } = new List<PedExt>();
//    public bool IsAlerted => IsBodyAlerted || IsGunshotAlerted || IsHelpCryAlerted;
//    public Vector3 PositionLastSeenUnconsciousPed { get; set; }
//    public uint GameTimeLastSeenDeadBody { get; private set; }
//    public uint GameTimeLastHeardGunshots { get; private set; }
//    public bool HasSeenUnconsciousPed { get; set; } = false;

//    public uint GameTimeLastHeardHelpCry { get; private set; }






//    public List<PedAlert> PedAlertList { get; private set; } = new List<PedAlert>();







//    public Vector3 AlertedPoint { get; set; }
//    public PedAlerts(PedExt pedExt, ISettingsProvideable settings)
//    {
//        PedExt = pedExt;
//        Settings = settings;
//        GunshotAlert = new GunshotAlert(Settings);
//        PedAlertList.Add(GunshotAlert);
//    }
//    public void LookForUnconsciousPeds(IEntityProvideable world)
//    {
//        if (!PedExt.Pedestrian.Exists() || PedExt.IsUnconscious || HasSeenUnconsciousPed || PedExt.PlayerPerception.DistanceToTarget > 150f)//only care in a bubble around the player, nothing to do with the player tho
//        {
//            return;
//        }
//        foreach (PedExt distressedPed in world.Pedestrians.PedExts.Where(x => (x.IsUnconscious || x.IsInWrithe) && !x.HasStartedEMTTreatment && !x.HasBeenTreatedByEMTs && NativeHelper.IsNearby(PedExt.CellX, PedExt.CellY, x.CellX, x.CellY, 4) && x.Pedestrian.Exists()))
//        {
//            float distanceToBody = PedExt.Pedestrian.DistanceTo2D(distressedPed.Pedestrian);
//            if (distanceToBody <= 15f || (distanceToBody <= 45f && distressedPed.Pedestrian.IsThisPedInFrontOf(PedExt.Pedestrian) && (distressedPed.HasBeenSeenUnconscious || NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", PedExt.Pedestrian, distressedPed.Pedestrian))))//if someone saw it assume ANYONE close saw it, only performance reason
//            {
//                AddSeenUnconsciousPed(distressedPed);
//                break;
//            }
//        }
//    }
//    public void LookForBodiesAlert(IEntityProvideable world)
//    {
//        if (!PedExt.Pedestrian.Exists() || PedExt.IsUnconscious || PedExt.PlayerPerception.DistanceToTarget > 150f || world.TotalWantedLevel >= 3)
//        {
//            return;
//        }
//        foreach (PedExt deadBody in world.Pedestrians.DeadPeds.Where(x => !BodiesSeen.Any(y => y.Handle == x.Handle) && x.Pedestrian.Exists() && PedExt.Pedestrian.Exists()))
//        {
//            float distanceToBody = PedExt.Pedestrian.DistanceTo2D(deadBody.Pedestrian);
//            bool CanSeeBody = false;
//            if (!deadBody.WasKilledByPlayer && distanceToBody <= 15f)
//            {
//                CanSeeBody = true;
//            }
//            else if (distanceToBody <= 45f && deadBody.Pedestrian.IsThisPedInFrontOf(PedExt.Pedestrian) && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", PedExt.Pedestrian, deadBody.Pedestrian))
//            {
//                CanSeeBody = true;
//                GameFiber.Yield();
//            }
//            if (CanSeeBody)
//            {
//                AddSeenBody(deadBody);
//            }
//        }
//        BodiesSeen.RemoveAll(x => !x.Pedestrian.Exists());
//    }
//    public void AddSeenUnconsciousPed(PedExt unconsciousPed)
//    {
//        if (unconsciousPed == null || !unconsciousPed.Pedestrian.Exists())
//        {
//            return;
//        }
//        PositionLastSeenUnconsciousPed = unconsciousPed.Position;
//        HasSeenUnconsciousPed = true;
//        unconsciousPed.HasBeenSeenUnconscious = true;
//        EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I JUST SAW AN Unconscious BODY {unconsciousPed.Handle}");
//    }
//    public void AddSeenBody(PedExt deadBody)
//    {
//        if (deadBody == null || !deadBody.Pedestrian.Exists())
//        {
//            return;
//        }
//        BodiesSeen.Add(deadBody);
//        GameTimeLastSeenDeadBody = Game.GameTime;
//        AlertedPoint = deadBody.Pedestrian.Position;
//        EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I JUST SAW A DEAD BODY {deadBody.Handle}");
//    }
//    public void AddHeardGunfire(Vector3 locationHeard)
//    {
//        if (locationHeard == Vector3.Zero)
//        {
//            return;
//        }
//        GameTimeLastHeardGunshots = Game.GameTime;
//        AlertedPoint = locationHeard;
//        EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I JUST HEARD GUNFIRE");
//    }
//    public void AddHeardHelpCry(Vector3 locationHeard)
//    {
//        if (locationHeard == Vector3.Zero)
//        {
//            return;
//        }
//        GameTimeLastHeardHelpCry = Game.GameTime;
//        if (!IsBodyAlerted && !IsGunshotAlerted)
//        {
//            AlertedPoint = locationHeard;
//        }
//        EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I JUST HEARD A HELP CRY");
//    }
//}

