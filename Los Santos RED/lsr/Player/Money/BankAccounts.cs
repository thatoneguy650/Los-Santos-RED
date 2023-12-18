using ExtensionsMethods;
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
    private IPlacesOfInterest PlacesOfInterest;
    private uint GameTimeLastChangedMoney;
    private int money = 0;
    private int currentMoney;

    public int LastChangeMoneyAmount { get; set; }
    public bool RecentlyChangedMoney => GameTimeLastChangedMoney != 0 && Game.GameTime - GameTimeLastChangedMoney <= 7000;
    public int TotalAccountMoney => BankAccountList == null || !BankAccountList.Any() ? 0 : BankAccountList.Sum(x => x.Money);
    public int TotalMoney => TotalAccountMoney + Money;
    public List<BankAccount> BankAccountList { get; set; } = new List<BankAccount>();
    private int Money
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

    public BankAccounts(IBankAccountHoldable player, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        Settings = settings;
        PlacesOfInterest = placesOfInterest;
    }
    public void Setup()
    {
        money = Money;
        currentMoney = Money;
    }
    public void Dispose()
    {
        BankAccountList = new List<BankAccount>();
    }
    public void Reset()
    {
        BankAccountList = new List<BankAccount>();
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
    public int GetMoney(bool useAccounts)
    {
        if(!useAccounts)
        {
            return Money;
        }
        return Money + TotalAccountMoney;
    }
    public int GetOnHandCashSafe()
    {
        int money = GetMoney(false);
        if (money >= 2147483647)
        {
            money = 2147483646;
        }
        return money;
    }
    public void GiveMoney(int Amount, bool useAccounts)
    {
        if (Amount != 0)
        {
            LastChangeMoneyAmount = Amount;
            GameTimeLastChangedMoney = Game.GameTime;
        }
        if (useAccounts)
        {
            EntryPoint.WriteToConsole($"GiveMoney ACCOUNT TO REMOVE {Amount}");
            Amount = GiveMoneyAccount(Amount);
            EntryPoint.WriteToConsole($"GiveMoney ACCOUNT STILL TO REMOVE {Amount}");
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
        //EntryPoint.WriteToConsoleTestLong($"PlayerCashHash {PlayerCashHash} ModelName {Player.ModelName}");
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
    private int GiveMoneyAccount(int Amount)
    {
        if(Amount > 0)
        {
            BankAccount bankAccount = BankAccountList.OrderBy(x => x.IsPrimary ? 0 : 999).FirstOrDefault();
            if(bankAccount != null)
            {
                bankAccount.Money += Amount;
                return 0;//is positive and has added the money to the account, sent remaining to zero
            }
            return Amount;
        }
        else
        {
            int AccountMoneyTakenAlready = Amount;
            foreach (BankAccount ba in BankAccountList.OrderByDescending(x => x.Money))
            {
                EntryPoint.WriteToConsole($"GiveMoneyAccount BEGIN {ba.BankContactName} {ba.Money}");
                if (AccountMoneyTakenAlready + ba.Money < 0)
                {
                    ba.Money = 0;
                    AccountMoneyTakenAlready += ba.Money;
                }
                else
                {
                    ba.Money = ba.Money += AccountMoneyTakenAlready;
                    AccountMoneyTakenAlready = 0;
                }
                EntryPoint.WriteToConsole($"GiveMoneyAccount END {ba.BankContactName} {ba.Money}");
            }
            return AccountMoneyTakenAlready;
        }
    }
    public void SetCash(int Amount)
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

        //EntryPoint.WriteToConsoleTestLong($"PlayerCashHash {PlayerCashHash} ModelName {Player.ModelName}");
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
    public string CashDisplay(bool showFull)
    {
        string toReturn = $"${Money}";

        if (showFull)
        {
            if (BankAccountList != null && BankAccountList.Any())
            {
                foreach (BankAccount ba in BankAccountList)
                {
                    toReturn += ba.CashDisplay;
                }
            }
        }
        else
        {
            int totalAccount = TotalAccountMoney;
            if (totalAccount > 0)
            {
                toReturn += $" (${totalAccount})";
            }
        }
        return toReturn;
    }
    public BankAccount GetAccount(string name)
    {
        BankAccount bankAccount = BankAccountList.Where(x => x.BankContactName == name).FirstOrDefault();
        return bankAccount;
    }
    public int GetAccountValue(string name)
    {
        BankAccount ba = GetAccount(name);
        if(ba == null)
        {
            return 0;
        }
        return ba.Money;
    }
    public void WriteToConsole()
    {
        EntryPoint.WriteToConsole("BANK ACCOUNTS---------------");

        EntryPoint.WriteToConsole($"On Hand Cash {Money}");
        foreach(BankAccount bankAccount in BankAccountList)
        {
            EntryPoint.WriteToConsole($"Account: {bankAccount.BankContactName} {bankAccount.Money}");
        }
        EntryPoint.WriteToConsole("BANK ACCOUNTS---------------");
    }
    public void CreateRandomAccount(int amount)
    {
        Bank randomBank = PlacesOfInterest.PossibleLocations.Banks.PickRandom();
        if(randomBank == null)
        {
            EntryPoint.WriteToConsole("CANNOT GIVE RANDOM BANK ACCOUNT, NO BANKS FOUND");
            return;
        }
        BankAccountList.Add(new BankAccount(randomBank.Name,randomBank.ShortName, amount));
    }
    public void CreateNewAccount(Bank bank)
    {
        if(bank == null)
        {
            return;
        }
        if(!BankAccountList.Any(x=> x.BankContactName == bank.Name))
        {
            BankAccountList.Add(new BankAccount(bank.Name, bank.ShortName, 0));
        }
    }

    public void Remove(BankAccount selectedItem)
    {
        if(selectedItem == null)
        {
            return;
        }
        BankAccountList.Remove(selectedItem);
    }
}

