
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class RoleService
{

public static async Task<ApiResult<List<RoleViewDTO>>> GetAllRoleAsync(
    CancellationToken cancellationToken = default)
{
    var result = await RoleData.GetAllRoleAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<RoleViewDTO>>(
            null,
            "No Roles found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<RoleViewDTO>>(
        result.Data,
        "Roles retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<RoleViewDTO>> GetRoleByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await RoleData.GetRoleByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<RoleDTO>> AddRoleAsync(
    RoleDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<RoleDTO>
        {
            Data = null,
            Message = "Role cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await RoleData.AddRoleAsync(dto, cancellationToken);
}


public static async Task<ApiResult<RoleUpdateDTO>> UpdateRoleByIDAsync(
    RoleUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<RoleUpdateDTO>
        {
            Data = null,
            Message = "Invalid Role data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await RoleData.UpdateRoleByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<RoleDTO>> DeleteRoleByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await RoleData.DeleteRoleByIDAsync(
        ID,
        cancellationToken);
}

}

