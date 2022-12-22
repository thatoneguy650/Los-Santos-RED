using ExtensionsMethods;
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
using System.Net.Http.Headers;
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
            SaveDateTime = DateTime.Now;
        }
        public Vector3 PlayerPosition { get; set; }
        public float PlayerHeading { get; set; }
        public string PlayerName { get; set; }
        public int Money { get; set; }
        public string ModelName { get; set; }
        public bool IsMale { get; set; }
        public int SaveNumber { get; set; }
        public DateTime SaveDateTime { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public DriversLicense DriversLicense { get; set; }
        public CCWLicense CCWLicense { get; set; }
        public PilotsLicense PilotsLicense { get; set; }
        public List<SavedTextMessage> TextMessages { get; set; } = new List<SavedTextMessage>();
        public List<PhoneContact> Contacts { get; set; } = new List<PhoneContact>();
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


        public string VoiceName { get; set; }

       // public List<InventoryItem> InventoryItemsList { get; set; } = new List<InventoryItem>();
        public GangKickSave GangKickSave { get; set; }
        public bool IsCop { get; set; }
        public void Save(ISaveable player, IWeapons weapons, ITimeReportable time, IPlacesOfInterest placesOfInterest, IModItems modItems)
        {
            PlayerName = player.PlayerName;
            ModelName = player.ModelName;
            Money = player.BankAccounts.Money;
            IsMale = player.IsMale;
            CurrentModelVariation = player.CurrentModelVariation.Copy();
            WeaponInventory = new List<StoredWeapon>();



            modItems.WriteToFile();
            SaveDateTime = DateTime.Now;

            InventoryItems.Clear();
            foreach (InventoryItem cii in player.Inventory.ItemsList)
            {
                InventoryItems.Add(new InventorySave(cii.ModItem.Name, cii.RemainingPercent));
            }

           // InventoryItemsList.Clear();
           // InventoryItemsList.AddRange(player.Inventory.ItemsList.ToList());




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

            Contacts = new List<PhoneContact>();
            foreach (PhoneContact ifc in player.CellPhone.ContactList.ToList())
            {
                Contacts.Add(ifc);
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
            VoiceName = player.FreeModeVoice;
        }
        public void Load(IWeapons weapons,IPedSwap pedSwap, IInventoryable player, ISettingsProvideable settings, IEntityProvideable world, IGangs gangs, ITimeControllable time, IPlacesOfInterest placesOfInterest, IModItems modItems)
        {
            try
            {
                Game.FadeScreenOut(1000, true);

                time.SetDateTime(CurrentDateTime);
                pedSwap.BecomeSavedPed(PlayerName, ModelName, Money, CurrentModelVariation, SpeechSkill, VoiceName);//, CurrentHeadBlendData, CurrentPrimaryHairColor, CurrentSecondaryColor, CurrentHeadOverlays);
                LoadWeapons(weapons);
                LoadInventory(player, modItems);
                LoadVehicles(player, world,settings);
                LoadPosition(player);
                LoadRelationships(player, gangs);
                LoadContacts(player, gangs);
                LoadLicenses(player);
                LoadResidences(player, placesOfInterest);
                LoadHumanState(player);
                player.SetCopStatus(IsCop, null);
                GameFiber.Sleep(1000);
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
        private void LoadWeapons(IWeapons weapons)
        {
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
        }
        private void LoadInventory(IInventoryable player,IModItems modItems)
        {
            player.Inventory.Clear();
            foreach (InventorySave cii in InventoryItems)
            {
                ModItem toadd = modItems.Get(cii.ModItemName);
                if (toadd != null)
                {
                    player.Inventory.Add(toadd, (int)cii.RemainingPercent);
                }
                //else if (cii.ModItemName.Contains("License Plate:"))
                //{
                //    string cleanedName = cii.ModItemName.Replace("License Plate:", "");
                //    string plateNumber = cleanedName.Split('-')[0];
                //    string plateType = cleanedName.Split('-')[1].ToString();
                //    bool isWanted = cleanedName.Split('-')[2] == "0" ? false : true;//THIS IS ABSOLUTE CRAPOLA, UNTIL I GET MORE OF THESE FUCKING DYNAMIC ITEMS THIS IS WHERE THEY WILL STAY
                //    if (int.TryParse(plateType, out int plateTypeInt))
                //    { 
                //        LicensePlateItem licensePlateItem = new LicensePlateItem(cii.ModItemName) { LicensePlate = new LicensePlate(plateNumber, plateTypeInt, isWanted) };
                //        player.Inventory.Add(licensePlateItem, 1.0f);
                //    }
                //}
            }
        }
        private void LoadVehicles(IInventoryable player, IEntityProvideable World, ISettingsProvideable settings)
        {
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
                    else if (OwnedVehicleVariation.ModelHash != 0)
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
                        player.VehicleOwnership.TakeOwnershipOfVehicle(MyVeh, false);
                        if (OwnedVehicleVariation.LastPosition != Vector3.Zero)
                        {
                            NewVehicle.Position = OwnedVehicleVariation.LastPosition;
                            NewVehicle.Heading = OwnedVehicleVariation.LastHeading;
                        }
                    }
                }
            }
        }
        private void LoadRelationships(IInventoryable player, IGangs gangs)
        {
            player.RelationshipManager.GangRelationships.ResetGang(false);
            foreach (GangRepSave gangRepSave in GangReputationsSave)
            {
                Gang myGang = gangs.GetGang(gangRepSave.GangID);
                if (myGang != null)
                {
                    player.RelationshipManager.GangRelationships.SetReputation(myGang, gangRepSave.Reputation, false);
                    player.RelationshipManager.GangRelationships.SetRepStats(myGang, gangRepSave.MembersHurt, gangRepSave.MembersHurtInTerritory, gangRepSave.MembersKilled, gangRepSave.MembersKilledInTerritory, gangRepSave.MembersCarJacked, gangRepSave.MembersCarJackedInTerritory, gangRepSave.PlayerDebt, gangRepSave.IsMember, gangRepSave.IsEnemy);
                    if (gangRepSave.IsMember)
                    {
                        player.RelationshipManager.GangRelationships.SetGang(myGang, false);
                        if(GangKickSave != null)
                        {
                            player.RelationshipManager.GangRelationships.SetKickStatus(myGang, GangKickSave.KickDueDate, GangKickSave.KickMissedPeriods, GangKickSave.KickMissedAmount);
                        }
                        else
                        {

                        }
                    }
                }
            }
            player.RelationshipManager.GunDealerRelationship.SetMoneySpent(UndergroundGunsMoneySpent, false);
            player.RelationshipManager.GunDealerRelationship.SetDebt(UndergroundGunsDebt);
            player.RelationshipManager.GunDealerRelationship.SetReputation(UndergroundGunsReputation, false);
            player.RelationshipManager.OfficerFriendlyRelationship.SetMoneySpent(OfficerFriendlyMoneySpent, false);
            player.RelationshipManager.OfficerFriendlyRelationship.SetDebt(OfficerFriendlyDebt);
            player.RelationshipManager.OfficerFriendlyRelationship.SetReputation(OfficerFriendlyReputation, false);
        }
        private void LoadContacts(IInventoryable player, IGangs gangs)
        {
            foreach (PhoneContact ifc in Contacts.OrderBy(x => x.Index))
            {
                //Gang gang = gangs.GetGangByContact(ifc.Name);
                //if (ifc.Name == StaticStrings.UndergroundGunsContactName)
                //{
                //    player.CellPhone.AddGunDealerContact(false);
                //}
                //else if (ifc.Name == StaticStrings.OfficerFriendlyContactName)
                //{
                //    player.CellPhone.AddCopContact(false);//mess with this and need to test contacts after loading
                //}
                //else if (gang != null)
                //{
                //    player.CellPhone.AddContact(gang, false);
                //}
                //else
                //{
                //    player.CellPhone.AddContact(ifc.Name, ifc.IconName, false);
                //}
            }
            foreach (SavedTextMessage ifc in TextMessages)
            {
                player.CellPhone.AddText(ifc.Name, ifc.IconName, ifc.Message, ifc.HourSent, ifc.MinuteSent, ifc.IsRead);
            }
        }
        private void LoadLicenses(IInventoryable player)
        {
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
        }
        private void LoadResidences(IInventoryable player, IPlacesOfInterest placesOfInterest)
        {
            foreach (SavedResidence res in SavedResidences)
            {
                if (res.IsOwnedByPlayer || res.IsRentedByPlayer)
                {
                    Residence savedPlace = placesOfInterest.PossibleLocations.Residences.Where(x => x.Name == res.Name).FirstOrDefault();
                    if (savedPlace != null)
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
        }
        private void LoadHumanState(IInventoryable player)
        {
            player.HumanState.Reset();
            player.HumanState.Thirst.Set(ThirstValue, true);
            player.HumanState.Hunger.Set(HungerValue, true);
            player.HumanState.Sleep.Set(SleepValue, true);
        }
        private void LoadPosition(IInventoryable player)
        {
            if (PlayerPosition != Vector3.Zero)
            {
                VehicleExt closestVehicle = player.VehicleOwnership.OwnedVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.DistanceTo2D(PlayerPosition) <= 4f);
                if (closestVehicle != null && closestVehicle.Vehicle.Exists() && closestVehicle.Vehicle.IsSeatFree(-1))
                {
                    player.Character.WarpIntoVehicle(closestVehicle.Vehicle, -1);
                    return;
                }
                List<Entity> BlockingVehicles = Rage.World.GetEntities(PlayerPosition, 3f, GetEntitiesFlags.ConsiderAllVehicles).ToList();
                foreach(Entity vehicle in BlockingVehicles)
                {
                    if(vehicle.Exists() && !vehicle.IsPersistent)
                    {
                        vehicle.Delete();
                    }
                }
                player.Character.Position = PlayerPosition;
                player.Character.Heading = PlayerHeading;   
            }
        }
        public TabMissionSelectItem SaveTabInfo(ITimeReportable time, IGangs gangs, IWeapons weapons, IModItems modItems)
        {
            List<MissionInformation> saveMissionInfos = new List<MissionInformation>();

            MissionInformation loadSubTab = new MissionInformation("Load", "Load the selected save", new List<Tuple<string, string>>());
            saveMissionInfos.Add(loadSubTab);


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

            MissionInformation weaponsSubTab = new MissionInformation("Weapons", "", WeaponInfo(weapons,modItems));
            saveMissionInfos.Add(weaponsSubTab);

            MissionInformation residenceSubTab = new MissionInformation("Residences", "", ResidenceInfo());
            saveMissionInfos.Add(residenceSubTab);


            MissionInformation deleteSubTab = new MissionInformation("Delete", "Deletes the selected save", new List<Tuple<string, string>>());
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
            bool isGangMember = false;
            if(IsCop)
            {
                toreturn.Add(Tuple.Create("Police Officer:", IsCop ? "Yes" : "No"));//store agency here
            }
            else 
            {
                GangRepSave memberSave = GangReputationsSave.Where(x => x.IsMember).FirstOrDefault();
                if(memberSave != null)
                {
                    Gang currentGang = gangs.GetGang(memberSave.GangID);
                    if(currentGang != null)
                    {
                        isGangMember = true;
                        toreturn.Add(Tuple.Create(currentGang.ShortName,"Member"));
                    }
                }
            }    
            if(!isGangMember && !IsCop)
            {
                toreturn.Add(Tuple.Create("None", ""));
            }
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
                string MakeName = "Unk";
                string ModelName = "Unk";
                if (savedVehicles.ModelName != "")
                {
                    ModelName = NativeHelper.VehicleModelName(Game.GetHashKey(savedVehicles.ModelName));
                    MakeName = NativeHelper.VehicleMakeName(Game.GetHashKey(savedVehicles.ModelName));
                }
                else
                {
                    ModelName = NativeHelper.VehicleModelName(savedVehicles.ModelHash);
                    MakeName = NativeHelper.VehicleMakeName(savedVehicles.ModelHash);
                }
                toreturn.Add(Tuple.Create(MakeName, ModelName));
            }
            return toreturn;
        }
        private List<Tuple<string, string>> WeaponInfo(IWeapons weapons, IModItems modItems)
        {
            List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
            foreach (StoredWeapon sw in WeaponInventory)
            {
                WeaponInformation wi = weapons.GetWeapon(sw.WeaponHash);
                if (wi != null)
                {
                    ModItem weaponItem = modItems.AllItems().Where(x => x.ModelItem != null && (x.ModelItem.ModelHash == sw.WeaponHash || x.ModelItem.ModelName == sw.WeaponHash.ToString())).FirstOrDefault();
                    if(weaponItem != null)
                    {
                        toreturn.Add(Tuple.Create($"{weaponItem.Name}", $"Ammo: ({sw.Ammo})"));
                    }
                    else
                    {
                        toreturn.Add(Tuple.Create(wi.ModelName, $"Ammo: ({sw.Ammo})"));
                    }
                }
                else
                {
                    toreturn.Add(Tuple.Create(sw.WeaponHash.ToString(), $"Ammo: ({sw.Ammo})"));
                }
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
        public string Title => $"{SaveNumber.ToString("D2")} - {PlayerName} ({Money.ToString("C0")}) - {CurrentDateTime.ToString("MM/dd/yyyy HH:mm")}";
        public string RightLabel => SaveDateTime.ToString("MM/dd/yyyy HH:mm");

    }

}
