using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class PedVariation
{
    public PedVariation()
    {

    }
    public PedVariation(List<PedPropComponent> _MyPedProps)
    {
        Props = _MyPedProps;
    }
    public PedVariation(List<PedComponent> _MyPedComponents)
    {
        Components = _MyPedComponents;
    }
    public PedVariation(List<PedComponent> _MyPedComponents, List<PedPropComponent> _MyPedProps)
    {
        Components = _MyPedComponents;
        Props = _MyPedProps;
    }
    public PedVariation(List<PedComponent> _MyPedComponents, List<PedPropComponent> _MyPedProps, List<HeadOverlayData> headOverlays, HeadBlendData headBlendData, int primaryHairColor, int secondaryHairColor)
    {
        Components = _MyPedComponents;
        Props = _MyPedProps;
        HeadOverlays = headOverlays;
        HeadBlendData = headBlendData;
        PrimaryHairColor = primaryHairColor;
        SecondaryHairColor = secondaryHairColor;
    }
    public List<PedComponent> Components { get; set; } = new List<PedComponent>();
    public List<PedPropComponent> Props { get; set; } = new List<PedPropComponent>();
    public List<HeadOverlayData> HeadOverlays { get; set; } = new List<HeadOverlayData>();
    public HeadBlendData HeadBlendData { get; set; } = new HeadBlendData();
    public int PrimaryHairColor { get; set; } = -1;
    public int SecondaryHairColor { get; set; } = -1;


    public bool SetRandomRegularHeadVariation { get; set; } = false;
    public bool IsMaleRandomRegularHeadVariation { get; set; } = true;

    public void ApplyToPed(Ped ped)
    {
        try
        {
            NativeFunction.Natives.SET_PED_DEFAULT_COMPONENT_VARIATION(ped);
            foreach (PedComponent Component in Components)
            {
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID);
            }
            NativeFunction.Natives.CLEAR_ALL_PED_PROPS(ped);
            foreach (PedPropComponent Prop in Props)
            {
                NativeFunction.Natives.SET_PED_PROP_INDEX(ped, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
            }
            //Freemode only below
            if (SetRandomRegularHeadVariation)
            {
                GetRegularRandomHead(ped);
            }
            else if (HeadBlendData != null && (HeadBlendData.shapeFirst != -1 || HeadBlendData.shapeSecond != -1 || HeadBlendData.shapeThird != -1))
            {
                NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, HeadBlendData.shapeFirst, HeadBlendData.shapeSecond, HeadBlendData.shapeThird, HeadBlendData.skinFirst, HeadBlendData.skinSecond, HeadBlendData.skinThird, HeadBlendData.shapeMix, HeadBlendData.skinMix, HeadBlendData.thirdMix, false);
                if (PrimaryHairColor != -1 && SecondaryHairColor != -1)
                {
                    NativeFunction.Natives.x4CFFC65454C93A49(ped, PrimaryHairColor, SecondaryHairColor);
                }
                foreach (HeadOverlayData headOverlayData in HeadOverlays)
                {
                    NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, headOverlayData.OverlayID, headOverlayData.Index, headOverlayData.Opacity);
                    NativeFunction.Natives.x497BF74A7B9CB952(ped, headOverlayData.OverlayID, headOverlayData.ColorType, headOverlayData.PrimaryColor, headOverlayData.SecondaryColor);//colors?
                }
            }
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"ReplacePedComponentVariation Error {ex.Message} {ex.StackTrace}", 0);
        }
    }
    private void GetRegularRandomHead(Ped ped)
    {
        GameFiber.Yield();
        if (ped.Exists())
        {
            List<RandomHeadData> HeadList = new List<RandomHeadData>();
            if (IsMaleRandomRegularHeadVariation)
            {
                List<int> WhiteHairStyles_Male = new List<int>() { 2, 3, 4, 5, 7, 9, 10, 11, 12, 18, 19, 66 };
                List<int> BrownHairStyles_Male = new List<int>() { 2, 3, 4, 5, 7, 9, 10, 11, 12, 18, 19, 66 };
                List<int> AsianHairStyles_Male = new List<int>() { 2, 3, 4, 5, 7, 9, 10, 11, 12, 18, 19, 66 };
                List<int> BlackHairStyles_Male = new List<int>() { 0, 1, 8, 14, 24, 25, 30, 72 };

                List<int> WhiteHairColors_Male = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
                List<int> BrownHairColors_Male = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                List<int> AsianHairColors_Male = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                List<int> BlackHairColors_Male = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };

                HeadList = new List<RandomHeadData>()
                    {
                        new RandomHeadData(0,"Benjamin",WhiteHairColors_Male,WhiteHairStyles_Male),//white male
                        new RandomHeadData(1,"Daniel",WhiteHairColors_Male,WhiteHairStyles_Male),//white male
                        new RandomHeadData(4,"Andrew",WhiteHairColors_Male,WhiteHairStyles_Male),//white male
                        new RandomHeadData(5,"Juan",WhiteHairColors_Male,WhiteHairStyles_Male),//white male
                        new RandomHeadData(12,"Diego",WhiteHairColors_Male,WhiteHairStyles_Male),//white male
                        new RandomHeadData(13,"Adrian",WhiteHairColors_Male,WhiteHairStyles_Male),//white male
                        new RandomHeadData(42,"Claude",WhiteHairColors_Male,WhiteHairStyles_Male),//white male
                        new RandomHeadData(43,"Niko",WhiteHairColors_Male,WhiteHairStyles_Male),//white male
                        new RandomHeadData(44,"John",WhiteHairColors_Male,WhiteHairStyles_Male),///white male
                        new RandomHeadData(2,"Joshua",BlackHairColors_Male,BlackHairStyles_Male),//black male
                        new RandomHeadData(3,"Noah",BlackHairColors_Male,BlackHairStyles_Male),//black male
                        new RandomHeadData(15,"Michael",BlackHairColors_Male,BlackHairStyles_Male),//black male
                        new RandomHeadData(19,"Samuel",BlackHairColors_Male,BlackHairStyles_Male),//black male
                         new RandomHeadData(8,"Evan",BrownHairColors_Male,BrownHairStyles_Male),//brown male
                        new RandomHeadData(9,"Ethan",BrownHairColors_Male,BrownHairStyles_Male),//brown male
                        new RandomHeadData(10,"Vincent",BrownHairColors_Male,BrownHairStyles_Male),//brown male
                        new RandomHeadData(11,"Angel",BrownHairColors_Male,BrownHairStyles_Male),//brown male
                        new RandomHeadData(16,"Santiago",BrownHairColors_Male,BrownHairStyles_Male),//brown male
                        new RandomHeadData(20,"Anthony",BrownHairColors_Male,BrownHairStyles_Male),//brown male
                        new RandomHeadData(7,"Isaac",AsianHairColors_Male,AsianHairStyles_Male),//asian male
                        new RandomHeadData(17,"Kevin",AsianHairColors_Male,AsianHairStyles_Male),//asian male
                        new RandomHeadData(18,"Louis",AsianHairColors_Male,AsianHairStyles_Male),//asian male            
                    };
            }
            else
            {

                List<int> WhiteHairStyles_Female = new List<int>() { 1, 2, 4, 10, 11, 14, 15, 16, 17, 21, 48 };
                List<int> BrownHairStyles_Female = new List<int>() { 1, 2, 4, 10, 11, 14, 15, 16, 17, 21, 48 };
                List<int> AsianHairStyles_Female = new List<int>() { 1, 2, 4, 10, 11, 14, 15, 16, 17, 21, 48 };
                List<int> BlackHairStyles_Female = new List<int>() { 6, 2, 4, 10, 11, 20, 22, 25, 54, 58 };

                List<int> WhiteHairColors_Female = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
                List<int> BrownHairColors_Female = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                List<int> AsianHairColors_Female = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
                List<int> BlackHairColors_Female = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };

                HeadList = new List<RandomHeadData>()
                    {
                        new RandomHeadData(6,"Alex",AsianHairColors_Female,AsianHairStyles_Female),//asian female
                        new RandomHeadData(27,"Zoe",AsianHairColors_Female,AsianHairStyles_Female),//asian female
                        new RandomHeadData(28,"Ava",AsianHairColors_Female,AsianHairStyles_Female),//asian female
                        new RandomHeadData(39,"Elizabeth",AsianHairColors_Female,AsianHairStyles_Female),//asian female
                        new RandomHeadData(33,"Nicole",WhiteHairColors_Female,WhiteHairStyles_Female),//white female
                        new RandomHeadData(34,"Ashley",WhiteHairColors_Female,WhiteHairStyles_Female),//white female
                        new RandomHeadData(21,"Hannah",WhiteHairColors_Female,WhiteHairStyles_Female),//white female
                        new RandomHeadData(22,"Audrey",WhiteHairColors_Female,WhiteHairStyles_Female),//white female
                        new RandomHeadData(40,"Charlotte",WhiteHairColors_Female,WhiteHairStyles_Female),//white female
                        new RandomHeadData(45,"Misty",WhiteHairColors_Female,WhiteHairStyles_Female),//white female
                        new RandomHeadData(14,"Gabriel",BlackHairColors_Female,BlackHairStyles_Female),//black female
                        new RandomHeadData(23,"Jasmine",BlackHairColors_Female,BlackHairStyles_Female),//black female
                        new RandomHeadData(24,"Giselle",BlackHairColors_Female,BlackHairStyles_Female),//black female
                        new RandomHeadData(35,"Grace",BlackHairColors_Female,BlackHairStyles_Female),//black female
                        new RandomHeadData(36,"Brianna",BlackHairColors_Female,BlackHairStyles_Female),//black female
                        new RandomHeadData(25,"Amelia",BrownHairColors_Female,BrownHairStyles_Female),//brown female
                        new RandomHeadData(26,"Isabella",BrownHairColors_Female,BrownHairStyles_Female),//brown female
                        new RandomHeadData(29,"Camila",BrownHairColors_Female,BrownHairStyles_Female),//brown female
                        new RandomHeadData(30,"Violet",BrownHairColors_Female,BrownHairStyles_Female),//brown female
                        new RandomHeadData(31,"Sophia",BrownHairColors_Female,BrownHairStyles_Female),//brown female
                        new RandomHeadData(32,"Evelyn",BrownHairColors_Female,BrownHairStyles_Female),//brown female
                        new RandomHeadData(37,"Natalie",BrownHairColors_Female,BrownHairStyles_Female),//brown female
                        new RandomHeadData(38,"Olivia",BrownHairColors_Female,BrownHairStyles_Female),//brown female     
                        new RandomHeadData(41,"Emma",BrownHairColors_Female,BrownHairStyles_Female),//brown female            
                    };
            }
            if (HeadList.Any())
            {

                RandomHeadData myHead = HeadList.PickRandom();
                int HairColor = myHead.HairColors.PickRandom();
                int HairID = myHead.HairComponents.PickRandom();
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, 2, HairID, 0, 0);


                NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, myHead.HeadID, myHead.HeadID, 0, myHead.HeadID, myHead.HeadID, 0, 1.0f, 0, 0, false);
                NativeFunction.Natives.x4CFFC65454C93A49(ped, HairColor, HairColor);
                NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, 2, RandomItems.GetRandomNumberInt(0,5), 1.0f);
                NativeFunction.Natives.x497BF74A7B9CB952(ped, 2, 1, HairColor, HairColor);//colors?


                //EntryPoint.WriteToConsole($"myHead {myHead.HeadID} {myHead.Name} HairID {HairID}");
            }
        }
    }
}

