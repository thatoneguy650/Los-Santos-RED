using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DrawableIcon
{
    public DrawableIcon()
    {
    }

    public DrawableIcon(Texture icon, RectangleF rectangle)
    {
        Icon = icon;
        Rectangle = rectangle;
    }

    public Texture Icon { get; set; }
    public RectangleF Rectangle { get; set; }
}

