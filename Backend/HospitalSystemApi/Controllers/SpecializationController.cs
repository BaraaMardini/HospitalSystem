
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route("api/specializations")]
[ApiController]
public class SpecializationController : ControllerBase
{
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
        routeName: nameof(GetSpecializationByid),routeParamName:"id" );
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPut("{id}", Name = "UpdateSpecializationByid")]
public async Task<ActionResult> UpdateSpecializationByid(
    int id, [FromBody] SpecializationUpdateDTO dto,
    CancellationToken cancellationToken)  
{  
      dto.id=id;

    var result = await SpecializationService.UpdateSpecializationByidAsync(
      dto,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
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
}

