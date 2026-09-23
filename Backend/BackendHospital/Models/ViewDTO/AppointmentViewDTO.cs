using System.ComponentModel.DataAnnotations;

public class AppointmentViewDTO
{
    public int ID { get; set; }
    public string DoctorName { get; set; }
    public string PatientName { get; set; }
    public string StatusName { get; set; }
    public string RoomNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    public AppointmentViewDTO() { }

    public AppointmentViewDTO(int iD, string doctorName, string patientName, string statusName, string roomNumber, DateTime startDate, DateTime endDate, string notes, DateTime createdAt)
    {
        ID = iD;
        DoctorName = doctorName;
        PatientName = patientName;
        StatusName = statusName;
        RoomNumber = roomNumber;
        StartDate = startDate;
        EndDate = endDate;
        Notes = notes;
        CreatedAt = createdAt;
    }
}

