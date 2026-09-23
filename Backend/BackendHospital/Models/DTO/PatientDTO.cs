using System.ComponentModel.DataAnnotations;

public class PatientDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid PersonID.")]
    public int PersonID { get; set; }
    public string BloodType { get; set; }

    public PatientDTO() { }

    public PatientDTO(int iD, int personID, string bloodType)
    {
        ID = iD;
        PersonID = personID;
        BloodType = bloodType;
    }
}

