using System;

[Serializable]
public class PedGroup
{
    public PedGroup()
    {
    }

    public PedGroup(string name, string internalName, string memberName, bool canChangeRelationship)
    {
        Name = name;
        InternalName = internalName;
        MemberName = memberName;
        CanChangeRelationship = canChangeRelationship;
    }

    public string Name { get; set; }
    public string InternalName { get; set; } = "";
    public string MemberName { get; set; }
    public bool CanChangeRelationship { get; set; }
}