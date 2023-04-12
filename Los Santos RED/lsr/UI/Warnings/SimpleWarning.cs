using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SimpleWarning
{
    private ButtonPrompts ButtonPrompts;
    private ISettingsProvideable Settings;
    private bool IsGamePaused = false;
    private int FramesSinceStarted = 0;
    private bool IsGameFaded;
    public bool IsButtonPromptsSuspended;
    private int globalScaleformID;

    private InstructionalButtons instructional = new InstructionalButtons();

    public SimpleWarning(string titleMessage, string warningMessage, string promptMessage, ButtonPrompts buttonPrompts, ISettingsProvideable settings)
    {
        TitleMessage = titleMessage;
        WarningMessage = warningMessage;
        PromptMessage = promptMessage;
        ButtonPrompts = buttonPrompts;
        Settings = settings;
    }
    public string TitleMessage { get; set; } = "Alert";
    public string WarningMessage { get; set; } = "";
    public string PromptMessage { get; set; } = "";
    public bool IsAccepted { get; set; } = false;
    public bool IsRejected { get; set; } = false;
    public bool IsAnswered => IsAccepted || IsRejected;
    private void Setup()
    {
        FramesSinceStarted = 0;
        globalScaleformID = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("POPUP_WARNING");
        while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(globalScaleformID))
        {
            GameFiber.Yield();
        }
        IsGameFaded = Game.IsScreenFadedOut || Game.IsScreenFadingOut;
        //Game.FadeScreenOut(0, true);
        IsButtonPromptsSuspended = ButtonPrompts.IsSuspended;
        ButtonPrompts.IsSuspended = true;
        IsGamePaused = Game.IsPaused;
        InstructionalButton yesButton = new InstructionalButton(GameControl.FrontendAccept, "Yes");
        InstructionalButton noButton = new InstructionalButton(GameControl.FrontendCancel, "No");
        instructional.Buttons.Clear();
        instructional.Buttons.Add(yesButton);
        instructional.Buttons.Add(noButton);
    }
    public void Show()
    {
        if (Settings.SettingsManager.UIGeneralSettings.ShowFullscreenWarnings)
        {
            Setup();
            EntryPoint.ModController.IsDisplayingAlertScreen = true;
            FramesSinceStarted = 0;
            SetupScaleform();
            while (true)
            {
                Tick();
                if (IsAnswered)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Simple Warning Exit Result IsAccepted{IsAccepted} IsRejected{IsRejected}");
                    break;
                }
                FramesSinceStarted++;
                GameFiber.Yield();
            }
        }
        else
        {
            IsAccepted = true;
            Dispose();
        }
    }
    private void Tick()
    {
        Game.IsPaused = true;
        DisableControls();
        DrawScaleform();
        if (FramesSinceStarted >= 10)
        {
            CheckPrompts();
        }
    }
    private void CheckPrompts()
    {

        if (Game.IsControlJustPressed(2, GameControl.FrontendAccept) || NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(2, (int)GameControl.FrontendAccept))
        {
            IsAccepted = true;
            IsRejected = false;
            Dispose();
        }
        else if (Game.IsControlJustPressed(2, GameControl.FrontendCancel) || NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(2, (int)GameControl.FrontendCancel))
        {
            IsAccepted = false;
            IsRejected = true;
            Dispose();
        }
    }
    private void SetupScaleform()
    {
        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SHOW_POPUP_WARNING");

        NativeFunction.Natives.xD69736AAE04DB51A(500.0f);

        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(TitleMessage);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(WarningMessage);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(PromptMessage);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();


        NativeFunction.Natives.xC58424BA936EB458(true);
        NativeFunction.Natives.xC3D0841A0CC546A6(0);


        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
    }

    private void DrawScaleform()
    {
        NativeFunction.Natives.DRAW_SCALEFORM_MOVIE_FULLSCREEN(globalScaleformID, 255, 255, 255, 255, 0);


        instructional.Update();
        instructional.Draw();

    }

    private void Dispose()
    {
        EntryPoint.ModController.IsDisplayingAlertScreen = false;
        Game.IsPaused = IsGamePaused;
        if(!IsGameFaded)
        {
           // Game.FadeScreenIn(0);
        }
        ButtonPrompts.IsSuspended = IsButtonPromptsSuspended;

        //if (Settings.SettingsManager.UIGeneralSettings.ShowFullscreenWarnings)
        //{
        //    NativeFunction.Natives.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED(globalScaleformID);
        //}


        instructional.Buttons.Clear();
        instructional.Update();
    }
    private void DisableControls()
    {
        Game.DisableControlAction(0, GameControl.FrontendPause, true);
        Game.DisableControlAction(0, GameControl.FrontendPauseAlternate, true);

        Game.DisableControlAction(0, GameControl.FrontendAccept, true);
        Game.DisableControlAction(0, GameControl.FrontendCancel, true);
    }

}

