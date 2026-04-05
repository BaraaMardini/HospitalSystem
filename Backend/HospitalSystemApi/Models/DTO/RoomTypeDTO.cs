using System.ComponentModel.DataAnnotations;

public class RoomTypeDTO
{
    public int RoomTypeID { get; set; }
    public string TypeName { get; set; }
    public string Description { get; set; }

    public RoomTypeDTO() { }

    public RoomTypeDTO(int roomTypeID, string typeName, string description)
    {
        RoomTypeID = roomTypeID;
        TypeName = typeName;
        Description = description;
    }
}

