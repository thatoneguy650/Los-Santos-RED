using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace LosSantosRED.lsr.Data
{
    public class GameConfig
    {
        private ModDataFileManager ModDataFileManager;
        public GameConfig(ModDataFileManager modDataFileManager)
        {
            ModDataFileManager = modDataFileManager;
        }

        public int ConfigNumber { get; set; }
        public string configName { get; set; }

        public void Load()
        {
            try
            {
                EntryPoint.ConfigName = configName;
                EntryPoint.IsLoadingAltConfig = true;
                EntryPoint.ModController.Dispose();
            }
            catch (Exception e)
            {
                Game.FadeScreenIn(0);
                EntryPoint.WriteToConsole($"Error Loading {configName} config: " + e.Message + " " + e.StackTrace, 0);
                Game.DisplayNotification($"Error Loading {configName} config");
            }
        }
    }
}