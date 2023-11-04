using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GroupMember
{
    public GroupMember(PedExt pedExt, int index)
    {
        PedExt = pedExt;
        Index = index;
    }

    public PedExt PedExt { get; set; }
    public int Index { get; set; }
   // public string Description => PedExt == null ? "Unknown" : $"{PedExt.Name} - {PedExt.willfi}";
}

