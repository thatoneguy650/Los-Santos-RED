using Blackjack;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ICasinoGamePlayable 
    {
        bool IsTransacting { get; set; }
        BankAccounts BankAccounts { get; }
        string PlayerName { get; }
        bool IsMoveControlPressed { get; }

        GamblingManager GamblingManager { get; }
        ButtonPrompts ButtonPrompts { get; }
        RelationshipManager RelationshipManager { get; }
    }
}
