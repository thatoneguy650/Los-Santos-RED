using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

[Serializable]
public class DispatchablePerson
{
    public string DebugName { get; set; } = "";
    public string ModelName { get; set; }
    public string GroupName { get; set; } = "";

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

    public int ShootRateMin { get; set; } = 200;//400
    public int ShootRateMax { get; set; } = 300;//400

    public int CombatAbilityMin { get; set; } = 1;//0 - poor, 1- average, 2 - professional
    public int CombatAbilityMax { get; set; } = 2;//0 - poor, 1- average, 2 - professional

    public int CombatRange { get; set; } = -1;
    public int CombatMovement { get; set; } = -1;//0 - poor, 1- average, 2 - professional


    public int TaserAccuracyMin { get; set; } = 30;
    public int TaserAccuracyMax { get; set; } = 30;
    public int TaserShootRateMin { get; set; } = 100;
    public int TaserShootRateMax { get; set; } = 100;

    public int VehicleAccuracyMin { get; set; } = 2;//5
    public int VehicleAccuracyMax { get; set; } = 4;//5
    public int VehicleShootRateMin { get; set; } = 10;//20
    public int VehicleShootRateMax { get; set; } = 15;//20


    public int TurretAccuracyMin { get; set; } = 10;
    public int TurretAccuracyMax { get; set; } = 30;
    public int TurretShootRateMin { get; set; } = 500;
    public int TurretShootRateMax { get; set; } = 1000;

    public string UnitCode { get; set; } = "";
    public int RequiredHelmetType { get; set; } = -1;

    public bool AllowRandomizeBeforeVariationApplied { get; set; } = false;
    public bool RandomizeHead { get; set; } = false;

    public bool OverrideAgencyLessLethalWeapons { get; set; } = false;
    public bool OverrideAgencySideArms { get; set; } = false;
    public bool OverrideAgencyLongGuns { get; set; } = false;

    public string OverrideLessLethalWeaponsID { get; set; }
    public string OverrideSideArmsID { get; set; }
    public string OverrideLongGunsID { get; set; }
    [XmlIgnore]
    public List<IssuableWeapon> OverrideSideArms { get; set; } = new List<IssuableWeapon>();
    [XmlIgnore]
    public List<IssuableWeapon> OverrideLongGuns { get; set; } = new List<IssuableWeapon>();
    [XmlIgnore]
    public List<IssuableWeapon> OverrideLessLethalWeapons { get; set; } = new List<IssuableWeapon>();
    public PedVariation RequiredVariation { get; set; }
    public List<PedPropComponent> OptionalProps { get; set; }
    public int OptionalPropChance { get; set; } = 30;
    public List<PedComponent> OptionalComponents { get; set; }
    public OptionalAppliedOverlayLogic OptionalAppliedOverlayLogic { get; set; }
    public int OptionalComponentChance { get; set; } = 30;
    public PedComponent EmptyHolster { get; set; }
    public PedComponent FullHolster { get; set; }
    public List<string> OverrideVoice { get; set; }
    public List<CustomPropAttachment> CustomPropAttachments { get; set; }
    public bool DisableWrithe { get; set; } = true;
    public bool DisableWritheShooting { get; set; } = true;
    public bool DisableCriticalHits { get; set; } = false;
    public bool DisableBulletRagdoll { get; set; } = false;
    public bool HasFullBodyArmor { get; set; } = false;
    public int FiringPatternHash { get; set; } = 0;
    public List<PedConfigFlagToSet> PedConfigFlagsToSet { get; set; }
    public List<CombatAttributeToSet> CombatAttributesToSet { get; set; }
    public List<CombatFloatToSet> CombatFloatsToSet { get; set; }
    public float FaceFeatureRandomizePercentage { get; set; } = 90f;
    public bool AlwaysHasLongGun { get; set; } = false;
    public bool IsAnimal { get; set; } = false;
    public float OverrideSightDistance { get; set; } = -1.0f;

