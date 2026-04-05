
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class EmployeeService
{

public static async Task<ApiResult<List<EmployeeViewDTO>>> GetAllEmployeeAsync(
    CancellationToken cancellationToken = default)
{
    var result = await EmployeeData.GetAllEmployeeAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<EmployeeViewDTO>>(
            null,
            "No Employees found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<EmployeeViewDTO>>(
        result.Data,
        "Employees retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<EmployeeViewDTO>> GetEmployeeByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await EmployeeData.GetEmployeeByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<EmployeeDTO>> AddEmployeeAsync(
    EmployeeDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<EmployeeDTO>
        {
            Data = null,
            Message = "Employee cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await EmployeeData.AddEmployeeAsync(dto, cancellationToken);
}


public static async Task<ApiResult<EmployeeUpdateDTO>> UpdateEmployeeByIDAsync(
    EmployeeUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<EmployeeUpdateDTO>
        {
            Data = null,
            Message = "Invalid Employee data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await EmployeeData.UpdateEmployeeByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<EmployeeDTO>> DeleteEmployeeByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await EmployeeData.DeleteEmployeeByIDAsync(
        ID,
        cancellationToken);
}

}

