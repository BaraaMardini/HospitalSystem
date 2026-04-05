using System.ComponentModel.DataAnnotations;

public class UsersDTO
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string role { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid EmployeeID.")]
    public int EmployeeID { get; set; }
    public DateTime CreatedAt { get; set; }

    public UsersDTO() { }

    public UsersDTO(int iD, string username, string passwordHash, string role, int employeeID, DateTime createdAt)
    {
        ID = iD;
        Username = username;
        PasswordHash = passwordHash;
        role = role;
        EmployeeID = employeeID;
        CreatedAt = createdAt;
    }
}

