using System.ComponentModel.DataAnnotations;

public class AppointmentDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid PatientID.")]
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid RoomID.")]
    public int RoomID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid CreatedBy.")]
    public int CreatedBy { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid StatusID.")]
    public int StatusID { get; set; }

    public DateTime StartDate{ get; set; }

    public DateTime EndDate { get; set; }

    public AppointmentDTO() { }

    public AppointmentDTO(int iD, int patientID, int doctorID, DateTime appointmentDate, string notes, DateTime createdAt, int roomID, int createdBy, int statusID, DateTime StartDate )
    {
        ID = iD;
        PatientID = patientID;
        DoctorID = doctorID;
        AppointmentDate = appointmentDate;
        Notes = notes;
        CreatedAt = createdAt;
        RoomID = roomID;
        CreatedBy = createdBy;
        StatusID = statusID;
    }
}

