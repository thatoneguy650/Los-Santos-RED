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
    private HumanNeed Hunger;
    private ThirstNeed Thirst;
    private HumanNeed Sleep;
    private HumanNeed Temperature;
    private HumanNeed Energy;
    private uint GameTimeLastChangedNeed;

    public bool RecentlyChangedNeed => GameTimeLastChangedNeed > 0 && Game.GameTime - GameTimeLastChangedNeed <= 5000;
    public HumanState(IHumanStateable player)
    {
        Player = player;
    }
    public void Setup()
    {
        Thirst = new ThirstNeed("Thirst", 0, 100f, Player);
        Hunger = new HungerNeed("Hunger", 0, 100f, Player);
        HumanNeeds = new List<HumanNeed>() { Thirst, Hunger };

    }
    public void Update()
    {
        foreach(HumanNeed humanNeed in HumanNeeds)
        {
            humanNeed.Update();
        }
    }
    public void Reset()
    {
        foreach (HumanNeed humanNeed in HumanNeeds)
        {
            humanNeed.Reset();
        }
    }
    public void Dispose()
    {
        Reset();
    }

    public void ChangeThirst(float v)
    {
        GameTimeLastChangedNeed = Game.GameTime;
        Thirst.Change(v);
    }
    public void ChangeHunger(float v)
    {
        GameTimeLastChangedNeed = Game.GameTime;
        Hunger.Change(v);
    }
    public string DisplayString()
    {
        return string.Join(" ", HumanNeeds.OrderBy(x=>x.Name).Select(x => $"{x.Name}: {Math.Round(x.CurrentValue, 4)}"));
    }
}

