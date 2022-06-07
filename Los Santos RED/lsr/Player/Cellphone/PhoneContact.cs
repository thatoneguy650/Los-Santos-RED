using Rage;
using Rage.Native;
using System;
using System.IO;



public class PhoneContact
{
    public string Name { get; set; } = ""; 
    public int Index { get; private set; } = 0;
    public bool Active { get; set; } = true;
    public int DialTimeout { get; set; } = 3000;  
    public bool RandomizeDialTimeout { get; set; } = true;
    public string IconName { get; set; } = "";
    public bool Bold { get; set; } = false;
    public PhoneContact(string name, int index)
    {
        Name = name;
        Index = index;
    }
    public PhoneContact()
    {

    }

 }
