using ExtensionsMethods;
using iFruitAddon2;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public void Save(ISaveable player, IWeapons weapons, ITimeReportable time, IPlacesOfInterest placesOfInterest)
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
                InventoryItems.Add(new InventorySave(cii.ModItem.Name, cii.RemainingPercent));
            }
            foreach (WeaponDescriptor wd in Game.LocalPlayer.Character.Inventory.Weapons)
            {
                WeaponInventory.Add(new StoredWeapon((uint)wd.Hash, Vector3.Zero, weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)wd.Hash), wd.Ammo));
            }
            OwnedVehicleVariations.Clear();
            foreach (VehicleExt car in player.OwnedVehicles)
            {
                if (car.Vehicle.Exists())
                {
                    int primaryColor;
                    int secondaryColor;
                    unsafe
                    {
                        NativeFunction.CallByName<int>("GET_VEHICLE_COLOURS", car.Vehicle, &primaryColor, &secondaryColor);
                    }


                    uint modelHash;
                    var hex = car.VehicleModelName.ToLower();
                    if (hex.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase) || hex.StartsWith("&H", StringComparison.CurrentCultureIgnoreCase))
                    {
                        hex = hex.Substring(2);
                    }
                    bool parsedSuccessfully = uint.TryParse(hex,NumberStyles.HexNumber,CultureInfo.CurrentCulture, out modelHash);

                    EntryPoint.WriteToConsole($"STRIPPED NAME {parsedSuccessfully} hex {hex} {modelHash}");
                    if (parsedSuccessfully)//uint.TryParse(car.VehicleModelName.ToLower().Replace("0x",""), out uint modelHash))
                    {
                        OwnedVehicleVariations.Add(new VehicleVariation(modelHash, primaryColor, secondaryColor, new LicensePlate(car.CarPlate.PlateNumber, car.CarPlate.PlateType, car.CarPlate.IsWanted), car.Vehicle.Position, car.Vehicle.Heading));
                    }
                    else
                    {
                        OwnedVehicleVariations.Add(new VehicleVariation(car.VehicleModelName, primaryColor, secondaryColor, new LicensePlate(car.CarPlate.PlateNumber, car.CarPlate.PlateType, car.CarPlate.IsWanted), car.Vehicle.Position, car.Vehicle.Heading));
                    }
                }
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
            foreach(GangReputation gr in player.GangRelationships.GangReputations)
            {
                GangReputations.Add(new GangRepSave(gr.Gang.ID, gr.ReputationLevel, gr.MembersHurt,gr.MembersKilled,gr.MembersCarJacked,gr.MembersHurtInTerritory,gr.MembersKilledInTerritory,gr.MembersCarJackedInTerritory,gr.PlayerDebt));
            }

            Contacts = new List<SavedContact>();
            foreach (iFruitContact ifc in player.CellPhone.ContactList)
            {
                Contacts.Add(new SavedContact(ifc.Name, ifc.Index, ""));
            }
            TextMessages = new List<SavedTextMessage>();
            foreach (iFruitText ifc in player.CellPhone.TextList)
            {
                TextMessages.Add(new SavedTextMessage(ifc.Name, ifc.Message,ifc.HourSent,ifc.MinuteSent,ifc.IsRead,ifc.Index, ""));
            }
            CurrentDateTime = time.CurrentDateTime;


            UndergroundGunsMoneySpent = player.GunDealerRelationship.TotalMoneySpentAtShops;
            UndergroundGunsDebt = player.GunDealerRelationship.PlayerDebt;
            UndergroundGunsReputation = player.GunDealerRelationship.ReputationLevel;

            OfficerFriendlyMoneySpent = player.OfficerFriendlyRelationship.TotalMoneySpentOnBribes;
            OfficerFriendlyDebt = player.OfficerFriendlyRelationship.PlayerDebt;
            OfficerFriendlyReputation = player.OfficerFriendlyRelationship.ReputationLevel;


            PlayerPosition = player.Character.Position;
            PlayerHeading = player.Character.Heading;

            if (player.Licenses.HasDriversLicense)
            {
                DriversLicense = new DriversLicense() { ExpirationDate = player.Licenses.DriversLicense.ExpirationDate, IssueDate = player.Licenses.DriversLicense.IssueDate };
            }
            if (player.Licenses.HasCCWLicense)
            {
                CCWLicense = new CCWLicense() { ExpirationDate = player.Licenses.CCWLicense.ExpirationDate, IssueDate = player.Licenses.CCWLicense.IssueDate };
            }
            SavedResidences.Clear();
            foreach (Residence res in player.Properties.Residences)//placesOfInterest.PossibleLocations.Residences)
            {
                if(res.IsOwned || res.IsRented)
                {
                    SavedResidence myRes = new SavedResidence(res.Name, res.IsOwned, res.IsRented);
                    if(res.IsRented)
                    {
                        myRes.DateOfLastRentalPayment = res.DateRentalPaymentPaid;
                        myRes.RentalPaymentDate = res.DateRentalPaymentDue;
                    }
                    SavedResidences.Add(myRes);
                }
            }

        }
        public Vector3 PlayerPosition { get; set; }
        public float PlayerHeading { get; set; }
        public string PlayerName { get; set; }
        public int Money { get; set; }
        public string ModelName { get; set; }
        public bool IsMale { get; set; }

        public DateTime CurrentDateTime { get; set; }


        public DriversLicense DriversLicense { get; set; }
        public CCWLicense CCWLicense { get; set; }

        public List<SavedTextMessage> TextMessages { get; set; } = new List<SavedTextMessage>();
        public List<SavedContact> Contacts { get; set; } = new List<SavedContact>();

        public List<GangRepSave> GangReputations { get; set; } = new List<GangRepSave>();
        public PedVariation CurrentModelVariation { get; set; }
        public List<StoredWeapon> WeaponInventory { get; set; }
        public List<InventorySave> InventoryItems { get; set; } = new List<InventorySave>();
        public List<VehicleVariation> OwnedVehicleVariations { get; set; } = new List<VehicleVariation>();

        public List<SavedResidence> SavedResidences { get; set; } = new List<SavedResidence>();
        public int UndergroundGunsMoneySpent { get; set; }
        public int UndergroundGunsDebt { get; set; }
        public int UndergroundGunsReputation { get; set; }

        public int OfficerFriendlyMoneySpent { get; set; }
        public int OfficerFriendlyDebt { get; set; }
        public int OfficerFriendlyReputation { get; set; }

        public void Load(IWeapons weapons,IPedSwap pedSwap, IInventoryable player, ISettingsProvideable settings, IEntityProvideable World, IGangs gangs, ITimeControllable time, IPlacesOfInterest placesOfInterest, IModItems modItems)
        {
            try
            {
                Game.FadeScreenOut(1500, true);
                time.SetDateTime(CurrentDateTime);
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
                foreach (InventorySave cii in InventoryItems)
                {
                    player.Inventory.Add(modItems.Get(cii.ModItemName), (int)cii.RemainingPercent);
                }
                player.ClearVehicleOwnership();
                foreach (VehicleVariation OwnedVehicleVariation in OwnedVehicleVariations)
                {
                    NativeHelper.GetStreetPositionandHeading(Game.LocalPlayer.Character.Position, out Vector3 SpawnPos, out float Heading, false);
                    if (SpawnPos != Vector3.Zero)
                    {
                        Vehicle NewVehicle = null;
                        if (OwnedVehicleVariation.ModelName != "")
                        {
                            NewVehicle = new Vehicle(OwnedVehicleVariation.ModelName, SpawnPos, Heading);
                        }
                        else if(OwnedVehicleVariation.ModelHash != 0)
                        {
                            NewVehicle = new Vehicle(OwnedVehicleVariation.ModelHash, SpawnPos, Heading);
                        }

                        if (NewVehicle.Exists())
                        {
                            NewVehicle.LicensePlate = OwnedVehicleVariation.LicensePlate.PlateNumber;
                            NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX(NewVehicle, OwnedVehicleVariation.LicensePlate.PlateType);
                            if (OwnedVehicleVariation.PrimaryColor != -1)
                            {
                                NativeFunction.Natives.SET_VEHICLE_COLOURS(NewVehicle, OwnedVehicleVariation.PrimaryColor, OwnedVehicleVariation.SecondaryColor);
                            }
                            NewVehicle.Wash();
                            VehicleExt MyVeh = World.Vehicles.GetVehicleExt(NewVehicle.Handle);
                            if (MyVeh == null)
                            {
                                MyVeh = new VehicleExt(NewVehicle, settings);
                                MyVeh.HasUpdatedPlateType = true;
                                World.Vehicles.AddEntity(MyVeh, ResponseType.None);
                            }
                            //VehicleExt MyNewCar = new VehicleExt(NewVehicle, settings);
                            player.TakeOwnershipOfVehicle(MyVeh,false);
                            if (OwnedVehicleVariation.LastPosition != Vector3.Zero)
                            {
                                NewVehicle.Position = OwnedVehicleVariation.LastPosition;
                                NewVehicle.Heading = OwnedVehicleVariation.LastHeading;
                            }
                        }
                    }
                }

                foreach(GangRepSave tuple in GangReputations)
                {
                    Gang myGang = gangs.GetGang(tuple.GangID);
                    if (myGang != null)
                    {
                        player.GangRelationships.SetReputation(myGang, tuple.Reputation, false);
                        player.GangRelationships.SetStats(myGang, tuple.MembersHurt, tuple.MembersHurtInTerritory, tuple.MembersKilled, tuple.MembersKilledInTerritory, tuple.MembersCarJacked, tuple.MembersCarJackedInTerritory, tuple.PlayerDebt);
                    }
                }
                foreach (SavedContact ifc in Contacts.OrderBy(x=> x.Index))
                {
                    Gang gang = gangs.GetGangByContact(ifc.Name);
                    if (ifc.Name == EntryPoint.UndergroundGunsContactName)
                    {
                        player.CellPhone.AddGunDealerContact(false);
                    }
                    else if (gang != null)
                    {
                        player.CellPhone.AddContact(gang, false);
                    }
                    else
                    {
                        player.CellPhone.AddContact(ifc.Name, ifc.IconName, false);
                    }
                
                }

                foreach (SavedTextMessage ifc in TextMessages)
                {
                    player.CellPhone.AddText(ifc.Name,ifc.IconName,ifc.Message,ifc.HourSent,ifc.MinuteSent, ifc.IsRead);
                }

                if (PlayerPosition != Vector3.Zero)
                {
                    player.Character.Position = PlayerPosition;
                    player.Character.Heading = PlayerHeading;
                }

                player.GunDealerRelationship.SetMoneySpent(UndergroundGunsMoneySpent,false);
                player.GunDealerRelationship.SetDebt(UndergroundGunsDebt);
                player.GunDealerRelationship.SetReputation(UndergroundGunsReputation, false);


                player.OfficerFriendlyRelationship.SetMoneySpent(OfficerFriendlyMoneySpent, false);
                player.OfficerFriendlyRelationship.SetDebt(OfficerFriendlyDebt);
                player.OfficerFriendlyRelationship.SetReputation(OfficerFriendlyReputation, false);



                if (DriversLicense != null)
                {
                    player.Licenses.DriversLicense = new DriversLicense() { ExpirationDate = DriversLicense.ExpirationDate, IssueDate = DriversLicense.IssueDate };
                }
                if (CCWLicense != null)
                {
                    player.Licenses.CCWLicense = new CCWLicense() { ExpirationDate = CCWLicense.ExpirationDate, IssueDate = CCWLicense.IssueDate };
                }

                foreach (SavedResidence res in SavedResidences)
                {
                    if (res.IsOwnedByPlayer || res.IsRentedByPlayer)
                    {
                        Residence savedPlace = placesOfInterest.PossibleLocations.Residences.Where(x => x.Name == res.Name).FirstOrDefault();
                        if(savedPlace != null)
                        {
                            player.Properties.AddResidence(savedPlace);
                            savedPlace.IsOwned = res.IsOwnedByPlayer;
                            savedPlace.IsRented = res.IsRentedByPlayer;
                            savedPlace.DateRentalPaymentDue = res.RentalPaymentDate;
                            savedPlace.DateRentalPaymentPaid = res.DateOfLastRentalPayment;
                            savedPlace.RefreshUI();
                        }
                    }
                }



                Game.FadeScreenIn(1500, true);
                player.DisplayPlayerNotification();
            }
            catch (Exception e)
            {
                Game.FadeScreenIn(0);
                Game.DisplayNotification("Error Loading Save");
            }
        }

        public override string ToString()
        {
            return $"{PlayerName}";//base.ToString();
        }
    }

}
