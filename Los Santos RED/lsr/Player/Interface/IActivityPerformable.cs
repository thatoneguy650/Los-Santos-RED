using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IActivityPerformable
    {
        bool IsPerformingActivity { get; set; }
        Ped Character { get; }
        string ModelName { get; }
        bool IsMale { get; }
        bool CanPerformActivities { get; }
        GangRelationships GangRelationships { get; }
        int Money { get; }

        void GiveMoney(int moneyToReceive);
        CellPhone CellPhone { get; }
        bool IsMoveControlPressed { get; }
        string FreeModeVoice { get; }
        bool CharacterModelIsFreeMode { get; }
        bool CanConverse { get; }
        bool IsInteractingWithLocation { get; set; }
        List<ButtonPrompt> ButtonPrompts { get; }

        void SetUnarmed();
        void StopDynamicActivity();
        void TakeOwnershipOfVehicle(VehicleExt myNewCar);
        void ConsumeItem(ModItem modItem);
        void AddToInventory(ModItem modItem, int v);
    }
}
