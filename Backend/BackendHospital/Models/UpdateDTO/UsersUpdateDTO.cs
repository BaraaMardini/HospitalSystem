using System.ComponentModel.DataAnnotations;

public class UsersUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  Username { get; set; }
    public bool?  IsActive { get; set; }
     public string? PasswordHash { get; set; }
    public UsersUpdateDTO() { }

    public UsersUpdateDTO(int iD, string username, string passwordHash, int roleID, long permissionMask1, long permissionMask2, bool isActive)
    {
        Username = username;
        IsActive = isActive;
        PasswordHash = passwordHash;
    }
}

