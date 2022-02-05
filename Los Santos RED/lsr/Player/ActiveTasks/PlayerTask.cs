using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerTask
{
    public PlayerTask(string contactName, bool isActive)
    {
        ContactName = contactName;
        IsActive = isActive;
    }

    public string ContactName { get; set; }
    public bool IsActive { get; set; } = false;
    public DateTime ExpireTime { get; set; }
}

