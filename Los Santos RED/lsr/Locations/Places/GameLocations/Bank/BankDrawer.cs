using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BankDrawer
{
    public BankDrawer()
    {
    }

    public BankDrawer(Vector3 position, float heading, int totalCash)
    {
        Position = position;
        Heading = heading;
        TotalCash = totalCash;
    }

    public Vector3 Position { get; set; }
    public float Heading { get; set; }
    public int TotalCash { get; set; }
}

