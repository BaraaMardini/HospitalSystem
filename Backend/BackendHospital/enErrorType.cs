
public enum ErrorType
{
    None = 0,
    InvalidId = 1,
    NotFound = 2,
    AlreadyExists = 3,
    LogicalDependency = 4,
    DatabaseError = 5
}

public static class ErrorTypeMapper
{
    public static ErrorType GetErrorType(int errorCodeFromDb)
    {
        return errorCodeFromDb switch
        {
            0 => ErrorType.None,
            1 => ErrorType.InvalidId,
            2 => ErrorType.NotFound,
            3 => ErrorType.AlreadyExists,
            4 => ErrorType.LogicalDependency,
            _ => ErrorType.DatabaseError,
        };
    }
}
