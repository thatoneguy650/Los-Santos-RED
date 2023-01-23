using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BurnerPhoneSettingTracker
{
    public BurnerPhoneSettingTracker(int index, string name)
    {
        Index = index;
        Name = name;
    }

    public int Index { get; set; }
    public string Name { get; set; }
    public bool IsSelected { get; set; }
    public float Value { get; set; }
    public int IntegerValue { get; set; }
}

