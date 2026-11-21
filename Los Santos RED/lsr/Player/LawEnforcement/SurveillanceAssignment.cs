using System;

namespace LosSantosRED.lsr
{
    // Persist only identifiers, state and last observed coordinates. Never persist GTA entity handles.
    [Serializable]
    public class SurveillanceAssignment
    {
        public string ID { get; set; }
        public string InvestigationID { get; set; }
        public string TeamID { get; set; }
        public string TargetSubjectID { get; set; }
        public SurveillanceAssignmentState State { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime LastStateChangedAt { get; set; }
        public DateTime LastSuccessfulObservationAt { get; set; }
        public float LastKnownX { get; set; }
        public float LastKnownY { get; set; }
        public float LastKnownZ { get; set; }
        public bool HasLastKnownPosition { get; set; }
        public bool IsCompromised { get; set; }
        public bool ReplacementRequired { get; set; }
        public string Notes { get; set; }

        public SurveillanceAssignment Copy()
        {
            return (SurveillanceAssignment)MemberwiseClone();
        }
    }

    public enum SurveillanceAssignmentState
    {
        Available,
        Assigned,
        Approaching,
        Following,
        LostContact,
        StationaryWatch,
        Reporting,
        Disengaging,
        Compromised,
        Completed
    }

    // State validation is deterministic and independent of spawned world entities.
    public class SurveillanceAssignmentManager
    {
        public bool Transition(SurveillanceAssignment assignment, SurveillanceAssignmentState next, DateTime at)
        {
            if (assignment == null || at == DateTime.MinValue ||
                (assignment.LastStateChangedAt != DateTime.MinValue && at < assignment.LastStateChangedAt))
                return false;
            if (!CanTransition(assignment.State, next)) return false;
            assignment.State = next;
            assignment.LastStateChangedAt = at;
            if (next == SurveillanceAssignmentState.Compromised)
            {
                assignment.IsCompromised = true;
                assignment.ReplacementRequired = true;
            }
            if (next == SurveillanceAssignmentState.LostContact)
                assignment.ReplacementRequired = false;
            if (next == SurveillanceAssignmentState.Completed)
                assignment.ReplacementRequired = false;
            return true;
        }

        public bool RecordObservation(SurveillanceAssignment assignment, DateTime at, float x, float y, float z)
        {
            if (assignment == null || at == DateTime.MinValue ||
                (assignment.LastSuccessfulObservationAt != DateTime.MinValue && at < assignment.LastSuccessfulObservationAt))
                return false;
            assignment.LastKnownX = x;
            assignment.LastKnownY = y;
            assignment.LastKnownZ = z;
            assignment.HasLastKnownPosition = true;
            assignment.LastSuccessfulObservationAt = at;
            return true;
        }

        private static bool CanTransition(SurveillanceAssignmentState current, SurveillanceAssignmentState next)
        {
            if (current == next) return false;
            switch (current)
            {
                case SurveillanceAssignmentState.Available:
                    return next == SurveillanceAssignmentState.Assigned;
                case SurveillanceAssignmentState.Assigned:
                    return next == SurveillanceAssignmentState.Approaching ||
                        next == SurveillanceAssignmentState.Disengaging;
                case SurveillanceAssignmentState.Approaching:
                    return next == SurveillanceAssignmentState.Following ||
                        next == SurveillanceAssignmentState.LostContact ||
                        next == SurveillanceAssignmentState.Disengaging ||
                        next == SurveillanceAssignmentState.Compromised;
                case SurveillanceAssignmentState.Following:
                    return next == SurveillanceAssignmentState.LostContact ||
                        next == SurveillanceAssignmentState.StationaryWatch ||
                        next == SurveillanceAssignmentState.Reporting ||
                        next == SurveillanceAssignmentState.Disengaging ||
                        next == SurveillanceAssignmentState.Compromised;
                case SurveillanceAssignmentState.LostContact:
                    return next == SurveillanceAssignmentState.Approaching ||
                        next == SurveillanceAssignmentState.Reporting ||
                        next == SurveillanceAssignmentState.Disengaging ||
                        next == SurveillanceAssignmentState.Compromised;
                case SurveillanceAssignmentState.StationaryWatch:
                    return next == SurveillanceAssignmentState.Following ||
                        next == SurveillanceAssignmentState.Reporting ||
                        next == SurveillanceAssignmentState.Disengaging ||
                        next == SurveillanceAssignmentState.Compromised;
                case SurveillanceAssignmentState.Reporting:
                    return next == SurveillanceAssignmentState.Following ||
                        next == SurveillanceAssignmentState.Disengaging ||
                        next == SurveillanceAssignmentState.Compromised;
                case SurveillanceAssignmentState.Disengaging:
                case SurveillanceAssignmentState.Compromised:
                    return next == SurveillanceAssignmentState.Completed ||
                        next == SurveillanceAssignmentState.Available;
                case SurveillanceAssignmentState.Completed:
                    return next == SurveillanceAssignmentState.Available;
                default:
                    return false;
            }
        }
    }
}
