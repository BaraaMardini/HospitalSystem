
// =====================================================================
// PatientController
// Patient.View=4398046511104 / Create=8796093022208 / Edit=17592186044416 / Delete=35184372088832 (index=1)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/patients")]
[ApiController]
public class PatientController : ControllerBase
{
    [HasPermission(4398046511104, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllPatient")]
    public async Task<ActionResult> GetAllPatient(
        CancellationToken cancellationToken)
    {
        var result = await PatientService.GetAllPatientAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(8796093022208, 0)]
    [Audit("CreatePatient")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddPatient")]
    public async Task<ActionResult> AddPatient(
        [FromBody] PatientDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await PatientService.AddPatientAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetPatientByID), routeParamName: "id");
    }

    [HasPermission(17592186044416, 0)]
    [Audit("UpdatePatient")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdatePatientByID")]
    public async Task<ActionResult> UpdatePatientByID(
        int id, [FromBody] PatientUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await PatientService.UpdatePatientByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(35184372088832, 0)]
    [Audit("DeletePatient")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeletePatientByID")]
    public async Task<ActionResult> DeletePatientByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await PatientService.DeletePatientByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(4398046511104, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchPatient")]
    public async Task<ActionResult> SearchPatient(
       [FromQuery] SearchPatientRequest request,
        CancellationToken cancellationToken)
    {
        var result = await PatientService.SearchPatient(
            request.PersonName, request.PersonID,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(4398046511104, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetPatientByID")]
    public async Task<ActionResult> GetPatientByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await PatientService.GetPatientByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}
