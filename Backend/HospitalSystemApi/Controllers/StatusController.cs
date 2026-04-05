
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route("api/statuss")]
[ApiController]
public class StatusController : ControllerBase
{
  [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("all", Name = "GetAllStatus")]
public async Task<ActionResult> GetAllStatus(
    CancellationToken cancellationToken)
{
    var result = await StatusService.GetAllStatusAsync(cancellationToken);
    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}

 [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("{statusid}", Name = "GetStatusByStatusID")]
public async Task<ActionResult> GetStatusByStatusID(
    int statusid,
    CancellationToken cancellationToken)
{
    var result = await StatusService.GetStatusByStatusIDAsync(
        statusid,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
       [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPost(Name = "AddStatus")]
public async Task<ActionResult> AddStatus(
    [FromBody] StatusDTO dto,
    CancellationToken cancellationToken)
{
    var result = await StatusService.AddStatusAsync(dto, cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(
        this,
        result.ErrorType,
        result,
        newID: result.Data?.StatusID,
        routeName: nameof(GetStatusByStatusID),routeParamName:"statusid" );
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPut("{statusid}", Name = "UpdateStatusByStatusID")]
public async Task<ActionResult> UpdateStatusByStatusID(
    int statusid, [FromBody] StatusUpdateDTO dto,
    CancellationToken cancellationToken)  
{  
      dto.StatusID=statusid;

    var result = await StatusService.UpdateStatusByStatusIDAsync(
      dto,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpDelete("{statusid}", Name = "DeleteStatusByStatusID")]
public async Task<ActionResult> DeleteStatusByStatusID(
    int statusid, CancellationToken cancellationToken)
{

    var result = await StatusService.DeleteStatusByStatusIDAsync(
        statusid
        , cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
}

