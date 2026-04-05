using System.ComponentModel.DataAnnotations;

public class StatusTypeUpdateDTO
{
 [Required]
    public int  StatusTypeID { get; set; }
    public string?  StatusTypeCode { get; set; }

    public StatusTypeUpdateDTO() { }

    public StatusTypeUpdateDTO(int statusTypeID, string statusTypeCode)
    {
        StatusTypeID = statusTypeID;
        StatusTypeCode = statusTypeCode;
    }
}

