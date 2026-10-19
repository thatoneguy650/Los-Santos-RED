using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr.Data
{
    /// <summary>
    /// Gang progression persistence, kept in its own partial so the feature adds a single
    /// property and two call sites to GameSave.cs and nothing else. Everything that could
    /// otherwise have gone into GangRepSave, SaveReputation, LoadRelationships and
    /// SetRepStats lives here instead, which keeps four busy upstream methods untouched.
    ///
    /// Format note: XmlSerializer omits a null or empty List from the output and yields the
    /// property initialiser when an element is absent, so a save written before this feature
    /// existed loads with an empty list and every gang starts at zero. No migration required —
    /// which is precisely why this was the first thing built.
    /// </summary>
    public partial class GameSave
    {
        public List<GangProgressionSave> GangProgressionSaves { get; set; } = new List<GangProgressionSave>();

        private void SaveGangProgression(ISaveable player)
        {
            GangProgressionSaves = new List<GangProgressionSave>();
            if (player?.GangProgressionManager == null)
            {
                return;
            }
            List<GangProgressionSave> records = player.GangProgressionManager.GetSaveRecords();
            if (records == null)
            {
                return;
            }
            GangProgressionSaves.AddRange(records);
        }

        private void LoadGangProgression(IInventoryable player)
        {
            if (player?.GangProgressionManager == null)
            {
                return;
            }
            player.GangProgressionManager.RestoreSaveRecords(GangProgressionSaves ?? new List<GangProgressionSave>());
        }
    }
}
