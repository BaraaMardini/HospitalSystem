using System.ComponentModel.DataAnnotations;

public class RoomViewDTO
{
    public int ID { get; set; }
    public string RoomNumber { get; set; }
    public string Notes { get; set; }
    public string RoomTypeDescription { get; set; }

    public bool IsActive { get; set; }

    public RoomViewDTO() { }

    public RoomViewDTO(int iD, string roomNumber, string notes, string roomTypeDescription,  bool isActive)
    {
        ID = iD;
        RoomNumber = roomNumber;
        Notes = notes;
        RoomTypeDescription = roomTypeDescription;
       
        IsActive = isActive;
    }
}

