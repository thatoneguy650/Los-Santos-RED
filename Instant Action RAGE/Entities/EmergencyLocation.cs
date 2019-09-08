
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class EmergencyLocation
{
    internal enum EmergencyLocationType : int
    {
        Police = 0,
        Hospital = 1,
    }
    public Vector3 Location;
    public float Heading;
    public EmergencyLocationType Type;
    public String Name;
    public EmergencyLocation()
    {

    }
    public EmergencyLocation(Vector3 _Location,float _Heading, EmergencyLocationType _Type,String _Name)
    {
        Location = _Location;
        Heading = _Heading;
        Type = _Type;
        Name = _Name;
    }
}

