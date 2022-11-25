using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IDispatches
{
    string RadioStartBeep();
    string RadioEndBeep();
    AudioSet AttentionAllUnits();
    DispatchReportStaticData GetStaticData(string dispatchID);
    AudioSet OfficersReport();
    AudioSet CiviliansReport();
    List<AudioSet> UnitEnRouteSet();
    AudioSet RespondCode2Set();
    AudioSet RespondCode3Set();
    List<string> GetCallsignScannerAudio(int division, string unityType, int beatNumber);
    string AttentionSpecificUnit();
    string UnitStart();
    ZoneLookup GetZoneScannerAudio(string internalGameName);


    List<AudioSet> SuspectDriving();//needs lots of little ones, maybe combine them?

}