    public PedPropComponent OverrideHelmet { get; set; }
    public float NoHelmetPercentage { get; set; } = 0f;
    public string GetDescription()
    {
        string description = "";

        description += $"DebugName: {DebugName}";
        description += $"~n~ModelName: {ModelName}";
        description += $"~n~AmbientSpawnChance: {AmbientSpawnChance} WantedSpawnChance: {WantedSpawnChance}";
        description += $"~n~MinWantedLevelSpawn: {MinWantedLevelSpawn} MaxWantedLevelSpawn: {MaxWantedLevelSpawn}";
        return description;
    }
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
    public bool IsFreeModeFemale => ModelName.ToLower() == "mp_f_freemode_01";
    public bool IsFreeModeMale => ModelName.ToLower() == "mp_m_freemode_01";
    public bool IsMale(Ped ped)
    {
        if(IsFreeMode)
        {
            return IsFreeModeMale;
        }
        if(!ped.Exists())
        {
            return true;
        }
        return ped.IsMale;
    }
    public void SetPedExtPermanentStats(PedExt pedExt, bool overrideHealth, bool overrideArmor, bool overrideAccuracy)
    {
        if (!pedExt.Pedestrian.Exists())
        {
            return;
        }
        pedExt.Accuracy = RandomItems.GetRandomNumberInt(AccuracyMin, AccuracyMax);
        pedExt.ShootRate = RandomItems.GetRandomNumberInt(ShootRateMin, ShootRateMax);
        pedExt.CombatAbility = RandomItems.GetRandomNumberInt(CombatAbilityMin, CombatAbilityMax);
        pedExt.CombatMovement = CombatMovement;
        pedExt.CombatRange = CombatRange;
        pedExt.TaserAccuracy = RandomItems.GetRandomNumberInt(TaserAccuracyMin, TaserAccuracyMax);
        pedExt.TaserShootRate = RandomItems.GetRandomNumberInt(TaserShootRateMin, TaserShootRateMax);
        pedExt.VehicleAccuracy = RandomItems.GetRandomNumberInt(VehicleAccuracyMin, VehicleAccuracyMax);
        pedExt.VehicleShootRate = RandomItems.GetRandomNumberInt(VehicleShootRateMin, VehicleShootRateMax);
        pedExt.TurretAccuracy = RandomItems.GetRandomNumberInt(TurretAccuracyMin, TurretAccuracyMax);
        pedExt.TurretShootRate = RandomItems.GetRandomNumberInt(TurretShootRateMin, TurretShootRateMax);
        if (AlwaysHasLongGun)
        {
            //EntryPoint.WriteToConsole($"SET AlwaysHasLongGun {pedExt.Handle}");
            pedExt.AlwaysHasLongGun = true;
        }
        if (OverrideVoice != null && OverrideVoice.Any())
        {
            pedExt.VoiceName = OverrideVoice.PickRandom();
        }
        if (DisableBulletRagdoll)
        {
            //EntryPoint.WriteToConsole($"SET DisableBulletRagdoll {pedExt.Handle}");
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(pedExt.Pedestrian, (int)107, true);//PCF_DontActivateRagdollFromBulletImpact		= 107,  // Blocks ragdoll activation when hit by a bullet
        }
        if (DisableCriticalHits)
        {
            //EntryPoint.WriteToConsole($"SET DisableCriticalHits {pedExt.Handle}");
            NativeFunction.Natives.SET_PED_SUFFERS_CRITICAL_HITS(pedExt.Pedestrian, false);
        }
        if(DisableWrithe)
        {
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(pedExt.Pedestrian, (int)281, true);
        }
        if (DisableWritheShooting)
        {
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(pedExt.Pedestrian, (int)327, true);
        }
        pedExt.HasFullBodyArmor = HasFullBodyArmor;
        if (FiringPatternHash != 0)
        {
            //EntryPoint.WriteToConsole($"SET FiringPatternHash {pedExt.Handle}");
            NativeFunction.Natives.SET_PED_FIRING_PATTERN(pedExt.Pedestrian, FiringPatternHash);
        }
        if (overrideHealth)
        {
            int health = RandomItems.GetRandomNumberInt(HealthMin, HealthMax) + 100 + (IsAnimal ? 100 : 0);
            pedExt.Pedestrian.MaxHealth = health;
            pedExt.Pedestrian.Health = health;
        }
        if (overrideArmor)
        {
            int armor = RandomItems.GetRandomNumberInt(ArmorMin, ArmorMax);
            pedExt.Pedestrian.Armor = armor;
        }
        if (overrideAccuracy)
        {
            pedExt.Pedestrian.Accuracy = pedExt.Accuracy;
            NativeFunction.Natives.SET_PED_SHOOT_RATE(pedExt.Pedestrian, pedExt.ShootRate);
            NativeFunction.Natives.SET_PED_COMBAT_ABILITY(pedExt.Pedestrian, pedExt.CombatAbility);
            if(pedExt.CombatMovement != -1)
            {
                NativeFunction.Natives.SET_PED_COMBAT_MOVEMENT(pedExt.Pedestrian, pedExt.CombatMovement);
               // EntryPoint.WriteToConsole($"SET COMBAT MOVEMENT {pedExt.Handle} {pedExt.CombatMovement}");
            }
            if(pedExt.CombatRange != -1)
            {
                NativeFunction.Natives.SET_PED_COMBAT_RANGE(pedExt.Pedestrian, pedExt.CombatRange);
               // EntryPoint.WriteToConsole($"SET COMBAT RANGE {pedExt.Handle} {pedExt.CombatRange}");
            }
        }
        if(OverrideSightDistance >= 1.0f)
        {
            NativeFunction.Natives.SET_PED_SEEING_RANGE(pedExt.Pedestrian, OverrideSightDistance);
            EntryPoint.WriteToConsole($"OverrideSightDistance {OverrideSightDistance}");
        }
        GameFiber.Yield();
        if (!pedExt.Pedestrian.Exists())
        {
            return;
        }
        if (RandomItems.RandomPercent(NoHelmetPercentage))
        {
            NativeFunction.Natives.SET_PED_HELMET(pedExt.Pedestrian, false);
        }
        else
        {
            if (OverrideHelmet != null)
            {
                NativeFunction.Natives.SET_PED_HELMET_PROP_INDEX(pedExt.Pedestrian, OverrideHelmet.DrawableID, true);
                NativeFunction.Natives.SET_PED_HELMET_TEXTURE_INDEX(pedExt.Pedestrian, OverrideHelmet.TextureID);
            }
        }
        if (PedConfigFlagsToSet != null && PedConfigFlagsToSet.Any())
        {
            PedConfigFlagsToSet.ForEach(x => x.ApplyToPed(pedExt.Pedestrian));
        }
        if (CombatAttributesToSet != null && CombatAttributesToSet.Any())
        {
            CombatAttributesToSet.ForEach(x => x.ApplyToPed(pedExt.Pedestrian));
        }
        if (CombatFloatsToSet != null && CombatFloatsToSet.Any())
        {
            CombatFloatsToSet.ForEach(x => x.ApplyToPed(pedExt.Pedestrian));
        }
    }
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
    public PedVariation SetPedVariation(Ped ped, List<RandomHeadData> PossibleHeads, bool isSlow)
    {
        PedVariation variationToSet = new PedVariation();
        if (RequiredVariation == null)
        {
            ped.RandomizeVariation();
        }
        else
        {
            if (AllowRandomizeBeforeVariationApplied)
            {
                ped.RandomizeVariation();
            }
            bool isFreemode = ModelName.ToLower() == "mp_m_freemode_01" || ModelName.ToLower() == "mp_f_freemode_01";
            bool setDefaultFirst = false;
            if (isFreemode)
            {
                setDefaultFirst = true;
            }
            if (isSlow)
            {
                variationToSet = RequiredVariation.ApplyToPedSlow(ped, setDefaultFirst);
            }
            else
            {
                variationToSet = RequiredVariation.ApplyToPed(ped, setDefaultFirst, false);
            }
            if (RandomizeHead)//need to have a variation for this as its just freemode otherwise
            {
                bool isMale = ModelName.ToLower() == "mp_m_freemode_01";
                RandomHeadData rhd = null;
                rhd = PossibleHeads?.Where(x => x.IsMale == isMale).PickRandom();
                if (rhd != null)
                {
                    RandomHeadData rhd2 = PossibleHeads.Where(x => x.IsMale == isMale && x.Name != rhd.Name).PickRandom();
                    SetRandomizeHead(ped, rhd, rhd2, variationToSet, true);
                }
            }
            if (OptionalProps != null)
            {
                foreach (PedPropComponent prop in OptionalProps.GroupBy(x => x.PropID).Select(x => x.PickRandom()))
                {
                    if (ped.Exists() && RandomItems.RandomPercent(OptionalPropChance))
                    {
                        EntryPoint.WriteToConsole($"SET_PED_PROP_INDEX {ped.Handle} PropID{prop.PropID} DrawableID{prop.DrawableID} TextureID{prop.TextureID}");
                        NativeFunction.Natives.SET_PED_PROP_INDEX(ped, prop.PropID, prop.DrawableID, prop.TextureID, false);
                        PedPropComponent existingprop = variationToSet.Props.Where(x => x.PropID == prop.PropID).FirstOrDefault();
                        if(existingprop != null)
                        {
                            existingprop.DrawableID = prop.DrawableID;
                            existingprop.TextureID = prop.TextureID;
                        }
                        else
                        {
                            variationToSet.Props.Add(new PedPropComponent(prop.PropID, prop.DrawableID, prop.TextureID));
                        }
                    }
                }
            }
            
            if (OptionalComponents != null)
            {
                foreach (PedComponent component in OptionalComponents.GroupBy(x => x.ComponentID).Select(x => x.PickRandom()))
                {
                    if (ped.Exists() && RandomItems.RandomPercent(OptionalComponentChance))
                    {
                        NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, component.ComponentID, component.DrawableID, component.TextureID, component.PaletteID);
                        PedComponent existingComponent = variationToSet.Components.Where(x => x.ComponentID == component.ComponentID).FirstOrDefault();
                        if (existingComponent != null)
                        {
                            existingComponent.DrawableID = component.DrawableID;
                            existingComponent.TextureID = component.TextureID;
                        }
                        else
                        {
                            variationToSet.Components.Add(new PedComponent(component.ComponentID, component.DrawableID, component.TextureID, component.PaletteID));
                        }
                    }
                }
            }
            if(OptionalAppliedOverlayLogic != null)
            {
                OptionalAppliedOverlayLogic.ApplyToPed(ped,variationToSet);
            }
            if (isFreemode)
            {
                NativeFunction.Natives.x50B56988B170AFDF(ped, variationToSet.EyeColor);
            }
            if(CustomPropAttachments != null && 1==0)
            {
                foreach(CustomPropAttachment cpa in CustomPropAttachments)
                {
                    if(ped.Exists() && RandomItems.RandomPercent(OptionalComponentChance))
                    {
                        Rage.Object itemToAttach = new Rage.Object(Game.GetHashKey(cpa.PropName), ped.GetOffsetPositionUp(50f));
                        if (itemToAttach.Exists())
                        {
                            itemToAttach.AttachTo(ped, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", ped, cpa.BoneName), cpa.Attachment, cpa.Rotation);

                        }
                        GameFiber.Yield();
                    }
                }
            }
        }
        if (RequiredHelmetType != -1)
        {
            EntryPoint.WriteToConsole($"HELMET REQUIRED: PersonType.RequiredHelmetType {RequiredHelmetType}");
            ped.GiveHelmet(false, (HelmetTypes)RequiredHelmetType, 4096);
        }
        return variationToSet;


        

    }
    public void SetRandomizeHead(Ped ped, RandomHeadData myHead, RandomHeadData blendHead, PedVariation pedVariation, bool allowMorph)
    {
        GameFiber.Yield();
        if(!ped.Exists())
        {
            return;
        }
        if(pedVariation == null)
        {
            pedVariation = new PedVariation();
        }
        int HairColor = myHead.HairColors.PickRandom();
        int HairID = myHead.HairComponents.PickRandom();
        int EyeColor = myHead.EyeColors.PickRandom();

        NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, 2, HairID, 0, 0);
        pedVariation.Components.Add(new PedComponent(2, HairID, 0, 0));
        GameFiber.Yield();

