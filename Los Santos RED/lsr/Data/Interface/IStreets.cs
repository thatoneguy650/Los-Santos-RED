using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IStreets
    {
        Street GetStreet(string currentCrossStreetName);
        Street GetStreet(int nodeID);
        Street GetStreet(Vector3 position);
        string GetStreetNames(Vector3 entrancePosition, bool withCross);
    }
}
