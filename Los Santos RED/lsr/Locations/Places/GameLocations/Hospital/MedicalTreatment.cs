using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class MedicalTreatment
{
    public MedicalTreatment()
    {

    }
    public MedicalTreatment(string name, string description, int healthGained, int price)
    {
        Name = name;
        Description = description;
        HealthGained = healthGained;
        Price = price;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public int HealthGained { get; set; }
    public int Price { get; set; }
    private string FormattedPrice => $"~r~${Price}~s~";
    private string FormattedDescription(ILocationInteractable Player) => Description + $"~n~Health Gained: ~g~{HealthGained}~s~~n~Health Required: ~r~{Player.Character.MaxHealth - Player.Character.Health}~s~";
    public void AddToMenu(Hospital hospital, UIMenu rageMenu, ILocationInteractable player)
    {
        UIMenuItem uIMenuItem = new UIMenuItem(Name, FormattedDescription(player)) { RightLabel = FormattedPrice };
        uIMenuItem.Activated += (sender, selecteditem) =>
        {
            if(player.BankAccounts.GetMoney(true) < Price)
            {
                hospital.DisplayInsufficientFundsMessage();
                return;
            }
            hospital.DisplayPurchaseMessage();
            player.BankAccounts.GiveMoney(-1 * Price, true);
            player.HealthManager.ChangeHealth(HealthGained);
            sender.Visible = false;
        };
        rageMenu.AddItem(uIMenuItem);
    }
}

