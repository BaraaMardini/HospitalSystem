using System.ComponentModel.DataAnnotations;

public class PermissionsViewDTO
{
    public int ID { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string ModuleName { get; set; }
    public string ActionName { get; set; }
    public bool IsActive { get; set; }

    public PermissionsViewDTO() { }

    public PermissionsViewDTO(int iD, string code, string name, string moduleName, string actionName, bool isActive)
    {
        ID = iD;
        Code = code;
        Name = name;
        ModuleName = moduleName;
        ActionName = actionName;
        IsActive = isActive;
    }
}

