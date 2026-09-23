
// =====================================================================
// MedicalHistoryController
// MedicalHistory.View=274877906944 / Create=549755813888 / Edit=1099511627776 / Delete=2199023255552 (index=1)
 using Microsoft.AspNetCore.Mvc;


[Route("api/medicalhistorys")]
[ApiController]
public class MedicalHistoryController : ControllerBase
{
    [HasPermission(274877906944, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllMedicalHistory")]
    public async Task<ActionResult> GetAllMedicalHistory(
        CancellationToken cancellationToken)
    {
        var result = await MedicalHistoryService.GetAllMedicalHistoryAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(549755813888, 0)]
    [Audit("CreateMedicalHistory")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddMedicalHistory")]
    public async Task<ActionResult> AddMedicalHistory(
        [FromBody] MedicalHistoryDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await MedicalHistoryService.AddMedicalHistoryAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetMedicalHistoryByID), routeParamName: "id");
    }

    [HasPermission(1099511627776, 0)]
    [Audit("UpdateMedicalHistory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateMedicalHistoryByID")]
    public async Task<ActionResult> UpdateMedicalHistoryByID(
        int id, [FromBody] MedicalHistoryUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await MedicalHistoryService.UpdateMedicalHistoryByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(2199023255552, 0)]
    [Audit("DeleteMedicalHistory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteMedicalHistoryByID")]
    public async Task<ActionResult> DeleteMedicalHistoryByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await MedicalHistoryService.DeleteMedicalHistoryByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(274877906944, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchMedicalHistory")]
    public async Task<ActionResult> SearchMedicalHistory(
       [FromQuery] SearchMedicalHistoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await MedicalHistoryService.SearchMedicalHistory(
            request.PersonName, request.PatientID, request.DiagnosisDate,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(274877906944, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetMedicalHistoryByID")]
    public async Task<ActionResult> GetMedicalHistoryByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await MedicalHistoryService.GetMedicalHistoryByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}
