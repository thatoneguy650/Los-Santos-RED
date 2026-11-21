using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr
{
    // BOLO lifecycle and matching are separate from warrant authority.
    public class BOLOManager
    {
        public BOLORecord Issue(LawEnforcementIntelligence intelligence, DateTime issuedAt,
            BOLOTargetType targetType, string description, string vehicleModel, string plateNumber,
            string zoneID, string lastKnownLocationID, DateTime? expiresAt,
            LawEnforcementIntelligenceSettings settings, string notes = null)
        {
            if (intelligence == null || settings == null || !settings.EnableBOLOSystem) return null;
            settings.Normalize();
            return intelligence.IssueBOLO(issuedAt, targetType, description, vehicleModel, plateNumber,
                zoneID, lastKnownLocationID, expiresAt, settings.BOLOInZoneStopChance / 100f,
                settings.BOLOOutOfZoneStopChance / 100f, notes);
        }

        public void Update(LawEnforcementIntelligence intelligence, DateTime currentTime)
        {
            intelligence?.UpdateExpirations(currentTime);
        }

        public IEnumerable<BOLORecord> GetActive(LawEnforcementIntelligence intelligence, DateTime currentTime)
        {
            if (intelligence == null || intelligence.BOLOs == null) return Enumerable.Empty<BOLORecord>();
            intelligence.UpdateExpirations(currentTime);
            return intelligence.BOLOs.Where(x => x != null && x.Status == BOLOStatus.Active)
                .OrderByDescending(x => x.StopChanceInZone).ToList();
        }

        public float GetVehicleStopChance(LawEnforcementIntelligence intelligence, DateTime currentTime,
            string zoneID, string plateNumber, string vehicleModel)
        {
            return intelligence == null ? 0f :
                intelligence.GetBOLOStopChance(currentTime, zoneID, plateNumber, vehicleModel);
        }

        public float GetPersonRecognitionChance(LawEnforcementIntelligence intelligence, DateTime currentTime,
            string personSubjectID, string zoneID)
        {
            if (intelligence == null || intelligence.BOLOs == null || string.IsNullOrWhiteSpace(personSubjectID))
                return 0f;
            intelligence.UpdateExpirations(currentTime);
            float chance = 0f;
            foreach (BOLORecord bolo in intelligence.BOLOs.Where(x => x != null &&
                x.Status == BOLOStatus.Active && x.TargetType == BOLOTargetType.Person &&
                string.Equals(x.SubjectID, personSubjectID, StringComparison.OrdinalIgnoreCase)))
            {
                float candidate = !string.IsNullOrWhiteSpace(bolo.ZoneID) &&
                    string.Equals(bolo.ZoneID, zoneID, StringComparison.OrdinalIgnoreCase)
                    ? bolo.StopChanceInZone : bolo.StopChanceOutsideZone;
                chance = Math.Max(chance, candidate);
            }
            return Math.Max(0f, Math.Min(1f, chance));
        }

        public bool Cancel(LawEnforcementIntelligence intelligence, string boloID, DateTime at)
        {
            return intelligence != null && intelligence.SetBOLOStatus(boloID, BOLOStatus.Cancelled, at);
        }
    }
}
