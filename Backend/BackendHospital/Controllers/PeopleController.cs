
// =====================================================================
// PeopleController
// People.View=70368744177664 / Create=140737488355328 / Edit=281474976710656 / Delete=562949953421312 (index=1)
// (كان في Audit على Create/Delete بس ناقص Update، ضفتو)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/peoples")]
[ApiController]
public class PeopleController : ControllerBase
{
    [HasPermission(70368744177664, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllPeople")]
    public async Task<ActionResult> GetAllPeople(
        CancellationToken cancellationToken)
    {
        var result = await PeopleService.GetAllPeopleAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(140737488355328, 0)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Audit("CreatePerson")]
    [HttpPost(Name = "AddPeople")]
    public async Task<ActionResult> AddPeople(
    [FromBody] PeopleDTO dto,
    CancellationToken cancellationToken)
    {
        var result = await PeopleService.AddPeopleAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetPeopleByID), routeParamName: "id");
    }

    [HasPermission(281474976710656, 0)]
    [Audit("UpdatePerson")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdatePeopleByID")]
    public async Task<ActionResult> UpdatePeopleByID(
        int id, [FromBody] PeopleUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await PeopleService.UpdatePeopleByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(562949953421312, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Audit("DeletePerson")]
    [HttpDelete("{id}", Name = "DeletePeopleByID")]
    public async Task<ActionResult> DeletePeopleByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await PeopleService.DeletePeopleByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(70368744177664, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchPeople")]
    public async Task<ActionResult> SearchPeople(
       [FromQuery] SearchPeopleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await PeopleService.SearchPeople(
            request.ID, request.Name,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(70368744177664, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetPeopleByID")]
    public async Task<ActionResult> GetPeopleByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await PeopleService.GetPeopleByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}
