
// =====================================================================
// DoctorController
// Doctor.View=16384 / Create=32768 / Edit=65536 / Delete=131072  (index=1)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/doctors")]
[ApiController]
public class DoctorController : ControllerBase
{
    [HasPermission(16384, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllDoctor")]
    public async Task<ActionResult> GetAllDoctor(
        CancellationToken cancellationToken)
    {
        var result = await DoctorService.GetAllDoctorAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(32768, 0)]
    [Audit("CreateDoctor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddDoctor")]
    public async Task<ActionResult> AddDoctor(
        [FromBody] DoctorDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await DoctorService.AddDoctorAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetDoctorByID), routeParamName: "id");
    }

    [HasPermission(65536, 0)]
    [Audit("UpdateDoctor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateDoctorByID")]
    public async Task<ActionResult> UpdateDoctorByID(
        int id, [FromBody] DoctorUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await DoctorService.UpdateDoctorByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(131072, 0)]
    [Audit("DeleteDoctor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteDoctorByID")]
    public async Task<ActionResult> DeleteDoctorByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await DoctorService.DeleteDoctorByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(16384, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchDoctor")]
    public async Task<ActionResult> SearchDoctor(
       [FromQuery] SearchDoctorRequest request,
        CancellationToken cancellationToken)
    {
        var result = await DoctorService.SearchDoctor(
            request.PersonName, request.DepartmentName, request.SpecializationName,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(16384, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetDoctorByID")]
    public async Task<ActionResult> GetDoctorByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await DoctorService.GetDoctorByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}