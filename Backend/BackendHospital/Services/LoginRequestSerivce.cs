using BackendHospital.Models;

public static class LoginRequestService
{
    public static async Task<ApiResult<UserInfo>> LoginRequest(
        LoginRequest dto,
        CancellationToken cancellationToken = default)
    {
        // ==========================================
        // Validate Request
        // ==========================================

        if (dto == null)
        {
            return new ApiResult<UserInfo>
            {
                Data = null,
                Message = "Login request is required.",
                ErrorType = ErrorType.InvalidId
            };
        }


        // ==========================================
        // Username Required
        // ==========================================

        if (string.IsNullOrWhiteSpace(
                dto.UserName))
        {
            return new ApiResult<UserInfo>
            {
                Data = null,
                Message = "Username is required.",
                ErrorType = ErrorType.InvalidId
            };
        }


        // ==========================================
        // Password Required
        // ==========================================

        if (string.IsNullOrWhiteSpace(
                dto.Password))
        {
            return new ApiResult<UserInfo>
            {
                Data = null,
                Message = "Password is required.",
                ErrorType = ErrorType.InvalidId
            };
        }


        // ==========================================
        // Get User From Database
        // ==========================================

        var result =
            await LoginRequestData.LoginRequest(
                dto,
                cancellationToken);


        // ==========================================
        // Database Error
        // ==========================================

        if (result.ErrorType ==
            ErrorType.DatabaseError)
        {
            return result;
        }


        // ==========================================
        // User Not Found
        // ==========================================

        if (result.Data == null)
        {
            return new ApiResult<UserInfo>
            {
                Data = null,
                Message =
                    "Invalid username or password.",
                ErrorType =
                    ErrorType.AlreadyExists
            };
        }


        // ==========================================
        // Validate Password Hash
        // ==========================================

        if (string.IsNullOrWhiteSpace(
                result.Data.PasswordHash))
        {
            return new ApiResult<UserInfo>
            {
                Data = null,
                Message =
                    "Invalid username or password.",
                ErrorType =
                    ErrorType.AlreadyExists
            };
        }


        // ==========================================
        // Verify Password
        // ==========================================

        bool isPasswordValid;

        try
        {
            isPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    result.Data.PasswordHash);
        }
        catch
        {
            return new ApiResult<UserInfo>
            {
                Data = null,
                Message =
                    "Invalid username or password.",
                ErrorType =
                    ErrorType.AlreadyExists
            };
        }


        // ==========================================
        // Invalid Password
        // ==========================================

        if (!isPasswordValid)
        {
            return new ApiResult<UserInfo>
            {
                Data = null,
                Message =
                    "Invalid username or password.",
                ErrorType =
                    ErrorType.AlreadyExists
            };
        }


        // ==========================================
        // Login Successful
        // ==========================================

        result.Data.PasswordHash = null;

        result.Message =
            "Login successful.";

        return result;
    }


    public static async Task<ApiResult<UserInfo>>
        GetUserByID(
            int userId,
            CancellationToken cancellationToken)
    {
        return await LoginRequestData
            .GetUserByID(
                userId,
                cancellationToken);
    }
}