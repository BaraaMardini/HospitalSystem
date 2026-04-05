using System.ComponentModel.DataAnnotations;

public class RoomTypeUpdateDTO
{
 [Required]
    public int  RoomTypeID { get; set; }
    public string?  TypeName { get; set; }
    public string?  Description { get; set; }

    public RoomTypeUpdateDTO() { }

    public RoomTypeUpdateDTO(int roomTypeID, string typeName, string description)
    {
        RoomTypeID = roomTypeID;
        TypeName = typeName;
        Description = description;
    }
}

