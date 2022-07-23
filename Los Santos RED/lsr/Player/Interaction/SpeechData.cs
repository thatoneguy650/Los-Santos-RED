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
    public SpeechData(string name, string description, bool isInsult, string groupName, string simpleName, string subName, bool isGeneric)
    {
        Name = name;
        Description = description;
        IsInsult = isInsult;
        GroupName = groupName;
        SimpleName = simpleName;
        SubName = subName;
        IsGeneric = isGeneric;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsInsult { get; set; } = false;
    public string GroupName { get; set; }
    public string SimpleName { get; set; }
    public string SubName { get; set; }
    public bool IsGeneric { get; set; } = false;


}

