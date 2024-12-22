using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BlacklistedPedComponent
{
    public BlacklistedPedComponent()
    {
    }

    public BlacklistedPedComponent(int componentID, List<int> drawableIDs, int startingDrawableID, bool isMale)
    {
        ComponentID = componentID;
        DrawableIDs = drawableIDs;
        StartingDrawableID = startingDrawableID;
        IsMale = isMale;
    }

    public int ComponentID { get; set; }
    public List<int> DrawableIDs { get; set; }
    public int StartingDrawableID { get; set; }
    public bool IsMale { get; set; }
 }

