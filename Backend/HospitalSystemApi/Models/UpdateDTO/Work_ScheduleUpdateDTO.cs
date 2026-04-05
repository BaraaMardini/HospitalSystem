using System.ComponentModel.DataAnnotations;

public class Work_ScheduleUpdateDTO
{
 [Required]
    public int?  DoctorID { get; set; }
    public int?  DayID { get; set; }
    public int  ID { get; set; }

    public Work_ScheduleUpdateDTO() { }

    public Work_ScheduleUpdateDTO(int doctorID, int dayID, int iD)
    {
        DoctorID = doctorID;
        DayID = dayID;
        ID = iD;
    }
}

