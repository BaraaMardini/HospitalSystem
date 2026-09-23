using System.ComponentModel.DataAnnotations;

public class RoomTypeUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  TypeName { get; set; }
    public string?  Description { get; set; }

    public RoomTypeUpdateDTO() { }

    public RoomTypeUpdateDTO(int iD, string typeName, string description)
    {
        TypeName = typeName;
        Description = description;
    }
}

