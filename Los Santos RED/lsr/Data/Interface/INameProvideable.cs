using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface INameProvideable
    {
        string GetRandomDogName(bool isMale);
        string GetRandomName(bool isMale);
    }
}
