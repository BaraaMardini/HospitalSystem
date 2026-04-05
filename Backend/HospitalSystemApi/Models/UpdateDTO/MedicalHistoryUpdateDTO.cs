using System.ComponentModel.DataAnnotations;

public class MedicalHistoryUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public int?  PatientID { get; set; }
    public string?  ConditionName { get; set; }
    public DateTime?  DiagnosisDate { get; set; }
    public string?  Status { get; set; }
    public string?  Notes { get; set; }
    public DateTime?  CreatedAt { get; set; }
    public int?  CreatedBy { get; set; }

    public MedicalHistoryUpdateDTO() { }

    public MedicalHistoryUpdateDTO(int iD, int patientID, string conditionName, DateTime diagnosisDate, string status, string notes, DateTime createdAt, int createdBy)
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

