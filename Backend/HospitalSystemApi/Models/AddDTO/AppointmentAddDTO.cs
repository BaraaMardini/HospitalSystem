using System.ComponentModel.DataAnnotations;

public class AppointmentAddDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid PatientID.")]
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Notes { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid RoomID.")]
    public int RoomID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid CreatedBy.")]
    public int CreatedBy { get; set; }
    


    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public AppointmentAddDTO() { }

    public AppointmentAddDTO(int iD, int patientID, int doctorID, DateTime appointmentDate, string notes,  int roomID, int createdBy,  DateTime StartDate, DateTime EndDate)
    {
        ID = iD;
        PatientID = patientID;
        DoctorID = doctorID;
        AppointmentDate = appointmentDate;
        Notes = notes;
        RoomID = roomID;
        CreatedBy = createdBy;
        this.StartDate = StartDate;
        this.EndDate = EndDate;
    }
}

