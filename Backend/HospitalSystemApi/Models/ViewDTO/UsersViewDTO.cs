using System.ComponentModel.DataAnnotations;

public class UsersViewDTO
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string role { get; set; }
    public DateTime CreatedAt { get; set; }
    public int EmployeeID { get; set; }
    public string Name { get; set; }

    public UsersViewDTO() { }

    public UsersViewDTO(int iD, string username, string passwordHash, string role, DateTime createdAt, int employeeID, string name)
    {
        ID = iD;
        Username = username;
        PasswordHash = passwordHash;
        role = role;
        CreatedAt = createdAt;
        EmployeeID = employeeID;
        Name = name;
    }
}

