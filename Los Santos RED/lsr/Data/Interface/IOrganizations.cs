using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IOrganizations
    {
        PossibleOrganizations PossibleOrganizations { get; }

        TaxiFirm GetDefaultTaxiFirm();
        Organization GetOrganizationByContact(string contactName);
        TaxiFirm GetRandomTaxiFirm(bool includeRideShare);
        TaxiFirm GetTaxiFirmFromVehicle(string v, int liveryID);
        List<Organization> GetOrganizations();
    }
}
