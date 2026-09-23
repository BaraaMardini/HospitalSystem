using System.ComponentModel.DataAnnotations;

public class Work_ScheduleViewDTO
{
    public int ID { get; set; }
    public string DayName { get; set; }
    public int DoctorID { get; set; }
    public string PeopleName { get; set; }

    public Work_ScheduleViewDTO() { }

    public Work_ScheduleViewDTO(int iD, string dayName, int doctorID, string peopleName)
    {
        ID = iD;
        DayName = dayName;
        DoctorID = doctorID;
        PeopleName = peopleName;
    }
}

