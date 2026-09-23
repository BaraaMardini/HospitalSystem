using System.ComponentModel.DataAnnotations;

public class EmployeeViewDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int PersonID { get; set; }
    public string RoleName { get; set; }
    public string DescriptionRole { get; set; }
    public string DepartmentName { get; set; }
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public string StatusName { get; set; }
    public string DescriptionSatus { get; set; }

    public EmployeeViewDTO() { }

    public EmployeeViewDTO(int iD, string name, int personID, string roleName, string descriptionRole, string departmentName, decimal salary, DateTime hireDate, string statusName, string descriptionSatus)
    {
        ID = iD;
        Name = name;
        PersonID = personID;
        RoleName = roleName;
        DescriptionRole = descriptionRole;
        DepartmentName = departmentName;
        Salary = salary;
        HireDate = hireDate;
        StatusName = statusName;
        DescriptionSatus = descriptionSatus;
    }
}

