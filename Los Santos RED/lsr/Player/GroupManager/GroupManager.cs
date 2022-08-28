using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GroupManager
{
    private IGroupManageable Player;
    private ISettingsProvideable Settings;
    private IEntityProvideable World;
    private IGangs Gangs;

    public GroupManager(IGroupManageable player, ISettingsProvideable settings, IEntityProvideable world, IGangs gangs)
    {
        Player = player;
        Settings = settings;
        World = world;
        Gangs = gangs;
    }
    public void Setup()
    {

    }
    public void Update()
    {

    }
    public void Dispose()
    {

    }
}

