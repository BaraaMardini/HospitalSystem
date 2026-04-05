
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class InsuranceData
{
public static async Task<ApiResult<List<InsuranceViewDTO>>> GetAllInsuranceAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<InsuranceViewDTO>>();
    var list = new List<InsuranceViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllInsurance", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new InsuranceViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                PolicyNumber = reader.IsDBNull(reader.GetOrdinal("PolicyNumber")) ? null : reader.GetString(reader.GetOrdinal("PolicyNumber")),
                CoveragePercentage = reader.IsDBNull(reader.GetOrdinal("CoveragePercentage")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoveragePercentage")),
                DefaultDurationMonths = reader.IsDBNull(reader.GetOrdinal("DefaultDurationMonths")) ? 0 : reader.GetInt32(reader.GetOrdinal("DefaultDurationMonths")),
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

public static async Task<ApiResult<InsuranceViewDTO>> GetInsuranceByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InsuranceViewDTO>();
    var dto = new InsuranceViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetInsuranceByID", connection) { CommandType = CommandType.StoredProcedure };
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
            dto.CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName"));
            dto.PolicyNumber = reader.IsDBNull(reader.GetOrdinal("PolicyNumber")) ? null : reader.GetString(reader.GetOrdinal("PolicyNumber"));
            dto.CoveragePercentage = reader.IsDBNull(reader.GetOrdinal("CoveragePercentage")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoveragePercentage"));
            dto.DefaultDurationMonths = reader.IsDBNull(reader.GetOrdinal("DefaultDurationMonths")) ? 0 : reader.GetInt32(reader.GetOrdinal("DefaultDurationMonths"));
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
        result.Message = "Database error occurred while fetching the insurance";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InsuranceDTO>> AddInsuranceAsync(
    InsuranceDTO insurance,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InsuranceDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddInsurance", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 50).Value = (object?)insurance.CompanyName ?? DBNull.Value;
        command.Parameters.Add("@PolicyNumber", SqlDbType.NVarChar, 50).Value = (object?)insurance.PolicyNumber ?? DBNull.Value;
        command.Parameters.Add("@CoveragePercentage", SqlDbType.Int).Value = insurance.CoveragePercentage;
        command.Parameters.Add("@DefaultDurationMonths", SqlDbType.Int).Value = insurance.DefaultDurationMonths;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 50).Value = (object?)insurance.Notes ?? DBNull.Value;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        insurance.ID = (int)outputID.Value;
        result.Data = insurance;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the insurance.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InsuranceUpdateDTO>> UpdateInsuranceByIDAsync(
    InsuranceUpdateDTO insurance,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InsuranceUpdateDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateInsuranceByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)insurance.ID ?? DBNull.Value;
        command.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 500).Value = (object?)insurance.CompanyName ?? DBNull.Value;
        command.Parameters.Add("@PolicyNumber", SqlDbType.NVarChar, 500).Value = (object?)insurance.PolicyNumber ?? DBNull.Value;
        command.Parameters.Add("@CoveragePercentage", SqlDbType.Int).Value = (object?)insurance.CoveragePercentage ?? DBNull.Value;
        command.Parameters.Add("@DefaultDurationMonths", SqlDbType.Int).Value = (object?)insurance.DefaultDurationMonths ?? DBNull.Value;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = (object?)insurance.Notes ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            insurance.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            insurance.CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName"));
            insurance.PolicyNumber = reader.IsDBNull(reader.GetOrdinal("PolicyNumber")) ? null : reader.GetString(reader.GetOrdinal("PolicyNumber"));
            insurance.CoveragePercentage = reader.IsDBNull(reader.GetOrdinal("CoveragePercentage")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoveragePercentage"));
            insurance.DefaultDurationMonths = reader.IsDBNull(reader.GetOrdinal("DefaultDurationMonths")) ? 0 : reader.GetInt32(reader.GetOrdinal("DefaultDurationMonths"));
            insurance.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
        }
        reader.Close();
        result.Data = insurance;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the insurance";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InsuranceDTO>> DeleteInsuranceByIDAsync(
    int iD,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InsuranceDTO>();
    var insurance = new InsuranceDTO
    {
        ID = iD,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteInsuranceByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = insurance.ID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = insurance;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the insurance";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

