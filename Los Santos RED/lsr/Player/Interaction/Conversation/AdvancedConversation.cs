using Rage.Native;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using ExtensionsMethods;

public class AdvancedConversation
{
    private IShopMenus ShopMenus;
    private IModItems ModItems;
    private IZones Zones;
    private Conversation_Simple ConversationSimple;
    private MenuPool MenuPool;
    private UIMenu ConversationMenu;

    public AdvancedConversation(Conversation_Simple conversation_Simple, IModItems modItems, IZones zones, IShopMenus shopMenus)
    {
        ConversationSimple = conversation_Simple;
        ModItems = modItems;
        Zones = zones;
        ShopMenus = shopMenus;
    }
    public void Setup()
    {
        CreateMenu();
    }
    public void Show()
    {
        UpdateMenuItems();
        ConversationMenu.Visible = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                while(EntryPoint.ModController.IsRunning && ConversationMenu.Visible)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                Dispose();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Conversation");

    }
    public void Dispose()
    {
        ConversationMenu.Visible = false;
        ConversationSimple.OnAdvancedConversationStopped();
    }

    private void CreateMenu()
    {
        MenuPool = new MenuPool();
        ConversationMenu = new UIMenu("Conversation", "Select an Option");
        ConversationMenu.RemoveBanner();
        MenuPool.Add(ConversationMenu);
    }
    private void UpdateMenuItems()
    {
        List<ModItem> dealerItems = ModItems.AllItems().Where(x => x.ItemType == ItemType.Drugs && x.ItemSubType == ItemSubType.Narcotic).ToList();
        UIMenuListScrollerItem<ModItem> AskForItemDealer = new UIMenuListScrollerItem<ModItem>("Dealers", "Ask where to find dealers for an item", dealerItems);
        AskForItemDealer.Activated += (menu, item) =>
        {
            AskForItem(AskForItemDealer.SelectedItem, true);
        };
        if (dealerItems.Any())
        {
            ConversationMenu.AddItem(AskForItemDealer);
        }
        List<ModItem> customerItems = ModItems.AllItems().Where(x => x.ItemType == ItemType.Drugs && x.ItemSubType == ItemSubType.Narcotic).ToList();
        UIMenuListScrollerItem<ModItem> AskForItemCustomer = new UIMenuListScrollerItem<ModItem>("Customers", "Ask where to find customers for an item", customerItems);
        AskForItemCustomer.Activated += (menu, item) =>
        {
            AskForItem(AskForItemCustomer.SelectedItem, false);
        };
        if (customerItems.Any())
        {
            ConversationMenu.AddItem(AskForItemCustomer);
        }

        UIMenuItem Cancel = new UIMenuItem("Cancel", "Stop asking questions");
        Cancel.Activated += (menu, item) =>
        {
            Dispose();
        };
        ConversationMenu.AddItem(Cancel);
    }
    private void AskForItem(ModItem modItem, bool isPurchase)
    {
        if(ConversationSimple.ConversingPed == null || !ConversationSimple.ConversingPed.KnownsDrugAreas)
        {
            ReplyUnknown();
            return;
        }
        List<Zone> PossibleZones = Zones.GetZoneByItem(modItem, ShopMenus, isPurchase);
        if (PossibleZones == null)
        {
            ReplyUnknown();
        }
        else
        {
            ReplyFound(PossibleZones, isPurchase);
            Dispose();
        }
        
    }
    private void ReplyUnknown()
    {
        List<string> PossibleReplies = new List<string>() { "I don't know", "How the fuck would I know?","I really have no idea", "I just live here man", "Not sure", "Maybe ask someone else?" };
        ConversationSimple.PedReply(PossibleReplies.PickRandom());
        //Game.DisplaySubtitle(PossibleReplies.PickRandom());
    }
    private void ReplyFound(List<Zone> PossibleZones, bool isPurchase)
    {
        if(PossibleZones == null)
        {
            return;
        }
        Zone selectedZone = PossibleZones.PickRandom();
        if(selectedZone == null)
        {
            return;
        }
        string ZoneString = selectedZone.DisplayName;
        List<string> PossibleReplies;
        if (isPurchase)
        {
            PossibleReplies = new List<string>() {
            $"Check out ~p~{ZoneString}~s~",
            $"Can probably find something in ~p~{ZoneString}~s~",
            $"Go ask around ~p~{ZoneString}~s~",
            $"I heard you can find some in ~p~{ZoneString}~s~",
            $"Swing by ~p~{ZoneString}~s~",
            $"Scope out ~p~{ZoneString}~s~",
            };
        }
        else
        {
            PossibleReplies = new List<string>() {
            $"Always find interested people in ~p~{ZoneString}~s~",
            $"The people in ~p~{ZoneString}~s~ go wild for that",
            $"A good spot to sell would be ~p~{ZoneString}~s~",
            $"There should be some customers in ~p~{ZoneString}~s~",
            $"The best place to start would be ~p~{ZoneString}~s~",
            $"Should be able to offload some of that in ~p~{ZoneString}~s~",
            };
        }
        ConversationSimple.PedReply(PossibleReplies.PickRandom());
        //Game.DisplaySubtitle(PossibleReplies.PickRandom());
        //string zoneName = $"{string.Join(",", PossibleZones.Select(x => x.DisplayName))}";
    }


}

