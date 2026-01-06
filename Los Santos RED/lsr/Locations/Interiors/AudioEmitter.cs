using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class AudioEmitter
{
    public string ID { get; set; }
    public string Name { get; set; }
    
    public AudioEmitter()
    {
    }

    public AudioEmitter(string iD, string name)
    {
        ID = iD;
        Name = name;
    }


    public void SetStation(string stationName)
    {
        NativeFunction.Natives.SET_EMITTER_RADIO_STATION(ID, stationName);
        if (stationName == "RADIO_OFF")
        {
            NativeFunction.Natives.SET_STATIC_EMITTER_ENABLED(ID, false);
            NativeFunction.Natives.SET_MOBILE_PHONE_RADIO_STATE(false);
        }
        else
        {
            NativeFunction.Natives.SET_STATIC_EMITTER_ENABLED(ID, true);
            NativeFunction.Natives.SET_MOBILE_PHONE_RADIO_STATE(true);
        }     
    }
}

