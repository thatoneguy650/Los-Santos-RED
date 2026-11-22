using System;

/// <summary>
/// One entry in the in-memory standing log. Not persisted — this is a tuning
/// instrument, not state. The reason string is the whole point: a standing total
/// tells you nothing while balancing, but a stream of "+40 job completed" does.
///
/// No Rage types.
/// </summary>
public class GangStandingEvent
{
    public GangStandingEvent()
    {
    }
    public GangStandingEvent(DateTime gameTime, string gangID, int standingDelta, int goodwillDelta, string reason)
    {
        GameTime = gameTime;
        GangID = gangID;
        StandingDelta = standingDelta;
        GoodwillDelta = goodwillDelta;
        Reason = reason;
    }

    public DateTime GameTime { get; set; }
    public string GangID { get; set; }
    public int StandingDelta { get; set; }
    public int GoodwillDelta { get; set; }
    public string Reason { get; set; }

    public string ShortLine
    {
        get
        {
            string standingPart = StandingDelta == 0 ? "" : (StandingDelta > 0 ? $"+{StandingDelta}" : StandingDelta.ToString());
            string goodwillPart = GoodwillDelta == 0 ? "" : (GoodwillDelta > 0 ? $" +{GoodwillDelta}gw" : $" {GoodwillDelta}gw");
            return $"{GameTime:HH:mm} {standingPart}{goodwillPart} {Reason}";
        }
    }

    public override string ToString() => ShortLine;
}
