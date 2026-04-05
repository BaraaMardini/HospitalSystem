
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route("api/medicalhistorys")]
[ApiController]
public class MedicalHistoryController : ControllerBase
{
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
    [HttpGet("name/{name}", Name = "GetAllMedicalHistoryByName")]
    public async Task<ActionResult> GetAllMedicalHistoryByName(
      string name,
      CancellationToken cancellationToken)
    {
        var result = await MedicalHistoryService.GetAllMedicalHistoryByNameAsync(
            name,
            cancellationToken);

        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }


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
        routeName: nameof(GetMedicalHistoryByID),routeParamName:"id" );
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPut("{id}", Name = "UpdateMedicalHistoryByID")]
public async Task<ActionResult> UpdateMedicalHistoryByID(
    int id, [FromBody] MedicalHistoryUpdateDTO dto,
    CancellationToken cancellationToken)  
{  
      dto.ID=id;

    var result = await MedicalHistoryService.UpdateMedicalHistoryByIDAsync(
      dto,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
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
}

