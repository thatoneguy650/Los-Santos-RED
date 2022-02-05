using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerTasks
{
    ITaskAssignable Player;
    ITimeReportable Time;

    public List<PlayerTask> PlayerTaskList { get; set; } = new List<PlayerTask>();
    public PlayerTasks(ITaskAssignable player, ITimeReportable time)
    {
        Player = player;
        Time = time;
    }

    public void Setup()
    {

    }
    public void Update()
    {
        PlayerTaskList.RemoveAll(x => !x.IsActive);
        foreach(PlayerTask pt in PlayerTaskList)
        {
            if(pt.ExpireTime != null && DateTime.Compare(pt.ExpireTime, Time.CurrentDateTime) < 0)
            {
                //expired?
            }
        }
    }
    public void Clear()
    {
        PlayerTaskList.Clear();
    }
    public void Dispose()
    {
        PlayerTaskList.Clear();
    }
    public void CompletedTask(string contactName)
    {
        PlayerTask myTask = PlayerTaskList.FirstOrDefault(x => x.ContactName == contactName && x.IsActive);
        if(myTask != null)
        {
            myTask.IsActive = false;
        }
    }
    public bool HasTask(string contactName)
    {
        return PlayerTaskList.Any(x => x.ContactName == contactName && x.IsActive);
    }
    public void AddTask(string contactName, DateTime expireTime)
    {
        if (!PlayerTaskList.Any(x => x.ContactName == contactName && x.IsActive))
        {
            PlayerTaskList.Add(new PlayerTask(contactName, true) { ExpireTime = expireTime });
        }
    }
    public void AddTask(string contactName)
    {
        if(!PlayerTaskList.Any(x => x.ContactName == contactName && x.IsActive))
        {
            PlayerTaskList.Add(new PlayerTask(contactName, true));
        }
    }
    public void RemoveTask(string contactName)
    {
        PlayerTaskList.RemoveAll(x => x.ContactName == contactName);
    }
}

