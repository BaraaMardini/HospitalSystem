using System.ComponentModel.DataAnnotations;

public class Work_ScheduleUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public int?  DoctorID { get; set; }
    public int?  DayID { get; set; }

    public Work_ScheduleUpdateDTO() { }

    public Work_ScheduleUpdateDTO(int iD, int doctorID, int dayID)
    {
        DoctorID = doctorID;
        DayID = dayID;
    }
}

