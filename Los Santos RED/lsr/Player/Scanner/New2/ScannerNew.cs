//using LosSantosRED.lsr.Interface;
//using Rage;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class ScannerNew : IScannerPlayable//handles the queue and playing the actual audio files
//{
//    private IEntityProvideable World;
//    private IAudioPlayable AudioPlayer;
//    private IPoliceRespondable Player;
//    private IPlacesOfInterest PlacesOfInterest;
//    private ITimeReportable Time;
//    private ISettingsProvideable Settings;
//    private bool ExecutingQueue;

//    public List<DispatchReport> DispatchReports { get; set; } = new List<DispatchReport>();
//    public int HighestOfficerReportedPriority { get; set; } = 99;
//    public int HighestCivilianReportedPriority { get; set; } = 99;

//    public ScannerNew(IEntityProvideable world, IPoliceRespondable currentPlayer, IAudioPlayable audioPlayer, ISettingsProvideable settings, ITimeReportable time, IPlacesOfInterest placesOfInterest)
//    {
//        AudioPlayer = audioPlayer;
//        Player = currentPlayer;
//        World = world;
//        Settings = settings;
//        Time = time;
//        PlacesOfInterest = placesOfInterest;
//    }

//    public void Setup()
//    {

//    }
//    public void Update()
//    {
//        AnnounceQueue();
//    }
//    public void Dispose()
//    {

//    }
//    public void Announce(string dispatchID, DispatchReportInformation dispatchReportInformation)
//    {
//        DispatchReport found = DispatchReports.FirstOrDefault(x => x.DispatchID == dispatchID);
//        if(found != null)
//        {
//            found.DispatchReportInformation = dispatchReportInformation;
//        }
//        else
//        {
//           // DispatchReports.Add(new DispatchReport(dispatchID, dispatchReportInformation));
//        }

//    }


//    private void AnnounceQueue()
//    {
//        if (DispatchReports.Count > 0 && !ExecutingQueue)
//        {
//            ExecutingQueue = true;
//            GameFiber.Yield();
//            GameFiber PlayDispatchQueue = GameFiber.StartNew(delegate
//            {
//                GameFiber.Sleep(RandomItems.MyRand.Next(Settings.SettingsManager.ScannerSettings.DelayMinTime, Settings.SettingsManager.ScannerSettings.DelayMaxTime));//GameFiber.Sleep(RandomItems.MyRand.Next(2500, 4500));//Next(1500, 2500)
//                CleanQueue();
//                PlayQueue();
//                ExecutingQueue = false;
//            }, "PlayDispatchQueue");
//        }
//    }
//    private void CleanQueue()
//    {
//        if (DispatchReports.Any(x => x.DispatchReportInformation.SeenByOfficers))
//        {
//            DispatchReports.RemoveAll(x => !x.DispatchReportInformation.SeenByOfficers);
//        }
//        if (DispatchReports.Count() > 1)
//        {
//            DispatchReport HighestItem = null;// DispatchReports.OrderBy(x => x.Priority).FirstOrDefault();
//            DispatchReports.Clear();
//            if (HighestItem != null)
//            {
//                DispatchReports.Add(HighestItem);
//            }
//        }
//    }
//    private void PlayQueue()
//    {
//        while (DispatchReports.Count > 0)
//        {
//            DispatchReport Item = null;// DispatchReports.OrderBy(x => x.Priority).ToList()[0];
//            bool AddToPlayed = true;
//            if (Player.IsNotWanted && Item.DispatchReportInformation.SeenByOfficers)
//            {
//                AddToPlayed = false;
//            }
//           // BuildDispatch(Item, AddToPlayed);




//            if (DispatchReports.Contains(Item))
//            {
//                DispatchReports.Remove(Item);
//            }
//            GameFiber.Yield();
//        }
//    }




//}

