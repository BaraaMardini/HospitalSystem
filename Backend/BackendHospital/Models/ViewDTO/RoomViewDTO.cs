using System.ComponentModel.DataAnnotations;

public class RoomViewDTO
{
    public int ID { get; set; }
    public string RoomNumber { get; set; }
    public string TypeName { get; set; }
    public string Description { get; set; }
    public  string Notes { get; set; }
    public bool IsActive { get; set; }

    public RoomViewDTO() { }

    public RoomViewDTO(int iD, string roomNumber, string typeName, string description,   string notes, bool isActive)
    {
        ID = iD;
        RoomNumber = roomNumber;
        TypeName = typeName;
        Description = description;
        Notes = notes;
        IsActive = isActive;
    }
}

