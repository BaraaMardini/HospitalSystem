
// =====================================================================
// SecurityLogsController
// !! TODO: مو موجودة إطلاقاً بالجدول يلي بعتيتو -> ما حطيت HasPermission
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/securitylogss")]
[ApiController]
public class SecurityLogsController : ControllerBase
{
    // TODO: HasPermission غير معرّف
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllSecurityLogs")]
    public async Task<ActionResult> GetAllSecurityLogs(
        CancellationToken cancellationToken)
    {
        var result = await SecurityLogsService.GetAllSecurityLogsAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    // TODO: HasPermission غير معرّف
    [Audit("CreateSecurityLogs")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddSecurityLogs")]
    public async Task<ActionResult> AddSecurityLogs(
        [FromBody] SecurityLogsDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await SecurityLogsService.AddSecurityLogsAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetSecurityLogsByID), routeParamName: "id");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetSecurityLogsByID")]
    public async Task<ActionResult> GetSecurityLogsByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await SecurityLogsService.GetSecurityLogsByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}