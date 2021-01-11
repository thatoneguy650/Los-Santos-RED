using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player
{
    public abstract class ConsumeActivity
    {
        public ConsumeActivity() 
        {

        }
        public abstract string DebugString { get; }
        public abstract string Prompt { get; }
        public abstract void Start();
    }
}
