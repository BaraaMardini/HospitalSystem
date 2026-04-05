using System.ComponentModel.DataAnnotations;

public class Work_DayDTO
{
    public int ID { get; set; }
    public string DayName { get; set; }

    public Work_DayDTO() { }

    public Work_DayDTO(int iD, string dayName)
    {
        ID = iD;
        DayName = dayName;
    }
}

