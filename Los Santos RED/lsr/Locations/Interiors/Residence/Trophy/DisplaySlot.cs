using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DisplaySlot
{
    private UIMenu SlotSubMenu;
    private Residence DisplayResidence;
    private DisplayInteract DisplayInteract;
    private UIMenuListScrollerItem<TrophyItem> PossibleModelsScroller;
    private List<DisplayPlacement> FinalDisplayPlacements;

    public DisplaySlot()
    {

    }

    public int SlotID { get; set; }
    public Vector3 Position { get; set; }
    public float Rotation { get; set; }
    public Vector3 CameraPosition { get; set; }
    public Vector3 CameraDirection { get; set; }
    public Rotator CameraRotation { get; set; }
    public string Description { get; set; }
    public void AddToMenu(MenuPool menuPool, UIMenu trophyMainMenu, LosSantosRED.lsr.Interface.IModItems modItems, Residence trophyableLocation, DisplayInteract trophyInteract, LocationCamera locationCamera)
    {
        DisplayResidence = trophyableLocation;
        DisplayInteract = trophyInteract;

        if(DisplayResidence == null || DisplayInteract == null)
        {
            return;
        }

        FinalDisplayPlacements = new List<DisplayPlacement>();

        if (DisplayResidence.DisplayPlacements != null)
        {
            foreach (DisplayPlacement trophyPlacement in DisplayResidence.DisplayPlacements)
            {
                FinalDisplayPlacements.Add(new DisplayPlacement(trophyPlacement.SlotID, trophyPlacement.ModItemName));
            }
        }
        SlotSubMenu = menuPool.AddSubMenu(trophyMainMenu, $"Slot {SlotID}");

        SlotSubMenu.SetBannerType(EntryPoint.LSRedColor);


        SlotSubMenu.OnMenuOpen += (sender) =>
        {
            //If No trophy, then spawn one, whatever is selected
            if(DisplayResidence.DisplayPlacements.FirstOrDefault(x => x.SlotID == SlotID)?.IsSpawned == true)
            {

            }
            else
            {
                DisplayResidence.DisplayPlacements.Add(new DisplayPlacement(SlotID, PossibleModelsScroller.SelectedItem.Name));
                DisplayResidence.DisplayPlacements.FirstOrDefault(x => x.SlotID == SlotID)?.SpawnDisplay(trophyInteract.CabinetData, modItems);
            }

            locationCamera?.MoveToPosition(CameraPosition, CameraDirection, CameraRotation, true, false, false);
        };
        SlotSubMenu.OnMenuClose += (sender) =>
        {
            //Cleanup the current trophy if it exists
            //check if we had a trophy and spawn it
            DisplayPlacement finalTrophyPlacement = FinalDisplayPlacements.Where(x => x.SlotID == SlotID).FirstOrDefault();
            DisplayResidence.DisplayPlacements.FirstOrDefault(x => x.SlotID == SlotID)?.DespawnDisplay();
            DisplayResidence.DisplayPlacements.RemoveAll(x=> x.SlotID == SlotID);
            if (finalTrophyPlacement != null)
            {
                DisplayResidence.DisplayPlacements.Add(new DisplayPlacement(finalTrophyPlacement.SlotID, finalTrophyPlacement.ModItemName));
                DisplayResidence.DisplayPlacements.FirstOrDefault(x => x.SlotID == SlotID)?.SpawnDisplay(DisplayInteract.CabinetData, modItems);
            }
            locationCamera?.MoveToPosition(trophyInteract.CameraPosition, trophyInteract.CameraDirection, trophyInteract.CameraRotation, true, false, false);
        };

        //Only do unlocked trophies? need to be in inventory to be able to be displayed? mayube just display any item lol
        PossibleModelsScroller = new UIMenuListScrollerItem<TrophyItem>("Trophies", "Select trophy to display in this slot", modItems.PossibleItems.TrophyItems);
        PossibleModelsScroller.IndexChanged += (sender,oldIndex,newIndex) =>
        {
            DisplayPlacement tp = DisplayResidence.DisplayPlacements.FirstOrDefault(x => x.SlotID == SlotID);
            if(tp == null)
            {
                tp = new DisplayPlacement(SlotID, PossibleModelsScroller.SelectedItem.Name);
                DisplayResidence.DisplayPlacements.Add(tp);
            }
            tp.SpawnDisplay(DisplayInteract.CabinetData, PossibleModelsScroller.SelectedItem);
        };
        PossibleModelsScroller.Activated += (sender,selectedItem) =>
        {          
            FinalDisplayPlacements.RemoveAll(x => x.SlotID == SlotID);
            FinalDisplayPlacements.Add(new DisplayPlacement(SlotID, PossibleModelsScroller.SelectedItem.Name));
            Game.DisplaySubtitle($"Updated Trophy to {PossibleModelsScroller.SelectedItem.Name} in Slot {SlotID}");
        };
        SlotSubMenu.AddItem(PossibleModelsScroller);


        UIMenuItem RemoveSlotMenuItem = new UIMenuItem("Clear", "Select to remove the displayed item for this slot");
        RemoveSlotMenuItem.Activated += (sender, selectedItem) =>
        {
            DisplayResidence.DisplayPlacements.FirstOrDefault(x => x.SlotID == SlotID)?.DespawnDisplay();
            DisplayResidence.DisplayPlacements.RemoveAll(x => x.SlotID == SlotID);
            FinalDisplayPlacements.RemoveAll(x => x.SlotID == SlotID);
            Game.DisplaySubtitle($"Removed Display in Slot {SlotID}");
        };
        SlotSubMenu.AddItem(RemoveSlotMenuItem);
    }
}