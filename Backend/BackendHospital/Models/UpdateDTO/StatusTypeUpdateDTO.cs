using System.ComponentModel.DataAnnotations;

public class StatusTypeUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  StatusTypeCode { get; set; }

    public StatusTypeUpdateDTO() { }

    public StatusTypeUpdateDTO(int iD, string statusTypeCode)
    {
        StatusTypeCode = statusTypeCode;
    }
}

