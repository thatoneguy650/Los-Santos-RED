using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class OfficerMIA
{
    private uint TimeToCallIn;
    private ISettingsProvideable Settings;
    public OfficerMIA(Cop cop, Vector3 positionLastReported, ISettingsProvideable settings)
    {
        Cop = cop;
        PositionLastReported = positionLastReported;
        GameTimeLastReported = Game.GameTime;
        Settings = settings;
        TimeToCallIn = RandomItems.GetRandomNumber(Settings.SettingsManager.WorldSettings.OfficerMIACallInTimeMin, Settings.SettingsManager.WorldSettings.OfficerMIACallInTimeMax);
    }

    public Cop Cop { get; set; }
    public Vector3 PositionLastReported { get; set; }
    public uint GameTimeLastReported { get; set; }
    public bool HasBeenReportedIn { get; set; }
    public bool IsExpired { get; set; }
    public void Update(IPoliceRespondable player)
    {
        if(Cop == null || !Cop.Pedestrian.Exists() || HasBeenReportedIn || IsExpired)
        {
            IsExpired = true;
            EntryPoint.WriteToConsole($"OfficerMIA EXPIRED 1");
            return;
        }
        float distanceToPlayer = player.Position.DistanceTo2D(PositionLastReported);
        if(distanceToPlayer >= Settings.SettingsManager.WorldSettings.OfficerMIACallInExpireDistance)
        {
            IsExpired = true;
            EntryPoint.WriteToConsole($"OfficerMIA EXPIRED 2");
            return;
        }
        if (Game.GameTime - GameTimeLastReported >= TimeToCallIn && distanceToPlayer <= Settings.SettingsManager.WorldSettings.OfficerMIACallInDistance)
        {
            player.AddOfficerMIACall(PositionLastReported);
            HasBeenReportedIn = true;
            EntryPoint.WriteToConsole($"OfficerMIA REPORTED IN 1");
        }
    }
}

