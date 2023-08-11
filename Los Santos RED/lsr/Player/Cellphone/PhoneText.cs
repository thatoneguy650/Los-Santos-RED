using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

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
    public string CustomPicture { get; set; }
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
    public string CleanMessage()
    {
        string toReturn = Message;
        List<string> InvalidStrings = new List<string>
        {
            "~r~",
            "~b~",
            "~g~",
            "~y~",
            "~p~",
            "~q~",
            "~o~",
            "~c~",
            "~m~",
            "~u~",
            "~n~",
            "~s~",
            "~w~",
            "~h~",

        };
        foreach(string s in InvalidStrings)
        {
            toReturn = toReturn.Replace(s, "");
        }
        return toReturn;
    }
}