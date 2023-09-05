using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class TreatmentOptions
{
    public TreatmentOptions()
    {
    }

    public TreatmentOptions(string iD, string name, List<MedicalTreatment> medicalTreatments)
    {
        ID = iD;
        Name = name;
        MedicalTreatments = medicalTreatments;
    }

    public string ID { get; set; }
    public string Name { get; set; }
    public List<MedicalTreatment> MedicalTreatments { get; set; } = new List<MedicalTreatment>();
}

