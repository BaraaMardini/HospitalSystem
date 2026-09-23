using System.ComponentModel.DataAnnotations;

public class UsersDTO
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid RoleID.")]
    public int RoleID { get; set; }
   
    public bool IsActive { get; set; }

    public  List<Permissions>permissions { get; set; }

    public UsersDTO() { }

    public UsersDTO(int iD, string username, string passwordHash, int roleID,  bool isActive)
    {
        ID = iD;
        Username = username;
        PasswordHash = passwordHash;
        RoleID = roleID;
  
        IsActive = isActive;
    }
}

