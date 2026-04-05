using System.ComponentModel.DataAnnotations;

public class StatusTypeViewDTO
{
    public int StatusTypeID { get; set; }
    public string StatusTypeCode { get; set; }

    public StatusTypeViewDTO() { }

    public StatusTypeViewDTO(int statusTypeID, string statusTypeCode)
    {
        StatusTypeID = statusTypeID;
        StatusTypeCode = statusTypeCode;
    }
}

