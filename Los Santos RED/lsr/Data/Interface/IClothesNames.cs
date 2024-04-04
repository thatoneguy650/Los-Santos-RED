using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IClothesNames
    {
        HashSet<FashionItemLookup> PedOverlays { get; }

        string GetName(bool isProp, int ItemID, int DrawableID, int TextureID, string Gender);
        FashionItemLookup GetItemFast(bool isProp, int ItemID, int DrawableID, int TextureID, string Gender);
        HashSet<FashionItemLookup> GetItemsFast(bool isProp, int ItemID, int DrawableID, string Gender);
    }
}
