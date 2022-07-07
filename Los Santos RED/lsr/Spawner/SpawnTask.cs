using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Drawing;
using ExtensionsMethods;

public abstract class SpawnTask
{
    public SpawnTask(SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType)
    {
        SpawnLocation = spawnLocation;
        PersonType = personType;
        VehicleType = vehicleType;
    }
    public List<PedExt> CreatedPeople { get; private set; } = new List<PedExt>();
    public List<VehicleExt> CreatedVehicles { get; private set; } = new List<VehicleExt>();
    public SpawnLocation SpawnLocation { get; set; }
    public DispatchableVehicle VehicleType { get; set; }
    public DispatchablePerson PersonType { get; set; }
    public bool AllowAnySpawn { get; set; } = false;
    public bool AllowBuddySpawn { get; set; } = true;
    public Vector3 Position
    {
        get
        {
            if (VehicleType == null)
            {
                if (SpawnLocation.HasSidewalk)
                {
                    return SpawnLocation.SidewalkPosition;
                }
                return SpawnLocation.InitialPosition;
            }
            else if (VehicleType.IsHelicopter)
            {
                return SpawnLocation.InitialPosition + new Vector3(0f, 0f, 250f);
            }
            else if (VehicleType.IsBoat)
            {
                return SpawnLocation.InitialPosition;
            }
            else
            {
                return SpawnLocation.StreetPosition;
            }
        }
    }
    public bool PlacePedOnGround { get; set; } = false;
    public abstract void AttemptSpawn();
    public void SetPedVariation(Ped ped, List<RandomHeadData> PossibleHeads)
    {
        if (PersonType.RequiredVariation == null)
        {
            ped.RandomizeVariation();
        }
        else
        {
            if (PersonType.AllowRandomizeBeforeVariationApplied)
            {
                ped.RandomizeVariation();
            }
            bool isFreemode = PersonType.ModelName.ToLower() == "mp_m_freemode_01" || PersonType.ModelName.ToLower() == "mp_f_freemode_01";
            bool setDefaultFirst = false;
            if(isFreemode)
            {
                setDefaultFirst = true;
            }
            PersonType.RequiredVariation.ApplyToPedSlow(ped, setDefaultFirst);
            if (PersonType.RandomizeHead)//need to have a variation for this as its just freemode otherwise
            {
                bool isMale = PersonType.ModelName.ToLower() == "mp_m_freemode_01";
                RandomHeadData rhd = null;
                rhd = PossibleHeads.Where(x => x.IsMale == isMale).PickRandom();
                if (rhd != null)
                {
                    RandomHeadData rhd2 = PossibleHeads.Where(x => x.IsMale == isMale && x.Name != rhd.Name).PickRandom();
                    RandomizeHead(ped, rhd, rhd2);
                }
            }
            if(PersonType.OptionalProps != null)
            {
                foreach(PedPropComponent prop in PersonType.OptionalProps.GroupBy(x=>x.PropID).Select(x=> x.PickRandom()))
                {
                    if (ped.Exists() && RandomItems.RandomPercent(PersonType.OptionalPropChance))
                    {
                        NativeFunction.Natives.SET_PED_PROP_INDEX(ped, prop.PropID, prop.DrawableID, prop.TextureID, false);
                    }
                }
            }
            if (PersonType.OptionalComponents != null)
            {
                foreach (PedComponent component in PersonType.OptionalComponents.GroupBy(x => x.ComponentID).Select(x => x.PickRandom()))
                {
                    if (ped.Exists() && RandomItems.RandomPercent(PersonType.OptionalComponentChance))
                    {
                        NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, component.ComponentID, component.DrawableID, component.TextureID, component.PaletteID);
                    }
                }
            }
        }
        if (PersonType.RequiredHelmetType != -1)
        {
            EntryPoint.WriteToConsole($"HELMET REQUIRED: PersonType.RequiredHelmetType {PersonType.RequiredHelmetType}");
            ped.GiveHelmet(false, (HelmetTypes)PersonType.RequiredHelmetType, 4096);
        }
    }
    public void RandomizeHead(Ped ped, RandomHeadData myHead, RandomHeadData blendHead)
    {
        GameFiber.Yield();
        if (ped.Exists())
        {
            int HairColor = myHead.HairColors.PickRandom();
            int HairID = myHead.HairComponents.PickRandom();
            int EyeColor = myHead.EyeColors.PickRandom();
            if (ped.Exists())
            {
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, 2, HairID, 0, 0);
                GameFiber.Yield();
            }
            if (ped.Exists())
            {
                NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, myHead.HeadID, myHead.HeadID, 0, myHead.HeadID, myHead.HeadID, 0, 1.0f, 0, 0, false);
                //if (blendHead == null)
                //{
                //    NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, myHead.HeadID, myHead.HeadID, 0, myHead.HeadID, myHead.HeadID, 0, 1.0f, 0, 0, false);
                //}
                //else
                //{
                //    float Mix1 = RandomItems.GetRandomNumber(0.6f, 1.0f);
                //    float Mix2 = 1.0f - Mix1;
                //    NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, myHead.HeadID, blendHead.HeadID, 0, myHead.HeadID, blendHead.HeadID, Mix1, Mix2, 0, 0, false);
                //}
                GameFiber.Yield();
            }
            if (ped.Exists())
            {
                NativeFunction.Natives.x4CFFC65454C93A49(ped, HairColor, HairColor);
                
                GameFiber.Yield();
            }
            if (ped.Exists())
            {
                NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, 2, RandomItems.GetRandomNumberInt(0, 5), 1.0f);
                GameFiber.Yield();
            }
            if (ped.Exists())
            {
                NativeFunction.Natives.x497BF74A7B9CB952(ped, 2, 1, HairColor, HairColor);//colors?
                GameFiber.Yield();
            }
            if (ped.Exists())
            {
                NativeFunction.Natives.x50B56988B170AFDF(ped, EyeColor);
            }
            EntryPoint.WriteToConsole($"myHead {myHead.HeadID} {myHead.Name} HairID {HairID} HairColor {HairColor}");
        }
    }
}
