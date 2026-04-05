
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route("api/roombookingss")]
[ApiController]
public class RoomBookingsController : ControllerBase
{
  [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("all", Name = "GetAllRoomBookings")]
public async Task<ActionResult> GetAllRoomBookings(
    CancellationToken cancellationToken)
{
    var result = await RoomBookingsService.GetAllRoomBookingsAsync(cancellationToken);
    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}

 [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("{appointmentid}", Name = "GetRoomBookingsByAppointmentID")]
public async Task<ActionResult> GetRoomBookingsByAppointmentID(
    int appointmentid,
    CancellationToken cancellationToken)
{
    var result = await RoomBookingsService.GetRoomBookingsByAppointmentIDAsync(
        appointmentid,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
     
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPut("{bookingid}", Name = "UpdateRoomBookingsByBookingID")]
public async Task<ActionResult> UpdateRoomBookingsByBookingID(
    int bookingid, [FromBody] RoomBookingsUpdateDTO dto,
    CancellationToken cancellationToken)  
{  
      dto.BookingID=bookingid;

    var result = await RoomBookingsService.UpdateRoomBookingsByBookingIDAsync(
      dto,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpDelete("{bookingid}", Name = "DeleteRoomBookingsByBookingID")]
public async Task<ActionResult> DeleteRoomBookingsByBookingID(
    int bookingid, CancellationToken cancellationToken)
{

    var result = await RoomBookingsService.DeleteRoomBookingsByBookingIDAsync(
        bookingid
        , cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
}

