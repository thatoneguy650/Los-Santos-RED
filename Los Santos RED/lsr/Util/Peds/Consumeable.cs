using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class Consumeable
{
    public Consumeable(float amount, string name)
    {
        Amount = amount;
        Name = name;
    }
    public float Amount { get; set; }
    public string Name { get; set; }
}

