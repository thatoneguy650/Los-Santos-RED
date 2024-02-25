using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Xml.Serialization;
using RAGENativeUI;
using LosSantosRED.lsr;
using ExtensionsMethods;

public class ToiletInteract : InteriorInteract
{
    public bool IsStanding { get; set; } = false;
    public ToiletInteract()
    {

    }

    public ToiletInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }

    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        SetupCamera(false);
        if (!WithWarp)
        {
            if (!MoveToPosition())
            {
                Interior.IsMenuInteracting = false;
                Game.DisplayHelp("Interact Failed");
                LocationCamera?.StopImmediately(true);
                return;
            }
        }

        if(IsStanding)
        {
            Player.ActivityManager.Urinate();
            GameFiber.Sleep(2000);
            while (!Player.IsMoveControlPressed && Player.IsAliveAndFree && Player.ActivityManager.IsUrinatingDefecting)
            {
                GameFiber.Yield();
            }
        }
        else
        {
            Player.ActivityManager.StartSittingOnToilet(false, false);
            GameFiber.Sleep(2000);
            while (!Player.IsMoveControlPressed && Player.IsAliveAndFree && Player.ActivityManager.IsSitting)
            {
                GameFiber.Yield();
            }
        }

        Interior.IsMenuInteracting = false;
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);
    }
    public override void AddPrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AttemptAddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
}

