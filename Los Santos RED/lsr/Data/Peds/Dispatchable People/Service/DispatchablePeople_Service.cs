using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DispatchablePeople_Service
{
    private DispatchablePeople DispatchablePeople;
    private List<PedPropComponent> MaleShortSleeveOptions;
    private List<PedPropComponent> FemaleShortSleeveOptions;

    public DispatchablePeople_Service(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void Setup()
    {
        MaleShortSleeveOptions = new List<PedPropComponent>() {

            new PedPropComponent(1, 35, 0),
            new PedPropComponent(1, 37, 0),
            new PedPropComponent(1, 38, 0),
            new PedPropComponent(1, 39, 0),
            




            new PedPropComponent(6, 3, 0),


        };

        FemaleShortSleeveOptions = new List<PedPropComponent>() {
            new PedPropComponent(1, 36, 0),
            new PedPropComponent(1, 37, 0),
            new PedPropComponent(1, 38, 0),
            new PedPropComponent(1, 39, 0),
            new PedPropComponent(1, 40, 0),
            



            new PedPropComponent(6, 20, 2),


        };
    }
    public DispatchablePerson CreateBurgerShotPed(bool isMale, bool withHat)
    {
        if (isMale)
        {
            DispatchablePerson Male = new DispatchablePerson("mp_m_freemode_01", 100, 100)
            {
                RandomizeHead = true,
                RequiredVariation = new PedVariation(new List<PedPropComponent>() { }, new List<PedComponent>(){
                    new PedComponent(4,22,7),//lower//colors
                    new PedComponent(6,8,0),//shoes//colors
                    new PedComponent(8,15,0),//undershirts
                    new PedComponent(11,22,2),//tops//colors
                }),
                OptionalAppliedOverlayLogic = new OptionalAppliedOverlayLogic()
                {
                    OptionalAppliedOverlays = new List<OptionalAppliedOverlay>()
                    {
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_068_M","ZONE_TORSO"),
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_069_M","ZONE_TORSO"),
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_070_M","ZONE_TORSO"),

                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_095_M","ZONE_TORSO"),//white bs text
                    },
                    AppliedOverlayZonePercentages = new List<AppliedOverlayZonePercentage>() { new AppliedOverlayZonePercentage("ZONE_TORSO", 100f, 1) }
                },
                OptionalComponents = new List<PedComponent>()
                {
                    new PedComponent(4,22,0),new PedComponent(4,22,8),
                    new PedComponent(6,8,1),new PedComponent(6,8,2),new PedComponent(6,8,3),new PedComponent(6,8,4),new PedComponent(6,8,5),new PedComponent(6,8,6),new PedComponent(6,8,7),new PedComponent(6,8,8),

                    new PedComponent(1,144,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),//goofy hat

                },
                OptionalComponentChance = 75,
                OptionalProps = new List<PedPropComponent>() {
                    new PedPropComponent(0, 130, 0),
                    new PedPropComponent(0, 130, 1),
                    new PedPropComponent(0, 130, 2),
                    new PedPropComponent(0, 130, 3),


                    
                },//hat
                OptionalPropChance = 65,
                OverrideVoice = DispatchablePeople.GeneralMaleVoices,

            };
            if (withHat)
            {
                Male.OptionalComponents.RemoveAll(x => x.DrawableID == 1);//remove all masks
            }
            else
            {
                Male.OptionalProps = new List<PedPropComponent>();
            }
            Male.OptionalProps.AddRange(MaleShortSleeveOptions);
            return Male;
        }
        else
        {
            DispatchablePerson Female = new DispatchablePerson("mp_f_freemode_01", 100, 100)
            {
                RandomizeHead = true,
                RequiredVariation = new PedVariation(new List<PedPropComponent>() { }, new List<PedComponent>(){
                    new PedComponent(3,14,0),//torso
                    new PedComponent(4,64,0),//lower//colors
                    new PedComponent(6,1,0),//shoes//colors
                    new PedComponent(8,15,0),//undershirts
                    new PedComponent(11,338,7),//tops//colors
                }),
                OptionalComponents = new List<PedComponent>()
                {
                    new PedComponent(4,64,3),
                    new PedComponent(6,1,1),new PedComponent(6,1,2),new PedComponent(6,1,3),new PedComponent(6,1,4),new PedComponent(6,1,5),new PedComponent(6,1,6),new PedComponent(6,1,7),new PedComponent(6,1,8),
                    new PedComponent(1,144,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),//goofy hat

                },
                OptionalComponentChance = 75,
                OptionalAppliedOverlayLogic = new OptionalAppliedOverlayLogic()
                {
                    OptionalAppliedOverlays = new List<OptionalAppliedOverlay>()
                    {
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_068_F","ZONE_TORSO"),
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_069_F","ZONE_TORSO"),
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_070_F","ZONE_TORSO"),


                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_095_F","ZONE_TORSO"),//white bs text
                    },
                    AppliedOverlayZonePercentages = new List<AppliedOverlayZonePercentage>() { new AppliedOverlayZonePercentage("ZONE_TORSO", 100f, 1) }
                },
                OptionalProps = new List<PedPropComponent>() {
                    new PedPropComponent(0, 129, 0),
                    new PedPropComponent(0, 129, 1),
                    new PedPropComponent(0, 129, 2),
                    new PedPropComponent(0, 129, 3),
                },
                OptionalPropChance = 65,
                OverrideVoice = DispatchablePeople.GeneralFemaleVoices,
            };
            if (withHat)
            {
                Female.OptionalComponents.RemoveAll(x => x.DrawableID == 1);//remove all masks
            }
            else
            {
                Female.OptionalProps = new List<PedPropComponent>();
            }
            Female.OptionalProps.AddRange(FemaleShortSleeveOptions);
            return Female;
        }
    }
    public DispatchablePerson CreateCluckinBellPed(bool isMale, bool withHat, bool isAlt)
    {
        if (isMale)
        {
            DispatchablePerson Male = new DispatchablePerson("mp_m_freemode_01", 100, 100)
            {
                RandomizeHead = true,
                RequiredVariation = new PedVariation(new List<PedPropComponent>() { }, new List<PedComponent>(){
                    new PedComponent(4,22,7),//legs//colors, 0 == white beige, 7 == beige, 8 == white
                    new PedComponent(6,8,0),//shoes//colors, 0 == white, 0-15 are OK

                    new PedComponent(8,15,0),//undershirt,none
                    new PedComponent(11,345,2),//top//colors, t shirt 0 == orange, 1 == pinkish,  2 == yellow, 3 = puke green, 4 == red, 5 == lime green, 6 = blue, 7 = lighter red

                    //new PedComponent(11,22,2),//top//colors t shirt 0 == white, 1 == black, 2 == red
                }),
                OptionalComponents = new List<PedComponent>()
                {
                    new PedComponent(4,22,0),new PedComponent(4,22,8),
                    new PedComponent(6,8,1),new PedComponent(6,8,2),new PedComponent(6,8,3),new PedComponent(6,8,4),new PedComponent(6,8,5),new PedComponent(6,8,6),new PedComponent(6,8,7),new PedComponent(6,8,8),

                    new PedComponent(1,145,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),//goofy hat
                },
                OptionalComponentChance = 75,
                OptionalAppliedOverlayLogic = new OptionalAppliedOverlayLogic()
                {
                    OptionalAppliedOverlays = new List<OptionalAppliedOverlay>()
                    {
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_073_M","ZONE_TORSO"),//colored logo
                    },
                    AppliedOverlayZonePercentages = new List<AppliedOverlayZonePercentage>() { new AppliedOverlayZonePercentage("ZONE_TORSO", 100f, 1) }
                },

                OptionalProps = new List<PedPropComponent>() {
                    new PedPropComponent(0, 130, 4),//hat//some variants  
                    new PedPropComponent(0, 130, 5),//hat//some variants  
                    new PedPropComponent(0, 130, 6),//hat//some variants  
                },//hat
                OptionalPropChance = 65,
                OverrideVoice = DispatchablePeople.GeneralMaleVoices,

            };
            if(withHat)
            {
                Male.OptionalComponents.RemoveAll(x => x.ComponentID == 1);//remove all masks
            }
            else
            {
                Male.OptionalProps = new List<PedPropComponent>();
            }
            if(isAlt)//give them a blue shirt with yellow logo
            {
                Male.OptionalAppliedOverlayLogic.OptionalAppliedOverlays.Clear();
                Male.OptionalAppliedOverlayLogic.OptionalAppliedOverlays = new List<OptionalAppliedOverlay>()
                    {
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_071_M","ZONE_TORSO"),//white text
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_072_M","ZONE_TORSO"),//white text
                    };
                Male.RequiredVariation.Components.RemoveAll(x => x.ComponentID == 11);
                Male.RequiredVariation.Components.Add(new PedComponent(11, 345, 6));

            }


            Male.OptionalProps.AddRange(MaleShortSleeveOptions);
            return Male;
        }
        else
        {
            DispatchablePerson Female = new DispatchablePerson("mp_f_freemode_01", 100, 100)
            {
                RandomizeHead = true,
                RequiredVariation = new PedVariation(new List<PedPropComponent>() { }, new List<PedComponent>(){
                    new PedComponent(3,14,0),//torso
                    new PedComponent(4,64,0),//legs//colors, 0 = beige, 1 == black, 2 == od, 3== grey,
                    new PedComponent(6,1,0),//shoes//colors, 3 == regular?
                    new PedComponent(8,15,0),//undershirt,none
                    new PedComponent(11,338,5),//top//colors,0==orange,1==black,2==white,3 = yellow, 4 = pink,purple, green, 5 == yellow, 6 == puke green, 7 = red, 8 = lime, 9 = sky blue
                }),
                OptionalComponents = new List<PedComponent>()
                {
                    new PedComponent(4,64,3),
                    new PedComponent(6,1,1),new PedComponent(6,1,2),new PedComponent(6,1,3),new PedComponent(6,1,4),new PedComponent(6,1,5),new PedComponent(6,1,6),new PedComponent(6,1,7),new PedComponent(6,1,8),
                    new PedComponent(1,145,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),new PedComponent(1,0,0),//goofy hat
                },
                OptionalComponentChance = 75,
                OptionalAppliedOverlayLogic = new OptionalAppliedOverlayLogic()
                {
                    OptionalAppliedOverlays = new List<OptionalAppliedOverlay>()
                    {
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_073_F","ZONE_TORSO"),//CluckinBellLogoBig
                    },
                    AppliedOverlayZonePercentages = new List<AppliedOverlayZonePercentage>() { new AppliedOverlayZonePercentage("ZONE_TORSO", 100f, 1) }
                },
                OptionalProps = new List<PedPropComponent>() {
                    new PedPropComponent(0, 129, 4),//hat//some variants 
                    new PedPropComponent(0, 129, 5),//hat//some variants 
                    new PedPropComponent(0, 129, 6),//hat//some variants 
                },
                OptionalPropChance = 65,
                OverrideVoice = DispatchablePeople.GeneralFemaleVoices,
            };
            if (withHat)
            {
                Female.OptionalComponents.RemoveAll(x => x.ComponentID == 1);//remove all masks
            }
            else
            {
                Female.OptionalProps = new List<PedPropComponent>();
            }
            if (isAlt)//give them a blue shirt with yellow logo
            {
                Female.OptionalAppliedOverlayLogic.OptionalAppliedOverlays.Clear();
                Female.OptionalAppliedOverlayLogic.OptionalAppliedOverlays = new List<OptionalAppliedOverlay>()
                    {
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_071_F","ZONE_TORSO"),//white text
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_072_F","ZONE_TORSO"),//white text
                    };
                Female.RequiredVariation.Components.RemoveAll(x => x.ComponentID == 11);
                Female.RequiredVariation.Components.Add(new PedComponent(11, 338, 9));

            }
            Female.OptionalProps.AddRange(FemaleShortSleeveOptions);
            return Female;
        }
    }

    public DispatchablePerson CreatetwatPed(bool isMale)
    {
        if (isMale)
        {
            DispatchablePerson Male = new DispatchablePerson("mp_m_freemode_01", 100, 100)
            {
                RandomizeHead = true,
                RequiredVariation = new PedVariation(new List<PedPropComponent>() { }, new List<PedComponent>(){
                    new PedComponent(4,22,7),//legs//colors, 0 == white beige, 7 == beige, 8 == white
                    new PedComponent(6,8,0),//shoes//colors, 0 == white, 0-15 are OK

                    new PedComponent(8,15,0),//undershirt,none
                    new PedComponent(11,22,0),//top//colors, t shirt 0 == orange, 1 == pinkish,  2 == yellow, 3 = puke green, 4 == red, 5 == lime green, 6 = blue, 7 = lighter red

                    //new PedComponent(11,22,2),//top//colors t shirt 0 == white, 1 == black, 2 == red
                }),
                OptionalComponents = new List<PedComponent>()
                {
                    new PedComponent(4,22,0),new PedComponent(4,22,8),
                    new PedComponent(6,8,1),new PedComponent(6,8,2),new PedComponent(6,8,3),new PedComponent(6,8,4),new PedComponent(6,8,5),new PedComponent(6,8,6),new PedComponent(6,8,7),new PedComponent(6,8,8),
                },
                OptionalAppliedOverlayLogic = new OptionalAppliedOverlayLogic()
                {
                    OptionalAppliedOverlays = new List<OptionalAppliedOverlay>()
                    {
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_124_M","ZONE_TORSO"),
                    },
                    AppliedOverlayZonePercentages = new List<AppliedOverlayZonePercentage>() { new AppliedOverlayZonePercentage("ZONE_TORSO", 100f, 1) }
                },

                OptionalProps = new List<PedPropComponent>() {
 
                },//hat

                OptionalPropChance = 65,
                OverrideVoice = DispatchablePeople.GeneralMaleVoices,

            };
            Male.OptionalProps.AddRange(MaleShortSleeveOptions);
            return Male;
        }
        else
        {
            DispatchablePerson Female = new DispatchablePerson("mp_f_freemode_01", 100, 100)
            {
                RandomizeHead = true,
                RequiredVariation = new PedVariation(new List<PedPropComponent>() { }, new List<PedComponent>(){
                    new PedComponent(3,14,0),//torso
                    new PedComponent(4,64,0),//legs//colors, 0 = beige, 1 == black, 2 == od, 3== grey,
                    new PedComponent(6,1,0),//shoes//colors, 3 == regular?
                    new PedComponent(8,15,0),//undershirt,none
                    new PedComponent(11,338,2),//top//colors,0==orange,1==black,2==white,3 = yellow, 4 = pink,purple, green, 5 == yellow, 6 == puke green, 7 = red, 8 = lime, 9 = sky blue
                }),
                OptionalComponents = new List<PedComponent>()
                {
                    new PedComponent(4,64,3),
                    new PedComponent(6,1,1),new PedComponent(6,1,2),new PedComponent(6,1,3),new PedComponent(6,1,4),new PedComponent(6,1,5),new PedComponent(6,1,6),new PedComponent(6,1,7),new PedComponent(6,1,8),
                },
                OptionalAppliedOverlayLogic = new OptionalAppliedOverlayLogic()
                {
                    OptionalAppliedOverlays = new List<OptionalAppliedOverlay>()
                    {
                        new OptionalAppliedOverlay("mpChristmas2018_overlays","MP_Christmas2018_Tee_124_F","ZONE_TORSO"),
                    },
                    AppliedOverlayZonePercentages = new List<AppliedOverlayZonePercentage>() { new AppliedOverlayZonePercentage("ZONE_TORSO", 100f, 1) }
                },
                OptionalProps = new List<PedPropComponent>() {

                },
                OptionalPropChance = 65,
                OverrideVoice = DispatchablePeople.GeneralFemaleVoices,
            };
            Female.OptionalProps.AddRange(FemaleShortSleeveOptions);
            return Female;
        }
    }
}

