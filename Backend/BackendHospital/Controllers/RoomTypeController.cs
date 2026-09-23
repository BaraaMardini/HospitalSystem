
// =====================================================================
// RoomTypeController
// RoomType.View=18014398509481984 / Create=36028797018963968 / Edit=72057594037927936 / Delete=144115188075855872 (index=1)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/roomtypes")]
[ApiController]
public class RoomTypeController : ControllerBase
{
    [HasPermission(18014398509481984, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllRoomType")]
    public async Task<ActionResult> GetAllRoomType(
        CancellationToken cancellationToken)
    {
        var result = await RoomTypeService.GetAllRoomTypeAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(36028797018963968, 0)]
    [Audit("CreateRoomType")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddRoomType")]
    public async Task<ActionResult> AddRoomType(
        [FromBody] RoomTypeDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await RoomTypeService.AddRoomTypeAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetRoomTypeByID), routeParamName: "id");
    }

    [HasPermission(72057594037927936, 0)]
    [Audit("UpdateRoomType")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateRoomTypeByID")]
    public async Task<ActionResult> UpdateRoomTypeByID(
        int id, [FromBody] RoomTypeUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await RoomTypeService.UpdateRoomTypeByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(144115188075855872, 0)]
    [Audit("DeleteRoomType")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteRoomTypeByID")]
    public async Task<ActionResult> DeleteRoomTypeByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await RoomTypeService.DeleteRoomTypeByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(18014398509481984, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetRoomTypeByID")]
    public async Task<ActionResult> GetRoomTypeByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await RoomTypeService.GetRoomTypeByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}
