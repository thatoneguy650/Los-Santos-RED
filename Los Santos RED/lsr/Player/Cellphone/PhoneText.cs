using System;

public class PhoneText
{
    public string ContactName { get; set; } = "";
    public string Message { get; set; } = "";
    public int HourSent { get; set; } = 1;
    public int MinuteSent { get; set; } = 0;
    public bool IsRead { get; set; } = false;
    public int Index { get; set; } = 0;
    public string IconName { get; set; } = "";
    public bool Bold { get; set; } = false;
    public DateTime TimeReceived { get; set; }
    public PhoneText()
    {

    }
    public PhoneText(string name, int index, string message, int hourSent, int minuteSent)
    {
        ContactName = name;
        Index = index;
        Message = message;
        HourSent = hourSent;
        MinuteSent = minuteSent;
    }
}