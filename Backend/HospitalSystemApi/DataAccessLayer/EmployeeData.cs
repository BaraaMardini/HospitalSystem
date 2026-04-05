
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
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName")),
                DescriptionRole = reader.IsDBNull(reader.GetOrdinal("DescriptionRole")) ? null : reader.GetString(reader.GetOrdinal("DescriptionRole")),
                Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Salary")),
                HireDate = reader.IsDBNull(reader.GetOrdinal("HireDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("HireDate")),
                status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : reader.GetString(reader.GetOrdinal("status")),
                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email"))
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
            dto.PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName"));
            dto.DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName"));
            dto.DescriptionRole = reader.IsDBNull(reader.GetOrdinal("DescriptionRole")) ? null : reader.GetString(reader.GetOrdinal("DescriptionRole"));
            dto.Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Salary"));
            dto.HireDate = reader.IsDBNull(reader.GetOrdinal("HireDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("HireDate"));
            dto.status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : reader.GetString(reader.GetOrdinal("status"));
            dto.PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber"));
            dto.Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender"));
            dto.Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age"));
            dto.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email"));
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
        command.Parameters.Add("@HireDate", SqlDbType.DateTime).Value = employee.HireDate;
        command.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = (object?)employee.status ?? DBNull.Value;
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

public static async Task<ApiResult<EmployeeUpdateDTO>> UpdateEmployeeByIDAsync(
    EmployeeUpdateDTO employee,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<EmployeeUpdateDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateEmployeeByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)employee.ID ?? DBNull.Value;
        command.Parameters.Add("@PersonID", SqlDbType.Int).Value = (object?)employee.PersonID ?? DBNull.Value;
        command.Parameters.Add("@RoleId", SqlDbType.Int).Value = (object?)employee.RoleId ?? DBNull.Value;
        command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = (object?)employee.DepartmentId ?? DBNull.Value;
        command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = (object?)employee.Salary ?? DBNull.Value;
        command.Parameters.Add("@HireDate", SqlDbType.DateTime).Value = (object?)employee.HireDate ?? DBNull.Value;
        command.Parameters.Add("@status", SqlDbType.NVarChar, 500).Value = (object?)employee.status ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            employee.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            employee.PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID"));
            employee.RoleId = reader.IsDBNull(reader.GetOrdinal("RoleId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoleId"));
            employee.DepartmentId = reader.IsDBNull(reader.GetOrdinal("DepartmentId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DepartmentId"));
            employee.Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Salary"));
            employee.HireDate = reader.IsDBNull(reader.GetOrdinal("HireDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("HireDate"));
            employee.status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : reader.GetString(reader.GetOrdinal("status"));
        }
        reader.Close();
        result.Data = employee;
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

}

