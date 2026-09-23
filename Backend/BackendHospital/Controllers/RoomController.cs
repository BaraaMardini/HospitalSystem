
// =====================================================================
// RoomController
// Room.View=1125899906842624 / Create=2251799813685248 / Edit=4503599627370496 / Delete=9007199254740992 (index=1)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/rooms")]
[ApiController]
public class RoomController : ControllerBase
{
    [HasPermission(1125899906842624, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllRoom")]
    public async Task<ActionResult> GetAllRoom(
        CancellationToken cancellationToken)
    {
        var result = await RoomService.GetAllRoomAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(2251799813685248, 0)]
    [Audit("CreateRoom")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddRoom")]
    public async Task<ActionResult> AddRoom(
        [FromBody] RoomDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await RoomService.AddRoomAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetRoomByID), routeParamName: "id");
    }

    [HasPermission(4503599627370496, 0)]
    [Audit("UpdateRoom")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateRoomByID")]
    public async Task<ActionResult> UpdateRoomByID(
        int id, [FromBody] RoomUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await RoomService.UpdateRoomByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(9007199254740992, 0)]
    [Audit("DeleteRoom")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteRoomByID")]
    public async Task<ActionResult> DeleteRoomByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await RoomService.DeleteRoomByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(1125899906842624, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchRoom")]
    public async Task<ActionResult> SearchRoom(
       [FromQuery] SearchRoomRequest request,
        CancellationToken cancellationToken)
    {
        var result = await RoomService.SearchRoom(
            request.RoomNumber, request.TypeName,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(1125899906842624, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetRoomByID")]
    public async Task<ActionResult> GetRoomByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await RoomService.GetRoomByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}

