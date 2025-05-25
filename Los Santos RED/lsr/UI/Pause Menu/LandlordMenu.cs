using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


public class LandlordMenu
{
    private ILocationInteractable Player;
    private TabView tabView;
    private ResidencesTab ResidencesTab;
    private BusinessesTab BusinessesTab;
    private List<ITabbableMenu> Tabs = new List<ITabbableMenu>();
    private ITimeReportable Time;
    public LandlordMenu(ILocationInteractable player, ITimeReportable time)
    {
        Player = player;
        Time = time;
    }
    public void Setup()
    {
        tabView = new TabView("Los Santos ~r~RED~s~ Property Manager");
        tabView.ScrollTabs = true;
        tabView.OnMenuClose += (s, e) =>
        {
            Player.ActivityManager.StopDynamicActivity();
            Game.IsPaused = false;
        };
        Game.RawFrameRender += (s, e) => tabView.DrawTextures(e.Graphics);
        BusinessesTab = new BusinessesTab(Player, tabView);
        ResidencesTab = new ResidencesTab(Player, tabView);

        Tabs.Add(BusinessesTab);
        Tabs.Add(ResidencesTab);
    }
    public void Toggle()
    {
        if (!TabView.IsAnyPauseMenuVisible)
        {
            if (!tabView.Visible)
            {
                UpdateMenu();
                Game.IsPaused = true;
            }
            tabView.Visible = !tabView.Visible;
        }
    }
    public void Update()
    {
        tabView.Update();
        if (tabView.Visible)
        {
            tabView.Money = Time.CurrentDateTime.ToString("ddd, dd MMM yyyy hh:mm tt");
        }
    }
    public void RefreshMenu()
    {
        UpdateMenu();
    }
    private void UpdateMenu()
    {
        tabView.MoneySubtitle = Player.BankAccounts.TotalMoney.ToString("C0");
        tabView.Name = Player.PlayerName;
        tabView.Money = Time.CurrentTime;
        tabView.Tabs.Clear();


        foreach (ITabbableMenu tabbableMenu in Tabs)
        {
            tabbableMenu.AddItems();
        }

        tabView.RefreshIndex();
    }
}
