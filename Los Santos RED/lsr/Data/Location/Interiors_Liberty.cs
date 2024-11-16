using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Interiors_Liberty
{
    private PossibleInteriors LibertyCityInteriors;

    public void DefaultConfig()
    {
        LibertyCityInteriors = new PossibleInteriors();
        GeneralInteriors();
        

        Serialization.SerializeParam(LibertyCityInteriors, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Interiors_{StaticStrings.LibertyConfigSuffix}.xml");
    }
    private void GeneralInteriors()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            new Interior(76290,"Perestroika") { },
            new Interior(138498, "Laundromat"){  },
            new Interior(160514,"Broker Fire Station"){  },
            new Interior(176898, "Bohan Fire Station"){  },


            new Interior(27394, "Memory Lanes"){  },
            new Interior(151554, "Beechwood Apts 2"),
            new Interior(119042, "Beechwood Apts 1"),
            new Interior(35330, "Beechwood Apts 3"),
            new Interior(118786, "Homebrew Cafe"){  },
            new Interior(37634, "Beechwood Apts 5"),
            
            new Interior(24578, "JJ China Limited"),

            new Interior(124418, "Burger Shot")
            { 
               
                Doors = new List<InteriorDoor>()  
                { 
                    //FRONT
                    new InteriorDoor(3024662465, new Vector3(1890.526f, 718.3006f, 25.46795f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                    new InteriorDoor(1050821746,new Vector3(1890.525f, 721.31f, 25.46795f)){ NeedsDefaultUnlock = true, LockWhenClosed = true },

                    //SIDE
                    new InteriorDoor(3024662465,new Vector3(1880.79f, 727.7058f, 25.4694f)){ NeedsDefaultUnlock = true,LockWhenClosed = true },
                    new InteriorDoor(1050821746,new Vector3(1877.78f, 727.7051f, 25.4694f)){ NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            },
            new Interior(156674, "Burger Shot"),
            new Interior(112642, "Burger Shot - Star Junction"),
            new Interior(109570, "Burger Shot - North Holland"),
            new Interior(134402, "Burger Shot - Bohan"),
            new Interior(105986, "Burger Shot - The Meat Quarter"),
            new Interior(59650, "Burger Shot - Alderney"),

            new Interior(143874, "Cluckin' Bell - Dukes"),
            new Interior(124162, "Cluckin' Bell - The Triangle"),

            new Interior(50178, "tw@ - Broker"){  },
            new Interior(166914, "tw@ - Bercham"),
            new Interior(66562, "tw@ - North Holland"),


            new Interior(139266, "The Libertonian"),





            //burger shot beechwoodd city
            //3024662465,new Vector3(1890.526f, 718.3006f, 25.46795f)

        });

        LibertyCityInteriors.ResidenceInteriors.AddRange(new List<ResidenceInterior>() 
        { 
           

        });

    }
}

