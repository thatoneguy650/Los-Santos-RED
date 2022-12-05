using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public int TaserAccuracyMin { get; set; } = 30;
    public int TaserAccuracyMax { get; set; } = 30;
    public int TaserShootRateMin { get; set; } = 100;
    public int TaserShootRateMax { get; set; } = 100;

    public int VehicleAccuracyMin { get; set; } = 2;//5
    public int VehicleAccuracyMax { get; set; } = 4;//5
    public int VehicleShootRateMin { get; set; } = 10;//20
    public int VehicleShootRateMax { get; set; } = 15;//20

    public string UnitCode { get; set; } = "";
    public int RequiredHelmetType { get; set; } = -1;

    public bool AllowRandomizeBeforeVariationApplied { get; set; } = false;
    public bool RandomizeHead { get; set; } = false;

    public PedVariation RequiredVariation { get; set; }
    public List<PedPropComponent> OptionalProps { get; set; }
    public int OptionalPropChance { get; set; } = 30;
    public List<PedComponent> OptionalComponents { get; set; }
    public int OptionalComponentChance { get; set; } = 30;
    public PedComponent EmptyHolster { get; set; }
    public PedComponent FullHolster { get; set; }
    public List<string> OverrideVoice { get; set; }
    public List<CustomPropAttachment> CustomPropAttachments { get; set; }
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
    public PedVariation SetPedVariation(Ped ped, List<RandomHeadData> PossibleHeads, bool isSlow)
    {
        PedVariation variationToSet = new PedVariation();
        if (RequiredVariation == null)
        {
            ped.RandomizeVariation();
        }
        else
        {
            variationToSet = RequiredVariation;
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
                RequiredVariation.ApplyToPedSlow(ped, setDefaultFirst);
            }
            else
            {
                RequiredVariation.ApplyToPed(ped, setDefaultFirst);
            }
            if (RandomizeHead)//need to have a variation for this as its just freemode otherwise
            {
                bool isMale = ModelName.ToLower() == "mp_m_freemode_01";
                RandomHeadData rhd = null;
                rhd = PossibleHeads.Where(x => x.IsMale == isMale).PickRandom();
                if (rhd != null)
                {
                    RandomHeadData rhd2 = PossibleHeads.Where(x => x.IsMale == isMale && x.Name != rhd.Name).PickRandom();
                    SetRandomizeHead(ped, rhd, rhd2, variationToSet);
                }
            }
            if (OptionalProps != null)
            {
                foreach (PedPropComponent prop in OptionalProps.GroupBy(x => x.PropID).Select(x => x.PickRandom()))
                {
                    if (ped.Exists() && RandomItems.RandomPercent(OptionalPropChance))
                    {
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
    public void SetRandomizeHead(Ped ped, RandomHeadData myHead, RandomHeadData blendHead, PedVariation pedVariation)
    {
        GameFiber.Yield();
        if (ped.Exists())
        {
            if(pedVariation == null)
            {
                pedVariation = new PedVariation();
            }
            int HairColor = myHead.HairColors.PickRandom();
            int HairID = myHead.HairComponents.PickRandom();
            int EyeColor = myHead.EyeColors.PickRandom();
            if (ped.Exists())
            {
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, 2, HairID, 0, 0);
                pedVariation.Components.Add(new PedComponent(2, HairID, 0, 0));
                GameFiber.Yield();
            }
            if (ped.Exists())
            {
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
            }
            if (ped.Exists())
            {
                pedVariation.PrimaryHairColor = HairColor;
                pedVariation.SecondaryHairColor = HairColor;
                NativeFunction.Natives.x4CFFC65454C93A49(ped, HairColor, HairColor);
                GameFiber.Yield();
            }
            if (ped.Exists())//set eyebrows
            {
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
                //NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, 2, EyebrowIndex, 1.0f);
                //NativeFunction.Natives.x497BF74A7B9CB952(ped, 2, 1, HairColor, HairColor);//colors?
                GameFiber.Yield();
            }
            if (ped.Exists())
            {
                pedVariation.EyeColor = EyeColor;
                NativeFunction.Natives.x50B56988B170AFDF(ped, EyeColor);
            }
            EntryPoint.WriteToConsole($"myHead {myHead.HeadID} {myHead.Name} HairID {HairID} HairColor {HairColor}");
        }
    }


}