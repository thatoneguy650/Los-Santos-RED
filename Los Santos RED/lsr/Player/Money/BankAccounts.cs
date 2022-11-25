using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BankAccounts
{
    private IBankAccountHoldable Player;
    private ISettingsProvideable Settings;
    private uint GameTimeLastChangedMoney;
    private int money = 0;
    private int currentMoney;

    public int LastChangeMoneyAmount { get; set; }
    public bool RecentlyChangedMoney => GameTimeLastChangedMoney != 0 && Game.GameTime - GameTimeLastChangedMoney <= 7000;
    public int Money
    {
        get
        {
            int CurrentCash;
            uint PlayerCashHash;
            if (Player.CharacterModelIsPrimaryCharacter)
            {
                PlayerCashHash = NativeHelper.CashHash(Player.ModelName.ToLower());
            }
            else
            {
                PlayerCashHash = NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias);
            }
            if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter || Player.CharacterModelIsPrimaryCharacter)
            {
                unsafe
                {
                    NativeFunction.CallByName<int>("STAT_GET_INT", PlayerCashHash, &CurrentCash, -1);
                }
                return CurrentCash;
            }
            else
            {
                return money;
            }
        }
    }

    public BankAccounts(IBankAccountHoldable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {

    }
    public void Reset()
    {

    }
    public void Update()
    {
        if (currentMoney != Money)
        {
            LastChangeMoneyAmount = Money - currentMoney;
            GameTimeLastChangedMoney = Game.GameTime;
            currentMoney = Money;
        }
    }
    public void GiveMoney(int Amount)
    {
        if (Amount != 0)
        {
            LastChangeMoneyAmount = Amount;
            GameTimeLastChangedMoney = Game.GameTime;
        }
        int CurrentCash;
        uint PlayerCashHash;
        if (Player.CharacterModelIsPrimaryCharacter)
        {
            PlayerCashHash = NativeHelper.CashHash(Player.ModelName.ToLower());
        }
        else
        {
            PlayerCashHash = NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias);
        }
        EntryPoint.WriteToConsole($"PlayerCashHash {PlayerCashHash} ModelName {Player.ModelName}");
        if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter || Player.CharacterModelIsPrimaryCharacter)
        {

            unsafe
            {
                NativeFunction.CallByName<int>("STAT_GET_INT", PlayerCashHash, &CurrentCash, -1);
            }
            if (CurrentCash + Amount < 0)
            {
                NativeFunction.CallByName<int>("STAT_SET_INT", PlayerCashHash, 0, 1);
            }
            else
            {
                NativeFunction.CallByName<int>("STAT_SET_INT", PlayerCashHash, CurrentCash + Amount, 1);
            }
        }
        else
        {
            if (money + Amount < 0)
            {
                money = 0;
            }
            else
            {
                money += Amount;
            }
        }
        //currentMoney = Money;
    }
    public void SetMoney(int Amount)
    {
        uint PlayerCashHash;
        if (Player.CharacterModelIsPrimaryCharacter)
        {
            PlayerCashHash = NativeHelper.CashHash(Player.ModelName.ToLower());
        }
        else
        {
            PlayerCashHash = NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias);
        }

        EntryPoint.WriteToConsole($"PlayerCashHash {PlayerCashHash} ModelName {Player.ModelName}");
        if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter || Player.CharacterModelIsPrimaryCharacter)
        {
            NativeFunction.CallByName<int>("STAT_SET_INT", PlayerCashHash, Amount, 1);
        }
        else
        {
            money = Amount;
        }
        currentMoney = Money;
    }


}

