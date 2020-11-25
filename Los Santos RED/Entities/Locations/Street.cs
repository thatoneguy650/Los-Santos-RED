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
    public bool IsHighway = false;
    public List<string> DirectionsStopped = new List<string>();
    public Street()
    {

    }
    public Street(string _Name, float _SpeedLimit)
    {
        Name = _Name;
        SpeedLimit = _SpeedLimit;
    }
    public Street(string _Name, float _SpeedLimit, bool _isFreeway)
    {
        Name = _Name;
        SpeedLimit = _SpeedLimit;
        IsHighway = _isFreeway;
    }
}
