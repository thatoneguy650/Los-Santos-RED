using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HumanState
{
    public IHumanStateable Player;
    private List<HumanNeed> HumanNeeds = new List<HumanNeed>();
    private ITimeReportable Time;
    private ISettingsProvideable Settings;
    private bool isApplyingNeeds = false;
    public bool RecentlyChangedNeed => HumanNeeds != null && HumanNeeds.Any(x=> x.RecentlyChanged);
    public HungerNeed Hunger { get; private set; }
    public ThirstNeed Thirst { get; private set; }
    public SleepNeed Sleep { get; private set; }
    public bool HasPressingNeeds { get; private set; }
    public bool HasNeedsManaged { get; private set; }
    public float MinimumNeedPercent { get; private set; }
    public HumanState(IHumanStateable player, ITimeReportable time, ISettingsProvideable settings)
    {
        Player = player;
        Time = time;
        Settings = settings;
    }
    public void Setup()
    {
        isApplyingNeeds = Settings.SettingsManager.NeedsSettings.ApplyNeeds;
        Thirst = new ThirstNeed("Thirst", 0, 100f, Player, Time, Settings);
        Hunger = new HungerNeed("Hunger", 0, 100f, Player, Time, Settings);
        Sleep = new SleepNeed("Sleep", 0, 100f, Player, Time, Settings);
        HumanNeeds = new List<HumanNeed>() { Thirst, Hunger, Sleep };

    }
    public void Update()
    {
        if(Settings.SettingsManager.NeedsSettings.ApplyNeeds != isApplyingNeeds)
        {
            //EntryPoint.WriteToConsoleTestLong("Changed Apply Needs Settings, Resetting Values");
            Reset();
            isApplyingNeeds = Settings.SettingsManager.NeedsSettings.ApplyNeeds;
        }
        if (Settings.SettingsManager.NeedsSettings.ApplyNeeds)
        {
            float MinNeed = 100f;
            bool IsBelowQuarter = false;
            bool IsBelowThreeQuarters = false;
            foreach (HumanNeed humanNeed in HumanNeeds)
            {
                humanNeed.Update();
                if(humanNeed.IsBelowQuarter)
                {
                    IsBelowQuarter = true;
                }
                if(humanNeed.IsBelowThreeQuarters)
                {
                    IsBelowThreeQuarters = true;
                }
                if(humanNeed.CurrentValue < MinNeed)
                {
                    MinNeed = humanNeed.CurrentValue;
                }
            }
            MinimumNeedPercent = MinNeed;
            HasNeedsManaged = !IsBelowThreeQuarters;
            HasPressingNeeds = IsBelowQuarter;
        }
        else
        {
            HasNeedsManaged = true;
            HasPressingNeeds = false;
        }
    }
    public void Reset()
    {
        foreach (HumanNeed humanNeed in HumanNeeds)
        {
            humanNeed.Reset();
        }
    }
    public void SetRandom()
    {
        foreach (HumanNeed humanNeed in HumanNeeds)
        {
            humanNeed.SetRandom(false);
        }
    }
    public void Dispose()
    {
        Reset();
    }
    public string DisplayString()
    {
        return string.Join(" ", HumanNeeds.OrderBy(x=>x.Name).Select(x => x.Display));
    }
    public string RecentlyChangedString()
    {
        return string.Join(" ", HumanNeeds.Where(x=> x.RecentlyChanged).OrderBy(x => x.Name).Select(x => x.Display));
    }
}

