using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TrophySlot
{
    private UIMenu SlotSubMenu;
    private Residence TrophyableLocation;
    private TrophyInteract TrophyInteract;
    private UIMenuListScrollerItem<TrophyItem> PossibleModelsScroller;
    private List<TrophyPlacement> FinalTrophyPlacements;

    public TrophySlot()
    {

    }

    public int SlotID { get; set; }
    public Vector3 Position { get; set; }
    public float Rotation { get; set; }
    public Vector3 CameraPosition { get; set; }
    public Vector3 CameraDirection { get; set; }
    public Rotator CameraRotation { get; set; }
    public string Description { get; set; }
    public void AddToMenu(MenuPool menuPool, UIMenu trophyMainMenu, LosSantosRED.lsr.Interface.IModItems modItems, Residence trophyableLocation, TrophyInteract trophyInteract, LocationCamera locationCamera)
    {
        TrophyableLocation = trophyableLocation;
        TrophyInteract = trophyInteract;

        FinalTrophyPlacements = new List<TrophyPlacement>();
        foreach(TrophyPlacement trophyPlacement in TrophyableLocation.TrophyPlacements)
        {
            FinalTrophyPlacements.Add(new TrophyPlacement(trophyPlacement.SlotID,trophyPlacement.TrophyModelName));
        }
        SlotSubMenu = menuPool.AddSubMenu(trophyMainMenu, $"Slot {SlotID}");
        SlotSubMenu.OnMenuOpen += (sender) =>
        {
            //If No trophy, then spawn one, whatever is selected
            if(TrophyableLocation.TrophyPlacements.FirstOrDefault(x => x.SlotID == SlotID)?.IsSpawned == true)
            {

            }
            else
            {
                TrophyableLocation.TrophyPlacements.Add(new TrophyPlacement(SlotID, PossibleModelsScroller.SelectedItem.ModelItemID));
                TrophyableLocation.TrophyPlacements.FirstOrDefault(x => x.SlotID == SlotID)?.SpawnTrophy(trophyInteract.CabinetData);
            }

            locationCamera?.MoveToPosition(CameraPosition, CameraDirection, CameraRotation, true, false, false);
        };
        SlotSubMenu.OnMenuClose += (sender) =>
        {
            //Cleanup the current trophy if it exists
            //check if we had a trophy and spawn it
            TrophyPlacement finalTrophyPlacement = FinalTrophyPlacements.Where(x => x.SlotID == SlotID).FirstOrDefault();
            TrophyableLocation.TrophyPlacements.FirstOrDefault(x => x.SlotID == SlotID)?.DespawnTrophy();
            TrophyableLocation.TrophyPlacements.RemoveAll(x=> x.SlotID == SlotID);
            if (finalTrophyPlacement != null)
            {
                TrophyableLocation.TrophyPlacements.Add(new TrophyPlacement(finalTrophyPlacement.SlotID, finalTrophyPlacement.TrophyModelName));
                TrophyableLocation.TrophyPlacements.FirstOrDefault(x => x.SlotID == SlotID)?.SpawnTrophy(TrophyInteract.CabinetData);
            }
            locationCamera?.MoveToPosition(trophyInteract.CameraPosition, trophyInteract.CameraDirection, trophyInteract.CameraRotation, true, false, false);
        };

        //Only do unlocked trophies? need to be in inventory to be able to be displayed? mayube just display any item lol
        PossibleModelsScroller = new UIMenuListScrollerItem<TrophyItem>("Trophy", "Select Trophy for display in this slot", modItems.PossibleItems.TrophyItems);
        PossibleModelsScroller.IndexChanged += (sender,oldIndex,newIndex) =>
        {
            TrophyPlacement tp = TrophyableLocation.TrophyPlacements.FirstOrDefault(x => x.SlotID == SlotID);
            if(tp == null)
            {
                tp = new TrophyPlacement(SlotID, PossibleModelsScroller.SelectedItem.ModelItemID);
                TrophyableLocation.TrophyPlacements.Add(tp);
            }
            tp.SpawnPreviewTrophy(TrophyInteract.CabinetData, PossibleModelsScroller.SelectedItem.ModelItemID);
        };
        PossibleModelsScroller.Activated += (sender,selectedItem) =>
        {          
            FinalTrophyPlacements.RemoveAll(x => x.SlotID == SlotID);
            FinalTrophyPlacements.Add(new TrophyPlacement(SlotID, PossibleModelsScroller.SelectedItem.ModelItemID));
            Game.DisplaySubtitle($"Updated Trophy to {PossibleModelsScroller.SelectedItem.Name} in Slot {SlotID}");
        };
        SlotSubMenu.AddItem(PossibleModelsScroller);
    }
}