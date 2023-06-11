public class GestureData
{
    public GestureData()
    {

    }
    public GestureData(string name, string animationName)
    {
        Name = name;
        AnimationName = animationName;
    }
    public GestureData(string name, string animationDictionary, string animationName)
    {
        Name = name;
        AnimationName = animationName;
        AnimationDictionary = animationDictionary;
    }

    public GestureData(string name, string animationDictionary, string animationName, string animationEnter, string animationExit) : this(name, animationDictionary, animationName)
    {
        AnimationEnter = animationEnter;
        AnimationExit = animationExit;
    }

    public string Name { get; set; } = "";
    public string AnimationName { get; set; } = "";
    public string AnimationDictionary { get; set; } = "";
    public string AnimationEnter { get; set; } = "";
    public string AnimationExit { get; set; } = "";
    public bool IsInsulting { get; set; } = false;
    public bool IsOnActionWheel { get; set; } = false;
    public bool SetRepeat { get; set; } = false;
    public bool IsWholeBody { get; set; } = false;
    public string Category { get; set; } = "Gesture";
    public override string ToString()
    {
        return Name;
    }
}