using System.ComponentModel.DataAnnotations;

public class RoomBookingsDTO
{
    public int BookingID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid AppointmentID.")]
    public int AppointmentID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid StatusID.")]
    public int StatusID { get; set; }

    public RoomBookingsDTO() { }

    public RoomBookingsDTO(int bookingID, int appointmentID, DateTime startDate, DateTime endDate, int statusID)
    {
        BookingID = bookingID;
        AppointmentID = appointmentID;
        StartDate = startDate;
        EndDate = endDate;
        StatusID = statusID;
    }
}

