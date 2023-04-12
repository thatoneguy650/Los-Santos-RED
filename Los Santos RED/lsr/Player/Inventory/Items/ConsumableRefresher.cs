using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ConsumableRefresher
{
    private IActionable Player;
    private ConsumableItem ConsumableItem;
    private ISettingsProvideable Settings;
    private int HealthGiven;
    private bool GivenFullHealth;
    private uint GameTimeLastGivenHealth;
    private uint GameTimeLastGivenNeeds;
    private float HungerGiven;
    private bool GivenFullHunger;
    private float ThirstGiven;
    private bool GivenFullThirst;
    private float SleepGiven;
    private bool GivenFullSleep;

    public ConsumableRefresher(IActionable player, ConsumableItem foodItem, ISettingsProvideable settings)
    {
        Player = player;
        ConsumableItem = foodItem;
        Settings = settings;
    }

    public bool IsFinished { get; internal set; }

    public void Update()
    {
        if(ConsumableItem == null)
        {
            IsFinished = true;
            return;
        }
        if (!IsFinished)
        {
            UpdateHealthGain();
            UpdateNeeds();
            CheckStatus();
        }
    }
    public void FullyConsume()
    {
        if (ConsumableItem.ChangesHealth && (!Settings.SettingsManager.NeedsSettings.ApplyNeeds || ConsumableItem.AlwaysChangesHealth))
        {
            Player.HealthManager.ChangeHealth(ConsumableItem.HealthChangeAmount);
        }
        if(Settings.SettingsManager.NeedsSettings.ApplyNeeds)
        {
            if (ConsumableItem.ChangesHunger)
            {
                Player.HumanState.Hunger.Change(ConsumableItem.HungerChangeAmount, true);
            }
            if (ConsumableItem.ChangesSleep)
            {
                Player.HumanState.Sleep.Change(ConsumableItem.SleepChangeAmount, true);
            }
            if (ConsumableItem.ChangesThirst)
            {
                Player.HumanState.Thirst.Change(ConsumableItem.ThirstChangeAmount, true);
            }
        }        
    }
    private void CheckStatus()
    {
        if (Settings.SettingsManager.NeedsSettings.ApplyNeeds)
        {
            if (GivenFullHunger && GivenFullSleep && GivenFullThirst && (GivenFullHealth || !ConsumableItem.AlwaysChangesHealth))
            {
                //EntryPoint.WriteToConsoleTestLong($"Finished CIG1 {ConsumableItem.Name}");
                IsFinished = true;
            }
        }
        else
        {
            if (GivenFullHealth)
            {
                //EntryPoint.WriteToConsoleTestLong($"Finished CIG2 {ConsumableItem.Name}");
                IsFinished = true;
            }
        }
    }
    private void UpdateHealthGain()
    {
        if (Game.GameTime - GameTimeLastGivenHealth >= Settings.SettingsManager.NeedsSettings.TimeBetweenGain)
        {
            if (ConsumableItem.ChangesHealth && (!Settings.SettingsManager.NeedsSettings.ApplyNeeds || ConsumableItem.AlwaysChangesHealth))
            {
                if (ConsumableItem.HealthChangeAmount > 0 && HealthGiven < ConsumableItem.HealthChangeAmount)
                {
                    HealthGiven++;
                    Player.HealthManager.ChangeHealth(1);
                }
                else if (ConsumableItem.HealthChangeAmount < 0 && HealthGiven > ConsumableItem.HealthChangeAmount)
                {
                    HealthGiven--;
                    Player.HealthManager.ChangeHealth(-1);
                }
            }
            if (HealthGiven == ConsumableItem.HealthChangeAmount)
            {
                //EntryPoint.WriteToConsoleTestLong("GIVEN FULL HEALTH");
                GivenFullHealth = true;
            }
            GameTimeLastGivenHealth = Game.GameTime;
        }
    }
    private void UpdateNeeds()
    {
        if (Game.GameTime - GameTimeLastGivenNeeds >= Settings.SettingsManager.NeedsSettings.TimeBetweenGain)
        {
            if (ConsumableItem.ChangesNeeds)
            {
                if (ConsumableItem.ChangesHunger)
                {
                    if (ConsumableItem.HungerChangeAmount < 0.0f)
                    {
                        if (HungerGiven > ConsumableItem.HungerChangeAmount)
                        {
                            Player.HumanState.Hunger.Change(-1.0f, true);
                            HungerGiven--;
                        }
                        else
                        {
                            GivenFullHunger = true;
                        }
                    }
                    else
                    {
                        if (HungerGiven < ConsumableItem.HungerChangeAmount)
                        {
                            Player.HumanState.Hunger.Change(1.0f, true);
                            HungerGiven++;
                        }
                        else
                        {
                            GivenFullHunger = true;
                        }
                    }
                }
                else
                {
                    GivenFullHunger = true;
                }
                if (ConsumableItem.ChangesThirst)
                {
                    if (ConsumableItem.ThirstChangeAmount < 0.0f)
                    {
                        if (ThirstGiven > ConsumableItem.ThirstChangeAmount)
                        {
                            Player.HumanState.Thirst.Change(-1.0f, true);
                            ThirstGiven--;
                        }
                        else
                        {
                            GivenFullThirst = true;
                        }
                    }
                    else
                    {
                        if (ThirstGiven < ConsumableItem.ThirstChangeAmount)
                        {
                            Player.HumanState.Thirst.Change(1.0f, true);
                            ThirstGiven++;
                        }
                        else
                        {
                            GivenFullThirst = true;
                        }
                    }
                }
                else
                {
                    GivenFullThirst = true;
                }
                if (ConsumableItem.ChangesSleep)
                {
                    if (ConsumableItem.SleepChangeAmount < 0.0f)
                    {
                        if (SleepGiven > ConsumableItem.SleepChangeAmount)
                        {
                            Player.HumanState.Sleep.Change(-1.0f, true);
                            SleepGiven--;
                        }
                        else
                        {
                            GivenFullSleep = true;
                        }
                    }
                    else
                    {
                        if (SleepGiven < ConsumableItem.SleepChangeAmount)
                        {
                            Player.HumanState.Sleep.Change(1.0f, true);
                            SleepGiven++;
                        }
                        else
                        {
                            GivenFullSleep = true;
                        }
                    }
                }
                else
                {
                    GivenFullSleep = true;
                }
            }
            GameTimeLastGivenNeeds = Game.GameTime;
        }
    }
}
