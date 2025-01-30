using Rage;
using Rage.Native;

public class CraftInteriortInteract : InteriorInteract
{
    public string CraftingFlag { get; set; }
    public CraftInteriortInteract()
    {
    }
    public CraftInteriortInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {
    }
    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        Interior.IsMenuInteracting = false;
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        LocationInteractable.CraftingFlags.Add(CraftingFlag);
        bool IsCancelled = false;
        while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
        {
            Player.WeaponEquipment.SetUnarmed();
            if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            {
                IsCancelled = true;
                LocationInteractable.CraftingFlags.Remove(CraftingFlag);
                break;
            }
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