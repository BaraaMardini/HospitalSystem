using System.ComponentModel.DataAnnotations;

public class RoomTypeDTO
{
    public int ID { get; set; }
    public string TypeName { get; set; }
    public string Description { get; set; }

    public RoomTypeDTO() { }

    public RoomTypeDTO(int iD, string typeName, string description)
    {
        ID = iD;
        TypeName = typeName;
        Description = description;
    }
}

