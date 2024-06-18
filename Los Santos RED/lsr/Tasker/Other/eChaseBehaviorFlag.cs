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
		UseContinuousRam = 128,
		CantPullAlongside = 256,
		CantPullAlongsideInFront = 512,
    }
/*	VEHICLE_CHASE_CANT_BLOCK						= 1,
	VEHICLE_CHASE_CANT_BLOCK_FROM_PURSUE			= 2,
	VEHICLE_CHASE_CANT_PURSUE						= 4,
	VEHICLE_CHASE_CANT_RAM							= 8,
	VEHICLE_CHASE_CANT_SPIN_OUT						= 16,
	VEHICLE_CHASE_CANT_MAKE_AGGRESSIVE_MOVE			= 32,
	VEHICLE_CHASE_CANT_CRUISE_IN_FRONT_DURING_BLOCK	= 64,
	VEHICLE_CHASE_USE_CONTINUOUS_RAM				= 128,
	VEHICLE_CHASE_CANT_PULL_ALONGSIDE				= 256,
	VEHICLE_CHASE_CANT_PULL_ALONGSIDE_INFRONT		= 512

*/
//Flag 1: Aggressive ramming of suspect
//Flag 2: Ram attempts
//Flag 8: Medium-aggressive boxing tactic with a bit of PIT
//Flag 16: Ramming, seems to be slightly less aggressive than 1-2.
//Flag 32: Stay back from suspect, no tactical contact. Convoy-like.

