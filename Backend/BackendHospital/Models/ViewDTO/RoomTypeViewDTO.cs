using System.ComponentModel.DataAnnotations;

public class RoomTypeViewDTO
{
    public int ID { get; set; }
    public string TypeName { get; set; }
    public string Description { get; set; }

    public RoomTypeViewDTO() { }

    public RoomTypeViewDTO(int iD, string typeName, string description)
    {
        ID = iD;
        TypeName = typeName;
        Description = description;
    }
}

