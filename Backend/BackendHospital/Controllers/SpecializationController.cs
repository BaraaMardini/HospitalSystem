
// =====================================================================
// SpecializationController
// Specialization.View=288230376151711744 / Create=576460752303423488 / Edit=1152921504606846976 / Delete=2305843009213693952 (index=1)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/specializations")]
[ApiController]
public class SpecializationController : ControllerBase
{
    [HasPermission(288230376151711744, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllSpecialization")]
    public async Task<ActionResult> GetAllSpecialization(
        CancellationToken cancellationToken)
    {
        var result = await SpecializationService.GetAllSpecializationAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(576460752303423488, 0)]
    [Audit("CreateSpecialization")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddSpecialization")]
    public async Task<ActionResult> AddSpecialization(
        [FromBody] SpecializationDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await SpecializationService.AddSpecializationAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.id,
            routeName: nameof(GetSpecializationByid), routeParamName: "id");
    }

    [HasPermission(1152921504606846976, 0)]
    [Audit("UpdateSpecialization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateSpecializationByid")]
    public async Task<ActionResult> UpdateSpecializationByid(
        int id, [FromBody] SpecializationUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.id = id;
        var result = await SpecializationService.UpdateSpecializationByidAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(2305843009213693952, 0)]
    [Audit("DeleteSpecialization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteSpecializationByid")]
    public async Task<ActionResult> DeleteSpecializationByid(
        int id, CancellationToken cancellationToken)
    {
        var result = await SpecializationService.DeleteSpecializationByidAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(288230376151711744, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetSpecializationByid")]
    public async Task<ActionResult> GetSpecializationByid(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await SpecializationService.GetSpecializationByidAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}
