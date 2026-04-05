using System.ComponentModel.DataAnnotations;

public class Work_DayUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  DayName { get; set; }

    public Work_DayUpdateDTO() { }

    public Work_DayUpdateDTO(int iD, string dayName)
    {
        ID = iD;
        DayName = dayName;
    }
}

