using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPlayerVoiceable
    {
        bool CharacterModelIsFreeMode { get; }
        string FreeModeVoice { get; }
        Ped Character { get; }
        bool IsAliveAndFree { get; }
        bool IsIncapacitated { get; }
        bool IsWanted { get; }
        int WantedLevel { get; }
        Stance Stance { get; }
        WeaponEquipment WeaponEquipment { get; }
        bool CharacterModelIsPrimaryCharacter { get; }
    }
}
