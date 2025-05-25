public class ModKitDescription
{
    public ModKitDescription()
    {
    }

    public ModKitDescription(string name, int iD)
    {
        Name = name;
        ID = iD;
    }

    public string Name { get; set; }
    public int ID { get; set; }
    public override string ToString()
    {
        return Name;
    }
}