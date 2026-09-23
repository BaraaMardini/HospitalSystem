using System.ComponentModel.DataAnnotations;
using System.Numerics;

public class PermissionsDTO
{
    public int ID { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string ModuleName { get; set; }
    public string ActionName { get; set; }
    public int BitIndex { get; set; }
    public long BitValue { get; set; }
    public int MaskNumber { get; set; }
    public bool IsActive { get; set; }

    public PermissionsDTO() { }

    public PermissionsDTO(int iD, string code, string name, string moduleName, string actionName, int bitIndex, int bitValue, int maskNumber, bool isActive)
    {
        ID = iD;
        Code = code;
        Name = name;
        ModuleName = moduleName;
        ActionName = actionName;
        BitIndex = bitIndex;
        BitValue = bitValue;
        MaskNumber = maskNumber;
        IsActive = isActive;
    }
}

