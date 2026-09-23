
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class RoomTypeService
{

public static async Task<ApiResult<List<RoomTypeViewDTO>>> GetAllRoomTypeAsync(
    CancellationToken cancellationToken = default)
{
    var result = await RoomTypeData.GetAllRoomTypeAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<RoomTypeViewDTO>>(
            null,
            "No RoomTypes found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<RoomTypeViewDTO>>(
        result.Data,
        "RoomTypes retrieved successfully.",
        ErrorType.None
    );
}


public static async Task<ApiResult<RoomTypeDTO>> AddRoomTypeAsync(
    RoomTypeDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<RoomTypeDTO>
        {
            Data = null,
            Message = "RoomType cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await RoomTypeData.AddRoomTypeAsync(dto, cancellationToken);
}


public static async Task<ApiResult<RoomTypeDTO>> UpdateRoomTypeByIDAsync(
    RoomTypeUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<RoomTypeDTO>
        {
            Data = null,
            Message = "Invalid RoomType data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await RoomTypeData.UpdateRoomTypeByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<RoomTypeDTO>> DeleteRoomTypeByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await RoomTypeData.DeleteRoomTypeByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<RoomTypeViewDTO>> GetRoomTypeByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await RoomTypeData.GetRoomTypeByIDAsync(
        ID,
        cancellationToken);
}

}

