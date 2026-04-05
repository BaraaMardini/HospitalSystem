
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class RoomService
{

public static async Task<ApiResult<List<RoomViewDTO>>> GetAllRoomAsync(
    CancellationToken cancellationToken = default)
{
    var result = await RoomData.GetAllRoomAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<RoomViewDTO>>(
            null,
            "No Rooms found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<RoomViewDTO>>(
        result.Data,
        "Rooms retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<RoomViewDTO>> GetRoomByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await RoomData.GetRoomByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<RoomDTO>> AddRoomAsync(
    RoomDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<RoomDTO>
        {
            Data = null,
            Message = "Room cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await RoomData.AddRoomAsync(dto, cancellationToken);
}


public static async Task<ApiResult<RoomUpdateDTO>> UpdateRoomByIDAsync(
    RoomUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<RoomUpdateDTO>
        {
            Data = null,
            Message = "Invalid Room data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await RoomData.UpdateRoomByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<RoomDTO>> DeleteRoomByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await RoomData.DeleteRoomByIDAsync(
        ID,
        cancellationToken);
}

}

