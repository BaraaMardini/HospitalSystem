using System.ComponentModel.DataAnnotations;

public class StatusDTO
{
    public int ID { get; set; }
    public string StatusName { get; set; }
    public string Description { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid StatusTypeID.")]
    public int StatusTypeID { get; set; }
    public bool IsActive { get; set; }

    public StatusDTO() { }

    public StatusDTO(int iD, string statusName, string description, int statusTypeID, bool isActive)
    {
        ID = iD;
        StatusName = statusName;
        Description = description;
        StatusTypeID = statusTypeID;
        IsActive = isActive;
    }
}

