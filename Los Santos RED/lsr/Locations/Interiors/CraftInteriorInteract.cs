using Rage;
using Rage.Native;

public class CraftInteriorInteract : InteriorInteract
{
    public string CraftingFlag { get; set; }
    public CraftInteriorInteract()
    {
    }
    public CraftInteriorInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {
    }
    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        //Interior.IsMenuInteracting = false;
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        LocationInteractable.Crafting.CraftingMenu.Show(CraftingFlag);
        bool IsCancelled = false;
        while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
        {
            //Player.WeaponEquipment.SetUnarmed();
            //if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            //{
            //    IsCancelled = true;
            //    LocationInteractable.Crafting.CraftingMenu.Hide();
            //    break;
            //}
            GameFiber.Yield();
        }
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