using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class PedAlert
{
    protected bool IsPositionAlert = false;
    protected bool IsPedAlert = false;
    protected uint TimeOutTime = 20000;
    protected uint PostPositionTimeout = 10000;
    protected ISettingsProvideable Settings;
    protected PedExt PedExt;
    protected ePedAlertType PedAlertType;
    protected uint GameTimeReachedPosition;
    protected bool HasReachedPosition;
   

    protected PedAlert(PedExt pedExt,ISettingsProvideable settings, ePedAlertType pedAlertType)
    {
        PedExt = pedExt;
        Settings = settings;
        PedAlertType = pedAlertType;
    }
    public int Priority { get; set; }
    public bool IsActive => GameTimeLastAlerted > 0 && Game.GameTime - GameTimeLastAlerted <= TimeOutTime;
    public Vector3 Position { get; set; }
    public PedExt AlertTarget { get; set; }
    public uint GameTimeLastAlerted { get; set; }
    public bool CanPositionAlert => Position != Vector3.Zero;
    public bool CanPedAlert => AlertTarget != null && AlertTarget.Pedestrian.Exists();

    public virtual void Update()
    {

    }
    public virtual void AddAlert(Vector3 positon)
    {

        if (positon == Vector3.Zero)
        {
            return;
        }
        HasReachedPosition = false;
        GameTimeReachedPosition = 0;
        Position = positon;
        AlertTarget = null;
        GameTimeLastAlerted = Game.GameTime;
    }
    public virtual void AddAlert(PedExt pedExt)
    {
        if (pedExt == null || !pedExt.Pedestrian.Exists())
        {
            return;
        }
        HasReachedPosition = false;
        GameTimeReachedPosition = 0;
        Position = pedExt.Pedestrian.Position;
        AlertTarget = pedExt;
        GameTimeLastAlerted = Game.GameTime;
    }
    public virtual void Update(IPoliceRespondable policeRespondable, IEntityProvideable world)
    {
        if(IsActive && PedExt.Pedestrian.Exists())
        {
            if(!HasReachedPosition)
            {
                float distance = PedExt.Pedestrian.DistanceTo2D(Position);
                if (distance <= 5f)
                {
                    HasReachedPosition = true;
                    GameTimeReachedPosition = Game.GameTime;
                    //EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I REACHED MY ALERT POSITION");
                }
            }
            else
            {
                if(Game.GameTime - GameTimeReachedPosition >= PostPositionTimeout)
                {
                    HasReachedPosition = false;
                    GameTimeReachedPosition = 0;
                    GameTimeLastAlerted = 0;
                    AlertTarget = null;
                    Position = Vector3.Zero;



                    //EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I HAVE POSITION TIMEOUT MY ALERT");
                }
            }
        }
        else
        {
            HasReachedPosition = false;
            GameTimeReachedPosition = 0;
        }
    }

    public virtual void OnReportedCrime(IPoliceRespondable policeRespondable)
    {

    }
}

