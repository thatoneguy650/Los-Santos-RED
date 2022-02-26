using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RandomHeadData
{
    public RandomHeadData()
    {

    }
    public RandomHeadData(int headID, string name, List<int> hairColors, List<int> hairComponents, bool isMale)
    {
        HeadID = headID;
        Name = name;
        HairColors = hairColors;
        HairComponents = hairComponents;
        IsMale = isMale;
    }

    public int HeadID { get; set; }
    public string Name { get; set; }
    public List<int> HairColors { get; set; }
    public List<int> HairComponents { get; set; }
    public bool IsMale { get; set; }
}