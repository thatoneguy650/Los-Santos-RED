using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ISeats
    {
        bool CanSit(Rage.Object currentLookedAtObject);
        SeatModel GetSeatModel(Rage.Object obj);
        float GetOffSet(Rage.Object closestSittableEntity);
    }
}
