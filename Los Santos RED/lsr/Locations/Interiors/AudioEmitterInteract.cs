using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

public class AudioEmitterInteract : InteriorInteract
{
    private MenuPool menuPool;
    private UIMenu radioMenu;
    private IRadioStations RadioStations;

    [XmlIgnore]
    public Residence RadioableLocation { get; set; }

    public AudioEmitterInteract()
    {
    }

    public AudioEmitterInteract(string name, Vector3 position, float heading, string buttonPromptText)
        : base(name, position, heading, buttonPromptText)
    {

    }
    public override void Setup(IModItems modItems, IClothesNames clothesNames, IRadioStations radioStations)
    {
        RadioStations = radioStations;
        base.Setup(modItems, clothesNames, radioStations);
    }
    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();

        SetupCamera(false);

        if (!MoveToPosition())
        {
            Interior.IsMenuInteracting = false;
            Game.DisplayHelp("Radio Interaction Failed");
            LocationCamera?.StopImmediately(true);
            return;
        }

        Player.InteriorManager.OnStartedInteriorInteract();

        Player.ActivityManager.IsInteractingWithLocation = true;

        menuPool = new MenuPool();
        radioMenu = new UIMenu("Radio Control", "Select a Station");
        menuPool.Add(radioMenu);
        radioMenu.Width = 0.55f;

        UIMenuListScrollerItem<RadioStation> AllEmittersStatation = new UIMenuListScrollerItem<RadioStation>("All Speakers", "Set all speakers to the same station", RadioStations.RadioStationList);
        AllEmittersStatation.Activated += (sender,SelectedItem) =>
        {
            foreach(AudioEmitter emitter in Interior.AudioEmitters)
            {
                emitter.SetStation(AllEmittersStatation.SelectedItem.InternalName);
            }
        };
        radioMenu.AddItem(AllEmittersStatation);
        foreach (AudioEmitter emitter in Interior.AudioEmitters)
        {
            UIMenuListScrollerItem<RadioStation> SpecificEmitterStation = new UIMenuListScrollerItem<RadioStation>(emitter.Name, $"Set {emitter.Name} to the selected station", RadioStations.RadioStationList);
            SpecificEmitterStation.Activated += (sender, SelectedItem) =>
            {
                emitter.SetStation(SpecificEmitterStation.SelectedItem.InternalName);
            };
            radioMenu.AddItem(SpecificEmitterStation);
        }
        radioMenu.SetBannerType(EntryPoint.LSRedColor);
        radioMenu.Visible = true;
        GameFiber.StartNew(() =>
        {
            while (radioMenu.Visible)
            {
                menuPool.ProcessMenus();
                GameFiber.Yield();
            }
            Player.ActivityManager.IsInteractingWithLocation = false;
            LocationCamera?.ReturnToGameplay(true);
            LocationCamera?.StopImmediately(true);
            Interior.IsMenuInteracting = false;
            Player.InteriorManager.OnEndedInteriorInteract();
        });
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