using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class Interior
{
    public Interior()
    {

    }
    public Interior(int iD, string name, List<string> requestIPLs)
    {
        Name = name;
        ID = iD;
        RequestIPLs = requestIPLs;
    }
    public Interior(int iD, string name, List<string> requestIPLs, List<string> removeIPLs)
    {
        Name = name;
        ID = iD;
        RequestIPLs = requestIPLs;
        RemoveIPLs = removeIPLs;
    }
    public Interior(int iD, string name, List<string> requestIPLs, List<string> removeIPLs, List<string> interiorSets)
    {
        Name = name;
        ID = iD;
        RequestIPLs = requestIPLs;
        RemoveIPLs = removeIPLs;
        InteriorSets = interiorSets;
    }
    public Interior(int iD, string name, List<string> requestIPLs, List<string> removeIPLs, List<InteriorDoor> interiorDoors)
    {
        Name = name;
        ID = iD;
        RequestIPLs = requestIPLs;
        RemoveIPLs = removeIPLs;
        Doors = interiorDoors;
    }
    public Interior(int iD, string name)
    {
        ID = iD;
        Name = name;
    }
    public int ID { get; set; }
    public string Name { get; set; }
    public bool IsMPOnly { get; set; } = false;
    public bool IsSPOnly { get; set; } = false;
    public bool IsTeleportEntry { get; set; } = false;
    public Vector3 DisabledInteriorCoords { get; set; } = Vector3.Zero;
    public List<InteriorDoor> Doors { get; set; } = new List<InteriorDoor>();
    public List<string> RequestIPLs { get; set; } = new List<string>();
    public List<string> RemoveIPLs { get; set; } = new List<string>();
    public List<string> InteriorSets { get; set; } = new List<string>();
    //public bool IsActive { get; set; } = false;
    public Vector3 InteriorEgressPosition { get; set; }
    public float InteriorEgressHeading { get; set; }
    public void Load()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                foreach (string iplName in RequestIPLs)
                {
                    NativeFunction.Natives.REQUEST_IPL(iplName);
                    GameFiber.Yield();
                }
                foreach (string iplName in RemoveIPLs)
                {
                    NativeFunction.Natives.REMOVE_IPL(iplName);
                    GameFiber.Yield();
                }
                foreach (string interiorSet in InteriorSets)
                {
                    NativeFunction.Natives.ACTIVATE_INTERIOR_ENTITY_SET(ID, interiorSet);
                    GameFiber.Yield();
                }

                foreach (InteriorDoor door in Doors)
                {
                    NativeFunction.Natives.x9B12F9A24FABEDB0(door.ModelHash, door.Position.X, door.Position.Y, door.Position.Z, false, door.Rotation.Pitch, door.Rotation.Roll, door.Rotation.Yaw);
                    door.IsLocked = false;
                    GameFiber.Yield();
                }
                if (DisabledInteriorCoords != Vector3.Zero)
                {
                    if (ID < 0)
                    {
                        ID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z);
                    }
                    NativeFunction.Natives.DISABLE_INTERIOR(ID, false);
                    NativeFunction.Natives.CAP_INTERIOR(ID, false);
                    GameFiber.Yield();
                    //NativeFunction.Natives.DISABLE_INTERIOR(NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z), false);
                    //NativeFunction.Natives.CAP_INTERIOR(NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z), false);
                }
                NativeFunction.Natives.REFRESH_INTERIOR(ID);
               // IsActive = true;
                GameFiber.Yield();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
            //EntryPoint.WriteToConsole($"Interior LOADED {Name} {ID}");
        }, "Load Interior");
    }
    public void Unload()
    {
        GameFiber.StartNew(delegate
            {
                try
                {
                    foreach (string iplName in RequestIPLs)
                    {
                        NativeFunction.Natives.REMOVE_IPL(iplName);
                        GameFiber.Yield();
                    }
                    foreach (string iplName in RemoveIPLs)
                    {
                        NativeFunction.Natives.REQUEST_IPL(iplName);
                        GameFiber.Yield();
                    }
                    foreach (string interiorSet in InteriorSets)
                    {
                        NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(ID, interiorSet);
                        GameFiber.Yield();
                    }
                    foreach (InteriorDoor door in Doors)
                    {
                        NativeFunction.Natives.x9B12F9A24FABEDB0(door.ModelHash, door.Position.X, door.Position.Y, door.Position.Z, true, door.Rotation.Pitch, door.Rotation.Roll, door.Rotation.Yaw);
                        door.IsLocked = true;
                        GameFiber.Yield();
                    }
                    if (DisabledInteriorCoords != Vector3.Zero)
                    {
                        if (ID < 0)
                        {
                            ID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z);
                        }

                        NativeFunction.Natives.DISABLE_INTERIOR(ID, true);
                        NativeFunction.Natives.CAP_INTERIOR(ID, true);
                        GameFiber.Yield();
                        //NativeFunction.Natives.DISABLE_INTERIOR(NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z), true);
                        //NativeFunction.Natives.CAP_INTERIOR(NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z), true);
                    }
                    NativeFunction.Natives.REFRESH_INTERIOR(ID);
                   // IsActive = false;
                    GameFiber.Yield();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
                //EntryPoint.WriteToConsole($"Interior UNLOADED {Name} {ID}");
            }, "Unload Interiors");
    }


}
