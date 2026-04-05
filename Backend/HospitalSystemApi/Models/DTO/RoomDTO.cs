using System.ComponentModel.DataAnnotations;

public class RoomDTO
{
    public int ID { get; set; }
    public string RoomNumber { get; set; }
    public string Notes { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid RoomTypeID.")]
    public int RoomTypeID { get; set; }
   
    public bool IsActive { get; set; }

    public RoomDTO() { }

    public RoomDTO(int iD, string roomNumber, string notes, int roomTypeID, bool isActive)
    {
        ID = iD;
        RoomNumber = roomNumber;
        Notes = notes;
        RoomTypeID = roomTypeID;
        IsActive = isActive;
    }
}

