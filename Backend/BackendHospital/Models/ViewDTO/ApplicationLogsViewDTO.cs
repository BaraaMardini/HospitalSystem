using System.ComponentModel.DataAnnotations;

public class ApplicationLogsViewDTO
{
    public int ID { get; set; }
    public int UserID { get; set; }
    public string LogLevel { get; set; }
    public string LogMessage { get; set; }
    public string Exception { get; set; }
    public string IPAddress { get; set; }
    public DateTime CreatedAt { get; set; }

    public ApplicationLogsViewDTO() { }

    public ApplicationLogsViewDTO(int iD, int userID, string logLevel, string logMessage, string exception, string iPAddress, DateTime createdAt)
    {
        ID = iD;
        UserID = userID;
        LogLevel = logLevel;
        LogMessage = logMessage;
        Exception = exception;
        IPAddress = iPAddress;
        CreatedAt = createdAt;
    }
}

