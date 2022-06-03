using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class PopUpMenu
{
    private IActionable Player;
    private ISettingsProvideable Settings;
    private List<PositionMap> PositionMaps = new List<PositionMap>();
    private List<PopUpMenuMap> OnFootMenuMaps;
    private List<PopUpMenuMap> InVehicleMenuMaps;
    private PopUpMenuMap SelectedMenuMap;
    private uint GameTimeLastClicked;
    private PopUpMenuMap PrevSelectedMenuMap;
    private PositionMap ClosestPositionMap;
    private int ActionSoundID;
    private UI UI;
    private float ConsistencyScale;
    public bool HasRanItem { get; private set; }
    private enum GTATextJustification
    {
        Center = 0,
        Left = 1,
        Right = 2,
    };
    public PopUpMenu(IActionable player, ISettingsProvideable settings, UI uI)
    {
        Player = player;
        Settings = settings;
        UI = uI;
        OnFootMenuMaps = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0, "Info", UI.ToggleReportingMenu,"Display the Player Info Menu"),
            new PopUpMenuMap(1, "Cell", UI.ToggleSimplePhoneMenu,"Display the Cell Phone Menu"),

            new PopUpMenuMap(2,"Last Gesture",Player.Gesture,"Perform last gesture"),
            new PopUpMenuMap(3,"Suicide",Player.CommitSuicide,"Commit suicide"),
            new PopUpMenuMap(4,"Dance",Player.Dance,"Dance in place"),
            new PopUpMenuMap(5,"Action Mode",Player.ToggleActionMode,"Toggle action mode"),
            new PopUpMenuMap(6,"Stealth Mode",Player.ToggleStealthMode,"Toggle stealth mode"),
            new PopUpMenuMap(7,"Hands Up",Player.ToggleSurrender,"Toggle hands-up mode"),
            new PopUpMenuMap(8,"Selector",Player.ToggleSelector,"Toggle current weapon selector") { ClosesMenu = false },

            new PopUpMenuMap(9,"Toggle Crouch",Player.Crouch,"Toggle Crouch"),//top
            new PopUpMenuMap(10,"Drop Weapon",Player.DropWeapon,"Drop Current Weapon"),
            //new PopUpMenuMap(11,"Drop Weapon 2",Player.DropWeapon),
        };

        InVehicleMenuMaps = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0, "Info", UI.ToggleReportingMenu,"Display the Player Info Menu"),
            new PopUpMenuMap(1, "Cell", UI.ToggleSimplePhoneMenu,"Display the Cell Phone Menu"),


            new PopUpMenuMap(2,"Left Indicator",Player.ToggleLeftIndicator,"Toggle the left vehicle indicator"),
            new PopUpMenuMap(3,"Hazards",Player.ToggleHazards,"Toggle the vehicle hazards"),
            new PopUpMenuMap(4,"Right Indicator",Player.ToggleRightIndicator,"Toggle right vehicle indicator"),
            new PopUpMenuMap(5,"Last Gesture",Player.Gesture,"Perform last gesture"),
            new PopUpMenuMap(6,"Engine",Player.ToggleVehicleEngine,"Toggle vehicle engine"),
            new PopUpMenuMap(7,"Driver Window",Player.ToggleDriverWindow,"Toggle driver window"),
            new PopUpMenuMap(8,"Selector",Player.ToggleSelector,"Toggle current weapon selector") { ClosesMenu = false },
            new PopUpMenuMap(9,"Driver Door",Player.CloseDriverDoor,"Close the dirver door (if open)"),
           // new PopUpMenuMap(10,"None",Player.CloseDriverDoor),
           // new PopUpMenuMap(11,"None 2",Player.CloseDriverDoor),
        };

    }
    public void Draw()
    {
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
    public void OnStopDisplaying()
    {
        //NativeFunction.Natives.RELEASE_SOUND_ID(SelectionSoundID);
        NativeFunction.Natives.RELEASE_SOUND_ID(ActionSoundID);
        Game.DisplaySubtitle("");//clear the subtitles out
        NativeFunction.Natives.x068E835A1D0DC0E3("MinigameTransitionIn");
        Game.TimeScale = 1.0f;
    }
    public void OnStartDisplaying()
    {      
        //SelectionSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
        ActionSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
        NativeFunction.Natives.xFC695459D4D0E219(0.5f, 0.5f);//_SET_CURSOR_LOCATION
        Game.TimeScale = 0.2f;
        NativeFunction.Natives.x2206bf9a37b7f724("MinigameTransitionIn", 0, true);
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
            NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);
            NativeFunction.Natives.x6C188BE134E074AA(TextToShow);
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
                if (popUpMenuMap.Action != null)
                {
                    if ((Game.IsControlJustReleased(0, GameControl.Attack) || NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 24)) && Game.GameTime - GameTimeLastClicked >= 200)//or is disbaled control just released.....
                    {
                        if (popUpMenuMap.ClosesMenu)
                        {
                            Dispose();
                            HasRanItem = true;
                        }
                        

                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(ActionSoundID, "CONTINUE", "HUD_FRONTEND_DEFAULT_SOUNDSET", 0);
                        

                        popUpMenuMap.Action();
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
            DisplayTextOnScreen(SelectedMenuMap.Description, 0.5f, 0.5f, Settings.SettingsManager.UISettings.ActionPopUpTextScale, Color.FromName(Settings.SettingsManager.UISettings.ActionPopUpTextColor), Settings.SettingsManager.UISettings.ActionPopUpTextFont, GTATextJustification.Center, 255);
        }


        if (PrevSelectedMenuMap?.Display != SelectedMenuMap?.Display)
        {
           // Game.DisplaySubtitle($"{SelectedMenuMap?.Description}");
            if(SelectedMenuMap != null)
            {
                
                
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(ActionSoundID, "CONTINUE", "HUD_FRONTEND_DEFAULT_SOUNDSET", 0);
            }


            PrevSelectedMenuMap = SelectedMenuMap;
        }

    }
    private void DrawShapesAndText()
    {
        if (Settings.SettingsManager.UISettings.ActionPopUpShowCursor)
        {
            NativeFunction.Natives.xAAE7CE1D63167423();//_SET_MOUSE_CURSOR_ACTIVE_THIS_FRAME
        }
        ConsistencyScale = (float)Game.Resolution.Height/(float)Game.Resolution.Width; //(float)Game.Resolution.Width / 3840f;
        PositionMaps.Clear();
        int ID = 0;
        int TotalItems = GetCurrentMenuMap().Count();
        if (TotalItems > 0)
        {
            double angle = 360.0 / TotalItems * Math.PI / 180.0;
            for (int i = 0; i < TotalItems; i++)
            {
                DrawSingle(ID, Settings.SettingsManager.UISettings.ActionPopUpItemCenterX + (float)Math.Cos((angle * i) - 1.5708) * Settings.SettingsManager.UISettings.ActionPopUpItemDistanceFromCenter * ConsistencyScale, Settings.SettingsManager.UISettings.ActionPopUpItemCenterY + (float)Math.Sin((angle * i) - 1.5708) * Settings.SettingsManager.UISettings.ActionPopUpItemDistanceFromCenter);
                ID++;
            }
        }
    }
    private void DrawSingle(int ID, float CurrentPositionX, float CurrentPositionY)
    {
        Color overrideColor = Color.FromName(Settings.SettingsManager.UISettings.ActionPopUpItemColor);
        PopUpMenuMap popUpMenuMap = GetCurrentMenuMap(ID);
        string display = ID.ToString();
        bool isSelected = false;
        if (popUpMenuMap != null)
        {
            display = popUpMenuMap.Display;
            if (SelectedMenuMap != null && SelectedMenuMap.ID == popUpMenuMap.ID)//is the selected item
            {
                overrideColor = Color.FromName(Settings.SettingsManager.UISettings.ActionPopUpItemSelectedColor);//Color.FromArgb(72, 133, 164, 100);
                isSelected = true;
            }
        }
        if (isSelected)
        {
            NativeFunction.Natives.DRAW_RECT(CurrentPositionX, CurrentPositionY, Settings.SettingsManager.UISettings.ActionPopUpItemWidth * ConsistencyScale * 1.05f * Settings.SettingsManager.UISettings.ActionPopUpItemScale, Settings.SettingsManager.UISettings.ActionPopUpItemHeight * 1.05f * Settings.SettingsManager.UISettings.ActionPopUpItemScale, overrideColor.R, overrideColor.G, overrideColor.B, 175, false);//NativeFunction.Natives.DRAW_RECT(CurrentPositionX, CurrentPositionY, ItemWidth, ItemHeight, 181, 48, 48, 255, false);
        }
        else
        {
            NativeFunction.Natives.DRAW_RECT(CurrentPositionX, CurrentPositionY, Settings.SettingsManager.UISettings.ActionPopUpItemWidth * ConsistencyScale * Settings.SettingsManager.UISettings.ActionPopUpItemScale, Settings.SettingsManager.UISettings.ActionPopUpItemHeight * Settings.SettingsManager.UISettings.ActionPopUpItemScale, overrideColor.R, overrideColor.G, overrideColor.B, 100, false);//NativeFunction.Natives.DRAW_RECT(CurrentPositionX, CurrentPositionY, ItemWidth, ItemHeight, 181, 48, 48, 255, false);
        }
        DisplayTextOnScreen(display, CurrentPositionX, CurrentPositionY, Settings.SettingsManager.UISettings.ActionPopUpTextScale, Color.FromName(Settings.SettingsManager.UISettings.ActionPopUpTextColor), Settings.SettingsManager.UISettings.ActionPopUpTextFont, GTATextJustification.Center, 255);
        PositionMaps.Add(new PositionMap(ID, display, CurrentPositionX, CurrentPositionY));
    }
    private PopUpMenuMap GetCurrentMenuMap(int ID)
    {
        return GetCurrentMenuMap().FirstOrDefault(x => x.ID == ID);
    }
    private List<PopUpMenuMap> GetCurrentMenuMap()
    {
        if (Player.IsInVehicle && Player.CurrentVehicle != null)
        {
            return InVehicleMenuMaps;
        }
        else
        {
            return OnFootMenuMaps;
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
}


