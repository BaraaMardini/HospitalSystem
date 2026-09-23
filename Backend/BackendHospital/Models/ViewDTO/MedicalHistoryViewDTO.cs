using System.ComponentModel.DataAnnotations;

public class MedicalHistoryViewDTO
{
    public int ID { get; set; }
    public string PersonName { get; set; }
    public int PatientID { get; set; }
    public string ConditionName { get; set; }
    public DateTime DiagnosisDate { get; set; }
    public string Notes { get; set; }

    public MedicalHistoryViewDTO() { }

    public MedicalHistoryViewDTO(int iD, string personName, int patientID, string conditionName, DateTime diagnosisDate, string notes)
    {
        ID = iD;
        PersonName = personName;
        PatientID = patientID;
        ConditionName = conditionName;
        DiagnosisDate = diagnosisDate;
        Notes = notes;
    }
}

