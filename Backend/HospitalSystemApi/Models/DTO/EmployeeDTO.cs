using System.ComponentModel.DataAnnotations;

public class EmployeeDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid PersonID.")]
    public int PersonID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid RoleId.")]
    public int RoleId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid DepartmentId.")]
    public int DepartmentId { get; set; }
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public string status { get; set; }

    public EmployeeDTO() { }

    public EmployeeDTO(int iD, int personID, int roleId, int departmentId, decimal salary, DateTime hireDate, string status)
    {
        ID = iD;
        PersonID = personID;
        RoleId = roleId;
        DepartmentId = departmentId;
        Salary = salary;
        HireDate = hireDate;
        status = status;
    }
}

