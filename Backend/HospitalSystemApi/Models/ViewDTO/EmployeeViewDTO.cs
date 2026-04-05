using System.ComponentModel.DataAnnotations;

public class EmployeeViewDTO
{
    public int ID { get; set; }
    public string PersonName { get; set; }
    public string DepartmentName { get; set; }
    public string DescriptionRole { get; set; }
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public string status { get; set; }
    public string PhoneNumber { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }

    public EmployeeViewDTO() { }

    public EmployeeViewDTO(int iD, string personName, string departmentName, string descriptionRole, decimal salary, DateTime hireDate, string status, string phoneNumber, string gender, int age, string email)
    {
        ID = iD;
        PersonName = personName;
        DepartmentName = departmentName;
        DescriptionRole = descriptionRole;
        Salary = salary;
        HireDate = hireDate;
        status = status;
        PhoneNumber = phoneNumber;
        Gender = gender;
        Age = age;
        Email = email;
    }
}

