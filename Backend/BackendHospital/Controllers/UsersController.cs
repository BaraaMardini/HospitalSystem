

// =====================================================================
// UsersController
// !! TODO: بالجدول بس Users.Create (mask=1, index=1) و Users.AssignPermissions (mask=2, index=1)
// ما في صلاحية View / Edit / Delete / Search معرّفة لليوزرز -> تركتها بدون HasPermission
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/userss")]
[ApiController]
public class UsersController : ControllerBase
{
    // TODO: HasPermission غير معرّف (مافي Users.View بالجدول)
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllUsers")]
    public async Task<ActionResult> GetAllUsers(
        CancellationToken cancellationToken)
    {
        var result = await UsersService.GetAllUsersAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(1, 0)]
    [Audit("CreateUsers")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddUsers")]
    public async Task<ActionResult> AddUsers(
        [FromBody] UsersDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await UsersService.AddUsersAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetUsersByID), routeParamName: "id");
    }

    // TODO: HasPermission غير معرّف (مافي Users.Edit بالجدول)
    [Audit("UpdateUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{username}/{passwordhash}", Name = "UpdateUsersByUsernameAndPasswordHash")]
    public async Task<ActionResult> UpdateUsersByUsernameAndPasswordHash(
        string username, string passwordhash, [FromBody] UsersUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.Username = username;
        dto.PasswordHash = passwordhash;
        var result = await UsersService.UpdateUsersByUsernameAndPasswordHashAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    // TODO: HasPermission غير معرّف (مافي Users.Delete بالجدول)
    [Audit("DeleteUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{username}/{passwordhash}", Name = "DeleteUsersByUsernameAndPasswordHash")]
    public async Task<ActionResult> DeleteUsersByUsernameAndPasswordHash(
        string username, string passwordhash, CancellationToken cancellationToken)
    {
        var result = await UsersService.DeleteUsersByUsernameAndPasswordHashAsync(
            username, passwordhash
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    // TODO: HasPermission غير معرّف
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchUsers")]
    public async Task<ActionResult> SearchUsers(
       [FromQuery] SearchUsersRequest request,
        CancellationToken cancellationToken)
    {
        var result = await UsersService.SearchUsers(
            request.Username, request.IsActive,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    // TODO: HasPermission غير معرّف
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetUsersByID")]
    public async Task<ActionResult> GetUsersByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await UsersService.GetUsersByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}

