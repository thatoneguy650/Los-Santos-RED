public class DanceData
{
    public DanceData()
    {

    }
    public DanceData(string name, string animationName)
    {
        Name = name;
        AnimationIdle = animationName;
    }
    public DanceData(string name, string animationDictionary, string animationIdle)
    {
        Name = name;
        AnimationIdle = animationIdle;
        AnimationDictionary = animationDictionary;
    }

    public DanceData(string name, string animationDictionary, string animationIdle, string animationEnter, string animationExit) : this(name, animationDictionary, animationIdle)
    {
        AnimationEnter = animationEnter;
        AnimationExit = animationExit;
    }
    public string Name { get; set; } = ""; 
    public string AnimationDictionary { get; set; } = "";
    public string AnimationEnter { get; set; } = "";
    public string AnimationIdle { get; set; } = "";
    public string AnimationExit { get; set; } = "";
    public string FacialAnimationEnter { get; set; } = "";
    public string FacialAnimationIdle { get; set; } = "";
    public string FacialAnimationExit { get; set; } = "";
    public bool IsInsulting { get; set; } = false;
    public bool IsVehicle { get; set; } = false;
    public bool IsOnActionWheel { get; set; } = false;
    public override string ToString()
    {
        return Name;
    }
}