using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

public class BankInterior : Interior
{
    protected Bank bank;
    public Bank Bank => bank;
    public List<BankDrawerInteract> BankDrawerInteracts { get; set; } = new List<BankDrawerInteract>();
    [XmlIgnore]
    public override List<InteriorInteract> AllInteractPoints
    {
        get
        {
            List<InteriorInteract> AllInteracts = new List<InteriorInteract>();
            AllInteracts.AddRange(InteractPoints);
            AllInteracts.AddRange(BankDrawerInteracts);
            return AllInteracts;
        }
    }
    public BankInterior()
    {

    }
    public BankInterior(int iD, string name) : base(iD, name)
    {

    }
    public void SetBank(Bank newBank)
    {
        bank = newBank;
        foreach (BankDrawerInteract test in BankDrawerInteracts)
        {
            test.Bank = newBank;
            //test.TotalCash = RandomItems.GetRandomNumberInt(Bank.DrawerCashMin, Bank.DrawerCashMax);
        }
    }
}

