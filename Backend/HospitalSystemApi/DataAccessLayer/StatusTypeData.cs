
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class StatusTypeData
{
public static async Task<ApiResult<List<StatusTypeViewDTO>>> GetAllStatusTypeAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<StatusTypeViewDTO>>();
    var list = new List<StatusTypeViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllStatusType", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new StatusTypeViewDTO
            {
                StatusTypeID = reader.IsDBNull(reader.GetOrdinal("StatusTypeID")) ? 0 : reader.GetInt32(reader.GetOrdinal("StatusTypeID")),
                StatusTypeCode = reader.IsDBNull(reader.GetOrdinal("StatusTypeCode")) ? null : reader.GetString(reader.GetOrdinal("StatusTypeCode"))
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

public static async Task<ApiResult<StatusTypeViewDTO>> GetStatusTypeByStatusTypeIDAsync(
    int StatusTypeID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<StatusTypeViewDTO>();
    var dto = new StatusTypeViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetStatusTypeByStatusTypeID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@StatusTypeID", SqlDbType.Int).Value = StatusTypeID;
        var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var errorTypeParameter = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(messageParameter);
        command.Parameters.Add(errorTypeParameter);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            dto.StatusTypeID = reader.IsDBNull(reader.GetOrdinal("StatusTypeID")) ? 0 : reader.GetInt32(reader.GetOrdinal("StatusTypeID"));
            dto.StatusTypeCode = reader.IsDBNull(reader.GetOrdinal("StatusTypeCode")) ? null : reader.GetString(reader.GetOrdinal("StatusTypeCode"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the statustype";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<StatusTypeDTO>> AddStatusTypeAsync(
    StatusTypeDTO statustype,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<StatusTypeDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddStatusType", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@StatusTypeCode", SqlDbType.NVarChar, 50).Value = (object?)statustype.StatusTypeCode ?? DBNull.Value;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        statustype.StatusTypeID = (int)outputID.Value;
        result.Data = statustype;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the statustype.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<StatusTypeUpdateDTO>> UpdateStatusTypeByStatusTypeIDAsync(
    StatusTypeUpdateDTO statustype,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<StatusTypeUpdateDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateStatusTypeByStatusTypeID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@StatusTypeID", SqlDbType.Int).Value = (object?)statustype.StatusTypeID ?? DBNull.Value;
        command.Parameters.Add("@StatusTypeCode", SqlDbType.NVarChar, 500).Value = (object?)statustype.StatusTypeCode ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            statustype.StatusTypeID = reader.IsDBNull(reader.GetOrdinal("StatusTypeID")) ? 0 : reader.GetInt32(reader.GetOrdinal("StatusTypeID"));
            statustype.StatusTypeCode = reader.IsDBNull(reader.GetOrdinal("StatusTypeCode")) ? null : reader.GetString(reader.GetOrdinal("StatusTypeCode"));
        }
        reader.Close();
        result.Data = statustype;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the statustype";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<StatusTypeDTO>> DeleteStatusTypeByStatusTypeIDAsync(
    int statusTypeID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<StatusTypeDTO>();
    var statustype = new StatusTypeDTO
    {
        StatusTypeID = statusTypeID,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteStatusTypeByStatusTypeID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@StatusTypeID", SqlDbType.Int).Value = statustype.StatusTypeID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = statustype;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the statustype";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

