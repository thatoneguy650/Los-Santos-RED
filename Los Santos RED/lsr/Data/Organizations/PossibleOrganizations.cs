using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PossibleOrganizations
{
    public List<Organization> GeneralOrganizations { get; set; }
    public List<TaxiFirm> TaxiFirms { get; set;}
    public List<Organization> AllOrganizations()
    {
        List<Organization> AllOrgs = new List<Organization>();
        AllOrgs.AddRange(GeneralOrganizations);
        AllOrgs.AddRange(TaxiFirms);
        return AllOrgs;
    }
}
