
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class RoomTypeData
{
public static async Task<ApiResult<List<RoomTypeViewDTO>>> GetAllRoomTypeAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<RoomTypeViewDTO>>();
    var list = new List<RoomTypeViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllRoomType", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new RoomTypeViewDTO
            {
                RoomTypeID = reader.IsDBNull(reader.GetOrdinal("RoomTypeID")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoomTypeID")),
                TypeName = reader.IsDBNull(reader.GetOrdinal("TypeName")) ? null : reader.GetString(reader.GetOrdinal("TypeName")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"))
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

public static async Task<ApiResult<RoomTypeViewDTO>> GetRoomTypeByRoomTypeIDAsync(
    int RoomTypeID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<RoomTypeViewDTO>();
    var dto = new RoomTypeViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetRoomTypeByRoomTypeID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@RoomTypeID", SqlDbType.Int).Value = RoomTypeID;
        var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var errorTypeParameter = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(messageParameter);
        command.Parameters.Add(errorTypeParameter);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            dto.RoomTypeID = reader.IsDBNull(reader.GetOrdinal("RoomTypeID")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoomTypeID"));
            dto.TypeName = reader.IsDBNull(reader.GetOrdinal("TypeName")) ? null : reader.GetString(reader.GetOrdinal("TypeName"));
            dto.Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the roomtype";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<RoomTypeDTO>> AddRoomTypeAsync(
    RoomTypeDTO roomtype,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<RoomTypeDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddRoomType", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@TypeName", SqlDbType.NVarChar, 50).Value = (object?)roomtype.TypeName ?? DBNull.Value;
        command.Parameters.Add("@Description", SqlDbType.NVarChar, 50).Value = (object?)roomtype.Description ?? DBNull.Value;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        roomtype.RoomTypeID = (int)outputID.Value;
        result.Data = roomtype;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the roomtype.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<RoomTypeUpdateDTO>> UpdateRoomTypeByRoomTypeIDAsync(
    RoomTypeUpdateDTO roomtype,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<RoomTypeUpdateDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateRoomTypeByRoomTypeID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@RoomTypeID", SqlDbType.Int).Value = (object?)roomtype.RoomTypeID ?? DBNull.Value;
        command.Parameters.Add("@TypeName", SqlDbType.NVarChar, 500).Value = (object?)roomtype.TypeName ?? DBNull.Value;
        command.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = (object?)roomtype.Description ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            roomtype.RoomTypeID = reader.IsDBNull(reader.GetOrdinal("RoomTypeID")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoomTypeID"));
            roomtype.TypeName = reader.IsDBNull(reader.GetOrdinal("TypeName")) ? null : reader.GetString(reader.GetOrdinal("TypeName"));
            roomtype.Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"));
        }
        reader.Close();
        result.Data = roomtype;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the roomtype";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<RoomTypeDTO>> DeleteRoomTypeByRoomTypeIDAsync(
    int roomTypeID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<RoomTypeDTO>();
    var roomtype = new RoomTypeDTO
    {
        RoomTypeID = roomTypeID,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteRoomTypeByRoomTypeID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@RoomTypeID", SqlDbType.Int).Value = roomtype.RoomTypeID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = roomtype;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the roomtype";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

