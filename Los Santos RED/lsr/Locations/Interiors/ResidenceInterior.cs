using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


public class ResidenceInterior : Interior
{
    protected Residence residence;
    public Residence Residence => residence;
    //public List<ResidenceInteriorInteract> ResidenceInteriorInteracts = new List<ResidenceInteriorInteract>();
    public ResidenceInterior()
    {
       
    }

    public ResidenceInterior(int iD, string name) : base(iD, name)
    {

    }
    public void SetResidence(Residence newResidence)
    {
        residence = newResidence;
        foreach (InteriorInteract test in InteractPoints)
        {
            test.
        }
    }
}

