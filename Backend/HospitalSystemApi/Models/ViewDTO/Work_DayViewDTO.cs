using System.ComponentModel.DataAnnotations;

public class Work_DayViewDTO
{
    public int ID { get; set; }
    public string DayName { get; set; }

    public Work_DayViewDTO() { }

    public Work_DayViewDTO(int iD, string dayName)
    {
        ID = iD;
        DayName = dayName;
    }
}

