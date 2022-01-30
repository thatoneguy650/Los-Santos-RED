using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class SavedTextMessage
    {
    public SavedTextMessage()
    {
    }

    public SavedTextMessage(string name, string message, int hourSent, int minuteSent, bool isRead, int index, string iconName)
    {
        Name = name;
        Message = message;
        HourSent = hourSent;
        MinuteSent = minuteSent;
        IsRead = isRead;
        Index = index;
        IconName = iconName;
    }

    public string Name { get; set; } = "";
    public string Message { get; set; } = "";
    public int HourSent { get; set; } = 1;
    public int MinuteSent { get; set; } = 0;
    public bool IsRead { get; set; } = false;
    public int Index { get; set; } = 0;
    public string IconName { get; set; } = "";
}

