using System;
using System.Linq;

namespace LosSantosRED.lsr
{
    // Coordinates lifecycle processing for the persistent intelligence data. This class
    // deliberately contains no GTA entity handles and performs no world scans.
    public class LawEnforcementIntelligenceManager
    {
        private DateTime LastUpdateAt { get; set; }

        public WarrantManager WarrantManager { get; private set; } = new WarrantManager();
        public BOLOManager BOLOManager { get; private set; } = new BOLOManager();
        public IntelligenceDatabase IntelligenceDatabase { get; private set; } = new IntelligenceDatabase();
        public InvestigationManager InvestigationManager { get; private set; } = new InvestigationManager();
        public SurveillanceAssignmentManager SurveillanceAssignmentManager { get; private set; } = new SurveillanceAssignmentManager();
        public PoliceOperationsManager PoliceOperationsManager { get; private set; } = new PoliceOperationsManager();

        public string GetDebugSummary(LawEnforcementIntelligence intelligence)
        {
            if (intelligence == null) return "Law enforcement intelligence: unavailable";
            intelligence.Normalize();
            int activeWarrants = intelligence.Warrants.Count(x => x != null && x.Status == WarrantStatus.Active);
            int activeBOLOs = intelligence.BOLOs.Count(x => x != null && x.Status == BOLOStatus.Active);
            int policeObservations = intelligence.Observations.Count(x => x != null && x.IsKnownToPolice);
            int activeSurveillance = intelligence.SurveillanceAssignments.Count(x => x != null &&
                x.State != SurveillanceAssignmentState.Available &&
                x.State != SurveillanceAssignmentState.Completed);
            int activeOperations = intelligence.PoliceOperations.Count(x => x != null &&
                x.Stage != PoliceOperationStage.Completed && x.Stage != PoliceOperationStage.Aborted);
            InvestigationRecord current = intelligence.Investigations
                .Where(x => x != null && x.Stage != InvestigationStage.Closed &&
                    x.Stage != InvestigationStage.Suspended)
                .OrderByDescending(x => x.HeatLevel).FirstOrDefault();
            return string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "LE Intelligence | Observations: {0} (Police-known: {1}) | Investigations: {2} | Stage: {3} | Warrants: {4} | BOLOs: {5} | FIB teams: {6} | Operations: {7}",
                intelligence.Observations.Count, policeObservations, intelligence.Investigations.Count,
                current == null ? "None" : current.Stage.ToString(), activeWarrants, activeBOLOs,
                activeSurveillance, activeOperations);
        }

        public bool TryAssignSurveillance(LawEnforcementIntelligence intelligence, string investigationID,
            string teamID, DateTime assignedAt, LawEnforcementIntelligenceSettings settings)
        {
            if (intelligence == null || settings == null || !settings.EnableFIBSurveillance ||
                string.IsNullOrWhiteSpace(teamID) || assignedAt == DateTime.MinValue) return false;
            settings.Normalize();
            intelligence.Normalize();
            int activeCount = intelligence.SurveillanceAssignments.Count(x => x != null &&
                x.State != SurveillanceAssignmentState.Available &&
                x.State != SurveillanceAssignmentState.Completed);
            if (activeCount >= settings.MaxActiveFIBTeams ||
                intelligence.SurveillanceAssignments.Any(x => x != null && x.TeamID == teamID &&
                    x.State != SurveillanceAssignmentState.Completed &&
                    x.State != SurveillanceAssignmentState.Available)) return false;
            InvestigationRecord investigation = intelligence.Investigations.FirstOrDefault(x =>
                x != null && x.ID == investigationID && x.Stage != InvestigationStage.Closed &&
                x.Stage != InvestigationStage.Suspended);
            if (investigation == null) return false;
            intelligence.SurveillanceAssignments.Add(new SurveillanceAssignment
            {
                ID = Guid.NewGuid().ToString("N"),
                InvestigationID = investigation.ID,
                TeamID = teamID,
                TargetSubjectID = intelligence.SubjectID,
                State = SurveillanceAssignmentState.Assigned,
                AssignedAt = assignedAt,
                LastStateChangedAt = assignedAt
            });
            intelligence.LastUpdatedAt = assignedAt;
            return true;
        }

        public void Update(LawEnforcementIntelligence intelligence, DateTime currentTime, LawEnforcementIntelligenceSettings settings)
        {
            if (intelligence == null || currentTime == DateTime.MinValue || settings == null) return;
            settings.Normalize();
            intelligence.Normalize();

            // Do not run against a clock that has moved backwards (for example, a save load).
            if (LastUpdateAt != DateTime.MinValue && currentTime < LastUpdateAt)
                LastUpdateAt = DateTime.MinValue;
            if (LastUpdateAt != DateTime.MinValue &&
                currentTime - LastUpdateAt < TimeSpan.FromMilliseconds(settings.InvestigationUpdateInterval))
                return;

            if (settings.EnableWarrants)
                WarrantManager.UpdateExpirations(intelligence, currentTime);
            if (settings.EnableBOLOSystem)
                BOLOManager.Update(intelligence, currentTime);

            if (settings.EnableBackgroundInvestigation)
            {
                bool hasPoliceEvidence = intelligence.Observations.Any(x => x != null && x.IsKnownToPolice && x.ObservedAt <= currentTime);
                bool hasActiveRecords = intelligence.Warrants.Any(x => x != null && x.Status == WarrantStatus.Active) ||
                    intelligence.BOLOs.Any(x => x != null && x.Status == BOLOStatus.Active);
                bool hasOpenInvestigation = intelligence.Investigations.Any(x => x != null &&
                    x.Stage != InvestigationStage.Closed && x.Stage != InvestigationStage.Suspended);
                if (hasPoliceEvidence || hasActiveRecords || hasOpenInvestigation)
                    InvestigationManager.Refresh(intelligence, currentTime);
            }

            PruneOldObservations(intelligence, currentTime, settings.IntelligenceRetentionDays);
            intelligence.LastUpdatedAt = currentTime;
            LastUpdateAt = currentTime;
        }

        private static void PruneOldObservations(LawEnforcementIntelligence intelligence, DateTime currentTime, int retentionDays)
        {
            DateTime cutoff = currentTime.AddDays(-Math.Max(1, retentionDays));
            List<string> expiredIDs = new List<string>();

            if (intelligence.Observations != null && intelligence.Observations.Count > 0)
            {
                expiredIDs = intelligence.Observations
                    .Where(x => x != null && x.ObservedAt < cutoff)
                    .Select(x => x.ID)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();

                if (expiredIDs.Count > 0)
                {
                    intelligence.Observations.RemoveAll(x => x == null || x.ObservedAt < cutoff);
                    foreach (InvestigationRecord investigation in intelligence.Investigations.Where(x => x != null))
                        if (investigation.ObservationIDs != null)
                            investigation.ObservationIDs.RemoveAll(x => expiredIDs.Contains(x));
                    foreach (WarrantRecord warrant in intelligence.Warrants.Where(x => x != null))
                        if (warrant.EvidenceObservationIDs != null)
                            warrant.EvidenceObservationIDs.RemoveAll(x => expiredIDs.Contains(x));
                }
            }

            if (intelligence.VehicleAssociations != null)
                intelligence.VehicleAssociations.RemoveAll(x => x == null || x.ObservedAt < cutoff);
        }

        public void ResetClock()
        {
            LastUpdateAt = DateTime.MinValue;
        }
    }
}
