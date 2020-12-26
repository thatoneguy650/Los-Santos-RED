using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Drawing;

public class Investigations
{
    private IWorld World;
    private IPlayer CurrentPlayer;
    private bool PrevIsActive;
    private Vector3 PrevPosition;
    private uint GameTimeStartedInvestigation;
    private uint GameTimeLastInvestigationExpired;
    private Blip blip;

    public Investigations(IPlayer currentPlayer, IWorld world)
    {
        World = world;
        CurrentPlayer = currentPlayer;
    }
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
        if (CurrentPlayer.IsWanted)
        {
            IsActive = false;
        }
        else if (Expired) //remove after 3 minutes
        {
            IsActive = false;
        }
        else if (!IsActive && CurrentPlayer.CurrentPoliceResponse.HasReportedCrimes)
        {
            StartInvestigation(CurrentPlayer.CurrentPoliceResponse.CurrentCrimes.PlaceLastReportedCrime, CurrentPlayer.CurrentPoliceResponse.CurrentCrimes.PoliceHaveDescription);
        }

        if (PrevPosition != Position)
        {
            InvestigationPositionChanged();
        }
        if (PrevIsActive != IsActive)
        {
            PoliceInInvestigationModeChanged();
        }
        if (CurrentPlayer.IsNotWanted && IsActive && NearInvestigationPosition && HaveDescription && CurrentPlayer.AnyPoliceCanRecognizePlayer && CurrentPlayer.CurrentPoliceResponse.HasBeenNotWantedFor >= 5000)
        {
            CurrentPlayer.CurrentPoliceResponse.ApplyReportedCrimes();
        }
    }
    private void InvestigationPositionChanged()
    {
        UpdateInvestigationUI();
        Game.Console.Print(string.Format("InvestigationPosition Changed to: {0}", Position));
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
            if (CurrentPlayer.IsNotWanted)
            {
                HaveDescription = false;
            }
            GameTimeStartedInvestigation = 0;
            GameTimeLastInvestigationExpired = Game.GameTime;
        }
        Game.Console.Print(string.Format("PoliceInInvestigationMode Changed to: {0}", IsActive));
        PrevIsActive = IsActive;
    }
    private void UpdateInvestigationUI()
    {
        UpdateInvestigationPosition();
        //AddUpdateInvestigationBlip(Position, NearInvestigationDistance);
    }
    private void UpdateInvestigationPosition()
    {
        GetStreetPositionandHeading(Position, out Vector3 SpawnLocation, out float Heading, false);
        if (SpawnLocation != Vector3.Zero)
        {
            Position = SpawnLocation;
        }
    }
    private void GetStreetPositionandHeading(Vector3 PositionNear, out Vector3 SpawnPosition, out float Heading, bool MainRoadsOnly)
    {
        Vector3 pos = PositionNear;
        SpawnPosition = Vector3.Zero;
        Heading = 0f;

        Vector3 outPos;
        float heading;
        float val;

        if (MainRoadsOnly)
        {
            unsafe
            {
                NativeFunction.CallByName<bool>("GET_CLOSEST_VEHICLE_NODE_WITH_HEADING", pos.X, pos.Y, pos.Z, &outPos, &heading, 0, 3, 0);
            }

            SpawnPosition = outPos;
            Heading = heading;
        }
        else
        {
            for (int i = 1; i < 40; i++)
            {
                unsafe
                {
                    NativeFunction.CallByName<bool>("GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING", pos.X, pos.Y, pos.Z, i, &outPos, &heading, &val, 1, 0x40400000, 0);
                }
                if (!NativeFunction.CallByName<bool>("IS_POINT_OBSCURED_BY_A_MISSION_ENTITY", outPos.X, outPos.Y, outPos.Z, 5.0f, 5.0f, 5.0f, 0))
                {
                    SpawnPosition = outPos;
                    Heading = heading;
                    break;
                }
            }
        }
    }
    //private void AddUpdateInvestigationBlip(Vector3 Position, float Size)
    //{
    //    if (Position == Vector3.Zero)
    //    {
    //        if (blip.Exists())
    //        {
    //            blip.Delete();
    //        }
    //        return;
    //    }
    //    if (!IsActive)
    //    {
    //        if (blip.Exists())
    //        {
    //            blip.Delete();
    //        }
    //        return;
    //    }
    //    if (!blip.Exists())
    //    {
    //        blip = new Blip(Position, Size)
    //        {
    //            Name = "Investigation Center",
    //            Color = Color.Orange,
    //            Alpha = 0.25f
    //        };
    //        NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)blip.Handle, true);
    //        World.AddBlip(blip);
    //    }
    //    if (blip.Exists())
    //    {
    //        blip.Position = Position;
    //    }
    //}
}

