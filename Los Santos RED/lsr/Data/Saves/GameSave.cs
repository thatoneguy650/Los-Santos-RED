using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
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
            foreach (WeaponDescriptor wd in Game.LocalPlayer.Character.Inventory.Weapons)
            {
                WeaponInventory.Add(new StoredWeapon((uint)wd.Hash,Vector3.Zero,weapons.GetWeaponVariation(Game.LocalPlayer.Character,(uint)wd.Hash),wd.Ammo));
            }
        }

        public string PlayerName { get; set; }
        public int Money { get; set; }
        public string ModelName { get; set; }
        public bool IsMale { get; set; }
        public uint OwnedVehicleHandle { get; set; }
        public PedVariation CurrentModelVariation { get; set; }
        public List<StoredWeapon> WeaponInventory { get; set; }
        public void Load(IWeapons weapons)
        {
            LoadModel();
            Mod.Player currentPlayer = EntryPoint.ModController.NewPlayer(ModelName, IsMale, PlayerName, Money);
            ResetPlayerState();
            SetPlayerStats();
            WeaponDescriptorCollection PlayerWeapons = Game.LocalPlayer.Character.Inventory.Weapons;
            foreach (StoredWeapon MyOldGuns in WeaponInventory)
            {
                currentPlayer.Character.Inventory.GiveNewWeapon(MyOldGuns.WeaponHash, (short)MyOldGuns.Ammo, false);
                if (PlayerWeapons.Contains(MyOldGuns.WeaponHash))
                {
                    WeaponInformation Gun2 = weapons.GetWeapon((uint)MyOldGuns.WeaponHash);
                    if (Gun2 != null)
                    {
                        Gun2.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)MyOldGuns.WeaponHash, MyOldGuns.Variation);
                    }
                   // NativeFunction.CallByName<bool>("ADD_AMMO_TO_PED", Game.LocalPlayer.Character, (uint)MyOldGuns.WeaponHash, MyOldGuns.Ammo + 1);
                }
            }
        }
        private void LoadModel()
        {
            NativeHelper.SetAsMainPlayer();
            NativeHelper.ChangeModel("player_zero");
            NativeHelper.ChangeModel(ModelName);
            if (CurrentModelVariation != null)
            {
                CurrentModelVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);
            }        
        }
        private void ResetPlayerState()
        {     
            NativeFunction.Natives.CLEAR_TIMECYCLE_MODIFIER<int>();
            NativeFunction.Natives.x80C8B1846639BB19(0);
            NativeFunction.Natives.STOP_GAMEPLAY_CAM_SHAKING<int>(true);
            Game.LocalPlayer.Character.Inventory.Weapons.Clear();
            Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
            Game.TimeScale = 1f;
            NativeFunction.Natives.xB4EDDC19532BFB85();
            Game.HandleRespawn();
            NativeFunction.Natives.NETWORK_REQUEST_CONTROL_OF_ENTITY<bool>(Game.LocalPlayer.Character);
            NativeFunction.Natives.xC0AA53F866B3134D();
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
        }
        private void SetPlayerStats()
        {

        }
        public override string ToString()
        {
            return $"{PlayerName}";//base.ToString();
        }
    }

}
