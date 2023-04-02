using LosSantosRED.lsr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ISavedOutfits
    {
        List<SavedOutfit> SavedOutfitList { get; }
        void AddOutfit(SavedOutfit outfit);
        void RemoveOutfit(SavedOutfit so);
    }
}
