using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface ILocationGangAssignable
{
    ShopMenu Menu { get; set; }
    Gang AssociatedGang { get; set; }
    string ButtonPromptText { get; set; }
    string MenuID { get; }
   // string GangID { get; }
    void StoreData(IGangs gangs);
}

