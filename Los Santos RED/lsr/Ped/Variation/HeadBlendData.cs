using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
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
    public int shapeFirst { get; set; } = -1;
    public int shapeSecond { get; set; } = -1;
    public int shapeThird { get; set; } = -1;
    public int skinFirst { get; set; } = -1;
    public int skinSecond { get; set; } = -1;
    public int skinThird { get; set; } = -1;
    public float shapeMix { get; set; } = 0.0f;
    public float skinMix { get; set; } = 0.0f;
    public float thirdMix { get; set; } = 0.0f;
}

