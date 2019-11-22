using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTAStreet
{
    public string Name = "";
    public float SpeedLimit = 50f;
    public string DispatchFile = "";
    public bool isFreeway = false;
    public GTAStreet(string _Name,float _SpeedLimit)
    {
        Name = _Name;
        SpeedLimit = _SpeedLimit;
    }
    public GTAStreet(string _Name, float _SpeedLimit,string _DispatchFile)
    {
        Name = _Name;
        SpeedLimit = _SpeedLimit;
        DispatchFile = _DispatchFile;
    }
    public GTAStreet(string _Name, float _SpeedLimit, string _DispatchFile,bool _isFreeway)
    {
        Name = _Name;
        SpeedLimit = _SpeedLimit;
        DispatchFile = _DispatchFile;
        isFreeway = _isFreeway;
    }
}

