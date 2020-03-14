using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CopTask
{
    public Tasking.AssignableTasks TaskToAssign { get; set; }
    public GTACop CopToAssign { get; set; }
    public uint GameTimeAssigned { get; set; }



    public CopTask(GTACop _CopToAssign, Tasking.AssignableTasks _TaskToAssign)
    {
        CopToAssign = _CopToAssign;
        TaskToAssign = _TaskToAssign;
        CopToAssign.TaskType = _TaskToAssign;
    }
    public CopTask(GTACop _CopToAssign, Tasking.AssignableTasks _TaskToAssign,uint _GameTimeAssigned)
    {
        CopToAssign = _CopToAssign;
        TaskToAssign = _TaskToAssign;
        CopToAssign.TaskType = _TaskToAssign;
        GameTimeAssigned = _GameTimeAssigned;
    }
}

