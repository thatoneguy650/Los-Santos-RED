using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ColorMapper
{

    //create the dictionary with the elements you are interested in
    private static Dictionary<int, String> colorMap = new Dictionary<int, String>()
    {
        //{0xFFB6C1, "Light Pink"},
        //{0x6B8E23, "Olive Drab"},
        {Color.Red.ToArgb(), Color.Red.Name},


                    
            {Color.Aqua.ToArgb(),Color.Aqua.Name },
            {Color.Beige.ToArgb(),Color.Beige.Name },
            {Color.Black.ToArgb(),Color.Black.Name },
            {Color.Blue.ToArgb(),Color.Blue.Name },
            {Color.Brown.ToArgb(),Color.Brown.Name },
            {Color.DarkBlue.ToArgb(),Color.DarkBlue.Name },
            {Color.DarkGreen.ToArgb(),Color.DarkGreen.Name },
            {Color.DarkGray.ToArgb(),Color.DarkGray.Name },
            {Color.DarkOrange.ToArgb(), Color.DarkOrange.Name},
            {Color.DarkRed.ToArgb(),Color.DarkRed.Name },
            {Color.Gold.ToArgb(),Color.Gold.Name},
            {Color.Green.ToArgb(),Color.Green.Name },
            {Color.Gray.ToArgb(),Color.Gray.Name },
            {Color.LightBlue.ToArgb(),Color.LightBlue.Name },
            {Color.Maroon.ToArgb(), Color.Maroon.Name},
            {Color.Orange.ToArgb(),Color.Orange.Name },
            {Color.Pink.ToArgb(),Color.Pink.Name },
            {Color.Purple.ToArgb(),Color.Purple.Name },
            {Color.Silver.ToArgb(),Color.Silver.Name },
            {Color.White.ToArgb(),Color.White.Name },
            {Color.Yellow.ToArgb(),Color.Yellow.Name }



        //and the list goes on
    };

    public static String GetName(Color color)
    {
        //mask out the alpha channel
        int myRgb = (int)(color.ToArgb() & 0x00FFFFFF);
        if (colorMap.ContainsKey(myRgb))
        {
            return colorMap[myRgb];
        }
        return null;
    }
    public static String GetNearestName(Color color)
    {
        //check first for an exact match
        String name = GetName(color);
        if (color != null)
        {
            return name;
        }
        //mask out the alpha channel
        int myRgb = (int)(color.ToArgb() & 0x00FFFFFF);
        //retrieve the color from the dictionary with the closest measure
        int closestColor = colorMap.Keys.Select(colorKey => new ColorDistance(colorKey, myRgb)).OrderBy(d => d.distance).FirstOrDefault().colorKey;
        //return the name
        return colorMap[closestColor];
    }
}

//Just a simple utility class to store our
//color values and the distance from the color of interest
public class ColorDistance
{
    private int _colorKey;
    public int colorKey
    {
        get { return _colorKey; }
    }
    private int _distance;
    public int distance
    {
        get { return _distance; }
    }

    public ColorDistance(int colorKeyRgb, int rgb2)
    {
        //store for use at end of query
        this._colorKey = colorKeyRgb;

        //we just pull the individual color components out
        byte r1 = (byte)((colorKeyRgb >> 16) & 0xff);
        byte g1 = (byte)((colorKeyRgb >> 8) & 0xff);
        byte b1 = (byte)((colorKeyRgb) & 0xff);

        byte r2 = (byte)((rgb2 >> 16) & 0xff);
        byte g2 = (byte)((rgb2 >> 8) & 0xff);
        byte b2 = (byte)((rgb2) & 0xff);

        //provide a simple distance measure between colors
        _distance = Math.Abs(r1 - r2) + Math.Abs(g1 - g2) + Math.Abs(b1 - b2);
    }
}

