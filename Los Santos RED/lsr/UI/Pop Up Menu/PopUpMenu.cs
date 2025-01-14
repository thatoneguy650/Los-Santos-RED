using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


//Needs a refactor big time, get rid of the strings and hokey lookup bullshit, but it works pretty well as is
public class PopUpMenu
{
    private readonly string DefaultOnFootGroupName = "DefaultOnFoot";
    private readonly string DefaultInVehicleName = "DefaultInVehicle";


    private readonly string GroupMemberSubMenuName = "GroupMembersSubMenu";
    private readonly string AffiliationSubMenuName = "AffiliationSubMenu";
    private readonly string InventoryCategoriesSubMenuName = "InventoryCategoriesSubMenu";
    private readonly string WeaponsSubMenuName = "WeaponsSubMenu";
    private readonly string ActionsSubMenuName = "ActionsSubMenu";
    private readonly string VehicleActionsSubMenuName = "VehicleActionsSubMenu";
    private readonly string AffilitationSubMenuName = "AffilitationSubMenu";

    private IActionable Player;
    private ISettingsProvideable Settings;
    private IGestures Gestures;
    private IDances Dances;
    private UI UI;
    private Regex rgx = new Regex("[^a-zA-Z0-9]");
    private List<PositionMap> PositionMaps = new List<PositionMap>();
    private PositionMap ClosestPositionMap;
    private PopUpBox SelectedMenuMap;
    private PopUpBox PrevSelectedMenuMap;
    private uint GameTimeLastClicked;
   // private int ActionSoundID;
    private float ConsistencyScale;
    private int TransitionInSound;
    private int TransitionOutSound;





    private int CurrentPage = 0;
    private int TotalPages = 0;
    private float excessiveItemScaler;
    private float excessiveCenterScaler;
    private List<PopUpBoxGroup> PopUpMenuGroups = new List<PopUpBoxGroup>();
    private PopUpBox NextPageMenuMap;
    private PopUpBox PrevPageMenuMap;
    private PopUpBox MainMenuMenuMap;
    private PopUpBox DebugMenuMenuMap;
    private PopUpBox BurnerPhoneMenuMap;
    private List<PopUpBox> ButtonPromptItems;
    private List<PopUpBox> AlwaysShownItems;
    private Texture Sign10;
    private Texture Sign15;
    private Texture Sign20;
    private Texture Sign25;
    private Texture Sign30;
    private Texture Sign35;
    private Texture Sign40;
    private Texture Sign45;
    private Texture Sign50;
    private Texture Sign55;
    private Texture Sign60;
    private Texture Sign65;
    private Texture Sign70;
    private Texture Sign75;
    private Texture Sign80;
    private Texture SpeedLimitToDraw;
    private HashSet<DrawableIcon> IconsToDraw = new HashSet<DrawableIcon>();
    private float SpeedLimitConsistencyScale;
    private float SpeedLimitScale;
    private float SpeedLimitPosX;
    private float SpeedLimitPosY;
    private float CursorXPos;
    private float CursorYPos;
    private uint GameTimeStartedDisplaying;
    private bool HasStoppedPressingDisplayKey;
    private uint GameTimeLastClosed;
    private PopUpBox PrevPopUpBox;
    private string PrevPopUpBoxGroupID;
    private string CurrentPopUpBoxGroupID;
    private List<Tuple<int, string>> MenuStack = new List<Tuple<int, string>>();

    private bool IsCurrentPopUpBoxGroupDefault => CurrentPopUpBoxGroupID == DefaultInVehicleName || CurrentPopUpBoxGroupID == DefaultOnFootGroupName;
    private PopUpBox GetCurrentPopUpBox(int ID) => GetCurrentPopUpBoxes()?.FirstOrDefault(x => x.ID == ID);
    private List<PopUpBox> GetCurrentPopUpBoxes() => PopUpMenuGroups.FirstOrDefault(x => x.ID == CurrentPopUpBoxGroupID)?.PopUpBoxes;
    public bool HasRanItem { get; private set; }

    private bool SetSlowMo;
    private bool SetPaused;
    private float prevXPercent;
    private uint GameTimeLastPressedSubMenu;

