
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class Work_ScheduleData
{
    public static async Task<ApiResult<List<Work_ScheduleViewDTO>>> GetAllWork_ScheduleByDoctorIDAsync(
    int DoctorID,
    CancellationToken cancellationToken = default)
    {
        var result = new ApiResult<List<Work_ScheduleViewDTO>>();
        var list = new List<Work_ScheduleViewDTO>();
        try
        {
            await using var connection = new SqlConnection(connectionString._connectionString);
            await using var command = new SqlCommand("SP_GetAllWork_ScheduleByDoctorID", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = DoctorID;
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (reader.Read())
            {
                list.Add(new Work_ScheduleViewDTO
                {
                    ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                    DayName = reader.IsDBNull(reader.GetOrdinal("DayName")) ? null : reader.GetString(reader.GetOrdinal("DayName")),
                    DoctorID = reader.IsDBNull(reader.GetOrdinal("DoctorID")) ? 0 : reader.GetInt32(reader.GetOrdinal("DoctorID")),
                    PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName"))
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
    public static async Task<ApiResult<List<Work_ScheduleViewDTO>>> GetAllWork_ScheduleAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<Work_ScheduleViewDTO>>();
    var list = new List<Work_ScheduleViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllWork_Schedule", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new Work_ScheduleViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                DayName = reader.IsDBNull(reader.GetOrdinal("DayName")) ? null : reader.GetString(reader.GetOrdinal("DayName")),
                DoctorID = reader.IsDBNull(reader.GetOrdinal("DoctorID")) ? 0 : reader.GetInt32(reader.GetOrdinal("DoctorID")),
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName"))
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

public static async Task<ApiResult<Work_ScheduleViewDTO>> GetWork_ScheduleByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<Work_ScheduleViewDTO>();
    var dto = new Work_ScheduleViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetWork_ScheduleByID", connection) { CommandType = CommandType.StoredProcedure };
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
            dto.DayName = reader.IsDBNull(reader.GetOrdinal("DayName")) ? null : reader.GetString(reader.GetOrdinal("DayName"));
            dto.DoctorID = reader.IsDBNull(reader.GetOrdinal("DoctorID")) ? 0 : reader.GetInt32(reader.GetOrdinal("DoctorID"));
            dto.PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the work_schedule";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<Work_ScheduleDTO>> AddWork_ScheduleAsync(
    Work_ScheduleDTO work_schedule,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<Work_ScheduleDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddWork_Schedule", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = work_schedule.DoctorID;
        command.Parameters.Add("@DayID", SqlDbType.Int).Value = work_schedule.DayID;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        work_schedule.ID = (int)outputID.Value;
        result.Data = work_schedule;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the work_schedule.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<Work_ScheduleUpdateDTO>> UpdateWork_ScheduleByIDAsync(
    Work_ScheduleUpdateDTO work_schedule,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<Work_ScheduleUpdateDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateWork_ScheduleByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = (object?)work_schedule.DoctorID ?? DBNull.Value;
        command.Parameters.Add("@DayID", SqlDbType.Int).Value = (object?)work_schedule.DayID ?? DBNull.Value;
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)work_schedule.ID ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            work_schedule.DoctorID = reader.IsDBNull(reader.GetOrdinal("DoctorID")) ? 0 : reader.GetInt32(reader.GetOrdinal("DoctorID"));
            work_schedule.DayID = reader.IsDBNull(reader.GetOrdinal("DayID")) ? 0 : reader.GetInt32(reader.GetOrdinal("DayID"));
            work_schedule.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
        }
        reader.Close();
        result.Data = work_schedule;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the work_schedule";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<Work_ScheduleDTO>> DeleteWork_ScheduleByIDAsync(
    int iD,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<Work_ScheduleDTO>();
    var work_schedule = new Work_ScheduleDTO
    {
        ID = iD,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteWork_ScheduleByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = work_schedule.ID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = work_schedule;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the work_schedule";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

