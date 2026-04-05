using System.ComponentModel.DataAnnotations;

public class DoctorUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public int?  PersonID { get; set; }
    public int?  SpecializationID { get; set; }
    public int?  DepartmentID { get; set; }
    public string?  Qualification { get; set; }
    public string?  DoctorLevel { get; set; }
    public decimal?  Salary { get; set; }

    public DoctorUpdateDTO() { }

    public DoctorUpdateDTO(int iD, int personID, int specializationID, int departmentID, string qualification, string doctorLevel, decimal salary)
    {
        ID = iD;
        PersonID = personID;
        SpecializationID = specializationID;
        DepartmentID = departmentID;
        Qualification = qualification;
        DoctorLevel = doctorLevel;
        Salary = salary;
    }
}

