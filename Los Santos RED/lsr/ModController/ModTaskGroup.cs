using System.Collections.Generic;
using System.Linq;

public class ModTaskGroup
{
    private int CurrentTaskOrderID;
    public ModTaskGroup(string name, List<ModTask> tasksToRun)
    {
        Name = name;
        TasksToRun = tasksToRun;
    }
    public string Name { get; set; }
    public bool IsRunning { get; set; } = true;
    public List<ModTask> TasksToRun { get; set; }
    public void Update()
    {
        if (IsRunning)
        {
            if (CurrentTaskOrderID > TasksToRun.Count)
            {
                CurrentTaskOrderID = 0;
            }
            ModTask taskToRun = TasksToRun.Where(x => x.ShouldRun && x.RunOrder == CurrentTaskOrderID).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
            if (taskToRun != null)
            {
                taskToRun.Run();
                CurrentTaskOrderID++;
            }
            else
            {
                ModTask altTaskToRun = TasksToRun.Where(x => x.ShouldRun).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
                if (altTaskToRun != null)
                {
                    altTaskToRun.Run();
                }
            }
        }
    }
    public override string ToString()
    {
        return $"{Name} Run: {IsRunning}";
    }

}