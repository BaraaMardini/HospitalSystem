using System.ComponentModel.DataAnnotations;

public class EmployeeUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public int?  PersonID { get; set; }
    public int?  RoleId { get; set; }
    public int?  DepartmentId { get; set; }
    public decimal?  Salary { get; set; }
    public DateTime?  HireDate { get; set; }
    public string?  status { get; set; }

    public EmployeeUpdateDTO() { }

    public EmployeeUpdateDTO(int iD, int personID, int roleId, int departmentId, decimal salary, DateTime hireDate, string status)
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

