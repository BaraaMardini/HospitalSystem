using System.ComponentModel.DataAnnotations;

public class PermissionsUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  Name { get; set; }
    public bool?  IsActive { get; set; }

    public PermissionsUpdateDTO() { }

    public PermissionsUpdateDTO(int iD, string code, string name, string moduleName, string actionName, int bitIndex, long bitValue, int maskNumber, bool isActive)
    {
        Name = name;
        IsActive = isActive;
    }
}

