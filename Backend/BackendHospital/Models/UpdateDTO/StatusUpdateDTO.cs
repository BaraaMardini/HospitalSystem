using System.ComponentModel.DataAnnotations;

public class StatusUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  StatusName { get; set; }
    public string?  Description { get; set; }
    public int?  StatusTypeID { get; set; }
    public bool?  IsActive { get; set; }

    public StatusUpdateDTO() { }

    public StatusUpdateDTO(int iD, string statusName, string description, int statusTypeID, bool isActive)
    {
        StatusName = statusName;
        Description = description;
        StatusTypeID = statusTypeID;
        IsActive = isActive;
    }
}

