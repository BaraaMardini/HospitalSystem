using System.ComponentModel.DataAnnotations;

public class RoleUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  RoleName { get; set; }
    public string?  description { get; set; }

    public RoleUpdateDTO() { }

    public RoleUpdateDTO(int iD, string roleName, string description)
    {
        ID = iD;
        RoleName = roleName;
        description = description;
    }
}

