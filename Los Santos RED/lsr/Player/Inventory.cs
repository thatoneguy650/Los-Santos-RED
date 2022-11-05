using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player
{
    [Serializable()]
    public class Inventory
    {
        private IInventoryable Player;
        private ISettingsProvideable Settings;
        private List<InventoryItem> ItemsList = new List<InventoryItem>();
        public List<InventoryItem> Items => ItemsList;
        public Inventory()
        {

        }
        public Inventory(IInventoryable player, ISettingsProvideable settings)
        {
            Player = player;
            Settings = settings;
        }




        //public List<InventoryItem> AllItems()
        //{
        //    List<InventoryItem> AllItems = new List<InventoryItem>();
        //    AllItems.AddRange(FlashlightInventoryItems);
        //    AllItems.AddRange(ShovelInventoryItems);
        //    AllItems.AddRange(LicensePlateInventoryItems);
        //    AllItems.AddRange(UmbrellaInventoryItems);
        //    AllItems.AddRange(FoodInventoryItems);
        //    AllItems.AddRange(SmokeInventoryItems);
        //    AllItems.AddRange(DrinkInventoryItems);
        //    AllItems.AddRange(PipeSmokeInventoryItems);
        //    AllItems.AddRange(IngestInventoryItems);
        //    AllItems.AddRange(InhaleInventoryItems);
        //    AllItems.AddRange(InjectInventoryItems);
        //    AllItems.AddRange(VehicleInventoryItems);
        //    AllItems.AddRange(WeaponInventoryItems);
        //    AllItems.AddRange(HotelStayInventoryItems);
        //    AllItems.AddRange(DrillInventoryItems);
        //    AllItems.AddRange(TapeInventoryItems);
        //    AllItems.AddRange(ScrewdriverInventoryItems);
        //    AllItems.AddRange(LighterInventoryItems);
        //    AllItems.AddRange(PliersInventoryItems);
        //    AllItems.AddRange(HammerInventoryItems);
        //    AllItems.AddRange(BongInventoryItems);
        //    return AllItems;
        //}


        //public List<FlashlightInventoryItem> FlashlightInventoryItems { get; private set; } = new List<FlashlightInventoryItem>();
        //public List<ShovelInventoryItem> ShovelInventoryItems { get; private set; } = new List<ShovelInventoryItem>();
        //public List<UmbrellaInventoryItem> UmbrellaInventoryItems { get; private set; } = new List<UmbrellaInventoryItem>();
        //public List<LicensePlateInventoryItem> LicensePlateInventoryItems { get; private set; } = new List<LicensePlateInventoryItem>();
        //public List<LighterInventoryItem> LighterInventoryItems { get; private set; } = new List<LighterInventoryItem>();
        //public List<ScrewdriverInventoryItem> ScrewdriverInventoryItems { get; private set; } = new List<ScrewdriverInventoryItem>();
        //public List<TapeInventoryItem> TapeInventoryItems { get; private set; } = new List<TapeInventoryItem>();
        //public List<DrillInventoryItem> DrillInventoryItems { get; private set; } = new List<DrillInventoryItem>();
        //public List<HammerInventoryItem> HammerInventoryItems { get; private set; } = new List<HammerInventoryItem>();
        //public List<PliersInventoryItem> PliersInventoryItems { get; private set; } = new List<PliersInventoryItem>();
        //public List<BongInventoryItem> BongInventoryItems { get; private set; } = new List<BongInventoryItem>();
        //public List<FoodInventoryItem> FoodInventoryItems { get; private set; } = new List<FoodInventoryItem>();
        //public List<SmokeInventoryItem> SmokeInventoryItems { get; private set; } = new List<SmokeInventoryItem>();
        //public List<PipeSmokeInventoryItem> PipeSmokeInventoryItems { get; private set; } = new List<PipeSmokeInventoryItem>();
        //public List<DrinkInventoryItem> DrinkInventoryItems { get; private set; } = new List<DrinkInventoryItem>();
        //public List<InhaleInventoryItem> InhaleInventoryItems { get; private set; } = new List<InhaleInventoryItem>();
        //public List<IngestInventoryItem> IngestInventoryItems { get; private set; } = new List<IngestInventoryItem>();
        //public List<InjectInventoryItem> InjectInventoryItems { get; private set; } = new List<InjectInventoryItem>();
        //public List<HotelStayInventoryItem> HotelStayInventoryItems { get; private set; } = new List<HotelStayInventoryItem>();
        //public List<WeaponInventoryItem> WeaponInventoryItems { get; private set; } = new List<WeaponInventoryItem>();
        //public List<VehicleInventoryItem> VehicleInventoryItems { get; private set; } = new List<VehicleInventoryItem>();


















        public void Reset()
        {
            Clear();
        }
        public void Clear()
        {
            ItemsList.Clear();
        }


        public bool Has(Type type)
        {
            return ItemsList.FirstOrDefault(x => x.ModItem.GetType() == type) != null;
        }
        public InventoryItem Get(Type type)
        {
            return ItemsList.Where(x => x.ModItem.GetType() == type).OrderBy(x=> x.RemainingPercent).FirstOrDefault();
        }




        public void Add(ModItem modItem, float remainingPercent)
        {
            if (modItem != null)
            {
                InventoryItem ExistingItem = Get(modItem);
                if (ExistingItem == null)
                {
                    ItemsList.Add(new InventoryItem(modItem, Settings) { RemainingPercent = remainingPercent });
                }
                else
                {
                    ExistingItem.RemainingPercent += remainingPercent;
                }
            }
        }
        public InventoryItem Get(ModItem modItem)
        {
            return ItemsList.FirstOrDefault(x => x.ModItem.Name == modItem.Name);
        }
        public void Use(ModItem modItem)
        {
            if (modItem != null)
            {
                if (modItem.PercentLostOnUse > 0.0f)
                {
                    Get(modItem)?.RemovePercent(modItem.PercentLostOnUse);                
                }
                else
                {
                    Remove(modItem, 1);
                }
            }
        }
        public bool Remove(ModItem modItem)
        {
            if (modItem != null)
            {
                InventoryItem ExistingItem = Get(modItem);
                if (ExistingItem != null)
                {
                    ItemsList.Remove(ExistingItem);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public bool Remove(ModItem modItem, int amount)
        {
            if (modItem != null)
            {
                InventoryItem ExistingItem = Get(modItem);
                if (ExistingItem != null)
                {
                    if (ExistingItem.Amount > amount)
                    {
                        ExistingItem.RemovePercent(amount);
                    }
                    else
                    {
                        ItemsList.Remove(ExistingItem);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
