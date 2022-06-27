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
    private string CurrentPopUpMenuGroup;
    private IGestures Gestures;
    private IDances Dances;

    private List<PopUpMenuGroup> PopUpMenuGroups = new List<PopUpMenuGroup>();
    private float excessiveItemScaler;

    private bool IsCurrentPopUpMenuGroupDefault => CurrentPopUpMenuGroup == "DefaultInVehicle" || CurrentPopUpMenuGroup == "DefaultOnFoot";

    public bool HasRanItem { get; private set; }
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
            new PopUpMenuMap(0, "Info", "InfoSubMenu","Open Player Info Sub Menu") { ClosesMenu = false },        
            new PopUpMenuMap(1,"Actions","ActionsSubMenu","Open Actions Sub Menu"){ ClosesMenu = false },
            new PopUpMenuMap(2,"Weapons","WeaponsSubMenu","Open Weapons Sub Menu"){ ClosesMenu = false },
            new PopUpMenuMap(3,"Stances","StancesSubMenu","Open Stances Sub Menu"){ ClosesMenu = false },
            new PopUpMenuMap(4,"Inventory","InventoryCategoriesSubMenu","Open Inventory Sub Menu"){ ClosesMenu = false },
        };
        List<PopUpMenuMap> InVehicleMenuMaps = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0, "Info", "InfoSubMenu","Open Player Info Sub Menu") { ClosesMenu = false },      
            new PopUpMenuMap(1,"Actions","ActionsSubMenu","Open Actions Sub Menu"){ ClosesMenu = false },
            new PopUpMenuMap(2,"Weapons","WeaponsSubMenu","Open Weapons Sub Menu"){ ClosesMenu = false },
            new PopUpMenuMap(3,"Vehicle Controls","VehicleSubMenu","Open Vehicle Control Sub Menu"){ ClosesMenu = false },
            new PopUpMenuMap(4,"Inventory","InventoryCategoriesSubMenu","Open Inventory Sub Menu"){ ClosesMenu = false },
        };
        List<PopUpMenuMap> InfoSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0, "Player Info", UI.TogglePlayerInfoMenu,"Display the Player Info Menu"),
            new PopUpMenuMap(1, "Messages", UI.ToggleMessagesMenu,"Display the Messages and Contacts Menu"),
            new PopUpMenuMap(2, "Burner Cell", Player.CellPhone.OpenBurner,"Open the burner phone"),
        };
        List<PopUpMenuMap> ActionsSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Gesture","Gesture","Open Gesture Sub Menu") { ClosesMenu = false },
            new PopUpMenuMap(1,"Dance","Dance","Open Dance Sub Menu") { ClosesMenu = false },
            new PopUpMenuMap(2,"Suicide",Player.CommitSuicide,"Commit suicide"),
            new PopUpMenuMap(3,"Hands Up",Player.ToggleSurrender,"Toggle hands up mode"),
        };
        List<PopUpMenuMap> WeaponsSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Selector",Player.ToggleSelector,"Toggle current weapon selector") { ClosesMenu = false },
            new PopUpMenuMap(1,"Drop Weapon",Player.DropWeapon,"Drop Current Weapon"),
        };
        List<PopUpMenuMap> StanceSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Action Mode",Player.ToggleActionMode,"Toggle action mode"),
            new PopUpMenuMap(1,"Stealth Mode",Player.ToggleStealthMode,"Toggle stealth mode"),
            new PopUpMenuMap(2,"Toggle Crouch",Player.Crouch,"Toggle Crouch"),//top
        };
        List<PopUpMenuMap> VehicleSubMenu = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Engine",Player.ToggleVehicleEngine,"Toggle vehicle engine"),
            new PopUpMenuMap(1,"Right Indicator",Player.ToggleRightIndicator,"Toggle right vehicle indicator"),
            new PopUpMenuMap(2,"Hazards",Player.ToggleHazards,"Toggle the vehicle hazards"),
            new PopUpMenuMap(3,"Left Indicator",Player.ToggleLeftIndicator,"Toggle the left vehicle indicator"),
            new PopUpMenuMap(4,"Driver Window",Player.ToggleDriverWindow,"Toggle driver window"),
            new PopUpMenuMap(5,"Driver Door",Player.CloseDriverDoor,"Close driver door"),
        };
        List<PopUpMenuMap> GestureMenuMaps = new List<PopUpMenuMap>() { new PopUpMenuMap(0, rgx.Replace(Player.LastGesture.Name, " "), new Action(() => Player.Gesture(Player.LastGesture)), rgx.Replace(Player.LastGesture.Name, " ")) };
        int ID = 1;
        foreach(GestureData gd in Gestures.GestureLookups.Where(x=> x.IsOnActionWheel).Take(30))
        {
            GestureMenuMaps.Add(new PopUpMenuMap(ID, rgx.Replace(gd.Name, " "), new Action(() => Player.Gesture(gd)), rgx.Replace(gd.Name, " ")));
            ID++;
        }
        List<PopUpMenuMap> DancesMenuMaps = new List<PopUpMenuMap>() { new PopUpMenuMap(0, rgx.Replace(Player.LastDance.Name, " "), new Action(() => Player.Dance(Player.LastDance)), rgx.Replace(Player.LastDance.Name, " ")) };
        ID = 1;
        foreach (DanceData gd in Dances.DanceLookups.Where(x=> x.IsOnActionWheel).Take(30))
        {
            DancesMenuMaps.Add(new PopUpMenuMap(ID, rgx.Replace(gd.Name, " ") , new Action(() => Player.Dance(gd)), rgx.Replace(gd.Name, " ")));
            ID++;
        }


        PopUpMenuGroups.Add(new PopUpMenuGroup("DefaultOnFoot", OnFootMenuMaps));
        PopUpMenuGroups.Add(new PopUpMenuGroup("DefaultInVehicle", InVehicleMenuMaps));
        PopUpMenuGroups.Add(new PopUpMenuGroup("Gesture", GestureMenuMaps) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("Dance", DancesMenuMaps) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("InfoSubMenu", InfoSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("ActionsSubMenu", ActionsSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("WeaponsSubMenu", WeaponsSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("StancesSubMenu", StanceSubMenu) { IsChild = true });
        PopUpMenuGroups.Add(new PopUpMenuGroup("VehicleSubMenu", VehicleSubMenu) { IsChild = true });



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
        foreach (ItemType mi in Player.Inventory.Items.GroupBy(x => x.ModItem?.ItemType).Select(x => x.Key).Distinct())
        {
            InventoryCategoriesSubMenu.Add(new PopUpMenuMap(ID, mi.ToString(), $"{mi}SubMenu", $"Open the {mi} Sub Menu") { ClosesMenu = false });
            ID2 = 0;

            List<PopUpMenuMap> InventoryCategorySubMenu = new List<PopUpMenuMap>();
            foreach (InventoryItem ii in Player.Inventory.Items.Where(x => x.ModItem != null && x.ModItem.ItemType == mi).Take(30))
            {
                InventoryCategorySubMenu.Add(new PopUpMenuMap(ID2, rgx.Replace(ii.ModItem.Name," "), new Action(() => Player.StartConsumingActivity(ii.ModItem,true)), $"Use {rgx.Replace(ii.ModItem.Name," ")} Remaining: {ii.Amount}"));
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
        NativeFunction.Natives.x068E835A1D0DC0E3(Settings.SettingsManager.ActionWheelSettings.TransitionInEffect);
        NativeFunction.Natives.x2206bf9a37b7f724(Settings.SettingsManager.ActionWheelSettings.TransitionOutEffect, 0, false);

        //NativeFunction.Natives.x068E835A1D0DC0E3("MinigameTransitionIn");
        Game.TimeScale = 1.0f;


        NativeFunction.Natives.STOP_SOUND(TransitionInSound);
        NativeFunction.Natives.RELEASE_SOUND_ID(TransitionInSound);
        CurrentPopUpMenuGroup = "DefaultOnFoot";

        //TransitionOutSound = NativeFunction.Natives.GET_SOUND_ID<int>();
        //NativeFunction.Natives.PLAY_SOUND_FRONTEND(TransitionOutSound, "1st_Person_Transition", "PLAYER_SWITCH_CUSTOM_SOUNDSET", 1);

    }
    public void OnStartDisplaying()
    {      
        //SelectionSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
        ActionSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
        NativeFunction.Natives.xFC695459D4D0E219(0.5f, 0.5f);//_SET_CURSOR_LOCATION
        Game.TimeScale = 0.2f;
        
        NativeFunction.Natives.x2206bf9a37b7f724(Settings.SettingsManager.ActionWheelSettings.TransitionInEffect, 0, true);

        NativeFunction.Natives.STOP_SOUND(TransitionOutSound);
        NativeFunction.Natives.RELEASE_SOUND_ID(TransitionOutSound);

        TransitionInSound = NativeFunction.Natives.GET_SOUND_ID<int>();
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(TransitionInSound, "1st_Person_Transition", "PLAYER_SWITCH_CUSTOM_SOUNDSET", 1);
        CurrentPopUpMenuGroup = "DefaultOnFoot";
        UpdateInventoryMenuGroups();
    }
    private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, int alpha)
    {
        try
        {
            if (TextToShow == "" || alpha == 0)
            {
                return;
            }
            NativeFunction.Natives.SET_TEXT_FONT((int)Font);
            NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
            NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

            NativeFunction.Natives.SetTextJustification((int)Justification);
            NativeFunction.Natives.SetTextDropshadow(10, 255, 0, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);

            if (Justification == GTATextJustification.Right)
            {
                NativeFunction.Natives.SET_TEXT_WRAP(0f, Y);
            }
            else
            {
                NativeFunction.Natives.SET_TEXT_WRAP(0f, 1f);
            }
            NativeFunction.Natives.x25fbb336df1804cb("STRING"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
            //NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);
            NativeFunction.Natives.x6C188BE134E074AA(TextToShow);

           // NativeFunction.Natives.x6c188be134e074aa(TextToShow);


            NativeFunction.Natives.xCD015E5BB0D96A57(X, Y);
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
        }
        //return;
    }
    private void DisplayTextBoxOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, int alpha)
    {
        try
        {
            if (TextToShow == "" || alpha == 0)
            {
                return;
            }
            NativeFunction.Natives.SET_TEXT_FONT((int)Font);
            NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
            NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

            NativeFunction.Natives.SetTextJustification((int)GTATextJustification.Left);
            NativeFunction.Natives.SetTextDropshadow(10, 255, 0, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);


            NativeFunction.Natives.SET_TEXT_WRAP(X, X + 0.15f);

            NativeFunction.Natives.x25fbb336df1804cb("jamyfafi"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
            //NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);

            NativeHelper.AddLongString(TextToShow);

            //NativeFunction.Natives.x6C188BE134E074AA(TextToShow);
            NativeFunction.Natives.xCD015E5BB0D96A57(X, Y);
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
        }
        //return;
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
                if (distanceToMouse <= ClosestDistance && distanceToMouse <= 0.2f)
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
            if (popUpMenuMap != null)
            {
                if (popUpMenuMap.Action != null || popUpMenuMap.ChildMenuID != "")
                {
                    if ((Game.IsControlJustReleased(0, GameControl.Attack) || NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 24)) && Game.GameTime - GameTimeLastClicked >= 100)//or is disbaled control just released.....
                    {
                        if (popUpMenuMap.ClosesMenu)
                        {
                            Dispose();
                            HasRanItem = true;
                        }
                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "CONTINUE", "HUD_FRONTEND_DEFAULT_SOUNDSET", 0);
                        if (popUpMenuMap.Action != null)
                        {
                            popUpMenuMap.Action();
                        }
                        else if (popUpMenuMap.ChildMenuID != "")
                        {
                            CurrentPopUpMenuGroup = popUpMenuMap.ChildMenuID;
                        }
                        GameTimeLastClicked = Game.GameTime;
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
            DisplayTextOnScreen(SelectedMenuMap.Description, 0.5f, 0.5f, Settings.SettingsManager.ActionWheelSettings.TextScale, Color.FromName(Settings.SettingsManager.ActionWheelSettings.TextColor), Settings.SettingsManager.ActionWheelSettings.TextFont, GTATextJustification.Center, 255);
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
        }
    }
    private void DrawShapesAndText()
    {
        if (Settings.SettingsManager.ActionWheelSettings.ShowCursor)
        {
            NativeFunction.Natives.xAAE7CE1D63167423();//_SET_MOUSE_CURSOR_ACTIVE_THIS_FRAME
        }
        ConsistencyScale = (float)Game.Resolution.Height/(float)Game.Resolution.Width; //(float)Game.Resolution.Width / 3840f;
        PositionMaps.Clear();
        int ID = 0;
        List<PopUpMenuMap> CurrentMenuMap = GetCurrentMenuMap();
        if (CurrentMenuMap != null)
        {
            int TotalItems = CurrentMenuMap.Count();
            if (TotalItems > 14)
            {
                float shrinkAmount = (TotalItems - 14) * 0.004f;
                excessiveItemScaler = 1.0f - shrinkAmount;
            }
            else
            {
                excessiveItemScaler = 1.0f;
            }




            if (excessiveItemScaler > 1.25f)
            {
                excessiveItemScaler = 1.25f;
            }
            if (excessiveItemScaler < 0.75f)
            {
                excessiveItemScaler = 0.75f;
            }
            if (TotalItems > 0)
            {
                double angle = 360.0 / TotalItems * Math.PI / 180.0;
                for (int i = 0; i < TotalItems; i++)
                {
                    DrawSingle(ID, Settings.SettingsManager.ActionWheelSettings.ItemCenterX + (float)Math.Cos((angle * i) - 1.5708) * Settings.SettingsManager.ActionWheelSettings.ItemDistanceFromCenter * ConsistencyScale * (1.0f / excessiveItemScaler), Settings.SettingsManager.ActionWheelSettings.ItemCenterY + (float)Math.Sin((angle * i) - 1.5708) * Settings.SettingsManager.ActionWheelSettings.ItemDistanceFromCenter * (1.0f / excessiveItemScaler));
                    ID++;
                }
            }
        }
        DrawMessages();
    }
    private void DrawSingle(int ID, float CurrentPositionX, float CurrentPositionY)
    {
        Color overrideColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.ItemColor);
        PopUpMenuMap popUpMenuMap = GetCurrentMenuMap(ID);
        string display = ID.ToString();
        bool isSelected = false;
        if (popUpMenuMap != null)
        {
            display = popUpMenuMap.Display;
            if (SelectedMenuMap != null && SelectedMenuMap.ID == popUpMenuMap.ID)//is the selected item
            {
                overrideColor = Color.FromName(Settings.SettingsManager.ActionWheelSettings.SelectedItemColor);//Color.FromArgb(72, 133, 164, 100);
                isSelected = true;
            }
        }
        if (isSelected)
        {
            NativeFunction.Natives.DRAW_RECT(CurrentPositionX, CurrentPositionY, Settings.SettingsManager.ActionWheelSettings.ItemWidth * ConsistencyScale * 1.05f * Settings.SettingsManager.ActionWheelSettings.ItemScale * excessiveItemScaler, Settings.SettingsManager.ActionWheelSettings.ItemHeight * 1.05f * Settings.SettingsManager.ActionWheelSettings.ItemScale * excessiveItemScaler, overrideColor.R, overrideColor.G, overrideColor.B, 175, false);//NativeFunction.Natives.DRAW_RECT(CurrentPositionX, CurrentPositionY, ItemWidth, ItemHeight, 181, 48, 48, 255, false);
        }
        else
        {
            NativeFunction.Natives.DRAW_RECT(CurrentPositionX, CurrentPositionY, Settings.SettingsManager.ActionWheelSettings.ItemWidth * ConsistencyScale * Settings.SettingsManager.ActionWheelSettings.ItemScale * excessiveItemScaler, Settings.SettingsManager.ActionWheelSettings.ItemHeight * Settings.SettingsManager.ActionWheelSettings.ItemScale * excessiveItemScaler, overrideColor.R, overrideColor.G, overrideColor.B, 100, false);//NativeFunction.Natives.DRAW_RECT(CurrentPositionX, CurrentPositionY, ItemWidth, ItemHeight, 181, 48, 48, 255, false);
        }
        DisplayTextOnScreen(display, CurrentPositionX, CurrentPositionY, Settings.SettingsManager.ActionWheelSettings.TextScale * excessiveItemScaler, Color.FromName(Settings.SettingsManager.ActionWheelSettings.TextColor), Settings.SettingsManager.ActionWheelSettings.TextFont, GTATextJustification.Center, 255);
        PositionMaps.Add(new PositionMap(ID, display, CurrentPositionX, CurrentPositionY));
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
                DisplayTextBoxOnScreen("~h~" + phoneResponse.ContactName + " - " + phoneResponse.TimeReceived.ToString("HH:mm"), Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionX, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionY + (YMessageSpacing * MessagesDisplayed), Settings.SettingsManager.ActionWheelSettings.MessageScale, Color.FromName(Settings.SettingsManager.ActionWheelSettings.MessageTextColor), Settings.SettingsManager.ActionWheelSettings.MessageFont, 255);
                DisplayTextBoxOnScreen(phoneResponse.Message, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionX, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionY + (YMessageSpacing * MessagesDisplayed) + YHeaderSpacing, Settings.SettingsManager.ActionWheelSettings.MessageScale, Color.FromName(Settings.SettingsManager.ActionWheelSettings.MessageTextColor), Settings.SettingsManager.ActionWheelSettings.MessageFont, 255); ;
                float Width = Settings.SettingsManager.ActionWheelSettings.ItemWidth * ConsistencyScale * Settings.SettingsManager.ActionWheelSettings.ItemScale * 3f;
                float Height = Settings.SettingsManager.ActionWheelSettings.ItemHeight * Settings.SettingsManager.ActionWheelSettings.ItemScale * 1.25f;


                float HalfWidth = Width / 2f;
                float HalfHeight = Height / 2f;


                float posX = Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionX + HalfWidth - (0.01f);
                float posY = Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionY + (YMessageSpacing * MessagesDisplayed) + HalfHeight - 0.01f;


                NativeFunction.Natives.DRAW_RECT(posX, posY, Width, Height, Color.Black.R, Color.Black.G, Color.Black.B, 100, false);
                MessagesDisplayed++;
            }
            PhoneText phoneText = Player.CellPhone.TextList.Where(x => x.TimeReceived == dateTime.Item2 && x.ContactName == dateTime.Item1).FirstOrDefault();
            if (phoneText != null)
            {
                DisplayTextBoxOnScreen("~h~" + phoneText.ContactName + " - " + phoneText.TimeReceived.ToString("HH:mm"), Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionX, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionY + (YMessageSpacing * MessagesDisplayed), Settings.SettingsManager.ActionWheelSettings.MessageScale, Color.FromName(Settings.SettingsManager.ActionWheelSettings.MessageTextColor), Settings.SettingsManager.ActionWheelSettings.MessageFont, 255);
                DisplayTextBoxOnScreen(phoneText.Message, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionX, Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionY + (YMessageSpacing * MessagesDisplayed) + YHeaderSpacing, Settings.SettingsManager.ActionWheelSettings.MessageScale, Color.FromName(Settings.SettingsManager.ActionWheelSettings.MessageTextColor), Settings.SettingsManager.ActionWheelSettings.MessageFont, 255); ;


                float Width = Settings.SettingsManager.ActionWheelSettings.ItemWidth * ConsistencyScale * Settings.SettingsManager.ActionWheelSettings.ItemScale * 3f;
                float Height = Settings.SettingsManager.ActionWheelSettings.ItemHeight * Settings.SettingsManager.ActionWheelSettings.ItemScale * 1.25f ;


                float HalfWidth = Width / 2f;
                float HalfHeight = Height / 2f;

                float posX = Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionX + HalfWidth - (0.01f);
                float posY = Settings.SettingsManager.ActionWheelSettings.MessageStartingPositionY + (YMessageSpacing * MessagesDisplayed) + HalfHeight - 0.01f;


                NativeFunction.Natives.DRAW_RECT(posX, posY, Width, Height, Color.Black.R, Color.Black.G, Color.Black.B, 100, false);

                MessagesDisplayed++;
            }
        }
    }
}


