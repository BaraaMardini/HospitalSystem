using System.ComponentModel.DataAnnotations;

public class StatusViewDTO
{
    public int ID { get; set; }
    public string StatusName { get; set; }
    public string Description { get; set; }
    public int StatusTypeID { get; set; }
    public string StatusTypeCode { get; set; }
    public bool IsActive { get; set; }

    public StatusViewDTO() { }

    public StatusViewDTO(int iD, string statusName, string description, int statusTypeID, string statusTypeCode, bool isActive)
    {
        ID = iD;
        StatusName = statusName;
        Description = description;
        StatusTypeID = statusTypeID;
        StatusTypeCode = statusTypeCode;
        IsActive = isActive;
    }
}

