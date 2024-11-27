using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace LosSantosRED.lsr.Data
{
    public class GameSave
    {
        public GameSave()
        {

        }
        public GameSave(string playerName, int money, string modelName, bool isMale, PedVariation currentModelVariation, List<StoredWeapon> weaponInventory, List<VehicleSaveStatus> vehicleVariations)
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
        public string PlayerName { get; set; }
        public int Money { get; set; }
        public List<BankAccount> SavedBankAccounts { get; set; } = new List<BankAccount>();
        public string ModelName { get; set; }
        public Vector3 PlayerPosition { get; set; }
        public float PlayerHeading { get; set; }
        public bool IsMale { get; set; }
        public int SaveNumber { get; set; }
        public DateTime SaveDateTime { get; set; }
        public DateTime CurrentDateTime { get; set; }
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
        public bool IsCop { get; set; }
        public bool IsEMT { get; set; }
        public bool IsFireFighter { get; set; }
        public bool IsSecurityGuard {get;set;}
        public string AssignedAgencyID { get; set; }
        public string VoiceName { get; set; }
        public int Health { get; set; }
        public int Armor { get; set; }
        public int MaxHealth { get; set; }
        public string CurrentTeleportInterior { get; set; }
        public PedVariation CurrentModelVariation { get; set; }
        public DriversLicense DriversLicense { get; set; }
        public CCWLicense CCWLicense { get; set; }
        public PilotsLicense PilotsLicense { get; set; }
        public List<SavedTextMessage> TextMessages { get; set; } = new List<SavedTextMessage>();
        public List<PhoneContact> Contacts { get; set; } = new List<PhoneContact>();
        public List<ContactRelationship> ContactRelationships { get; set; } = new List<ContactRelationship>();
        public List<GangRepSave> GangReputationsSave { get; set; } = new List<GangRepSave>();
        public GangKickSave GangKickSave { get; set; }
        public List<StoredWeapon> WeaponInventory { get; set; } = new List<StoredWeapon>();
        public List<InventorySave> InventoryItems { get; set; } = new List<InventorySave>();
        public List<VehicleSaveStatus> OwnedVehicleVariations { get; set; } = new List<VehicleSaveStatus>();
        public List<SavedResidence> SavedResidences { get; set; } = new List<SavedResidence>();
        public CellPhoneSave CellPhoneSave { get; set; } = new CellPhoneSave();

        public List<GangLoanSave> GangLoanSaves { get; set; } = new List<GangLoanSave>();

        [OnDeserialized()]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            MaxHealth = 200;
        }



        //Save
        public void Save(ISaveable player, IWeapons weapons, ITimeReportable time, IPlacesOfInterest placesOfInterest, IModItems modItems)
        {
            SaveDemographics(player);
            SaveMoney(player);
            SaveInventory(player, modItems);
            SaveWeapons(player, weapons);
            SaveVehicles(player);
            SaveReputation(player);
            SaveContacts(player, time);
            SavePostition(player);
            SaveHumanState(player);
            SaveLicenses(player);
            SaveResidences(player);
            SaveAgencies(player);
            SaveCellPhone(player); 
        }
        private void SaveDemographics(ISaveable player)
        {
            PlayerName = player.PlayerName;
            ModelName = player.ModelName;

            IsMale = player.IsMale;
            CurrentModelVariation = player.CurrentModelVariation.Copy();
            WeaponInventory = new List<StoredWeapon>();

            Health = player.Character.Health;
            MaxHealth = player.Character.MaxHealth;
            Armor = player.Character.Armor;

            SpeechSkill = player.SpeechSkill;
            VoiceName = player.FreeModeVoice;

            SaveDateTime = DateTime.Now;
        }
        private void SaveMoney(ISaveable player)
        {
            Money = player.BankAccounts.GetMoney(false);
            SavedBankAccounts.Clear();
            foreach (BankAccount bankAccount in player.BankAccounts.BankAccountList)
            {
                SavedBankAccounts.Add(new BankAccount(bankAccount.BankContactName, bankAccount.AccountName, bankAccount.Money) { IsPrimary = bankAccount.IsPrimary });
            }
        }
        private void SaveInventory(ISaveable player, IModItems modItems)
        {
            modItems.WriteToFile();
            InventoryItems.Clear();
            foreach (InventoryItem cii in player.Inventory.ItemsList)
            {
                InventoryItems.Add(new InventorySave(cii.ModItem.Name, cii.RemainingPercent));
            }
        }
        private void SaveWeapons(ISaveable player, IWeapons weapons)
        {
            foreach (WeaponDescriptor wd in Game.LocalPlayer.Character.Inventory.Weapons)
            {
                WeaponInventory.Add(new StoredWeapon((uint)wd.Hash, Vector3.Zero, weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)wd.Hash), wd.Ammo));
            }
        }
        private void SaveVehicles(ISaveable player)
        {
            OwnedVehicleVariations.Clear();
            foreach (VehicleExt car in player.VehicleOwnership.OwnedVehicles)
            {
                if (car.Vehicle.Exists())
                {
                    VehicleSaveStatus vss;
                    vss = new VehicleSaveStatus(car.Vehicle.Model.Hash, car.Vehicle.Position, car.Vehicle.Heading);
                    vss.IsImpounded = car.IsImpounded;
                    vss.DateTimeImpounded = car.DateTimeImpounded;
                    vss.TimesImpounded = car.TimesImpounded;
                    vss.ImpoundedLocation = car.ImpoundedLocation;
                    if (car.CashStorage != null)
                    {
                        vss.StoredCash = car.CashStorage.StoredCash;
                    }
                    if (car.WeaponStorage != null)
                    {
                        vss.WeaponInventory = new List<StoredWeapon>();
                        foreach (StoredWeapon storedWeapon in car.WeaponStorage.StoredWeapons)
                        {
                            vss.WeaponInventory.Add(storedWeapon.Copy());
                        }
                    }
                    if (car.SimpleInventory != null)
                    {
                        vss.InventoryItems = new List<InventorySave>();
                        foreach (InventoryItem ii in car.SimpleInventory.ItemsList)
                        {
                            vss.InventoryItems.Add(new InventorySave(ii.ModItem?.Name, ii.RemainingPercent));
                        }
                    }
                    vss.VehicleVariation = NativeHelper.GetVehicleVariation(car.Vehicle);
                    OwnedVehicleVariations.Add(vss);
                }
            }
        }
        private void SaveReputation(ISaveable player)
        {
            GangReputationsSave = new List<GangRepSave>();
            foreach (GangReputation gr in player.RelationshipManager.GangRelationships.GangReputations)
            {
                GangReputationsSave.Add(new GangRepSave(gr.Gang.ID, gr.ReputationLevel, gr.MembersHurt, gr.MembersKilled, gr.MembersCarJacked, gr.MembersHurtInTerritory, gr.MembersKilledInTerritory, gr.MembersCarJackedInTerritory, gr.PlayerDebt, gr.IsMember, gr.IsEnemy, gr.TasksCompleted));
                if(gr.GangLoan != null && gr.GangLoan.DueAmount > 0)
                {
                    GangLoanSaves.Add(new GangLoanSave(gr.Gang.ID, gr.GangLoan.DueAmount,gr.GangLoan.VigAmount,gr.GangLoan.MissedPeriods,gr.GangLoan.DueDate));
                }
            }

            if (player.RelationshipManager.GangRelationships.CurrentGang != null && player.RelationshipManager.GangRelationships.CurrentGangKickUp != null)
            {
                GangKickSave = new GangKickSave(player.RelationshipManager.GangRelationships.CurrentGang.ID, player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueDate, player.RelationshipManager.GangRelationships.CurrentGangKickUp.MissedPeriods, player.RelationshipManager.GangRelationships.CurrentGangKickUp.MissedAmount);
            }
            
        }
        private void SaveContacts(ISaveable player, ITimeReportable time)
        {
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


            ContactRelationships = new List<ContactRelationship>();
            foreach (ContactRelationship test in player.RelationshipManager.ContactRelationships)
            {
                EntryPoint.WriteToConsole($"RELATIONSHIP SAVE {test.ContactName} MONEY:{test.TotalMoneySpent} REP:{test.ReputationLevel}");
                ContactRelationships.Add(test);
            }
        }
        private void SavePostition(ISaveable player)
        {
            if(player.InteriorManager.IsInsideTeleportInterior && player.InteriorManager.CurrentTeleportInteriorLocation != null)
            {
                CurrentTeleportInterior = player.InteriorManager.CurrentTeleportInteriorLocation.Name;
            }
            else
            {
                CurrentTeleportInterior = null;
            }
            PlayerPosition = player.Character.Position;
            PlayerHeading = player.Character.Heading;
        }
        private void SaveHumanState(ISaveable player)
        {
            HungerValue = player.HumanState.Hunger.CurrentValue;
            SleepValue = player.HumanState.Sleep.CurrentValue;
            ThirstValue = player.HumanState.Thirst.CurrentValue;
        }
        private void SaveLicenses(ISaveable player)
        {
            if (player.Licenses.HasDriversLicense)
            {
                DriversLicense = new DriversLicense() { ExpirationDate = player.Licenses.DriversLicense.ExpirationDate, IssueDate = player.Licenses.DriversLicense.IssueDate, IssueStateID = player.Licenses.DriversLicense.IssueStateID };
            }
            if (player.Licenses.HasCCWLicense)
            {
                CCWLicense = new CCWLicense() { ExpirationDate = player.Licenses.CCWLicense.ExpirationDate, IssueDate = player.Licenses.CCWLicense.IssueDate, IssueStateID = player.Licenses.DriversLicense.IssueStateID };
            }
            if (player.Licenses.HasPilotsLicense)
            {
                PilotsLicense = new PilotsLicense() { ExpirationDate = player.Licenses.PilotsLicense.ExpirationDate, IssueDate = player.Licenses.PilotsLicense.IssueDate, IssueStateID = player.Licenses.DriversLicense.IssueStateID, IsFixedWingEndorsed = player.Licenses.PilotsLicense.IsFixedWingEndorsed, IsRotaryEndorsed = player.Licenses.PilotsLicense.IsRotaryEndorsed, IsLighterThanAirEndorsed = player.Licenses.PilotsLicense.IsLighterThanAirEndorsed };
            }
        }
        private void SaveResidences(ISaveable player)
        {
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
                    if (res.WeaponStorage != null)
                    {
                        myRes.WeaponInventory = new List<StoredWeapon>();
                        foreach (StoredWeapon storedWeapon in res.WeaponStorage.StoredWeapons)
                        {
                            myRes.WeaponInventory.Add(storedWeapon.Copy());
                        }
                    }
                    if (res.SimpleInventory != null)
                    {
                        myRes.InventoryItems = new List<InventorySave>();
                        foreach (InventoryItem ii in res.SimpleInventory.ItemsList)
                        {
                            myRes.InventoryItems.Add(new InventorySave(ii.ModItem?.Name, ii.RemainingPercent));
                        }
                    }
                    if(res.CashStorage != null)
                    {
                        myRes.StoredCash = res.CashStorage.StoredCash;
                    }
                    SavedResidences.Add(myRes);
                }
            }
        }
        private void SaveAgencies(ISaveable player)
        {
            IsCop = player.IsCop;
            IsEMT = player.IsEMT;
            IsFireFighter = player.IsFireFighter;
            IsSecurityGuard = player.IsSecurityGuard;
            AssignedAgencyID = player.AssignedAgency?.ID;
        }
        private void SaveCellPhone(ISaveable player)
        {
            CellPhoneSave = new CellPhoneSave(player.CellPhone.CustomRingtone, player.CellPhone.CustomTextTone, player.CellPhone.CustomTheme, player.CellPhone.CustomBackground, player.CellPhone.CustomVolume, player.CellPhone.SleepMode, player.CellPhone.CustomPhoneType, player.CellPhone.CustomPhoneOS);
        }
        //Load
        public void Load(IWeapons weapons,IPedSwap pedSwap, IInventoryable player, ISettingsProvideable settings, IEntityProvideable world, IGangs gangs, IAgencies agencies, ITimeControllable time, IPlacesOfInterest placesOfInterest, IModItems modItems, IContacts contacts, IInteractionable interactionable)
        {
            try
            {
                Game.FadeScreenOut(1000, true);
                time.SetDateTime(CurrentDateTime);
                pedSwap.BecomeSavedPed(PlayerName, ModelName, Money, CurrentModelVariation, SpeechSkill, VoiceName);//, CurrentHeadBlendData, CurrentPrimaryHairColor, CurrentSecondaryColor, CurrentHeadOverlays);
                LoadMoney(player);
                LoadWeapons(weapons);
                LoadInventory(player, modItems);
                LoadLicenses(player);
                LoadVehicles(player, world,settings, modItems, placesOfInterest, time, weapons);
                LoadPosition(player, placesOfInterest, world, interactionable);
                LoadRelationships(player, gangs, contacts, time);
                LoadContacts(player, gangs);    
                LoadResidences(player, placesOfInterest, modItems, settings);
                LoadHumanState(player);
                LoadCellPhoneSettings(player);
                LoadAgencies(agencies, player);
                LoadHealth(player);
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
        private void LoadMoney(IInventoryable player)
        {
            player.BankAccounts.Reset();
            foreach (BankAccount bankAccount in SavedBankAccounts)
            {
                player.BankAccounts.BankAccountList.Add(new BankAccount(bankAccount.BankContactName, bankAccount.AccountName, bankAccount.Money) { IsPrimary = bankAccount.IsPrimary });
            }
        }
        private void LoadAgencies(IAgencies agencies, IInventoryable player)
        {
            if (IsCop || IsEMT || IsFireFighter || IsSecurityGuard)
            {
                EntryPoint.WriteToConsole($" LoadAgencies {AssignedAgencyID}");

                Agency toAssign = agencies.GetAgency(AssignedAgencyID);
                if(toAssign == null)
                {
                    EntryPoint.WriteToConsole($" LoadAgencies NO AGENCY FOUND");
                    return;
                }
                player.SetAgencyStatus(toAssign);
            }
            else
            {
                player.RemoveAgencyStatus();
            }
        }
        private void LoadHealth(IInventoryable player)
        {
            if (Health > 0)
            {

                player.Character.Health = Health;
            }
            if(MaxHealth > 0)
            {
                player.Character.MaxHealth = MaxHealth;
            }
            else
            {
                player.Character.MaxHealth = 200;
            }
            if (Armor > 0)
            {
                player.Character.Armor = Armor;
            }
        }
        private void LoadCellPhoneSettings(IInventoryable player)
        {
            player.CellPhone.CustomRingtone = CellPhoneSave.CustomRingtone;
            player.CellPhone.CustomTextTone = CellPhoneSave.CustomTextTone;
            player.CellPhone.CustomTheme = CellPhoneSave.CustomTheme;
            player.CellPhone.CustomBackground = CellPhoneSave.CustomBackground;
            player.CellPhone.CustomVolume = CellPhoneSave.CustomVolume;
            player.CellPhone.SleepMode = CellPhoneSave.SleepMode;
            player.CellPhone.CustomPhoneType = CellPhoneSave.CustomPhoneType;
            player.CellPhone.CustomPhoneOS = CellPhoneSave.CustomPhoneOS;
        }
        public override string ToString()
        {
            return $"{PlayerName}";//base.ToString();
        }
        private void LoadWeapons(IWeapons weapons)
        {
            //WeaponDescriptorCollection PlayerWeapons = Game.LocalPlayer.Character.Inventory.Weapons;
            foreach (StoredWeapon MyOldGuns in WeaponInventory)
            {
                MyOldGuns.GiveToPlayer(weapons);


                //Game.LocalPlayer.Character.Inventory.GiveNewWeapon(MyOldGuns.WeaponHash, 0, false);
                //if (PlayerWeapons.Contains(MyOldGuns.WeaponHash))
                //{
                //    WeaponInformation Gun2 = weapons.GetWeapon((uint)MyOldGuns.WeaponHash);
                //    if (Gun2 != null)
                //    {
                //        Gun2.ApplyWeaponVariation(Game.LocalPlayer.Character, MyOldGuns.Variation);
                //        //WeaponDescriptor wd = Game.LocalPlayer.Character.Inventory.Weapons.Where(x => (uint)x.Hash == (uint)MyOldGuns.WeaponHash).FirstOrDefault();
                //        //if(wd != null)
                //        //{
                //        //    wd.Ammo = (short)MyOldGuns.Ammo;
                //        //}    
                //    }
                //}
                //NativeFunction.Natives.SET_PED_AMMO(Game.LocalPlayer.Character, (uint)MyOldGuns.WeaponHash, 0, false);
                //NativeFunction.Natives.ADD_AMMO_TO_PED(Game.LocalPlayer.Character, (uint)MyOldGuns.WeaponHash, MyOldGuns.Ammo);
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
                    player.Inventory.Add(toadd, cii.RemainingPercent); //player.Inventory.Add(toadd, (int)cii.RemainingPercent);
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
        private void LoadVehicles(IInventoryable player, IEntityProvideable World, ISettingsProvideable settings, IModItems modItems,IPlacesOfInterest placesOfInterest, ITimeReportable time, IWeapons weapons)
        {
            player.VehicleOwnership.ClearVehicleOwnership();
            foreach (VehicleSaveStatus OwnedVehicleVariation in OwnedVehicleVariations)
            {
                if(OwnedVehicleVariation.LastPosition == Vector3.Zero)
                {
                    continue;
                }
                Vehicle NewVehicle = null;
                if (OwnedVehicleVariation.ModelName != "")
                {
                    World.Vehicles.CleanupAmbient();
                    NewVehicle = new Vehicle(OwnedVehicleVariation.ModelName, OwnedVehicleVariation.LastPosition, OwnedVehicleVariation.LastHeading);
                }
                else if (OwnedVehicleVariation.ModelHash != 0)
                {
                    World.Vehicles.CleanupAmbient();
                    NewVehicle = new Vehicle(OwnedVehicleVariation.ModelHash, OwnedVehicleVariation.LastPosition, OwnedVehicleVariation.LastHeading);
                }
                if (!NewVehicle.Exists())
                {
                    continue;
                }
                NewVehicle.Wash();
                VehicleExt MyVeh = World.Vehicles.GetVehicleExt(NewVehicle.Handle);
                if (MyVeh == null)
                {
                    MyVeh = new VehicleExt(NewVehicle, settings);
                    MyVeh.Setup();
                    MyVeh.HasUpdatedPlateType = true;
                    MyVeh.CanHaveRandomItems = false;
                    MyVeh.CanHaveRandomCash = false;
                    MyVeh.CanHaveRandomWeapons = false;
                    MyVeh.AddVehicleToList(World);
                    //World.Vehicles.AddEntity(MyVeh, ResponseType.None);
                    OwnedVehicleVariation.VehicleVariation?.Apply(MyVeh);
                }
                foreach (StoredWeapon storedWeap in OwnedVehicleVariation.WeaponInventory)
                {
                    MyVeh.WeaponStorage.StoredWeapons.Add(storedWeap.Copy());
                }
                foreach (InventorySave stest in OwnedVehicleVariation.InventoryItems)
                {
                    MyVeh.SimpleInventory.Add(modItems.Get(stest.ModItemName), stest.RemainingPercent);
                }
                MyVeh.CashStorage.StoredCash = OwnedVehicleVariation.StoredCash;
                player.VehicleOwnership.TakeOwnershipOfVehicle(MyVeh, false);
                if (OwnedVehicleVariation.LastPosition != Vector3.Zero)
                {
                    NewVehicle.Position = OwnedVehicleVariation.LastPosition;
                    NewVehicle.Heading = OwnedVehicleVariation.LastHeading;
                }   
                if(OwnedVehicleVariation.IsImpounded)
                {
                    MyVeh.IsImpounded = OwnedVehicleVariation.IsImpounded;
                    MyVeh.TimesImpounded = OwnedVehicleVariation.TimesImpounded;
                    MyVeh.DateTimeImpounded = OwnedVehicleVariation.DateTimeImpounded;
                    ILocationImpoundable locationImpoundable = placesOfInterest.VehicleImpoundLocations().Where(x => x.Name == OwnedVehicleVariation.ImpoundedLocation).FirstOrDefault();
                    if(locationImpoundable != null && locationImpoundable.HasImpoundLot && locationImpoundable.VehicleImpoundLot.ImpoundVehicle(MyVeh, time, player.Licenses.HasValidCCWLicense(time), weapons))
                    {
                        //MyVeh.TimesImpounded = OwnedVehicleVariation.TimesImpounded;
                        //MyVeh.DateTimeImpounded = OwnedVehicleVariation.DateTimeImpounded;
                    }
                }
            }
        }
        private void LoadRelationships(IInventoryable player, IGangs gangs, IContacts contacts, ITimeReportable time)
        {
            player.RelationshipManager.GangRelationships.ResetGang(false);
            player.RelationshipManager.Reset(false);
            foreach (GangRepSave gangRepSave in GangReputationsSave)
            {
                Gang myGang = gangs.GetGang(gangRepSave.GangID);
                if (myGang != null)
                {
                    player.RelationshipManager.GangRelationships.SetReputation(myGang, gangRepSave.Reputation, false);
                    player.RelationshipManager.GangRelationships.SetRepStats(myGang, gangRepSave.MembersHurt, gangRepSave.MembersHurtInTerritory, gangRepSave.MembersKilled, gangRepSave.MembersKilledInTerritory, gangRepSave.MembersCarJacked, gangRepSave.MembersCarJackedInTerritory, gangRepSave.PlayerDebt, gangRepSave.IsMember, gangRepSave.IsEnemy, gangRepSave.TasksCompleted);
                    if (gangRepSave.IsMember)
                    {
                        player.RelationshipManager.GangRelationships.SetGang(myGang, false);
                        if(GangKickSave != null)
                        {
                            player.RelationshipManager.GangRelationships.SetKickStatus(myGang, GangKickSave.KickDueDate, GangKickSave.KickMissedPeriods, GangKickSave.KickMissedAmount);
                        }
                    }
                }
            }
            foreach (ContactRelationship contactRelationship in ContactRelationships)
            {
                if(!contacts.PossibleContacts.AllContacts().Any(x=> x.Name.ToLower() == contactRelationship.ContactName.ToLower()))
                {
                    EntryPoint.WriteToConsole($"Game Saves: Removing Contact Relationship (Not Found) from {contactRelationship.ContactName}");//can turn off eventually i guess
                    continue;
                }



                EntryPoint.WriteToConsole($"RELATIONSHIP LOAD {contactRelationship.ContactName} MONEY:{contactRelationship.TotalMoneySpent} REP:{contactRelationship.ReputationLevel}");
                contactRelationship.SetupContact(contacts);
                player.RelationshipManager.Add(contactRelationship);
                contactRelationship.Activate();

                EntryPoint.WriteToConsole($"contactRelationship.HasPhoneContact: {contactRelationship.HasPhoneContact}");
            }
            if(GangLoanSaves == null || !GangLoanSaves.Any())
            {
                return;
            }
            foreach(GangLoanSave gls in GangLoanSaves)
            {
                Gang gang = gangs.GetGang(gls.GangID);
                if(gang == null)
                {
                    continue;
                }
                GangReputation gr = player.RelationshipManager.GangRelationships.GetReputation(gang);
                if(gr == null)
                {
                    continue;
                }
                gr.RestartLoan(gls, time);
            }

        }
        private void LoadContacts(IInventoryable player, IGangs gangs)
        {
            foreach (PhoneContact ifc in Contacts.OrderBy(x => x.Index))
            {
                player.CellPhone.AddContact(ifc,false);
            }
            foreach (SavedTextMessage ifc in TextMessages)
            {
                player.CellPhone.AddText(ifc.Name, ifc.IconName, ifc.Message, ifc.HourSent, ifc.MinuteSent, ifc.IsRead, null);
            }
        }
        private void LoadLicenses(IInventoryable player)
        {
            if (DriversLicense != null)
            {
                player.Licenses.DriversLicense = new DriversLicense() { ExpirationDate = DriversLicense.ExpirationDate, IssueDate = DriversLicense.IssueDate, IssueStateID = DriversLicense.IssueStateID };
            }
            if (CCWLicense != null)
            {
                player.Licenses.CCWLicense = new CCWLicense() { ExpirationDate = CCWLicense.ExpirationDate, IssueDate = CCWLicense.IssueDate, IssueStateID = DriversLicense.IssueStateID };
            }
            if (PilotsLicense != null)
            {
                player.Licenses.PilotsLicense = new PilotsLicense() { ExpirationDate = PilotsLicense.ExpirationDate, IssueDate = PilotsLicense.IssueDate, IssueStateID = DriversLicense.IssueStateID, IsFixedWingEndorsed = PilotsLicense.IsFixedWingEndorsed, IsRotaryEndorsed = PilotsLicense.IsRotaryEndorsed, IsLighterThanAirEndorsed = PilotsLicense.IsLighterThanAirEndorsed };
            }
        }
        private void LoadResidences(IInventoryable player, IPlacesOfInterest placesOfInterest, IModItems modItems, ISettingsProvideable settings)
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
                        if(savedPlace.WeaponStorage == null)
                        {
                            savedPlace.WeaponStorage = new WeaponStorage(settings);
                        }
                        if(savedPlace.SimpleInventory == null)
                        {
                            savedPlace.SimpleInventory = new SimpleInventory(settings);
                        }
                        foreach (StoredWeapon storedWeap in res.WeaponInventory)
                        {
                            savedPlace.WeaponStorage.StoredWeapons.Add(storedWeap.Copy());
                        }
                        foreach (InventorySave stest in res.InventoryItems)
                        {
                            savedPlace.SimpleInventory.Add(modItems.Get(stest.ModItemName), stest.RemainingPercent);
                        }
                        savedPlace.CashStorage.StoredCash = res.StoredCash;
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
        private void LoadPosition(IInventoryable player, IPlacesOfInterest placesOfInterest, IEntityProvideable world, IInteractionable interactionable)
        {
            if (PlayerPosition != Vector3.Zero)
            {
                VehicleExt closestVehicle = player.VehicleOwnership.OwnedVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.DistanceTo2D(PlayerPosition) <= 4f);
                if (closestVehicle != null && closestVehicle.Vehicle.Exists() && closestVehicle.Vehicle.IsSeatFree(-1))
                {
                    player.Character.WarpIntoVehicle(closestVehicle.Vehicle, -1);
                    return;
                }
                //List<Entity> BlockingVehicles = Rage.World.GetEntities(PlayerPosition, 3f, GetEntitiesFlags.ConsiderAllVehicles).ToList();//TR NOTE REMOVED ENTITY CHECK
                //foreach(Entity vehicle in BlockingVehicles)
                //{
                //    if(vehicle.Exists() && !vehicle.IsPersistent)
                //    {
                //        vehicle.Delete();
                //    }
                //}

                else if(!string.IsNullOrEmpty(CurrentTeleportInterior))
                {
                    GameLocation toTele = placesOfInterest.PossibleLocations.InteractableLocations().Where(x => x.IsCorrectMap(world.IsMPMapLoaded) && x.Name == CurrentTeleportInterior).FirstOrDefault();
                    if(toTele != null && toTele.Interior != null)
                    {
                        toTele.Interior.Teleport(interactionable, toTele, null);
                    }
                }


                player.Character.Position = PlayerPosition;
                player.Character.Heading = PlayerHeading;   
            }
        }
        ////Info
        //public TabMissionSelectItem SaveTabInfo(ITimeReportable time, IGangs gangs, IWeapons weapons, IModItems modItems)
        //{
        //    List<MissionInformation> saveMissionInfos = new List<MissionInformation>();

        //    MissionInformation loadSubTab = new MissionInformation("Load", "Load the selected save", new List<Tuple<string, string>>());
        //    saveMissionInfos.Add(loadSubTab);


        //    MissionInformation demographicsSubTab = new MissionInformation("Demographics", "", DemographicsInfo());
        //    saveMissionInfos.Add(demographicsSubTab);

        //    MissionInformation affiliationsSubTab = new MissionInformation("Affiliations", "", AffiliationsInfo(gangs));
        //    saveMissionInfos.Add(affiliationsSubTab);

        //    MissionInformation needsSubTab = new MissionInformation("Human State", "", HumanStateInfo());
        //    saveMissionInfos.Add(needsSubTab);

        //    MissionInformation licensesSubTab = new MissionInformation("Licenses", "", LicenseInfo(time));
        //    saveMissionInfos.Add(licensesSubTab);

        //    MissionInformation vehiclesSubTab = new MissionInformation("Vehicles", "", VehicleInfo());
        //    saveMissionInfos.Add(vehiclesSubTab);

        //    MissionInformation weaponsSubTab = new MissionInformation("Weapons", "", WeaponInfo(weapons,modItems));
        //    saveMissionInfos.Add(weaponsSubTab);

        //    MissionInformation residenceSubTab = new MissionInformation("Residences", "", ResidenceInfo());
        //    saveMissionInfos.Add(residenceSubTab);


        //    MissionInformation deleteSubTab = new MissionInformation("Delete", "Deletes the selected save", new List<Tuple<string, string>>());
        //    saveMissionInfos.Add(deleteSubTab);

        //    TabMissionSelectItem GameSaveEntry = new TabMissionSelectItem($"{PlayerName}~s~", saveMissionInfos);
        //    return GameSaveEntry;
        //}
        //private List<Tuple<string, string>> DemographicsInfo()
        //{
        //    List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
        //    toreturn.Add(Tuple.Create("Name:", PlayerName));
        //    toreturn.Add(Tuple.Create("Money:", Money.ToString("C0")));
        //    toreturn.Add(Tuple.Create("Model Name:", ModelName));       
        //    return toreturn;
        //}
        //private List<Tuple<string, string>> AffiliationsInfo(IGangs gangs)
        //{
        //    List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
        //    toreturn.Add(Tuple.Create("Police Officer:", IsCop ? "Yes" : "No"));//store agency here
        //    toreturn.Add(Tuple.Create("EMT:", IsEMT ? "Yes" : "No"));//store agency here
        //    toreturn.Add(Tuple.Create("Fire Fighter:", IsFireFighter ? "Yes" : "No"));//store agency here
        //    toreturn.Add(Tuple.Create("Security Guard:", IsSecurityGuard ? "Yes" : "No"));//store agency here
        //    GangRepSave memberSave = GangReputationsSave.Where(x => x.IsMember).FirstOrDefault();
        //    if(memberSave != null)
        //    {
        //        Gang currentGang = gangs.GetGang(memberSave.GangID);
        //        if(currentGang != null)
        //        {
        //            toreturn.Add(Tuple.Create(currentGang.ShortName, currentGang.MemberName));
        //        }
        //    }
        //    return toreturn;
        //}
        //private List<Tuple<string, string>> HumanStateInfo()
        //{
        //    List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
        //    toreturn.Add(Tuple.Create("Thirst:", $"{Math.Round(ThirstValue, 0)}%"));
        //    toreturn.Add(Tuple.Create("Hunger:", $"{Math.Round(HungerValue, 0)}%"));
        //    toreturn.Add(Tuple.Create("Sleep:", $"{Math.Round(SleepValue, 0)}%"));
        //    return toreturn;
        //}
        //private List<Tuple<string, string>> LicenseInfo(ITimeReportable time)
        //{
        //    List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
        //    toreturn.Add(Tuple.Create("Drivers License:", DriversLicense?.IsValid(time) == true ? "Valid" : "Not Available"));
        //    toreturn.Add(Tuple.Create("CCW License:", CCWLicense?.IsValid(time) == true ? "Valid" : "Not Available"));
        //    toreturn.Add(Tuple.Create("Pilots License:", PilotsLicense?.IsValid(time) == true ? "Valid" : "Not Available"));
        //    return toreturn;
        //}
        //private List<Tuple<string, string>> VehicleInfo()
        //{
        //    List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
        //    foreach (VehicleSaveStatus savedVehicles in OwnedVehicleVariations)
        //    {
        //        string MakeName = "Unk";
        //        string ModelName = "Unk";
        //        if (savedVehicles.ModelName != "")
        //        {
        //            ModelName = NativeHelper.VehicleModelName(Game.GetHashKey(savedVehicles.ModelName));
        //            MakeName = NativeHelper.VehicleMakeName(Game.GetHashKey(savedVehicles.ModelName));
        //        }
        //        else
        //        {
        //            ModelName = NativeHelper.VehicleModelName(savedVehicles.ModelHash);
        //            MakeName = NativeHelper.VehicleMakeName(savedVehicles.ModelHash);
        //        }
        //        toreturn.Add(Tuple.Create(MakeName, ModelName));
        //    }
        //    return toreturn;
        //}
        //private List<Tuple<string, string>> WeaponInfo(IWeapons weapons, IModItems modItems)
        //{
        //    List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
        //    foreach (StoredWeapon sw in WeaponInventory)
        //    {
        //        WeaponInformation wi = weapons.GetWeapon(sw.WeaponHash);
        //        if (wi != null)
        //        {
        //            ModItem weaponItem = modItems.AllItems().Where(x => x.ModelItem != null && (x.ModelItem.ModelHash == sw.WeaponHash || x.ModelItem.ModelName == sw.WeaponHash.ToString())).FirstOrDefault();
        //            if(weaponItem != null)
        //            {
        //                toreturn.Add(Tuple.Create($"{weaponItem.Name}", $"Ammo: ({sw.Ammo})"));
        //            }
        //            else
        //            {
        //                toreturn.Add(Tuple.Create(wi.ModelName, $"Ammo: ({sw.Ammo})"));
        //            }
        //        }
        //        else
        //        {
        //            toreturn.Add(Tuple.Create(sw.WeaponHash.ToString(), $"Ammo: ({sw.Ammo})"));
        //        }
        //    }
        //    return toreturn;
        //}
        //private List<Tuple<string, string>> ResidenceInfo()
        //{
        //    List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
        //    foreach (SavedResidence wi in SavedResidences)
        //    {
        //        toreturn.Add(Tuple.Create("Residence:", wi.Name));
        //    }
        //    return toreturn;
        //}
        public string Title => $"{SaveNumber.ToString("D2")} - {PlayerName} ({(Money + (SavedBankAccounts == null ? 0 : SavedBankAccounts.Sum(x=> x.Money))).ToString("C0")}) - {CurrentDateTime.ToString("MM/dd/yyyy HH:mm")}";
        public string RightLabel => SaveDateTime.ToString("MM/dd/yyyy HH:mm");





    }

}
