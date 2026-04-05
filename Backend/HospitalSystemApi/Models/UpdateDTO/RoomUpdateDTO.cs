using System.ComponentModel.DataAnnotations;

public class RoomUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  RoomNumber { get; set; }
    public string?  Notes { get; set; }
    public int?  RoomTypeID { get; set; }
    public bool?  IsActive { get; set; }

    public RoomUpdateDTO() { }

    public RoomUpdateDTO(int iD, string roomNumber, string notes, int roomTypeID,bool isActive)
    {
        ID = iD;
        RoomNumber = roomNumber;
        Notes = notes;
        RoomTypeID = roomTypeID;
      
        IsActive = isActive;
    }
}

