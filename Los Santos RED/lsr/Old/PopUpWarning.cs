//using LosSantosRED.lsr.Interface;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class PopUpWarning
//{
//    private int globalScaleformID;
//    private ButtonPrompts ButtonPrompts;
//    private ISettingsProvideable Settings;
//    private bool IsGamePaused = false;
//    private int FramesSinceStarted = 0;


    
//    public PopUpWarning(string titleMessage, string warningMessage, string promptMessage, ButtonPrompts buttonPrompts, ISettingsProvideable settings)
//    {
//        TitleMessage = titleMessage;
//        WarningMessage = warningMessage;
//        PromptMessage = promptMessage;
//        ButtonPrompts = buttonPrompts;
//        Settings = settings;
//    }

//    public string TitleMessage { get; set; } = "Alert";
//    public string WarningMessage { get; set; } = "";
//    public string PromptMessage { get; set; } = "";
//    public bool IsAccepted { get; set; } = false;
//    public bool IsRejected { get; set; } = false;
//    public bool IsAnswered => IsAccepted || IsRejected;
//    public void Setup()
//    {
//        FramesSinceStarted = 0;
//        globalScaleformID = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("POPUP_WARNING");
//        while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(globalScaleformID))
//        {
//            GameFiber.Yield();
//        }
//        ButtonPrompts.AddPrompt("PopUpWarning", "No", "PopUpWarningNo", System.Windows.Forms.Keys.Escape, 1);
//        ButtonPrompts.AddPrompt("PopUpWarning", "Yes", "PopUpWarningYes", System.Windows.Forms.Keys.Enter, 999);
//        IsGamePaused = Game.IsPaused;
//    }
//    public void Show()
//    {
//        EntryPoint.ModController.IsDisplayingAlertScreen = true;
//        FramesSinceStarted = 0;
//        GameFiber.StartNew(delegate
//        {
//            try
//            {
//                while (true)
//                {
//                    Tick();
//                    if (IsAnswered)
//                    {
//                        EntryPoint.WriteToConsole($"Pop Up Warning Exit Result IsAccepted{IsAccepted} IsRejected{IsRejected}");
//                        break;
//                    }
//                    FramesSinceStarted++;
//                    GameFiber.Yield();
//                }
//            }
//            catch (Exception ex)
//            {
//                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
//                EntryPoint.ModController.CrashUnload();
//            }
//        }, "Run Debug Logic");
//    }
//    public void ShowAndWait()
//    {
//        EntryPoint.ModController.IsDisplayingAlertScreen = true;
//        FramesSinceStarted = 0;
//        while (true)
//        {
//            Tick();
//            if (IsAnswered)
//            {
//                EntryPoint.WriteToConsole($"Pop Up Warning Exit Result IsAccepted{IsAccepted} IsRejected{IsRejected}");
//                break;
//            }
//            FramesSinceStarted++;
//            GameFiber.Yield();
//        }
//    }
//    public void Tick()
//    {
//        Game.IsPaused = true;
//        SetupScaleform();
//        DrawScaleform();
//        DisableControls();
//        if (FramesSinceStarted >= 30)
//        {
//            CheckPrompts();
//        }
//    }
//    private void CheckPrompts()
//    {
//        if (ButtonPrompts.IsPressed("PopUpWarningYes"))
//        {
//            IsAccepted = true;
//            IsRejected = false;
//            Dispose();
//        }
//        else if (ButtonPrompts.IsPressed("PopUpWarningNo"))
//        {
//            IsAccepted = false;
//            IsRejected = true;
//            Dispose();
//        }
//    }

//    public void Dispose()
//    {
//        EntryPoint.ModController.IsDisplayingAlertScreen = false;
//        Game.IsPaused = IsGamePaused;
//        ButtonPrompts.RemovePrompts("PopUpWarning");

//        NativeFunction.Natives.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED(globalScaleformID);


//    }
//    private void SetupScaleform()
//    {
//        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SHOW_POPUP_WARNING");

//        NativeFunction.Natives.xD69736AAE04DB51A(500.0f);

//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(TitleMessage);
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(WarningMessage);
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
//        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(PromptMessage);
//        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();


//        NativeFunction.Natives.xC58424BA936EB458(true);
//        NativeFunction.Natives.xC3D0841A0CC546A6(0);


//        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();
//    }

//    private void DrawScaleform()
//    {
//        NativeFunction.Natives.DRAW_SCALEFORM_MOVIE_FULLSCREEN(globalScaleformID, 255, 255, 255, 255, 0);
//    }

//    private void DisableControls()
//    {
//        Game.DisableControlAction(0, GameControl.FrontendPause, true);
//        Game.DisableControlAction(0, GameControl.FrontendPauseAlternate, true);

//        Game.DisableControlAction(0, GameControl.FrontendAccept, true);
//        Game.DisableControlAction(0, GameControl.FrontendCancel, true);
//    }

//}

