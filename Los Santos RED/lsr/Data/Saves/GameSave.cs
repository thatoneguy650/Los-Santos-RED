using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
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
        public GameSave(string playerName, int money, string modelName,bool isMale, PedVariation currentModelVariation, List<StoredWeapon> weaponInventory, VehicleVariation vehicleVariation)
        {
            PlayerName = playerName;
            Money = money;
            ModelName = modelName;
            IsMale = isMale;
            CurrentModelVariation = currentModelVariation;
            WeaponInventory = weaponInventory;
            OwnedVehicleVariation = vehicleVariation;
        }
        public void Save(ISaveable player, IWeapons weapons)
        {
            PlayerName = player.PlayerName;
            ModelName = player.ModelName;
            Money = player.Money;
            IsMale = player.IsMale;
            CurrentModelVariation = player.CurrentModelVariation.Copy();
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
            //CurrentHeadBlendData = player.CurrentHeadBlendData;
            //CurrentPrimaryHairColor = player.CurrentPrimaryHairColor;
            //CurrentSecondaryColor = player.CurrentSecondaryColor;
            //CurrentHeadOverlays = player.CurrentHeadOverlays;
            if(player.OwnedVehicle != null && player.OwnedVehicle.Vehicle.Exists())
            {
                int primaryColor;
                int secondaryColor;
                unsafe
                {
                    NativeFunction.CallByName<int>("GET_VEHICLE_COLOURS", player.OwnedVehicle.Vehicle, &primaryColor, &secondaryColor);
                }
                OwnedVehicleVariation = new VehicleVariation(player.OwnedVehicle.Vehicle.Model.Name, primaryColor, secondaryColor, new LicensePlate(player.OwnedVehicle.CarPlate.PlateNumber,player.OwnedVehicle.CarPlate.PlateType,player.OwnedVehicle.CarPlate.IsWanted));
            }
        }
        public string PlayerName { get; set; }
        public int Money { get; set; }
        public string ModelName { get; set; }
        public bool IsMale { get; set; }
        //public int CurrentPrimaryHairColor { get; set; }
        //public int CurrentSecondaryColor { get; set; }
        //public List<HeadOverlayData> CurrentHeadOverlays { get; set; }
        //public HeadBlendData CurrentHeadBlendData { get; set; }
        public PedVariation CurrentModelVariation { get; set; }
        public List<StoredWeapon> WeaponInventory { get; set; }
        public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
        public VehicleVariation OwnedVehicleVariation { get; set; }
        public void Load(IWeapons weapons,IPedSwap pedSwap, IInventoryable player, ISettingsProvideable settings, IEntityProvideable World)
        {
            pedSwap.BecomeSavedPed(PlayerName, ModelName, Money, CurrentModelVariation);//, CurrentHeadBlendData, CurrentPrimaryHairColor, CurrentSecondaryColor, CurrentHeadOverlays);



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




            if(OwnedVehicleVariation != null)
            {
                NativeHelper.GetStreetPositionandHeading(Game.LocalPlayer.Character.Position,out Vector3 SpawnPos, out float Heading,false);
                if (SpawnPos != Vector3.Zero)
                {
                    Vehicle NewVehicle = new Vehicle(OwnedVehicleVariation.ModelName, SpawnPos, Heading);
                    if (NewVehicle.Exists())
                    {
                        NewVehicle.LicensePlate = OwnedVehicleVariation.LicensePlate.PlateNumber;
                        NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX(NewVehicle,OwnedVehicleVariation.LicensePlate.PlateType);                     
                        NativeFunction.Natives.SET_VEHICLE_COLOURS(NewVehicle, OwnedVehicleVariation.PrimaryColor, OwnedVehicleVariation.SecondaryColor);
                        NewVehicle.Wash();
                        VehicleExt MyNewCar = new VehicleExt(NewVehicle, settings);
                        World.AddEntity(MyNewCar, ResponseType.None);
                        player.TakeOwnershipOfVehicle(MyNewCar);
                    }
                }
            }

        }

        public override string ToString()
        {
            return $"{PlayerName}";//base.ToString();
        }
    }

}
