public class ColorLookup
{
    public ColorLookup()
    {
    }
    public ColorLookup(int colorID, string colorName)
    {
        ColorID = colorID;
        ColorName = colorName;
    }

    public int ColorID { get; set; }
    public string ColorName { get; set; }
    public override string ToString()
    {
        return ColorName;
    }
}