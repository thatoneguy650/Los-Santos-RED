public class FashionProp
{
    public FashionProp()
    {
    }
    public FashionProp(int propID, string propName)
    {
        PropID = propID;
        PropName = propName;
    }

    public int PropID { get; set; }
    public string PropName { get; set; }
}