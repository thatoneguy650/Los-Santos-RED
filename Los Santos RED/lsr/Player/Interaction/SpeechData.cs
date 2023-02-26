using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SpeechData
{
    public SpeechData()
    {

    }
    public SpeechData(string name, string description, string groupName, string simpleName, string subName)
    {
        Name = name;
        Description = description;
        GroupName = groupName;
        SimpleName = simpleName;
        SubName = subName;
    }


    public SpeechData(string name, string description, string groupName)
    {
        Name = name;
        Description = description;
        GroupName = groupName;
        SimpleName = name;
        SubName = name;
    }
    public string Name { get; set; }
    public string Description { get; set; }   
    public string GroupName { get; set; }
    public string SimpleName { get; set; }
    public string SubName { get; set; }
    public bool CanUseInConversation { get; set; } = false;
    public eSpeechType SpeechType { get; set; } = eSpeechType.None;
}

