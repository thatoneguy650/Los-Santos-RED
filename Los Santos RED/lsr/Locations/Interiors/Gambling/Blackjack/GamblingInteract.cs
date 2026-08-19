using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

public class GamblingInteract : InteriorInteract
{

    protected virtual bool CanInteract => true;

    public bool AllowBlackjack { get; set; }
    public bool AllowRoulette { get; set; }
    public bool AllowLoans { get; set; }
    [XmlIgnore]
    public GamblingDen GamblingDen { get; set; }

    public GamblingInteract()
    {

    }

    public GamblingInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
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
            if (!MoveToPosition(3.0f))
            {
                Interior.IsMenuInteracting = false;
                Game.DisplayHelp("Interact Failed");
                LocationCamera?.StopImmediately(true);
                return;
            }
        }
        if (!CanInteract)
        {
            Interior.IsMenuInteracting = false;
            Game.DisplaySubtitle("Cannot Interact");
            LocationCamera?.StopImmediately(true);
            return;
        }
        StartGame();
        Interior.IsMenuInteracting = false;
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);
    }
    private void StartGame()
    {
        GamblingDen.CreateInteriorGameMenu(AllowLoans, AllowBlackjack, AllowRoulette);
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
