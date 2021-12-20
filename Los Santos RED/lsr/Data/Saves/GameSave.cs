using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Data
{
    public class GameSave
    {
        public GameSave()
        {

        }
        public GameSave(string playerName, int money, string modelName,bool isMale, uint ownedVehicleHandle, PedVariation currentModelVariation, List<StoredWeapon> weaponInventory)
        {
            PlayerName = playerName;
            Money = money;
            ModelName = modelName;
            IsMale = isMale;
            OwnedVehicleHandle = ownedVehicleHandle;
            CurrentModelVariation = currentModelVariation;
            WeaponInventory = weaponInventory;
        }

        public void Save(ISaveable player, IWeapons weapons)
        {
            PlayerName = player.PlayerName;
            ModelName = player.ModelName;
            Money = player.Money;
            IsMale = player.IsMale;
            CurrentModelVariation = player.CurrentModelVariation;
            WeaponInventory = new List<StoredWeapon>();
            InventoryItems.Clear();
            foreach (InventoryItem cii in player.Inventory.Items)
            {
                InventoryItems.Add(new InventoryItem(cii.ModItem, cii.Amount));
            }
            foreach (WeaponDescriptor wd in Game.LocalPlayer.Character.Inventory.Weapons)
            {
                WeaponInventory.Add(new StoredWeapon((uint)wd.Hash, Vector3.Zero, weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)wd.Hash), wd.Ammo));
            }
            CurrentHeadBlendData = player.CurrentHeadBlendData;
            CurrentPrimaryHairColor = player.CurrentPrimaryHairColor;
            CurrentSecondaryColor = player.CurrentSecondaryColor;
            CurrentHeadOverlays = player.CurrentHeadOverlays;
        }
        public string PlayerName { get; set; }
        public int Money { get; set; }
        public string ModelName { get; set; }
        public bool IsMale { get; set; }
        public uint OwnedVehicleHandle { get; set; }
        public int CurrentPrimaryHairColor { get; set; }
        public int CurrentSecondaryColor { get; set; }
        public List<HeadOverlay> CurrentHeadOverlays { get; set; }
        public HeadBlendData CurrentHeadBlendData { get; set; }
        public PedVariation CurrentModelVariation { get; set; }
        public List<StoredWeapon> WeaponInventory { get; set; }
        public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
        public void Load(IWeapons weapons,IPedSwap pedSwap, IInventoryable player)
        {
            pedSwap.BecomeSavedPed(PlayerName, IsMale, Money, ModelName, CurrentModelVariation, CurrentHeadBlendData, CurrentPrimaryHairColor, CurrentSecondaryColor, CurrentHeadOverlays);
            WeaponDescriptorCollection PlayerWeapons = Game.LocalPlayer.Character.Inventory.Weapons;
            foreach (StoredWeapon MyOldGuns in WeaponInventory)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(MyOldGuns.WeaponHash, (short)MyOldGuns.Ammo, false);
                if (PlayerWeapons.Contains(MyOldGuns.WeaponHash))
                {
                    WeaponInformation Gun2 = weapons.GetWeapon((uint)MyOldGuns.WeaponHash);
                    if (Gun2 != null)
                    {
                        Gun2.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)MyOldGuns.WeaponHash, MyOldGuns.Variation);
                    }
                }
            }
            player.Inventory.Clear();
            foreach (InventoryItem cii in InventoryItems)
            {
                player.Inventory.Add(cii.ModItem, cii.Amount);
            }
        }

        public override string ToString()
        {
            return $"{PlayerName}";//base.ToString();
        }
    }

}
