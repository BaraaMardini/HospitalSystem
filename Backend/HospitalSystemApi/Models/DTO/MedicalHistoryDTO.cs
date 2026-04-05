using System.ComponentModel.DataAnnotations;

public class MedicalHistoryDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid PatientID.")]
    public int PatientID { get; set; }
    public string ConditionName { get; set; }
    public DateTime DiagnosisDate { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
    public DateTime? CreatedAt { get; set; } = null;
    [Range(1, int.MaxValue, ErrorMessage = "Invalid CreatedBy.")]
    public int CreatedBy { get; set; }

    public MedicalHistoryDTO() { }

    public MedicalHistoryDTO(int iD, int patientID, string conditionName, DateTime diagnosisDate, string status, string notes, DateTime createdAt, int createdBy)
    {
        ID = iD;
        PatientID = patientID;
        ConditionName = conditionName;
        DiagnosisDate = diagnosisDate;
        Status = status;
        Notes = notes;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }
}

