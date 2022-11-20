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
        public abstract bool CanCancel { get; set; }
        public abstract string CancelPrompt { get; set; }
        public abstract string PausePrompt { get; set; }
        public abstract string ContinuePrompt { get; set; }
        public abstract bool IsUpperBodyOnly { get; set; }
        public abstract bool IsPaused();
        public abstract void Start();
        public abstract void Continue();
        public abstract void Cancel();
        public abstract void Pause();
        public abstract bool CanPerform(IActionable player);
    }
}
