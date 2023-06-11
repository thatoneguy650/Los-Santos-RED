using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface ILocationAreaRestrictable
{
    string Name { get; }
    RestrictedAreas RestrictedAreas { get; }
    //bool IsPlayerInRestrictedArea { get;  }
    //void SetRestrictedArea(bool isInside);
    //void RemoveRestriction();
}

