public class HeadLookup
{
    public HeadLookup()
    {
    }
    public HeadLookup(int headID, string headName)
    {
        HeadID = headID;
        HeadName = headName;
    }

    public int HeadID { get; set; }
    public string HeadName { get; set; }
    public override string ToString()
    {
        return HeadName;
    }
}