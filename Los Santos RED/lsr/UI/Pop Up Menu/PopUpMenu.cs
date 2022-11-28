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

public class PopUpMenu
{
    private Regex rgx = new Regex("[^a-zA-Z0-9]");
    private IActionable Player;
    private ISettingsProvideable Settings;
    private List<PositionMap> PositionMaps = new List<PositionMap>();
    private PopUpMenuMap SelectedMenuMap;
    private uint GameTimeLastClicked;
    private PopUpMenuMap PrevSelectedMenuMap;
    private PositionMap ClosestPositionMap;
    private int ActionSoundID;
    private UI UI;
    private float ConsistencyScale;
    private int TransitionInSound;
    private int TransitionOutSound;
    private string PrevPopUpMenuGroup;
    private string CurrentPopUpMenuGroup;



    private IGestures Gestures;
    private IDances Dances;
    private int CurrentPage = 0;
    private int TotalPages = 0;

    private List<PopUpMenuGroup> PopUpMenuGroups = new List<PopUpMenuGroup>();
    private float excessiveItemScaler;
    private float excessiveCenterScaler;
    private PopUpMenuMap NextPageMenuMap;

    private PopUpMenuMap PrevPageMenuMap;
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
    private float SpeedLimitConsistencyScale;
    private float SpeedLimitScale;
    private float SpeedLimitPosX;
    private float SpeedLimitPosY;
    private HashSet<DrawableIcon> IconsToDraw = new HashSet<DrawableIcon>();

    private bool IsCurrentPopUpMenuGroupDefault => CurrentPopUpMenuGroup == "DefaultInVehicle" || CurrentPopUpMenuGroup == "DefaultOnFoot";

