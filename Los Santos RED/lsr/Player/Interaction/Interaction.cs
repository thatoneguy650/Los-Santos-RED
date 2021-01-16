using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class Interaction
{
    public Interaction()
    {

    }
    public abstract string Prompt { get; }
    public abstract string DebugString { get; }
    public abstract void Start();
    public abstract void Dispose();
}
