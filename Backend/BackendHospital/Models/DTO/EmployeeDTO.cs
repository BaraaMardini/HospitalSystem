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
    [Range(1, int.MaxValue, ErrorMessage = "Invalid StatusID.")]
    public int StatusID { get; set; }

    public EmployeeDTO() { }

    public EmployeeDTO(int iD, int personID, int roleId, int departmentId, decimal salary, int statusID)
    {
        ID = iD;
        PersonID = personID;
        RoleId = roleId;
        DepartmentId = departmentId;
        Salary = salary;
        StatusID = statusID;
    }
}

