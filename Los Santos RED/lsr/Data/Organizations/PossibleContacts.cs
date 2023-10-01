using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PossibleContacts
{
    public PossibleContacts()
    {

    }
    public List<GangContact> GangContacts { get; private set; } = new List<GangContact>();
    public List<GunDealerContact> GunDealerContacts { get; private set; } = new List<GunDealerContact>();
    public EmergencyServicesContact EmergencyServicesContact { get; set; } = new EmergencyServicesContact();
    public CorruptCopContact CorruptCopContact { get; set; } = new CorruptCopContact();
    public List<KillerContact> KillerContacts { get; private set; } = new List<KillerContact>();
    public List<VehicleExporterContact> VehicleExporterContacts { get; private set; } = new List<VehicleExporterContact>();
    public List<TaxiServiceContact> TaxiServiceContacts { get; private set; } = new List<TaxiServiceContact>();
    public List<PhoneContact> AllContacts()
    {
        List<PhoneContact> AllLocations = new List<PhoneContact>();
        AllLocations.AddRange(GangContacts);
        AllLocations.AddRange(GunDealerContacts);
        AllLocations.Add(EmergencyServicesContact);
        AllLocations.AddRange(KillerContacts);
        AllLocations.AddRange(VehicleExporterContacts);
        AllLocations.AddRange(TaxiServiceContacts);
        AllLocations.Add(CorruptCopContact);
        return AllLocations;
    }
}
