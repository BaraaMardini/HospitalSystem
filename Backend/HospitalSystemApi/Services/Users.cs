
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



public static async Task<ApiResult<UsersViewDTO>> GetUsersByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await UsersData.GetUsersByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<UsersDTO>> AddUsersAsync(
    UsersDTO dto,
    CancellationToken cancellationToken = default)
{
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


public static async Task<ApiResult<UsersUpdateDTO>> UpdateUsersByIDAsync(
    UsersUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<UsersUpdateDTO>
        {
            Data = null,
            Message = "Invalid Users data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await UsersData.UpdateUsersByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<UsersDTO>> DeleteUsersByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await UsersData.DeleteUsersByIDAsync(
        ID,
        cancellationToken);
}

}

