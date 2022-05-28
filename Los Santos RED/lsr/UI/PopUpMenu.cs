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
    private float ItemWidth = 0.07f;
    private float ItemHeight = 0.07f;
    private float ItemSpacingX = 0.04f;
    private float ItemSpacingY = 0.04f;
    private float ItemScale = 1.0f;
    private float TextScale = 0.3f;
    private int Rows = 3;
    private int Columns = 3;
    private GTAFont TextFont = GTAFont.FontMonospace;
    private Color TextColor = Color.White;
    private Color ItemColor = Color.Red;
    private Keys SelectKey = Keys.LButton;
    private PopUpMenuMap SelectedMenuMap;

    public bool HasRanItem { get; private set; }
    private enum GTATextJustification
    {
        Center = 0,
        Left = 1,
        Right = 2,
    };
    public PopUpMenu(IActionable player, ISettingsProvideable settings, float itemWidth, float itemHeight, float itemSpacingX, float itemSpacingY, float itemScale, Color itemColor, float textScale, GTAFont textFont, Color textColor, Keys selectKey)
    {
        Player = player;
        Settings = settings;
       // OnFootMenuMaps = onFootpopUpMenuMaps;

        //Rows = rows;
        //Columns = columns;
        ItemWidth = itemWidth;
        ItemHeight = itemHeight;
        ItemSpacingX = itemSpacingX;
        ItemSpacingY = itemSpacingY;
        ItemScale = itemScale;
        //ItemColor = itemColor;


        TextScale = textScale;
        TextFont = textFont;
        //TextColor = textColor;
        SelectKey = selectKey;



        OnFootMenuMaps = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Last Gesture",Player.Gesture),
            new PopUpMenuMap(1,"Suicide",Player.CommitSuicide),
            new PopUpMenuMap(2,"Dance",Player.Dance),
            new PopUpMenuMap(3,"Action Mode",Player.ToggleActionMode),
            new PopUpMenuMap(4,"Stealth Mode",Player.ToggleStealthMode),
            new PopUpMenuMap(5,"Raise Hands",Player.ToggleSurrender),
            new PopUpMenuMap(6,"Selector",Player.ToggleSelector) { ClosesMenu = false },
            new PopUpMenuMap(7,"Crouch",Player.Crouch),
            new PopUpMenuMap(8,"Drop Weapon",Player.DropWeapon),
        };

        InVehicleMenuMaps = new List<PopUpMenuMap>()
        {
            new PopUpMenuMap(0,"Left Indicator",Player.ToggleLeftIndicator),
            new PopUpMenuMap(1,"Hazards",Player.ToggleHazards),
            new PopUpMenuMap(2,"Right Indicator",Player.ToggleRightIndicator),
            new PopUpMenuMap(3,"Last Gesture",Player.Gesture),
            new PopUpMenuMap(4,"Toggle Engine",Player.ToggleVehicleEngine),
            new PopUpMenuMap(5,"",null),
            new PopUpMenuMap(6,"Selector",Player.ToggleSelector) { ClosesMenu = false },
            new PopUpMenuMap(7,"Close Driver Door",Player.CloseDriverDoor),
            new PopUpMenuMap(8,"",null),
        };

    }
    public void Draw()
    {
        DrawShapesAndText();
        DisableControls();
        CheckForClick();
    }
    public void Dispose()
    {
        //Game.LocalPlayer.HasControl = true;
        Game.TimeScale = 1.0f;
    }
    public void Reset()
    {
        HasRanItem = false;
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
    private void CheckForClick()
    {
        NativeFunction.Natives.xAAE7CE1D63167423();//_SET_MOUSE_CURSOR_ACTIVE_THIS_FRAME
        if (1==1)//Game.IsKeyDownRightNow(SelectKey))
        {
            MouseState mouseState = Game.GetMouseState();
            if (mouseState != null)
            {
                float XPercent = (float)mouseState.X / (float)Game.Resolution.Width;
                float YPercent = (float)mouseState.Y / (float)Game.Resolution.Height;
                //EntryPoint.WriteToConsole($"mouseState.X: {mouseState.X} {XPercent} mouseState.Y: {mouseState.Y} {YPercent}");
                foreach (PositionMap positionMap in PositionMaps)
                {
                    float LowerX = positionMap.PositionX - (ItemWidth / 2f);
                    float HigherX = positionMap.PositionX + (ItemWidth / 2f);
                    if (XPercent >= LowerX && XPercent <= HigherX)
                    {
                        float LowerY = positionMap.PositionY - (ItemHeight / 2f);
                        float HigherY = positionMap.PositionY + (ItemHeight / 2f);
                        if (YPercent >= LowerY && YPercent <= HigherY)
                        {
                            PopUpMenuMap popUpMenuMap;
                            if (Player.IsInVehicle && Player.CurrentVehicle != null)
                            {
                                popUpMenuMap = InVehicleMenuMaps.FirstOrDefault(x => x.ID == positionMap.ID);
                            }
                            else
                            {
                                popUpMenuMap = OnFootMenuMaps.FirstOrDefault(x => x.ID == positionMap.ID);
                            }

                            if (popUpMenuMap != null)
                            {
                                if (popUpMenuMap.Action != null)
                                {


                                    if (Game.IsKeyDownRightNow(SelectKey))
                                    {

                                        if (popUpMenuMap.ClosesMenu)
                                        {
                                            Dispose();
                                            HasRanItem = true;
                                        }
                                        popUpMenuMap.Action();
                                    }
                                    SelectedMenuMap = popUpMenuMap;
                                }
                               // Game.DisplaySubtitle($"You clicked {popUpMenuMap.ID} {popUpMenuMap.Display}");
                            }
                            //RunItem(positionMap.ID);
                            break;
                        }
                    }
                }
            }
        }
    }
    private void DrawShapesAndText()
    {
        float ConsistencyScale = (float)Game.Resolution.Width / 2160f;

        PositionMaps.Clear();

       // ItemWidth = ItemWidth * ConsistencyScale * ItemScale;
        //ItemHeight = ItemHeight * ConsistencyScale * ItemScale;

        float CurrentPositionX;
        float CurrentPositionY;

        float startingX;
        float startingY;

        startingX = 0.5f - (((ItemWidth / 2f) + ItemSpacingX));// * (Columns-1));
        startingY = 0.5f - (((ItemHeight / 2f) + ItemSpacingY));// * (Rows-1));

        CurrentPositionX = startingX;
        CurrentPositionY = startingY;
        int ID = 0;

        for (int i = 0; i < Rows; i++)
        {
            CurrentPositionX = startingX;
            for (int j = 0; j < Columns; j++)
            {

                Color overrideColor = ItemColor;
                                                                                                                                                                       //string toShow = $"i:{i}j:{j} {ID}";
                PopUpMenuMap popUpMenuMap;
                if (Player.IsInVehicle)
                {
                    popUpMenuMap = InVehicleMenuMaps.FirstOrDefault(x => x.ID == ID);
                }
                else
                {
                    popUpMenuMap = OnFootMenuMaps.FirstOrDefault(x => x.ID == ID);
                }
                //
                string display = ID.ToString();
                if (popUpMenuMap != null)
                {
                    display = popUpMenuMap.Display;





                    if(SelectedMenuMap != null && SelectedMenuMap.ID == popUpMenuMap.ID)//is the selected item
                    {
                        overrideColor = Color.FromArgb(72, 133, 164, 100);
                    }
                    
                }

                NativeFunction.Natives.DRAW_RECT(CurrentPositionX, CurrentPositionY, ItemWidth, ItemHeight, overrideColor.R, overrideColor.G, overrideColor.B, 100, false);//NativeFunction.Natives.DRAW_RECT(CurrentPositionX, CurrentPositionY, ItemWidth, ItemHeight, 181, 48, 48, 255, false);


                DisplayTextOnScreen(display, CurrentPositionX, CurrentPositionY, TextScale, TextColor, TextFont, GTATextJustification.Center, 255);
                PositionMaps.Add(new PositionMap(ID, display, CurrentPositionX, CurrentPositionY));
                CurrentPositionX += ((ItemWidth / 2f) + ItemSpacingX);
                ID++;
            }
            CurrentPositionY += ((ItemHeight / 2f) + ItemSpacingY);
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
    private class PositionMap
    {
        public PositionMap(int id, string display, float posX, float posY)
        {
            ID = id;
            Display = display;
            PositionX = posX;
            PositionY = posY;
        }
        public int ID { get; set; }
        public string Display { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
    }

}


