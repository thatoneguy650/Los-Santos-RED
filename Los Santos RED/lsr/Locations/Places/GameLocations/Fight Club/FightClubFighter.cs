using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FightClubFighter
{
    public virtual bool IsPlayer { get;set; }
    protected FightClubArena FightClubArena;
    protected IFightClubable Player;
    protected bool IsOutsideRing;
    protected uint GameTimeLeftRing;
    public bool HasLost { get; set; }

    public uint GameTimeOutsideRing => GameTimeLeftRing == 0 ? 0 : Game.GameTime - GameTimeLeftRing;
    public virtual void Setup()
    {
        
    }
    public virtual void StartFight()
    {

    }

    public virtual void Update()
    {
       
    }

    public virtual void Dispose()
    {
        
    }
}

