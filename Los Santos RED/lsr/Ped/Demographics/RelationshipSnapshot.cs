using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RelationshipSnapshot
{
    public RelationshipSnapshot(RelationshipGroup primaryGroup, RelationshipGroup secondaryGroup, Relationship primaryRelationship, Relationship secondaryRelationship)
    {
        PrimaryGroup = primaryGroup;
        SecondaryGroup = secondaryGroup;
        PrimaryRelationship = primaryRelationship;
        SecondaryRelationship = secondaryRelationship;
    }
    public RelationshipGroup PrimaryGroup { get; set; }
    public RelationshipGroup SecondaryGroup { get; set; }
    public Relationship PrimaryRelationship { get; set; }
    public Relationship SecondaryRelationship { get; set; }
}

