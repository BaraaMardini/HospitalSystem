
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class UsersService
{

public static async Task<ApiResult<List<UsersViewDTO>>> GetAllUsersAsync(
    CancellationToken cancellationToken = default)
{
    var result = await UsersData.GetAllUsersAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<UsersViewDTO>>(
            null,
            "No Userss found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<UsersViewDTO>>(
        result.Data,
        "Userss retrieved successfully.",
        ErrorType.None
    );
}


public static async Task<ApiResult<UsersDTO>> AddUsersAsync(
    UsersDTO dto,
    CancellationToken cancellationToken = default)
{
        string PassHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash); 
        
        dto.PasswordHash = PassHash;
    if (dto == null)
    {
        return new ApiResult<UsersDTO>
        {
            Data = null,
            Message = "Users cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await UsersData.AddUsersAsync(dto, cancellationToken);
}


public static async Task<ApiResult<UsersDTO>> UpdateUsersByUsernameAndPasswordHashAsync(
    UsersUpdateDTO dto,
    CancellationToken cancellationToken = default)
{

    if (dto == null )
    {
        return new ApiResult<UsersDTO>
        {
            Data = null,
            Message = "Invalid Users data.",
            ErrorType = ErrorType.InvalidId
        };
    }
        string PassHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
        dto.PasswordHash= PassHash;

        return await UsersData.UpdateUsersByUsernameAndPasswordHashAsync(dto, cancellationToken);
}


public static async Task<ApiResult<UsersDTO>> DeleteUsersByUsernameAndPasswordHashAsync(
    string Username, string PasswordHash,
    CancellationToken cancellationToken = default)
{
        string PassHash = BCrypt.Net.BCrypt.HashPassword(PasswordHash);


        return await UsersData.DeleteUsersByUsernameAndPasswordHashAsync(
        Username, PassHash,
        cancellationToken);
}


public static async Task<ApiResult<List<UsersViewDTO>>> SearchUsers(
    string? Username, bool? IsActive,
    CancellationToken cancellationToken = default)
{
    var result = await UsersData.SearchUsers(
        Username, IsActive,
        cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<UsersViewDTO>>(
            null,
            "No Userss found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<UsersViewDTO>>(
        result.Data,
        "Userss retrieved successfully.",
        ErrorType.None
    );
}


public static async Task<ApiResult<UsersViewDTO>> GetUsersByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await UsersData.GetUsersByIDAsync(
        ID,
        cancellationToken);
}

}

