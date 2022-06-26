using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IGestures
    {
        List<GestureData> GestureLookups { get; }

        GestureData GetRandomGesture();
    }
}
