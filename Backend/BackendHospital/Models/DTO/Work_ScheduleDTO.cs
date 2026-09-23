using System.ComponentModel.DataAnnotations;

public class Work_ScheduleDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid DoctorID.")]
    public int DoctorID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid DayID.")]
    public int DayID { get; set; }

    public Work_ScheduleDTO() { }

    public Work_ScheduleDTO(int iD, int doctorID, int dayID)
    {
        ID = iD;
        DoctorID = doctorID;
        DayID = dayID;
    }
}

