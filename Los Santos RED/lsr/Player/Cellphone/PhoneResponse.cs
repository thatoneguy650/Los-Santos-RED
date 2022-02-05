using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PhoneResponse
{
    public PhoneResponse()
    {
    }

    public PhoneResponse(string contactName, string contactIcon, string message, DateTime timeReceived)
    {
        ContactName = contactName;
        ContactIcon = contactIcon;
        Message = message;
        TimeReceived = timeReceived;
    }

    public string ContactName { get; set; }
    public string ContactIcon { get; set; }
    public string Message { get; set; }
    public DateTime TimeReceived { get; set; }
}

