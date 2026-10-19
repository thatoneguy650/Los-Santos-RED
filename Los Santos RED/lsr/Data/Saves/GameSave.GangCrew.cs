using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr.Data
{
    /// <summary>
    /// Crew roster persistence, in its own partial for the same reason gang progression is:
    /// the feature costs GameSave.cs two dispatch lines and nothing else.
    ///
    /// NextCrewMemberID is saved alongside the roster and restored FIRST. Deriving it from
    /// the records instead — max(ID) + 1 — would recycle an ID the moment the highest
    /// numbered man was pruned, and two crew would share an identity.
    /// </summary>
    public partial class GameSave
    {
        public List<GangCrewMember> GangCrewMembers { get; set; } = new List<GangCrewMember>();

        public int NextCrewMemberID { get; set; } = 1;

        /// <summary>Everyone ever lost, including men the memorial has since dropped.</summary>
        public int TotalCrewLost { get; set; } = 0;

        private void SaveGangCrew(ISaveable player)
        {
            GangCrewMembers = new List<GangCrewMember>();
            if (player?.GangCrewManager == null)
            {
                return;
            }
            List<GangCrewMember> records = player.GangCrewManager.GetSaveRecords();
            if (records != null)
            {
                GangCrewMembers.AddRange(records);
            }
            NextCrewMemberID = player.GangCrewManager.GetNextID();
            TotalCrewLost = player.GangCrewManager.GetTotalLost();
        }

        private void LoadGangCrew(IInventoryable player)
        {
            if (player?.GangCrewManager == null)
            {
                return;
            }
            player.GangCrewManager.RestoreSaveRecords(GangCrewMembers ?? new List<GangCrewMember>(), NextCrewMemberID, TotalCrewLost);
        }
    }
}
