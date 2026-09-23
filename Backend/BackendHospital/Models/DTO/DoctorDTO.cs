using System.ComponentModel.DataAnnotations;

public class DoctorDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid PersonID.")]
    public int PersonID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid SpecializationID.")]
    public int SpecializationID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid DepartmentID.")]
    public int DepartmentID { get; set; }
    public string Qualification { get; set; }
    public string DoctorLevel { get; set; }
    public decimal Salary { get; set; }

    public DoctorDTO() { }

    public DoctorDTO(int iD, int personID, int specializationID, int departmentID, string qualification, string doctorLevel, decimal salary)
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

