using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr
{
    public enum PoliceOperationType
    {
        ArrestWarrantRaid,
        SearchWarrantExecution
    }

    public enum PoliceOperationStage
    {
        Planned,
        Deploying,
        Perimeter,
        Announcement,
        Entry,
        ArrestAttempt,
        Search,
        Completed,
        Aborted
    }

    [Serializable]
    public class PoliceOperationRecord
    {
        public string ID { get; set; }
        public string WarrantID { get; set; }
        public string InvestigationID { get; set; }
        public string ResidenceID { get; set; }
        public PoliceOperationType Type { get; set; }
        public PoliceOperationStage Stage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public bool PlayerBelievedHome { get; set; }
        public string Outcome { get; set; }

        public PoliceOperationRecord Copy()
        {
            return (PoliceOperationRecord)MemberwiseClone();
        }
    }

    // Plans operations and enforces legal prerequisites. Runtime deployment is delegated to game-facing tasks.
    public class PoliceOperationsManager
    {
        public PoliceOperationRecord PlanResidenceOperation(LawEnforcementIntelligence intelligence,
            string warrantID, string investigationID, string residenceID, bool playerBelievedHome,
            int availableUnits, DateTime now, LawEnforcementIntelligenceSettings settings)
        {
            if (intelligence == null || settings == null || now == DateTime.MinValue ||
                string.IsNullOrWhiteSpace(residenceID) || !playerBelievedHome || availableUnits <= 0)
                return null;
            settings.Normalize();
            WarrantRecord warrant = intelligence.Warrants == null ? null :
                intelligence.Warrants.FirstOrDefault(x => x != null && x.ID == warrantID &&
                    x.Status == WarrantStatus.Active);
            if (warrant == null || !string.Equals(warrant.SuspectedResidenceID, residenceID,
                StringComparison.OrdinalIgnoreCase) ||
                warrant.ResidenceConfidence * 100f < settings.RaidMinimumConfidence)
                return null;

            PoliceOperationType type;
            if (warrant.Type == WarrantType.Search)
                type = PoliceOperationType.SearchWarrantExecution;
            else if (IsArrestWarrant(warrant.Type))
                type = PoliceOperationType.ArrestWarrantRaid;
            else
                return null;

            if (intelligence.PoliceOperations == null)
                intelligence.PoliceOperations = new List<PoliceOperationRecord>();
            if (intelligence.PoliceOperations.Any(x => x != null &&
                x.WarrantID == warrant.ID && x.Stage != PoliceOperationStage.Completed &&
                x.Stage != PoliceOperationStage.Aborted))
                return null;

            PoliceOperationRecord operation = new PoliceOperationRecord
            {
                ID = Guid.NewGuid().ToString("N"),
                WarrantID = warrant.ID,
                InvestigationID = investigationID,
                ResidenceID = residenceID,
                Type = type,
                Stage = PoliceOperationStage.Planned,
                CreatedAt = now,
                LastUpdatedAt = now,
                PlayerBelievedHome = true
            };
            intelligence.PoliceOperations.Add(operation);
            intelligence.LastUpdatedAt = now;
            return operation;
        }

        public bool Advance(LawEnforcementIntelligence intelligence, string operationID,
            PoliceOperationStage next, DateTime at, string outcome = null)
        {
            if (intelligence == null || intelligence.PoliceOperations == null ||
                string.IsNullOrWhiteSpace(operationID) || at == DateTime.MinValue) return false;
            PoliceOperationRecord operation = intelligence.PoliceOperations.FirstOrDefault(x =>
                x != null && x.ID == operationID);
            if (operation == null || at < operation.LastUpdatedAt ||
                operation.Stage == PoliceOperationStage.Completed || operation.Stage == PoliceOperationStage.Aborted)
                return false;
            if (operation.Type == PoliceOperationType.ArrestWarrantRaid &&
                (next == PoliceOperationStage.Search))
                return false;
            if (operation.Type == PoliceOperationType.SearchWarrantExecution &&
                (next == PoliceOperationStage.ArrestAttempt))
                return false;
            if (!CanAdvance(operation.Stage, next)) return false;
            operation.Stage = next;
            operation.LastUpdatedAt = at;
            if (!string.IsNullOrWhiteSpace(outcome)) operation.Outcome = outcome;
            intelligence.LastUpdatedAt = at;
            return true;
        }

        public bool AbortIfPlayerAbsent(LawEnforcementIntelligence intelligence, string operationID,
            DateTime at)
        {
            if (intelligence == null || intelligence.PoliceOperations == null) return false;
            PoliceOperationRecord operation = intelligence.PoliceOperations.FirstOrDefault(x =>
                x != null && x.ID == operationID && x.Stage != PoliceOperationStage.Completed &&
                x.Stage != PoliceOperationStage.Aborted);
            if (operation == null || at < operation.LastUpdatedAt) return false;
            operation.Stage = PoliceOperationStage.Aborted;
            operation.LastUpdatedAt = at;
            operation.Outcome = "Operation aborted: subject not present at the verified location.";
            intelligence.LastUpdatedAt = at;
            return true;
        }

        private static bool IsArrestWarrant(WarrantType type)
        {
            return type == WarrantType.Arrest || type == WarrantType.Bench ||
                type == WarrantType.ProbationParole || type == WarrantType.Extradition;
        }

        private static bool CanAdvance(PoliceOperationStage current, PoliceOperationStage next)
        {
            switch (current)
            {
                case PoliceOperationStage.Planned: return next == PoliceOperationStage.Deploying || next == PoliceOperationStage.Aborted;
                case PoliceOperationStage.Deploying: return next == PoliceOperationStage.Perimeter || next == PoliceOperationStage.Aborted;
                case PoliceOperationStage.Perimeter: return next == PoliceOperationStage.Announcement || next == PoliceOperationStage.Aborted;
                case PoliceOperationStage.Announcement:
                    return next == PoliceOperationStage.Entry || next == PoliceOperationStage.Search ||
                        next == PoliceOperationStage.Aborted;
                case PoliceOperationStage.Entry:
                    return next == PoliceOperationStage.ArrestAttempt || next == PoliceOperationStage.Completed ||
                        next == PoliceOperationStage.Aborted;
                case PoliceOperationStage.ArrestAttempt:
                case PoliceOperationStage.Search:
                    return next == PoliceOperationStage.Completed || next == PoliceOperationStage.Aborted;
                default: return false;
            }
        }
    }
}
