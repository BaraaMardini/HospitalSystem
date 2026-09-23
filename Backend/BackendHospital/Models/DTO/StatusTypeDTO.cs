using System.ComponentModel.DataAnnotations;

public class StatusTypeDTO
{
    public int ID { get; set; }
    public string StatusTypeCode { get; set; }

    public StatusTypeDTO() { }

    public StatusTypeDTO(int iD, string statusTypeCode)
    {
        ID = iD;
        StatusTypeCode = statusTypeCode;
    }
}

