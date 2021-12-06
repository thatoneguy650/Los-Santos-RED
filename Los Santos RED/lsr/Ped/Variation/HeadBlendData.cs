using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HeadBlendData
{
    public HeadBlendData()
    {
    }

    public HeadBlendData(int shapeFirst, int shapeSecond, int shapeThird, int skinFirst, int skinSecond, int skinThird, float shapeMix, float skinMix, float thirdMix)
    {
        this.shapeFirst = shapeFirst;
        this.shapeSecond = shapeSecond;
        this.shapeThird = shapeThird;
        this.skinFirst = skinFirst;
        this.skinSecond = skinSecond;
        this.skinThird = skinThird;
        this.shapeMix = shapeMix;
        this.skinMix = skinMix;
        this.thirdMix = thirdMix;
    }

    public int shapeFirst { get; set; }
    public int shapeSecond { get; set; }
    public int shapeThird { get; set; }
    public int skinFirst { get; set; }
    public int skinSecond { get; set; }
    public int skinThird { get; set; }
    public float shapeMix { get; set; }
    public float skinMix { get; set; }
    public float thirdMix { get; set; }
}

