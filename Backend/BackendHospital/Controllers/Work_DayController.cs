
// =====================================================================
// Work_DayController
// Work_Day.View=128 / Create=256 / Edit=512 / Delete=1024 (كلها index=2)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/work_days")]
[ApiController]
public class Work_DayController : ControllerBase
{
    [HasPermission(0, 128)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllWork_Day")]
    public async Task<ActionResult> GetAllWork_Day(
        CancellationToken cancellationToken)
    {
        var result = await Work_DayService.GetAllWork_DayAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(0, 256)]
    [Audit("CreateWork_Day")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddWork_Day")]
    public async Task<ActionResult> AddWork_Day(
        [FromBody] Work_DayDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await Work_DayService.AddWork_DayAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetWork_DayByID), routeParamName: "id");
    }

    [HasPermission(0, 512)]
    [Audit("UpdateWork_Day")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateWork_DayByID")]
    public async Task<ActionResult> UpdateWork_DayByID(
        int id, [FromBody] Work_DayUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await Work_DayService.UpdateWork_DayByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(0, 1024)]
    [Audit("DeleteWork_Day")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteWork_DayByID")]
    public async Task<ActionResult> DeleteWork_DayByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await Work_DayService.DeleteWork_DayByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(0, 128)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetWork_DayByID")]
    public async Task<ActionResult> GetWork_DayByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await Work_DayService.GetWork_DayByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}
