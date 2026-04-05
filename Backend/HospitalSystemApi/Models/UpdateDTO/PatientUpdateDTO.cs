using System.ComponentModel.DataAnnotations;

public class PatientUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public int?  PersonID { get; set; }
    public string?  BloodType { get; set; }

    public PatientUpdateDTO() { }

    public PatientUpdateDTO(int iD, int personID, string bloodType)
    {
        ID = iD;
        PersonID = personID;
        BloodType = bloodType;
    }
}

