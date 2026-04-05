
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class RoomBookingsService
{

public static async Task<ApiResult<List<RoomBookingsViewDTO>>> GetAllRoomBookingsAsync(
    CancellationToken cancellationToken = default)
{
    var result = await RoomBookingsData.GetAllRoomBookingsAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<RoomBookingsViewDTO>>(
            null,
            "No RoomBookingss found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<RoomBookingsViewDTO>>(
        result.Data,
        "RoomBookingss retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<RoomBookingsViewDTO>> GetRoomBookingsByAppointmentIDAsync(
    int AppointmentID,
    CancellationToken cancellationToken = default)
{
  
    return await RoomBookingsData.GetRoomBookingsByAppointmentIDAsync(
        AppointmentID,
        cancellationToken);
}





public static async Task<ApiResult<RoomBookingsUpdateDTO>> UpdateRoomBookingsByBookingIDAsync(
    RoomBookingsUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<RoomBookingsUpdateDTO>
        {
            Data = null,
            Message = "Invalid RoomBookings data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await RoomBookingsData.UpdateRoomBookingsByBookingIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<RoomBookingsDTO>> DeleteRoomBookingsByBookingIDAsync(
    int BookingID,
    CancellationToken cancellationToken = default)
{
   

    return await RoomBookingsData.DeleteRoomBookingsByBookingIDAsync(
        BookingID,
        cancellationToken);
}

}

