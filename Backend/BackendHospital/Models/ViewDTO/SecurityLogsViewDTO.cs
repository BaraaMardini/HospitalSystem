using System.ComponentModel.DataAnnotations;

public class SecurityLogsViewDTO
{
    public int ID { get; set; }
    public int UserID { get; set; }
    public string Action { get; set; }
    public string Description { get; set; }
    public string IPAddress { get; set; }
    public DateTime CreatedAt { get; set; }

    public SecurityLogsViewDTO() { }

    public SecurityLogsViewDTO(int iD, int userID, string action, string description, string iPAddress, DateTime createdAt)
    {
        ID = iD;
        UserID = userID;
        Action = action;
        Description = description;
        IPAddress = iPAddress;
        CreatedAt = createdAt;
    }
}

