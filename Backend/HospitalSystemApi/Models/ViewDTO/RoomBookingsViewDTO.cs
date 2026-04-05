using System.ComponentModel.DataAnnotations;

public class RoomBookingsViewDTO
{
    public int BookingID { get; set; }
    public int AppointmentID { get; set; }
    public string RoomNumber { get; set; }
    public string DescriptionStatus { get; set; }
    public string PatientName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public RoomBookingsViewDTO() { }

    public RoomBookingsViewDTO(int bookingID, int appointmentID, string roomNumber, string descriptionStatus, string patientName, DateTime startDate, DateTime endDate)
    {
        BookingID = bookingID;
        AppointmentID = appointmentID;
        RoomNumber = roomNumber;
        DescriptionStatus = descriptionStatus;
        PatientName = patientName;
        StartDate = startDate;
        EndDate = endDate;
    }
}

