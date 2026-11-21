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

    public enum PoliceOperationPlayerResponse
    {
        Undecided,
        Surrendered,
        Resisted,
        DidNotComply,
        Absent,
        CompliedWithSearch,
        RefusedEntry
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
        public PoliceOperationPlayerResponse PlayerResponse { get; set; }

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
            if (intelligence == null || settings == null || !settings.EnableWarrants ||
                now == DateTime.MinValue || string.IsNullOrWhiteSpace(residenceID) ||
                availableUnits <= 0)
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
            {
                if (warrant.Priority < WarrantPriority.Moderate) return null;
                type = PoliceOperationType.SearchWarrantExecution;
            }
            else if (IsArrestWarrant(warrant.Type))
            {
                if (!playerBelievedHome || warrant.Priority < WarrantPriority.High || availableUnits < 2)
                    return null;
                type = PoliceOperationType.ArrestWarrantRaid;
            }
            else
                return null;

            if (intelligence.PoliceOperations == null)
                intelligence.PoliceOperations = new List<PoliceOperationRecord>();
            if (intelligence.PoliceOperations.Any(x => x != null &&
                x.WarrantID == warrant.ID &&
                (x.Stage == PoliceOperationStage.Completed ||
                 (x.Stage != PoliceOperationStage.Aborted && x.Stage != PoliceOperationStage.Completed))))
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
            WarrantRecord warrant = intelligence.Warrants == null ? null :
                intelligence.Warrants.FirstOrDefault(x => x != null && x.ID == operation.WarrantID);
            bool completedByServedArrest = operation.Type == PoliceOperationType.ArrestWarrantRaid &&
                next == PoliceOperationStage.Completed && warrant != null &&
                warrant.Status == WarrantStatus.Served;
            if (warrant == null || (warrant.Status != WarrantStatus.Active && !completedByServedArrest))
            {
                operation.Stage = PoliceOperationStage.Aborted;
                operation.LastUpdatedAt = at;
                operation.Outcome = "Operation aborted because its authorizing warrant is no longer active.";
                intelligence.LastUpdatedAt = at;
                return false;
            }
            if (operation.Type == PoliceOperationType.ArrestWarrantRaid &&
                (next == PoliceOperationStage.Search))
                return false;
            if (operation.Type == PoliceOperationType.SearchWarrantExecution &&
                (next == PoliceOperationStage.ArrestAttempt))
                return false;
            if (!CanAdvance(operation, next)) return false;
            operation.Stage = next;
            operation.LastUpdatedAt = at;
            if (!string.IsNullOrWhiteSpace(outcome)) operation.Outcome = outcome;
            intelligence.LastUpdatedAt = at;
            return true;
        }

        public bool RecordAnnouncementResponse(LawEnforcementIntelligence intelligence,
            string operationID, PoliceOperationPlayerResponse response, DateTime at)
        {
            if (intelligence == null || intelligence.PoliceOperations == null ||
                response == PoliceOperationPlayerResponse.Undecided || at == DateTime.MinValue)
                return false;
            PoliceOperationRecord operation = intelligence.PoliceOperations.FirstOrDefault(x =>
                x != null && x.ID == operationID && x.Stage == PoliceOperationStage.Announcement);
            if (operation == null || at < operation.LastUpdatedAt) return false;
            WarrantRecord authorizingWarrant = intelligence.Warrants == null ? null :
                intelligence.Warrants.FirstOrDefault(x => x != null && x.ID == operation.WarrantID);
            if (authorizingWarrant == null || authorizingWarrant.Status != WarrantStatus.Active)
            {
                operation.Stage = PoliceOperationStage.Aborted;
                operation.LastUpdatedAt = at;
                operation.Outcome = "Operation aborted because its authorizing warrant is no longer active.";
                intelligence.LastUpdatedAt = at;
                return false;
            }

            if (response == PoliceOperationPlayerResponse.Absent)
            {
                if (operation.Type == PoliceOperationType.ArrestWarrantRaid)
                    return AbortIfPlayerAbsent(intelligence, operationID, at);
                operation.Stage = PoliceOperationStage.Search;
                operation.PlayerResponse = response;
                operation.LastUpdatedAt = at;
                operation.Outcome = "Subject was absent; the active search warrant remains executable at the verified premises.";
                intelligence.LastUpdatedAt = at;
                return true;
            }
            if (operation.Type == PoliceOperationType.ArrestWarrantRaid)
            {
                if (response != PoliceOperationPlayerResponse.Surrendered &&
                    response != PoliceOperationPlayerResponse.Resisted &&
                    response != PoliceOperationPlayerResponse.DidNotComply) return false;
                operation.Stage = PoliceOperationStage.Entry;
                operation.Outcome = response == PoliceOperationPlayerResponse.Surrendered
                    ? "Subject surrendered during the announcement phase; officers must complete the arrest and restraint."
                    : response == PoliceOperationPlayerResponse.Resisted
                        ? "Subject resisted the announcement; officers proceed with controlled entry and arrest attempt."
                        : "Subject did not comply with the announcement; officers proceed with controlled entry and arrest attempt.";
            }
            else
            {
                if (response != PoliceOperationPlayerResponse.CompliedWithSearch &&
                    response != PoliceOperationPlayerResponse.RefusedEntry) return false;
                operation.Stage = response == PoliceOperationPlayerResponse.CompliedWithSearch
                    ? PoliceOperationStage.Search : PoliceOperationStage.Entry;
                operation.Outcome = response == PoliceOperationPlayerResponse.CompliedWithSearch
                    ? "Occupant complied with the search warrant."
                    : "Occupant refused entry; officers proceed under the search warrant's authorized entry procedure.";
            }
            operation.PlayerResponse = response;
            operation.LastUpdatedAt = at;
            intelligence.LastUpdatedAt = at;
            return true;
        }

        public bool Abort(LawEnforcementIntelligence intelligence, string operationID, DateTime at, string reason)
        {
            if (intelligence == null || intelligence.PoliceOperations == null || at == DateTime.MinValue)
                return false;
            PoliceOperationRecord operation = intelligence.PoliceOperations.FirstOrDefault(x =>
                x != null && x.ID == operationID &&
                x.Stage != PoliceOperationStage.Completed && x.Stage != PoliceOperationStage.Aborted);
            if (operation == null || at < operation.LastUpdatedAt) return false;
            operation.Stage = PoliceOperationStage.Aborted;
            operation.LastUpdatedAt = at;
            operation.Outcome = string.IsNullOrWhiteSpace(reason) ? "Operation aborted." : reason;
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
            if (operation == null || operation.Type != PoliceOperationType.ArrestWarrantRaid ||
                at < operation.LastUpdatedAt) return false;
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

        private static bool CanAdvance(PoliceOperationRecord operation, PoliceOperationStage next)
        {
            switch (operation.Stage)
            {
                case PoliceOperationStage.Planned: return next == PoliceOperationStage.Deploying || next == PoliceOperationStage.Aborted;
                case PoliceOperationStage.Deploying: return next == PoliceOperationStage.Perimeter || next == PoliceOperationStage.Aborted;
                case PoliceOperationStage.Perimeter: return next == PoliceOperationStage.Announcement || next == PoliceOperationStage.Aborted;
                case PoliceOperationStage.Announcement:
                    return next == PoliceOperationStage.Entry || next == PoliceOperationStage.Search ||
                        next == PoliceOperationStage.Aborted;
                case PoliceOperationStage.Entry:
                    return next == PoliceOperationStage.ArrestAttempt ||
                        (operation.Type == PoliceOperationType.SearchWarrantExecution && next == PoliceOperationStage.Search) ||
                        next == PoliceOperationStage.Completed || next == PoliceOperationStage.Aborted;
                case PoliceOperationStage.ArrestAttempt:
                case PoliceOperationStage.Search:
                    return next == PoliceOperationStage.Completed || next == PoliceOperationStage.Aborted;
                default: return false;
            }
        }
    }
}
