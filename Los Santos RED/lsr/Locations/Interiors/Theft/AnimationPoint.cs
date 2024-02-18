using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AnimationPoint
{
    public AnimationPoint()
    {
    }

    public AnimationPoint(int order, float position, bool visible)
    {
        Order = order;
        Position = position;
        Visible = visible;
    }

    public int Order { get; set; }
    public float Position { get; set; }
    public bool Visible { get; set; }
}

