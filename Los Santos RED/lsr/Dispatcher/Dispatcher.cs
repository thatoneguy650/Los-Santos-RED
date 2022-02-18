using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Dispatcher
{
    private readonly IAgencies Agencies;
    private readonly IDispatchable Player;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IJurisdictions Jurisdictions;
    private readonly IZones Zones;
    private LEDispatcher LEDispatcher;
    private EMSDispatcher EMSDispatcher;
    private FireDispatcher FireDispatcher;
    private ZombieDispatcher ZombieDispatcher;
    private GangDispatcher GangDispatcher;
    private IWeapons Weapons;
    private INameProvideable Names;
    private List<RandomHeadData> RandomHeadList;

    private ICrimes Crimes;
    private IPedGroups PedGroups;
    private IGangs Gangs;
    private IGangTerritories GangTerritories;
    private IShopMenus ShopMenus;


    public Dispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, ICrimes crimes, IPedGroups pedGroups, IGangs gangs, IGangTerritories gangTerritories, IShopMenus shopMenus)
    {
        Player = player;
        World = world;
        Agencies = agencies;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        Jurisdictions = jurisdictions;
        Weapons = weapons;
        Names = names;
        Crimes = crimes;
        Gangs = gangs;
        PedGroups = pedGroups;
        GangTerritories = gangTerritories;
        ShopMenus = shopMenus;

    }
    public void Setup()
    {

            List<int> WhiteHairStyles_Male = new List<int>() { 2, 3, 4, 5, 7, 9, 10, 11, 12, 18, 19, 66 };
            List<int> BrownHairStyles_Male = new List<int>() { 2, 3, 4, 9, 10, 11, 12, 18, 19, 66 };
            List<int> AsianHairStyles_Male = new List<int>() { 2, 3, 4, 9, 10, 11, 12, 18, 19, 66 };
            List<int> BlackHairStyles_Male = new List<int>() { 0, 1, 8, 14, 24, 25, 30, 72 };

            List<int> WhiteHairColors_Male = new List<int>() { 0, 1, 2, 3, 4, 7, 8, 9, 10, 11, 12, 13 };
            List<int> BrownHairColors_Male = new List<int>() { 0, 1, 2, 3 };
            List<int> AsianHairColors_Male = new List<int>() { 0, 1, 2, 3 };
            List<int> BlackHairColors_Male = new List<int>() { 0, 1, 2, 3 };

            List<int> WhiteHairStyles_Female = new List<int>() { 1, 2, 4, 10, 11, 14, 15, 16, 17, 21, 48 };
            List<int> BrownHairStyles_Female = new List<int>() { 1, 2, 4, 10, 11, 14, 15, 16, 17, 21, 48 };
            List<int> AsianHairStyles_Female = new List<int>() { 1, 2, 4, 10, 11, 14, 15, 16, 17, 21, 48 };
            List<int> BlackHairStyles_Female = new List<int>() { 6, 2, 4, 10, 11, 20, 22, 25, 54, 58 };

            List<int> WhiteHairColors_Female = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            List<int> BrownHairColors_Female = new List<int>() { 0, 1, 2, 3 };
            List<int> AsianHairColors_Female = new List<int>() { 0, 1, 2, 3 };
            List<int> BlackHairColors_Female = new List<int>() { 0, 1, 2, 3 };

        RandomHeadList = new List<RandomHeadData>()
                    {
                        new RandomHeadData(0,"Benjamin",WhiteHairColors_Male,WhiteHairStyles_Male, true),//white male
                        new RandomHeadData(1,"Daniel",WhiteHairColors_Male,WhiteHairStyles_Male, true),//white male
                        new RandomHeadData(4,"Andrew",WhiteHairColors_Male,WhiteHairStyles_Male, true),//white male
                        new RandomHeadData(5,"Juan",WhiteHairColors_Male,WhiteHairStyles_Male, true),//white male
                        new RandomHeadData(12,"Diego",WhiteHairColors_Male,WhiteHairStyles_Male, true),//white male
                        new RandomHeadData(13,"Adrian",WhiteHairColors_Male,WhiteHairStyles_Male, true),//white male
                        new RandomHeadData(42,"Claude",WhiteHairColors_Male,WhiteHairStyles_Male, true),//white male
                        new RandomHeadData(43,"Niko",WhiteHairColors_Male,WhiteHairStyles_Male, true),//white male
                        new RandomHeadData(44,"John",WhiteHairColors_Male,WhiteHairStyles_Male, true),///white male
                        new RandomHeadData(2,"Joshua",BlackHairColors_Male,BlackHairStyles_Male, true),//black male
                        new RandomHeadData(3,"Noah",BlackHairColors_Male,BlackHairStyles_Male, true),//black male
                        new RandomHeadData(15,"Michael",BlackHairColors_Male,BlackHairStyles_Male, true),//black male
                        new RandomHeadData(19,"Samuel",BlackHairColors_Male,BlackHairStyles_Male, true),//black male
                        new RandomHeadData(8,"Evan",BrownHairColors_Male,BrownHairStyles_Male, true),//brown male
                        new RandomHeadData(9,"Ethan",BrownHairColors_Male,BrownHairStyles_Male, true),//brown male
                        new RandomHeadData(10,"Vincent",BrownHairColors_Male,BrownHairStyles_Male, true),//brown male
                        new RandomHeadData(11,"Angel",BrownHairColors_Male,BrownHairStyles_Male, true),//brown male
                        new RandomHeadData(16,"Santiago",BrownHairColors_Male,BrownHairStyles_Male, true),//brown male
                        new RandomHeadData(20,"Anthony",BrownHairColors_Male,BrownHairStyles_Male, true),//brown male
                        new RandomHeadData(7,"Isaac",AsianHairColors_Male,AsianHairStyles_Male, true),//asian male
                        new RandomHeadData(17,"Kevin",AsianHairColors_Male,AsianHairStyles_Male, true),//asian male
                        new RandomHeadData(18,"Louis",AsianHairColors_Male,AsianHairStyles_Male, true),//asian male            




                        new RandomHeadData(6,"Alex",AsianHairColors_Female,AsianHairStyles_Female,false),//asian female
                        new RandomHeadData(27,"Zoe",AsianHairColors_Female,AsianHairStyles_Female,false),//asian female
                        new RandomHeadData(28,"Ava",AsianHairColors_Female,AsianHairStyles_Female,false),//asian female
                        new RandomHeadData(39,"Elizabeth",AsianHairColors_Female,AsianHairStyles_Female,false),//asian female
                        new RandomHeadData(33,"Nicole",WhiteHairColors_Female,WhiteHairStyles_Female,false),//white female
                        new RandomHeadData(34,"Ashley",WhiteHairColors_Female,WhiteHairStyles_Female,false),//white female
                        new RandomHeadData(21,"Hannah",WhiteHairColors_Female,WhiteHairStyles_Female,false),//white female
                        new RandomHeadData(22,"Audrey",WhiteHairColors_Female,WhiteHairStyles_Female,false),//white female
                        new RandomHeadData(40,"Charlotte",WhiteHairColors_Female,WhiteHairStyles_Female,false),//white female
                        new RandomHeadData(45,"Misty",WhiteHairColors_Female,WhiteHairStyles_Female,false),//white female
                        new RandomHeadData(14,"Gabriel",BlackHairColors_Female,BlackHairStyles_Female,false),//black female
                        new RandomHeadData(23,"Jasmine",BlackHairColors_Female,BlackHairStyles_Female,false),//black female
                        new RandomHeadData(24,"Giselle",BlackHairColors_Female,BlackHairStyles_Female,false),//black female
                        new RandomHeadData(35,"Grace",BlackHairColors_Female,BlackHairStyles_Female,false),//black female
                        new RandomHeadData(36,"Brianna",BlackHairColors_Female,BlackHairStyles_Female,false),//black female
                        new RandomHeadData(25,"Amelia",BrownHairColors_Female,BrownHairStyles_Female,false),//brown female
                        new RandomHeadData(26,"Isabella",BrownHairColors_Female,BrownHairStyles_Female,false),//brown female
                        new RandomHeadData(29,"Camila",BrownHairColors_Female,BrownHairStyles_Female,false),//brown female
                        new RandomHeadData(30,"Violet",BrownHairColors_Female,BrownHairStyles_Female,false),//brown female
                        new RandomHeadData(31,"Sophia",BrownHairColors_Female,BrownHairStyles_Female,false),//brown female
                        new RandomHeadData(32,"Evelyn",BrownHairColors_Female,BrownHairStyles_Female,false),//brown female
                        new RandomHeadData(37,"Natalie",BrownHairColors_Female,BrownHairStyles_Female,false),//brown female
                        new RandomHeadData(38,"Olivia",BrownHairColors_Female,BrownHairStyles_Female,false),//brown female     
                        new RandomHeadData(41,"Emma",BrownHairColors_Female,BrownHairStyles_Female,false),//brown female            
                    };


        LEDispatcher = new LEDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, RandomHeadList);
        EMSDispatcher = new EMSDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, RandomHeadList);
        FireDispatcher = new FireDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, RandomHeadList);
        ZombieDispatcher = new ZombieDispatcher(World, Player, Settings, Streets, Zones, Jurisdictions, Weapons, Names, Crimes, RandomHeadList);
        GangDispatcher = new GangDispatcher(World, Player, Gangs, Settings, Streets, Zones, GangTerritories, Weapons, Names, PedGroups, Crimes, ShopMenus, RandomHeadList);
    }
    public void Dispatch()
    {
        if (!LEDispatcher.Dispatch())
        {
            if (!EMSDispatcher.Dispatch())
            {
                if(!FireDispatcher.Dispatch())
                {
                    GangDispatcher.Dispatch();
                }
            }
        }
        if(World.IsZombieApocalypse)
        {
            GameFiber.Yield();
            ZombieDispatcher.Dispatch();
        }
        //GangDispatcher.Dispatch();
    }
    public void Recall()
    {
        LEDispatcher.Recall();
        EMSDispatcher.Recall();
        FireDispatcher.Recall();
        if (World.IsZombieApocalypse)
        {
            GameFiber.Yield();
            ZombieDispatcher.Recall();
        }
        GangDispatcher.Recall();
    }
    public void Dispose()
    {
        LEDispatcher.Dispose();
    }
    public void SpawnRoadblock()
    {
        LEDispatcher.SpawnRoadblock();
    }
    public void RemoveRoadblock()
    {
        LEDispatcher.RemoveRoadblock();
    }
    public void DebugSpawnCop()
    {
        LEDispatcher.DebugSpawnCop();
    }
    public void DebugSpawnGang()
    {
        GangDispatcher.ForceDispatch();
    }
}

