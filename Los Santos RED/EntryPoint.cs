using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("Los Santos RED", Description = "Total Conversion", Author = "Greskrendtregk")]
public static class EntryPoint
{
    private static Ped Cop;
    public static ModController ModController { get; set; }
    public static void Main()
    {
        while (Game.IsLoading)
        {
            GameFiber.Yield();
        }  
        Loop();
        //TempLoop();
    }
    private static void Loop()
    {
        Game.DisplayNotification("~s~Los Santos ~r~RED ~s~v0.1 ~n~By ~g~Greskrendtregk ~n~~s~Press F10 to Start");
        while (true)
        {
            if (Game.IsKeyDown(Keys.F10))
            {
                if (ModController == null || !ModController.IsRunning)
                {
                    ModController = new ModController();
                    ModController.Start();
                }
            }
            GameFiber.Yield();
        }
    }
    private static void TempLoop()
    {
        Game.DisplayNotification("Temp Crap Started");
        NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", 5);
        while (true)
        {
            if (Game.IsKeyDown(Keys.NumPad0))
            {
                Game.LocalPlayer.WantedLevel = 0;
                Game.Console.Print($"Wanted Level Set to 0");
            }
            if (Game.IsKeyDown(Keys.NumPad1))
            {
                Game.LocalPlayer.WantedLevel = 2;
                Game.LocalPlayer.IsInvincible = true;
                Game.Console.Print($"Wanted Level Set to 2");
            }
            if (Game.IsKeyDown(Keys.NumPad2) && !Cop.Exists())
            {
                Vector3 Pos = Game.LocalPlayer.Character.GetOffsetPositionFront(3f);
                Cop = new Ped("s_m_y_cop_01", Pos, Game.LocalPlayer.Character.Heading);
                //Cop = NativeFunction.Natives.CREATE_PED<Ped>(6, Game.GetHashKey("s_m_y_cop_01"), Pos.X, Pos.Y, Pos.Z, Game.LocalPlayer.Character.Heading, false, false);
                Cop.RelationshipGroup = RelationshipGroup.Cop;
                Game.Console.Print($"Cop Spawned");
            }
            if (Game.IsKeyDown(Keys.NumPad3) && Cop.Exists())
            {
                if (Cop.Exists())
                {
                    Cop.Delete();
                    Game.Console.Print($"Cop Deleted");
                }
            }
            if(Game.IsKeyDown(Keys.NumPad7))
            {
                Cop.Tasks.Wander();

                
            }
            if (Game.IsKeyDown(Keys.NumPad8))
            {
                ChangeModel(Game.LocalPlayer.Character.Model.Name);
                PedVariation MyVariation = GetPedVariation(Game.LocalPlayer.Character);
                MyVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);
            }
            if (Game.IsKeyDown(Keys.NumPad9))
            {
                break;
            }
            GameFiber.Yield();
        }
        if (Cop.Exists())
        {
            Cop.Delete();
            Game.Console.Print($"Cop Deleted");
        }
        Game.LocalPlayer.IsInvincible = false;
    }

    private static void ChangeModel(string ModelRequested)
    {
        Model characterModel = new Model(ModelRequested);
        characterModel.LoadAndWait();
        characterModel.LoadCollisionAndWait();
        Game.LocalPlayer.Model = characterModel;
        Game.LocalPlayer.Character.IsCollisionEnabled = true;
    }
    private static  PedVariation GetPedVariation(Ped myPed)
    {
        try
        {
            PedVariation myPedVariation = new PedVariation
            {
                MyPedComponents = new List<PedComponent>(),
                MyPedProps = new List<PedPropComponent>()
            };
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                myPedVariation.MyPedComponents.Add(new PedComponent(ComponentNumber, NativeFunction.Natives.GET_PED_DRAWABLE_VARIATION<int>(myPed, ComponentNumber), NativeFunction.Natives.GET_PED_TEXTURE_VARIATION<int>(myPed, ComponentNumber), NativeFunction.Natives.GET_PED_PALETTE_VARIATION<int>(myPed, ComponentNumber)));
            }
            for (int PropNumber = 0; PropNumber < 8; PropNumber++)
            {
                myPedVariation.MyPedProps.Add(new PedPropComponent(PropNumber, NativeFunction.Natives.GET_PED_PROP_INDEX<int>(myPed, PropNumber), NativeFunction.Natives.GET_PED_PROP_TEXTURE_INDEX<int>(myPed, PropNumber)));
            }
            return myPedVariation;
        }
        catch (Exception e)
        {
            //Game.Console.Print("CopyPedComponentVariation! CopyPedComponentVariation Error; " + e.Message);
            return null;
        }
    }

}