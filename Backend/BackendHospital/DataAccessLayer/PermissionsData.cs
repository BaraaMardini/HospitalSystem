
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class PermissionsData
{
public static async Task<ApiResult<List<PermissionsViewDTO>>> GetAllPermissionsAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<PermissionsViewDTO>>();
    var list = new List<PermissionsViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllPermissions", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new PermissionsViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                Code = reader.IsDBNull(reader.GetOrdinal("Code")) ? null : reader.GetString(reader.GetOrdinal("Code")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                ModuleName = reader.IsDBNull(reader.GetOrdinal("ModuleName")) ? null : reader.GetString(reader.GetOrdinal("ModuleName")),
                ActionName = reader.IsDBNull(reader.GetOrdinal("ActionName")) ? null : reader.GetString(reader.GetOrdinal("ActionName")),
                IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean(reader.GetOrdinal("IsActive"))
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

public static async Task<ApiResult<PermissionsDTO>> AddPermissionsAsync(
    PermissionsDTO permissions,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<PermissionsDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddPermissions", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = (object?)permissions.Code ?? DBNull.Value;
        command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = (object?)permissions.Name ?? DBNull.Value;
        command.Parameters.Add("@ModuleName", SqlDbType.NVarChar, 50).Value = (object?)permissions.ModuleName ?? DBNull.Value;
        command.Parameters.Add("@ActionName", SqlDbType.NVarChar, 50).Value = (object?)permissions.ActionName ?? DBNull.Value;
        command.Parameters.Add("@BitIndex", SqlDbType.Int).Value = permissions.BitIndex;
        command.Parameters.Add("@BitValue", SqlDbType.BigInt).Value = permissions.BitValue;
        command.Parameters.Add("@MaskNumber", SqlDbType.TinyInt).Value = permissions.MaskNumber;
        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = permissions.IsActive;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        permissions.ID = (int)outputID.Value;
        result.Data = permissions;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the permissions.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<PermissionsDTO>> UpdatePermissionsByIDAsync(
    PermissionsUpdateDTO permissions,
    CancellationToken cancellationToken = default)
{
PermissionsDTO permissionsDTO = new  PermissionsDTO();
    var result = new ApiResult<PermissionsDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdatePermissionsByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)permissions.ID ?? DBNull.Value;
        command.Parameters.Add("@Name", SqlDbType.NVarChar, 500).Value = (object?)permissions.Name ?? DBNull.Value;
        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = (object?)permissions.IsActive ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            permissionsDTO.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            permissionsDTO.Code = reader.IsDBNull(reader.GetOrdinal("Code")) ? null : reader.GetString(reader.GetOrdinal("Code"));
            permissionsDTO.Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name"));
            permissionsDTO.ModuleName = reader.IsDBNull(reader.GetOrdinal("ModuleName")) ? null : reader.GetString(reader.GetOrdinal("ModuleName"));
            permissionsDTO.ActionName = reader.IsDBNull(reader.GetOrdinal("ActionName")) ? null : reader.GetString(reader.GetOrdinal("ActionName"));
            permissionsDTO.BitIndex = reader.IsDBNull(reader.GetOrdinal("BitIndex")) ? 0 : reader.GetInt32(reader.GetOrdinal("BitIndex"));
            permissionsDTO.BitValue = reader.IsDBNull(reader.GetOrdinal("BitValue"))  ? 0 : reader.GetInt64(reader.GetOrdinal("BitValue"));
                permissionsDTO.MaskNumber = reader.IsDBNull(reader.GetOrdinal("MaskNumber")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaskNumber"));
            permissionsDTO.IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean(reader.GetOrdinal("IsActive"));
        }
        reader.Close();
        result.Data = permissionsDTO;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the permissions";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<PermissionsDTO>> DeletePermissionsByIDAsync(
    int iD,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<PermissionsDTO>();
    var permissions = new PermissionsDTO
    {
        ID = iD,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeletePermissionsByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = permissions.ID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = permissions;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the permissions";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<List<PermissionsViewDTO>>> SearchPermissions(
string? ModuleName,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<PermissionsViewDTO>>();
    var list = new List<PermissionsViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_SearchPermissions", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ModuleName", SqlDbType.NVarChar).Value = ModuleName ;
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new PermissionsViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                Code = reader.IsDBNull(reader.GetOrdinal("Code")) ? null : reader.GetString(reader.GetOrdinal("Code")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                ModuleName = reader.IsDBNull(reader.GetOrdinal("ModuleName")) ? null : reader.GetString(reader.GetOrdinal("ModuleName")),
                ActionName = reader.IsDBNull(reader.GetOrdinal("ActionName")) ? null : reader.GetString(reader.GetOrdinal("ActionName")),
                IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean(reader.GetOrdinal("IsActive"))
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

public static async Task<ApiResult<PermissionsViewDTO>> GetPermissionsByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<PermissionsViewDTO>();
    var dto = new PermissionsViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetPermissionsByID", connection) { CommandType = CommandType.StoredProcedure };
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
            dto.Code = reader.IsDBNull(reader.GetOrdinal("Code")) ? null : reader.GetString(reader.GetOrdinal("Code"));
            dto.Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name"));
            dto.ModuleName = reader.IsDBNull(reader.GetOrdinal("ModuleName")) ? null : reader.GetString(reader.GetOrdinal("ModuleName"));
            dto.ActionName = reader.IsDBNull(reader.GetOrdinal("ActionName")) ? null : reader.GetString(reader.GetOrdinal("ActionName"));
            dto.IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean(reader.GetOrdinal("IsActive"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the permissions";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

