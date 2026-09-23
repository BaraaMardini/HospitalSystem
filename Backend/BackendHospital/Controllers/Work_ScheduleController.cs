
// =====================================================================
// Work_ScheduleController
// Work_Schedule.View=2048 / Create=4096 / Edit=8192 / Delete=16384 (كلها index=2)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/work_schedules")]
[ApiController]
public class Work_ScheduleController : ControllerBase
{
    [HasPermission(0, 2048)]
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

    [HasPermission(0, 4096)]
    [Audit("CreateWork_Schedule")]
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
            routeName: nameof(GetWork_ScheduleByID), routeParamName: "id");
    }

    [HasPermission(0, 8192)]
    [Audit("UpdateWork_Schedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateWork_ScheduleByID")]
    public async Task<ActionResult> UpdateWork_ScheduleByID(
        int id, [FromBody] Work_ScheduleUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await Work_ScheduleService.UpdateWork_ScheduleByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(0, 16384)]
    [Audit("DeleteWork_Schedule")]
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

    [HasPermission(0, 2048)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchWork_Schedule")]
    public async Task<ActionResult> SearchWork_Schedule(
       [FromQuery] SearchWork_ScheduleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Work_ScheduleService.SearchWork_Schedule(
            request.DayName, request.DoctorID, request.PeopleName,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(0, 2048)]
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
}
