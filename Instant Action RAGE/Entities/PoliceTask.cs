using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    class PoliceTask
    {
        public Task TaskToAssign { get; set; }
        public GTACop CopToAssign { get; set; }

        public enum Task
        {
            Chase = 0,
            Arrest = 1,
            Untask = 2,
        }

        public PoliceTask(GTACop _CopToAssign,Task _TaskToAssign)
        {
            CopToAssign = _CopToAssign;
            TaskToAssign = _TaskToAssign;
        }
    }

