using System.ComponentModel.DataAnnotations;

public class PatientViewDTO
{
    public int ID { get; set; }
    public string PersonName { get; set; }
    public int PersonID { get; set; }
    public string BloodType { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }

    public PatientViewDTO() { }

    public PatientViewDTO(int iD, string personName, int personID, string bloodType, string gender, int age)
    {
        ID = iD;
        PersonName = personName;
        PersonID = personID;
        BloodType = bloodType;
        Gender = gender;
        Age = age;
    }
}

