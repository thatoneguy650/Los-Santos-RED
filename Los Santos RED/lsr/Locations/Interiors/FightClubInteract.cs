using Rage;
using Rage.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class FightClubInteract : InteriorInteract
{
    [XmlIgnore]
    public FightClub FightClub { get; set; }
    public override int MarkerType { get; set; } = 31;
    public FightClubInteract()
    {
    }

    public FightClubInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }
    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;

        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        //SetupCamera(false);
        //if (!MoveToPosition())
        //{
        //    Interior.IsMenuInteracting = false;
        //    Game.DisplayHelp("Access Failed");
        //    LocationCamera?.StopImmediately(true);
        //    return;
        //}
        Player.InteriorManager.OnStartedInteriorInteract();
        FightClub?.GenerateFightMenu(LocationCamera);
        //LocationCamera?.ReturnToGameplay(true);
       // LocationCamera?.StopImmediately(true);
        Interior.IsMenuInteracting = false;
        Player.InteriorManager.OnEndedInteriorInteract();
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

