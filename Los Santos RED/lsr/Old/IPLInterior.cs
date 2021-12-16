//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class IPLInterior
//{
//    public IPLInterior()
//    {

//    }
//    public IPLInterior(string name, int iD, List<string> requestIPLs, List<string> removeIPLs, List<string> interiorSets)
//    {
//        Name = name;
//        ID = iD;
//        RequestIPLs = requestIPLs;
//        RemoveIPLs = removeIPLs;
//        InteriorSets = interiorSets;
//    }
//    public string Name { get; private set; }
//    public int ID { get; private set; }
//    public List<string> RequestIPLs { get; private set; }
//    public List<string> RemoveIPLs { get; private set; }
//    public List<string> InteriorSets { get; private set; }
//    public bool IsActive { get; set; } = false;
//    public void Load()
//    {
//        foreach (string iplName in RequestIPLs)
//        {
//            NativeFunction.Natives.REQUEST_IPL(iplName);
//        }
//        foreach (string iplName in RemoveIPLs)
//        {
//            NativeFunction.Natives.REMOVE_IPL(iplName);
//        }
//        foreach (string interiorSet in InteriorSets)
//        {
//            NativeFunction.Natives.ACTIVATE_INTERIOR_ENTITY_SET(ID, interiorSet);
//        }
//        NativeFunction.Natives.REFRESH_INTERIOR(ID);
//        IsActive = true;
//    }
//    public void Unload()
//    {
//        foreach(string iplName in RequestIPLs)
//        {
//            NativeFunction.Natives.REMOVE_IPL(iplName);
//        }
//        foreach (string iplName in RemoveIPLs)
//        {
//            NativeFunction.Natives.REQUEST_IPL(iplName);
//        }
//        foreach (string interiorSet in InteriorSets)
//        {
//            NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(ID, interiorSet);
//        }
//        NativeFunction.Natives.REFRESH_INTERIOR(ID);
//        IsActive = false;
//    }

//}

