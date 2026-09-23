using System.ComponentModel.DataAnnotations;

public class MedicalHistoryDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid PatientID.")]
    public int PatientID { get; set; }
    public string ConditionName { get; set; }
    public string Notes { get; set; }

    public MedicalHistoryDTO() { }

    public MedicalHistoryDTO(int iD, int patientID, string conditionName, string notes)
    {
        ID = iD;
        PatientID = patientID;
        ConditionName = conditionName;
        Notes = notes;
    }
}