        if (!ped.Exists())
        {
            return;
        }
        NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, myHead.HeadID, myHead.HeadID, 0, myHead.HeadID, myHead.HeadID, 0, 1.0f, 0, 0, false);
        if (blendHead == null)
        {
            NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, myHead.HeadID, myHead.HeadID, 0, myHead.HeadID, myHead.HeadID, 0, 1.0f, 0, 0, false);
            pedVariation.HeadBlendData = new HeadBlendData(myHead.HeadID, myHead.HeadID, 0, myHead.HeadID, myHead.HeadID, 0, 1.0f, 0, 0);
        }
        else
        {
            float Mix1 = RandomItems.GetRandomNumber(0.6f, 1.0f);
            float Mix2 = 1.0f - Mix1;
            NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, myHead.HeadID, blendHead.HeadID, 0, myHead.HeadID, blendHead.HeadID, 0, Mix1, Mix2, 0, false);
            pedVariation.HeadBlendData = new HeadBlendData(myHead.HeadID, blendHead.HeadID, 0, myHead.HeadID, blendHead.HeadID, 0, Mix1, Mix2, 0);
        }            
        GameFiber.Yield();
        if (!ped.Exists())
        {
            return;
        }
        pedVariation.PrimaryHairColor = HairColor;
        pedVariation.SecondaryHairColor = HairColor;
        NativeFunction.Natives.x4CFFC65454C93A49(ped, HairColor, HairColor);
        GameFiber.Yield();
        if (!ped.Exists())
        {
            return;
        }
        //set eyebrows
        //always set eyebrows
        pedVariation.HeadOverlays.Add(new HeadOverlayData(2, "Eyebrows") { ColorType = 1, Index = RandomItems.GetRandomNumberInt(0, 33), Opacity = 1.0f, PrimaryColor = HairColor, SecondaryColor = HairColor });
        if (RandomItems.RandomPercent(myHead.AgingPercentage))
        {
            pedVariation.HeadOverlays.Add(new HeadOverlayData(3, "Ageing") { Index = RandomItems.GetRandomNumberInt(0, 12), Opacity = RandomItems.GetRandomNumber(0.25f, 1.0f) });
        }
        if (myHead.IsMale)
        {
            if (RandomItems.RandomPercent(myHead.FacialHairPercentage))
            {
                pedVariation.HeadOverlays.Add(new HeadOverlayData(1, "Facial Hair") { ColorType = 1, Index = RandomItems.GetRandomNumberInt(0, 28), Opacity = RandomItems.GetRandomNumber(0.5f, 1.0f), PrimaryColor = HairColor, SecondaryColor = HairColor });
            }
        }
        else
        {
            if (RandomItems.RandomPercent(myHead.LipstickAndMakeupPercentage))
            {
                pedVariation.HeadOverlays.Add(new HeadOverlayData(4, "Makeup") { Index = RandomItems.GetRandomNumberInt(0, 15), Opacity = RandomItems.GetRandomNumber(0.25f, 1.0f) });
                int LipstickColor = RandomItems.GetRandomNumberInt(0, 13);
                pedVariation.HeadOverlays.Add(new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = RandomItems.GetRandomNumberInt(0, 7), Opacity = RandomItems.GetRandomNumber(0.25f,1.0f), PrimaryColor = LipstickColor, SecondaryColor = LipstickColor });
            }
        }
        foreach (HeadOverlayData hod in pedVariation.HeadOverlays)
        {
            NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, hod.OverlayID, hod.Index, hod.Opacity);
            NativeFunction.Natives.x497BF74A7B9CB952(ped, hod.OverlayID, hod.ColorType, hod.ColorType, hod.ColorType);//colors?
        }
        GameFiber.Yield();
        if (!ped.Exists())
        {
            return;
        }
        pedVariation.EyeColor = EyeColor;
        NativeFunction.Natives.x50B56988B170AFDF(ped, EyeColor);

        GameFiber.Yield();

        if (!allowMorph || !ped.Exists())
        {
            return;
        }
        List<FaceFeature>  FaceFeatures = new List<FaceFeature>() {
            new FaceFeature(0,"Nose Width"),
            new FaceFeature(1, "Nose Peak"),
            new FaceFeature(2, "Nose Length"),
            new FaceFeature(3, "Nose Bone Curveness"),
            new FaceFeature(4, "Nose Tip"),
            new FaceFeature(5, "Nose Bone Twist"),
            new FaceFeature(6, "Eyebrow Up/Down"),
            new FaceFeature(7, "Eyebrow In/Out"),
            new FaceFeature(8, "Cheek Bones Up/Down"),
            new FaceFeature(9, "Cheek Sideways Bone Size"),
            new FaceFeature(10, "Cheek Bones Width"),
            new FaceFeature(11, "Eye Opening"),
            new FaceFeature(12, "Lip Thickness"),
            new FaceFeature(13, "Jaw Bone Width"),
            new FaceFeature(14, "Jaw Bone Shape"),
            new FaceFeature(15, "Chin Bone"),
            new FaceFeature(16, "Chin Bone Length"),
            new FaceFeature(17, "Chin Bone Shape"),
            new FaceFeature(18, "Chin Hole") { RangeLow = 0.0f },
            new FaceFeature(19, "Neck Thickness") { RangeLow = 0.0f },
        };
        foreach (FaceFeature faceFeature in FaceFeatures)
        {
            if (!ped.Exists())
            {
                return;
            }
            if(RandomItems.RandomPercent(FaceFeatureRandomizePercentage))
            {
                float newScale = RandomItems.GetRandomNumber(faceFeature.RangeLow/2.0f, faceFeature.RangeHigh/2.0f);
                NativeFunction.Natives.x71A5C1DBA060049E(ped, faceFeature.Index, newScale);
                pedVariation.FaceFeatures.Add(new FaceFeature(faceFeature.Index, faceFeature.Name) { Index = faceFeature.Index, Scale = newScale, RangeLow = faceFeature.RangeLow, RangeHigh = faceFeature.RangeHigh });
                GameFiber.Yield();
            }

        }
        // EntryPoint.WriteToConsole($"myHead {myHead.HeadID} {myHead.Name} HairID {HairID} HairColor {HairColor}");      
    }
    public override string ToString()
    {
        string toReturn = "";
        if(!string.IsNullOrEmpty(DebugName))
        {
            toReturn = DebugName;
        }
        if (toReturn.Length > 0)
        {
            toReturn += $" - {ModelName}";
        }
        else
        {
            toReturn = ModelName;
        }
        return toReturn;
    }

    public void Setup(IIssuableWeapons issuableWeapons)
    {
        OverrideLessLethalWeapons = issuableWeapons.GetWeaponData(OverrideLessLethalWeaponsID);
        OverrideLongGuns = issuableWeapons.GetWeaponData(OverrideLongGunsID);
        OverrideSideArms = issuableWeapons.GetWeaponData(OverrideSideArmsID);
        OptionalAppliedOverlayLogic?.Setup();     
    }
}