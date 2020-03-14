
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTACop : GTAPed
{
    //private PedTask.Task PreviousTaskAssigned { get; set; } = PedTask.Task.NoTask;
    //private PedTask.Task taskAssigned { get; set; } = PedTask.Task.NoTask;
    public GTACop(Ped _Pedestrian, bool _canSeePlayer, int _Health, Agency _Agency) : base(_Pedestrian, _canSeePlayer, _Health)
    {
        //Pedestrian = _Pedestrian;
        //canSeePlayer = _canSeePlayer;
        //Health = _Health;
        AssignedAgency = _Agency;
        SetAccuracyAndSightRange();
        if (_Pedestrian.Model.Name.ToLower() == "s_m_y_swat_01")
            IsSwat = true;
    }
    public GTACop(Ped _Pedestrian, bool _canSeePlayer, uint _gameTimeLastSeenPlayer, Vector3 _positionLastSeenPlayer, int _Health, Agency _Agency) : base(_Pedestrian, _canSeePlayer, _Health)
    {
        Pedestrian = _Pedestrian;
        CanSeePlayer = _canSeePlayer;
        GameTimeLastSeenPlayer = _gameTimeLastSeenPlayer;
        PositionLastSeenPlayer = _positionLastSeenPlayer;
        Health = _Health;
        AssignedAgency = _Agency;
        SetAccuracyAndSightRange();
        if (_Pedestrian.Model.Name.ToLower() == "s_m_y_swat_01")
            IsSwat = true;
    }

    //public bool isTasked { get; set; } = false;
    public bool WasRandomSpawn { get; set; } = false;
    public bool WasRandomSpawnDriver { get; set; } = false;
    public bool WasInvestigationSpawn { get; set; } = false;
    public bool IsBikeCop { get; set; } = false;
    public bool IsSwat { get; set; } = false;
    public bool isPursuitPrimary { get; set; } = false;
    //private PedTask.Task taskType = PedTask.Task.NoTask;
    //public PedTask.Task TaskType {//temp like this to check for task loops
    //    get { return taskType; }
    //    set
    //    {
    //        if(taskType != value)
    //        {
    //            if(PreviousTaskAssigned == value && taskType != PedTask.Task.Untask && taskType != PedTask.Task.NoTask)
    //            {
    //                Debugging.WriteToLog("GTACop", string.Format("Cop {0} Possile Task Loop: Previous: {1} Current: {2} New: {3}", Pedestrian.Handle, PreviousTaskAssigned, taskType, value));
    //            }
    //            PreviousTaskAssigned = taskType;
    //            taskType = value;
    //        }
    //    }
    //}
    //public GameFiber TaskFiber { get; set; }
    public bool SetTazer { get; set; } = false;
    public bool SetUnarmed { get; set; } = false;
    public bool SetDeadly { get; set; } = false;
    //public bool TaskIsQueued { get; set; } = false;
    public uint GameTimeLastWeaponCheck { get; set; }
    public uint GameTimeLastTask { get; set; }
    public uint GameTimeLastSpoke { get; set; }
    public GTAWeapon IssuedPistol { get; set; } = new GTAWeapon("weapon_pistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 453432689, true,true,false,true);
    public GTAWeapon IssuedHeavyWeapon { get; set; }
    public WeaponVariation PistolVariation { get; set; }
    public WeaponVariation HeavyVariation { get; set; }
    public Agency AssignedAgency { get; set; } = Agencies.LSPD;
    public bool AtWantedCenterDuringSearchMode { get; set; } = false;
    public void SetAccuracyAndSightRange()
    {
        Pedestrian.VisionRange = 55f;
        Pedestrian.HearingRange = 25;
        if(Settings.OverridePoliceAccuracy)
            Pedestrian.Accuracy = Settings.PoliceGeneralAccuracy;
    }
    public bool NeedsWeaponCheck
    {
        get
        {
            if (GameTimeLastWeaponCheck == 0)
                return true;
            else if (Game.GameTime > GameTimeLastWeaponCheck + 500)
                return true;
            else
                return false;
        }       
    }
    public bool CanSpeak
    {
        get
        {
            if (GameTimeLastSpoke == 0)
                return true;
            else if (Game.GameTime > GameTimeLastSpoke + 15000)
                return true;
            else
                return false;
        }
    }

    //public void SetTask(PedTask.Task MyTaskType)
    //{

    //    TaskType = MyTaskType;
    //}
}

