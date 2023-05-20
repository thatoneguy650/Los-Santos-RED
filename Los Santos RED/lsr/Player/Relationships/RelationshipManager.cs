using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RelationshipManager
{
    private IGangs Gangs;
    private ISettingsProvideable Settings;
    private IPlacesOfInterest PlacesOfInterest;
    private ITimeControllable TimeControllable;
    public RelationshipManager(IGangs gangs, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, ITimeControllable timeControllable, IGangRelateable gangRelateable, IGunDealerRelateable gunDealerRelateable)
    {
        Gangs = gangs;
        Settings = settings;
        PlacesOfInterest = placesOfInterest;
        TimeControllable = timeControllable;
        GangRelationships = new GangRelationships(Gangs, gangRelateable, Settings, PlacesOfInterest, TimeControllable);
        GunDealerRelationship = new GunDealerRelationship(gunDealerRelateable, PlacesOfInterest);
        OfficerFriendlyRelationship = new OfficerFriendlyRelationship(gunDealerRelateable, PlacesOfInterest);
    }

    public OfficerFriendlyRelationship OfficerFriendlyRelationship { get; private set; }
    public GangRelationships GangRelationships { get; private set; }
    public GunDealerRelationship GunDealerRelationship { get; private set; }

    public void Dispose()
    {
        GangRelationships.Dispose();
        GunDealerRelationship.Dispose();
        OfficerFriendlyRelationship.Dispose();
    }

    public void Reset(bool sendText)
    {
        GangRelationships.Reset();
        OfficerFriendlyRelationship.Reset(sendText);
        GunDealerRelationship.Reset(sendText);
    }

    public void Setup()
    {
        GangRelationships.Setup();
        GunDealerRelationship.Setup();
        OfficerFriendlyRelationship.Setup();
    }
    public void SetCompleteTask(string contactName, int repAmountOnCompletion, bool joinGangOnComplete)
    {
        Gang myGang = Gangs.GetGangByContact(contactName);
        if (myGang != null)
        {
            if (repAmountOnCompletion != 0)
            {
                GangRelationships.ChangeReputation(myGang, repAmountOnCompletion, false);
            }
            GangRelationships.SetDebt(myGang, 0);
            GangRelationships.SetCompletedTask(myGang);
            if (joinGangOnComplete)
            {
                GangRelationships.SetGang(myGang, true);
            }
        }
        else if (contactName == StaticStrings.UndergroundGunsContactName)
        {
            if (repAmountOnCompletion != 0)
            {
                GunDealerRelationship.ChangeReputation(repAmountOnCompletion, false);
            }
            GunDealerRelationship.SetDebt(0);
        }
        else if (contactName == StaticStrings.OfficerFriendlyContactName)
        {
            if (repAmountOnCompletion != 0)
            {
                OfficerFriendlyRelationship.ChangeReputation(repAmountOnCompletion, false);
            }
            OfficerFriendlyRelationship.SetDebt(0);
        }
    }
    public void SetFailedTask(string contactName, int repAmountOnFail, int debtAmountOnFail)
    {
        Gang myGang = Gangs.GetGangByContact(contactName);
        if (myGang != null)
        {
            if (repAmountOnFail != 0)
            {
                GangRelationships.ChangeReputation(myGang, repAmountOnFail, false);
            }
            if (debtAmountOnFail != 0)
            {
                GangRelationships.AddDebt(myGang, debtAmountOnFail);
            }
            GangRelationships.SetFailedTask(myGang);
        }
        else if (contactName == StaticStrings.UndergroundGunsContactName)
        {
            if (repAmountOnFail != 0)
            {
                GunDealerRelationship.ChangeReputation(repAmountOnFail, false);
            }
            if (debtAmountOnFail != 0)
            {
                GunDealerRelationship.AddDebt(debtAmountOnFail);
            }
        }
        else if (contactName == StaticStrings.OfficerFriendlyContactName)
        {
            if (repAmountOnFail != 0)
            {
                OfficerFriendlyRelationship.ChangeReputation(repAmountOnFail, false);
            }
            if (debtAmountOnFail != 0)
            {
                OfficerFriendlyRelationship.AddDebt(debtAmountOnFail);
            }
        }
    }
}
//default groups and relationships 20221026
/*#	Acquaintance options:
#	- Hate
#	- Dislike 
#	- Like	
#	- Respect
PLAYER
CIVMALE	
CIVFEMALE	
COP 
SECURITY_GUARD
PRIVATE_SECURITY
FIREMAN
GANG_1
GANG_2
GANG_9
GANG_10
AMBIENT_GANG_LOST
AMBIENT_GANG_MEXICAN
AMBIENT_GANG_FAMILY
AMBIENT_GANG_BALLAS
AMBIENT_GANG_MARABUNTE
AMBIENT_GANG_CULT
AMBIENT_GANG_SALVA
AMBIENT_GANG_WEICHENG
AMBIENT_GANG_HILLBILLY
DEALER
HATES_PLAYER
HEN
WILD_ANIMAL
SHARK
COUGAR
NO_RELATIONSHIP
SPECIAL
MISSION2
MISSION3
MISSION4
MISSION5
MISSION6
MISSION7
MISSION8
ARMY
GUARD_DOG
AGGRESSIVE_INVESTIGATE
MEDIC
CAT

#
PLAYER
	Like PLAYER
CIVMALE	
#   Respect CIVMALE
#   Respect CIVFEMALE
CIVFEMALE	
#   Respect CIVFEMALE
#   Respect CIVMALE
COP 
    Respect MEDIC FIREMAN COP
    Respect ARMY
    Like SECURITY_GUARD
ARMY
    Like ARMY
    Respect COP 
SECURITY_GUARD
    Like COP SECURITY_GUARD GUARD_DOG
PRIVATE_SECURITY
	Like PRIVATE_SECURITY GUARD_DOG
PRISONER
	Like PRISONER
    Hate PLAYER
FIREMAN
    Respect MEDIC FIREMAN COP
GANG_1
	Respect GANG_1
GANG_2
	Respect GANG_2
GANG_9
	Respect GANG_9
GANG_10
	Respect GANG_10
HATES_PLAYER
	Hate PLAYER
	Like HATES_PLAYER AGGRESSIVE_INVESTIGATE
HEN
	Dislike PLAYER
AMBIENT_GANG_LOST
	Like AMBIENT_GANG_LOST GUARD_DOG
AMBIENT_GANG_MEXICAN
	Like AMBIENT_GANG_MEXICAN GUARD_DOG
AMBIENT_GANG_FAMILY
	Like AMBIENT_GANG_FAMILY GUARD_DOG
AMBIENT_GANG_BALLAS
	Like AMBIENT_GANG_BALLAS GUARD_DOG
AMBIENT_GANG_MARABUNTE
	Like AMBIENT_GANG_MARABUNTE GUARD_DOG
AMBIENT_GANG_CULT
	Like AMBIENT_GANG_CULT GUARD_DOG
AMBIENT_GANG_SALVA
	Like AMBIENT_GANG_SALVA GUARD_DOG
AMBIENT_GANG_WEICHENG
	Like AMBIENT_GANG_WEICHENG GUARD_DOG
AMBIENT_GANG_HILLBILLY
	Like AMBIENT_GANG_HILLBILLY GUARD_DOG
DOMESTIC_ANIMAL
	Hate PLAYER
	Like CIVMALE CIVFEMALE COP SECURITY_GUARD DOMESTIC_ANIMAL FIREMAN GANG_1 GANG_2 GANG_9 GANG_10 AMBIENT_GANG_LOST AMBIENT_GANG_MEXICAN AMBIENT_GANG_BALLAS AMBIENT_GANG_FAMILY DEALER HATES_PLAYER
WILD_ANIMAL
	Hate PLAYER CIVMALE CIVFEMALE COP SECURITY_GUARD FIREMAN GANG_1 GANG_2 GANG_9 GANG_10 AMBIENT_GANG_LOST AMBIENT_GANG_MEXICAN AMBIENT_GANG_BALLAS AMBIENT_GANG_FAMILY DEALER HATES_PLAYER
DEER
	Hate PLAYER CIVMALE CIVFEMALE COP SECURITY_GUARD FIREMAN GANG_1 GANG_2 GANG_9 GANG_10 AMBIENT_GANG_LOST AMBIENT_GANG_MEXICAN AMBIENT_GANG_BALLAS AMBIENT_GANG_FAMILY DEALER HATES_PLAYER
	Respect DEER
SHARK
	Hate PLAYER
GUARD_DOG
	Like GUARD_DOG CIVMALE CIVFEMALE SECURITY_GUARD AMBIENT_GANG_LOST AMBIENT_GANG_MEXICAN AMBIENT_GANG_FAMILY AMBIENT_GANG_BALLAS AMBIENT_GANG_MARABUNTE AMBIENT_GANG_CULT AMBIENT_GANG_SALVA AMBIENT_GANG_WEICHENG AMBIENT_GANG_HILLBILLY PRIVATE_SECURITY
AGGRESSIVE_INVESTIGATE
	Hate PLAYER
	Like HATES_PLAYER AGGRESSIVE_INVESTIGATE
MEDIC
	Like MEDIC
	Respect COP ARMY SECURITY_GUARD FIREMAN
COUGAR
	Hate PLAYER CIVMALE CIVFEMALE SECURITY_GUARD AMBIENT_GANG_LOST AMBIENT_GANG_MEXICAN AMBIENT_GANG_FAMILY AMBIENT_GANG_BALLAS AMBIENT_GANG_MARABUNTE AMBIENT_GANG_CULT AMBIENT_GANG_SALVA AMBIENT_GANG_WEICHENG AMBIENT_GANG_HILLBILLY PRIVATE_SECURITY COP ARMY PRISONER FIREMAN
CAT
	Hate PLAYER
	Like CIVMALE CIVFEMALE COP SECURITY_GUARD DOMESTIC_ANIMAL FIREMAN GANG_1 GANG_2 GANG_9 GANG_10 AMBIENT_GANG_LOST AMBIENT_GANG_MEXICAN AMBIENT_GANG_BALLAS AMBIENT_GANG_FAMILY DEALER HATES_PLAYER
*/