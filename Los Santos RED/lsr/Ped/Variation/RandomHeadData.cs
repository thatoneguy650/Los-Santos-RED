using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class RandomHeadData
{
    public RandomHeadData()
    {

    }
    public RandomHeadData(int headID, string name, List<int> hairColors, List<int> hairComponents, List<int> eyeColors, bool isMale)
    {
        HeadID = headID;
        Name = name;
        HairColors = hairColors;
        HairComponents = hairComponents;
        EyeColors = eyeColors;
        IsMale = isMale;
    }

    public int HeadID { get; set; }
    public string Name { get; set; }
    public List<int> HairColors { get; set; }
    public List<int> HairComponents { get; set; }
    public List<int> EyeColors { get; set; }
    public bool IsMale { get; set; }

    public float FacialHairPercentage { get; set; } = 20f;
    public float LipstickAndMakeupPercentage { get; set; } = 70f;
    public float AgingPercentage { get; set; } = 20f;

}