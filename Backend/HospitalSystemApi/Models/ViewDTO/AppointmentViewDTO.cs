using System.ComponentModel.DataAnnotations;

public class AppointmentViewDTO
{
    public int ID { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string DoctorName { get; set; }
    public string PatientName { get; set; }
    public string DescriptionStatus { get; set; }
    public string RoomNumber { get; set; }

    public AppointmentViewDTO() { }

    public AppointmentViewDTO(int iD, DateTime appointmentDate, string notes, DateTime createdAt, string createdBy, string doctorName, string patientName, string descriptionStatus, string roomNumber)
    {
        ID = iD;
        AppointmentDate = appointmentDate;
        Notes = notes;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        DoctorName = doctorName;
        PatientName = patientName;
        DescriptionStatus = descriptionStatus;
        RoomNumber = roomNumber;
    }
}

