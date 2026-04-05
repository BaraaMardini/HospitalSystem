
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
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID")),
                ConditionName = reader.IsDBNull(reader.GetOrdinal("ConditionName")) ? null : reader.GetString(reader.GetOrdinal("ConditionName")),
                DiagnosisDate = reader.IsDBNull(reader.GetOrdinal("DiagnosisDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DiagnosisDate")),
                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("CreatedBy"))
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
 
    public static async Task<ApiResult<List<MedicalHistoryViewDTO>>> GetAllMedicalHistoryByNameAsync(
     string Name,
     CancellationToken cancellationToken = default)
    {
        var result = new ApiResult<List<MedicalHistoryViewDTO>>();
        var list = new List<MedicalHistoryViewDTO>();
        try
        {
            await using var connection = new SqlConnection(connectionString._connectionString);
            await using var command = new SqlCommand("SP_GetAllMedicalHistoryByName", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (reader.Read())
            {
                list.Add(new MedicalHistoryViewDTO
                {
                    ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                    Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                    PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID")),
                    ConditionName = reader.IsDBNull(reader.GetOrdinal("ConditionName")) ? null : reader.GetString(reader.GetOrdinal("ConditionName")),
                    DiagnosisDate = reader.IsDBNull(reader.GetOrdinal("DiagnosisDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DiagnosisDate")),
                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                    CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("CreatedBy"))
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
            dto.Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name"));
            dto.PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID"));
            dto.ConditionName = reader.IsDBNull(reader.GetOrdinal("ConditionName")) ? null : reader.GetString(reader.GetOrdinal("ConditionName"));
            dto.DiagnosisDate = reader.IsDBNull(reader.GetOrdinal("DiagnosisDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DiagnosisDate"));
            dto.Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status"));
            dto.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
            dto.CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            dto.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("CreatedBy"));
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
        command.Parameters.Add("@DiagnosisDate", SqlDbType.DateTime).Value = medicalhistory.DiagnosisDate;
        command.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = (object?)medicalhistory.Status ?? DBNull.Value;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 50).Value = (object?)medicalhistory.Notes ?? DBNull.Value;
 
        command.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = medicalhistory.CreatedBy;
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

public static async Task<ApiResult<MedicalHistoryUpdateDTO>> UpdateMedicalHistoryByIDAsync(
    MedicalHistoryUpdateDTO medicalhistory,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<MedicalHistoryUpdateDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateMedicalHistoryByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)medicalhistory.ID ?? DBNull.Value;
        command.Parameters.Add("@PatientID", SqlDbType.Int).Value = (object?)medicalhistory.PatientID ?? DBNull.Value;
        command.Parameters.Add("@ConditionName", SqlDbType.NVarChar, 500).Value = (object?)medicalhistory.ConditionName ?? DBNull.Value;
        command.Parameters.Add("@DiagnosisDate", SqlDbType.DateTime).Value = (object?)medicalhistory.DiagnosisDate ?? DBNull.Value;
        command.Parameters.Add("@Status", SqlDbType.NVarChar, 500).Value = (object?)medicalhistory.Status ?? DBNull.Value;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = (object?)medicalhistory.Notes ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            medicalhistory.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            medicalhistory.PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID"));
            medicalhistory.ConditionName = reader.IsDBNull(reader.GetOrdinal("ConditionName")) ? null : reader.GetString(reader.GetOrdinal("ConditionName"));
            medicalhistory.DiagnosisDate = reader.IsDBNull(reader.GetOrdinal("DiagnosisDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DiagnosisDate"));
            medicalhistory.Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status"));
            medicalhistory.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
            medicalhistory.CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            medicalhistory.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("CreatedBy"));
        }
        reader.Close();
        result.Data = medicalhistory;
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

}

