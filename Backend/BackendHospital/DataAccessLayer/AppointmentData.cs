using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ConnectionString;

public static class AppointmentData
{
    public static async Task<ApiResult<List<AppointmentViewDTO>>>
        GetAllAppointmentAsync(
            CancellationToken cancellationToken = default)
    {
        var result =
            new ApiResult<List<AppointmentViewDTO>>();

        var list =
            new List<AppointmentViewDTO>();

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_GetAllAppointment",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };


            await connection
                .OpenAsync(cancellationToken)
                .ConfigureAwait(false);


            await using var reader =
                await command
                    .ExecuteReaderAsync(cancellationToken)
                    .ConfigureAwait(false);


            while (await reader
                .ReadAsync(cancellationToken)
                .ConfigureAwait(false))
            {
                list.Add(
                    new AppointmentViewDTO
                    {
                        ID =
                            reader.IsDBNull(
                                reader.GetOrdinal("ID"))
                                ? 0
                                : reader.GetInt32(
                                    reader.GetOrdinal("ID")),

                        DoctorName =
                            reader.IsDBNull(
                                reader.GetOrdinal("DoctorName"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("DoctorName")),

                        PatientName =
                            reader.IsDBNull(
                                reader.GetOrdinal("PatientName"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("PatientName")),

                        StatusName =
                            reader.IsDBNull(
                                reader.GetOrdinal("StatusName"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("StatusName")),

                        RoomNumber =
                            reader.IsDBNull(
                                reader.GetOrdinal("RoomNumber"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("RoomNumber")),

                        StartDate =
                            reader.IsDBNull(
                                reader.GetOrdinal("StartDate"))
                                ? DateTime.MinValue
                                : reader.GetDateTime(
                                    reader.GetOrdinal("StartDate")),

                        EndDate =
                            reader.IsDBNull(
                                reader.GetOrdinal("EndDate"))
                                ? DateTime.MinValue
                                : reader.GetDateTime(
                                    reader.GetOrdinal("EndDate")),

                        Notes =
                            reader.IsDBNull(
                                reader.GetOrdinal("Notes"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("Notes")),

                        CreatedAt =
                            reader.IsDBNull(
                                reader.GetOrdinal("CreatedAt"))
                                ? DateTime.MinValue
                                : reader.GetDateTime(
                                    reader.GetOrdinal("CreatedAt"))
                    });
            }


            result.Data =
                list;
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            result.Data = null;

            result.Message =
                "Database error occurred while fetching appointments.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }


    public static async Task<ApiResult<AppointmentDTO>>
        AddAppointmentAsync(
            AppointmentDTO appointment,
            CancellationToken cancellationToken = default)
    {
        var result =
            new ApiResult<AppointmentDTO>();

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_AddAppointment",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };


            command.Parameters
                .Add(
                    "@PatientID",
                    SqlDbType.Int)
                .Value =
                    appointment.PatientID;


            command.Parameters
                .Add(
                    "@DoctorID",
                    SqlDbType.Int)
                .Value =
                    appointment.DoctorID;


            command.Parameters
                .Add(
                    "@StartDate",
                    SqlDbType.DateTime)
                .Value =
                    appointment.StartDate;


            command.Parameters
                .Add(
                    "@Notes",
                    SqlDbType.NVarChar,
                    500)
                .Value =
                    (object?)appointment.Notes
                    ?? DBNull.Value;


            command.Parameters
                .Add(
                    "@StatusID",
                    SqlDbType.Int)
                .Value =
                    appointment.StatusID;


            command.Parameters
                .Add(
                    "@EndDate",
                    SqlDbType.DateTime)
                .Value =
                    appointment.EndDate;


            command.Parameters
                .Add(
                    "@RoomID",
                    SqlDbType.Int)
                .Value =
                    appointment.RoomID;


            var outputID =
                new SqlParameter(
                    "@NewID",
                    SqlDbType.Int)
                {
                    Direction =
                        ParameterDirection.Output
                };


            var outputMsg =
                new SqlParameter(
                    "@Message",
                    SqlDbType.NVarChar,
                    250)
                {
                    Direction =
                        ParameterDirection.Output
                };


            var outputErrorType =
                new SqlParameter(
                    "@ErrorType",
                    SqlDbType.Int)
                {
                    Direction =
                        ParameterDirection.Output
                };


            command.Parameters.Add(outputID);

            command.Parameters.Add(outputMsg);

            command.Parameters.Add(outputErrorType);


            await connection
                .OpenAsync(cancellationToken)
                .ConfigureAwait(false);


            await command
                .ExecuteNonQueryAsync(cancellationToken)
                .ConfigureAwait(false);


            result.Message =
                outputMsg.Value == DBNull.Value
                    ? null
                    : outputMsg.Value?.ToString();


            result.ErrorType =
                ErrorTypeMapper.GetErrorType(
                    outputErrorType.Value == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            outputErrorType.Value));


            if (outputID.Value == DBNull.Value)
            {
                result.Data = null;
            }
            else
            {
                appointment.ID =
                    Convert.ToInt32(
                        outputID.Value);

                result.Data =
                    appointment;
            }
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            result.Data = null;

            result.Message =
                "Database error occurred while adding the appointment.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }


    public static async Task<ApiResult<AppointmentDTO>>
        UpdateAppointmentByIDAsync(
            AppointmentUpdateDTO appointment,
            CancellationToken cancellationToken = default)
    {
        var result =
            new ApiResult<AppointmentDTO>();

        AppointmentDTO? appointmentDTO =
            null;

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_UpdateAppointmentByID",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };


            command.Parameters
                .Add(
                    "@ID",
                    SqlDbType.Int)
                .Value =
                    (object?)appointment.ID
                    ?? DBNull.Value;


            command.Parameters
                .Add(
                    "@DoctorID",
                    SqlDbType.Int)
                .Value =
                    (object?)appointment.DoctorID
                    ?? DBNull.Value;


            command.Parameters
                .Add(
                    "@StartDate",
                    SqlDbType.DateTime)
                .Value =
                    (object?)appointment.StartDate
                    ?? DBNull.Value;


            command.Parameters
                .Add(
                    "@Notes",
                    SqlDbType.NVarChar,
                    500)
                .Value =
                    (object?)appointment.Notes
                    ?? DBNull.Value;


            command.Parameters
                .Add(
                    "@StatusID",
                    SqlDbType.Int)
                .Value =
                    (object?)appointment.StatusID
                    ?? DBNull.Value;


            command.Parameters
                .Add(
                    "@EndDate",
                    SqlDbType.DateTime)
                .Value =
                    (object?)appointment.EndDate
                    ?? DBNull.Value;


            command.Parameters
                .Add(
                    "@RoomID",
                    SqlDbType.Int)
                .Value =
                    (object?)appointment.RoomID
                    ?? DBNull.Value;


            var outputMsg =
                new SqlParameter(
                    "@Message",
                    SqlDbType.NVarChar,
                    250)
                {
                    Direction =
                        ParameterDirection.Output
                };


            var outputErrorType =
                new SqlParameter(
                    "@ErrorType",
                    SqlDbType.Int)
                {
                    Direction =
                        ParameterDirection.Output
                };


            command.Parameters.Add(outputMsg);

            command.Parameters.Add(outputErrorType);


            await connection
                .OpenAsync(cancellationToken)
                .ConfigureAwait(false);


            await using (
                var reader =
                    await command
                        .ExecuteReaderAsync(
                            cancellationToken)
                        .ConfigureAwait(false))
            {
                if (await reader
                    .ReadAsync(cancellationToken)
                    .ConfigureAwait(false))
                {
                    appointmentDTO =
                        new AppointmentDTO
                        {
                            ID =
                                reader.IsDBNull(
                                    reader.GetOrdinal("ID"))
                                    ? 0
                                    : reader.GetInt32(
                                        reader.GetOrdinal("ID")),

                            PatientID =
                                reader.IsDBNull(
                                    reader.GetOrdinal("PatientID"))
                                    ? 0
                                    : reader.GetInt32(
                                        reader.GetOrdinal("PatientID")),

                            DoctorID =
                                reader.IsDBNull(
                                    reader.GetOrdinal("DoctorID"))
                                    ? 0
                                    : reader.GetInt32(
                                        reader.GetOrdinal("DoctorID")),

                            StartDate =
                                reader.IsDBNull(
                                    reader.GetOrdinal("StartDate"))
                                    ? DateTime.MinValue
                                    : reader.GetDateTime(
                                        reader.GetOrdinal("StartDate")),

                            Notes =
                                reader.IsDBNull(
                                    reader.GetOrdinal("Notes"))
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal("Notes")),

                            StatusID =
                                reader.IsDBNull(
                                    reader.GetOrdinal("StatusID"))
                                    ? 0
                                    : reader.GetInt32(
                                        reader.GetOrdinal("StatusID")),

                            EndDate =
                                reader.IsDBNull(
                                    reader.GetOrdinal("EndDate"))
                                    ? DateTime.MinValue
                                    : reader.GetDateTime(
                                        reader.GetOrdinal("EndDate")),

                            RoomID =
                                reader.IsDBNull(
                                    reader.GetOrdinal("RoomID"))
                                    ? 0
                                    : reader.GetInt32(
                                        reader.GetOrdinal("RoomID"))
                        };
                }
            }


            result.Data =
                appointmentDTO;

            result.Message =
                outputMsg.Value == DBNull.Value
                    ? null
                    : outputMsg.Value?.ToString();


            result.ErrorType =
                ErrorTypeMapper.GetErrorType(
                    outputErrorType.Value == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            outputErrorType.Value));
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            result.Data = null;

            result.Message =
                "Database error occurred while updating the appointment.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }


    public static async Task<ApiResult<AppointmentDTO>>
        DeleteAppointmentByIDAsync(
            int id,
            CancellationToken cancellationToken = default)
    {
        var result =
            new ApiResult<AppointmentDTO>();

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_DeleteAppointmentByID",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };


            command.Parameters
                .Add(
                    "@ID",
                    SqlDbType.Int)
                .Value =
                    id;


            var outputMsg =
                new SqlParameter(
                    "@Message",
                    SqlDbType.NVarChar,
                    250)
                {
                    Direction =
                        ParameterDirection.Output
                };


            var outputErrorType =
                new SqlParameter(
                    "@ErrorType",
                    SqlDbType.Int)
                {
                    Direction =
                        ParameterDirection.Output
                };


            command.Parameters.Add(outputMsg);

            command.Parameters.Add(outputErrorType);


            await connection
                .OpenAsync(cancellationToken)
                .ConfigureAwait(false);


            await command
                .ExecuteNonQueryAsync(cancellationToken)
                .ConfigureAwait(false);


            result.Data =
                new AppointmentDTO
                {
                    ID = id
                };


            result.Message =
                outputMsg.Value == DBNull.Value
                    ? null
                    : outputMsg.Value?.ToString();


            result.ErrorType =
                ErrorTypeMapper.GetErrorType(
                    outputErrorType.Value == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            outputErrorType.Value));
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            result.Data = null;

            result.Message =
                "Database error occurred while deleting the appointment.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }


    public static async Task<ApiResult<List<AppointmentViewDTO>>>
        SearchAppointment(
            string? DoctorName,
            string? PatientName,
            string? RoomNumber,
            DateTime? StartDate,
            DateTime? EndDate,
            CancellationToken cancellationToken = default)
    {
        var result =
            new ApiResult<List<AppointmentViewDTO>>();

        var list =
            new List<AppointmentViewDTO>();

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_SearchAppointment",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };


            command.Parameters
                .Add(
                    "@DoctorName",
                    SqlDbType.NVarChar,
                    200)
                .Value =
                    (object?)DoctorName
                    ?? DBNull.Value;


            command.Parameters
                .Add(
                    "@PatientName",
                    SqlDbType.NVarChar,
                    200)
                .Value =
                    (object?)PatientName
                    ?? DBNull.Value;


            command.Parameters
                .Add(
                    "@RoomNumber",
                    SqlDbType.NVarChar,
                    100)
                .Value =
                    (object?)RoomNumber
                    ?? DBNull.Value;


            command.Parameters
                .Add(
                    "@StartDate",
                    SqlDbType.DateTime)
                .Value =
                    (object?)StartDate
                    ?? DBNull.Value;


            command.Parameters
                .Add(
                    "@EndDate",
                    SqlDbType.DateTime)
                .Value =
                    (object?)EndDate
                    ?? DBNull.Value;


            await connection
                .OpenAsync(cancellationToken)
                .ConfigureAwait(false);


            await using var reader =
                await command
                    .ExecuteReaderAsync(cancellationToken)
                    .ConfigureAwait(false);


            while (await reader
                .ReadAsync(cancellationToken)
                .ConfigureAwait(false))
            {
                list.Add(
                    new AppointmentViewDTO
                    {
                        ID =
                            reader.IsDBNull(
                                reader.GetOrdinal("ID"))
                                ? 0
                                : reader.GetInt32(
                                    reader.GetOrdinal("ID")),

                        DoctorName =
                            reader.IsDBNull(
                                reader.GetOrdinal("DoctorName"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("DoctorName")),

                        PatientName =
                            reader.IsDBNull(
                                reader.GetOrdinal("PatientName"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("PatientName")),

                        StatusName =
                            reader.IsDBNull(
                                reader.GetOrdinal("StatusName"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("StatusName")),

                        RoomNumber =
                            reader.IsDBNull(
                                reader.GetOrdinal("RoomNumber"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("RoomNumber")),

                        StartDate =
                            reader.IsDBNull(
                                reader.GetOrdinal("StartDate"))
                                ? DateTime.MinValue
                                : reader.GetDateTime(
                                    reader.GetOrdinal("StartDate")),

                        EndDate =
                            reader.IsDBNull(
                                reader.GetOrdinal("EndDate"))
                                ? DateTime.MinValue
                                : reader.GetDateTime(
                                    reader.GetOrdinal("EndDate")),

                        Notes =
                            reader.IsDBNull(
                                reader.GetOrdinal("Notes"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("Notes")),

                        CreatedAt =
                            reader.IsDBNull(
                                reader.GetOrdinal("CreatedAt"))
                                ? DateTime.MinValue
                                : reader.GetDateTime(
                                    reader.GetOrdinal("CreatedAt"))
                    });
            }


            result.Data =
                list;
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            result.Data = null;

            result.Message =
                "Database error occurred while searching appointments.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }


    public static async Task<ApiResult<AppointmentViewDTO>>
        GetAppointmentByIDAsync(
            int id,
            CancellationToken cancellationToken = default)
    {
        var result =
            new ApiResult<AppointmentViewDTO>();

        AppointmentViewDTO? dto =
            null;

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_GetAppointmentByID",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };


            command.Parameters
                .Add(
                    "@ID",
                    SqlDbType.Int)
                .Value =
                    id;


            var messageParameter =
                new SqlParameter(
                    "@Message",
                    SqlDbType.NVarChar,
                    250)
                {
                    Direction =
                        ParameterDirection.Output
                };


            var errorTypeParameter =
                new SqlParameter(
                    "@ErrorType",
                    SqlDbType.Int)
                {
                    Direction =
                        ParameterDirection.Output
                };


            command.Parameters.Add(
                messageParameter);

            command.Parameters.Add(
                errorTypeParameter);


            await connection
                .OpenAsync(cancellationToken)
                .ConfigureAwait(false);


            await using (
                var reader =
                    await command
                        .ExecuteReaderAsync(
                            cancellationToken)
                        .ConfigureAwait(false))
            {
                if (await reader
                    .ReadAsync(cancellationToken)
                    .ConfigureAwait(false))
                {
                    dto =
                        new AppointmentViewDTO
                        {
                            ID =
                                reader.IsDBNull(
                                    reader.GetOrdinal("ID"))
                                    ? 0
                                    : reader.GetInt32(
                                        reader.GetOrdinal("ID")),

                            DoctorName =
                                reader.IsDBNull(
                                    reader.GetOrdinal("DoctorName"))
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal("DoctorName")),

                            PatientName =
                                reader.IsDBNull(
                                    reader.GetOrdinal("PatientName"))
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal("PatientName")),

                            StatusName =
                                reader.IsDBNull(
                                    reader.GetOrdinal("StatusName"))
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal("StatusName")),

                            RoomNumber =
                                reader.IsDBNull(
                                    reader.GetOrdinal("RoomNumber"))
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal("RoomNumber")),

                            StartDate =
                                reader.IsDBNull(
                                    reader.GetOrdinal("StartDate"))
                                    ? DateTime.MinValue
                                    : reader.GetDateTime(
                                        reader.GetOrdinal("StartDate")),

                            EndDate =
                                reader.IsDBNull(
                                    reader.GetOrdinal("EndDate"))
                                    ? DateTime.MinValue
                                    : reader.GetDateTime(
                                        reader.GetOrdinal("EndDate")),

                            Notes =
                                reader.IsDBNull(
                                    reader.GetOrdinal("Notes"))
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal("Notes")),

                            CreatedAt =
                                reader.IsDBNull(
                                    reader.GetOrdinal("CreatedAt"))
                                    ? DateTime.MinValue
                                    : reader.GetDateTime(
                                        reader.GetOrdinal("CreatedAt"))
                        };
                }
            }


            result.Data =
                dto;


            result.Message =
                messageParameter.Value == DBNull.Value
                    ? null
                    : messageParameter.Value?.ToString();


            result.ErrorType =
                ErrorTypeMapper.GetErrorType(
                    errorTypeParameter.Value == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            errorTypeParameter.Value));
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            result.Data = null;

            result.Message =
                "Database error occurred while fetching the appointment.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }
}