using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public enum eChaseBehaviorFlag : int
    {       
        FullContact = 1, 
        MediumContact = 2,
        PIT = 8,
        LowContact = 16,
        NoContact = 32,
    }

//Flag 1: Aggressive ramming of suspect
//Flag 2: Ram attempts
//Flag 8: Medium-aggressive boxing tactic with a bit of PIT
//Flag 16: Ramming, seems to be slightly less aggressive than 1-2.
//Flag 32: Stay back from suspect, no tactical contact. Convoy-like.

