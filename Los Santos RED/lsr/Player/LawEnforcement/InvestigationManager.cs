using System;
using System.Linq;

namespace LosSantosRED.lsr
{
    // Escalation is driven exclusively by police-known evidence, never elapsed wanted time.
    public class InvestigationManager
    {
        public InvestigationRecord Refresh(LawEnforcementIntelligence intelligence, DateTime now,
            string assignedAgencyID = null)
        {
            if (intelligence == null || now == DateTime.MinValue) return null;
            intelligence.Normalize();
            return intelligence.RefreshInvestigation(now, assignedAgencyID);
        }

        public bool Close(LawEnforcementIntelligence intelligence, string investigationID, DateTime at, string reason)
        {
            if (intelligence == null || intelligence.Investigations == null ||
                string.IsNullOrWhiteSpace(investigationID) || at == DateTime.MinValue) return false;
            InvestigationRecord record = intelligence.Investigations.FirstOrDefault(x =>
                x != null && x.ID == investigationID && x.Stage != InvestigationStage.Closed);
            if (record == null || at < record.CreatedAt) return false;
            record.Stage = InvestigationStage.Closed;
            record.LastActivityAt = at;
            record.Notes = string.IsNullOrWhiteSpace(reason) ? record.Notes : reason;
            intelligence.LastUpdatedAt = at;
            return true;
        }

        public bool Suspend(LawEnforcementIntelligence intelligence, string investigationID, DateTime at)
        {
            if (intelligence == null || intelligence.Investigations == null ||
                string.IsNullOrWhiteSpace(investigationID) || at == DateTime.MinValue) return false;
            InvestigationRecord record = intelligence.Investigations.FirstOrDefault(x =>
                x != null && x.ID == investigationID && x.Stage != InvestigationStage.Closed);
            if (record == null || at < record.CreatedAt) return false;
            record.Stage = InvestigationStage.Suspended;
            record.LastActivityAt = at;
            intelligence.LastUpdatedAt = at;
            return true;
        }
    }
}
