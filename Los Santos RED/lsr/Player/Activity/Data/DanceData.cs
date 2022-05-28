public class DanceData
{
    public DanceData(string name, string animationName)
    {
        Name = name;
        AnimationName = animationName;
    }
    public DanceData(string name, string animationDictionary, string animationName)
    {
        Name = name;
        AnimationName = animationName;
        AnimationDictionary = animationDictionary;
    }

    public DanceData(string name, string animationDictionary, string animationName, string animationEnter, string animationExit) : this(name, animationDictionary, animationName)
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
    public override string ToString()
    {
        return Name;
    }
}