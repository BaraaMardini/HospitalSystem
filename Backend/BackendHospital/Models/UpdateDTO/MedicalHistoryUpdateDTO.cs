using System.ComponentModel.DataAnnotations;

public class MedicalHistoryUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  ConditionName { get; set; }
    public string?  Notes { get; set; }

    public MedicalHistoryUpdateDTO() { }

    public MedicalHistoryUpdateDTO(int iD, int patientID, string conditionName, DateTime diagnosisDate, string notes)
    {
        ConditionName = conditionName;
        Notes = notes;
    }
}

