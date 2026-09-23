// =====================================================================
// EmployeeController
// Employee.View=262144 / Create=524288 / Edit=1048576 / Delete=2097152  (index=1)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/employees")]
[ApiController]
public class EmployeeController : ControllerBase
{
    [HasPermission(262144, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllEmployee")]
    public async Task<ActionResult> GetAllEmployee(
        CancellationToken cancellationToken)
    {
        var result = await EmployeeService.GetAllEmployeeAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(524288, 0)]
    [Audit("CreateEmployee")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddEmployee")]
    public async Task<ActionResult> AddEmployee(
        [FromBody] EmployeeDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await EmployeeService.AddEmployeeAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetEmployeeByID), routeParamName: "id");
    }

    [HasPermission(1048576, 0)]
    [Audit("UpdateEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateEmployeeByID")]
    public async Task<ActionResult> UpdateEmployeeByID(
        int id, [FromBody] EmployeeUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await EmployeeService.UpdateEmployeeByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(2097152, 0)]
    [Audit("DeleteEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteEmployeeByID")]
    public async Task<ActionResult> DeleteEmployeeByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await EmployeeService.DeleteEmployeeByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(262144, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchEmployee")]
    public async Task<ActionResult> SearchEmployee(
       [FromQuery] SearchEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await EmployeeService.SearchEmployee(
            request.Name, request.RoleName, request.DepartmentName, request.StatusName,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(262144, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetEmployeeByID")]
    public async Task<ActionResult> GetEmployeeByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await EmployeeService.GetEmployeeByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}