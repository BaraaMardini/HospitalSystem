using System.ComponentModel.DataAnnotations;

public class StatusTypeDTO
{
    public int StatusTypeID { get; set; }
    public string StatusTypeCode { get; set; }

    public StatusTypeDTO() { }

    public StatusTypeDTO(int statusTypeID, string statusTypeCode)
    {
        StatusTypeID = statusTypeID;
        StatusTypeCode = statusTypeCode;
    }
}

