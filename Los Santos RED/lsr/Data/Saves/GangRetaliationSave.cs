using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangRetaliationSave
{
    public GangRetaliationSave()
    {

    }
    public string TargetGangID { get; set; }
    public List<string> ZoneIds { get; set; }
    public uint TimeToStartRetaliation { get; set; }
    public float RetaliationPercentAtIncrement { get; set; }
    public uint RetaliationTime { get; set; }
    public uint TimeToReturnToZone { get; set; }
    public bool HasRetaliationStarted { get; set; }
    public bool IsEnded { get; set; }
    public bool HasPlayerReturnedToZone { get; set; }
    public uint GameTimeStarted { get; set; }
    public uint GameTimeEnded { get; set; }
    public uint GameTimeReturnedToZone { get; set; }
    public uint GameTimeWarEnded { get; set; }
}

