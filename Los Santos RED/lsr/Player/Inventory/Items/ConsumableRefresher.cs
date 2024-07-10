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
    private int ArmorGiven;
    private bool GivenFullHealth;
    private bool GivenFullArmor;
    private uint GameTimeLastGivenHealth;
    private uint GameTimeLastGivenArmor;
    private uint GameTimeLastGivenNeeds;
    private float HungerGiven;
    private bool GivenFullHunger { get; set; } = false;
    private float ThirstGiven;
    private bool GivenFullThirst { get; set; } = false;
    private float SleepGiven;
    private bool GivenFullSleep { get; set; } = false;

    public ConsumableRefresher(IActionable player, ConsumableItem foodItem, ISettingsProvideable settings)
    {
        Player = player;
        ConsumableItem = foodItem;
        Settings = settings;
    }

    public bool IsFinished { get; internal set; }
    public float SpeedMultiplier { get; set; } = 1.0f;

    public bool IsIntervalBased { get; set; } = false;
    public int IntervalHealthScalar { get; set; } = 2;
    public int IntervalArmorScalar { get; set; } = 2;

    public void Update()
    {
        if (ConsumableItem == null)
        {
            IsFinished = true;
            return;
        }
        if (!IsFinished)
        {
            if (IsIntervalBased)
            {
                UpdateIntervalHealthGain();
                UpdateIntervalArmorGain();
                UpdateIntervalNeeds();
            }
            else
            {
                UpdateHealthGain();
                UpdateArmorGain();
                UpdateNeeds();
            }
            CheckStatus();
        }


    }
    public void FullyConsume()
    {
        if (ConsumableItem.ChangesHealth && (!Settings.SettingsManager.NeedsSettings.ApplyNeeds || ConsumableItem.AlwaysChangesHealth))
        {
            Player.HealthManager.ChangeHealth(ConsumableItem.HealthChangeAmount);
        }
        if (ConsumableItem.ChangesArmor)
        {
            Player.ArmorManager.ChangeArmor(ConsumableItem.ArmorChangeAmount);
        }
        if (Settings.SettingsManager.NeedsSettings.ApplyNeeds)
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
            if (GivenFullHunger && GivenFullSleep && GivenFullThirst && (GivenFullHealth || !ConsumableItem.AlwaysChangesHealth) && GivenFullArmor)
            {
                EntryPoint.WriteToConsole($"Finished CIG1 {ConsumableItem.Name}");
                IsFinished = true;
            }
        }
        else
        {
            if (GivenFullHealth && GivenFullArmor)
            {
                EntryPoint.WriteToConsole($"Finished CIG2 {ConsumableItem.Name}");
                IsFinished = true;
            }
        }
    }
    private void UpdateHealthGain()
    {
        if (Game.GameTime - GameTimeLastGivenHealth >= (Settings.SettingsManager.NeedsSettings.TimeBetweenGain / SpeedMultiplier))
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
            if (HealthGiven == ConsumableItem.HealthChangeAmount || Player.HealthManager.IsMaxHealth)
            {
                EntryPoint.WriteToConsole("GIVEN FULL HEALTH");
                GivenFullHealth = true;
            }
            GameTimeLastGivenHealth = Game.GameTime;
        }
    }
    private void UpdateArmorGain()
    {
        if (Game.GameTime - GameTimeLastGivenArmor >= (Settings.SettingsManager.NeedsSettings.TimeBetweenGain / SpeedMultiplier))
        {
            if (ConsumableItem.ChangesArmor)
            {
                if (ConsumableItem.ArmorChangeAmount > 0 && ArmorGiven < ConsumableItem.ArmorChangeAmount)
                {
                    ArmorGiven++;
                    Player.ArmorManager.ChangeArmor(1);
                }
                else if (ConsumableItem.ArmorChangeAmount < 0 && ArmorGiven > ConsumableItem.ArmorChangeAmount)
                {
                    ArmorGiven--;
                    Player.ArmorManager.ChangeArmor(-1);
                }
            }
            if (ArmorGiven == ConsumableItem.ArmorChangeAmount)
            {
                EntryPoint.WriteToConsole("GIVEN FULL ARMOR");
                GivenFullArmor = true;
            }
            GameTimeLastGivenArmor = Game.GameTime;
        }
    }
    private void UpdateNeeds()
    {
        if (Game.GameTime - GameTimeLastGivenNeeds >= (Settings.SettingsManager.NeedsSettings.TimeBetweenGain / SpeedMultiplier))
        {
            if (ConsumableItem.ChangesNeeds)
            {
                if (ConsumableItem.ChangesHunger)
                {
                    if (ConsumableItem.HungerChangeAmount < 0.0f)
                    {
                        if (!Player.HumanState.Hunger.IsMin && HungerGiven > ConsumableItem.HungerChangeAmount)
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
                        if (!Player.HumanState.Hunger.IsMax && HungerGiven < ConsumableItem.HungerChangeAmount)
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
                        if (!Player.HumanState.Thirst.IsMin && ThirstGiven > ConsumableItem.ThirstChangeAmount)
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
                        if (!Player.HumanState.Thirst.IsMax && ThirstGiven < ConsumableItem.ThirstChangeAmount)
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
                        if (!Player.HumanState.Sleep.IsMin && SleepGiven > ConsumableItem.SleepChangeAmount)
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
                        if (!Player.HumanState.Sleep.IsMax && SleepGiven < ConsumableItem.SleepChangeAmount)
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


    private void UpdateIntervalHealthGain()
    {
        if (ConsumableItem.ChangesHealth && (!Settings.SettingsManager.NeedsSettings.ApplyNeeds || ConsumableItem.AlwaysChangesHealth))
        {
            if (ConsumableItem.HealthChangeAmount > 0 && HealthGiven < ConsumableItem.HealthChangeAmount)
            {
                HealthGiven++;
                Player.HealthManager.ChangeHealth(1 * IntervalHealthScalar);
            }
            else if (ConsumableItem.HealthChangeAmount < 0 && HealthGiven > ConsumableItem.HealthChangeAmount)
            {
                HealthGiven--;
                Player.HealthManager.ChangeHealth(-1 * IntervalHealthScalar);
            }
        }
        if (HealthGiven == ConsumableItem.HealthChangeAmount || Player.HealthManager.IsMaxHealth)
        {
            EntryPoint.WriteToConsole("GIVEN FULL HEALTH");
            GivenFullHealth = true;
        }
    }
    private void UpdateIntervalArmorGain()
    {
        if (ConsumableItem.ChangesArmor)
        {
            if (ConsumableItem.ArmorChangeAmount > 0 && ArmorGiven < ConsumableItem.ArmorChangeAmount)
            {
                ArmorGiven++;
                Player.ArmorManager.ChangeArmor(1 * IntervalArmorScalar);
            }
            else if (ConsumableItem.ArmorChangeAmount < 0 && ArmorGiven > ConsumableItem.ArmorChangeAmount)
            {
                ArmorGiven--;
                Player.ArmorManager.ChangeArmor(-1 * IntervalArmorScalar);
            }
        }
        if (ArmorGiven == ConsumableItem.ArmorChangeAmount)
        {
            EntryPoint.WriteToConsole("GIVEN FULL ARMOR");
            GivenFullArmor = true;
        }
    }


    private void UpdateIntervalNeeds()
    {
        if (ConsumableItem.ChangesNeeds)
        {
            if (ConsumableItem.ChangesHunger)
            {
                if (ConsumableItem.HungerChangeAmount < 0.0f)
                {
                    if (!Player.HumanState.Hunger.IsMin && HungerGiven > ConsumableItem.HungerChangeAmount)
                    {
                        Player.HumanState.Hunger.Change(-Settings.SettingsManager.ActivitySettings.IntervalHungerScalar, true);
                        HungerGiven-=Settings.SettingsManager.ActivitySettings.IntervalHungerScalar;
                    }
                    else
                    {
                        GivenFullHunger = true;
                    }
                }
                else
                {
                    if (!Player.HumanState.Hunger.IsMax && HungerGiven < ConsumableItem.HungerChangeAmount)
                    {
                        Player.HumanState.Hunger.Change(Settings.SettingsManager.ActivitySettings.IntervalHungerScalar, true);
                        HungerGiven+=Settings.SettingsManager.ActivitySettings.IntervalHungerScalar;
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
                    if (!Player.HumanState.Thirst.IsMin && ThirstGiven > ConsumableItem.ThirstChangeAmount)
                    {
                        Player.HumanState.Thirst.Change(-Settings.SettingsManager.ActivitySettings.IntervalThirstScalar, true);
                        ThirstGiven-=Settings.SettingsManager.ActivitySettings.IntervalThirstScalar;
                    }
                    else
                    {
                        GivenFullThirst = true;
                    }
                }
                else
                {
                    if (!Player.HumanState.Thirst.IsMax && ThirstGiven < ConsumableItem.ThirstChangeAmount)
                    {
                        Player.HumanState.Thirst.Change(Settings.SettingsManager.ActivitySettings.IntervalThirstScalar, true);
                        ThirstGiven+=Settings.SettingsManager.ActivitySettings.IntervalThirstScalar;
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
                    if (!Player.HumanState.Sleep.IsMin && SleepGiven > ConsumableItem.SleepChangeAmount)
                    {
                        Player.HumanState.Sleep.Change(-Settings.SettingsManager.ActivitySettings.IntervalSleepScalar, true);
                        SleepGiven-=Settings.SettingsManager.ActivitySettings.IntervalSleepScalar;
                    }
                    else
                    {
                        GivenFullSleep = true;
                    }
                }
                else
                {
                    if (!Player.HumanState.Sleep.IsMax && SleepGiven < ConsumableItem.SleepChangeAmount)
                    {
                        Player.HumanState.Sleep.Change(Settings.SettingsManager.ActivitySettings.IntervalSleepScalar, true);
                        SleepGiven+=Settings.SettingsManager.ActivitySettings.IntervalSleepScalar;
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
    }
}
