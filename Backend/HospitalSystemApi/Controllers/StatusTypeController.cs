
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route("api/statustypes")]
[ApiController]
public class StatusTypeController : ControllerBase
{
  [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("all", Name = "GetAllStatusType")]
public async Task<ActionResult> GetAllStatusType(
    CancellationToken cancellationToken)
{
    var result = await StatusTypeService.GetAllStatusTypeAsync(cancellationToken);
    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}

 [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("{statustypeid}", Name = "GetStatusTypeByStatusTypeID")]
public async Task<ActionResult> GetStatusTypeByStatusTypeID(
    int statustypeid,
    CancellationToken cancellationToken)
{
    var result = await StatusTypeService.GetStatusTypeByStatusTypeIDAsync(
        statustypeid,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
       [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPost(Name = "AddStatusType")]
public async Task<ActionResult> AddStatusType(
    [FromBody] StatusTypeDTO dto,
    CancellationToken cancellationToken)
{
    var result = await StatusTypeService.AddStatusTypeAsync(dto, cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(
        this,
        result.ErrorType,
        result,
        newID: result.Data?.StatusTypeID,
        routeName: nameof(GetStatusTypeByStatusTypeID),routeParamName:"statustypeid" );
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPut("{statustypeid}", Name = "UpdateStatusTypeByStatusTypeID")]
public async Task<ActionResult> UpdateStatusTypeByStatusTypeID(
    int statustypeid, [FromBody] StatusTypeUpdateDTO dto,
    CancellationToken cancellationToken)  
{  
      dto.StatusTypeID=statustypeid;

    var result = await StatusTypeService.UpdateStatusTypeByStatusTypeIDAsync(
      dto,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpDelete("{statustypeid}", Name = "DeleteStatusTypeByStatusTypeID")]
public async Task<ActionResult> DeleteStatusTypeByStatusTypeID(
    int statustypeid, CancellationToken cancellationToken)
{

    var result = await StatusTypeService.DeleteStatusTypeByStatusTypeIDAsync(
        statustypeid
        , cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
}

