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
    private ITimeReportable Time;
    private float RealTimeScalar;
    private ISettingsProvideable Settings;

    private bool ShouldSlowDrain => Player.IsResting || Player.IsSleeping || Player.ActivityManager.IsSitting || Player.ActivityManager.IsLayingDown;
    private bool ShouldChange => Player.IsAlive && !RecentlyChanged;
    public HungerNeed(string name, float minValue, float maxValue, IHumanStateable humanStateable, ITimeReportable time, ISettingsProvideable settings) : base(name, minValue, maxValue, humanStateable, time)
    {
        Player = humanStateable;
        Time = time;
        TimeLastUpdatedValue = Time.CurrentDateTime;
        Settings = settings;
    }
    public override int Digits => Settings.SettingsManager.NeedsSettings.HungerDisplayDigits;
    public override void OnMaximum()
    {

    }
    public override void OnMinimum()
    {

    }
    public override void Update()
    {
        if (NeedsValueUpdate && Settings.SettingsManager.NeedsSettings.ApplyHunger)
        {
            UpdateRealTimeScalar();
            if (ShouldChange)
            {
                Drain();
            }
        }
    }
    private void Drain()
    {
        float ChangeAmount = MinChangeValue * RealTimeScalar;
        if (ShouldSlowDrain)
        {
            ChangeAmount *= 0.25f;
        }
        if (!Player.IsInVehicle)
        {         
            ChangeAmount *= FootSpeedMultiplier();
        }
        ChangeAmount *= 0.5f * Settings.SettingsManager.NeedsSettings.HungerChangeScalar;
        Change(ChangeAmount, false);
    }
    private float FootSpeedMultiplier()
    {
        float Multiplier = 1.0f;
        if (Player.FootSpeed >= 1.0f)
        {
            Multiplier = Player.FootSpeed / 5.0f;
        }
        if (Multiplier <= 1.0f)
        {
            Multiplier = 1.0f;
        }
        return Multiplier;
    }
    private void UpdateRealTimeScalar()
    {
        RealTimeScalar = 1.0f;
        TimeSpan TimeDifference = Time.CurrentDateTime - TimeLastUpdatedValue;
        RealTimeScalar = (float)TimeDifference.TotalSeconds;
        TimeLastUpdatedValue = Time.CurrentDateTime;
    }
}

