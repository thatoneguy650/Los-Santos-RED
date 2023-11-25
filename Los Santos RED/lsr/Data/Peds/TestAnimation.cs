using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TestAnimation
{
    public TestAnimation()
    {
    }

    public TestAnimation(string dictionary, string name)
    {
        Dictionary = dictionary;
        Name = name;
    }

    public string Dictionary { get; set; }
    public string Name { get; set; }
}

