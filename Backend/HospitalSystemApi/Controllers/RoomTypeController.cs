
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route("api/roomtypes")]
[ApiController]
public class RoomTypeController : ControllerBase
{
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

 [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("{roomtypeid}", Name = "GetRoomTypeByRoomTypeID")]
public async Task<ActionResult> GetRoomTypeByRoomTypeID(
    int roomtypeid,
    CancellationToken cancellationToken)
{
    var result = await RoomTypeService.GetRoomTypeByRoomTypeIDAsync(
        roomtypeid,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
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
        newID: result.Data?.RoomTypeID,
        routeName: nameof(GetRoomTypeByRoomTypeID),routeParamName:"roomtypeid" );
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPut("{roomtypeid}", Name = "UpdateRoomTypeByRoomTypeID")]
public async Task<ActionResult> UpdateRoomTypeByRoomTypeID(
    int roomtypeid, [FromBody] RoomTypeUpdateDTO dto,
    CancellationToken cancellationToken)  
{  
      dto.RoomTypeID=roomtypeid;

    var result = await RoomTypeService.UpdateRoomTypeByRoomTypeIDAsync(
      dto,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpDelete("{roomtypeid}", Name = "DeleteRoomTypeByRoomTypeID")]
public async Task<ActionResult> DeleteRoomTypeByRoomTypeID(
    int roomtypeid, CancellationToken cancellationToken)
{

    var result = await RoomTypeService.DeleteRoomTypeByRoomTypeIDAsync(
        roomtypeid
        , cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
}