    public bool IsActive { get; private set; }
    public bool VeryRecentlyClosed => Game.GameTime - GameTimeLastClosed <= 200;
    public bool RecentlyClosed => Game.GameTime - GameTimeLastClosed <= 500;
    public PopUpMenu(IActionable player, ISettingsProvideable settings, UI uI, IGestures gestures, IDances dances)
    {
        Player = player;
        Settings = settings;
        UI = uI;
        Gestures = gestures;
        Dances = dances;
    }
    public void Setup()
    {
        SetupBoxes();
        SetupTextures();
    }
    public void Draw()
    {
        if (IsActive)
        {
            UpdateDefaultMapping(false);
            ProcessControllerInput();
            DrawItems();
            DisableControls();
            FindClosestPositionMap();
            UpdateSelection();
        }
        else if (VeryRecentlyClosed)
        {
            DisableControls();
        }
    }
    public void Dispose()
    {
        IsActive = false;
        Game.TimeScale = 1.0f;
        Game.RawFrameRender -= DrawSprites;
    }
    public void CloseMenu()
    {
        IsActive = false;
        //Game.TimeScale = 1.0f;
        GameTimeLastClosed = Game.GameTime;
        Game.DisplaySubtitle("");//clear the subtitles out
        if (Settings.SettingsManager.ActionWheelSettings.SetTransitionEffectOnActivate)
        {
            NativeFunction.Natives.x068E835A1D0DC0E3(Settings.SettingsManager.ActionWheelSettings.TransitionInEffect);
            NativeFunction.Natives.x2206bf9a37b7f724(Settings.SettingsManager.ActionWheelSettings.TransitionOutEffect, 0, false);
        }
        if (SetSlowMo)
        {
            Game.TimeScale = 1.0f;
        }
        if (SetPaused)
        {
            Game.IsPaused = false;
        }
        if (Settings.SettingsManager.ActionWheelSettings.PlayTransitionSoundsOnActivate)
        {
            NativeFunction.Natives.STOP_SOUND(TransitionInSound);
            NativeFunction.Natives.RELEASE_SOUND_ID(TransitionInSound);
        }
        CurrentPopUpBoxGroupID = DefaultOnFootGroupName;
        IsActive = false;
        Game.RawFrameRender -= DrawSprites;
    }
    public void ShowMenu()
    {
        GameTimeStartedDisplaying = Game.GameTime;
        IsActive = true;
        SelectedMenuMap = null;
        ClosestPositionMap = null;
        PrevSelectedMenuMap = null;
        HasStoppedPressingDisplayKey = false;
        HasRanItem = false;
        SetSlowMo = false;
        SetPaused = false;
        MenuStack = new List<Tuple<int, string>>();
        Game.RawFrameRender += DrawSprites;
        
        NativeFunction.Natives.xFC695459D4D0E219(0.5f, 0.5f);//_SET_CURSOR_LOCATION
        CursorXPos = 0.5f;
        CursorYPos = 0.5f;
        prevXPercent = 0.0f;


        if (Settings.SettingsManager.ActionWheelSettings.SetPauseOnActivate || (NativeHelper.IsUsingController && Settings.SettingsManager.ActionWheelSettings.SetPauseOnActivateControllerOnly))
        {
            if(!Game.IsPaused)
            {
                SetPaused = true;
                Game.IsPaused = true;
            }
        }
        else if (Settings.SettingsManager.ActionWheelSettings.SetSlowMoOnActivate || (NativeHelper.IsUsingController && Settings.SettingsManager.ActionWheelSettings.SetSlowMoOnActivateControllerOnly))
        {
            if(Game.TimeScale == 1.0f)
            {
                SetSlowMo = true;
                Game.TimeScale = Settings.SettingsManager.ActionWheelSettings.SlowMoScale;
            }
        }   

        if (Settings.SettingsManager.ActionWheelSettings.SetTransitionEffectOnActivate)
        {
            NativeFunction.Natives.x2206bf9a37b7f724(Settings.SettingsManager.ActionWheelSettings.TransitionInEffect, 0, true);
        }
        //ActionSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
        if (Settings.SettingsManager.ActionWheelSettings.PlayTransitionSoundsOnActivate)
        {        
            NativeFunction.Natives.STOP_SOUND(TransitionOutSound);
            NativeFunction.Natives.RELEASE_SOUND_ID(TransitionOutSound);
            TransitionInSound = NativeFunction.Natives.GET_SOUND_ID<int>();
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(TransitionInSound, "1st_Person_Transition", "PLAYER_SWITCH_CUSTOM_SOUNDSET", 1);
        }
        CurrentPopUpBoxGroupID = DefaultOnFootGroupName;
        UpdateInventoryMenuGroups();
        GameFiber.Yield();
        UpdateGroupMenuGroups();
        GameFiber.Yield();
        UpdateAffiliationMenuGroups();
        GameFiber.Yield();
    }
    private void UpdateInventoryMenuGroups_Old()
    {
        PopUpMenuGroups.RemoveAll(x => x.Group == "Inventory");
        int CategoryID = 0;
        int ItemID = 0;
        List<PopUpBox> InventoryCategoriesSubMenu = new List<PopUpBox>();



        foreach (ItemType mi in Player.Inventory.ItemsList.GroupBy(x => x.ModItem?.ItemType).Select(x => x.Key).Distinct().OrderBy(x => x.Value))
        {
            InventoryCategoriesSubMenu.Add(new PopUpBox(CategoryID, mi.ToString(), $"{mi}SubMenu", $"Open the {mi} Sub Menu") { ClosesMenu = false });
            ItemID = 0;
            List<PopUpBox> InventoryCategorySubMenu = new List<PopUpBox>();
            foreach (InventoryItem ii in Player.Inventory.ItemsList.Where(x => x.ModItem != null && x.ModItem.ItemType == mi))
            {
                InventoryCategorySubMenu.Add(new PopUpBox(ItemID, ii.ModItem.Name, $"{ii.ModItem.Name}SubMenu", ii.Description) { ClosesMenu = false });
                List<PopUpBox> InventoryActionSubMenu = new List<PopUpBox>();
                InventoryActionSubMenu.Add(new PopUpBox(0, "Use", new Action(() => Player.ActivityManager.UseInventoryItem(ii.ModItem, true)), $"Use {ii.ModItem.Name}"));
                InventoryActionSubMenu.Add(new PopUpBox(1, "Discard All", new Action(() => Player.ActivityManager.DropInventoryItem(ii.ModItem, ii.Amount)), $"Discard All {ii.ModItem.Name} ({ii.Amount})"));
                InventoryActionSubMenu.Add(new PopUpBox(2, "Discard One", new Action(() => Player.ActivityManager.DropInventoryItem(ii.ModItem, 1)), $"Discard {ii.ModItem.Name}"));
                if (ii.Amount > 5)
                {
                    InventoryActionSubMenu.Add(new PopUpBox(3, "Discard Five", new Action(() => Player.ActivityManager.DropInventoryItem(ii.ModItem, 5)), $"Discard {ii.ModItem.Name} ({5})"));
                }
                PopUpMenuGroups.Add(new PopUpBoxGroup($"{ii.ModItem.Name}SubMenu", InventoryActionSubMenu) { IsChild = true, Group = "Inventory" });
                ItemID++;
            }
            PopUpMenuGroups.Add(new PopUpBoxGroup($"{mi}SubMenu", InventoryCategorySubMenu) { IsChild = true, Group = "Inventory" });
            CategoryID++;
        }
        PopUpMenuGroups.Add(new PopUpBoxGroup(InventoryCategoriesSubMenuName, InventoryCategoriesSubMenu) { IsChild = true, Group = "Inventory" });
    }
    private void UpdateInventoryMenuGroups()
    {
        PopUpMenuGroups.RemoveAll(x => x.Group == "Inventory");
        int ItemTypeID = 0;
        int ItemSubTypeID = 0;
        int ItemID = 0;
        List<PopUpBox> TopLevelList = new List<PopUpBox>();
        ItemTypeID = 0;
        foreach (ItemType mi in Player.Inventory.ItemsList.GroupBy(x => x.ModItem?.ItemType).Select(x => x.Key).Distinct().OrderBy(x => x.Value))
        {
            PopUpBox itemTypeBox = new PopUpBox(ItemTypeID, mi.ToString(), $"{mi}SubMenu", $"Open the {mi} Sub Menu") { ClosesMenu = false };
            TopLevelList.Add(itemTypeBox);    
            List<PopUpBox> MiddleLevelList = new List<PopUpBox>();
            ItemSubTypeID = 0;
            foreach (ItemSubType itemSubType in Player.Inventory.ItemsList.Where(x => x.ModItem != null && x.ModItem.ItemType == mi).GroupBy(x=> x.ModItem?.ItemSubType).Select(x => x.Key).Distinct().OrderBy(x=> x.Value))
            {
                PopUpBox itemSubTypeBox = new PopUpBox(ItemSubTypeID, itemSubType.ToString(), $"{mi}{itemSubType}SubMenu", $"Open the {itemSubType} Sub Menu") { ClosesMenu = false };
                MiddleLevelList.Add(itemSubTypeBox);
                List<PopUpBox> LowLevelList = new List<PopUpBox>();
                ItemID = 0;
                foreach (InventoryItem ii in Player.Inventory.ItemsList.Where(x => x.ModItem != null && x.ModItem.ItemType == mi && x.ModItem.ItemSubType == itemSubType))
                {
                    LowLevelList.Add(new PopUpBox(ItemID, ii.ModItem.DisplayName, $"{ii.ModItem.Name}SubMenu", ii.Description) { ClosesMenu = false });

                    //LowLevelList.Add(new PopUpBox(ItemID, ii.ModItem.DisplayName, $"{ii.ModItem.Name}SubMenu", ii.ModItem.DisplayDescription) { ClosesMenu = false });
                    List<PopUpBox> InventoryActionSubMenu = new List<PopUpBox>();
                    InventoryActionSubMenu.Add(new PopUpBox(0, "Use", new Action(() => Player.ActivityManager.UseInventoryItem(ii.ModItem, true)), $"Use {ii.ModItem.DisplayName}"));
                    InventoryActionSubMenu.Add(new PopUpBox(1, "Discard", $"Discard{ii.ModItem.Name}SubMenu", $"Discard {ii.ModItem.DisplayName}") { ClosesMenu = false });
                    List<PopUpBox> InventoryDiscardSubMenu = new List<PopUpBox>();
                    InventoryDiscardSubMenu.Add(new PopUpBox(0, "Discard All", new Action(() => Player.ActivityManager.DropInventoryItem(ii.ModItem, ii.Amount)), $"Discard All {ii.ModItem.DisplayName} ({ii.Amount})"));
                    InventoryDiscardSubMenu.Add(new PopUpBox(1, "Discard One", new Action(() => Player.ActivityManager.DropInventoryItem(ii.ModItem, 1)), $"Discard {ii.ModItem.DisplayName}"));
                    if (ii.Amount > 5)
                    {
                        InventoryDiscardSubMenu.Add(new PopUpBox(2, "Discard Five", new Action(() => Player.ActivityManager.DropInventoryItem(ii.ModItem, 5)), $"Discard {ii.ModItem.DisplayName} ({5})"));
                    }

                    PopUpMenuGroups.Add(new PopUpBoxGroup($"Discard{ii.ModItem.Name}SubMenu", InventoryDiscardSubMenu) { IsChild = true, Group = "Inventory" });
                    PopUpMenuGroups.Add(new PopUpBoxGroup($"{ii.ModItem.Name}SubMenu", InventoryActionSubMenu) { IsChild = true, Group = "Inventory" });
                    ItemID++;
                }
                PopUpMenuGroups.Add(new PopUpBoxGroup($"{mi}{itemSubType}SubMenu", LowLevelList) { IsChild = true, Group = "Inventory" });
                ItemSubTypeID++;
            }
            PopUpMenuGroups.Add(new PopUpBoxGroup($"{mi}SubMenu", MiddleLevelList) { IsChild = true, Group = "Inventory" });
            ItemTypeID++;
        }
        PopUpMenuGroups.Add(new PopUpBoxGroup(InventoryCategoriesSubMenuName, TopLevelList) { IsChild = true, Group = "Inventory" });
    }


    private void AddGestureMenuGroup()
    {
        PopUpMenuGroups.RemoveAll(x => x.Group == "Gesture");
        int CategoryID = 0;
        int GestureID = 0;
        List<PopUpBox> TopLevelList = new List<PopUpBox>();
        foreach (string mi in Gestures.GestureLookups.Where(x => x.IsOnActionWheel).GroupBy(x => x.Category).Select(x => x.Key).Distinct().OrderBy(x => x))
        {
            PopUpBox gestureCategorySubTypeBox = new PopUpBox(CategoryID, mi, $"{mi}SubMenu", $"Open the {mi} Sub Menu") { ClosesMenu = false };
            TopLevelList.Add(gestureCategorySubTypeBox);
            List<PopUpBox> MiddleLevelList = new List<PopUpBox>();
            GestureID = 0;
            foreach (GestureData gd in Gestures.GestureLookups.Where(x => x.Category == mi && x.IsOnActionWheel))
            {
                MiddleLevelList.Add(new PopUpBox(GestureID, rgx.Replace(gd.Name, " "), new Action(() => Player.ActivityManager.Gesture(gd)), rgx.Replace(gd.Name, " ")));
                GestureID++;
            }
            PopUpMenuGroups.Add(new PopUpBoxGroup($"{mi}SubMenu", MiddleLevelList) { IsChild = true, Group = "Gesture" });
            CategoryID++;
        }
        PopUpMenuGroups.Add(new PopUpBoxGroup("Gesture", TopLevelList) { IsChild = true, Group = "Gesture" });
    }

