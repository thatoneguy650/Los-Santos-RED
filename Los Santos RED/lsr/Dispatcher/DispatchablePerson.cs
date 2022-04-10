public class DispatchablePerson
{
    public string ModelName { get; set; }
    public string GroupName { get; set; } = "";
    public PedVariation RequiredVariation { get; set; }
    public bool RandomizeHead { get; set; }
    public string OverrideVoice { get; set; } = "";

    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
    public int MinWantedLevelSpawn { get; set; } = 0;
    public int MaxWantedLevelSpawn { get; set; } = 6;






    public int HealthMin { get; set; } = 85;
    public int HealthMax { get; set; } = 125;
    public int ArmorMin { get; set; } = 0;
    public int ArmorMax { get; set; } = 0;


    public int AccuracyMin { get; set; } = 20;//40
    public int AccuracyMax { get; set; } = 20;//40


    public int ShootRateMin { get; set; } = 400;
    public int ShootRateMax { get; set; } = 400;


    public int CombatAbilityMin { get; set; } = 1;//0 - poor, 1- average, 2 - professional
    public int CombatAbilityMax { get; set; } = 2;//0 - poor, 1- average, 2 - professional




    public int TaserAccuracyMin { get; set; } = 30;
    public int TaserAccuracyMax { get; set; } = 30;
    public int TaserShootRateMin { get; set; } = 100;
    public int TaserShootRateMax { get; set; } = 100;


    public int VehicleAccuracyMin { get; set; } = 5;
    public int VehicleAccuracyMax { get; set; } = 5;
    public int VehicleShootRateMin { get; set; } = 20;
    public int VehicleShootRateMax { get; set; } = 20;


    public string UnitCode { get; set; } = "";
    public int RequiredHelmetType { get; set; } = -1;

    public bool CanCurrentlySpawn(int WantedLevel)
    {
        if (WantedLevel > 0)
        {
            if (WantedLevel >= MinWantedLevelSpawn && WantedLevel <= MaxWantedLevelSpawn)
            {
                return WantedSpawnChance > 0;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return AmbientSpawnChance > 0;
        }
    }
    public int CurrentSpawnChance(int WantedLevel)
    {
        if (!CanCurrentlySpawn(WantedLevel))
        {
            return 0;
        }
        if (WantedLevel > 0)
        {
            if (WantedLevel >= MinWantedLevelSpawn && WantedLevel <= MaxWantedLevelSpawn)
            {
                return WantedSpawnChance;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return AmbientSpawnChance;
        }
    }

    public bool IsFreeMode => ModelName.ToLower() == "mp_f_freemode_01" || ModelName.ToLower() == "mp_m_freemode_01";

    public DispatchablePerson()
    {

    }
    public DispatchablePerson(string _ModelName, int ambientSpawnChance, int wantedSpawnChance)
    {
        ModelName = _ModelName;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
    }
    public DispatchablePerson(string modelName, int ambientSpawnChance, int wantedSpawnChance, int accuracyMin, int accuracyMax, int shootRateMin, int shootRateMax, int combatAbilityMin, int combatAbilityMax) : this(modelName, ambientSpawnChance, wantedSpawnChance)
    {

        AccuracyMin = accuracyMin;
        AccuracyMax = accuracyMax;
        ShootRateMin = shootRateMin;
        ShootRateMax = shootRateMax;
        CombatAbilityMin = combatAbilityMin;
        CombatAbilityMax = combatAbilityMax;
    }
    public DispatchablePerson(string modelName, int ambientSpawnChance, int wantedSpawnChance, int healthMin, int healthMax, int armorMin, int armorMax, int accuracyMin, int accuracyMax, int shootRateMin, int shootRateMax, int combatAbilityMin, int combatAbilityMax, PedVariation requiredVariation, bool randomizeHead) : this(modelName, ambientSpawnChance, wantedSpawnChance)
    {
        HealthMin = healthMin;
        HealthMax = healthMax;
        ArmorMin = armorMin;
        ArmorMax = armorMax;
        AccuracyMin = accuracyMin;
        AccuracyMax = accuracyMax;
        ShootRateMin = shootRateMin;
        ShootRateMax = shootRateMax;
        CombatAbilityMin = combatAbilityMin;
        CombatAbilityMax = combatAbilityMax;
        RequiredVariation = requiredVariation;
        RandomizeHead = randomizeHead;
    }
    public DispatchablePerson(string modelName, int ambientSpawnChance, int wantedSpawnChance, int healthMin, int healthMax, int armorMin, int armorMax, int accuracyMin, int accuracyMax, int shootRateMin, int shootRateMax, int combatAbilityMin, int combatAbilityMax) : this(modelName, ambientSpawnChance, wantedSpawnChance)
    {
        HealthMin = healthMin;
        HealthMax = healthMax;
        ArmorMin = armorMin;
        ArmorMax = armorMax;
        AccuracyMin = accuracyMin;
        AccuracyMax = accuracyMax;
        ShootRateMin = shootRateMin;
        ShootRateMax = shootRateMax;
        CombatAbilityMin = combatAbilityMin;
        CombatAbilityMax = combatAbilityMax;
    }
}