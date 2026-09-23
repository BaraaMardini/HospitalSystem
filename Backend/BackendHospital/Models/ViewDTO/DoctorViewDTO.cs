using System.ComponentModel.DataAnnotations;

public class DoctorViewDTO
{
    public int ID { get; set; }
    public string PersonName { get; set; }
    public int PeopleID { get; set; }
    public string DepartmentName { get; set; }
    public string SpecializationName { get; set; }
    public string Qualification { get; set; }
    public string DoctorLevel { get; set; }
    public decimal Salary { get; set; }

    public DoctorViewDTO() { }

    public DoctorViewDTO(int iD, string personName, int peopleID, string departmentName, string specializationName, string qualification, string doctorLevel, decimal salary)
    {
        ID = iD;
        PersonName = personName;
        PeopleID = peopleID;
        DepartmentName = departmentName;
        SpecializationName = specializationName;
        Qualification = qualification;
        DoctorLevel = doctorLevel;
        Salary = salary;
    }
}