    public bool HasRanItem { get; private set; }
    public bool IsActive { get; private set; }
    private enum GTATextJustification
    {
        Center = 0,
        Left = 1,
        Right = 2,
    };
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
        List<PopUpMenuMap> OnFootMenuMaps = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0, "Info", "InfoSubMenu","Open Player Info Sub Menu") { ClosesMenu = false, 
                IconNameDefault = "info_white.png", IconNameSelected = "info_red.png", IconNameInvalid = "info_black.png" },        
            new PopUpMenuMap(1,"Actions","ActionsSubMenu","Open Actions Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "actions_white.png", IconNameSelected = "actions_red.png", IconNameInvalid = "actions_black.png" },
            new PopUpMenuMap(2,"Weapons","WeaponsSubMenu","Open Weapons Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "weapons_white.png", IconNameSelected = "weapons_red.png", IconNameInvalid = "weapons_black.png", IsCurrentlyValid = new Func<bool>(() => Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon.Category != WeaponCategory.Melee) },
            new PopUpMenuMap(3,"Stances","StancesSubMenu","Open Stances Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "stance_white.png", IconNameSelected = "stance_red.png", IconNameInvalid = "stance_black.png", },
            new PopUpMenuMap(4,"Inventory","InventoryCategoriesSubMenu","Open Inventory Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "inventory_white.png", IconNameSelected = "inventory_red.png", IconNameInvalid = "inventory_black.png", },
            new PopUpMenuMap(5,"Group","GroupMembersSubMenu","Open Group Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "group_white.png", IconNameSelected = "group_red.png", IconNameInvalid = "group_black.png",IsCurrentlyValid = new Func<bool>(() => Player.GroupManager.MemberCount > 0) },
#if DEBUG
            new PopUpMenuMap(6,"Affiliation","AffiliationSubMenu","Open Affiliation Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "affiliation_white.png", IconNameSelected = "affiliation_red.png", IconNameInvalid = "affiliation_black.png",IsCurrentlyValid = new Func<bool>(() => false) },
           // new PopUpMenuMap(7,"Belt","BeltItemsSubMenu","Open Belt Sub Menu"){ ClosesMenu = false },
#endif
        };
        List<PopUpMenuMap> InVehicleMenuMaps = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0, "Info", "InfoSubMenu","Open Player Info Sub Menu") { ClosesMenu = false, 
                IconNameDefault = "info_white.png", IconNameSelected = "info_red.png", IconNameInvalid = "info_black.png" },      
            new PopUpMenuMap(1,"Actions","ActionsSubMenu","Open Actions Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "actions_white.png", IconNameSelected = "actions_red.png", IconNameInvalid = "actions_black.png" },
            new PopUpMenuMap(2,"Weapons","WeaponsSubMenu","Open Weapons Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "weapons_white.png", IconNameSelected = "weapons_red.png", IconNameInvalid = "weapons_black.png", IsCurrentlyValid = new Func<bool>(() => Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon.Category != WeaponCategory.Melee) },
            new PopUpMenuMap(3,"Vehicle Controls","VehicleSubMenu","Open Vehicle Control Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "vehicle_white.png", IconNameSelected = "vehicle_red.png", IconNameInvalid = "vehicle_black.png" },
            new PopUpMenuMap(4,"Inventory","InventoryCategoriesSubMenu","Open Inventory Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "inventory_white.png", IconNameSelected = "inventory_red.png", IconNameInvalid = "inventory_black.png" },
            new PopUpMenuMap(5,"Group","GroupMembersSubMenu","Open Group Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "group_white.png", IconNameSelected = "group_red.png", IconNameInvalid = "group_black.png",IsCurrentlyValid = new Func<bool>(() => Player.GroupManager.MemberCount > 0) },
#if DEBUG
            new PopUpMenuMap(6,"Affiliation","AffiliationSubMenu","Open Affiliation Sub Menu"){ ClosesMenu = false, 
                IconNameDefault = "affiliation_white.png", IconNameSelected = "affiliation_red.png", IconNameInvalid = "affiliation_black.png",IsCurrentlyValid = new Func<bool>(() => false)  },
           // new PopUpMenuMap(7,"Belt","BeltItemsSubMenu","Open Belt Sub Menu"){ ClosesMenu = false },
#endif
        };
        List<PopUpMenuMap> InfoSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0, "Player Info", UI.TogglePlayerInfoMenu,"Display the Player Info Menu") { 
                IconNameDefault = "info_white.png", IconNameSelected = "info_red.png", IconNameInvalid = "info_black.png" },
            new PopUpMenuMap(1, "Messages", UI.ToggleMessagesMenu,"Display the Messages and Contacts Menu") { 
                IconNameDefault = "message_white.png", IconNameSelected = "message_red.png", IconNameInvalid = "message_black.png" },
            new PopUpMenuMap(2, "Burner Cell", Player.CellPhone.OpenBurner,"Open the burner phone") {
                IconNameDefault = "burnerphone_white.png", IconNameSelected = "burnerphone_red.png", IconNameInvalid = "burnerphone_black.png" },
        };
        List<PopUpMenuMap> ActionsSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Gesture","Gesture","Open Gesture Sub Menu") { ClosesMenu = false },//,IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities) },
            new PopUpMenuMap(1,"Dance","Dance","Open Dance Sub Menu") { ClosesMenu = false },//,IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities && !Player.ActivityManager.IsSitting && !Player.IsInVehicle) },
            new PopUpMenuMap(2,"Suicide",Player.ActivityManager.CommitSuicide,"Commit suicide"),//  { IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities && !Player.ActivityManager.IsSitting && !Player.IsInVehicle)},
            new PopUpMenuMap(3,"Hands Up",Player.Surrendering.ToggleSurrender,"Toggle hands up mode"),
            new PopUpMenuMap(4,"Sitting", "SitSubMenu","Open Sitting Sub Menu") { ClosesMenu = false },//, IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities && !Player.ActivityManager.IsSitting && !Player.IsInVehicle)},
            new PopUpMenuMap(5,"Sleep", new Action(() => Player.ActivityManager.StartSleeping()),"Start sleeping here"),//  { IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities && !Player.ActivityManager.IsSitting && !Player.ActivityManager.IsLayingDown)},
            new PopUpMenuMap(6,"Enter Vehicle (Passenger)", new Action(() => Player.ActivityManager.EnterVehicleAsPassenger(false)),"Enter vehicle you are looking at as passenger"),//  { IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && !Player.IsInVehicle && Player.ActivityManager.CanPerformActivities && !Player.ActivityManager.IsSitting && !Player.ActivityManager.IsLayingDown && Player.CurrentLookedAtVehicle != null)},
            new PopUpMenuMap(7,"Remove Plate",new Action(() => Player.ActivityManager.RemovePlate()),"Remove the license plate from the nearest vehicle.") ,// { IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities) },
            new PopUpMenuMap(8,"Stop Activities",new Action(() => Player.ActivityManager.ForceCancelAllActivities()),"Stops all active and paused activites"),//  { IsCurrentlyValid = new Func<bool>(() => true) },
        };


        List<PopUpMenuMap> SitSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Sit At Nearest", new Action(() => Player.ActivityManager.StartSittingDown(true,true)),"Sit down at nearest seat"),// { IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities && !Player.ActivityManager.IsSitting && !Player.IsInVehicle)},
            new PopUpMenuMap(1,"Sit Here Facing Front", new Action(() => Player.ActivityManager.StartSittingDown(false,true)),"Sit here facing forwards"),//  { IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities && !Player.ActivityManager.IsSitting && !Player.IsInVehicle)},
            new PopUpMenuMap(2,"Sit Here Facing Back", new Action(() => Player.ActivityManager.StartSittingDown(false,false)),"Sit here facing forwards"),//  { IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities && !Player.ActivityManager.IsSitting && !Player.IsInVehicle)},
        };



        List<PopUpMenuMap> WeaponsSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Selector",Player.WeaponEquipment.ToggleSelector,"Toggle current weapon selector") { ClosesMenu = false, IsCurrentlyValid = new Func<bool>(() => Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon.Category != WeaponCategory.Melee) },
            new PopUpMenuMap(1,"Drop Weapon",Player.WeaponEquipment.DropWeapon,"Drop Current Weapon") { IsCurrentlyValid = new Func<bool>(() => Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon.Category != WeaponCategory.Melee) },
        };
        List<PopUpMenuMap> StanceSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Action Mode",Player.Stance.ToggleActionMode,"Toggle action mode"),
            new PopUpMenuMap(1,"Stealth Mode",Player.Stance.ToggleStealthMode,"Toggle stealth mode"),
            new PopUpMenuMap(2,"Toggle Crouch",Player.Stance.Crouch,"Toggle Crouch"),//top
        };
        List<PopUpMenuMap> VehicleSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Engine",Player.ActivityManager.ToggleVehicleEngine,"Toggle vehicle engine") { IsCurrentlyValid = new Func<bool>(() => Player.CurrentVehicle?.Engine.CanToggle == true)},
            new PopUpMenuMap(1,"Indicators","IndicatorsSubMenu","Open Indicators Sub Menu") { ClosesMenu = false },
            new PopUpMenuMap(2,"Driver Window",Player.ActivityManager.ToggleDriverWindow,"Toggle driver window"),
            new PopUpMenuMap(3,"Driver Door",Player.ActivityManager.CloseDriverDoor,"Close driver door"),
        };
        List<PopUpMenuMap> IndicatorsSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Hazards",Player.ActivityManager.ToggleHazards,"Toggle the vehicle hazards"),
            new PopUpMenuMap(1,"Right Indicator",Player.ActivityManager.ToggleRightIndicator,"Toggle right vehicle indicator"),
            new PopUpMenuMap(2,"Left Indicator",Player.ActivityManager.ToggleLeftIndicator,"Toggle the left vehicle indicator"),
        };


        List<PopUpMenuMap> GestureMenuMaps = new List<PopUpMenuMap>() { new PopUpMenuMap(0, rgx.Replace(Player.ActivityManager.LastGesture.Name, " "), new Action(() => Player.ActivityManager.Gesture(Player.ActivityManager.LastGesture)), rgx.Replace(Player.ActivityManager.LastGesture.Name, " ")) };
        int ID = 1;
        foreach(GestureData gd in Gestures.GestureLookups.Where(x=> x.IsOnActionWheel).Take(30))
        {
            GestureMenuMaps.Add(new PopUpMenuMap(ID, rgx.Replace(gd.Name, " "), new Action(() => Player.ActivityManager.Gesture(gd)), rgx.Replace(gd.Name, " "))

            //{ 
            //    IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities) }

                )
                
                
                
                ;
            ID++;
        }
        List<PopUpMenuMap> DancesMenuMaps = new List<PopUpMenuMap>() { new PopUpMenuMap(0, rgx.Replace(Player.ActivityManager.LastDance.Name, " "), new Action(() => Player.ActivityManager.Dance(Player.ActivityManager.LastDance)), rgx.Replace(Player.ActivityManager.LastDance.Name, " ")) };
        ID = 1;
        foreach (DanceData gd in Dances.DanceLookups.Where(x=> x.IsOnActionWheel).Take(30))
        {
            DancesMenuMaps.Add(new PopUpMenuMap(ID, rgx.Replace(gd.Name, " ") , new Action(() => Player.ActivityManager.Dance(gd)), rgx.Replace(gd.Name, " "))


            //{ 
                
            //    IsCurrentlyValid = new Func<bool>(() => !Player.ActivityManager.IsPerformingActivity && Player.ActivityManager.CanPerformActivities && !Player.ActivityManager.IsSitting && !Player.IsInVehicle) }


                )
                ;
            ID++;
        }
        NextPageMenuMap = new PopUpMenuMap(999, "Next Page", new Action(() => CurrentPage++), "") { ClosesMenu = false } ;
        PrevPageMenuMap = new PopUpMenuMap(998, "Prev Page", new Action(() => CurrentPage--), "") { ClosesMenu = false };
        PopUpMenuGroups.Add(new PopUpMenuGroup("DefaultOnFoot", OnFootMenuMaps));
        PopUpMenuGroups.Add(new PopUpMenuGroup("DefaultInVehicle", InVehicleMenuMaps));
        PopUpMenuGroups.Add(new PopUpMenuGroup("Gesture", GestureMenuMaps) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("Dance", DancesMenuMaps) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("InfoSubMenu", InfoSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("ActionsSubMenu", ActionsSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("WeaponsSubMenu", WeaponsSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("StancesSubMenu", StanceSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("VehicleSubMenu", VehicleSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("IndicatorsSubMenu", IndicatorsSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("SitSubMenu", SitSubMenu) { IsChild = true });



        //Game.RawFrameRender += DrawSprites;
        Sign10 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\10mph.png");
        Sign15 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\15mph.png");
        Sign20 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\20mph.png");
        Sign25 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\25mph.png");
        Sign30 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\30mph.png");
        Sign35 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\35mph.png");
        Sign40 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\40mph.png");
        Sign45 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\45mph.png");
        Sign50 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\50mph.png");
        Sign55 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\55mph.png");
        Sign60 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\60mph.png");
        Sign65 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\65mph.png");
        Sign70 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\70mph.png");
        Sign75 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\75mph.png");
        Sign80 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\80mph.png");


        foreach(PopUpMenuGroup pumg in PopUpMenuGroups)
        {
            foreach(PopUpMenuMap b in pumg.PopUpMenuMaps)
            {
                if(!String.IsNullOrEmpty(b.IconNameDefault))
                {
                    b.MakeTexture();
                }
            }
        }

        //UpdateInventoryMenuGroups();
    }
    public void Draw()
    {
        UpdateDefaultMapping(false);
        DrawShapesAndText();
        DisableControls();
        FindClosestPositionMap();
        UpdateSelection();
    }
    public void Dispose()
    {
        IsActive = false;
        Game.TimeScale = 1.0f;
        Game.RawFrameRender -= DrawSprites;
    }
    public void OnMenuClosed()
    {
        IsActive = false;
        Game.TimeScale = 1.0f;
    }
    public void Reset()
    {
        HasRanItem = false;
    }
    private void UpdateInventoryMenuGroups()
    {
        PopUpMenuGroups.RemoveAll(x => x.Group == "Inventory");
        int ID = 0;
        int ID2 = 0;
        List<PopUpMenuMap> InventoryCategoriesSubMenu = new List<PopUpMenuMap>();
        foreach (ItemType mi in Player.Inventory.ItemsList.GroupBy(x => x.ModItem?.ItemType).Select(x => x.Key).Distinct().OrderBy(x=>x.Value))
        {
            InventoryCategoriesSubMenu.Add(new PopUpMenuMap(ID, mi.ToString(), $"{mi}SubMenu", $"Open the {mi} Sub Menu") { ClosesMenu = false });
            ID2 = 0;

            List<PopUpMenuMap> InventoryCategorySubMenu = new List<PopUpMenuMap>();
            foreach (InventoryItem ii in Player.Inventory.ItemsList.Where(x => x.ModItem != null && x.ModItem.ItemType == mi))
            {
                InventoryCategorySubMenu.Add(new PopUpMenuMap(ID2, ii.ModItem.Name, new Action(() => Player.ActivityManager.UseInventoryItem(ii.ModItem,true)), ii.Description));
                ID2++;
            }

            PopUpMenuGroups.Add(new PopUpMenuGroup($"{mi}SubMenu", InventoryCategorySubMenu) { IsChild = true, Group = "Inventory" });

            ID++;
        }
        PopUpMenuGroups.Add(new PopUpMenuGroup("InventoryCategoriesSubMenu", InventoryCategoriesSubMenu) { IsChild = true, Group = "Inventory" });
    }
    public void OnStopDisplaying()
    {
        //NativeFunction.Natives.RELEASE_SOUND_ID(SelectionSoundID);
        NativeFunction.Natives.RELEASE_SOUND_ID(ActionSoundID);
        Game.DisplaySubtitle("");//clear the subtitles out
        if (Settings.SettingsManager.ActionWheelSettings.SetTransitionEffectOnActivate)
        {
            NativeFunction.Natives.x068E835A1D0DC0E3(Settings.SettingsManager.ActionWheelSettings.TransitionInEffect);
            NativeFunction.Natives.x2206bf9a37b7f724(Settings.SettingsManager.ActionWheelSettings.TransitionOutEffect, 0, false);
        }

        //NativeFunction.Natives.x068E835A1D0DC0E3("MinigameTransitionIn");
        Game.TimeScale = 1.0f;


        NativeFunction.Natives.STOP_SOUND(TransitionInSound);
        NativeFunction.Natives.RELEASE_SOUND_ID(TransitionInSound);
        CurrentPopUpMenuGroup = "DefaultOnFoot";

        IsActive = false;
        Game.RawFrameRender -= DrawSprites;

        //TransitionOutSound = NativeFunction.Natives.GET_SOUND_ID<int>();
        //NativeFunction.Natives.PLAY_SOUND_FRONTEND(TransitionOutSound, "1st_Person_Transition", "PLAYER_SWITCH_CUSTOM_SOUNDSET", 1);

    }
    public void OnStartDisplaying()
    {




        //SelectionSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
        IsActive = true;

        Game.RawFrameRender += DrawSprites;

        ActionSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
        NativeFunction.Natives.xFC695459D4D0E219(0.5f, 0.5f);//_SET_CURSOR_LOCATION

        if (Settings.SettingsManager.ActionWheelSettings.SetSlowMoOnActivate)
        {

            Game.TimeScale = 0.2f;
        }

        if (Settings.SettingsManager.ActionWheelSettings.SetTransitionEffectOnActivate)
        {
            NativeFunction.Natives.x2206bf9a37b7f724(Settings.SettingsManager.ActionWheelSettings.TransitionInEffect, 0, true);
        }
        NativeFunction.Natives.STOP_SOUND(TransitionOutSound);
        NativeFunction.Natives.RELEASE_SOUND_ID(TransitionOutSound);

        TransitionInSound = NativeFunction.Natives.GET_SOUND_ID<int>();
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(TransitionInSound, "1st_Person_Transition", "PLAYER_SWITCH_CUSTOM_SOUNDSET", 1);
        CurrentPopUpMenuGroup = "DefaultOnFoot";
        UpdateInventoryMenuGroups();
        GameFiber.Yield();
        UpdateGroupMenuGroups();
        GameFiber.Yield();
        UpdateAffiliationMenuGroups();
        GameFiber.Yield();
        UpdateBeltItems();
        GameFiber.Yield();




        //UpdateGroupMembers();
    }
    private void UpdateAffiliationMenuGroups()
    {
        PopUpMenuGroups.RemoveAll(x => x.Group == "Affiliation");
        List<PopUpMenuMap> AffiliationSubMenu = new List<PopUpMenuMap>();
        if(Player.IsCop)
        {
            AffiliationSubMenu.Add(new PopUpMenuMap(0, "Set Taskable", new Action(() => Player.ToggleCopTaskable()), "Set As Taskable"));
        }
        PopUpMenuGroups.Add(new PopUpMenuGroup("AffiliationSubMenu", AffiliationSubMenu) { IsChild = true, Group = "Affiliation" });
    }
    private void UpdateBeltItems()
    {
        //PopUpMenuGroups.RemoveAll(x => x.Group == "Belt");
        //int ID2 = 0;
        //List<PopUpMenuMap> beltItems = new List<PopUpMenuMap>();
        //foreach (AttachedItem ii in Player.ActivityManager.AttachedItems.Where(x => x.ModItem != null))
        //{
        //    beltItems.Add(new PopUpMenuMap(ID2, ii.ModItem.Name, new Action(() => Player.ActivityManager.UseBeltItem(ii, true)), ii.ModItem.Description));
        //    ID2++;
        //}
        //PopUpMenuGroups.Add(new PopUpMenuGroup("BeltItemsSubMenu", beltItems) { IsChild = true, Group = "Belt" });
    }
    private void UpdateGroupMenuGroups()
    {
        PopUpMenuGroups.RemoveAll(x => x.Group == "Group");
        int ID = 0;
        List<PopUpMenuMap> GroupMembersSubMenu = new List<PopUpMenuMap>();
        foreach (PedExt mi in Player.GroupManager.CurrentGroupMembers)
        {
            GroupMembersSubMenu.Add(new PopUpMenuMap(ID, mi.Name, $"{mi.Name}SubMenu", $"Open the {mi} Sub Menu") { ClosesMenu = false });
            List<PopUpMenuMap> GroupMemberSubMenu = new List<PopUpMenuMap>();
            GroupMemberSubMenu.Add(new PopUpMenuMap(0, "Give Weapon", new Action(() => Player.GroupManager.GiveCurrentWeapon(mi)), "Give Current Weapon"));
            GroupMemberSubMenu.Add(new PopUpMenuMap(1, "Remove Member", new Action(() => Player.GroupManager.Remove(mi)), "Remove the Member"));


            GroupMemberSubMenu.Add(new PopUpMenuMap(2, "Rest Tasks", new Action(() => Player.GroupManager.ResetStatus(mi)), "Reset the member's tasks"));

            PopUpMenuGroups.Add(new PopUpMenuGroup($"{mi.Name}SubMenu", GroupMemberSubMenu) { IsChild = true, Group = "Group" });
            ID++;
        }
        PopUpMenuGroups.Add(new PopUpMenuGroup("GroupMembersSubMenu", GroupMembersSubMenu) { IsChild = true, Group = "Group" });
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
        return NativeFunction.Natives.x9040DFB09BE75706<int>(X,Y);
    }
    private void FindClosestPositionMap()
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
                if (distanceToMouse <= ClosestDistance && distanceToMouse <= 0.15f)
                {
                    ClosestDistance = distanceToMouse;
                    ClosestPositionMap = positionMap2;
                }
            }
        }
    }
    private void UpdateSelection()
    {
        if (ClosestPositionMap != null)
        {
            PopUpMenuMap popUpMenuMap = GetCurrentMenuMap(ClosestPositionMap.ID);

            if(popUpMenuMap == null && ClosestPositionMap.ID == 999)
            {
                popUpMenuMap = NextPageMenuMap;
            }
            else if (popUpMenuMap == null && ClosestPositionMap.ID == 998)
            {
                popUpMenuMap = PrevPageMenuMap;
            }
            if (popUpMenuMap != null && popUpMenuMap.IsCurrentlyValid())
            {
                if ((popUpMenuMap.Action != null || popUpMenuMap.ChildMenuID != ""))
                {
                    if ((Game.IsControlJustReleased(0, GameControl.Attack) || NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 24)) && Game.GameTime - GameTimeLastClicked >= 50)//or is disbaled control just released.....//&& Environment.TickCount - GameTimeLastClicked >= 100)//or is disbaled control just released.....
                    {
                        if (popUpMenuMap.ClosesMenu)
                        {
                            OnMenuClosed();
                            HasRanItem = true;
                        }
                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "CONTINUE", "HUD_FRONTEND_DEFAULT_SOUNDSET", 0);
                        if (popUpMenuMap.Action != null)
                        {
                            popUpMenuMap.Action();
                        }
                        else if (popUpMenuMap.ChildMenuID != "")
                        {
                            PrevPopUpMenuGroup = CurrentPopUpMenuGroup;
                            CurrentPopUpMenuGroup = popUpMenuMap.ChildMenuID;
                            CurrentPage = 0;
                            TotalPages = 0;
                        }
                        GameTimeLastClicked = Game.GameTime;//Environment.TickCount;
                    }
                }
                SelectedMenuMap = popUpMenuMap;
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
            if(SelectedMenuMap != null)
            {
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "CONTINUE", "HUD_FRONTEND_DEFAULT_SOUNDSET", 0);
            }
            PrevSelectedMenuMap = SelectedMenuMap;
        }
        if(!IsCurrentPopUpMenuGroupDefault && Game.IsControlJustPressed(0, GameControl.Aim) || NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 25))
        {

            UpdateDefaultMapping(true);
            CurrentPage = 0;
            TotalPages = 0;
        }
    }
    private void DrawShapesAndText()
    {

        SpeedLimitConsistencyScale = (float)Game.Resolution.Width / 2160f;
        SpeedLimitScale = Settings.SettingsManager.ActionWheelSettings.SpeedLimitIconScale * SpeedLimitConsistencyScale;

        if (SpeedLimitToDraw != null)
        {
            SpeedLimitPosX = (Game.Resolution.Height - (SpeedLimitToDraw.Size.Height * SpeedLimitScale)) * Settings.SettingsManager.ActionWheelSettings.SpeedLimitIconX;
            SpeedLimitPosY = (Game.Resolution.Width - (SpeedLimitToDraw.Size.Width * SpeedLimitScale)) * Settings.SettingsManager.ActionWheelSettings.SpeedLimitIconY;
        }

        if (Settings.SettingsManager.ActionWheelSettings.ShowCursor)
        {
            NativeFunction.Natives.xAAE7CE1D63167423();//_SET_MOUSE_CURSOR_ACTIVE_THIS_FRAME
        }
        ConsistencyScale = (float)Game.Resolution.Height/(float)Game.Resolution.Width; //(float)Game.Resolution.Width / 3840f;
        PositionMaps.Clear();
        IconsToDraw.Clear();
        bool DrawPages = false;
        int ID = 0;
        TotalPages = 0;
        List<PopUpMenuMap> CurrentMenuMap = GetCurrentMenuMap();
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
                if(TotalItems > Settings.SettingsManager.ActionWheelSettings.ItemsPerPage)
                {
                    excessiveItemScaler = 1.0f;
                    excessiveCenterScaler = 1.0f;
                    DrawPages = true;
                    if(CurrentPage * Settings.SettingsManager.ActionWheelSettings.ItemsPerPage > TotalItems)
                    {
                        CurrentPage = 0;
                    }
                    TotalPages = (int)Math.Ceiling(TotalItems / (float)Settings.SettingsManager.ActionWheelSettings.ItemsPerPage);
                    if (CurrentPage < 0)
                    {
                        CurrentPage = TotalPages-1;
                    }
                    startingItem = CurrentPage * Settings.SettingsManager.ActionWheelSettings.ItemsPerPage;
                    if(TotalItems - startingItem > Settings.SettingsManager.ActionWheelSettings.ItemsPerPage)
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
                if(DrawPages)
                {
                    DrawPage(PrevPageMenuMap, Settings.SettingsManager.ActionWheelSettings.PrevPageCenterX, Settings.SettingsManager.ActionWheelSettings.PrevPageCenterY);
                    DrawPage(NextPageMenuMap, Settings.SettingsManager.ActionWheelSettings.NextPageCenterX, Settings.SettingsManager.ActionWheelSettings.NextPageCenterY);
                    //DisplayTextBoxOnScreen($"{CurrentPage + 1}/{TotalPages}", Settings.SettingsManager.ActionWheelSettings.PageCenterX + 0.15f, Settings.SettingsManager.ActionWheelSettings.PageCenterY, Settings.SettingsManager.ActionWheelSettings.TextScale, Color.FromName(Settings.SettingsManager.ActionWheelSettings.TextColor), Settings.SettingsManager.ActionWheelSettings.TextFont, 255, true, Color.Black);
                }
            }
        }
        DrawMessages();
        DrawSpeedLimit();
        DrawAffiliation();
    }
    private void DrawSingle(int ID, float CurrentPositionX, float CurrentPositionY)
    {
        Color overrideColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.ItemColor);
        PopUpMenuMap popUpMenuMap = GetCurrentMenuMap(ID);
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
           if(!popUpMenuMap.IsCurrentlyValid())
            {
                textColor = Color.Gray;
                isValid = false;
            }
            if (popUpMenuMap.HasIcon)
            {

                hasIcon = true;
                Texture texture;
                if(!isValid)
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
        float selectedSizeScalar = 1.05f;
        if (!hasIcon || !Settings.SettingsManager.ActionWheelSettings.ShowOnlyIcon)
        {
            DisplayTextBoxOnScreen(display, CurrentPositionX, CurrentPositionY, Settings.SettingsManager.ActionWheelSettings.TextScale * excessiveItemScaler, textColor, Settings.SettingsManager.ActionWheelSettings.TextFont, 255, true, overrideColor);
        }
        PositionMaps.Add(new PositionMap(ID, display, CurrentPositionX, CurrentPositionY));
    }
    private void DrawPage(PopUpMenuMap popUpMenuMap, float CurrentPositionX, float CurrentPositionY)
    {
        Color overrideColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.ItemColor);
        string display = "Next Page!";
        bool isSelected = false;
        Color textColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.TextColor);
        if (popUpMenuMap != null)
        {
            display = popUpMenuMap.Display;
            if (popUpMenuMap.ID == NextPageMenuMap.ID)
            {
                display += $" {CurrentPage + 1}/{TotalPages}";
            }
            if (SelectedMenuMap != null && SelectedMenuMap.ID == popUpMenuMap.ID && SelectedMenuMap.IsCurrentlyValid())//is the selected item
            {
                overrideColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.SelectedItemColor);//Color.FromArgb(72, 133, 164, 100);
                isSelected = true;
            }
            if (!popUpMenuMap.IsCurrentlyValid())
            {
                textColor = Color.Gray;
            }
        }

        float selectedSizeScalar = 1.05f;
        DisplayTextBoxOnScreen(display, CurrentPositionX, CurrentPositionY, Settings.SettingsManager.ActionWheelSettings.TextScale * excessiveItemScaler, textColor, Settings.SettingsManager.ActionWheelSettings.TextFont, 255, true, overrideColor);
        PositionMaps.Add(new PositionMap(popUpMenuMap.ID, display, CurrentPositionX, CurrentPositionY));
    }
    private PopUpMenuMap GetCurrentMenuMap(int ID)
    {
        return GetCurrentMenuMap()?.FirstOrDefault(x => x.ID == ID);
    }
    private List<PopUpMenuMap> GetCurrentMenuMap()
    {
        return PopUpMenuGroups.FirstOrDefault(x => x.ID == CurrentPopUpMenuGroup)?.PopUpMenuMaps;
    }
    private void UpdateDefaultMapping(bool force)
    {
        if (CurrentPopUpMenuGroup == "DefaultInVehicle" || CurrentPopUpMenuGroup == "DefaultOnFoot" || force)
        {
            if (Player.IsInVehicle && Player.CurrentVehicle != null)
            {
                CurrentPopUpMenuGroup = "DefaultInVehicle";
            }
            else
            {
                CurrentPopUpMenuGroup = "DefaultOnFoot";
            }
        }



    }
    private void DisableControls()
    {
        Game.DisableControlAction(0, GameControl.LookLeftRight, false);
        Game.DisableControlAction(0, GameControl.LookUpDown, false);
        Game.DisableControlAction(0, GameControl.Attack, false);
        Game.DisableControlAction(0, GameControl.Attack2, false);
        Game.DisableControlAction(0, GameControl.MeleeAttack1, false);
        Game.DisableControlAction(0, GameControl.MeleeAttack2, false);
        Game.DisableControlAction(0, GameControl.Aim, false);
        Game.DisableControlAction(0, GameControl.VehicleAim, false);
        Game.DisableControlAction(0, GameControl.AccurateAim, false);
        Game.DisableControlAction(0, GameControl.VehiclePassengerAim, false);
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
        if (Player.IsCop || Player.IsGangMember)
        {
            string toDisplay = "Affiliation: ~n~";
            if(Player.IsCop)
            {
                toDisplay += "~b~LSPD~s~";
            }
            else if (Player.IsGangMember)
            {
                toDisplay += Player.RelationshipManager.GangRelationships.CurrentGang.ColorInitials;
            }
            DisplayTextBoxOnScreen(toDisplay, Settings.SettingsManager.ActionWheelSettings.AffiliationCenterX, Settings.SettingsManager.ActionWheelSettings.AffiliationCenterY, Settings.SettingsManager.ActionWheelSettings.TextScale, Color.White, Settings.SettingsManager.ActionWheelSettings.TextFont, 255, false, Color.FromName(Settings.SettingsManager.ActionWheelSettings.ItemColor));
        }
    }
    private void DrawSpeedLimit()
    {
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
    private void DrawSprites(object sender, GraphicsEventArgs args)
    {
        try
        {
            if (Settings.SettingsManager.ActionWheelSettings.ShowSpeedLimitIcon && IsActive && SpeedLimitToDraw != null)
            {
                if (SpeedLimitToDraw != null && SpeedLimitToDraw.Size != null)
                {
                    //float ConsistencyScale = (float)Game.Resolution.Width / 2160f;
                    //float SpeedLimitScale = Settings.SettingsManager.ActionWheelSettings.SpeedLimitIconScale * ConsistencyScale;
                    //float SpeedLimitPosX = (Game.Resolution.Height - (SpeedLimitToDraw.Size.Height * SpeedLimitScale)) * Settings.SettingsManager.ActionWheelSettings.SpeedLimitIconX;
                    //float SpeedLimitPosY = (Game.Resolution.Width - (SpeedLimitToDraw.Size.Width * SpeedLimitScale)) * Settings.SettingsManager.ActionWheelSettings.SpeedLimitIconY;
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

}


