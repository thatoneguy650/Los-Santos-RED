using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WheelMenu
{
    private IActionable Player;
    private ISettingsProvideable Settings;
    private UI UI;
    private List<PositionMap> PositionMaps = new List<PositionMap>();
    private uint GameTimeLastRanItem;

    public bool RecentlyRanItem => Game.GameTime - GameTimeLastRanItem <= 1000;
    public bool HasRanItem { get; private set; }
    private enum GTATextJustification
    {
        Center = 0,
        Left = 1,
        Right = 2,
    };
    public WheelMenu(IActionable player, ISettingsProvideable settings, UI uI)
    {
        Player = player;
        Settings = settings;
        UI = uI;
    }

    public void Draw()
    {
        //if (Settings.SettingsManager.UISettings.ShowStaminaBar && NativeFunction.Natives.IS_HUD_COMPONENT_ACTIVE<bool>(0))
        //{

            DrawMenu();
        
       // }
    }
    public void Dispose()
    {
        Game.LocalPlayer.HasControl = true;
    }
    public void Reset()
    {
        HasRanItem = false;
    }
    private void DrawMenu()
    {

        float ConsistencyScale = (float)Game.Resolution.Width / 2160f;
        float Scale = ConsistencyScale;



        PositionMaps.Clear();





        float MiddlePosX = 0.5f;
        float MiddlePosY = 0.5f;


        float ItemWidth = 0.07f;
        float ItemHeight = 0.07f;
        float ItemSpacingX = 0.04f;
        float ItemSpacingY = 0.04f;

        int Rows = 3;
        int Columns = 3;

        float CurrentPositionX;// = StartingPositionX;
        float CurrentPositionY;// = StartingPositionY;







        float startingX;
        float startingY;


        startingX = MiddlePosX - (((ItemWidth / 2f) + ItemSpacingX));// * (Columns-1));
        startingY = MiddlePosY - (((ItemHeight / 2f) + ItemSpacingY));// * (Rows-1));


        CurrentPositionX = startingX;
        CurrentPositionY = startingY;
        int ID = 0;

        for (int i = 0; i < Rows; i++)
        {
            CurrentPositionX = startingX;
            for (int j = 0; j < Columns; j++)
            {

                NativeFunction.Natives.DRAW_RECT(CurrentPositionX, CurrentPositionY, ItemWidth, ItemHeight, 181, 48, 48, 255, false);
                string toShow = $"i:{i}j:{j} {ID}";
                string display =  GetIDText(ID);



                //EntryPoint.WriteToConsole($"WHEEL MENU:                    {toShow}                 {display}");







                DisplayTextOnScreen(display, CurrentPositionX, CurrentPositionY, 0.3f, Color.White, GTAFont.FontMonospace, GTATextJustification.Center, 255);
                PositionMaps.Add(new PositionMap(ID, display, CurrentPositionX, CurrentPositionY));


                CurrentPositionX += ((ItemWidth / 2f) + ItemSpacingX);









                
                
                ID++;
            }
            CurrentPositionY += ((ItemHeight / 2f) + ItemSpacingY);
            

            
        }
        //NativeFunction.Natives.DRAW_RECT(0.5f, 0.5f, ItemWidth, ItemHeight, 72, 133, 164, 255, false);


        NativeFunction.Natives.xAAE7CE1D63167423();//_SET_MOUSE_CURSOR_ACTIVE_THIS_FRAME
                                                   //Game.LocalPlayer.HasControl = false;

        Game.DisableControlAction(0, GameControl.LookLeftRight, false);
        Game.DisableControlAction(0, GameControl.LookUpDown, false);



        Game.DisableControlAction(0, GameControl.Attack, false);
        Game.DisableControlAction(0, GameControl.Attack2, false);
        Game.DisableControlAction(0, GameControl.MeleeAttack1, false);
        Game.DisableControlAction(0, GameControl.MeleeAttack2, false);

        if (Game.IsKeyDownRightNow(System.Windows.Forms.Keys.LButton))
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
                            RunItem(positionMap.ID);

//#if DEBUG

//                            Game.DisplaySubtitle($"You Clicked in {positionMap.ID} {positionMap.Display}");

//#endif
                            break;
                        }
                    }
                }
            }
        }

        

    }
    private string GetIDText(int ID)
    {
        if(ID == 0)
        {
            return "Gesture";
        }
        else if (ID == 1)
        {
            return "Suicide";
        }
        else if (ID == 2)
        {

            return "Dance";


        }



        else if (ID == 3)
        {
            return "Action Mode";
            
        }
        else if (ID == 4)
        {
            return "Stealth Mode";
            
        }
        else if (ID == 5)
        {
            return "";
        }



        else if (ID == 6)
        {
            return "Toggle Selector";
            
        }
        else if (ID == 7)
        {
            return "Toggle Crouch";
        }
        else if (ID == 8)
        {
            return "";
        }
        else
        {
            return "Nine";
        }
    }
    private void RunItem(int ID)
    {
        UI.DisposeWheelMenu();
        if (ID == 0)
        {
            GameTimeLastRanItem = Game.GameTime;
            HasRanItem = true;
            Player.Gesture();
        }
        if (ID == 1)
        {
            GameTimeLastRanItem = Game.GameTime;
            HasRanItem = true;
            Player.CommitSuicide();
        }
        if (ID == 2)
        {
            GameTimeLastRanItem = Game.GameTime;
            HasRanItem = true;
            Player.Dance();
            //Player.StopDynamicActivity();
        }
        if (ID == 3)
        {
            GameTimeLastRanItem = Game.GameTime;
            HasRanItem = true;
            Player.ToggleActionMode();
        }
        if (ID == 4)
        {
            GameTimeLastRanItem = Game.GameTime;
            HasRanItem = true;
            Player.ToggleStealthMode();
        }
        if (ID == 5)
        {
            GameTimeLastRanItem = Game.GameTime;
            HasRanItem = true;
            //Player.PauseDynamicActivity();
        }
        if (ID == 6)
        {
            GameTimeLastRanItem = Game.GameTime;
            HasRanItem = true;
            Player.ToggleSelector();
        }
        if (ID == 7)
        {
            GameTimeLastRanItem = Game.GameTime;
            HasRanItem = true;
            Player.Crouch();
        }
        if (ID == 8)
        {
            GameTimeLastRanItem = Game.GameTime;
            HasRanItem = true;
            //Player.StopDynamicActivity();
        }
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


