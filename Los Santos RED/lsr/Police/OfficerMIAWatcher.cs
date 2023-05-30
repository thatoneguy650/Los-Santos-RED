using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


public class OfficerMIAWatcher
{
    private List<OfficerMIA> OfficerMIAs = new List<OfficerMIA>();
    private IPoliceRespondable Player;
    private IPerceptable Perceptable;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    public OfficerMIAWatcher(IEntityProvideable world, IPoliceRespondable currentPlayer, IPerceptable perceptable, ISettingsProvideable settings, ITimeReportable time)
    {
        World = world;
        Player = currentPlayer;
        Settings = settings;
        Perceptable = perceptable;
        Time = time;
    }
    public void Setup()
    {
        OfficerMIAs.Clear();
    }
    public void Update()
    {
        if(!Settings.SettingsManager.WorldSettings.AllowOfficerMIACallIn)
        {
            return;
        }
        foreach(OfficerMIA officerMIA in OfficerMIAs) 
        {
            officerMIA.Update(Player);
        }
        OfficerMIAs.RemoveAll(x => x.HasBeenReportedIn || x.IsExpired);
    }
    public void Dispose()
    {
        OfficerMIAs.Clear();
    }
    public void AddMIA(Cop cop, Vector3 position)
    {
        if(cop == null || position == Vector3.Zero || !Settings.SettingsManager.WorldSettings.AllowOfficerMIACallIn)
        {
            return;
        }
        OfficerMIA existingMIA = OfficerMIAs.FirstOrDefault(x => x.Cop.Handle == cop.Handle);
        if (existingMIA == null)
        {
            OfficerMIAs.Add(new OfficerMIA(cop, position, Settings));
            EntryPoint.WriteToConsole($"Adding New OfficerMIA for {cop.Handle} AT {position}");
        }
        else
        {
            existingMIA.GameTimeLastReported = Game.GameTime;
            existingMIA.PositionLastReported = position;
            EntryPoint.WriteToConsole($"UPDATING EXISTING OfficerMIA for {cop.Handle} AT {position}");
        }
    }
}

