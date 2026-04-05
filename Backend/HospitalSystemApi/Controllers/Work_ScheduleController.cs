
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route("api/work_schedules")]
[ApiController]
public class Work_ScheduleController : ControllerBase
{

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("doctorid/{doctorid}", Name = "GetAllWork_ScheduleByDoctorID")]
    public async Task<ActionResult> GetAllWork_ScheduleByDoctorID(
  int doctorid,
  CancellationToken cancellationToken)
    {
        var result = await Work_ScheduleService.GetAllWork_ScheduleByDoctorIDAsync(
            doctorid,
            cancellationToken);

        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("all", Name = "GetAllWork_Schedule")]
public async Task<ActionResult> GetAllWork_Schedule(
    CancellationToken cancellationToken)
{
    var result = await Work_ScheduleService.GetAllWork_ScheduleAsync(cancellationToken);
    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}


 [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("{id}", Name = "GetWork_ScheduleByID")]
public async Task<ActionResult> GetWork_ScheduleByID(
    int id,
    CancellationToken cancellationToken)
{
    var result = await Work_ScheduleService.GetWork_ScheduleByIDAsync(
        id,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
       [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPost(Name = "AddWork_Schedule")]
public async Task<ActionResult> AddWork_Schedule(
    [FromBody] Work_ScheduleDTO dto,
    CancellationToken cancellationToken)
{
    var result = await Work_ScheduleService.AddWork_ScheduleAsync(dto, cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(
        this,
        result.ErrorType,
        result,
        newID: result.Data?.ID,
        routeName: nameof(GetWork_ScheduleByID),routeParamName:"id" );
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPut("{id}", Name = "UpdateWork_ScheduleByID")]
public async Task<ActionResult> UpdateWork_ScheduleByID(
    int id, [FromBody] Work_ScheduleUpdateDTO dto,
    CancellationToken cancellationToken)  
{  
      dto.ID=id;

    var result = await Work_ScheduleService.UpdateWork_ScheduleByIDAsync(
      dto,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpDelete("{id}", Name = "DeleteWork_ScheduleByID")]
public async Task<ActionResult> DeleteWork_ScheduleByID(
    int id, CancellationToken cancellationToken)
{

    var result = await Work_ScheduleService.DeleteWork_ScheduleByIDAsync(
        id
        , cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
}

