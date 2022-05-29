using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player
{
    public abstract class DynamicActivity
    {
        public DynamicActivity() 
        {

        }
        public abstract ModItem ModItem { get; set; }
        public abstract string DebugString { get; }
        public abstract bool CanPause { get; set; }
        public abstract void Start();
        public abstract void Continue();
        public abstract void Cancel();
        public abstract void Pause();
    }
}
