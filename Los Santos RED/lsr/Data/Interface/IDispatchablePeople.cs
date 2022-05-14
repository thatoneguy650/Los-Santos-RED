using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IDispatchablePeople
    {
        List<DispatchablePerson> GetPersonData(string dispatchablePersonGroupID);
    }
}