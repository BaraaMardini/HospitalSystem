using System.ComponentModel.DataAnnotations;

public class Work_ScheduleDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Invalid DoctorID.")]
    public int DoctorID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid DayID.")]
    public int DayID { get; set; }
    public int ID { get; set; }

    public Work_ScheduleDTO() { }

    public Work_ScheduleDTO(int doctorID, int dayID, int iD)
    {
        DoctorID = doctorID;
        DayID = dayID;
        ID = iD;
    }
}

