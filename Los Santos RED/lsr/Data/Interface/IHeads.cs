using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IHeads
    {
        List<RandomHeadData> GetHeadData(string headDataGroupID);
    }
}