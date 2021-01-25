using System;

[Serializable]
public class RelationshipGroupExt
{
    public RelationshipGroupExt()
    {
    }

    public RelationshipGroupExt(string name, string internalName, string memberName)
    {
        Name = name;
        InternalName = internalName;
        MemberName = memberName;
    }

    public string Name { get; set; }
    public string InternalName { get; set; }
    public string MemberName { get; set; }
}