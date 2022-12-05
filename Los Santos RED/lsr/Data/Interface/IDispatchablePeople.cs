using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IDispatchablePeople
    {
        List<DispatchablePersonGroup> AllPeople { get; }

        List<DispatchablePerson> GetPersonData(string dispatchablePersonGroupID);
    }
}