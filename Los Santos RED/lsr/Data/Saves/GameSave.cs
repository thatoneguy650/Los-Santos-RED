﻿using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI.PauseMenu;
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
        public GameSave(string playerName, int money, string modelName,bool isMale, PedVariation currentModelVariation, List<StoredWeapon> weaponInventory, List<VehicleSaveStatus> vehicleVariations)
        {
            PlayerName = playerName;
            Money = money;
            ModelName = modelName;
            IsMale = isMale;
            CurrentModelVariation = currentModelVariation;
            WeaponInventory = weaponInventory;
            OwnedVehicleVariations = vehicleVariations;
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
        public PilotsLicense PilotsLicense { get; set; }
        public List<SavedTextMessage> TextMessages { get; set; } = new List<SavedTextMessage>();
        public List<SavedContact> Contacts { get; set; } = new List<SavedContact>();
        public List<GangRepSave> GangReputationsSave { get; set; } = new List<GangRepSave>();
        public PedVariation CurrentModelVariation { get; set; }
        public List<StoredWeapon> WeaponInventory { get; set; }
        public List<InventorySave> InventoryItems { get; set; } = new List<InventorySave>();
        public List<VehicleSaveStatus> OwnedVehicleVariations { get; set; } = new List<VehicleSaveStatus>();
        public List<SavedResidence> SavedResidences { get; set; } = new List<SavedResidence>();
        public int UndergroundGunsMoneySpent { get; set; }
        public int UndergroundGunsDebt { get; set; }
        public int UndergroundGunsReputation { get; set; }
        public int OfficerFriendlyMoneySpent { get; set; }
        public int OfficerFriendlyDebt { get; set; }
        public int OfficerFriendlyReputation { get; set; }
        public float HungerValue { get; set; }
        public float ThirstValue { get; set; }
        public float SleepValue { get; set; }
        public int SpeechSkill { get; set; }
        public GangKickSave GangKickSave { get; set; }
        public bool IsCop { get; set; }
        public void Save(ISaveable player, IWeapons weapons, ITimeReportable time, IPlacesOfInterest placesOfInterest)
        {
            PlayerName = player.PlayerName;
            ModelName = player.ModelName;
            Money = player.BankAccounts.Money;
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
            foreach (VehicleExt car in player.VehicleOwnership.OwnedVehicles)
            {
                if (car.Vehicle.Exists())
                {
                    uint modelHash;
                    var hex = car.VehicleModelName.ToLower();
                    if (hex.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase) || hex.StartsWith("&H", StringComparison.CurrentCultureIgnoreCase))
                    {
                        hex = hex.Substring(2);
                    }
                    bool parsedSuccessfully = uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out modelHash);
                    VehicleSaveStatus vss;
                    if (parsedSuccessfully)//uint.TryParse(car.VehicleModelName.ToLower().Replace("0x",""), out uint modelHash))
                    {
                        vss = new VehicleSaveStatus(modelHash, car.Vehicle.Position, car.Vehicle.Heading);
                    }
                    else
                    {
                        vss = new VehicleSaveStatus(car.VehicleModelName, car.Vehicle.Position, car.Vehicle.Heading);
                    }
                    vss.VehicleVariation = NativeHelper.GetVehicleVariation(car.Vehicle);
                    OwnedVehicleVariations.Add(vss);
                }
            }
            GangReputationsSave = new List<GangRepSave>();
            foreach (GangReputation gr in player.RelationshipManager.GangRelationships.GangReputations)
            {
                GangReputationsSave.Add(new GangRepSave(gr.Gang.ID, gr.ReputationLevel, gr.MembersHurt, gr.MembersKilled, gr.MembersCarJacked, gr.MembersHurtInTerritory, gr.MembersKilledInTerritory, gr.MembersCarJackedInTerritory, gr.PlayerDebt, gr.IsMember, gr.IsEnemy));
            }

            if(player.RelationshipManager.GangRelationships.CurrentGang != null && player.RelationshipManager.GangRelationships.CurrentGangKickUp != null)
            {
                GangKickSave = new GangKickSave(player.RelationshipManager.GangRelationships.CurrentGang.ID, player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueDate, player.RelationshipManager.GangRelationships.CurrentGangKickUp.MissedPeriods, player.RelationshipManager.GangRelationships.CurrentGangKickUp.MissedAmount);
            }

            Contacts = new List<SavedContact>();
            foreach (PhoneContact ifc in player.CellPhone.ContactList)
            {
                Contacts.Add(new SavedContact(ifc.Name, ifc.Index, ifc.IconName));
            }
            TextMessages = new List<SavedTextMessage>();
            foreach (PhoneText ifc in player.CellPhone.TextList)
            {
                TextMessages.Add(new SavedTextMessage(ifc.ContactName, ifc.Message, ifc.HourSent, ifc.MinuteSent, ifc.IsRead, ifc.Index, ifc.IconName));
            }
            CurrentDateTime = time.CurrentDateTime;
            UndergroundGunsMoneySpent = player.RelationshipManager.GunDealerRelationship.TotalMoneySpentAtShops;
            UndergroundGunsDebt = player.RelationshipManager.GunDealerRelationship.PlayerDebt;
            UndergroundGunsReputation = player.RelationshipManager.GunDealerRelationship.ReputationLevel;
            OfficerFriendlyMoneySpent = player.RelationshipManager.OfficerFriendlyRelationship.TotalMoneySpentOnBribes;
            OfficerFriendlyDebt = player.RelationshipManager.OfficerFriendlyRelationship.PlayerDebt;
            OfficerFriendlyReputation = player.RelationshipManager.OfficerFriendlyRelationship.ReputationLevel;
            PlayerPosition = player.Character.Position;
            PlayerHeading = player.Character.Heading;


            HungerValue = player.HumanState.Hunger.CurrentValue;
            SleepValue = player.HumanState.Sleep.CurrentValue;
            ThirstValue = player.HumanState.Thirst.CurrentValue;

            if (player.Licenses.HasDriversLicense)
            {
                DriversLicense = new DriversLicense() { ExpirationDate = player.Licenses.DriversLicense.ExpirationDate, IssueDate = player.Licenses.DriversLicense.IssueDate };
            }
            if (player.Licenses.HasCCWLicense)
            {
                CCWLicense = new CCWLicense() { ExpirationDate = player.Licenses.CCWLicense.ExpirationDate, IssueDate = player.Licenses.CCWLicense.IssueDate };
            }
            if (player.Licenses.HasPilotsLicense)
            {
                PilotsLicense = new PilotsLicense() { ExpirationDate = player.Licenses.PilotsLicense.ExpirationDate, IssueDate = player.Licenses.PilotsLicense.IssueDate, IsFixedWingEndorsed = player.Licenses.PilotsLicense.IsFixedWingEndorsed, IsRotaryEndorsed = player.Licenses.PilotsLicense.IsRotaryEndorsed, IsLighterThanAirEndorsed = player.Licenses.PilotsLicense.IsLighterThanAirEndorsed };
            }



            SavedResidences.Clear();
            foreach (Residence res in player.Properties.Residences)//placesOfInterest.PossibleLocations.Residences)
            {
                if (res.IsOwned || res.IsRented)
                {
                    SavedResidence myRes = new SavedResidence(res.Name, res.IsOwned, res.IsRented);
                    if (res.IsRented)
                    {
                        myRes.DateOfLastRentalPayment = res.DateRentalPaymentPaid;
                        myRes.RentalPaymentDate = res.DateRentalPaymentDue;
                    }
                    SavedResidences.Add(myRes);
                }
            }
            SpeechSkill = player.SpeechSkill;
            IsCop = player.IsCop;
        }
        public void Load(IWeapons weapons,IPedSwap pedSwap, IInventoryable player, ISettingsProvideable settings, IEntityProvideable World, IGangs gangs, ITimeControllable time, IPlacesOfInterest placesOfInterest, IModItems modItems)
        {
            try
            {
                Game.FadeScreenOut(1500, true);
                time.SetDateTime(CurrentDateTime);
                pedSwap.BecomeSavedPed(PlayerName, ModelName, Money, CurrentModelVariation, SpeechSkill);//, CurrentHeadBlendData, CurrentPrimaryHairColor, CurrentSecondaryColor, CurrentHeadOverlays);

                WeaponDescriptorCollection PlayerWeapons = Game.LocalPlayer.Character.Inventory.Weapons;
                foreach (StoredWeapon MyOldGuns in WeaponInventory)
                {
                    Game.LocalPlayer.Character.Inventory.GiveNewWeapon(MyOldGuns.WeaponHash, 0, false);
                    if (PlayerWeapons.Contains(MyOldGuns.WeaponHash))
                    {
                        WeaponInformation Gun2 = weapons.GetWeapon((uint)MyOldGuns.WeaponHash);
                        if (Gun2 != null)
                        {
                            Gun2.ApplyWeaponVariation(Game.LocalPlayer.Character, MyOldGuns.Variation);




                            //WeaponDescriptor wd = Game.LocalPlayer.Character.Inventory.Weapons.Where(x => (uint)x.Hash == (uint)MyOldGuns.WeaponHash).FirstOrDefault();
                            //if(wd != null)
                            //{
                            //    wd.Ammo = (short)MyOldGuns.Ammo;
                            //}    
                        }
                    }
                    NativeFunction.Natives.SET_PED_AMMO(Game.LocalPlayer.Character, (uint)MyOldGuns.WeaponHash, 0, false);
                    NativeFunction.Natives.ADD_AMMO_TO_PED(Game.LocalPlayer.Character, (uint)MyOldGuns.WeaponHash, MyOldGuns.Ammo);
                }
                player.Inventory.Clear();
                foreach (InventorySave cii in InventoryItems)
                {
                    player.Inventory.Add(modItems.Get(cii.ModItemName), (int)cii.RemainingPercent);
                }
                player.VehicleOwnership.ClearVehicleOwnership();
                foreach (VehicleSaveStatus OwnedVehicleVariation in OwnedVehicleVariations)
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
                            NewVehicle.Wash();
                            VehicleExt MyVeh = World.Vehicles.GetVehicleExt(NewVehicle.Handle);
                            if (MyVeh == null)
                            {
                                MyVeh = new VehicleExt(NewVehicle, settings);
                                MyVeh.Setup();
                                MyVeh.HasUpdatedPlateType = true;
                                World.Vehicles.AddEntity(MyVeh, ResponseType.None);
                                OwnedVehicleVariation.VehicleVariation?.Apply(MyVeh);
                            }
                            player.VehicleOwnership.TakeOwnershipOfVehicle(MyVeh,false);
                            if (OwnedVehicleVariation.LastPosition != Vector3.Zero)
                            {
                                NewVehicle.Position = OwnedVehicleVariation.LastPosition;
                                NewVehicle.Heading = OwnedVehicleVariation.LastHeading;
                            }
                        }
                    }
                }




                player.RelationshipManager.GangRelationships.ResetGang(false);
                foreach (GangRepSave tuple in GangReputationsSave)
                {
                    Gang myGang = gangs.GetGang(tuple.GangID);
                    if (myGang != null)
                    { 
                        player.RelationshipManager.GangRelationships.SetReputation(myGang, tuple.Reputation, false);
                        player.RelationshipManager.GangRelationships.SetRepStats(myGang, tuple.MembersHurt, tuple.MembersHurtInTerritory, tuple.MembersKilled, tuple.MembersKilledInTerritory, tuple.MembersCarJacked, tuple.MembersCarJackedInTerritory, tuple.PlayerDebt, tuple.IsMember, tuple.IsEnemy);
                        if(tuple.IsMember)
                        {
                            player.RelationshipManager.GangRelationships.SetGang(myGang, false);
                        }
                    }
                }





                if (GangKickSave != null)
                {
                    Gang myGang = gangs.GetGang(GangKickSave.GangID);
                    if (myGang != null)
                    {
                        EntryPoint.WriteToConsole($"Loaded Kick Save {myGang.ShortName}");
                        player.RelationshipManager.GangRelationships.SetKickStatus(myGang, GangKickSave.KickDueDate, GangKickSave.KickMissedPeriods, GangKickSave.KickMissedAmount);
                    }
                    else
                    {
                        EntryPoint.WriteToConsole($"NO KICK SAVE");
                        player.RelationshipManager.GangRelationships.ResetGang(false);
                    }
                }
                else
                {
                    EntryPoint.WriteToConsole($"NO KICK SAVE");
                    player.RelationshipManager.GangRelationships.ResetGang(false);
                }









                foreach (SavedContact ifc in Contacts.OrderBy(x=> x.Index))
                {
                    Gang gang = gangs.GetGangByContact(ifc.Name);
                    if (ifc.Name == EntryPoint.UndergroundGunsContactName)
                    {
                        player.CellPhone.AddGunDealerContact(false);
                    }
                    else if (ifc.Name == EntryPoint.OfficerFriendlyContactName)
                    {
                        player.CellPhone.AddCopContact(false);//mess with this and need to test contacts after loading
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





                player.RelationshipManager.GunDealerRelationship.SetMoneySpent(UndergroundGunsMoneySpent,false);
                player.RelationshipManager.GunDealerRelationship.SetDebt(UndergroundGunsDebt);
                player.RelationshipManager.GunDealerRelationship.SetReputation(UndergroundGunsReputation, false);
                player.RelationshipManager.OfficerFriendlyRelationship.SetMoneySpent(OfficerFriendlyMoneySpent, false);
                player.RelationshipManager.OfficerFriendlyRelationship.SetDebt(OfficerFriendlyDebt);
                player.RelationshipManager.OfficerFriendlyRelationship.SetReputation(OfficerFriendlyReputation, false);




                if (DriversLicense != null)
                {
                    player.Licenses.DriversLicense = new DriversLicense() { ExpirationDate = DriversLicense.ExpirationDate, IssueDate = DriversLicense.IssueDate };
                }
                if (CCWLicense != null)
                {
                    player.Licenses.CCWLicense = new CCWLicense() { ExpirationDate = CCWLicense.ExpirationDate, IssueDate = CCWLicense.IssueDate };
                }
                if (PilotsLicense != null)
                {
                    player.Licenses.PilotsLicense = new PilotsLicense() { ExpirationDate = PilotsLicense.ExpirationDate, IssueDate = PilotsLicense.IssueDate, IsFixedWingEndorsed = PilotsLicense.IsFixedWingEndorsed, IsRotaryEndorsed = PilotsLicense.IsRotaryEndorsed, IsLighterThanAirEndorsed = PilotsLicense.IsLighterThanAirEndorsed };
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

                EntryPoint.WriteToConsole($"PRE LOAD {player.HumanState.DisplayString()} ThirstValue {ThirstValue} HungerValue {HungerValue} SleepValue {SleepValue}");


                player.HumanState.Reset();

                player.HumanState.Thirst.Set(ThirstValue, true);
                player.HumanState.Hunger.Set(HungerValue, true);
                player.HumanState.Sleep.Set(SleepValue, true);


                player.SetCopStatus(IsCop, null);

                //player.IsCop = IsCop;


                EntryPoint.WriteToConsole($"POST LOAD {player.HumanState.DisplayString()}");


                Game.FadeScreenIn(1500, true);
                player.DisplayPlayerNotification();
            }
            catch (Exception e)
            {
                Game.FadeScreenIn(0);
                EntryPoint.WriteToConsole("Error Loading Game Save: " + e.Message + " " + e.StackTrace, 0);
                Game.DisplayNotification("Error Loading Save");
            }
        }
        public override string ToString()
        {
            return $"{PlayerName}";//base.ToString();
        }

        public TabMissionSelectItem SaveTabInfo(ITimeReportable time, IGangs gangs)
        {
            List<MissionInformation> saveMissionInfos = new List<MissionInformation>();

            MissionInformation demographicsSubTab = new MissionInformation("Demographics", "", DemographicsInfo());
            saveMissionInfos.Add(demographicsSubTab);

            MissionInformation affiliationsSubTab = new MissionInformation("Affiliations", "", AffiliationsInfo(gangs));
            saveMissionInfos.Add(affiliationsSubTab);

            MissionInformation needsSubTab = new MissionInformation("Human State", "", HumanStateInfo());
            saveMissionInfos.Add(needsSubTab);

            MissionInformation licensesSubTab = new MissionInformation("Licenses", "", LicenseInfo(time));
            saveMissionInfos.Add(licensesSubTab);

            MissionInformation vehiclesSubTab = new MissionInformation("Vehicles", "", VehicleInfo());
            saveMissionInfos.Add(vehiclesSubTab);

            MissionInformation weaponsSubTab = new MissionInformation("Weapons", "", WeaponInfo());
            saveMissionInfos.Add(weaponsSubTab);

            MissionInformation residenceSubTab = new MissionInformation("Residences", "", ResidenceInfo());
            saveMissionInfos.Add(residenceSubTab);
            MissionInformation loadSubTab = new MissionInformation("Load", "", new List<Tuple<string, string>>());
            saveMissionInfos.Add(loadSubTab);

            MissionInformation deleteSubTab = new MissionInformation("Delete", "", new List<Tuple<string, string>>());
            saveMissionInfos.Add(deleteSubTab);

            TabMissionSelectItem GameSaveEntry = new TabMissionSelectItem($"{PlayerName}~s~", saveMissionInfos);
            return GameSaveEntry;
        }



        private List<Tuple<string, string>> DemographicsInfo()
        {
            List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
            toreturn.Add(Tuple.Create("Name:", PlayerName));
            toreturn.Add(Tuple.Create("Money:", Money.ToString("C0")));
            toreturn.Add(Tuple.Create("Model Name:", ModelName));       
            return toreturn;
        }
        private List<Tuple<string, string>> AffiliationsInfo(IGangs gangs)
        {
            List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
            toreturn.Add(Tuple.Create("Police Officer:", IsCop ? "Yes" : "No"));
            //Gang myGang = gangs?.GetGang(GangKickSave?.GangID);
            //if (myGang != null)
            //{
            //    toreturn.Add(Tuple.Create("Gang:", myGang.ColorInitials));
            //}
            return toreturn;
        }
        private List<Tuple<string, string>> HumanStateInfo()
        {
            List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
            toreturn.Add(Tuple.Create("Thirst:", $"{Math.Round(ThirstValue, 0)}%"));
            toreturn.Add(Tuple.Create("Hunger:", $"{Math.Round(HungerValue, 0)}%"));
            toreturn.Add(Tuple.Create("Sleep:", $"{Math.Round(SleepValue, 0)}%"));
            return toreturn;
        }
        private List<Tuple<string, string>> LicenseInfo(ITimeReportable time)
        {
            List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
            toreturn.Add(Tuple.Create("Drivers License:", DriversLicense?.IsValid(time) == true ? "Valid" : "Not Available"));
            toreturn.Add(Tuple.Create("CCW License:", CCWLicense?.IsValid(time) == true ? "Valid" : "Not Available"));
            toreturn.Add(Tuple.Create("Pilots License:", PilotsLicense?.IsValid(time) == true ? "Valid" : "Not Available"));
            return toreturn;
        }
        private List<Tuple<string, string>> VehicleInfo()
        {
            List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
            foreach (VehicleSaveStatus savedVehicles in OwnedVehicleVariations)
            {
                toreturn.Add(Tuple.Create("Vehicle:", savedVehicles.ModelName));
            }
            return toreturn;
        }
        private List<Tuple<string, string>> WeaponInfo()
        {
            List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
            foreach (StoredWeapon wi in WeaponInventory)
            {
                toreturn.Add(Tuple.Create("Weapon:", wi.WeaponHash.ToString()));
            }
            return toreturn;
        }
        private List<Tuple<string, string>> ResidenceInfo()
        {
            List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
            foreach (SavedResidence wi in SavedResidences)
            {
                toreturn.Add(Tuple.Create("Residence:", wi.Name));
            }
            return toreturn;
        }
    }

}
