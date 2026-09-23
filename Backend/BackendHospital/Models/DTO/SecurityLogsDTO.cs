using System.ComponentModel.DataAnnotations;

public class SecurityLogsDTO
{
    public int ID { get; set; }
    public int? UserID { get; set; }
    public string Action { get; set; }
    public string Description { get; set; }
    public string IPAddress { get; set; }

    public SecurityLogsDTO() { }

    public SecurityLogsDTO(int iD, int userID, string action, string description, string iPAddress)
    {
        ID = iD;
        UserID = userID;
        Action = action;
        Description = description;
        IPAddress = iPAddress;
    }
}

