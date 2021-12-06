using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Street
{
    public string Name = "";
    public float SpeedLimit = 50f;
    private string SpeedLimitUnits = "MPH";
    public float SpeedLimitKMH => SpeedLimitUnits == "KM/H" ? SpeedLimit : SpeedLimit * 1.60933f;
    public float SpeedLimitMPH => SpeedLimitUnits == "MPH" ? SpeedLimit : SpeedLimit * 0.621371f;
    public bool IsHighway = false;
    public List<string> DirectionsStopped = new List<string>();
    public Street()
    {

    }
    public Street(string _Name, float _SpeedLimit, string speedLimitUnits)
    {
        Name = _Name;
        SpeedLimit = _SpeedLimit;
        SpeedLimitUnits = speedLimitUnits;
    }
    public Street(string _Name, float _SpeedLimit, string speedLimitUnits, bool _isFreeway)
    {
        Name = _Name;
        SpeedLimit = _SpeedLimit;
        IsHighway = _isFreeway;
        SpeedLimitUnits = speedLimitUnits;
    }
}
