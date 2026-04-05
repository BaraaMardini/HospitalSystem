using System.ComponentModel.DataAnnotations;

public class PatientViewDTO
{
    public int ID { get; set; }
    public string BloodType { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }

    public PatientViewDTO() { }

    public PatientViewDTO(int iD, string bloodType, string name, string phoneNumber, string address, int age, string gender, string email)
    {
        ID = iD;
        BloodType = bloodType;
        Name = name;
        PhoneNumber = phoneNumber;
        Address = address;
        Age = age;
        Gender = gender;
        Email = email;
    }
}

