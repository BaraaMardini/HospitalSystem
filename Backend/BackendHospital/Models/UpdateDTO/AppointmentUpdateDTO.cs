using System.ComponentModel.DataAnnotations;

public class AppointmentUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public int?  DoctorID { get; set; }
    public DateTime?  StartDate { get; set; }
    public string?  Notes { get; set; }
    public int?  StatusID { get; set; }
    public DateTime?  EndDate { get; set; }
    public int?  RoomID { get; set; }

    public AppointmentUpdateDTO() { }

    public AppointmentUpdateDTO(int iD, int patientID, int doctorID, DateTime startDate, string notes, DateTime createdAt, int statusID, DateTime endDate, int roomID)
    {
        DoctorID = doctorID;
        StartDate = startDate;
        Notes = notes;
        StatusID = statusID;
        EndDate = endDate;
        RoomID = roomID;
    }
}

