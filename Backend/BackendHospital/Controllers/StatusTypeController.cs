
// =====================================================================
// StatusTypeController
// StatusType.View=8 / Create=16 / Edit=32 / Delete=64 (كلها index=2)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/statustypes")]
[ApiController]
public class StatusTypeController : ControllerBase
{
    [HasPermission(0, 8)]
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

    [HasPermission(0, 16)]
    [Audit("CreateStatusType")]
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
            newID: result.Data?.ID,
            routeName: nameof(GetStatusTypeByID), routeParamName: "id");
    }

    [HasPermission(0, 32)]
    [Audit("UpdateStatusType")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateStatusTypeByID")]
    public async Task<ActionResult> UpdateStatusTypeByID(
        int id, [FromBody] StatusTypeUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await StatusTypeService.UpdateStatusTypeByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(0, 64)]
    [Audit("DeleteStatusType")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteStatusTypeByID")]
    public async Task<ActionResult> DeleteStatusTypeByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await StatusTypeService.DeleteStatusTypeByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(0, 8)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetStatusTypeByID")]
    public async Task<ActionResult> GetStatusTypeByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await StatusTypeService.GetStatusTypeByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}
