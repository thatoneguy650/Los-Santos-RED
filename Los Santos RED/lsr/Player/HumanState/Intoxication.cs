using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Intoxication
{
    private IIntoxicatable Player;
    private List<Intoxicator> CurrentIntoxicators = new List<Intoxicator>();
    private Intoxicator PrimaryIntoxicator;
    public Intoxication(IIntoxicatable player)
    {
        Player = player;
    }
    private uint GameTimeStartedSwerving;
    private uint GameTimeToStopSwerving;
    private uint GameTimeUntilNextSwerve;
    private float SteeringBias;
    private string CurrentClipset;
    private string OverLayEffect;
    private bool IsPrimary;
    public string DebugString { get; set; }
    public bool IsIntoxicated { get; private set; }
    private string ClipsetAtCurrentIntensity
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return "NONE";
            }
            else if (CurrentIntensity >= 3)
            {
                return "move_m@drunk@verydrunk";
            }
            else if (CurrentIntensity >= 2)
            {
                return "move_m@drunk@moderatedrunk";
            }
            else
            {
                return "move_m@drunk@slightlydrunk";
            }
        }
    }
    private float SwerveAtCurrentIntensity
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return 0.1f;
            }
            else if (CurrentIntensity >= 2)
            {
                return 0.5f;
            }
            else if (CurrentIntensity >= 3)
            {
                return 0.75f;
            }
            else
            {
                return 0.1f;
            }
        }
    }
    private uint SwerveMinLength
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return 1000;
            }
            else if (CurrentIntensity >= 2)
            {
                return 2000;
            }
            else if (CurrentIntensity >= 3)
            {
                return 3000;
            }
            else
            {
                return 1000;
            }
        }
    }
    private uint SwerveMaxLength
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return 2000;
            }
            else if (CurrentIntensity >= 2)
            {
                return 3500;
            }
            else if (CurrentIntensity >= 3)
            {
                return 5000;
            }
            else
            {
                return 2000;
            }
        }
    }
    private uint SwerveMinDelay
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return 10000;
            }
            else if (CurrentIntensity >= 2)
            {
                return 5000;
            }
            else if (CurrentIntensity >= 3)
            {
                return 2500;
            }
            else
            {
                return 10000;
            }
        }
    }
    private uint SwerveMaxDelay
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return 15000;
            }
            else if (CurrentIntensity >= 2)
            {
                return 10000;
            }
            else if (CurrentIntensity >= 3)
            {
                return 5000;
            }
            else
            {
                return 15000;
            }
        }
    }
    public bool IsSwerving { get; private set; }
    public float CurrentIntensity { get; private set; } = 0f;
    public float CurrentIntensityPercent { get; private set; }
    public void Reset()
    {
        CurrentIntoxicators.Clear();
        PrimaryIntoxicator = null;
        if (IsIntoxicated)
        {
            SetSober(true);
        }
        if(IsIntoxicated)
        {
            Restart();
        }
    }
    public void Dispose()
    {
        CurrentIntoxicators.Clear();
        PrimaryIntoxicator = null;
        if (IsIntoxicated)
        {
            SetSober(true);
        }
    }
    public void Restart()
    {
        Update(IsPrimary);
        if (IsPrimary && CurrentIntensity >= PrimaryIntoxicator.Intoxicant.EffectIntoxicationLimit)// 0.25f)
        {
            SetIntoxicated();
            Update(IsPrimary);
        }
    }

    public void Ingest(Intoxicant intoxicant)
    {
        if (intoxicant == null)
        {
            return;
        }
        Intoxicator existing = CurrentIntoxicators.FirstOrDefault(x => x.Intoxicant.Name == intoxicant.Name);
        if (existing != null)
        {
            existing.StartConsuming();
        }
        else
        {
            Intoxicator toAdd = new Intoxicator(Player, intoxicant);
            toAdd.SetConsumed();
            CurrentIntoxicators.Add(toAdd);
        }
        //EntryPoint.WriteToConsole($"Intoxication Started Ingesting {intoxicant.Name}");
    }

    public void StartIngesting(Intoxicant intoxicant)
    {
        if(intoxicant == null)
        {
            return;
        }
        Intoxicator existing = CurrentIntoxicators.FirstOrDefault(x => x.Intoxicant.Name == intoxicant.Name);
        if (existing != null)
        {
            existing.StartConsuming();
        }
        else
        {
            Intoxicator toAdd = new Intoxicator(Player, intoxicant);
            toAdd.StartConsuming();
            CurrentIntoxicators.Add(toAdd);
        }
        //EntryPoint.WriteToConsole($"Intoxication Started Ingesting {intoxicant.Name}");
    }

    public void StartIngestingIntervalBased(Intoxicant intoxicant, float intervalUpdateAmount)
    {
        if (intoxicant == null)
        {
            return;
        }
        Intoxicator existing = CurrentIntoxicators.FirstOrDefault(x => x.Intoxicant.Name == intoxicant.Name);
        if (existing != null)
        {
            existing.StartConsuming();
        }
        else
        {
            Intoxicator toAdd = new Intoxicator(Player, intoxicant);
            toAdd.StartConsuming();
            toAdd.IsIntervalBased = true;
            toAdd.IntervalAddition = intervalUpdateAmount;
            CurrentIntoxicators.Add(toAdd);
        }
    }

    public void StopIngesting(Intoxicant intoxicant)
    {
        if (intoxicant == null)
        {
            return;
        }
        Intoxicator existing = CurrentIntoxicators.FirstOrDefault(x => x.Intoxicant.Name == intoxicant.Name);
        if (existing != null)
        {
            existing.StopConsuming();
        }
        //EntryPoint.WriteToConsole($"Intoxication Stopped Ingesting {intoxicant.Name}");
    }
    public void Update(bool isPrimary)
    {
        IsPrimary = isPrimary;
        CurrentIntoxicators.RemoveAll(x => x.CurrentIntensity == 0.0f && !x.IsConsuming);
        float HighestIntensity = 0.0f;
        PrimaryIntoxicator = null;
        foreach(Intoxicator intox in CurrentIntoxicators)
        {
            if(intox.CurrentIntensity > HighestIntensity)
            {
                PrimaryIntoxicator = intox;
                HighestIntensity = intox.CurrentIntensity;
            }
            if(intox.Intoxicant.ContinuesWithoutCurrentUse)
            {
                if(intox.CurrentIntensity == intox.Intoxicant.MaxEffectAllowed && intox.IsConsuming)
                {
                    intox.StopConsuming();
                    //EntryPoint.WriteToConsole($"Intoxication Intoxicant.ContinuesWithoutCurrentUse, Reached Max, Stopping {intox.Intoxicant?.Name}");
                }
            }
        }
        if (PrimaryIntoxicator != null)
        {
            OverLayEffect = PrimaryIntoxicator.Intoxicant?.OverLayEffect;
            CurrentIntensity = PrimaryIntoxicator.CurrentIntensity;

            //EntryPoint.WriteToConsole($"CurrentIntensity {CurrentIntensity}");

            UpdateDrunkStatus();
        }
        else
        {
            if (IsIntoxicated)
            {
                SetSober(true);
            }
        }
        DebugString = $" PName: {PrimaryIntoxicator?.Intoxicant?.Name} Int: {PrimaryIntoxicator?.CurrentIntensity} int2 {CurrentIntensity} Total: {CurrentIntoxicators.Count()} IsIntoxicated {Player.Intoxication.IsIntoxicated}  EffectLimit {PrimaryIntoxicator?.Intoxicant?.EffectIntoxicationLimit}";
        //EntryPoint.WriteToConsole(DebugString, 5);
    }
    private void UpdateDrunkStatus()
    {
        if (!IsIntoxicated && IsPrimary && CurrentIntensity >= PrimaryIntoxicator.Intoxicant.EffectIntoxicationLimit)// 0.25f)
        {
            SetIntoxicated();
        }
        else if (IsIntoxicated && IsPrimary && CurrentIntensity <= PrimaryIntoxicator.Intoxicant.EffectIntoxicationLimit)//0.25f)
        {
            SetSober(true);
        }
        if (IsIntoxicated && IsPrimary)
        {
            if (CurrentClipset != ClipsetAtCurrentIntensity && ClipsetAtCurrentIntensity != "NONE" && PrimaryIntoxicator.Intoxicant.Effects.HasFlag(IntoxicationEffect.ImparesWalking))
            {
                CurrentClipset = ClipsetAtCurrentIntensity;
                Player.ClipsetManager.SetMovementClipset(CurrentClipset);
            }

            if(!PrimaryIntoxicator.Intoxicant.NoCameraShake)
            {
                NativeFunction.CallByName<int>("SET_GAMEPLAY_CAM_SHAKE_AMPLITUDE", CurrentIntensity);

            }
            if (!string.IsNullOrEmpty(PrimaryIntoxicator.Intoxicant.OverLayEffect))
            {
                NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", CurrentIntensity / 5.0f);
            }
            CurrentIntensityPercent = CurrentIntensity / PrimaryIntoxicator.Intoxicant.MaxEffectAllowed;
            if (Player.IsInVehicle && PrimaryIntoxicator.Intoxicant.Effects.HasFlag(IntoxicationEffect.ImparesDriving))
            {
                UpdateSwerving();
            }

            Player.Sprinting.InfiniteStamina = PrimaryIntoxicator.Intoxicant.Effects.HasFlag(IntoxicationEffect.InfiniteStamina);
            Player.Sprinting.TurboSpeed = PrimaryIntoxicator.Intoxicant.Effects.HasFlag(IntoxicationEffect.FastSpeed);
            Player.IsOnMuscleRelaxants = PrimaryIntoxicator.Intoxicant.Effects.HasFlag(IntoxicationEffect.RelaxesMuscles);
        }
    }
    private void UpdateSwerving()
    {
        //SET_VEHICLE_STEER_BIAS
        if (Game.GameTime >= GameTimeUntilNextSwerve)
        {
            GameTimeUntilNextSwerve = Game.GameTime + RandomItems.GetRandomNumber(15000, 30000);
            if (!IsSwerving && Player.IsDriver)
            {
                IsSwerving = true;
                GameTimeStartedSwerving = Game.GameTime;
                GameTimeToStopSwerving = Game.GameTime + RandomItems.GetRandomNumber(SwerveMinLength, SwerveMaxLength);
                SteeringBias = RandomItems.GetRandomNumber(-1f * SwerveAtCurrentIntensity, SwerveAtCurrentIntensity);
                //EntryPoint.WriteToConsole($"PLAYER EVENT: DRUNK SWERVE STARTED BIAS: {SwerveAtCurrentIntensity}", 3);
            }
        }
        if (IsSwerving && Game.GameTime > GameTimeToStopSwerving)
        {
            IsSwerving = false;
            SteeringBias = 0f;
            //EntryPoint.WriteToConsole($"PLAYER EVENT: DRUNK SWERVE ENDED", 3);
        }
        if (Player.IsDriver && IsSwerving && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        {
            NativeFunction.Natives.SET_VEHICLE_STEER_BIAS(Player.CurrentVehicle.Vehicle, SteeringBias);
        }
    }
    private void SetIntoxicated()
    {
        IsIntoxicated = true;
        CurrentClipset = ClipsetAtCurrentIntensity;
        NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, true);
        if (CurrentClipset != "NONE" && !Player.ActivityManager.IsSitting && !Player.IsInVehicle && PrimaryIntoxicator.Intoxicant.Effects.HasFlag(IntoxicationEffect.ImparesWalking))
        {
            Player.ClipsetManager.SetMovementClipset(CurrentClipset);
        }
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, true);
        if (!PrimaryIntoxicator.Intoxicant.NoCameraShake)
        {
            NativeFunction.Natives.x80C8B1846639BB19(1);
            NativeFunction.CallByName<int>("SHAKE_GAMEPLAY_CAM", "DRUNK_SHAKE", CurrentIntensity);
        }
        if (!string.IsNullOrEmpty(OverLayEffect))
        {
            NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER", OverLayEffect);
            NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", CurrentIntensity / 5.0f);
        }
        Player.Sprinting.InfiniteStamina = PrimaryIntoxicator.Intoxicant.Effects.HasFlag(IntoxicationEffect.InfiniteStamina);
        Player.Sprinting.TurboSpeed = PrimaryIntoxicator.Intoxicant.Effects.HasFlag(IntoxicationEffect.FastSpeed);
        Player.IsOnMuscleRelaxants = PrimaryIntoxicator.Intoxicant.Effects.HasFlag(IntoxicationEffect.RelaxesMuscles);
        GameTimeUntilNextSwerve = Game.GameTime + RandomItems.GetRandomNumber(15000, 30000);
    }
    private void SetSober(bool ResetClipset)
    {
        IsIntoxicated = false;
        NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, false);
        if (ResetClipset)
        {
            Player.ClipsetManager.ResetMovementClipset();      
        }
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
        NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
        NativeFunction.Natives.x80C8B1846639BB19(0);
        NativeFunction.CallByName<int>("STOP_GAMEPLAY_CAM_SHAKING", true);
        CurrentIntensityPercent = 0.0f;
        CurrentIntensity = 0.0f;

        Player.Sprinting.InfiniteStamina = false;
        Player.Sprinting.TurboSpeed = false;
        Player.IsOnMuscleRelaxants = false;
    }

    public void AddIntervalConsumption(Intoxicant intoxicant)
    {
        if (intoxicant == null)
        {
            return;
        }
        Intoxicator existing = CurrentIntoxicators.FirstOrDefault(x => x.Intoxicant.Name == intoxicant.Name);
        if (existing != null)
        {
            existing.OnIntervalConsumed();
        }
    }
}

