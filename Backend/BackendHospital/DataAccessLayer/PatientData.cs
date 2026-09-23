
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class PatientData
{
public static async Task<ApiResult<List<PatientViewDTO>>> GetAllPatientAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<PatientViewDTO>>();
    var list = new List<PatientViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllPatient", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new PatientViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID")),
                BloodType = reader.IsDBNull(reader.GetOrdinal("BloodType")) ? null : reader.GetString(reader.GetOrdinal("BloodType")),
                Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age"))
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

public static async Task<ApiResult<PatientDTO>> AddPatientAsync(
    PatientDTO patient,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<PatientDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddPatient", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@PersonID", SqlDbType.Int).Value = patient.PersonID;
        command.Parameters.Add("@BloodType", SqlDbType.NVarChar, 50).Value = (object?)patient.BloodType ?? DBNull.Value;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        patient.ID = (int)outputID.Value;
        result.Data = patient;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the patient.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<PatientDTO>> UpdatePatientByIDAsync(
    PatientUpdateDTO patient,
    CancellationToken cancellationToken = default)
{
PatientDTO patientDTO = new  PatientDTO();
    var result = new ApiResult<PatientDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdatePatientByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)patient.ID ?? DBNull.Value;
        command.Parameters.Add("@PersonID", SqlDbType.Int).Value = (object?)patient.PersonID ?? DBNull.Value;
        command.Parameters.Add("@BloodType", SqlDbType.NVarChar, 500).Value = (object?)patient.BloodType ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            patientDTO.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            patientDTO.PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID"));
            patientDTO.BloodType = reader.IsDBNull(reader.GetOrdinal("BloodType")) ? null : reader.GetString(reader.GetOrdinal("BloodType"));
        }
        reader.Close();
        result.Data = patientDTO;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the patient";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<PatientDTO>> DeletePatientByIDAsync(
    int iD,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<PatientDTO>();
    var patient = new PatientDTO
    {
        ID = iD,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeletePatientByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = patient.ID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = patient;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the patient";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<List<PatientViewDTO>>> SearchPatient(
string? PersonName,
int? PersonID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<PatientViewDTO>>();
    var list = new List<PatientViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_SearchPatient", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@PersonName", SqlDbType.NVarChar).Value = PersonName ;
        command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID ;
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new PatientViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID")),
                BloodType = reader.IsDBNull(reader.GetOrdinal("BloodType")) ? null : reader.GetString(reader.GetOrdinal("BloodType")),
                Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age"))
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

public static async Task<ApiResult<PatientViewDTO>> GetPatientByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<PatientViewDTO>();
    var dto = new PatientViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetPatientByID", connection) { CommandType = CommandType.StoredProcedure };
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
            dto.PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID"));
            dto.BloodType = reader.IsDBNull(reader.GetOrdinal("BloodType")) ? null : reader.GetString(reader.GetOrdinal("BloodType"));
            dto.Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender"));
            dto.Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the patient";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

