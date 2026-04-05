using System.ComponentModel.DataAnnotations;

public class RoomBookingsUpdateDTO
{
 [Required]
    public int  BookingID { get; set; }
    public int?  AppointmentID { get; set; }
    public DateTime?  StartDate { get; set; }
    public DateTime?  EndDate { get; set; }
    public int?  StatusID { get; set; }

    public RoomBookingsUpdateDTO() { }

    public RoomBookingsUpdateDTO(int bookingID, int appointmentID, DateTime startDate, DateTime endDate, int statusID)
    {
        BookingID = bookingID;
        AppointmentID = appointmentID;
        StartDate = startDate;
        EndDate = endDate;
        StatusID = statusID;
    }
}

