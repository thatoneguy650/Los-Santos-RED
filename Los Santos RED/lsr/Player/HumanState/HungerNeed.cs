using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HungerNeed : HumanNeed
{
    private IHumanStateable Player;
    private float MinChangeValue = -0.003f;
    private DateTime TimeLastUpdatedValue;
    private ITimeReportable Time;
    private float RealTimeScalar;
    private ISettingsProvideable Settings;
    public HungerNeed(string name, float minValue, float maxValue, IHumanStateable humanStateable, ITimeReportable time, ISettingsProvideable settings) : base(name, minValue, maxValue, humanStateable, time)
    {
        Player = humanStateable;
        Time = time;
        TimeLastUpdatedValue = Time.CurrentDateTime;
        Settings = settings;
    }

    public override void OnMaximum()
    {

    }

    public override void OnMinimum()
    {

    }

    public override void Update()
    {
        if (NeedsUpdate && Settings.SettingsManager.NeedsSettings.ApplyHunger)
        {
            if (Player.IsAlive)
            {
                RealTimeScalar = 1.0f;
                TimeSpan TimeDifference = Time.CurrentDateTime - TimeLastUpdatedValue;
                RealTimeScalar = (float)TimeDifference.TotalSeconds;
                Drain();
                TimeLastUpdatedValue = Time.CurrentDateTime;
            }
        }
    }
    private void Drain()
    {
        float RestScalar = 1.0f;
        if(Player.IsResting)
        {
            RestScalar = 0.25f;
        }
        if (Player.IsInVehicle)
        {
            Change(MinChangeValue * RealTimeScalar * RestScalar);
        }
        else
        {
            float Multiplier = 1.0f * RealTimeScalar;
            if (Player.FootSpeed >= 1.0f)
            {
                Multiplier = Player.FootSpeed / 5.0f;
            }
            if (Multiplier <= 1.0f)
            {
                Multiplier = 1.0f;
            }
            Change(MinChangeValue * Multiplier * RestScalar);
        }
    }
}

