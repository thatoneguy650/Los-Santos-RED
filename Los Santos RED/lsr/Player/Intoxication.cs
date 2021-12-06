using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Intoxication
{
    private IIntoxicatable Player;
    private List<Intoxicator> CurrentIntoxicators = new List<Intoxicator>();
    private Intoxicator PrimaryIntoxicator;
    public Intoxication(IIntoxicatable player)
    {
        Player = player;
    }

    public void Update()
    {
        //PrimaryIntoxicator = CurrentIntoxicators.OrderByDescending (x=>x.)
    }
}

