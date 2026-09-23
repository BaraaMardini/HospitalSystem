
// =====================================================================
// StatusController
// Status.View=4611686018427387904 (index=1)
// Status.Create=1 / Edit=2 / Delete=4 (index=2)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/statuss")]
[ApiController]
public class StatusController : ControllerBase
{
    [HasPermission(4611686018427387904, 0)]
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

    [HasPermission(0, 1)]
    [Audit("CreateStatus")]
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
            newID: result.Data?.ID,
            routeName: nameof(GetStatusByID), routeParamName: "id");
    }

    [HasPermission(0, 2)]
    [Audit("UpdateStatus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateStatusByID")]
    public async Task<ActionResult> UpdateStatusByID(
        int id, [FromBody] StatusUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await StatusService.UpdateStatusByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(0, 4)]
    [Audit("DeleteStatus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteStatusByID")]
    public async Task<ActionResult> DeleteStatusByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await StatusService.DeleteStatusByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(4611686018427387904, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchStatus")]
    public async Task<ActionResult> SearchStatus(
       [FromQuery] SearchStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await StatusService.SearchStatus(
            request.StatusTypeCode,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(4611686018427387904, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetStatusByID")]
    public async Task<ActionResult> GetStatusByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await StatusService.GetStatusByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}