    private void UpdateGroupMenuGroups()
    {
        PopUpMenuGroups.RemoveAll(x => x.Group == "Group");
        int GroupMemberID = 0;
        List<PopUpBox> GroupMembersSubMenu = new List<PopUpBox>();

        GroupMembersSubMenu.Add(new PopUpBox(0, "All Tasking Menu", $"AllTaskingSubMenu", $"Open the All Tasking Sub Menu") { ClosesMenu = false });
        List<PopUpBox> AllTaskingSubMenu = new List<PopUpBox>
        {
            new PopUpBox(0, "Toggle Use Player Car", 
            new Action(() =>
            {
                Player.GroupManager.OnToggleUsePlayerCar();
            }), $"Tell all members to ride in the players car if available, otherwise they will use the assigned vehicle if it exists. Currently {(Player.GroupManager.RideInPlayerVehicleIfPossible ? "Enabled" : "Disabled")}"),
            new PopUpBox(1, "Set Auto",
            new Action(() =>
            {
                Player.GroupManager.OnSetAutoTasking();
            }), $"Set auto tasking. Currently {(!Player.GroupManager.IsSetFollow && !Player.GroupManager.IsSetCombat ? "Enabled" : "Disabled")}"),
            new PopUpBox(2, "Set Combat", new Action(() =>
            {
                Player.GroupManager.OnSetCombatTasking();
            }), $"Set exclusively combat. Currently {(Player.GroupManager.IsSetCombat ? "Enabled" : "Disabled")}"),
            new PopUpBox(3, "Set Non Combat",
            new Action(() =>
            {
                Player.GroupManager.OnSetNonCombatTasking();
            }), $"Set exclusively follow. Currently {(Player.GroupManager.IsSetFollow ? "Enabled" : "Disabled")}"),
            new PopUpBox(4, "Toggle Force Tasking",
            new Action(() =>
            {
                Player.GroupManager.ToggleForceTasking();
            }), $"Set the ped to always do the requested item by force tasking. Currently {(Player.GroupManager.BlockPermanentEvents ? "Enabled" : "Disabled")}"),
            new PopUpBox(5, "Toggle Auto Armed",
            new Action(() =>
            {
                Player.GroupManager.OnSetAutoArmed();
            }), $"Set automatically armed based on the current world state."),
            new PopUpBox(6, "Set Always Armed",
            new Action(() =>
            {
                Player.GroupManager.OnSetAlwaysArmed();
            }), $"Set always armed. Currently {(Player.GroupManager.AlwaysArmed ? "Enabled" : "Disabled")}"),
            new PopUpBox(7, "Set Never Armed",
            new Action(() =>
            {
                Player.GroupManager.OnSetNeverArmed();
            }), $"Set never armed. Currently {(Player.GroupManager.NeverArmed ? "Enabled" : "Disabled")}")
        };
        PopUpMenuGroups.Add(new PopUpBoxGroup($"AllTaskingSubMenu", AllTaskingSubMenu) { IsChild = true, Group = "Group" });

        GroupMembersSubMenu.Add(new PopUpBox(1, "Disband", new Action(() => Player.GroupManager.Disband()), "Disband the group."));
        int startingID = GroupMembersSubMenu.Count();
        foreach (GroupMember mi in Player.GroupManager.CurrentGroupMembers)
        {
            GroupMembersSubMenu.Add(new PopUpBox(GroupMemberID+ startingID, mi.PedExt.Name, $"{mi.PedExt.Name}SubMenu", $"Open the {mi.PedExt.Name} Sub Menu") { ClosesMenu = false });
            List<PopUpBox> GroupMemberSubMenu = new List<PopUpBox>();
            List<PopUpBox> GroupMemberTaskingSubMenu = new List<PopUpBox>();
            GroupMemberSubMenu.Add(new PopUpBox(0, "Give Weapon", new Action(() => mi.GiveCurrentWeapon()), "Give Current Weapon"));
            GroupMemberSubMenu.Add(new PopUpBox(1, "Remove Member", new Action(() => Player.GroupManager.Remove(mi.PedExt)), "Remove the Member"));
            GroupMemberSubMenu.Add(new PopUpBox(2, "Tasking Menu", $"{mi.PedExt.Name}TaskingSubMenu", $"Open the Tasking Sub Menu") { ClosesMenu = false });
            //GroupMemberSubMenu.Add(new PopUpBox(2, "Rest Tasks", new Action(() => Player.GroupManager.ResetStatus(mi.PedExt, true)), "Reset the member's tasks"));
            //GroupMemberSubMenu.Add(new PopUpBox(3, "Set Follow", new Action(() => Player.GroupManager.SetFollow(mi.PedExt)), "Tell the member to escort you around. Will use their own vehicle if it exists and is close"));
            //GroupMemberTaskingSubMenu.Add(new PopUpBox(0, "Set Violent", new Action(() => mi.SetViolent()), "Set the group member to always fight police and other gang members"));
            //GroupMemberTaskingSubMenu.Add(new PopUpBox(1, "Set Passive", new Action(() => mi.SetPassive()), "Set the group member to never fight police and other gang members"));
            GroupMemberTaskingSubMenu.Add(new PopUpBox(0, "Set Specialist", new Action(() => mi.SetSpecialist()), "Set the group member to be better at combat"));
            PopUpMenuGroups.Add(new PopUpBoxGroup($"{mi.PedExt.Name}TaskingSubMenu", GroupMemberTaskingSubMenu) { IsChild = true, Group = "Group" });
            PopUpMenuGroups.Add(new PopUpBoxGroup($"{mi.PedExt.Name}SubMenu", GroupMemberSubMenu) { IsChild = true, Group = "Group" });
            GroupMemberID++;
        }
        PopUpMenuGroups.Add(new PopUpBoxGroup(GroupMemberSubMenuName, GroupMembersSubMenu) { IsChild = true, Group = "Group" });
    }
    private void UpdateAffiliationMenuGroups()
    {
        PopUpMenuGroups.RemoveAll(x => x.Group == "Affiliation");
        List<PopUpBox> AffiliationSubMenu = new List<PopUpBox>();
        if(Player.IsCop)
        {
            AffiliationSubMenu.Add(new PopUpBox(0, "Set Taskable", new Action(() => Player.ToggleCopTaskable()), "Set As Taskable"));
        }
        PopUpMenuGroups.Add(new PopUpBoxGroup(AffiliationSubMenuName, AffiliationSubMenu) { IsChild = true, Group = "Affiliation" });
    }
    private void FindClosestPositionMap()
    {
        if (Settings.SettingsManager.ActionWheelSettings.UseNewClosest)
        {
            float XPercent = NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, (int)GameControl.CursorX);
            float YPercent = NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, (int)GameControl.CursorY);
            if(XPercent == 0.0f)
            {
                XPercent = prevXPercent;
            }
            prevXPercent = XPercent;//will randomly get set to 0.0 when you click for a single frame, when inside a vehicle. Need a better fix
            float ClosestDistance = 1.0f;
            ClosestPositionMap = null;
            foreach (PositionMap positionMap2 in PositionMaps)
            {
                float distanceToMouse = (float)Math.Sqrt(Math.Pow(positionMap2.PositionX - XPercent, 2) + Math.Pow(positionMap2.PositionY - YPercent, 2));
                if (distanceToMouse <= ClosestDistance && distanceToMouse <= Settings.SettingsManager.ActionWheelSettings.SelectedItemMinimumDistance)// 0.15f)
                {
                    ClosestDistance = distanceToMouse;
                    ClosestPositionMap = positionMap2;
                }
            }
            //Game.DisplaySubtitle($"XPercent {XPercent} YPercent {YPercent} ClosestDistance {ClosestDistance} ClosestPositionMap {ClosestPositionMap?.Display} {(float)Game.Resolution.Width}X{(float)Game.Resolution.Height}");  
        }
        else
        {
            MouseState mouseState = Game.GetMouseState();
            if (mouseState != null)
            {
                float XPercent = (float)mouseState.X / (float)Game.Resolution.Width;
                float YPercent = (float)mouseState.Y / (float)Game.Resolution.Height;
                float ClosestDistance = 1.0f;
                ClosestPositionMap = null;
                foreach (PositionMap positionMap2 in PositionMaps)
                {
                    float distanceToMouse = (float)Math.Sqrt(Math.Pow(positionMap2.PositionX - XPercent, 2) + Math.Pow(positionMap2.PositionY - YPercent, 2));
                    if (distanceToMouse <= ClosestDistance && distanceToMouse <= Settings.SettingsManager.ActionWheelSettings.SelectedItemMinimumDistance)// 0.15f)
                    {
                        ClosestDistance = distanceToMouse;
                        ClosestPositionMap = positionMap2;
                    }
                }
                //Game.DisplaySubtitle($" X:{(float)mouseState.X} Y:{(float)mouseState.Y} XPercent {XPercent} YPercent {YPercent} ClosestDistance {ClosestDistance} ClosestPositionMap {ClosestPositionMap?.Display} {(float)Game.Resolution.Width}X{(float)Game.Resolution.Height}");
            }
        }
    }
    private void UpdateSelection()
    {
        if(!UI.IsPressingActionWheelButton && !HasStoppedPressingDisplayKey)
        {
            //EntryPoint.WriteToConsoleTestLong("HAS STOPPED PRESSING ACTION WHEEL SHOW");
            HasStoppedPressingDisplayKey = true;
        }
        if (ClosestPositionMap != null)
        {
            PopUpBox popUpBox = GetCurrentPopUpBox(ClosestPositionMap.ID);
            if(popUpBox == null)
            {
                popUpBox = AlwaysShownItems.FirstOrDefault(x => x.ID == ClosestPositionMap.ID);
            }
            if (popUpBox == null)
            {
                popUpBox = ButtonPromptItems.FirstOrDefault(x => x.ID == ClosestPositionMap.ID);
            }
            if (popUpBox != null && popUpBox.IsCurrentlyValid())
            {
                if ((popUpBox.Action != null || popUpBox.ChildMenuID != ""))
                {
                    //EntryPoint.WriteToConsole($"ACTION WHEEL POP UP BOX IS VALID {popUpBox.Description} ChildMenuID: {popUpBox.ChildMenuID} HasAction:{popUpBox.Action != null}");
                    if (PressedSelect())// && Game.GameTime - GameTimeLastClicked >= 50)//or is disbaled control just released.....//&& Environment.TickCount - GameTimeLastClicked >= 100)//or is disbaled control just released.....
                    {
                       // EntryPoint.WriteToConsole($"ACTION WHEEL PRESSED SELECT 2");
                        if (popUpBox.ClosesMenu)
                        {
                            CloseMenu();
                            HasRanItem = true;
                        }
                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "CONTINUE", "HUD_FRONTEND_DEFAULT_SOUNDSET", 0);
                        if (popUpBox.Action != null)
                        {
                            popUpBox.Action();
                            //EntryPoint.WriteToConsole($"ACTION WHEEL PRESSED SELECT ACTION RAN");
                        }
                        else if (popUpBox.ChildMenuID != "")
                        {
                            PrevPopUpBoxGroupID = CurrentPopUpBoxGroupID;
                            CurrentPopUpBoxGroupID = popUpBox.ChildMenuID;
                            CurrentPage = 0;
                            TotalPages = 0;

                            int max = 0;
                            if (MenuStack.Any())
                            {
                                max = MenuStack.Max(x => x.Item1);
                            }
                            MenuStack.Add(new Tuple<int, string>(max + 1, PrevPopUpBoxGroupID));
                           // EntryPoint.WriteToConsole($"ACTION WHEEL: ToMenu:{popUpBox?.ChildMenuID} PrevMenu:{PrevPopUpBoxGroupID}");
                        }
                        //GameTimeLastClicked = Game.GameTime;//Environment.TickCount;
                    }
                }
                SelectedMenuMap = popUpBox;
            }
            else
            {
                SelectedMenuMap = null;
            }
        }
        else
        {
            SelectedMenuMap = null;
        }
        if(SelectedMenuMap != null)
        {
            DisplayTextBoxOnScreen(SelectedMenuMap.Description, 0.5f, 0.5f, Settings.SettingsManager.ActionWheelSettings.TextScale, Color.FromName(Settings.SettingsManager.ActionWheelSettings.TextColor), Settings.SettingsManager.ActionWheelSettings.TextFont, 255, true, Color.Black);
        }
        if (PrevSelectedMenuMap?.Display != SelectedMenuMap?.Display)
        {
            if(SelectedMenuMap != null && Game.GameTime - GameTimeLastPressedSubMenu >= 50)
            {
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "CONTINUE", "HUD_FRONTEND_DEFAULT_SOUNDSET", 0);
            }
            //EntryPoint.WriteToConsole($"ACTION WHEEL PrevSelectedMenuMap:{PrevSelectedMenuMap?.ChildMenuID} SelectedMenuMap {SelectedMenuMap?.ChildMenuID}");
            PrevSelectedMenuMap = SelectedMenuMap;
        }

        bool isPressingAim = PressedReturn();
        if (!IsCurrentPopUpBoxGroupDefault && isPressingAim)
        {
            if(!MenuStack.Any())
            {
                UpdateDefaultMapping(true);
            }
            else
            {
                string menuID = MenuStack.OrderByDescending(x => x.Item1).FirstOrDefault()?.Item2;
                if(string.IsNullOrEmpty(menuID))
                {
                    UpdateDefaultMapping(true);
                }
                else
                {
                    MenuStack.RemoveAll(x=> x.Item2 == menuID);
                    CurrentPopUpBoxGroupID = menuID;
                }
            }
            CurrentPage = 0;
            TotalPages = 0;
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", 0);
            GameTimeLastPressedSubMenu = Game.GameTime;
        }
        else if(IsCurrentPopUpBoxGroupDefault && !Settings.SettingsManager.ActionWheelSettings.RequireButtonHold && isPressingAim)
        {
            //EntryPoint.WriteToConsoleTestLong($"ACTION WHEEL PRESSED BACK, CLOSING MENU");
            CloseMenu();
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", 0);
        }
        else if (!Settings.SettingsManager.ActionWheelSettings.RequireButtonHold && UI.IsPressingActionWheelButton && HasStoppedPressingDisplayKey)
        {
            CloseMenu();
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", 0);
        }
    }

    private bool PressedSelect()
    {
       // EntryPoint.WriteToConsole($"CHECKING PRESSED SELECT");
        //return Game.IsControlJustReleased(0, GameControl.Attack) || NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 24);

        //if (Game.IsControlJustReleased(0, GameControl.Attack))
        //{
        //    EntryPoint.WriteToConsole($"GameControl.Attack:{Game.IsControlJustReleased(0, GameControl.Attack)}");
        //}
        //if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 24))
        //{
        //    EntryPoint.WriteToConsole($"GameControl.Attack (Disabled):{NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 24)}");
        //}
        //if (Game.IsControlJustReleased(0, GameControl.VehicleAttack))
        //{
        //    EntryPoint.WriteToConsole($"GameControl.VehicleAttack:{Game.IsControlJustReleased(0, GameControl.VehicleAttack)}");
        //}
        //if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 69))
        //{
        //    EntryPoint.WriteToConsole($"GameControl.VehicleAttack (Disabled):{NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 69)}");
        //}



        //if (Game.IsControlJustPressed(0, GameControl.Aim))
        //{
        //    EntryPoint.WriteToConsole($"GameControl.Aim:{Game.IsControlJustPressed(0, GameControl.Aim)}");
        //}
        //if (NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 25))
        //{
        //    EntryPoint.WriteToConsole($"GameControl.Aim (Disabled):{NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 25)}");
        //}
        //if (Game.IsControlJustPressed(0, GameControl.VehicleAim))
        //{
        //    EntryPoint.WriteToConsole($"GameControl.VehicleAim:{Game.IsControlJustPressed(0, GameControl.VehicleAim)}");
        //}
        //if (NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 68))
        //{
        //    EntryPoint.WriteToConsole($"GameControl.VehicleAim (Disabled):{NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 68)}");
        //}




        if (Player.IsInVehicle)
        {
            //if(Game.IsControlJustReleased(0, GameControl.VehicleAttack) || NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 69) || Game.IsControlJustReleased(0, GameControl.Attack) || NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 24))
            //{
            //    EntryPoint.WriteToConsole("PRESSED SELECT 1");
            //}
            return Game.IsControlJustReleased(0, GameControl.VehicleAttack) || NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 69) || Game.IsControlJustReleased(0,GameControl.VehicleFlyMouseControlOverride);
        }
        else
        {
            //if (Game.IsControlJustReleased(0, GameControl.Attack) || NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 24))
            //{
            //    EntryPoint.WriteToConsole("PRESSED SELECT 2");
            //}
            return Game.IsControlJustReleased(0, GameControl.Attack) || NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 24);
        }
    }
    private bool PressedReturn()
    {
        //return Game.IsControlJustPressed(0, GameControl.Aim) || NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 25);
        if (Player.IsInVehicle)
        {
            return Game.IsControlJustPressed(0, GameControl.VehicleAim) || NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 68);
        }
        else
        {
            return Game.IsControlJustPressed(0, GameControl.Aim) || NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 25);
        }
    }

    private void ProcessControllerInput()
    {
        if (!NativeFunction.Natives.IS_USING_KEYBOARD_AND_MOUSE<bool>(2))
        {
            float XChange = NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, (int)GameControl.ScriptRightAxisX);
            float YChange = NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, (int)GameControl.ScriptRightAxisY);
            float xAmount = XChange * Settings.SettingsManager.ActionWheelSettings.ControllerCursorScale; ;// Game.FrameTime * XChange * Settings.SettingsManager.ActionWheelSettings.ControllerCursorScale;
            float yAmount = YChange * Settings.SettingsManager.ActionWheelSettings.ControllerCursorScale; ;// Game.FrameTime * YChange * Settings.SettingsManager.ActionWheelSettings.ControllerCursorScale;
            CursorXPos += xAmount;
            CursorYPos += yAmount;
            if (CursorXPos > 1.0f)
            {
                CursorXPos = 1.0f;
            }
            if (CursorXPos < 0.0f)
            {
                CursorXPos = 0.0f;
            }
            if (CursorYPos > 1.0f)
            {
                CursorYPos = 1.0f;
            }
            if (CursorYPos < 0.0f)
            {
                CursorYPos = 0.0f;
            }
            NativeFunction.Natives.SET_CURSOR_POSITION(CursorXPos, CursorYPos);
        }
    }
    private void DisableControls()
    {
        Game.DisableControlAction(0, GameControl.LookLeftRight, true);
        Game.DisableControlAction(0, GameControl.LookUpDown, true);
        Game.DisableControlAction(0, GameControl.Attack, true);
        Game.DisableControlAction(0, GameControl.Attack2, true);
        Game.DisableControlAction(0, GameControl.MeleeAttack1, true);
        Game.DisableControlAction(0, GameControl.MeleeAttack2, true);
        Game.DisableControlAction(0, GameControl.Aim, true);
        Game.DisableControlAction(0, GameControl.VehicleAim, true);
        Game.DisableControlAction(0, GameControl.AccurateAim, true);
        Game.DisableControlAction(0, GameControl.VehiclePassengerAim, true);
        Game.DisableControlAction(0, GameControl.VehicleAttack, true);

        Game.DisableControlAction(0, GameControl.VehicleFlyMouseControlOverride, true);


        Game.DisableControlAction(0, GameControl.VehicleAttack, true);
        Game.DisableControlAction(0, GameControl.VehicleAttack2, true);


        Game.DisableControlAction(0, GameControl.VehiclePassengerAim, true);
        Game.DisableControlAction(0, GameControl.VehiclePassengerAttack, true);
    }
    private void UpdateDefaultMapping(bool force)
    {
        if (CurrentPopUpBoxGroupID == DefaultInVehicleName || CurrentPopUpBoxGroupID == DefaultOnFootGroupName || force)
        {
            if (Player.IsInVehicle && Player.CurrentVehicle != null)
            {
                CurrentPopUpBoxGroupID = DefaultInVehicleName;
            }
            else
            {
                CurrentPopUpBoxGroupID = DefaultOnFootGroupName;
            }
        }
    }
    private void DrawItems()
    {
        if (Player.IsAliveAndFree)
        {
            DrawCurrentMenuMap();
            DrawButtonPrompts();
        }
        DrawMessages();
        DrawSpeedLimit();
        DrawAffiliation();
        DrawMenuShortcuts();
    }
    private void DrawButtonPrompts()
    {
        ButtonPromptItems.Clear();
        int id = 500;
        int added = 1;
        foreach (ButtonPrompt bp in Player.ButtonPrompts.Prompts.OrderBy(x => x.Order))
        {
            PopUpBox toAdd = new PopUpBox(id, bp.Text, new Action(() => bp.SetFakePressed()), "Press Prompt");
            ButtonPromptItems.Add(toAdd);
            id++;

            DrawPopUpBox(toAdd, Settings.SettingsManager.ActionWheelSettings.ButtonPromptXStart, Settings.SettingsManager.ActionWheelSettings.ButtonPromptYStart + (added * 0.05f));
            added++;
        }
    }
    private void DrawMenuShortcuts()
    {
        if (Player.IsDead)
        {
            MainMenuMenuMap.Display = "Death Menu";
        }
        else if (Player.IsBusted)
        {
            MainMenuMenuMap.Display = "Busted Menu";
        }
        else
        {
            MainMenuMenuMap.Display = "Main Menu";
        }
        DrawPopUpBox(MainMenuMenuMap, Settings.SettingsManager.ActionWheelSettings.MainMenuCenterX, Settings.SettingsManager.ActionWheelSettings.MainMenuCenterY);
        DrawPopUpBox(DebugMenuMenuMap, Settings.SettingsManager.ActionWheelSettings.DebugMenuCenterX, Settings.SettingsManager.ActionWheelSettings.DebugMenuCenterY);
        DrawPopUpBox(BurnerPhoneMenuMap, Settings.SettingsManager.ActionWheelSettings.BurnerPhoneOpenCenterX, Settings.SettingsManager.ActionWheelSettings.BurnerPhoneOpenCenterY);
    }
    private void DrawCurrentMenuMap()
    {
        if (Settings.SettingsManager.ActionWheelSettings.ShowCursor)
        {
            NativeFunction.Natives.xAAE7CE1D63167423();//_SET_MOUSE_CURSOR_ACTIVE_THIS_FRAME
        }
        ConsistencyScale = (float)Game.Resolution.Height / (float)Game.Resolution.Width; //(float)Game.Resolution.Width / 3840f;
        PositionMaps.Clear();
        IconsToDraw.Clear();
        bool DrawPages = false;
        int ID = 0;
        TotalPages = 0;



        List<PopUpBox> CurrentMenuMap = GetCurrentPopUpBoxes();


        if (CurrentMenuMap != null)
        {
            int TotalItems = CurrentMenuMap.Count();
            if (TotalItems > Settings.SettingsManager.ActionWheelSettings.ItemsPerPage)
            {
                float shrinkAmount = (TotalItems - 9) * Settings.SettingsManager.ActionWheelSettings.ItemScaleExtraItemScalar;
                excessiveItemScaler = 1.0f - shrinkAmount;
                float centershrinkAmount = (TotalItems - 9) * Settings.SettingsManager.ActionWheelSettings.ItemDistanceFromCenterExtraItemScalar;
                excessiveCenterScaler = 1.0f - centershrinkAmount;
            }
            else
            {
                excessiveItemScaler = 1.0f;
                excessiveCenterScaler = 1.0f;
            }
            if (TotalItems > 0)
            {
                int startingItem = 0;
                int ItemsToDisplay = TotalItems;
                if (TotalItems > Settings.SettingsManager.ActionWheelSettings.ItemsPerPage)
                {
                    excessiveItemScaler = 1.0f;
                    excessiveCenterScaler = 1.0f;
                    DrawPages = true;
                    if (CurrentPage * Settings.SettingsManager.ActionWheelSettings.ItemsPerPage > TotalItems)
                    {
                        CurrentPage = 0;
                    }
                    TotalPages = (int)Math.Ceiling(TotalItems / (float)Settings.SettingsManager.ActionWheelSettings.ItemsPerPage);
                    if (CurrentPage < 0)
                    {
                        CurrentPage = TotalPages - 1;
                    }
                    startingItem = CurrentPage * Settings.SettingsManager.ActionWheelSettings.ItemsPerPage;
                    if (TotalItems - startingItem > Settings.SettingsManager.ActionWheelSettings.ItemsPerPage)
                    {
                        ItemsToDisplay = Settings.SettingsManager.ActionWheelSettings.ItemsPerPage;
                    }
                    else
                    {
                        ItemsToDisplay = TotalItems - startingItem;
                    }

                }
                double angle = 360.0 / ItemsToDisplay * Math.PI / 180.0;
                for (int i = 0; i < ItemsToDisplay; i++)
                {
                    DrawSingle(startingItem + i, Settings.SettingsManager.ActionWheelSettings.ItemCenterX + (float)Math.Cos((angle * i) - 1.5708) * Settings.SettingsManager.ActionWheelSettings.ItemDistanceFromCenter * ConsistencyScale * excessiveCenterScaler, Settings.SettingsManager.ActionWheelSettings.ItemCenterY + (float)Math.Sin((angle * i) - 1.5708) * Settings.SettingsManager.ActionWheelSettings.ItemDistanceFromCenter * excessiveCenterScaler);
                    ID++;
                }
                if (DrawPages)
                {
                    DrawPopUpBox(PrevPageMenuMap, Settings.SettingsManager.ActionWheelSettings.PrevPageCenterX, Settings.SettingsManager.ActionWheelSettings.PrevPageCenterY);

                    NextPageMenuMap.Display = $"Next Page {CurrentPage + 1}/{TotalPages}";
                    DrawPopUpBox(NextPageMenuMap, Settings.SettingsManager.ActionWheelSettings.NextPageCenterX, Settings.SettingsManager.ActionWheelSettings.NextPageCenterY);
                }
            }
        }
    }
    private void DrawMessages()
    {
        List<Tuple<string, DateTime>> MessageTimes = new List<Tuple<string, DateTime>>();
        MessageTimes.AddRange(Player.CellPhone.PhoneResponseList.OrderByDescending(x => x.TimeReceived).Take(Settings.SettingsManager.ActionWheelSettings.MessagesToShow).Select(x => new Tuple<string, DateTime>(x.ContactName, x.TimeReceived)));
        MessageTimes.AddRange(Player.CellPhone.TextList.OrderByDescending(x => x.TimeReceived).Take(Settings.SettingsManager.ActionWheelSettings.MessagesToShow).Select(x => new Tuple<string, DateTime>(x.ContactName, x.TimeReceived)));
        int MessagesDisplayed = 0;
        float YMessageSpacing = Settings.SettingsManager.ActionWheelSettings.MessageBodySpacingY;
        float YHeaderSpacing = Settings.SettingsManager.ActionWheelSettings.MessageHeaderSpacingY;// 0.02f;
        foreach (Tuple<string, DateTime> dateTime in MessageTimes.OrderByDescending(x => x.Item2).Take(Settings.SettingsManager.ActionWheelSettings.MessagesToShow))
        {
            PhoneResponse phoneResponse = Player.CellPhone.PhoneResponseList.Where(x => x.TimeReceived == dateTime.Item2 && x.ContactName == dateTime.Item1).FirstOrDefault();
            if (phoneResponse != null)
            {
                //DisplayTextBoxOnScreen("~h~" + phoneResponse.ContactName + " - " + phoneResponse.TimeReceived.ToString("HH:mm"), Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionX, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionY + (YMessageSpacing * MessagesDisplayed), Settings.SettingsManager.ActionWheelSettings.MessageScale, Color.FromName(Settings.SettingsManager.ActionWheelSettings.MessageTextColor), Settings.SettingsManager.ActionWheelSettings.MessageFont, 255, true, Color.Black);
                DisplayTextBoxOnScreen("~h~" + phoneResponse.ContactName + "~h~ - " + phoneResponse.TimeReceived.ToString("HH:mm") +"~n~~s~" + phoneResponse.Message, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionX, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionY + (YMessageSpacing * MessagesDisplayed), Settings.SettingsManager.ActionWheelSettings.MessageScale, Color.FromName(Settings.SettingsManager.ActionWheelSettings.MessageTextColor), Settings.SettingsManager.ActionWheelSettings.MessageFont, 255, true, Color.Black); ;
                MessagesDisplayed++;
            }
            PhoneText phoneText = Player.CellPhone.TextList.Where(x => x.TimeReceived == dateTime.Item2 && x.ContactName == dateTime.Item1).FirstOrDefault();
            if (phoneText != null)
            {
                //DisplayTextBoxOnScreen("~h~" + phoneText.ContactName + " - " + phoneText.TimeReceived.ToString("HH:mm"), Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionX, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionY + (YMessageSpacing * MessagesDisplayed), Settings.SettingsManager.ActionWheelSettings.MessageScale, Color.FromName(Settings.SettingsManager.ActionWheelSettings.MessageTextColor), Settings.SettingsManager.ActionWheelSettings.MessageFont, 255, true, Color.Black);
                DisplayTextBoxOnScreen("~h~" + phoneText.ContactName + "~h~ - " + phoneText.TimeReceived.ToString("HH:mm") + "~n~~s~" + phoneText.Message, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionX, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionY + (YMessageSpacing * MessagesDisplayed), Settings.SettingsManager.ActionWheelSettings.MessageScale, Color.FromName(Settings.SettingsManager.ActionWheelSettings.MessageTextColor), Settings.SettingsManager.ActionWheelSettings.MessageFont, 255, true, Color.Black); ;
                MessagesDisplayed++;
            }
        }
    }
    private void DrawAffiliation()
    {
        bool display = false;
        string toDisplay = "Affiliation: ~n~";
        if (Player.AssignedAgency != null)
        {
            display = true;
            toDisplay += Player.AssignedAgency.ColorInitials;
        }
        else if (Player.CurrentGang != null)
        {
            display = true;
            toDisplay += Player.CurrentGang.ColorInitials;
        }
        if(display)
        {
            DisplayTextBoxOnScreen(toDisplay, Settings.SettingsManager.ActionWheelSettings.AffiliationCenterX, Settings.SettingsManager.ActionWheelSettings.AffiliationCenterY, Settings.SettingsManager.ActionWheelSettings.TextScale, Color.White, Settings.SettingsManager.ActionWheelSettings.TextFont, 255, false, Color.FromName(Settings.SettingsManager.ActionWheelSettings.ItemColor));
        }
    }
    private void DrawSpeedLimit()
    {
        SpeedLimitConsistencyScale = (float)Game.Resolution.Width / 2160f;
        SpeedLimitScale = Settings.SettingsManager.ActionWheelSettings.SpeedLimitIconScale * SpeedLimitConsistencyScale;

        if (SpeedLimitToDraw != null)
        {
            SpeedLimitPosX = (Game.Resolution.Height - (SpeedLimitToDraw.Size.Height * SpeedLimitScale)) * Settings.SettingsManager.ActionWheelSettings.SpeedLimitIconX;
            SpeedLimitPosY = (Game.Resolution.Width - (SpeedLimitToDraw.Size.Width * SpeedLimitScale)) * Settings.SettingsManager.ActionWheelSettings.SpeedLimitIconY;
        }

        if (Settings.SettingsManager.ActionWheelSettings.ShowSpeedLimitIcon && Player.IsInVehicle && Player.CurrentLocation.CurrentStreet != null)
        {
            float speedLimit = 60f;
            if (Settings.SettingsManager.LSRHUDSettings.SpeedDisplayUnits == "MPH")
            {
                speedLimit = Player.CurrentLocation.CurrentStreet.SpeedLimitMPH;
            }
            else if (Settings.SettingsManager.LSRHUDSettings.SpeedDisplayUnits == "KM/H")
            {
                speedLimit = Player.CurrentLocation.CurrentStreet.SpeedLimitKMH;
            }
            if (speedLimit <= 10f)
            {
                SpeedLimitToDraw = Sign10;
            }
            else if (speedLimit <= 15f)
            {
                SpeedLimitToDraw = Sign15;
            }
            else if (speedLimit <= 20f)
            {
                SpeedLimitToDraw = Sign20;
            }
            else if (speedLimit <= 25f)
            {
                SpeedLimitToDraw = Sign25;
            }
            else if (speedLimit <= 30f)
            {
                SpeedLimitToDraw = Sign30;
            }
            else if (speedLimit <= 35f)
            {
                SpeedLimitToDraw = Sign35;
            }
            else if (speedLimit <= 40f)
            {
                SpeedLimitToDraw = Sign40;
            }
            else if (speedLimit <= 45f)
            {
                SpeedLimitToDraw = Sign45;
            }
            else if (speedLimit <= 50f)
            {
                SpeedLimitToDraw = Sign50;
            }
            else if (speedLimit <= 55f)
            {
                SpeedLimitToDraw = Sign55;
            }
            else if (speedLimit <= 60f)
            {
                SpeedLimitToDraw = Sign60;
            }
            else if (speedLimit <= 65f)
            {
                SpeedLimitToDraw = Sign65;
            }
            else if (speedLimit <= 70f)
            {
                SpeedLimitToDraw = Sign70;
            }
            else if (speedLimit <= 75f)
            {
                SpeedLimitToDraw = Sign75;
            }
            else if (speedLimit <= 80f)
            {
                SpeedLimitToDraw = Sign80;
            }
            else
            {
                SpeedLimitToDraw = null;
            }
        }
        else
        {
            SpeedLimitToDraw = null;
        }
    }
    private void DrawSingle(int ID, float CurrentPositionX, float CurrentPositionY)
    {
        Color overrideColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.ItemColor);
        PopUpBox popUpMenuMap = GetCurrentPopUpBox(ID);
        string display = ID.ToString();
        bool isSelected = false;
        Color textColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.TextColor);
        bool hasIcon = false;
        bool isValid = true;
        if (popUpMenuMap != null)
        {
            display = popUpMenuMap.Display;
            if (SelectedMenuMap != null && SelectedMenuMap.ID == popUpMenuMap.ID && SelectedMenuMap.IsCurrentlyValid())//is the selected item
            {
                overrideColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.SelectedItemColor);//Color.FromArgb(72, 133, 164, 100);
                isSelected = true;
            }
            if (!popUpMenuMap.IsCurrentlyValid())
            {
                textColor = Color.Gray;
                isValid = false;
            }
            if (popUpMenuMap.HasIcon)
            {
                hasIcon = true;
                Texture texture;
                if (!isValid)
                {
                    texture = popUpMenuMap.IconInvalid;
                }
                else if (isSelected)
                {
                    texture = popUpMenuMap.IconSelected;
                }
                else
                {
                    texture = popUpMenuMap.IconDefault;
                }

                float width = texture.Size.Width * 1.25f * Settings.SettingsManager.ActionWheelSettings.DebugIconScale;
                float height = texture.Size.Height * 1.25f * Settings.SettingsManager.ActionWheelSettings.DebugIconScale;

                float posX = Game.Resolution.Width * (CurrentPositionX + Settings.SettingsManager.ActionWheelSettings.DebugIconX) - width / 2;
                float posY = Game.Resolution.Height * (CurrentPositionY + Settings.SettingsManager.ActionWheelSettings.DebugIconY) - height / 2;

                IconsToDraw.Add(new DrawableIcon(texture, new RectangleF(posX, posY, width, height)));
            }
        }
        if (!hasIcon || !Settings.SettingsManager.ActionWheelSettings.ShowOnlyIcon)
        {
            DisplayTextBoxOnScreen(display, CurrentPositionX, CurrentPositionY, Settings.SettingsManager.ActionWheelSettings.TextScale * excessiveItemScaler, textColor, Settings.SettingsManager.ActionWheelSettings.TextFont, 255, true, overrideColor);
        }
        PositionMaps.Add(new PositionMap(ID, display, CurrentPositionX, CurrentPositionY));
    }
    private void DrawPopUpBox(PopUpBox popUpMenuMap, float CurrentPositionX, float CurrentPositionY)
    {
        Color overrideColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.ItemColor);
        string display = "Item";
        Color textColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.TextColor);
        if (popUpMenuMap != null)
        {
            display = popUpMenuMap.Display;

            if (SelectedMenuMap != null && SelectedMenuMap.ID == popUpMenuMap.ID && SelectedMenuMap.IsCurrentlyValid())//is the selected item
            {
                overrideColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.SelectedItemColor);//Color.FromArgb(72, 133, 164, 100);
            }
            if (!popUpMenuMap.IsCurrentlyValid())
            {
                textColor = Color.Gray;
            }
        }
        DisplayTextBoxOnScreen(display, CurrentPositionX, CurrentPositionY, Settings.SettingsManager.ActionWheelSettings.TextScale * excessiveItemScaler, textColor, Settings.SettingsManager.ActionWheelSettings.TextFont, 255, true, overrideColor);
        PositionMaps.Add(new PositionMap(popUpMenuMap.ID, display, CurrentPositionX, CurrentPositionY));
    }
    private void DrawSprites(object sender, GraphicsEventArgs args)
    {
        try
        {
            if (Settings.SettingsManager.ActionWheelSettings.ShowSpeedLimitIcon && IsActive && SpeedLimitToDraw != null)
            {
                if (SpeedLimitToDraw != null && SpeedLimitToDraw.Size != null)
                {
                    args.Graphics.DrawTexture(SpeedLimitToDraw, new RectangleF(SpeedLimitPosY, SpeedLimitPosX, SpeedLimitToDraw.Size.Width * SpeedLimitScale, SpeedLimitToDraw.Size.Height * SpeedLimitScale));
                }
            }
            if(Settings.SettingsManager.ActionWheelSettings.ShowIcons && IsActive && IconsToDraw != null) 
            {
                foreach(DrawableIcon obj in IconsToDraw.ToList())
                {
                    args.Graphics.DrawTexture(obj.Icon, obj.Rectangle);// new RectangleF(SpeedLimitPosY, SpeedLimitPosX, SpeedLimitToDraw.Size.Width * SpeedLimitScale, SpeedLimitToDraw.Size.Height * SpeedLimitScale));
                }
            }
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI: Draw ERROR {ex.Message} {ex.StackTrace} ", 0);
        }
    }
    private void DisplayTextBoxOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, int alpha, bool addBackground, Color BackGroundColor)
    {
        try
        {
            if (TextToShow == "" || alpha == 0)
            {
                return;
            }

            float Height = GetTextBoxLines(TextToShow, Scale, X, Y, Font) * Scale * Settings.SettingsManager.ActionWheelSettings.TextBoxScale;// ((TextToShow.Length + 90) / 90) * Settings.SettingsManager.ActionWheelSettings.TextScale * 0.3f;
            float Width = 0.0f;
            NativeFunction.Natives.SET_TEXT_FONT((int)Font);
            NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
            NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);
            NativeFunction.Natives.SetTextJustification((int)GTATextJustification.Center);
            NativeFunction.Natives.SetTextDropshadow(10, 255, 0, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);
            NativeFunction.Natives.SET_TEXT_WRAP(X, X + 0.15f);
            NativeFunction.Natives.x25fbb336df1804cb("jamyfafi"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
            NativeHelper.AddLongString(TextToShow);
            NativeFunction.Natives.xCD015E5BB0D96A57(X, Y - (Height / 2.0f));
            if (addBackground)
            {
                Width = GetTextBoxWidth(TextToShow, Scale, X, Font);
                if (Width >= 0.15f)
                {
                    Width = 0.15f;
                }
                NativeFunction.Natives.DRAW_RECT(X, Y, Width + 0.01f, Height + 0.01f, BackGroundColor.R, BackGroundColor.G, BackGroundColor.B, 100, false);
            }
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
        }
    }
    private float GetTextBoxWidth(string TextToShow, float Scale, float X, GTAFont Font)
    {
        NativeFunction.Natives.x54CE8AC98E120CAB("jamyfafi");
        NativeFunction.Natives.SET_TEXT_FONT((int)Font);
        NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
        NativeFunction.Natives.SET_TEXT_WRAP(X, X + 0.15f);
        NativeHelper.AddLongString(TextToShow);
        return NativeFunction.Natives.x85F061DA64ED2F67<float>(true);
    }
    private float GetTextBoxLines(string TextToShow, float Scale, float X, float Y, GTAFont Font)
    {
        NativeFunction.Natives.x521FB041D93DD0E4("jamyfafi");
        NativeFunction.Natives.SET_TEXT_FONT((int)Font);
        NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
        NativeFunction.Natives.SET_TEXT_WRAP(X, X + 0.15f);
        NativeHelper.AddLongString(TextToShow);
        return NativeFunction.Natives.x9040DFB09BE75706<int>(X, Y);
    }
    private void SetupBoxes()
    {
        PopUpBox InfoBox = new PopUpBox(0, "Info", "InfoSubMenu", "Open Player Info Sub Menu")
        {
            ClosesMenu = false,
            IconNameDefault = "info_white.png",
            IconNameSelected = "info_red.png",
            IconNameInvalid = "info_black.png"
        };
        PopUpBox ActionsBox = new PopUpBox(1, "Actions", ActionsSubMenuName, "Open Actions Sub Menu")
        {
            ClosesMenu = false,
            IconNameDefault = "actions_white.png",
            IconNameSelected = "actions_red.png",
            IconNameInvalid = "actions_black.png"
        };
        PopUpBox WeaponsBox = new PopUpBox(2, "Weapons", WeaponsSubMenuName, "Open Weapons Sub Menu")
        {
            ClosesMenu = false,
            IconNameDefault = "weapons_white.png",
            IconNameSelected = "weapons_red.png",
            IconNameInvalid = "weapons_black.png",
            IsCurrentlyValid = new Func<bool>(() => Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon.Category != WeaponCategory.Melee)
        };
        PopUpBox InventoryBox = new PopUpBox(4, "Inventory", InventoryCategoriesSubMenuName, "Open Inventory Sub Menu")
        {
            ClosesMenu = false,
            IconNameDefault = "inventory_white.png",
            IconNameSelected = "inventory_red.png",
            IconNameInvalid = "inventory_black.png",
        };
        PopUpBox GroupBox = new PopUpBox(5, "Group", GroupMemberSubMenuName, "Open Group Sub Menu")
        {
            ClosesMenu = false,
            IconNameDefault = "group_white.png",
            IconNameSelected = "group_red.png",
            IconNameInvalid = "group_black.png",
            IsCurrentlyValid = new Func<bool>(() => Player.GroupManager.MemberCount > 0)
        };

        PopUpBox VehicleActionBox = new PopUpBox(6, "Vehicle Actions", VehicleActionsSubMenuName, "Open Vehicle Actions Sub Menu")
        {
            ClosesMenu = false,
            IsCurrentlyValid = new Func<bool>(() => true)
        };



        PopUpBox AffiliationActionBox = new PopUpBox(7, "Affilitation", AffilitationSubMenuName, "Open the Affiliation Sub Menu")
        {
            ClosesMenu = false,
            IsCurrentlyValid = new Func<bool>(() => true)
        };




        List<PopUpBox> OnFootMenuMaps = new List<PopUpBox>()
        {
            InfoBox,
            ActionsBox,
            WeaponsBox,
            new PopUpBox(3,"Stances","StancesSubMenu","Open Stances Sub Menu"){ ClosesMenu = false,
                IconNameDefault = "stance_white.png",
                IconNameSelected = "stance_red.png",
                IconNameInvalid = "stance_black.png", },
            InventoryBox,
            GroupBox,
            VehicleActionBox,
            //AffiliationActionBox,
        };
        List<PopUpBox> InVehicleMenuMaps = new List<PopUpBox>()
        {
            InfoBox,
            ActionsBox,
            WeaponsBox,
            new PopUpBox(3,"Vehicle Controls","VehicleSubMenu","Open Vehicle Control Sub Menu"){ ClosesMenu = false,
                IconNameDefault = "vehicle_white.png",
                IconNameSelected = "vehicle_red.png",
                IconNameInvalid = "vehicle_black.png" },
            InventoryBox,
            GroupBox,
            //AffiliationActionBox,
        };

        if(Player.IsServicePed)
        {
            OnFootMenuMaps.Add(AffiliationActionBox);
            InVehicleMenuMaps.Add(AffiliationActionBox);
        }

        List<PopUpBox> InfoSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0, "Player Info", UI.PlayerInfoMenu.Toggle,"Display the Player Info Menu") {
                IconNameDefault = "info_white.png",
                IconNameSelected = "info_red.png",
                IconNameInvalid = "info_black.png" },
            new PopUpBox(1, "Messages", UI.MessagesMenu.Toggle,"Display the Messages and Contacts Menu") {
                IconNameDefault = "message_white.png",
                IconNameSelected = "message_red.png",
                IconNameInvalid = "message_black.png" },
            new PopUpBox(2, "Burner Cell", Player.CellPhone.OpenBurner,"Open the burner phone") {
                IconNameDefault = "burnerphone_white.png",
                IconNameSelected = "burnerphone_red.png",
                IconNameInvalid = "burnerphone_black.png" },
    };
        List<PopUpBox> ActionsSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Gesture","Gesture","Open Gesture Sub Menu") { ClosesMenu = false },
            new PopUpBox(1,"Dance","Dance","Open Dance Sub Menu") { ClosesMenu = false },
            new PopUpBox(2,"Suicide",Player.ActivityManager.CommitSuicide,"Commit suicide"),
            new PopUpBox(3,"Sitting", "SitSubMenu","Open Sitting Sub Menu") { ClosesMenu = false },
            new PopUpBox(4,"Sleep", new Action(() => Player.ActivityManager.StartSleeping()),"Start sleeping here"),
            new PopUpBox(5,"Stop Activities",new Action(() => Player.ActivityManager.ForceCancelAllActivities()),"Stops all active and paused activites"),
            new PopUpBox(6,"Surrender",new Action(() => Player.Surrendering.ToggleSurrender()),"Toggle surrendering"),
            new PopUpBox(7,"BodyArmor","BodyArmorSubMenu","Open Body Armor Menu") { ClosesMenu = false },
            new PopUpBox(8,"Wave Hands",new Action(() => Player.ActivityManager.WaveHands()),"Get Nearby Attention"),
            new PopUpBox(9,"Taxi Actions","TaxiOptionsSubMenu","Access taxi items") { ClosesMenu = false },
            new PopUpBox(10,"Bodily Functions","BodilyFunctionsSubMenu","Access bodily functions") { ClosesMenu = false },
            new PopUpBox(11, "Crafting", new Action(() => UI.ToggleCraftingMenu()), "Opens the crafting menu") { ClosesMenu = true },
        };

        List<PopUpBox> TaxiOptionsSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Hail Taxi",new Action(() => Player.ActivityManager.HailTaxi()),"Hail a nearby taxi"),
            new PopUpBox(1,"Dismiss Hailed Taxi",new Action(() => Player.ActivityManager.DismissTaxi()),"Dismiss a hailed nearby taxi"),
        };
        List<PopUpBox> BodilyFunctionsSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Urinate Here", new Action(() => Player.ActivityManager.Urinate()),"Urinite here"),
            new PopUpBox(1,"Defecate Here", new Action(() => Player.ActivityManager.Defecate()),"Defecate here"),
            new PopUpBox(2,"Sit On Toilet Here (Front)", new Action(() => Player.ActivityManager.StartSittingOnToilet(false,true)),"Sit on toilet here facing forwards"),
            new PopUpBox(3,"Sit On Toilet Here (Back)", new Action(() => Player.ActivityManager.StartSittingOnToilet(false,false)),"Sit on toilet here facing backwards"),
        };



        List<PopUpBox> VehicleActionsSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Remove Plate",new Action(() => Player.ActivityManager.RemovePlate()),"Remove the license plate from the nearest vehicle."),
            new PopUpBox(1,"Enter Vehicle (Passenger)", new Action(() => Player.ActivityManager.EnterVehicleAsPassenger(false, false, true)),"Enter vehicle you are looking at as passenger"),
            new PopUpBox(2,"Enter Vehicle (By Seat)", "EnterSeatSubMenu","Enter vehicle you are looking at and sit on the specific seat") { ClosesMenu = false },
            new PopUpBox(3,"Toggle Door", "OpenDoorSubMenu","Toggle the door of the vehicle you are looking at") { ClosesMenu = false },
            new PopUpBox(4,"Menu",new Action(() => Player.ShowVehicleInteractMenu(true)),"Show Vehicle Interaction Menu"),
        };


        List<PopUpBox> AffiliationSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Toggle Auto Backup",new Action(() => Player.ToggleAutoBackup()),"Toggle automatic backup being called."),
        };


        List<PopUpBox> SitSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Sit (Nearest)", new Action(() => Player.ActivityManager.StartSittingDown(true,true)),"Sit down at nearest seat"),
            new PopUpBox(1,"Sit Here (Front)", new Action(() => Player.ActivityManager.StartSittingDown(false,true)),"Sit here facing forwards"),
            new PopUpBox(2,"Sit Here (Back)", new Action(() => Player.ActivityManager.StartSittingDown(false,false)),"Sit here facing backwards"),
        };


        List<PopUpBox> BodyArmorSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Remove Armor", new Action(() => Player.ArmorManager.RemoveArmor()),"Remove any armor you have on"),
        };



        List<PopUpBox> EnterSeatSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Driver", new Action(() => Player.ActivityManager.EnterVehicleInSpecificSeat(false,-1, false, true)),"Sit in the drivers seat"),
            new PopUpBox(1,"Passenger", new Action(() => Player.ActivityManager.EnterVehicleInSpecificSeat(false,0, false, true)),"Sit in the passenger seat"),
            new PopUpBox(2,"Left Rear", new Action(() => Player.ActivityManager.EnterVehicleInSpecificSeat(false,1, false, true)),"Sit in the left rear seat"),
            new PopUpBox(3,"Right Rear", new Action(() => Player.ActivityManager.EnterVehicleInSpecificSeat(false,2, false, true)),"Sit in the right rear seat"),
            new PopUpBox(4,"Seat Extra 1", new Action(() => Player.ActivityManager.EnterVehicleInSpecificSeat(false,3, false, true)),"Sit in the first extra seat"),
            new PopUpBox(5,"Seat Extra 2", new Action(() => Player.ActivityManager.EnterVehicleInSpecificSeat(false,4, false, true)),"Sit in the second extra seat"),
        };


        List<PopUpBox> OpenDoorSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Driver", new Action(() => Player.ActivityManager.ToggleDoor(0,true, null)),"Toggle the driver door"),
            new PopUpBox(1,"Passenger", new Action(() => Player.ActivityManager.ToggleDoor(1,true, null)),"Toggle the passenger door"),
            new PopUpBox(2,"Left Rear", new Action(() => Player.ActivityManager.ToggleDoor(2,true, null)),"Toggle the left rear door"),
            new PopUpBox(3,"Right Rear", new Action(() => Player.ActivityManager.ToggleDoor(3,true, null)),"Toggle the right rear door"),
            new PopUpBox(4,"Hood", new Action(() => Player.ActivityManager.ToggleDoor(4,true, null)),"Toggle the hood (bonnet)"),
            new PopUpBox(5,"Trunk", new Action(() => Player.ActivityManager.ToggleDoor(5,true, null)),"Toggle the trunk (boot)"),
            new PopUpBox(6,"Other", new Action(() => Player.ActivityManager.ToggleDoor(6,true, null)),"Toggle the other door"),
        };




        List<PopUpBox> WeaponsSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Selector",Player.WeaponEquipment.ToggleSelector,"Toggle current weapon selector") { ClosesMenu = false, IsCurrentlyValid = new Func<bool>(() => Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon.Category != WeaponCategory.Melee) },
            new PopUpBox(1,"Drop Weapon",Player.WeaponEquipment.DropWeapon,"Drop Current Weapon") { IsCurrentlyValid = new Func<bool>(() => Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon.Category != WeaponCategory.Melee) },
        };
        List<PopUpBox> StanceSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Action Mode",Player.Stance.ToggleActionMode,"Toggle action mode"),
            new PopUpBox(1,"Stealth Mode",Player.Stance.ToggleStealthMode,"Toggle stealth mode"),
            new PopUpBox(2,"Toggle Crouch",Player.Stance.Crouch,"Toggle Crouch"),
        };
        List<PopUpBox> VehicleSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Engine",Player.ActivityManager.ToggleVehicleEngine,"Toggle vehicle engine") { IsCurrentlyValid = new Func<bool>(() => Player.CurrentVehicle?.Engine.CanToggle == true)},
            new PopUpBox(1,"Indicators","IndicatorsSubMenu","Open Indicators Sub Menu") { ClosesMenu = false },

            new PopUpBox(2,"Windows","WindowsSubMenu","Open the Windows Sub Menu.") { ClosesMenu = false },


            new PopUpBox(3,"Driver Window",Player.ActivityManager.ToggleDriverWindow,"Toggle driver window. Access other windows in the menu when inside the vehicle."),
            new PopUpBox(4,"Driver Door",Player.ActivityManager.ToggleDriverDoor,"Toggle driver door. Access other doors in the menu when outside the vehicle"),
            new PopUpBox(5,"Door Locks",Player.ActivityManager.ToggleDoorLocks,"Toggle door locks."),
            new PopUpBox(6,"Menu",new Action(() => Player.ShowVehicleInteractMenu(true)),"Show Vehicle Interaction Menu"),
        };
        List<PopUpBox> IndicatorsSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Hazards",Player.ActivityManager.ToggleHazards,"Toggle the vehicle hazards"),
            new PopUpBox(1,"Right Indicator",Player.ActivityManager.ToggleRightIndicator,"Toggle right vehicle indicator"),
            new PopUpBox(2,"Left Indicator",Player.ActivityManager.ToggleLeftIndicator,"Toggle the left vehicle indicator"),
        };

        List<PopUpBox> WindowsSubMenu = new List<PopUpBox>()
        {
            new PopUpBox(0,"Driver",new Action(() => Player.ActivityManager.ToggleWindowState(0)),"Toggle the driver window"),
            new PopUpBox(1,"Passenger",new Action(() => Player.ActivityManager.ToggleWindowState(1)),"Toggle the Passenger window"),
            new PopUpBox(2,"Rear Driver",new Action(() => Player.ActivityManager.ToggleWindowState(2)),"Toggle the Rear Driver window"),
            new PopUpBox(3,"Rear Passenger",new Action(() => Player.ActivityManager.ToggleWindowState(3)),"Toggle the Rear Passenger window"),
            new PopUpBox(4,"Middle Driver",new Action(() => Player.ActivityManager.ToggleWindowState(4)),"Toggle the Middle Driver window"),
            new PopUpBox(5,"Middle Passenger",new Action(() => Player.ActivityManager.ToggleWindowState(5)),"Toggle the Middle Passenger window"),
            new PopUpBox(6,"Front Windshield",new Action(() => Player.ActivityManager.ToggleWindowState(6)),"Toggle the Front Windshield"),
            new PopUpBox(7,"Rear Windshield",new Action(() => Player.ActivityManager.ToggleWindowState(7)),"Toggle the Rear Windshield"),
        };

 

        //List<PopUpBox> GestureMenuMaps = new List<PopUpBox>();
        //int CatID = 0;
        int ID = 1;
        //foreach (string mi in Gestures.GestureLookups.Where(x => x.IsOnActionWheel).GroupBy(x => x.Category).Select(x => x.Key).Distinct().OrderBy(x => x))
        //{

        //    PopUpBox gestureCategorySubTypeBox = new PopUpBox(CatID, mi, $"{mi}{mi}SubMenu", $"Open the {mi} Sub Menu") { ClosesMenu = false };
        //    GestureMenuMaps.Add(gestureCategorySubTypeBox);

        //    List<PopUpBox> MiddleLevelList = new List<PopUpBox>();


        //    ID = 1;
        //    foreach (GestureData gd in Gestures.GestureLookups.Where(x => x.Category == mi && x.IsOnActionWheel))
        //    {
        //        MiddleLevelList.Add(new PopUpBox(ID, rgx.Replace(gd.Name, " "), new Action(() => Player.ActivityManager.Gesture(gd)), rgx.Replace(gd.Name, " ")));
        //        ID++;
        //    }
        //    CatID++;
        //}









        AddGestureMenuGroup();






        List<PopUpBox> DancesMenuMaps = new List<PopUpBox>() { new PopUpBox(0, rgx.Replace(Player.ActivityManager.LastDance.Name, " "), new Action(() => Player.ActivityManager.Dance(Player.ActivityManager.LastDance)), rgx.Replace(Player.ActivityManager.LastDance.Name, " ")) };
        ID = 1;
        foreach (DanceData gd in Dances.DanceLookups.Where(x => x.IsOnActionWheel))
        {
            DancesMenuMaps.Add(new PopUpBox(ID, rgx.Replace(gd.Name, " "), new Action(() => Player.ActivityManager.Dance(gd)), rgx.Replace(gd.Name, " ")));
            ID++;
        }

        
        NextPageMenuMap = new PopUpBox(999, "Next Page", new Action(() => CurrentPage++), "") { ClosesMenu = false };
        PrevPageMenuMap = new PopUpBox(998, "Prev Page", new Action(() => CurrentPage--), "") { ClosesMenu = false };

        MainMenuMenuMap = new PopUpBox(997, "Main Menu", new Action(() => UI.ToggleMenu()), "") { ClosesMenu = true };
        DebugMenuMenuMap = new PopUpBox(996, "Debug Menu", new Action(() => UI.ToggleDebugMenu()), "") { ClosesMenu = true };
        BurnerPhoneMenuMap = new PopUpBox(995, "Burner Phone", new Action(() => Player.CellPhone.OpenBurner()), "") { ClosesMenu = true };

        ButtonPromptItems = new List<PopUpBox>();
        AlwaysShownItems = new List<PopUpBox>
        {
            NextPageMenuMap,
            PrevPageMenuMap,
            MainMenuMenuMap,
            DebugMenuMenuMap,
            BurnerPhoneMenuMap,
        };

        PopUpMenuGroups.Add(new PopUpBoxGroup(DefaultOnFootGroupName, OnFootMenuMaps));
        PopUpMenuGroups.Add(new PopUpBoxGroup(DefaultInVehicleName, InVehicleMenuMaps));
       // PopUpMenuGroups.Add(new PopUpBoxGroup("Gesture", GestureMenuMaps) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup("Dance", DancesMenuMaps) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup("InfoSubMenu", InfoSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup(ActionsSubMenuName, ActionsSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup(VehicleActionsSubMenuName, VehicleActionsSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup(AffiliationSubMenuName, AffiliationSubMenu) { IsChild = true });



        PopUpMenuGroups.Add(new PopUpBoxGroup(WeaponsSubMenuName, WeaponsSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup("StancesSubMenu", StanceSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup("VehicleSubMenu", VehicleSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup("IndicatorsSubMenu", IndicatorsSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup("SitSubMenu", SitSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup("BodyArmorSubMenu", BodyArmorSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup("EnterSeatSubMenu", EnterSeatSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup("OpenDoorSubMenu", OpenDoorSubMenu) { IsChild = true });

        PopUpMenuGroups.Add(new PopUpBoxGroup("TaxiOptionsSubMenu", TaxiOptionsSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpBoxGroup("BodilyFunctionsSubMenu", BodilyFunctionsSubMenu) { IsChild = true });
        //TaxiOptionsSubMenu

        PopUpMenuGroups.Add(new PopUpBoxGroup("WindowsSubMenu", WindowsSubMenu) { IsChild = true });

    }
    private void SetupTextures()
    {
        Sign10 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\10mph.png");
        Sign15 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\15mph.png");
        Sign20 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\20mph.png");
        Sign25 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\25mph.png");
        Sign30 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\30mph.png");
        Sign35 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\35mph.png");
        Sign40 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\40mph.png");
        Sign45 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\45mph.png");
        Sign50 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\50mph.png");
        Sign55 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\55mph.png");
        Sign60 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\60mph.png");
        Sign65 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\65mph.png");
        Sign70 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\70mph.png");
        Sign75 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\75mph.png");
        Sign80 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\speedlimit\\80mph.png");
    }
}


