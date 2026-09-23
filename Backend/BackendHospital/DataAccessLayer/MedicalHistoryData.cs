
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class MedicalHistoryData
{
public static async Task<ApiResult<List<MedicalHistoryViewDTO>>> GetAllMedicalHistoryAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<MedicalHistoryViewDTO>>();
    var list = new List<MedicalHistoryViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllMedicalHistory", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new MedicalHistoryViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID")),
                ConditionName = reader.IsDBNull(reader.GetOrdinal("ConditionName")) ? null : reader.GetString(reader.GetOrdinal("ConditionName")),
                DiagnosisDate = reader.IsDBNull(reader.GetOrdinal("DiagnosisDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DiagnosisDate")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
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

public static async Task<ApiResult<MedicalHistoryDTO>> AddMedicalHistoryAsync(
    MedicalHistoryDTO medicalhistory,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<MedicalHistoryDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddMedicalHistory", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@PatientID", SqlDbType.Int).Value = medicalhistory.PatientID;
        command.Parameters.Add("@ConditionName", SqlDbType.NVarChar, 50).Value = (object?)medicalhistory.ConditionName ?? DBNull.Value;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 50).Value = (object?)medicalhistory.Notes ?? DBNull.Value;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        medicalhistory.ID = (int)outputID.Value;
        result.Data = medicalhistory;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the medicalhistory.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<MedicalHistoryDTO>> UpdateMedicalHistoryByIDAsync(
    MedicalHistoryUpdateDTO medicalhistory,
    CancellationToken cancellationToken = default)
{
MedicalHistoryDTO medicalhistoryDTO = new  MedicalHistoryDTO();
    var result = new ApiResult<MedicalHistoryDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateMedicalHistoryByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)medicalhistory.ID ?? DBNull.Value;
        command.Parameters.Add("@ConditionName", SqlDbType.NVarChar, 500).Value = (object?)medicalhistory.ConditionName ?? DBNull.Value;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = (object?)medicalhistory.Notes ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            medicalhistoryDTO.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            medicalhistoryDTO.PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID"));
            medicalhistoryDTO.ConditionName = reader.IsDBNull(reader.GetOrdinal("ConditionName")) ? null : reader.GetString(reader.GetOrdinal("ConditionName"));
            medicalhistoryDTO.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
        }
        reader.Close();
        result.Data = medicalhistoryDTO;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the medicalhistory";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<MedicalHistoryDTO>> DeleteMedicalHistoryByIDAsync(
    int iD,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<MedicalHistoryDTO>();
    var medicalhistory = new MedicalHistoryDTO
    {
        ID = iD,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteMedicalHistoryByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = medicalhistory.ID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = medicalhistory;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the medicalhistory";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<List<MedicalHistoryViewDTO>>> SearchMedicalHistory(
string? PersonName,
int? PatientID,
DateTime? DiagnosisDate,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<MedicalHistoryViewDTO>>();
    var list = new List<MedicalHistoryViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_SearchMedicalHistory", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@PersonName", SqlDbType.NVarChar).Value = PersonName ;
        command.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID ;
        command.Parameters.Add("@DiagnosisDate", SqlDbType.DateTime).Value = DiagnosisDate ;
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new MedicalHistoryViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID")),
                ConditionName = reader.IsDBNull(reader.GetOrdinal("ConditionName")) ? null : reader.GetString(reader.GetOrdinal("ConditionName")),
                DiagnosisDate = reader.IsDBNull(reader.GetOrdinal("DiagnosisDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DiagnosisDate")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
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

public static async Task<ApiResult<MedicalHistoryViewDTO>> GetMedicalHistoryByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<MedicalHistoryViewDTO>();
    var dto = new MedicalHistoryViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetMedicalHistoryByID", connection) { CommandType = CommandType.StoredProcedure };
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
            dto.PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID"));
            dto.ConditionName = reader.IsDBNull(reader.GetOrdinal("ConditionName")) ? null : reader.GetString(reader.GetOrdinal("ConditionName"));
            dto.DiagnosisDate = reader.IsDBNull(reader.GetOrdinal("DiagnosisDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DiagnosisDate"));
            dto.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the medicalhistory";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

