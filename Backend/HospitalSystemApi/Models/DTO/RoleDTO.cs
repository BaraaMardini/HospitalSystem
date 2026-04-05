using System.ComponentModel.DataAnnotations;

public class RoleDTO
{
    public int ID { get; set; }
    public string RoleName { get; set; }
    public string description { get; set; }

    public RoleDTO() { }

    public RoleDTO(int iD, string roleName, string Description)
    {
       ID = iD;
       RoleName = roleName;
       description = Description;
    }
}

