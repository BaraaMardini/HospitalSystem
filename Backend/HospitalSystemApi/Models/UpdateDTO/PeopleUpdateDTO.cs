using System.ComponentModel.DataAnnotations;

public class PeopleUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  Name { get; set; }
    public string?  PhoneNumber { get; set; }
    public string?  Address { get; set; }
    public string?  Gender { get; set; }
    public int?  Age { get; set; }
    public string?  Email { get; set; }

    public PeopleUpdateDTO() { }

    public PeopleUpdateDTO(int iD, string name, string phoneNumber, string address, string gender, int age, string email)
    {
        ID = iD;
        Name = name;
        PhoneNumber = phoneNumber;
        Address = address;
        Gender = gender;
        Age = age;
        Email = email;
    }
}

