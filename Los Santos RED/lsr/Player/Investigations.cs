using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System.Drawing;

public class Investigations
{
    private bool PrevIsInInvestigationMode;
    private Vector3 PrevInvestigationPosition;
    private uint GameTimeStartedInvestigation;
    private uint GameTimeLastInvestigationExpired;
    private Blip InvestigationBlip;
    public float InvestigationDistance { get; private set; } = 800f;
    public Vector3 InvestigationPosition { get; set; }
    public float NearInvestigationDistance { get; private set; } = 250f;
    public bool InInvestigationMode { get; private set; }
    public bool HavePlayerDescription { get; private set; }
    public bool InvestigationModeExpired
    {
        get
        {
            if (GameTimeStartedInvestigation == 0)
                return false;
            else if (Game.GameTime - GameTimeStartedInvestigation >= 180000)
                return true;
            else
                return false;
        }
    }
    public bool IsSuspicious
    {
        get
        {
            if (!InInvestigationMode)
                return false;
            else if (InInvestigationMode && NearInvestigationPosition && HavePlayerDescription)
                return true;
            else
                return false;
        }
    }
    public bool LastInvestigationRecentlyExpired
    {
        get
        {
            if (GameTimeLastInvestigationExpired == 0)
                return false;
            else if (Game.GameTime - GameTimeLastInvestigationExpired <= 5000)
                return true;
            else
                return false;
        }
    }
    public bool NearInvestigationPosition
    {
        get
        {
            return InvestigationPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(InvestigationPosition) <= NearInvestigationDistance;
        }
    }
    public float DistanceToInvestigationPosition
    {
        get
        {
            if (!InInvestigationMode || InvestigationPosition == Vector3.Zero)
                return 9999f;
            else
                return Game.LocalPlayer.Character.DistanceTo2D(InvestigationPosition);
        }
    }
    public void Tick()
    {
        InvestigationTick(); 
    }
    public void Reset()
    {
        InInvestigationMode = false;
        HavePlayerDescription = false;
    }
    public void StartInvestigation(Vector3 PositionToInvestigate,bool HaveDescription)
    {
        InInvestigationMode = true;
        InvestigationPosition = PositionToInvestigate;
        HavePlayerDescription = HaveDescription;
    }
    private void InvestigationTick()
    {
        if (Mod.Player.IsWanted)
            InInvestigationMode = false;

        if (InvestigationModeExpired) //remove after 3 minutes
            InInvestigationMode = false;

        if (PrevInvestigationPosition != InvestigationPosition)
            InvestigationPositionChanged();

        if (PrevIsInInvestigationMode != InInvestigationMode)
            PoliceInInvestigationModeChanged();


        if (Mod.Player.IsNotWanted && InInvestigationMode && NearInvestigationPosition && HavePlayerDescription && Mod.World.Police.AnyCanRecognizePlayer && Mod.Player.CurrentPoliceResponse.HasBeenNotWantedFor >= 5000)
        {
            Mod.Player.CurrentPoliceResponse.ApplyReportedCrimes();
        }
    }
    private void InvestigationPositionChanged()
    {
        UpdateInvestigationUI();
        Mod.Debug.WriteToLog("ValueChecker", string.Format("InvestigationPosition Changed to: {0}", InvestigationPosition));
        PrevInvestigationPosition = InvestigationPosition;
    }
    private void PoliceInInvestigationModeChanged()
    {
        if (InInvestigationMode) //added
        {
            UpdateInvestigationUI();
            GameTimeStartedInvestigation = Game.GameTime;
        }
        else //removed
        {
            if (InvestigationBlip.Exists())
                InvestigationBlip.Delete();
            if (Mod.Player.IsNotWanted)
            {
                HavePlayerDescription = false;
            }
            GameTimeStartedInvestigation = 0;
            GameTimeLastInvestigationExpired = Game.GameTime;
        }
        Mod.Debug.WriteToLog("ValueChecker", string.Format("PoliceInInvestigationMode Changed to: {0}", InInvestigationMode));
        PrevIsInInvestigationMode = InInvestigationMode;
    }
    private void UpdateInvestigationUI()
    {
        UpdateInvestigationPosition();
        AddUpdateInvestigationBlip(InvestigationPosition, NearInvestigationDistance);
    }
    private void UpdateInvestigationPosition()
    {
        Vector3 SpawnLocation = Vector3.Zero;
        Mod.DataMart.Streets.GetStreetPositionandHeading(InvestigationPosition, out SpawnLocation, out float Heading, false);
        if (SpawnLocation != Vector3.Zero)
            InvestigationPosition = SpawnLocation;
    } 
    private void AddUpdateInvestigationBlip(Vector3 Position, float Size)
    {
        if (Position == Vector3.Zero)
        {
            if (InvestigationBlip.Exists())
                InvestigationBlip.Delete();
            return;
        }
        if (!InInvestigationMode)
        {
            if (InvestigationBlip.Exists())
                InvestigationBlip.Delete();
            return;
        }
        if (!InvestigationBlip.Exists())
        {
            InvestigationBlip = new Blip(Position, Size)
            {
                Name = "Investigation Center",
                Color = Color.Orange,
                Alpha = 0.25f
            };

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)InvestigationBlip.Handle, true);
            Mod.World.AddBlip(InvestigationBlip);
        }
        if (InvestigationBlip.Exists())
            InvestigationBlip.Position = Position;
    } 
}

