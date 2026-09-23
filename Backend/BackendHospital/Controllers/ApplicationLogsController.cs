
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route("api/applicationlogss")]
[ApiController]
public class ApplicationLogsController : ControllerBase
{
  [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("all", Name = "GetAllApplicationLogs")]
public async Task<ActionResult> GetAllApplicationLogs(
    CancellationToken cancellationToken)
{
    var result = await ApplicationLogsService.GetAllApplicationLogsAsync(cancellationToken);
    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
   
 [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("{id}", Name = "GetApplicationLogsByID")]
public async Task<ActionResult> GetApplicationLogsByID(
    int id,
    CancellationToken cancellationToken)
{
    var result = await ApplicationLogsService.GetApplicationLogsByIDAsync(
        id,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
}

