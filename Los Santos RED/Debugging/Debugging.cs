using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public static class Debugging
{
    public static SlidingBuffer<string> LogMessages = new SlidingBuffer<string>(10);
    public static bool ShowCopTaskStatus;
    public static List<GameFiber> GameFibers;

    public static bool IsRunning { get; set; }
    public static bool IsTesting { get;  }
    public static int NumberPlateIndexSelected { get; private set; }

    public static void Initialize()
    {
        ShowCopTaskStatus = false;
        GameFibers = new List<GameFiber>();
        IsRunning = true;
        LogMessages = new SlidingBuffer<string>(10);
        MainLoop();
    }
    public static void MainLoop()
    {
        var Stopwatch = new Stopwatch();
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    Stopwatch.Start();
                    DebugLoop();
                    Stopwatch.Stop();
                    if (Stopwatch.ElapsedMilliseconds >= 16)
                        WriteToLog("DebuggingTick", string.Format("Tick took {0} ms", Stopwatch.ElapsedMilliseconds));
                    Stopwatch.Reset();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                Dispose();
                WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private static void DebugLoop()
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

        if (SettingsManager.MySettings.Police.DebugShowPoliceTask)
        {
            foreach (Cop MyCop in PedManager.Cops.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive))
            {
                Color ToShow = Color.Black;
                TaskStatus CurrentOne = MyCop.Pedestrian.Tasks.CurrentTaskStatus;
                if (CurrentOne == TaskStatus.InProgress)
                    ToShow = Color.Green;
                else if (CurrentOne == TaskStatus.Interrupted)
                    ToShow = Color.Red;
                else if (CurrentOne == TaskStatus.NoTask)
                    ToShow = Color.White;
                else if (CurrentOne == TaskStatus.None)
                    ToShow = Color.Blue;
                else if (CurrentOne == TaskStatus.NoTask)
                    ToShow = Color.Purple;
                else if (CurrentOne == TaskStatus.Preparing)
                    ToShow = Color.Yellow;

                    Rage.Debug.DrawArrowDebug(new Vector3(MyCop.Pedestrian.Position.X, MyCop.Pedestrian.Position.Y, MyCop.Pedestrian.Position.Z + 2f), Vector3.Zero, Rotator.Zero, 1f, ToShow);
            }
        }
    }
    private static void DebugNonInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = false;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        WriteToLog("KeyDown", "You are NOT invicible");
    }
    private static void DebugInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = true;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        WriteToLog("KeyDown", "You are invicible");
    }
    private static void DebugCopReset()
    {
        WantedLevelManager.Reset();
        Game.LocalPlayer.WantedLevel = 0;
        PedManager.ClearPoliceCompletely();
        Game.TimeScale = 1f;
        PlayerStateManager.ResetState(true);
        NativeFunction.Natives.xB4EDDC19532BFB85();
    }
    private static void DebugNumpad0()
    {
        DebugNonInvincible();
    }
    private static void DebugNumpad1()
    {
        DebugInvincible();
    }
    private static void DebugNumpad2()
    {
        int Toassign = PlayerStateManager.WantedLevel;
        if (Toassign == 7)
            return;
        Toassign++;
        WantedLevelManager.SetWantedLevel(Toassign, "Debug", true);

    }
    private static void DebugNumpad3()
    {
        WantedLevelManager.SetWantedLevel(0, "Debug", true);
    }
    private static void DebugNumpad4()
    {

        if (NumberPlateIndexSelected <= 47)
        {
            NumberPlateIndexSelected++;
        }
        else
        {
            NumberPlateIndexSelected = 5;
        }
        NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Game.LocalPlayer.Character.CurrentVehicle, NumberPlateIndexSelected);



        //PoliceSpawning.SpawnGTACop(Agencies.GetAllSpawnableAgencies(Game.LocalPlayer.Character.GetOffsetPositionFront(5f)).PickRandom(), Game.LocalPlayer.Character.GetOffsetPositionFront(5f), Game.LocalPlayer.Character.Heading, null, true);
        //BribePoliceAnimation(0);
    }
    private static void BribePoliceAnimation(int Amount)//temp public
    {
        GameFiber.StartNew(delegate
        {
            GameFiber.Wait(1000);
            Cop CopToBribe = PedManager.Cops.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
            NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
            Game.TimeScale = 1.0f;


            Debugging.WriteToLog("BribePoliceAnimation", "Started");

            CopToBribe.CanBeTasked = false;

            SurrenderManager.UnSetArrestedAnimation(Game.LocalPlayer.Character);

            //while (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Game.LocalPlayer.Character, "random@arrests", "kneeling_arrest_escape", 1))
            //    GameFiber.Wait(250);

            GameFiber.Wait(2000);

            if (!CopToBribe.Pedestrian.Exists())
                return;

            CopToBribe.Pedestrian.BlockPermanentEvents = true;
            //CopToBribe.Pedestrian.IsPositionFrozen = true;



            Vector3 CopPosition;// = GetPoint(Game.LocalPlayer.Character.Position, CopToBribe.Pedestrian.Position, 2f);
            Vector3 PlayerPosition;// = GetPoint(CopToBribe.Pedestrian.Position, Game.LocalPlayer.Character.Position, 2f);


            Vector3 redPos = Game.LocalPlayer.Character.Position;
            Vector3 bluePos = CopToBribe.Pedestrian.Position;
            Vector3 dir = bluePos - redPos;
            float distance = Vector3.Distance(redPos, bluePos);
            Vector3 oneThird = redPos + dir * (distance - 0.5f);



            PlayerPosition = oneThird;




            Vector3 redPos2 = CopToBribe.Pedestrian.Position;
            Vector3 bluePos2 = Game.LocalPlayer.Character.Position;
            Vector3 dir2 = bluePos2 - redPos2;
            float distance2 = Vector3.Distance(redPos2, bluePos2);
            Vector3 oneThird2 = redPos2 + dir2 * (distance2 - 0.5f);

            CopPosition = oneThird2;


            WriteToLog("BribePoliceAnimation", "Got Position");

            bool Continue = true;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", 0, CopPosition.X, CopPosition.Y, CopPosition.Z, Game.LocalPlayer.Character.Heading);
                NativeFunction.CallByName<uint>("TASK_TURN_PED_TO_FACE_ENTITY", 0, CopToBribe.Pedestrian,-1);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Game.LocalPlayer.Character, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }

            unsafe
            {
                int lol2 = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol2);
                NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", 0, CopPosition.X, CopPosition.Y, CopPosition.Z, CopToBribe.Pedestrian.Heading);
                NativeFunction.CallByName<uint>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Game.LocalPlayer.Character, -1);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol2, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol2);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", CopToBribe.Pedestrian, lol2);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol2);
            }


            uint GameTimeStarted = Game.GameTime;
            while (Game.GameTime - GameTimeStarted <= 15000 && !(Game.LocalPlayer.Character.DistanceTo2D(PlayerPosition) <= 0.15f && CopToBribe.Pedestrian.DistanceTo2D(CopPosition) <= 0.15f && Game.LocalPlayer.Character.FacingSameDirection(CopToBribe.Pedestrian)))// PedToMove.Heading.IsWithin(DesiredHeading - 15f, DesiredHeading + 15f)))
            {


                Rage.Debug.DrawArrowDebug(CopPosition, Vector3.Zero, Rotator.Zero, 1f, Color.Red);
                Rage.Debug.DrawArrowDebug(PlayerPosition, Vector3.Zero, Rotator.Zero, 1f, Color.White);

                GameFiber.Yield();
                if (Extensions.IsMoveControlPressed())
                {
                    Continue = false;
                    break;
                }
            }
            if (!Continue)
            {
                CopToBribe.Pedestrian.BlockPermanentEvents = false;
                CopToBribe.Pedestrian.IsPositionFrozen = false;
                CopToBribe.Pedestrian.Delete();//delete for debug
                Game.LocalPlayer.Character.Tasks.Clear();
                return;
            }

            General.RequestAnimationDictionay("mp_common");
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "mp_common", "givetake1_a", 8.0f, -8.0f, -1, 2, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", CopToBribe.Pedestrian, "mp_common", "givetake1_b", 8.0f, -8.0f, -1, 2, 0, false, false, false);

            //Dont need for debug, works fine
            //Rage.Object MoneyPile = AttachMoneyToPed(PedToMove);

            //GameFiber.Wait(1500);
            //if (MoneyPile.Exists())
            //    MoneyPile.Delete();

            //MoneyPile = AttachMoneyToPed(PedToMoveTo);
            //GameFiber.Wait(1500);
            //if (MoneyPile.Exists())
            //    MoneyPile.Delete();

            Game.LocalPlayer.Character.Tasks.Clear();
            CopToBribe.Pedestrian.Tasks.Clear();
            CopToBribe.Pedestrian.BlockPermanentEvents = false;
            CopToBribe.Pedestrian.IsPositionFrozen = false;

            //Game.LocalPlayer.Character.GiveCash(-1 * Amount);//none for debugging
            CopToBribe.Pedestrian.PlayAmbientSpeech("GENERIC_THANKS");

            Game.LocalPlayer.Character.Tasks.Clear();
            CopToBribe.Pedestrian.Delete();
        });
    }
    private static Vector3 GetPoint(Vector3 pos1,Vector3 pos2, float offset)
    {
        //get the direction between the two transforms -->
        Vector3 dir = (pos2 - pos1);
        dir.Normalize();

        //get a direction that crosses our [dir] direction
        //NOTE! : this can be any of a buhgillion directions that cross our [dir] in 3D space
        //To alter which direction we're crossing in, assign another directional value to the 2nd parameter
        Vector3 perpDir = Vector3.Cross(dir, Vector3.RelativeRight);

        //get our midway point
        Vector3 midPoint = (pos1 + pos2) / 2f;

        //get the offset point
        //This is the point you're looking for.
        Vector3 offsetPoint = midPoint + (perpDir * offset);

        return offsetPoint;
    }
    private static void DebugNumpad5()
    {
        if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null)
        {
            int Group = NativeFunction.CallByName<int>("GET_WEAPONTYPE_GROUP", (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            int Slot = NativeFunction.CallByName<int>("GET_WEAPONTYPE_SLOT", (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            WriteToLog("Debugging", string.Format("Hash: {0} GET_WEAPONTYPE_GROUP: {1} GET_WEAPONTYPE_SLOT: {2}", Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, Group, Slot));
        }


        ScriptController.OutputTable();

    }
    private static void DebugNumpad6()
    {
        if (PlayerStateManager.CurrentVehicle != null && PlayerStateManager.CurrentVehicle.VehicleEnt.Exists())
        {
            Colorbullshit();
            Colorbullshit2();
        }
    }       
    private static void Colorbullshit()
    {
        Color Color1 = Extensions.GetBaseColor1(PlayerStateManager.CurrentVehicle.VehicleEnt.PrimaryColor);
        Color Color2 = Extensions.GetBaseColor2(PlayerStateManager.CurrentVehicle.VehicleEnt.PrimaryColor);
        Color Color3 = Extensions.GetBaseColor3(PlayerStateManager.CurrentVehicle.VehicleEnt.PrimaryColor);
        WriteToLog("Debugging", string.Format("ColorBS: {0} Match1: {1} Match2: {2} Match3: {3}", PlayerStateManager.CurrentVehicle.VehicleEnt.PrimaryColor.ToString(), Color1.ToString(), Color2.ToString(), Color3.ToString()));
    }
    private static void Colorbullshit2()
    {
        Color c = PlayerStateManager.CurrentVehicle.VehicleEnt.PrimaryColor;
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
    private static void DebugNumpad7()
    {
        WriteToLog("Debugging", ClockManager.CurrentTime);

        if (PlayerStateManager.CurrentVehicle != null)
        {
            WriteToLog("Debugging", string.Format("CurrentVehicle  IsStolen:{0} WasReportedStolen:{1} NeedsToBeReportedStolen:{2}", PlayerStateManager.CurrentVehicle.IsStolen, PlayerStateManager.CurrentVehicle.WasReportedStolen, PlayerStateManager.CurrentVehicle.NeedsToBeReportedStolen));
            WriteToLog("Debugging", string.Format("CurrentVehicle  CarPlate.IsWanted:{0} OriginalLicensePlate.IsWanted: {1} ColorMatchesDescription:{2} CopsRecognizeAsStolen: {3}", PlayerStateManager.CurrentVehicle.CarPlate.IsWanted, PlayerStateManager.CurrentVehicle.OriginalLicensePlate.IsWanted, PlayerStateManager.CurrentVehicle.ColorMatchesDescription,PlayerStateManager.CurrentVehicle.CopsRecognizeAsStolen));
        }

        TaskManager.PrintActivities();
    }
    public static void DebugNumpad8()
    {

        try
        {
            CameraManager.DebugAbort();

            WriteToLog("Debugging", "--------------------------------");
            WriteToLog("Debugging", "--------Police Status-----------");

            foreach (Cop Cop in PedManager.Cops.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && x.AssignedAgency != null).OrderBy(x => x.DistanceToPlayer))
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
            WriteToLog("Debugging", string.Format("PoliceInInvestigationMode: {0}", InvestigationManager.InInvestigationMode));
            WriteToLog("Debugging", string.Format("InvestigationPosition: {0}", InvestigationManager.InvestigationPosition));
            WriteToLog("Debugging", string.Format("InvestigationDistance: {0}", InvestigationManager.InvestigationDistance));
            WriteToLog("Debugging", string.Format("ActiveDistance: {0}", PolicePedManager.ActiveDistance));
            WriteToLog("Debugging", string.Format("AnyNear Investigation Position: {0}", PedManager.Cops.Any(x => x.Pedestrian.DistanceTo2D(InvestigationManager.InvestigationPosition) <= InvestigationManager.InvestigationDistance)));
            WriteToLog("Debugging", string.Format("CurrentPoliceStateString: {0}", WantedLevelManager.CurrentPoliceStateString));
            


        }
        catch(Exception e)
        {
            WriteToLog("Debugging error", e.Message + e.StackTrace);
        }
    }
    private static void DebugNumpad9()
    {
        ScriptController.IsRunning = false;
        GameFiber.Sleep(500);
        DebugCopReset();
        Game.DisplayNotification("Instant Action Deactivated");
        ScriptController.Dispose();
    }

    public static void WriteToLog(String ProcedureString, String TextToLog)
    {
        if (ProcedureString == "Error")
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
        }
        string Message = DateTime.Now.ToString("HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog;
        if (SettingsManager.MySettings != null && SettingsManager.MySettings.General.Logging)
        {
            LogMessages.Add(Message);
            Game.Console.Print(Message);
        }
    }

    public class SlidingBuffer<T> : IEnumerable<T>
    {
        private readonly Queue<T> _queue;
        private readonly int _maxCount;

        public SlidingBuffer(int maxCount)
        {
            _maxCount = maxCount;
            _queue = new Queue<T>(maxCount);
        }

        public void Add(T item)
        {
            if (_queue.Count == _maxCount)
                _queue.Dequeue();
            _queue.Enqueue(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

