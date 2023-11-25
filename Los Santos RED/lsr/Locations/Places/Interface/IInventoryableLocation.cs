using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public interface IInventoryableLocation
    {
        void CreateInventoryMenu(bool withitems, bool withweapons, bool withcash);
        LocationCamera LocationCamera { get; }
        GameLocation GameLocation { get; }
    }
