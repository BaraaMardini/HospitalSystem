using System.ComponentModel.DataAnnotations;

public class MedicalHistoryViewDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int PatientID { get; set; }
    public string ConditionName { get; set; }
    public DateTime DiagnosisDate { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }

    public MedicalHistoryViewDTO() { }

    public MedicalHistoryViewDTO(int iD, string name, int patientID, string conditionName, DateTime diagnosisDate, string status, string notes, DateTime createdAt, int createdBy)
    {
        ID = iD;
        Name = name;
        PatientID = patientID;
        ConditionName = conditionName;
        DiagnosisDate = diagnosisDate;
        Status = status;
        Notes = notes;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }
}

