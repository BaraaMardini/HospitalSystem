
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class RoomBookingsData
{
public static async Task<ApiResult<List<RoomBookingsViewDTO>>> GetAllRoomBookingsAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<RoomBookingsViewDTO>>();
    var list = new List<RoomBookingsViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllRoomBookings", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new RoomBookingsViewDTO
            {
                BookingID = reader.IsDBNull(reader.GetOrdinal("BookingID")) ? 0 : reader.GetInt32(reader.GetOrdinal("BookingID")),
                AppointmentID = reader.IsDBNull(reader.GetOrdinal("AppointmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("AppointmentID")),
                RoomNumber = reader.IsDBNull(reader.GetOrdinal("RoomNumber")) ? null : reader.GetString(reader.GetOrdinal("RoomNumber")),
                DescriptionStatus = reader.IsDBNull(reader.GetOrdinal("DescriptionStatus")) ? null : reader.GetString(reader.GetOrdinal("DescriptionStatus")),
                PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? null : reader.GetString(reader.GetOrdinal("PatientName")),
                StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("StartDate")),
                EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("EndDate"))
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

public static async Task<ApiResult<RoomBookingsViewDTO>> GetRoomBookingsByAppointmentIDAsync(
    int AppointmentID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<RoomBookingsViewDTO>();
    var dto = new RoomBookingsViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetRoomBookingsByAppointmentID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
        var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var errorTypeParameter = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(messageParameter);
        command.Parameters.Add(errorTypeParameter);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            dto.BookingID = reader.IsDBNull(reader.GetOrdinal("BookingID")) ? 0 : reader.GetInt32(reader.GetOrdinal("BookingID"));
            dto.AppointmentID = reader.IsDBNull(reader.GetOrdinal("AppointmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("AppointmentID"));
            dto.RoomNumber = reader.IsDBNull(reader.GetOrdinal("RoomNumber")) ? null : reader.GetString(reader.GetOrdinal("RoomNumber"));
            dto.DescriptionStatus = reader.IsDBNull(reader.GetOrdinal("DescriptionStatus")) ? null : reader.GetString(reader.GetOrdinal("DescriptionStatus"));
            dto.PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? null : reader.GetString(reader.GetOrdinal("PatientName"));
            dto.StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("StartDate"));
            dto.EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("EndDate"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the roombookings";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}


public static async Task<ApiResult<RoomBookingsUpdateDTO>> UpdateRoomBookingsByBookingIDAsync(
    RoomBookingsUpdateDTO roombookings,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<RoomBookingsUpdateDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateRoomBookingsByBookingID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@BookingID", SqlDbType.Int).Value = (object?)roombookings.BookingID ?? DBNull.Value;
        command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = (object?)roombookings.StartDate ?? DBNull.Value;
        command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = (object?)roombookings.EndDate ?? DBNull.Value;
        command.Parameters.Add("@StatusID", SqlDbType.Int).Value = (object?)roombookings.StatusID ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            roombookings.BookingID = reader.IsDBNull(reader.GetOrdinal("BookingID")) ? 0 : reader.GetInt32(reader.GetOrdinal("BookingID"));
            roombookings.AppointmentID = reader.IsDBNull(reader.GetOrdinal("AppointmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("AppointmentID"));
            roombookings.StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("StartDate"));
            roombookings.EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("EndDate"));
            roombookings.StatusID = reader.IsDBNull(reader.GetOrdinal("StatusID")) ? 0 : reader.GetInt32(reader.GetOrdinal("StatusID"));
        }
        reader.Close();
        result.Data = roombookings;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the roombookings";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<RoomBookingsDTO>> DeleteRoomBookingsByBookingIDAsync(
    int bookingID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<RoomBookingsDTO>();
    var roombookings = new RoomBookingsDTO
    {
        BookingID = bookingID,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteRoomBookingsByBookingID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@BookingID", SqlDbType.Int).Value = roombookings.BookingID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = roombookings;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the roombookings";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

