using LosSantosRED.lsr.Interface;
using Rage.Native;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugLCYMAPSubMenu : DebugSubMenu
{
    public DebugLCYMAPSubMenu(UIMenu debug, MenuPool menuPool, IActionable player) : base(debug, menuPool, player)
    {
    }
    public override void AddItems()
    {
        UIMenu OtherMapMenuItem = MenuPool.AddSubMenu(Debug, "LC YMAP Stuff");
        OtherMapMenuItem.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Various LC YMAP Map items";
        OtherMapMenuItem.Width = 0.35f;
        List<YmapDisabler> CoolStuff = new List<YmapDisabler>() {

new YmapDisabler("manhatsw_stream3",true),
new YmapDisabler("manhatsw_stream2",true),
new YmapDisabler("manhatsw_stream1",true),
new YmapDisabler("manhatsw_stream0",true),
new YmapDisabler("manhatsw_strbig1",true),
new YmapDisabler("manhatsw_strbig0",true),
new YmapDisabler("manhatsw_lod",true),
new YmapDisabler("manhatsw",true),
new YmapDisabler("manhat12_stream8_lod",true),
new YmapDisabler("manhat12_stream8",true),
new YmapDisabler("manhat12_stream7",true),
new YmapDisabler("manhat12_stream6",true),
new YmapDisabler("manhat12_stream5",true),
new YmapDisabler("manhat12_stream4",true),
new YmapDisabler("manhat12_stream3",true),
new YmapDisabler("manhat12_stream2",true),
new YmapDisabler("manhat12_stream1",true),
new YmapDisabler("manhat12_stream0",true),
new YmapDisabler("manhat12_strbig0_lod",true),
new YmapDisabler("manhat12_strbig0",true),
new YmapDisabler("manhat12_lod",true),
new YmapDisabler("manhat12",true),
new YmapDisabler("manhat11_stream8",true),
new YmapDisabler("manhat11_stream7",true),
new YmapDisabler("manhat11_stream6",true),
new YmapDisabler("manhat11_stream5",true),
new YmapDisabler("manhat11_stream4",true),
new YmapDisabler("manhat11_stream3",true),
new YmapDisabler("manhat11_stream2",true),
new YmapDisabler("manhat11_stream1",true),
new YmapDisabler("manhat11_stream0",true),
new YmapDisabler("manhat11_strbig0",true),
new YmapDisabler("manhat11_lod",true),
new YmapDisabler("manhat11",true),
new YmapDisabler("manhat10_stream8",true),
new YmapDisabler("manhat10_stream7",true),
new YmapDisabler("manhat10_stream6",true),
new YmapDisabler("manhat10_stream5",true),
new YmapDisabler("manhat10_stream4",true),
new YmapDisabler("manhat10_stream3",true),
new YmapDisabler("manhat10_stream2",true),
new YmapDisabler("manhat10_stream1",true),
new YmapDisabler("manhat10_stream0",true),
new YmapDisabler("manhat10_strbig0",true),
new YmapDisabler("manhat10_lod",true),
new YmapDisabler("manhat10",true),
new YmapDisabler("manhat09_stream7",true),
new YmapDisabler("manhat09_stream6",true),
new YmapDisabler("manhat09_stream5",true),
new YmapDisabler("manhat09_stream4",true),
new YmapDisabler("manhat09_stream3",true),
new YmapDisabler("manhat09_stream2",true),
new YmapDisabler("manhat09_stream1",true),
new YmapDisabler("manhat09_stream0",true),
new YmapDisabler("manhat09_strbig0",true),
new YmapDisabler("manhat09_lod",true),
new YmapDisabler("manhat09",true),
new YmapDisabler("manhat08_stream9",true),
new YmapDisabler("manhat08_stream8",true),
new YmapDisabler("manhat08_stream7",true),
new YmapDisabler("manhat08_stream6",true),
new YmapDisabler("manhat08_stream5",true),
new YmapDisabler("manhat08_stream4",true),
new YmapDisabler("manhat08_stream3",true),
new YmapDisabler("manhat08_stream2",true),
new YmapDisabler("manhat08_stream11",true),
new YmapDisabler("manhat08_stream10",true),
new YmapDisabler("manhat08_stream1",true),
new YmapDisabler("manhat08_stream0",true),
new YmapDisabler("manhat08_strbig0",true),
new YmapDisabler("manhat08_lod",true),
new YmapDisabler("manhat08",true),
new YmapDisabler("manhat07_stream9",true),
new YmapDisabler("manhat07_stream8",true),
new YmapDisabler("manhat07_stream7",true),
new YmapDisabler("manhat07_stream6",true),
new YmapDisabler("manhat07_stream5",true),
new YmapDisabler("manhat07_stream4",true),
new YmapDisabler("manhat07_stream3",true),
new YmapDisabler("manhat07_stream2",true),
new YmapDisabler("manhat07_stream12",true),
new YmapDisabler("manhat07_stream11",true),
new YmapDisabler("manhat07_stream10",true),
new YmapDisabler("manhat07_stream1",true),
new YmapDisabler("manhat07_stream0",true),
new YmapDisabler("manhat07_strbig0",true),
new YmapDisabler("manhat07_lod",true),
new YmapDisabler("manhat07",true),
new YmapDisabler("manhat06_stream2",true),
new YmapDisabler("manhat06_stream1",true),
new YmapDisabler("manhat06_stream0",true),
new YmapDisabler("manhat06_strbig0",true),
new YmapDisabler("manhat06_lod",true),
new YmapDisabler("manhat06",true),
new YmapDisabler("manhat05_stream9",true),
new YmapDisabler("manhat05_stream8",true),
new YmapDisabler("manhat05_stream7",true),
new YmapDisabler("manhat05_stream6",true),
new YmapDisabler("manhat05_stream5",true),
new YmapDisabler("manhat05_stream4",true),
new YmapDisabler("manhat05_stream3",true),
new YmapDisabler("manhat05_stream2",true),
new YmapDisabler("manhat05_stream13",true),
new YmapDisabler("manhat05_stream12",true),
new YmapDisabler("manhat05_stream11",true),
new YmapDisabler("manhat05_stream10",true),
new YmapDisabler("manhat05_stream1",true),
new YmapDisabler("manhat05_stream0",true),
new YmapDisabler("manhat05_strbig0",true),
new YmapDisabler("manhat05_lod",true),
new YmapDisabler("manhat05",true),
new YmapDisabler("manhat04_stream7",true),
new YmapDisabler("manhat04_stream6",true),
new YmapDisabler("manhat04_stream5",true),
new YmapDisabler("manhat04_stream4",true),
new YmapDisabler("manhat04_stream3",true),
new YmapDisabler("manhat04_stream2",true),
new YmapDisabler("manhat04_stream1",true),
new YmapDisabler("manhat04_stream0",true),
new YmapDisabler("manhat04_strbig0",true),
new YmapDisabler("manhat04_lod",true),
new YmapDisabler("manhat04",true),
new YmapDisabler("manhat03_stream6",true),
new YmapDisabler("manhat03_stream5",true),
new YmapDisabler("manhat03_stream4",true),
new YmapDisabler("manhat03_stream3",true),
new YmapDisabler("manhat03_stream2",true),
new YmapDisabler("manhat03_stream1",true),
new YmapDisabler("manhat03_stream0",true),
new YmapDisabler("manhat03_strbig0",true),
new YmapDisabler("manhat03_lod",true),
new YmapDisabler("manhat03",true),
new YmapDisabler("manhat02_stream9",true),
new YmapDisabler("manhat02_stream8",true),
new YmapDisabler("manhat02_stream7",true),
new YmapDisabler("manhat02_stream6",true),
new YmapDisabler("manhat02_stream5",true),
new YmapDisabler("manhat02_stream4",true),
new YmapDisabler("manhat02_stream3",true),
new YmapDisabler("manhat02_stream2",true),
new YmapDisabler("manhat02_stream12",true),
new YmapDisabler("manhat02_stream11",true),
new YmapDisabler("manhat02_stream10",true),
new YmapDisabler("manhat02_stream1",true),
new YmapDisabler("manhat02_stream0",true),
new YmapDisabler("manhat02_strbig0",true),
new YmapDisabler("manhat02_lod",true),
new YmapDisabler("manhat02",true),
new YmapDisabler("manhat01_stream9",true),
new YmapDisabler("manhat01_stream8",true),
new YmapDisabler("manhat01_stream7",true),
new YmapDisabler("manhat01_stream6",true),
new YmapDisabler("manhat01_stream5",true),
new YmapDisabler("manhat01_stream4",true),
new YmapDisabler("manhat01_stream3",true),
new YmapDisabler("manhat01_stream26",true),
new YmapDisabler("manhat01_stream25",true),
new YmapDisabler("manhat01_stream24",true),
new YmapDisabler("manhat01_stream23",true),
new YmapDisabler("manhat01_stream22",true),
new YmapDisabler("manhat01_stream21",true),
new YmapDisabler("manhat01_stream20",true),
new YmapDisabler("manhat01_stream2",true),
new YmapDisabler("manhat01_stream19",true),
new YmapDisabler("manhat01_stream18",true),
new YmapDisabler("manhat01_stream17",true),
new YmapDisabler("manhat01_stream16",true),
new YmapDisabler("manhat01_stream15",true),
new YmapDisabler("manhat01_stream14",true),
new YmapDisabler("manhat01_stream13",true),
new YmapDisabler("manhat01_stream12",true),
new YmapDisabler("manhat01_stream11",true),
new YmapDisabler("manhat01_stream10",true),
new YmapDisabler("manhat01_stream1",true),
new YmapDisabler("manhat01_stream0",true),
new YmapDisabler("manhat01_strbig1",true),
new YmapDisabler("manhat01_strbig0",true),
new YmapDisabler("manhat01_lod",true),
new YmapDisabler("manhat01",true),
        };
        UIMenuListScrollerItem<YmapDisabler> uIMenuListScrollerItem = new UIMenuListScrollerItem<YmapDisabler>("Toggler", "Toggle YMAP ON and OFF", CoolStuff);
        uIMenuListScrollerItem.Activated += (menu, item) =>
        {
            uIMenuListScrollerItem.SelectedItem.Toggle();
            //menu.Visible = false;
        };
        List<YmapGroupDisabler> ymapGroupDisablers = new List<YmapGroupDisabler>()
        {
            new YmapGroupDisabler("manhat01",new List<string>() {
                "manhat01_stream9",
                "manhat01_stream8",
                "manhat01_stream7",
                "manhat01_stream6",
                "manhat01_stream5",
                "manhat01_stream4",
                "manhat01_stream3",
                "manhat01_stream26",
                "manhat01_stream25",
                "manhat01_stream24",
                "manhat01_stream23",
                "manhat01_stream22",
                "manhat01_stream21",
                "manhat01_stream20",
                "manhat01_stream2",
                "manhat01_stream19",
                "manhat01_stream18",
                "manhat01_stream17",
                "manhat01_stream16",
                "manhat01_stream15",
                "manhat01_stream14",
                "manhat01_stream13",
                "manhat01_stream12",
                "manhat01_stream11",
                "manhat01_stream10",
                "manhat01_stream1",
                "manhat01_stream0",
                "manhat01_strbig1",
                "manhat01_strbig0",
                "manhat01_lod",
                "manhat01",
            }, true),
            new YmapGroupDisabler("manhat02",new List<string>() {
"manhat02_stream9",
"manhat02_stream8",
"manhat02_stream7",
"manhat02_stream6",
"manhat02_stream5",
"manhat02_stream4",
"manhat02_stream3",
"manhat02_stream2",
"manhat02_stream12",
"manhat02_stream11",
"manhat02_stream10",
"manhat02_stream1",
"manhat02_stream0",
"manhat02_strbig0",
"manhat02_lod",
"manhat02",
        }, true),
            new YmapGroupDisabler("manhat03",new List<string>() {
"manhat03_stream6",
"manhat03_stream5",
"manhat03_stream4",
"manhat03_stream3",
"manhat03_stream2",
"manhat03_stream1",
"manhat03_stream0",
"manhat03_strbig0",
"manhat03_lod",
"manhat03",
        }, true),
            new YmapGroupDisabler("manhat04",new List<string>() {
"manhat04_stream7",
"manhat04_stream6",
"manhat04_stream5",
"manhat04_stream4",
"manhat04_stream3",
"manhat04_stream2",
"manhat04_stream1",
"manhat04_stream0",
"manhat04_strbig0",
"manhat04_lod",
"manhat04",
        }, true),
            new YmapGroupDisabler("manhat05",new List<string>() {
"manhat05_stream9",
"manhat05_stream8",
"manhat05_stream7",
"manhat05_stream6",
"manhat05_stream5",
"manhat05_stream4",
"manhat05_stream3",
"manhat05_stream2",
"manhat05_stream13",
"manhat05_stream12",
"manhat05_stream11",
"manhat05_stream10",
"manhat05_stream1",
"manhat05_stream0",
"manhat05_strbig0",
"manhat05_lod",
"manhat05",
        }, true),
            new YmapGroupDisabler("manhat06",new List<string>() {
"manhat06_stream2",
"manhat06_stream1",
"manhat06_stream0",
"manhat06_strbig0",
"manhat06_lod",
"manhat06",
        }, true),
            new YmapGroupDisabler("manhat07",new List<string>() {
"manhat07_stream9",
"manhat07_stream8",
"manhat07_stream7",
"manhat07_stream6",
"manhat07_stream5",
"manhat07_stream4",
"manhat07_stream3",
"manhat07_stream2",
"manhat07_stream12",
"manhat07_stream11",
"manhat07_stream10",
"manhat07_stream1",
"manhat07_stream0",
"manhat07_strbig0",
"manhat07_lod",
"manhat07",
        }, true),
            new YmapGroupDisabler("manhat08",new List<string>() {
"manhat08_stream9",
"manhat08_stream8",
"manhat08_stream7",
"manhat08_stream6",
"manhat08_stream5",
"manhat08_stream4",
"manhat08_stream3",
"manhat08_stream2",
"manhat08_stream11",
"manhat08_stream10",
"manhat08_stream1",
"manhat08_stream0",
"manhat08_strbig0",
"manhat08_lod",
"manhat08",
        }, true),
            new YmapGroupDisabler("manhat09",new List<string>() {
"manhat09_stream7",
"manhat09_stream6",
"manhat09_stream5",
"manhat09_stream4",
"manhat09_stream3",
"manhat09_stream2",
"manhat09_stream1",
"manhat09_stream0",
"manhat09_strbig0",
"manhat09_lod",
"manhat09",
        }, true),
            new YmapGroupDisabler("manhat10",new List<string>() {
"manhat10_stream8",
"manhat10_stream7",
"manhat10_stream6",
"manhat10_stream5",
"manhat10_stream4",
"manhat10_stream3",
"manhat10_stream2",
"manhat10_stream1",
"manhat10_stream0",
"manhat10_strbig0",
"manhat10_lod",
"manhat10",
        }, true),
            new YmapGroupDisabler("manhat11",new List<string>() {
"manhat11_stream8",
"manhat11_stream7",
"manhat11_stream6",
"manhat11_stream5",
"manhat11_stream4",
"manhat11_stream3",
"manhat11_stream2",
"manhat11_stream1",
"manhat11_stream0",
"manhat11_strbig0",
"manhat11_lod",
"manhat11",
        }, true),
            new YmapGroupDisabler("manhat12",new List<string>() {
"manhat12_stream8_lod",
"manhat12_stream8",
"manhat12_stream7",
"manhat12_stream6",
"manhat12_stream5",
"manhat12_stream4",
"manhat12_stream3",
"manhat12_stream2",
"manhat12_stream1",
"manhat12_stream0",
"manhat12_strbig0_lod",
"manhat12_strbig0",
"manhat12_lod",
"manhat12",
        }, true),
            new YmapGroupDisabler("manhatsw",new List<string>() {
"manhatsw_stream3",
"manhatsw_stream2",
"manhatsw_stream1",
"manhatsw_stream0",
"manhatsw_strbig1",
"manhatsw_strbig0",
"manhatsw_lod",
"manhatsw",
        }, true),
        };
        UIMenuListScrollerItem<YmapGroupDisabler> uIMenuListScrollerItem2 = new UIMenuListScrollerItem<YmapGroupDisabler>("Toggler Group", "Toggle YMAP ON and OFF", ymapGroupDisablers);
        uIMenuListScrollerItem2.Activated += (menu, item) =>
        {
            uIMenuListScrollerItem2.SelectedItem.Toggle();
            //menu.Visible = false;
        };
        OtherMapMenuItem.AddItem(uIMenuListScrollerItem);
        OtherMapMenuItem.AddItem(uIMenuListScrollerItem2);
        UIMenuItem fixLCLOD = new UIMenuItem("LC Disable 7", "disable some pesky ymap stuffo");
        fixLCLOD.Activated += (menu, item) =>
        {
            //Player.ClipsetManager.SetWeaponAnimationOverride(fixLCLOD.SelectedItem);

            List<string> toDisable = new List<string>() { "manhat07_stream9",
"manhat07_stream8",
"manhat07_stream7",
"manhat07_stream6",
"manhat07_stream5",
"manhat07_stream4",
"manhat07_stream3",
"manhat07_stream2",
"manhat07_stream12",
"manhat07_stream11",
"manhat07_stream10",
"manhat07_stream1",
"manhat07_stream0",
"manhat07_strbig0",
"manhat07_lod",
"manhat07",};

            foreach (string ipl in toDisable)
            {
                NativeFunction.Natives.REMOVE_IPL(ipl);
            }
            foreach (string ipl in toDisable)
            {
                if (ipl != "manhat07")
                {
                    NativeFunction.Natives.REQUEST_IPL(ipl);
                }
            }
            menu.Visible = false;
        };
        OtherMapMenuItem.AddItem(fixLCLOD);
    }
    private class YmapDisabler
    {
        public YmapDisabler(string name, bool isEnabled)
        {
            Name = name;
            IsEnabled = isEnabled;
        }

        public string Name { get; set; }
        public bool IsEnabled { get; set; } = true;
        public override string ToString()
        {
            return Name + $"-{IsEnabled}";
        }
        public void Toggle()
        {
            if (IsEnabled)
            {
                NativeFunction.Natives.REMOVE_IPL(Name);
                Game.DisplaySubtitle($"Disabled {Name}");
            }
            else
            {
                NativeFunction.Natives.REQUEST_IPL(Name);
                Game.DisplaySubtitle($"Requested {Name}");
            }
            IsEnabled = !IsEnabled;
        }
    }
    private class YmapGroupDisabler
    {
        public YmapGroupDisabler(string name, List<string> iplList, bool isEnabled)
        {
            Name = name;
            IPLList = iplList;
            IsEnabled = isEnabled;
        }
        public string Name { get; set; }
        public List<string> IPLList { get; set; } = new List<string>();
        public bool IsEnabled { get; set; } = true;
        public override string ToString()
        {
            return Name + $"-{IsEnabled}";
        }
        public void Toggle()
        {
            if (IsEnabled)
            {
                foreach (string ipl in IPLList)
                {
                    NativeFunction.Natives.REMOVE_IPL(ipl);
                }
                Game.DisplaySubtitle($"Disabled");
            }
            else
            {
                foreach (string ipl in IPLList)
                {
                    NativeFunction.Natives.REQUEST_IPL(ipl);
                }
                Game.DisplaySubtitle($"Requested");
            }
            IsEnabled = !IsEnabled;
        }
    }
}

