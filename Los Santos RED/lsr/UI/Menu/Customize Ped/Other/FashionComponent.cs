public class FashionComponent
{
    public FashionComponent()
    {
    }
    public FashionComponent(int componentID, string componentName)
    {
        ComponentID = componentID;
        ComponentName = componentName;
    }

    public int ComponentID { get; set; }
    public string ComponentName { get; set; }
}