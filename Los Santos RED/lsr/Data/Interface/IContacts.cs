using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IContacts
    {
        GangContact GetGangContactData(string contactName);
        PhoneContact GetContactData(string contactName);
        List<PhoneContact> GetDefaultContacts();
        PhoneContact GetContactByNumber(string numpadString);
        PossibleContacts PossibleContacts { get; }
    }
}
