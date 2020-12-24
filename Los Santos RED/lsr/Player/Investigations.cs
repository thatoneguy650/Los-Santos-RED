using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System.Drawing;

public class Investigations
{
    private bool PrevIsActive;
    private Vector3 PrevPosition;
    private uint GameTimeStartedInvestigation;
    private uint GameTimeLastInvestigationExpired;
    private Blip blip;
    public float Distance { get; private set; } = 800f;
    public Vector3 Position { get; set; }
    public float NearInvestigationDistance { get; private set; } = 250f;
    public bool IsActive { get; private set; }
    public bool HaveDescription { get; private set; }
    public bool Expired
    {
        get
        {
            if (GameTimeStartedInvestigation == 0)
            {
                return false;
            }
            else if (Game.GameTime - GameTimeStartedInvestigation >= 180000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool IsSuspicious
    {
        get
        {
            if (!IsActive)
            {
                return false;
            }
            else if (IsActive && NearInvestigationPosition && HaveDescription)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool LastInvestigationRecentlyExpired
    {
        get
        {
            if (GameTimeLastInvestigationExpired == 0)
            {
                return false;
            }
            else if (Game.GameTime - GameTimeLastInvestigationExpired <= 5000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool NearInvestigationPosition
    {
        get
        {
            return Position != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(Position) <= NearInvestigationDistance;
        }
    }
    public float DistanceToInvestigationPosition
    {
        get
        {
            if (!IsActive || Position == Vector3.Zero)
            {
                return 9999f;
            }
            else
            {
                return Game.LocalPlayer.Character.DistanceTo2D(Position);
            }
        }
    }
    public void Update()
    {
        InvestigationTick();
    }
    public void Reset()
    {
        IsActive = false;
        HaveDescription = false;
    }
    public void StartInvestigation(Vector3 position, bool haveDescription)
    {
        IsActive = true;
        Position = position;
        HaveDescription = haveDescription;
    }
    private void InvestigationTick()
    {
        if (Mod.Player.Instance.IsWanted)
        {
            IsActive = false;
        }
        else if (Expired) //remove after 3 minutes
        {
            IsActive = false;
        }
        else if (!IsActive && Mod.Player.Instance.CurrentPoliceResponse.HasReportedCrimes)
        {
            StartInvestigation(Mod.Player.Instance.CurrentPoliceResponse.CurrentCrimes.PlaceLastReportedCrime, Mod.Player.Instance.CurrentPoliceResponse.CurrentCrimes.PoliceHaveDescription);
        }

        if (PrevPosition != Position)
        {
            InvestigationPositionChanged();
        }
        if (PrevIsActive != IsActive)
        {
            PoliceInInvestigationModeChanged();
        }
        if (Mod.Player.Instance.IsNotWanted && IsActive && NearInvestigationPosition && HaveDescription && Mod.World.Instance.AnyPoliceCanRecognizePlayer && Mod.Player.Instance.CurrentPoliceResponse.HasBeenNotWantedFor >= 5000)
        {
            Mod.Player.Instance.CurrentPoliceResponse.ApplyReportedCrimes();
        }
    }
    private void InvestigationPositionChanged()
    {
        UpdateInvestigationUI();
        Debug.Instance.WriteToLog("ValueChecker", string.Format("InvestigationPosition Changed to: {0}", Position));
        PrevPosition = Position;
    }
    private void PoliceInInvestigationModeChanged()
    {
        if (IsActive) //added
        {
            UpdateInvestigationUI();
            GameTimeStartedInvestigation = Game.GameTime;
        }
        else //removed
        {
            if (blip.Exists())
            {
                blip.Delete();
            }
            if (Mod.Player.Instance.IsNotWanted)
            {
                HaveDescription = false;
            }
            GameTimeStartedInvestigation = 0;
            GameTimeLastInvestigationExpired = Game.GameTime;
        }
        Debug.Instance.WriteToLog("ValueChecker", string.Format("PoliceInInvestigationMode Changed to: {0}", IsActive));
        PrevIsActive = IsActive;
    }
    private void UpdateInvestigationUI()
    {
        UpdateInvestigationPosition();
        AddUpdateInvestigationBlip(Position, NearInvestigationDistance);
    }
    private void UpdateInvestigationPosition()
    {
        DataMart.Instance.Streets.GetStreetPositionandHeading(Position, out Vector3 SpawnLocation, out float Heading, false);
        if (SpawnLocation != Vector3.Zero)
        {
            Position = SpawnLocation;
        }
    }
    private void AddUpdateInvestigationBlip(Vector3 Position, float Size)
    {
        if (Position == Vector3.Zero)
        {
            if (blip.Exists())
            {
                blip.Delete();
            }
            return;
        }
        if (!IsActive)
        {
            if (blip.Exists())
            {
                blip.Delete();
            }
            return;
        }
        if (!blip.Exists())
        {
            blip = new Blip(Position, Size)
            {
                Name = "Investigation Center",
                Color = Color.Orange,
                Alpha = 0.25f
            };
            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)blip.Handle, true);
            Mod.World.Instance.AddBlip(blip);
        }
        if (blip.Exists())
        {
            blip.Position = Position;
        }
    }
}

