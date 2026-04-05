using System.ComponentModel.DataAnnotations;

public class AppointmentUpdateDTO
{
    [Required]
    public int ID { get; set; }
    public int? PatientID { get; set; }
    public int? DoctorID { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public string? Notes { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? RoomID { get; set; }
    public int? CreatedBy { get; set; }
    public int? StatusID { get; set; }

    public AppointmentUpdateDTO() { }

    public AppointmentUpdateDTO(int iD, int patientID, int doctorID, DateTime appointmentDate, string notes, DateTime createdAt, int roomID, int createdBy, int statusID)
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
