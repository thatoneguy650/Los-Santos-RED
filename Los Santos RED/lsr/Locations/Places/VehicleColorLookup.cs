public class VehicleColorLookup
{
    public VehicleColorLookup()
    {
    }
    public VehicleColorLookup(int colorID, string fullColorName, string colorGroup, string colorName, int order)
    {
        ColorID = colorID;
        FullColorName = fullColorName;
        ColorGroup = colorGroup;
        ColorName = colorName;
        Order = order;
    }

    public int ColorID { get; set; }
    public string FullColorName { get; set; }
    public string ColorGroup { get; set; }
    public string ColorName { get; set; }
    public int Order { get; set; }
    public System.Drawing.Color RGBColor { get; set; }
    public override string ToString()
    {
        return ColorName;
    }
}