
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class ApplicationLogsData
{
    public static async Task<ApiResult<List<ApplicationLogsViewDTO>>> GetAllApplicationLogsAsync(
        CancellationToken cancellationToken = default)
    {
        var result = new ApiResult<List<ApplicationLogsViewDTO>>();
        var list = new List<ApplicationLogsViewDTO>();
        try
        {
            await using var connection = new SqlConnection(connectionString._connectionString);
            await using var command = new SqlCommand("SP_GetAllApplicationLogs", connection) { CommandType = CommandType.StoredProcedure };
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (reader.Read())
            {
                list.Add(new ApplicationLogsViewDTO
                {
                    ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                    UserID = reader.IsDBNull(reader.GetOrdinal("UserID")) ? 0 : reader.GetInt32(reader.GetOrdinal("UserID")),
                    LogLevel = reader.IsDBNull(reader.GetOrdinal("LogLevel")) ? null : reader.GetString(reader.GetOrdinal("LogLevel")),
                    LogMessage = reader.IsDBNull(reader.GetOrdinal("LogMessage")) ? null : reader.GetString(reader.GetOrdinal("LogMessage")),
                    Exception = reader.IsDBNull(reader.GetOrdinal("Exception")) ? null : reader.GetString(reader.GetOrdinal("Exception")),
                    IPAddress = reader.IsDBNull(reader.GetOrdinal("IPAddress")) ? null : reader.GetString(reader.GetOrdinal("IPAddress")),
                    CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
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

    public static async Task<ApiResult<ApplicationLogsDTO>> AddApplicationLogsAsync(
        ApplicationLogsDTO applicationlogs,
        CancellationToken cancellationToken = default)
    {
        var result = new ApiResult<ApplicationLogsDTO>();
        try
        {
            await using var connection = new SqlConnection(connectionString._connectionString);
            await using var command = new SqlCommand("SP_AddApplicationLogs", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = applicationlogs.UserID;
            command.Parameters.Add("@LogLevel", SqlDbType.NVarChar, 50).Value = (object?)applicationlogs.LogLevel ?? DBNull.Value;
            command.Parameters.Add("@LogMessage", SqlDbType.NVarChar, 50).Value = (object?)applicationlogs.LogMessage ?? DBNull.Value;
            command.Parameters.Add("@Exception", SqlDbType.NVarChar, 50).Value = (object?)applicationlogs.Exception ?? DBNull.Value;
            command.Parameters.Add("@IPAddress", SqlDbType.NVarChar, 50).Value = (object?)applicationlogs.IPAddress ?? DBNull.Value;
            var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
            var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(outputID);
            command.Parameters.Add(outputMsg);
            command.Parameters.Add(outputErrorType);
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            applicationlogs.ID = (int)outputID.Value;
            result.Data = applicationlogs;
            result.Message = outputMsg.Value?.ToString();
            result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
        }
        catch (Exception ex)
        {
            result.Data = null;
            result.Message = "Database error occurred while adding the applicationlogs.";
            result.ErrorType = ErrorType.DatabaseError;
        }
        return result;
    }

    public static async Task<ApiResult<ApplicationLogsViewDTO>> GetApplicationLogsByIDAsync(
        int ID,
        CancellationToken cancellationToken = default)
    {
        var result = new ApiResult<ApplicationLogsViewDTO>();
        var dto = new ApplicationLogsViewDTO();
        try
        {
            await using var connection = new SqlConnection(connectionString._connectionString);
            await using var command = new SqlCommand("SP_GetApplicationLogsByID", connection) { CommandType = CommandType.StoredProcedure };
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
                dto.UserID = reader.IsDBNull(reader.GetOrdinal("UserID")) ? 0 : reader.GetInt32(reader.GetOrdinal("UserID"));
                dto.LogLevel = reader.IsDBNull(reader.GetOrdinal("LogLevel")) ? null : reader.GetString(reader.GetOrdinal("LogLevel"));
                dto.LogMessage = reader.IsDBNull(reader.GetOrdinal("LogMessage")) ? null : reader.GetString(reader.GetOrdinal("LogMessage"));
                dto.Exception = reader.IsDBNull(reader.GetOrdinal("Exception")) ? null : reader.GetString(reader.GetOrdinal("Exception"));
                dto.IPAddress = reader.IsDBNull(reader.GetOrdinal("IPAddress")) ? null : reader.GetString(reader.GetOrdinal("IPAddress"));
                dto.CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            }
            reader.Close();
            result.Data = dto;
            result.Message = messageParameter.Value?.ToString();
            result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
        }
        catch (Exception ex)
        {
            result.Data = null;
            result.Message = "Database error occurred while fetching the applicationlogs";
            result.ErrorType = ErrorType.DatabaseError;
        }
        return result;
    }

}

