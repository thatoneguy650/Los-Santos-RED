using LosSantosRED.lsr.Interface;
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
}

