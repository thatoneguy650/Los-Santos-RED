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
        public GameSave(string playerName, int money, string modelName,bool isMale, PedVariation currentModelVariation, List<StoredWeapon> weaponInventory, List<VehicleVariation> vehicleVariations)
        {
            PlayerName = playerName;
            Money = money;
            ModelName = modelName;
            IsMale = isMale;
            CurrentModelVariation = currentModelVariation;
            WeaponInventory = weaponInventory;
            OwnedVehicleVariations = vehicleVariations;
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
            OwnedVehicleVariations.Clear();
            foreach (VehicleExt car in player.OwnedVehicles)
            {
                int primaryColor;
                int secondaryColor;
                unsafe
                {
                    NativeFunction.CallByName<int>("GET_VEHICLE_COLOURS", car.Vehicle, &primaryColor, &secondaryColor);
                }
                OwnedVehicleVariations.Add(new VehicleVariation(car.Vehicle.Model.Name, primaryColor, secondaryColor, new LicensePlate(car.CarPlate.PlateNumber, car.CarPlate.PlateType, car.CarPlate.IsWanted), car.Vehicle.Position, car.Vehicle.Heading));
            }

            //if(player.OwnedVehicle != null && player.OwnedVehicle.Vehicle.Exists())
            //{
            //    int primaryColor;
            //    int secondaryColor;
            //    unsafe
            //    {
            //        NativeFunction.CallByName<int>("GET_VEHICLE_COLOURS", player.OwnedVehicle.Vehicle, &primaryColor, &secondaryColor);
            //    }
            //    OwnedVehicleVariation = new VehicleVariation(player.OwnedVehicle.Vehicle.Model.Name, primaryColor, secondaryColor, new LicensePlate(player.OwnedVehicle.CarPlate.PlateNumber,player.OwnedVehicle.CarPlate.PlateType,player.OwnedVehicle.CarPlate.IsWanted), player.OwnedVehicle.Vehicle.Position, player.OwnedVehicle.Vehicle.Heading);
            //}
            GangReputations = new List<GangRepSave>();
            foreach(GangReputation gr in player.GangReputations)
            {
                GangReputations.Add(new GangRepSave(gr.Gang.ID, gr.ReputationLevel));
            }
            PlayerPosition = player.Character.Position;
            PlayerHeading = player.Character.Heading;
        }
        public Vector3 PlayerPosition { get; set; }
        public float PlayerHeading { get; set; }
        public string PlayerName { get; set; }
        public int Money { get; set; }
        public string ModelName { get; set; }
        public bool IsMale { get; set; }
        public List<GangRepSave> GangReputations { get; set; } = new List<GangRepSave>();
        public PedVariation CurrentModelVariation { get; set; }
        public List<StoredWeapon> WeaponInventory { get; set; }
        public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
        public List<VehicleVariation> OwnedVehicleVariations { get; set; } = new List<VehicleVariation>();
        public void Load(IWeapons weapons,IPedSwap pedSwap, IInventoryable player, ISettingsProvideable settings, IEntityProvideable World, IGangs gangs)
        {
            Game.FadeScreenOut(2500, true);


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
                        Gun2.ApplyWeaponVariation(Game.LocalPlayer.Character, MyOldGuns.Variation);
                    }
                }
            }
            player.Inventory.Clear();
            foreach (InventoryItem cii in InventoryItems)
            {
                player.Inventory.Add(cii.ModItem, cii.Amount);
            }

            player.ClearVehicleOwnership();

            foreach (VehicleVariation OwnedVehicleVariation in OwnedVehicleVariations)
            {
                NativeHelper.GetStreetPositionandHeading(Game.LocalPlayer.Character.Position, out Vector3 SpawnPos, out float Heading, false);
                if (SpawnPos != Vector3.Zero)
                {
                    Vehicle NewVehicle = new Vehicle(OwnedVehicleVariation.ModelName, SpawnPos, Heading);
                    if (NewVehicle.Exists())
                    {
                        NewVehicle.LicensePlate = OwnedVehicleVariation.LicensePlate.PlateNumber;
                        NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX(NewVehicle, OwnedVehicleVariation.LicensePlate.PlateType);
                        NativeFunction.Natives.SET_VEHICLE_COLOURS(NewVehicle, OwnedVehicleVariation.PrimaryColor, OwnedVehicleVariation.SecondaryColor);
                        NewVehicle.Wash();
                        VehicleExt MyVeh = World.GetVehicleExt(NewVehicle.Handle);
                        if (MyVeh == null)
                        {
                            MyVeh = new VehicleExt(NewVehicle, settings);
                            MyVeh.HasUpdatedPlateType = true;
                            World.AddEntity(MyVeh, ResponseType.None);
                        }
                        //VehicleExt MyNewCar = new VehicleExt(NewVehicle, settings);
                        player.TakeOwnershipOfVehicle(MyVeh);


                        if (OwnedVehicleVariation.LastPosition != Vector3.Zero)
                        {
                            NewVehicle.Position = OwnedVehicleVariation.LastPosition;
                            NewVehicle.Heading = OwnedVehicleVariation.LastHeading;
                        }


                    }
                }
            }

            //if(OwnedVehicleVariation != null)
            //{
            //    NativeHelper.GetStreetPositionandHeading(Game.LocalPlayer.Character.Position,out Vector3 SpawnPos, out float Heading,false);
            //    if (SpawnPos != Vector3.Zero)
            //    {
            //        Vehicle NewVehicle = new Vehicle(OwnedVehicleVariation.ModelName, SpawnPos, Heading);
            //        if (NewVehicle.Exists())
            //        {
            //            NewVehicle.LicensePlate = OwnedVehicleVariation.LicensePlate.PlateNumber;
            //            NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX(NewVehicle,OwnedVehicleVariation.LicensePlate.PlateType);                     
            //            NativeFunction.Natives.SET_VEHICLE_COLOURS(NewVehicle, OwnedVehicleVariation.PrimaryColor, OwnedVehicleVariation.SecondaryColor);
            //            NewVehicle.Wash();
            //            VehicleExt MyVeh = World.GetVehicleExt(NewVehicle.Handle);
            //            if (MyVeh == null)
            //            {
            //                MyVeh = new VehicleExt(NewVehicle, settings);
            //                MyVeh.HasUpdatedPlateType = true;
            //                World.AddEntity(MyVeh, ResponseType.None);
            //            }
            //            //VehicleExt MyNewCar = new VehicleExt(NewVehicle, settings);
            //            player.TakeOwnershipOfVehicle(MyVeh);


            //            if (OwnedVehicleVariation.LastPosition != Vector3.Zero)
            //            {
            //                NewVehicle.Position = OwnedVehicleVariation.LastPosition;
            //                NewVehicle.Heading = OwnedVehicleVariation.LastHeading;
            //            }


            //        }
            //    }
            //}

            foreach(GangRepSave tuple in GangReputations)
            {
                Gang myGang = gangs.GetGang(tuple.GangID);
                if (myGang != null)
                {
                    player.SetReputation(myGang, tuple.Reputation);
                }
            }
            if (PlayerPosition != Vector3.Zero)
            {
                player.Character.Position = PlayerPosition;
                player.Character.Heading = PlayerHeading;
            }
            Game.FadeScreenIn(2500, true);
            player.DisplayPlayerNotification();
        }

        public override string ToString()
        {
            return $"{PlayerName}";//base.ToString();
        }
    }

}
