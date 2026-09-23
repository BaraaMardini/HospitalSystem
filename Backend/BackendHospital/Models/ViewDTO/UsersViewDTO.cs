using System.ComponentModel.DataAnnotations;

public class UsersViewDTO
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string RoleName { get; set; }
    public string description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }


    public UsersViewDTO() { }

    public UsersViewDTO(int iD, string username, string roleName, string _description, DateTime createdAt, bool isActive)
    {
        ID = iD;
        Username = username;
        RoleName = roleName;
        description = _description;
        CreatedAt = createdAt;
        IsActive = isActive;
    
    }
}

