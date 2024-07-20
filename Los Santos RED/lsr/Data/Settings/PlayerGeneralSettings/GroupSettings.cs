using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class GroupSettings : ISettingsDefaultable
{
    [Description("Max number of group members allowed.")]
    public int MaxGroupMembers { get; set; }

    [Description("If enabled group members will have increase health applied.")]
    public bool EnableIncreasedGroupHealth { get; set; }
    [Description("Min increased health value.")]
    public int IncreasedHealthMin { get; set; }
    [Description("Max increased health value.")]
    public int IncreasedHealthMax { get; set; }
    public bool EnableAutoArmor { get; set; }
    public int AutoArmorMin { get; set; }
    public int AutoArmorMax { get; set; }
    public bool AlwaysSetSpecialist { get; set; }
    public float MaxPlayerDistanceDuringCombat { get; set; }
    public float PlayerMoveDistanceToUpdate { get; set; }
    public float MaxPlayerDistanceDuringCombatBeforeForceReturn { get; set; }
    public bool EnableGroupButtonPrompts { get; set; }


    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }
    public GroupSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        MaxGroupMembers = 15;
        EnableIncreasedGroupHealth = true;
        IncreasedHealthMin = 250;
        IncreasedHealthMax = 350;
        EnableAutoArmor = true;
        AutoArmorMin = 100;
        AutoArmorMax = 150;
        AlwaysSetSpecialist = true;

        MaxPlayerDistanceDuringCombat = 10f;
        PlayerMoveDistanceToUpdate = 4.0f;
        MaxPlayerDistanceDuringCombatBeforeForceReturn = 35f;

        EnableGroupButtonPrompts = true;
    }

}