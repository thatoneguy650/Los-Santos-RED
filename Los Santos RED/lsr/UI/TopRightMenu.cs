using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TopRightMenu
{
    private IDisplayable DisplayablePlayer;
    private ITimeReportable Time;
    private ISettingsProvideable Settings;
    private UI UI;

    private bool IsVanillaWeaponHUDVisible;
    private int VanillaDiplayedLines;
    private bool IsVanillaStarsHUDVisible;
    private bool IsVanillaCashHUDVisible;
    private string lastWeaponDisplay;
    private Texture WantedRegular;
    private Texture WantedRed;
    private Texture WantedGreyed;
    private Texture WantedWhiteGreyed;

    private Texture QuestionGreyed;
    private Texture QuestionWhite;
    private Texture QuestionYellow;
    private Texture QuestionOrange;
    private Texture QuestionDarkOrange;
    private Texture QuestionRed;

    private Texture StanceBlack;
    private Texture WeaponsBlack;
    private Texture GroupBlack;
    private Texture WeaponsRed;
    private Texture GroupGrey;

    private bool willShowCustomStars;
    private bool willShowGroup;
    private float CustomStarsPosition;
    private uint lastGameTime;
    private bool isPaused;
    private uint GameTimeLastFlashedWantedStars;
    private bool isShowingGreyedWantedStars;
    private float GroupPosition;

    public TopRightMenu(IDisplayable displayablePlayer, ITimeReportable time, ISettingsProvideable settings, UI uI)
    {
        DisplayablePlayer = displayablePlayer;
        Time = time;
        Settings = settings;
        UI = uI;
    }

    public void Setup()
    {
        WantedRegular = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\wantedlevel\\wantedstarwhite.png");
        WantedRed = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\wantedlevel\\wantedstarred.png");
        WantedGreyed = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\wantedlevel\\wantedstargreyed.png");
        WantedWhiteGreyed = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\wantedlevel\\wantedstarwhitegrey.png");

        //wantedstarwhitegrey

        QuestionGreyed = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\wantedlevel\\questionmarkgrey.png");
        QuestionWhite = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\wantedlevel\\questionmarkwhite.png");
        QuestionYellow = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\wantedlevel\\questionmarkyellow.png");
        QuestionOrange = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\wantedlevel\\questionmarkorange.png");
        QuestionDarkOrange = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\wantedlevel\\questionmarkdarkorange.png");
        QuestionRed = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\wantedlevel\\questionmarkred.png");

        GroupBlack = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\hudicons\\group_black.png");
        StanceBlack = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\hudicons\\stance_black.png");
        WeaponsBlack = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\hudicons\\weapons_black.png");
        WeaponsRed = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\hudicons\\weapons_red.png");
        GroupGrey = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\hudicons\\group_grey.png");
        Game.RawFrameRender += DrawSprites;
    }
    public void Dispose()
    {
        Game.RawFrameRender -= DrawSprites;
    }
    public void CacheData()
    {
        if (Settings.SettingsManager.LSRHUDSettings.TopDisplayShowWeapon)
        {
            lastWeaponDisplay = GetWeaponDisplay();
        }

    }
    public void Display()
    {
        isPaused = lastGameTime == Game.GameTime;
        lastGameTime = Game.GameTime;
        if (UI.IsDrawingWheelMenu && DisplayablePlayer.WeaponEquipment.CurrentWeapon != null)
        {
            NativeFunction.Natives.SHOW_HUD_COMPONENT_THIS_FRAME(2);//WEAPON_ICON
        }

       // IsVanillaStarsHUDVisible = NativeFunction.Natives.IS_HUD_COMPONENT_ACTIVE<bool>(1);
        IsVanillaWeaponHUDVisible = NativeFunction.Natives.IS_HUD_COMPONENT_ACTIVE<bool>(2);
        IsVanillaCashHUDVisible = NativeFunction.Natives.IS_HUD_COMPONENT_ACTIVE<bool>(3);

        IsVanillaCashHUDVisible = !Settings.SettingsManager.UIGeneralSettings.DisableVanillaCashDisplay;


        bool willShowCash = !IsVanillaCashHUDVisible && (DisplayablePlayer.IsTransacting || DisplayablePlayer.BankAccounts.RecentlyChangedMoney || DisplayablePlayer.IsBusted || UI.IsDrawingWheelMenu);
        bool willShowWeapon = Settings.SettingsManager.LSRHUDSettings.TopDisplayShowWeapon 
            && Settings.SettingsManager.SelectorSettings.ApplySelector && (Settings.SettingsManager.SelectorSettings.ApplySelectorWithController || !DisplayablePlayer.IsUsingController)
            && (IsVanillaWeaponHUDVisible || UI.IsDrawingWheelMenu) 
            && DisplayablePlayer.WeaponEquipment.CurrentWeapon != null 
            && DisplayablePlayer.WeaponEquipment.CurrentWeapon.Category != WeaponCategory.Melee 
            && DisplayablePlayer.WeaponEquipment.CurrentWeapon.Category != WeaponCategory.Throwable;

        bool willShowCashChange = willShowCash && DisplayablePlayer.BankAccounts.RecentlyChangedMoney;
        bool willShowNeeds = (UI.IsDrawingWheelMenu || DisplayablePlayer.HumanState.RecentlyChangedNeed || DisplayablePlayer.HealthManager.RecentlyDrainedHealth || DisplayablePlayer.HealthManager.RecentlyRegenedHealth || DisplayablePlayer.IsSleeping) && Settings.SettingsManager.NeedsSettings.ApplyNeeds;
        willShowCustomStars =  (DisplayablePlayer.IsAlive || UI.IsDrawingWheelMenu) 
            && ((DisplayablePlayer.IsWanted && Settings.SettingsManager.UIGeneralSettings.UseCustomWantedLevelStars) || (DisplayablePlayer.Investigation.IsActive && DisplayablePlayer.Investigation.RequiresPolice && Settings.SettingsManager.UIGeneralSettings.UseCustomInvestigationMarks));
        willShowGroup = DisplayablePlayer.GroupManager.MemberCount > 0 && DisplayablePlayer.IsAliveAndFree;

        CustomStarsPosition = 0.0f;
        float WeaponPosition = 0.0f;
        float CashPosition = 0.0f;
        float CashChangePosition = 0.0f;
        float NeedsPosition = 0.0f;
        GroupPosition = 0.0f;
        float StartingPosition = Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionX;
        if (IsVanillaStarsHUDVisible)
        {
            StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing;//
        }
        if (IsVanillaWeaponHUDVisible)
        {
            StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing;// 0.035f;
        }
        if (IsVanillaCashHUDVisible)
        {
            StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing;//0.035f;
        }

        if(willShowCustomStars)
        {
            CustomStarsPosition = StartingPosition;
            StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing;//0.035f;
        }
        if (willShowWeapon)
        {
            WeaponPosition = StartingPosition;
            StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing;//0.035f;
        }
        if (willShowCash)
        {
            CashPosition = StartingPosition;
            StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing;//0.035f;
        }

        if (willShowCashChange)
        {
            CashChangePosition = StartingPosition;
            StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing;//0.035f;
        }

        if (willShowNeeds)
        {
            NeedsPosition = StartingPosition;
            StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing;//0.035f;
        }

        if(willShowGroup)
        {
            GroupPosition = StartingPosition;
            StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing;//0.035f;
        }




        if (willShowWeapon)
        {
            DisplayTextOnScreen(lastWeaponDisplay, WeaponPosition, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.TopDisplayScale, Color.White, GTAFont.FontPricedown, (GTATextJustification)2, false);

        }
        if (willShowCash)
        {
            DisplayTextOnScreen(DisplayablePlayer.BankAccounts.CashDisplay(UI.IsDrawingWheelMenu), CashPosition, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.TopDisplayScale, Color.White, GTAFont.FontPricedown, (GTATextJustification)2, true);
        }
        if (willShowCashChange)
        {
            string Prefix = DisplayablePlayer.BankAccounts.LastChangeMoneyAmount > 0 ? "~g~" : "~r~";
            string indicator = DisplayablePlayer.BankAccounts.LastChangeMoneyAmount > 0 ? "+" : "-";
            DisplayTextOnScreen(Prefix + indicator + "$" + Math.Abs(DisplayablePlayer.BankAccounts.LastChangeMoneyAmount).ToString(), CashChangePosition, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.TopDisplayScale, Color.White, GTAFont.FontPricedown, (GTATextJustification)2, true);
        }
        if (willShowNeeds)
        {
            string NeedsString = "";
            if (UI.IsDrawingWheelMenu || DisplayablePlayer.IsSleeping || DisplayablePlayer.HealthManager.RecentlyDrainedHealth)
            {
                NeedsString = DisplayablePlayer.HumanState.DisplayString();
            }
            else
            {
                NeedsString = DisplayablePlayer.HumanState.RecentlyChangedString();
            }
            if (DisplayablePlayer.HealthManager.RecentlyDrainedHealth)
            {
                NeedsString += " ~r~-HP~s~";
            }
            else if (DisplayablePlayer.HealthManager.RecentlyRegenedHealth)
            {
                NeedsString += " ~g~+HP~s~";
            }
            DisplayTextOnScreen(NeedsString, NeedsPosition, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.TopDisplayScale, Color.White, GTAFont.FontPricedown, (GTATextJustification)2, true);
        }

        if (DisplayablePlayer.IsInSearchMode && Settings.SettingsManager.UIGeneralSettings.CustomWantedLevelStarsFlashWhenSearching)
        {
            uint GameTimeBetweenFlash = (uint)(Settings.SettingsManager.UIGeneralSettings.CustomWantedLevelStarsTimeBetweenFlash * (1.0f - DisplayablePlayer.SearchMode.SearchModePercentage));
            if (GameTimeBetweenFlash <= 200)
            {
                GameTimeBetweenFlash = 200;
            }
            if (Game.GameTime - GameTimeLastFlashedWantedStars >= GameTimeBetweenFlash)
            {
                GameTimeLastFlashedWantedStars = Game.GameTime;
                isShowingGreyedWantedStars = !isShowingGreyedWantedStars;
                //EntryPoint.WriteToConsole("WANTED STARS FLASHING CHANGE");
            }
        }
        else
        {
            isShowingGreyedWantedStars = false;
        }

    }
    private string GetWeaponDisplay()
    {
        string WeaponDisplay = "";
        if (Settings.SettingsManager.LSRHUDSettings.TopDisplayShowWeapon)
        {
            if (WeaponDisplay == "")
            {
                WeaponDisplay = $"{UI.CurrentDefaultTextColor}" + GetSelectorText();
            }
            else
            {
                WeaponDisplay += $" {UI.CurrentDefaultTextColor}" + GetSelectorText();
            }
        }
        return WeaponDisplay;
    }
    private string GetSelectorText()
    {
        if (DisplayablePlayer.WeaponEquipment.CurrentWeapon != null && DisplayablePlayer.WeaponEquipment.CurrentWeapon.Category != WeaponCategory.Melee && DisplayablePlayer.WeaponEquipment.CurrentWeapon.Category != WeaponCategory.Throwable)
        {
            if (Settings.SettingsManager.LSRHUDSettings.TopDisplayWeaponSimpleSelector)
            {
                if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.Safe)
                {
                    return "~w~S~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.SemiAuto)
                {
                    return "~r~1~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.TwoRoundBurst)
                {
                    return "~r~2~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.ThreeRoundBurst)
                {
                    return "~r~3~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.FourRoundBurst)
                {
                    return "~r~4~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.FiveRoundBurst)
                {
                    return "~r~5~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.FullAuto)
                {
                    if (DisplayablePlayer.WeaponEquipment.CurrentWeaponMagazineSize == 0)
                    {
                        return $"~r~FULL AUTO~s~";
                    }
                    else
                    {
                        return $"~r~{DisplayablePlayer.WeaponEquipment.CurrentWeaponMagazineSize}~s~";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.Safe)
                {
                    return "~s~SAFE~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.SemiAuto)
                {
                    return "~r~Semi-Auto~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.TwoRoundBurst)
                {
                    return "~r~2 Round Burst~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.ThreeRoundBurst)
                {
                    return "~r~3 Round Burst~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.FourRoundBurst)
                {
                    return "~r~4 Round Burst~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.FiveRoundBurst)
                {
                    return "~r~5 Round Burst~s~";
                }
                else if (DisplayablePlayer.WeaponEquipment.CurrentSelectorSetting == SelectorOptions.FullAuto)
                {
                    if (DisplayablePlayer.WeaponEquipment.CurrentWeaponMagazineSize == 0)
                    {
                        return $"~r~FULL AUTO~s~";
                    }
                    else
                    {
                        return $"~r~FULL AUTO~s~";// ~s~(~r~{DisplayablePlayer.CurrentWeaponMagazineSize}~s~)~s~";
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        else
        {
            return "";
        }
    }

    private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline)
    {
        DisplayTextOnScreen(TextToShow, X, Y, Scale, TextColor, Font, Justification, outline, 255);
    }
    private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline, int alpha)
    {
        try
        {
            if (TextToShow == "" || alpha == 0 || TextToShow is null)
            {
                return;
            }
            NativeFunction.Natives.SET_TEXT_FONT((int)Font);
            NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
            NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

            NativeFunction.Natives.SetTextJustification((int)Justification);

            NativeFunction.Natives.SET_TEXT_DROP_SHADOW();

            if (outline)
            {
                NativeFunction.Natives.SET_TEXT_OUTLINE(true);


                NativeFunction.Natives.SET_TEXT_EDGE(1, 0, 0, 0, 255);
            }
            NativeFunction.Natives.SET_TEXT_DROP_SHADOW();
            //NativeFunction.Natives.SetTextDropshadow(20, 255, 255, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);
            //NativeFunction.Natives.SetTextJustification((int)GTATextJustification.Center);
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
            NativeFunction.Natives.xCD015E5BB0D96A57(Y, X);
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
        }
        //return;
    }


    private void GetStuff(GraphicsEventArgs args)
    {
        if(isPaused || !EntryPoint.ModController.IsRunning)
        {
            return;
        }
        if (willShowCustomStars || willShowGroup)
        {
            float ConsistencyScale = (float)Game.Resolution.Width / 2160f;
            float InitialPosX = Game.Resolution.Width * Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY;
            if(willShowCustomStars)
            {
                float InitialPosY = Game.Resolution.Height * CustomStarsPosition;
                if (DisplayablePlayer.IsWanted)
                {
                    DisplayWantedLevel(args, InitialPosX, InitialPosY, ConsistencyScale * Settings.SettingsManager.UIGeneralSettings.CustomWantedLevelStarsScale);
                }
                else if (DisplayablePlayer.Investigation.IsActive)
                {
                    DisplayInvestigationMarks(args, InitialPosX, InitialPosY, ConsistencyScale * Settings.SettingsManager.UIGeneralSettings.CustomInvestigationMarksScale);
                }
            }
            if (willShowGroup)
            {
                float InitialPosY = Game.Resolution.Height * GroupPosition;
                DisplayGroupIcons(args, InitialPosX, InitialPosY, ConsistencyScale * Settings.SettingsManager.UIGeneralSettings.CustomInvestigationMarksScale);
            }
        }


    }
    private void DisplayWantedLevel(GraphicsEventArgs args, float InitialPosX, float InitialPosY, float Scale)
    {




        for (int wantedLevelStar = 1; wantedLevelStar <= Settings.SettingsManager.PoliceSettings.MaxWantedLevel; wantedLevelStar++)
        {
            Texture toShow;
            if(isShowingGreyedWantedStars)
            {
                toShow = WantedGreyed;
            }
            else if (wantedLevelStar <= DisplayablePlayer.WantedLevel)
            {
                if(!DisplayablePlayer.PoliceResponse.WantedLevelHasBeenRadioedIn)
                {
                    toShow = WantedWhiteGreyed;
                }
                else if (wantedLevelStar >= Settings.SettingsManager.UIGeneralSettings.CustomWantedLevelStarsRedColorLimit)
                {
                    toShow = WantedRed;
                }
                else
                {
                    toShow = WantedRegular;
                }
            }
            else
            {
                toShow = WantedGreyed;
            }
            float FinalPosX = InitialPosX - (wantedLevelStar * ((toShow.Size.Width - Settings.SettingsManager.UIGeneralSettings.CustomWantedLevelStarsSpacingPixelReduction) * Scale));//InitialPosX - (i * (toShow.Size.Width * Scale));
            float FinalPosY = InitialPosY;
            if (toShow == null || toShow.Size == null)
            {
                continue;
                
            }
            RectangleF rectangleF = new RectangleF(FinalPosX, FinalPosY, toShow.Size.Width * Scale, toShow.Size.Height * Scale);
            args.Graphics.DrawTexture(toShow, rectangleF);
        }
    }
    private void DisplayGroupIcons(GraphicsEventArgs args, float InitialPosX, float InitialPosY, float Scale)
    {
        Texture toShow;
        if (DisplayablePlayer.GroupManager.IsSetCombat)
        {
            toShow = WeaponsRed;
        }
        else if (DisplayablePlayer.GroupManager.IsSetFollow)
        {
            toShow = StanceBlack;
        }
        else
        {
            toShow = GroupGrey;
        }
        float FinalPosX = InitialPosX - ((toShow.Size.Width - Settings.SettingsManager.UIGeneralSettings.CustomGroupIconsSpacingPixelReduction) * Scale);//InitialPosX - (i * (toShow.Size.Width * Scale));
        float FinalPosY = InitialPosY;
        if (toShow != null && toShow.Size != null)
        {
            args.Graphics.DrawTexture(toShow, new RectangleF(FinalPosX, FinalPosY, toShow.Size.Width * Scale, toShow.Size.Height * Scale));
        }
        
    }
    private void DisplayInvestigationMarks(GraphicsEventArgs args, float InitialPosX, float InitialPosY, float Scale)
    {    
        for (int possibleWantedLevel = 1; possibleWantedLevel <= DisplayablePlayer.Investigation.InvestigationWantedLevel; possibleWantedLevel++)
        {
            Texture toShow;
            if (possibleWantedLevel <= DisplayablePlayer.Investigation.InvestigationWantedLevel)
            {   
                if(DisplayablePlayer.Investigation.IsSuspicious)
                {
                    toShow = QuestionOrange;
                }
                else 
                {
                    toShow = QuestionYellow;
                }
            }
            else
            {
                toShow = QuestionGreyed;
            }
            float FinalPosX = InitialPosX - (possibleWantedLevel * ((toShow.Size.Width - Settings.SettingsManager.UIGeneralSettings.CustomInvestigationMarksSpacingPixelReduction) * Scale));//InitialPosX - (i * (toShow.Size.Width * Scale));
            float FinalPosY = InitialPosY;
            if (toShow != null && toShow.Size != null)
            {
                args.Graphics.DrawTexture(toShow, new RectangleF(FinalPosX, FinalPosY, toShow.Size.Width * Scale, toShow.Size.Height * Scale));
            }
        }
    }
    private void DrawSprites(object sender, GraphicsEventArgs args)
    {
        try
        {
            if(isPaused) 
            { 
                return; 
            }
            GetStuff(args);
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI: Draw ERROR {ex.Message} {ex.StackTrace} ", 0);
        }
    }
}

