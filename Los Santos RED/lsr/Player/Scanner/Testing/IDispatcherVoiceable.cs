using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IDispatcherVoiceable
{
    List<Dispatch> DispatchList { get; }
    IScannerDispatchableInformation SDI { get; }
    void Setup();
    CrimeDispatch GetCrimeDispatch(Crime crimeAssociated);

    IVehicleScannerAudio VehicleScannerAudio { get; }

}

