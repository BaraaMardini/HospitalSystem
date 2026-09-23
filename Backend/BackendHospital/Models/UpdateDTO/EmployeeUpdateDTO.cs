using System.ComponentModel.DataAnnotations;

public class EmployeeUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public int?  RoleId { get; set; }
    public int?  DepartmentId { get; set; }
    public decimal?  Salary { get; set; }
    public int?  StatusID { get; set; }

    public EmployeeUpdateDTO() { }

    public EmployeeUpdateDTO(int iD, int personID, int roleId, int departmentId, decimal salary, DateTime hireDate, int statusID)
    {
        RoleId = roleId;
        DepartmentId = departmentId;
        Salary = salary;
        StatusID = statusID;
    }
}

