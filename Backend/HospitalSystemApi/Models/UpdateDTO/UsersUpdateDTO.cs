using System.ComponentModel.DataAnnotations;

public class UsersUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  Username { get; set; }
    public string?  PasswordHash { get; set; }
    public string?  role { get; set; }
    public int?  EmployeeID { get; set; }
    public DateTime?  CreatedAt { get; set; }

    public UsersUpdateDTO() { }

    public UsersUpdateDTO(int iD, string username, string passwordHash, string role, int employeeID, DateTime createdAt)
    {
        ID = iD;
        Username = username;
        PasswordHash = passwordHash;
        role = role;
        EmployeeID = employeeID;
        CreatedAt = createdAt;
    }
}

