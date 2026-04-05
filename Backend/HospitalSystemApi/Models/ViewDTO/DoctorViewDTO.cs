using System.ComponentModel.DataAnnotations;

public class DoctorViewDTO
{
    public int ID { get; set; }
    public string NamePerson { get; set; }
    public string SpecializationName { get; set; }
    public string DepartmentName { get; set; }
    public string Qualification { get; set; }
    public string DoctorLevel { get; set; }
    public decimal Salary { get; set; }

    public DoctorViewDTO() { }

    public DoctorViewDTO(int iD, string namePerson, string specializationName, string departmentName, string qualification, string doctorLevel, decimal salary)
    {
        ID = iD;
        NamePerson = namePerson;
        SpecializationName = specializationName;
        DepartmentName = departmentName;
        Qualification = qualification;
        DoctorLevel = doctorLevel;
        Salary = salary;
    }
}

