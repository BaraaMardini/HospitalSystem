using System.ComponentModel.DataAnnotations;

public class RoleViewDTO
{
    public int ID { get; set; }
    public string RoleName { get; set; }
    public string description { get; set; }

    public RoleViewDTO() { }

    public RoleViewDTO(int iD, string roleName, string description)
    {
        ID = iD;
        RoleName = roleName;
     this.description = description;
    }
}

