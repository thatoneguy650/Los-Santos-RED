using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ArmorManager
{
    private IArmorManageable Player;
    private ISettingsProvideable Settings;
    public bool HasOnBodyArmor => EquippedArmorItem != null;
    public BodyArmorItem EquippedArmorItem { get; private set; }
    public ArmorManager(IArmorManageable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public void Setup()
    {

    }
    public void Update()
    {

    }

    public void Dispose()
    {

    }
    public void Reset()
    {
        EquippedArmorItem = null;
    }
    public void UseArmor(BodyArmorItem equipmentItem)
    {
        RemoveArmor();
        AddArmorItem(equipmentItem);
        EquippedArmorItem = equipmentItem;
        Game.DisplaySubtitle($"Added Armor {equipmentItem.Name}");
    }
    private void AddArmorItem(BodyArmorItem equipmentItem)
    {
        SetArmor(equipmentItem.ArmorChangeAmount);
        if (!Player.CharacterModelIsFreeMode)
        {
            return;
        }
        if (Settings.SettingsManager.ActivitySettings.DisplayBodyArmor)
        {
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(Player.Character, 9, Settings.SettingsManager.ActivitySettings.BodyArmorDefaultDrawableID, Settings.SettingsManager.ActivitySettings.BodyArmorDefaultTextureID, 0);
        }
    }
    public void RemoveArmor()
    {
        if(!HasOnBodyArmor)
        {
            return;
        }
        Player.Inventory.Add(EquippedArmorItem, 1.0f);
        SetArmor(0);
        Game.DisplaySubtitle($"Removed Armor {EquippedArmorItem.Name}");
        if(!Player.CharacterModelIsFreeMode)
        {
            return;
        }
        if (Settings.SettingsManager.ActivitySettings.DisplayBodyArmor)
        {
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(Player.Character, 9, 0, 0, 0);
        }
    }
    public void ChangeArmor(int ToAdd)
    {
        if (Player.Character.Armor + ToAdd <= 0)
        {
            Player.Character.Armor = 0;
            return;
        }
        Player.Character.Armor += ToAdd;
    }
    public void SetArmor(int toset)
    {
        if (toset <= 0)
        {
            Player.Character.Armor = 0;
            return;
        }
        Player.Character.Armor = toset;
    }
    public void ChangeArmorVisual(int drawableID, int textureID)
    {
        if(HasOnBodyArmor && Settings.SettingsManager.ActivitySettings.DisplayBodyArmor)
        {
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(Player.Character, 9, drawableID, textureID, 0);
        }
    }
}

