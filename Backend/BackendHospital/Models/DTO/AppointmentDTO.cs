using System.ComponentModel.DataAnnotations;

public class AppointmentDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid PatientID.")]
    public int PatientID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid DoctorID.")]
    public int DoctorID { get; set; }
    public DateTime StartDate { get; set; }
    public string Notes { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid StatusID.")]
    public int StatusID { get; set; }
    public DateTime EndDate { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid RoomID.")]
    public int RoomID { get; set; }

    public AppointmentDTO() { }

    public AppointmentDTO(int iD, int patientID, int doctorID, DateTime startDate, string notes,  int statusID, DateTime endDate, int roomID)
    {
        ID = iD;
        PatientID = patientID;
        DoctorID = doctorID;
        StartDate = startDate;
        Notes = notes;
        StatusID = statusID;
        EndDate = endDate;
        RoomID = roomID;
    }
}

