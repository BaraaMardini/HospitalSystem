
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class EmployeeData
{
public static async Task<ApiResult<List<EmployeeViewDTO>>> GetAllEmployeeAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<EmployeeViewDTO>>();
    var list = new List<EmployeeViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllEmployee", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new EmployeeViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID")),
                RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? null : reader.GetString(reader.GetOrdinal("RoleName")),
                DescriptionRole = reader.IsDBNull(reader.GetOrdinal("DescriptionRole")) ? null : reader.GetString(reader.GetOrdinal("DescriptionRole")),
                DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName")),
                Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Salary")),
                HireDate = reader.IsDBNull(reader.GetOrdinal("HireDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("HireDate")),
                StatusName = reader.IsDBNull(reader.GetOrdinal("StatusName")) ? null : reader.GetString(reader.GetOrdinal("StatusName")),
                DescriptionSatus = reader.IsDBNull(reader.GetOrdinal("DescriptionSatus")) ? null : reader.GetString(reader.GetOrdinal("DescriptionSatus"))
            });
        }
        result.Data = list;
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching data.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<EmployeeDTO>> AddEmployeeAsync(
    EmployeeDTO employee,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<EmployeeDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddEmployee", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@PersonID", SqlDbType.Int).Value = employee.PersonID;
        command.Parameters.Add("@RoleId", SqlDbType.Int).Value = employee.RoleId;
        command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = employee.DepartmentId;
        command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = employee.Salary;
        command.Parameters.Add("@StatusID", SqlDbType.Int).Value = employee.StatusID;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        employee.ID = (int)outputID.Value;
        result.Data = employee;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the employee.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<EmployeeDTO>> UpdateEmployeeByIDAsync(
    EmployeeUpdateDTO employee,
    CancellationToken cancellationToken = default)
{
EmployeeDTO employeeDTO = new  EmployeeDTO();
    var result = new ApiResult<EmployeeDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateEmployeeByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)employee.ID ?? DBNull.Value;
        command.Parameters.Add("@RoleId", SqlDbType.Int).Value = (object?)employee.RoleId ?? DBNull.Value;
        command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = (object?)employee.DepartmentId ?? DBNull.Value;
        command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = (object?)employee.Salary ?? DBNull.Value;
        command.Parameters.Add("@StatusID", SqlDbType.Int).Value = (object?)employee.StatusID ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            employeeDTO.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            employeeDTO.PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID"));
            employeeDTO.RoleId = reader.IsDBNull(reader.GetOrdinal("RoleId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoleId"));
            employeeDTO.DepartmentId = reader.IsDBNull(reader.GetOrdinal("DepartmentId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DepartmentId"));
            employeeDTO.Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Salary"));
            employeeDTO.StatusID = reader.IsDBNull(reader.GetOrdinal("StatusID")) ? 0 : reader.GetInt32(reader.GetOrdinal("StatusID"));
        }
        reader.Close();
        result.Data = employeeDTO;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the employee";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<EmployeeDTO>> DeleteEmployeeByIDAsync(
    int iD,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<EmployeeDTO>();
    var employee = new EmployeeDTO
    {
        ID = iD,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteEmployeeByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = employee.ID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = employee;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the employee";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<List<EmployeeViewDTO>>> SearchEmployee(
string? Name,
string? RoleName,
string? DepartmentName,
string? StatusName,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<EmployeeViewDTO>>();
    var list = new List<EmployeeViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_SearchEmployee", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name ;
        command.Parameters.Add("@RoleName", SqlDbType.NVarChar).Value = RoleName ;
        command.Parameters.Add("@DepartmentName", SqlDbType.NVarChar).Value = DepartmentName ;
        command.Parameters.Add("@StatusName", SqlDbType.NVarChar).Value = StatusName ;
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new EmployeeViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID")),
                RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? null : reader.GetString(reader.GetOrdinal("RoleName")),
                DescriptionRole = reader.IsDBNull(reader.GetOrdinal("DescriptionRole")) ? null : reader.GetString(reader.GetOrdinal("DescriptionRole")),
                DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName")),
                Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Salary")),
                HireDate = reader.IsDBNull(reader.GetOrdinal("HireDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("HireDate")),
                StatusName = reader.IsDBNull(reader.GetOrdinal("StatusName")) ? null : reader.GetString(reader.GetOrdinal("StatusName")),
                DescriptionSatus = reader.IsDBNull(reader.GetOrdinal("DescriptionSatus")) ? null : reader.GetString(reader.GetOrdinal("DescriptionSatus"))
            });
        }
        result.Data = list;
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching data.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<EmployeeViewDTO>> GetEmployeeByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<EmployeeViewDTO>();
    var dto = new EmployeeViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetEmployeeByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
        var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var errorTypeParameter = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(messageParameter);
        command.Parameters.Add(errorTypeParameter);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            dto.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            dto.Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name"));
            dto.PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID"));
            dto.RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? null : reader.GetString(reader.GetOrdinal("RoleName"));
            dto.DescriptionRole = reader.IsDBNull(reader.GetOrdinal("DescriptionRole")) ? null : reader.GetString(reader.GetOrdinal("DescriptionRole"));
            dto.DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName"));
            dto.Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Salary"));
            dto.HireDate = reader.IsDBNull(reader.GetOrdinal("HireDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("HireDate"));
            dto.StatusName = reader.IsDBNull(reader.GetOrdinal("StatusName")) ? null : reader.GetString(reader.GetOrdinal("StatusName"));
            dto.DescriptionSatus = reader.IsDBNull(reader.GetOrdinal("DescriptionSatus")) ? null : reader.GetString(reader.GetOrdinal("DescriptionSatus"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the employee";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

