using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CivilianTask
{
    public Tasking.AssignableTasks TaskToAssign { get; set; }
    public GTAPed CivilianToAssign { get; set; }
    public uint GameTimeAssigned { get; set; }

    public CivilianTask(GTAPed _PedToAssign, Tasking.AssignableTasks _TaskToAssign)
    {
        CivilianToAssign = _PedToAssign;
        TaskToAssign = _TaskToAssign;
        CivilianToAssign.TaskType = _TaskToAssign;
    }
    public CivilianTask(GTAPed _PedToAssign, Tasking.AssignableTasks _TaskToAssign, uint _GameTimeAssigned)
    {
        CivilianToAssign = _PedToAssign;
        TaskToAssign = _TaskToAssign;
        CivilianToAssign.TaskType = _TaskToAssign;
        GameTimeAssigned = _GameTimeAssigned;
    }
}

