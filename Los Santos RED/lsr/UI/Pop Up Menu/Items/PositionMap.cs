public class PositionMap
{
    public PositionMap()
    {

    }
    public PositionMap(int id, string display, float posX, float posY)
    {
        ID = id;
        Display = display;
        PositionX = posX;
        PositionY = posY;
    }
    public int ID { get; set; }
    public string Display { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }
}