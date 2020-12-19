using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class Debug
{
    public List<GameFiber> GameFibers = new List<GameFiber>();
    public void WriteToLog(String ProcedureString, String TextToLog)
    {
        if (ProcedureString == "Error")
        {
            Game.Console.Print("Los Santos RED has crashed and needs to be restarted");
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
        }
        string Message = DateTime.Now.ToString("HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog;
        Game.Console.Print(Message);
    }
    public void DebugLoop()
    {
        if (Game.IsKeyDown(Keys.NumPad0))
        {
            DebugNumpad0();
        }
        if (Game.IsKeyDown(Keys.NumPad1))
        {
            DebugNumpad1();
        }
        if (Game.IsKeyDown(Keys.NumPad2))
        {
            DebugNumpad2();
        }
        if (Game.IsKeyDown(Keys.NumPad3))
        {
            DebugNumpad3();
        }
        if (Game.IsKeyDown(Keys.NumPad4))
        {
            DebugNumpad4();
        }
        if (Game.IsKeyDown(Keys.NumPad5))
        {
            DebugNumpad5();
        }
        if (Game.IsKeyDown(Keys.NumPad6))
        {
            DebugNumpad6();
        }
        if (Game.IsKeyDown(Keys.NumPad7))
        {
            DebugNumpad7();
        }
        if (Game.IsKeyDown(Keys.NumPad8))
        {
            DebugNumpad8();
        }
        if (Game.IsKeyDown(Keys.NumPad9))
        {
            DebugNumpad9();
        }

        //if (false)
        //{
        //    foreach (Cop MyCop in Mod.World.Pedestrians.Police.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive))
        //    {
        //        Color ToShow = Color.Black;
        //        TaskStatus CurrentOne = MyCop.Pedestrian.Tasks.CurrentTaskStatus;
        //        if (CurrentOne == TaskStatus.InProgress)
        //            ToShow = Color.Green;
        //        else if (CurrentOne == TaskStatus.Interrupted)
        //            ToShow = Color.Red;
        //        else if (CurrentOne == TaskStatus.NoTask)
        //            ToShow = Color.White;
        //        else if (CurrentOne == TaskStatus.None)
        //            ToShow = Color.Blue;
        //        else if (CurrentOne == TaskStatus.NoTask)
        //            ToShow = Color.Purple;
        //        else if (CurrentOne == TaskStatus.Preparing)
        //            ToShow = Color.Yellow;

        //            Rage.Debug.DrawArrowDebug(new Vector3(MyCop.Pedestrian.Position.X, MyCop.Pedestrian.Position.Y, MyCop.Pedestrian.Position.Z + 2f), Vector3.Zero, Rotator.Zero, 1f, ToShow);
        //    }
        //}
    }
    private void DebugNumpad0()
    {
        DebugNonInvincible();
    }
    private void DebugNumpad1()
    {
        DebugInvincible();
    }
    private void DebugNumpad2()
    {
        int Toassign = Mod.Player.WantedLevel;
        if (Toassign == 7)
            return;
        Toassign++;
        Mod.Player.CurrentPoliceResponse.SetWantedLevel(Toassign, "Debug", true);

    }
    private void DebugNumpad3()
    {
        Mod.Player.CurrentPoliceResponse.SetWantedLevel(0, "Debug", true);
    }
    private void DebugNumpad4()
    {
        string GET_PLAYER_RADIO_STATION_NAME = "";
        unsafe
        {
            IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_PLAYER_RADIO_STATION_NAME");
            GET_PLAYER_RADIO_STATION_NAME = Marshal.PtrToStringAnsi(ptr);
        }


        int GET_PLAYER_RADIO_STATION_INDEX = NativeFunction.CallByName<int>("GET_PLAYER_RADIO_STATION_INDEX");
        

        WriteToLog("Debugging", string.Format("GET_PLAYER_RADIO_STATION_NAME: {0}, GET_PLAYER_RADIO_STATION_INDEX: {1}", GET_PLAYER_RADIO_STATION_NAME, GET_PLAYER_RADIO_STATION_INDEX));
        NativeFunction.CallByName<bool>("SET_RADIO_TO_STATION_NAME", "RADIO_19_USER");
    }
    private void DebugNumpad5()
    {
        //if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null)
        //{
        //    int Group = NativeFunction.CallByName<int>("GET_WEAPONTYPE_GROUP", (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
        //    int Slot = NativeFunction.CallByName<int>("GET_WEAPONTYPE_SLOT", (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
        //    WriteToLog("Debugging", string.Format("Hash: {0} GET_WEAPONTYPE_GROUP: {1} GET_WEAPONTYPE_SLOT: {2}", Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, Group, Slot));
        //}
        //string GET_RADIO_STATION_NAME = "";
        //unsafe
        //{
        //    IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_RADIO_STATION_NAME");
        //    GET_RADIO_STATION_NAME = Marshal.PtrToStringAnsi(ptr);
        //}


        //NativeFunction.CallByName<bool>("SET_RADIO_TO_STATION_INDEX", 1);
        //WriteToLog("Debugging", string.Format("GET_RADIO_STATION_NAME: {0}", GET_RADIO_STATION_NAME));


        //WriteToLog("Debugging", string.Format("GET_TIMECYCLE_MODIFIER_INDEX: {0}", NativeFunction.CallByName<int>("GET_TIMECYCLE_MODIFIER_INDEX")));
        ////722 drunk

        ////Vehicle[] Vehicles = Array.ConvertAll(Rage.World.GetEntities(Game.LocalPlayer.Character.Position, 450f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle && x.Exists()).ToArray(), x => (Vehicle)x);//250
        //WriteToLog("Debugging", string.Format("PED_FLAG_DRUNK: {0}", NativeFunction.CallByName<bool>("GET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, 1)));

        //WriteToLog("Debugging", string.Format("Mod.World.Pedestrians.Civilians.Any(x => x.CanSeePlayer))): {0}", Mod.World.Pedestrians.Civilians.Any(x => x.CanSeePlayer)));
        //WriteToLog("Debugging", string.Format("Game.LocalPlayer.Character.GetBoneOrientation(0): {0}", Game.LocalPlayer.Character.GetBoneOrientation(0)));

        


    }
    private void DebugNumpad6()
    {
        if (Mod.Player.CurrentVehicle != null && Mod.Player.CurrentVehicle.Vehicle.Exists())
        {
            Colorbullshit();
            Colorbullshit2();
        }
    }       
    private void Colorbullshit()
    {
        //Color Color1 = Extensions.GetBaseColor1(Mod.Player.CurrentVehicle.Vehicle.PrimaryColor);
        //Color Color2 = Extensions.GetBaseColor2(Mod.Player.CurrentVehicle.Vehicle.PrimaryColor);
        //Color Color3 = Extensions.GetBaseColor3(Mod.Player.CurrentVehicle.Vehicle.PrimaryColor);
        //WriteToLog("Debugging", string.Format("ColorBS: {0} Match1: {1} Match2: {2} Match3: {3}", Mod.Player.CurrentVehicle.Vehicle.PrimaryColor.ToString(), Color1.ToString(), Color2.ToString(), Color3.ToString()));
    }
    private void Colorbullshit2()
    {
        Color c = Mod.Player.CurrentVehicle.Vehicle.PrimaryColor;
        float targetHue = c.GetHue();
        float targetSat = c.GetSaturation();
        float targetBri = c.GetBrightness();

        Color closestColor = Color.Red;
        double smallestDiff = double.MaxValue;

        List<Color> BaseColorList = new List<Color>
        {
            Color.Red,
            Color.Aqua,
            Color.Beige,
            Color.Black,
            Color.Blue,
            Color.Brown,
            Color.DarkBlue,
            Color.DarkGreen,
            Color.DarkGray,
            Color.DarkOrange,
            Color.DarkRed,
            Color.Gold,
            Color.Green,
            Color.Gray,
            Color.LightBlue,
            Color.Maroon,
            Color.Orange,
            Color.Pink,
            Color.Purple,
            Color.Silver,
            Color.White,
            Color.Yellow
        };


        foreach (Color currentColor in BaseColorList)
        {

            float currentHue = currentColor.GetHue();
            float currentSat = currentColor.GetSaturation();
            float currentBri = currentColor.GetBrightness();

            double currentDiff = Math.Pow(targetHue - currentHue, 2) + Math.Pow(targetSat - currentSat, 2) + Math.Pow(targetBri - currentBri, 2);

            if (currentDiff < smallestDiff)
            {
                smallestDiff = currentDiff;
                closestColor = currentColor;
            }
        }
        WriteToLog("Debugging", string.Format("ColorBS2: {0} ", closestColor.ToString()));
    }
    private void DebugNumpad7()
    {
        WriteToLog("Debugging", Mod.World.Time.CurrentTime);

        if (Mod.Player.CurrentVehicle != null)
        {
            WriteToLog("Debugging", string.Format("CurrentVehicle  IsStolen:{0} WasReportedStolen:{1} NeedsToBeReportedStolen:{2}", Mod.Player.CurrentVehicle.IsStolen, Mod.Player.CurrentVehicle.WasReportedStolen, Mod.Player.CurrentVehicle.NeedsToBeReportedStolen));
            WriteToLog("Debugging", string.Format("CurrentVehicle  CarPlate.IsWanted:{0} OriginalLicensePlate.IsWanted: {1} ColorMatchesDescription:{2} CopsRecognizeAsStolen: {3}", Mod.Player.CurrentVehicle.CarPlate.IsWanted, Mod.Player.CurrentVehicle.OriginalLicensePlate.IsWanted, Mod.Player.CurrentVehicle.ColorMatchesDescription,Mod.Player.CurrentVehicle.CopsRecognizeAsStolen));
        }

        Mod.World.Tasking.PrintActivities();
    }
    public void DebugNumpad8()
    {

        try
        {
            //CameraManager.DebugAbort();

            WriteToLog("Debugging", "--------------------------------");
            WriteToLog("Debugging", "--------Police Status-----------");

            foreach (Cop Cop in Mod.World.Pedestrians.Police.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && x.AssignedAgency != null).OrderBy(x => x.DistanceToPlayer))
            {

                WriteToLog("Debugging", string.Format("Cop {0,-20},  Model {1,-20}, Agency {2,-20},Distance {3,-20},Relationship1 {4,-20},Relationship2 {5,-20}", 
                    Cop.Pedestrian.Handle
                    ,Cop.Pedestrian.Model.Name
                    ,Cop.AssignedAgency.Initials
                    ,Cop.DistanceToPlayer
                    ,NativeFunction.CallByName<int>("GET_RELATIONSHIP_BETWEEN_PEDS",Cop.Pedestrian,Game.LocalPlayer.Character)
                    ,NativeFunction.CallByName<int>("GET_RELATIONSHIP_BETWEEN_PEDS", Game.LocalPlayer.Character, Cop.Pedestrian)


                    ));


            }
            WriteToLog("Debugging", string.Format("PoliceInInvestigationMode: {0}", Mod.Player.Investigations.InInvestigationMode));
            WriteToLog("Debugging", string.Format("InvestigationPosition: {0}", Mod.Player.Investigations.InvestigationPosition));
            WriteToLog("Debugging", string.Format("InvestigationDistance: {0}", Mod.Player.Investigations.InvestigationDistance));
            WriteToLog("Debugging", string.Format("ActiveDistance: {0}", Mod.World.Police.ActiveDistance));
            WriteToLog("Debugging", string.Format("AnyNear Investigation Position: {0}", Mod.World.Pedestrians.Police.Any(x => x.Pedestrian.DistanceTo2D(Mod.Player.Investigations.InvestigationPosition) <= Mod.Player.Investigations.InvestigationDistance)));
            WriteToLog("Debugging", string.Format("CurrentPoliceStateString: {0}", Mod.Player.CurrentPoliceResponse.CurrentPoliceStateString));
            


        }
        catch(Exception e)
        {
            WriteToLog("Debugging error", e.Message + e.StackTrace);
        }
    }
    private void DebugNumpad9()
    {

        WriteToLog("Debugging", "Pressed Num9");
        Mod.Dispose();
        Game.LocalPlayer.WantedLevel = 0;
        Game.TimeScale = 1f;
        NativeFunction.Natives.xB4EDDC19532BFB85();
        Game.DisplayNotification("Instant Action Deactivated");
    }
    private void DebugNonInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = false;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        WriteToLog("KeyDown", "You are NOT invicible");
    }
    private void DebugInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = true;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        WriteToLog("KeyDown", "You are invicible");
    }
}

