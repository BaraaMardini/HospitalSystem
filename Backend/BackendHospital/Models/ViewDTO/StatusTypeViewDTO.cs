using System.ComponentModel.DataAnnotations;

public class StatusTypeViewDTO
{
    public int ID { get; set; }
    public string StatusTypeCode { get; set; }

    public StatusTypeViewDTO() { }

    public StatusTypeViewDTO(int iD, string statusTypeCode)
    {
        ID = iD;
        StatusTypeCode = statusTypeCode;
    }
}

