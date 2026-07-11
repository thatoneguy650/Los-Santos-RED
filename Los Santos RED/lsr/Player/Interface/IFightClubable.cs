using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IFightClubable
    {
        Dispatcher Dispatcher { get; }
        Ped Character { get;  }
        bool IsDead { get; }
        string PlayerName { get; }
        BankAccounts BankAccounts { get; }
        //ButtonPrompts ButtonPrompts { get; }
        ActivityManager ActivityManager { get; }
        ButtonPrompts ButtonPrompts { get; }
        HealthManager HealthManager { get; }
        Respawning Respawning { get; }
        bool DisableMainMenu { get; set; }
    }
}
