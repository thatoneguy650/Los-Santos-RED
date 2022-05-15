using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FaceFeature
{
    public int Index { get; set; }
    public float Scale { get; set; }
    public FaceFeature()
    {

    }

    public FaceFeature(int index, float scale)
    {
        Index = index;
        Scale = scale;
    }
}

