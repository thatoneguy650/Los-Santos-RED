using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface ILocationAgencyAssignable
{
    string AssignedAgencyID { get; }
    Agency AssignedAgency { get; set; }

    void StoreData(IAgencies agencies);
}

