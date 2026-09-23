
// =====================================================================
// RoleController
// Role.View=4 / Create=8 / Edit=16 / Delete=32 (index=1)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/roles")]
[ApiController]
public class RoleController : ControllerBase
{
    [HasPermission(4, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllRole")]
    public async Task<ActionResult> GetAllRole(
        CancellationToken cancellationToken)
    {
        var result = await RoleService.GetAllRoleAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(8, 0)]
    [Audit("CreateRole")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddRole")]
    public async Task<ActionResult> AddRole(
        [FromBody] RoleDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await RoleService.AddRoleAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetRoleByID), routeParamName: "id");
    }

    [HasPermission(16, 0)]
    [Audit("UpdateRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateRoleByID")]
    public async Task<ActionResult> UpdateRoleByID(
        int id, [FromBody] RoleUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await RoleService.UpdateRoleByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(32, 0)]
    [Audit("DeleteRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteRoleByID")]
    public async Task<ActionResult> DeleteRoleByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await RoleService.DeleteRoleByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(4, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetRoleByID")]
    public async Task<ActionResult> GetRoleByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await RoleService.GetRoleByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}
