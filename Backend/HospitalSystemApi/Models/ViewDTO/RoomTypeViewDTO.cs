using System.ComponentModel.DataAnnotations;

public class RoomTypeViewDTO
{
    public int RoomTypeID { get; set; }
    public string TypeName { get; set; }
    public string Description { get; set; }

    public RoomTypeViewDTO() { }

    public RoomTypeViewDTO(int roomTypeID, string typeName, string description)
    {
        RoomTypeID = roomTypeID;
        TypeName = typeName;
        Description = description;
    }
}

