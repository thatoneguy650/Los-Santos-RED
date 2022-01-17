﻿using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IWeaponIssuable
    {

        Ped Pedestrian { get; }
        ComplexTask CurrentTask { get; }
        bool IsInVehicle { get; }
        bool IsInHelicopter { get; }
        bool IsDriver { get; }
        bool IsCop { get; }
        int CombatAbility { get; }
        int Accuracy { get; }
        int ShootRate { get; }

        IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons);
    }
}