
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class AppointmentData
{
public static async Task<ApiResult<List<AppointmentViewDTO>>> GetAllAppointmentAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<AppointmentViewDTO>>();
    var list = new List<AppointmentViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllAppointment", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new AppointmentViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                AppointmentDate = reader.IsDBNull(reader.GetOrdinal("AppointmentDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                DoctorName = reader.IsDBNull(reader.GetOrdinal("DoctorName")) ? null : reader.GetString(reader.GetOrdinal("DoctorName")),
                PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? null : reader.GetString(reader.GetOrdinal("PatientName")),
                DescriptionStatus = reader.IsDBNull(reader.GetOrdinal("DescriptionStatus")) ? null : reader.GetString(reader.GetOrdinal("DescriptionStatus")),
                RoomNumber = reader.IsDBNull(reader.GetOrdinal("RoomNumber")) ? null : reader.GetString(reader.GetOrdinal("RoomNumber"))
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

public static async Task<ApiResult<AppointmentViewDTO>> GetAppointmentByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<AppointmentViewDTO>();
    var dto = new AppointmentViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAppointmentByID", connection) { CommandType = CommandType.StoredProcedure };
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
            dto.AppointmentDate = reader.IsDBNull(reader.GetOrdinal("AppointmentDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("AppointmentDate"));
            dto.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
            dto.CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            dto.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy"));
            dto.DoctorName = reader.IsDBNull(reader.GetOrdinal("DoctorName")) ? null : reader.GetString(reader.GetOrdinal("DoctorName"));
            dto.PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? null : reader.GetString(reader.GetOrdinal("PatientName"));
            dto.DescriptionStatus = reader.IsDBNull(reader.GetOrdinal("DescriptionStatus")) ? null : reader.GetString(reader.GetOrdinal("DescriptionStatus"));
            dto.RoomNumber = reader.IsDBNull(reader.GetOrdinal("RoomNumber")) ? null : reader.GetString(reader.GetOrdinal("RoomNumber"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the appointment";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<AppointmentAddDTO>> AddAppointmentAsync(
    AppointmentAddDTO appointment,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<AppointmentAddDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddAppointment", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@PatientID", SqlDbType.Int).Value = appointment.PatientID;
        command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = appointment.DoctorID;
        command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = appointment.AppointmentDate;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 50).Value = (object?)appointment.Notes ?? DBNull.Value;
        command.Parameters.Add("@RoomID", SqlDbType.Int).Value = appointment.RoomID;
        command.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = appointment.CreatedBy;

            command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = appointment.StartDate;
            command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = appointment.EndDate;

            var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        appointment.ID = (int)outputID.Value;
        result.Data = appointment;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the appointment.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<AppointmentUpdateDTO>> UpdateAppointmentByIDAsync(
    AppointmentUpdateDTO appointment,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<AppointmentUpdateDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateAppointmentByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)appointment.ID ?? DBNull.Value;
        command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = (object?)appointment.DoctorID ?? DBNull.Value;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = (object?)appointment.Notes ?? DBNull.Value;
        command.Parameters.Add("@RoomID", SqlDbType.Int).Value = (object?)appointment.RoomID ?? DBNull.Value;
        command.Parameters.Add("@StatusID", SqlDbType.Int).Value = (object?)appointment.StatusID ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            appointment.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            appointment.PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID"));
            appointment.DoctorID = reader.IsDBNull(reader.GetOrdinal("DoctorID")) ? 0 : reader.GetInt32(reader.GetOrdinal("DoctorID"));
            appointment.AppointmentDate = reader.IsDBNull(reader.GetOrdinal("AppointmentDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("AppointmentDate"));
            appointment.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
            appointment.CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            appointment.RoomID = reader.IsDBNull(reader.GetOrdinal("RoomID")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoomID"));
            appointment.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("CreatedBy"));
            appointment.StatusID = reader.IsDBNull(reader.GetOrdinal("StatusID")) ? 0 : reader.GetInt32(reader.GetOrdinal("StatusID"));
        }
        reader.Close();
        result.Data = appointment;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the appointment";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<AppointmentDTO>> DeleteAppointmentByIDAsync(
    int iD,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<AppointmentDTO>();
    var appointment = new AppointmentDTO
    {
        ID = iD,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteAppointmentByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = appointment.ID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = appointment;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the appointment";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

