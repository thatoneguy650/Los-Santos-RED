using System.ComponentModel;

public class SprintSettings : ISettingsDefaultable
{
    [Description("Total amount of stamina the player has in reserve.")]
    public float MaxStamina { get; set; }
    [Description("Minimum amount of stamina needed to start sprinting")]
    public float MinStaminaToStart { get; set; }
    [Description("Rate at which stamina drains when sprinting.")]
    public float DrainRate { get; set; }
    [Description("Rate at which stamina recovers when not sprinting.")]
    public float RecoverRate { get; set; }
    [Description("How fast to make the player run when sprinting")]
    public float MoveSpeedOverride { get; set; }

    public SprintSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        MaxStamina = 50f;
        MinStaminaToStart = 10f;
        DrainRate = 1.0f;
        RecoverRate = 1.0f;
        MoveSpeedOverride = 4.0f;//5.0f;
    }

}