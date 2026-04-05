
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class InsuranceSubscriptionsData
{
public static async Task<ApiResult<List<InsuranceSubscriptionsViewDTO>>> GetAllInsuranceSubscriptionsAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<InsuranceSubscriptionsViewDTO>>();
    var list = new List<InsuranceSubscriptionsViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllInsuranceSubscriptions", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new InsuranceSubscriptionsViewDTO
            {
                SubscriptionID = reader.IsDBNull(reader.GetOrdinal("SubscriptionID")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubscriptionID")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age")),
                CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                CoveragePercentage = reader.IsDBNull(reader.GetOrdinal("CoveragePercentage")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoveragePercentage")),
                DefaultDurationMonths = reader.IsDBNull(reader.GetOrdinal("DefaultDurationMonths")) ? 0 : reader.GetInt32(reader.GetOrdinal("DefaultDurationMonths")),
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

public static async Task<ApiResult<InsuranceSubscriptionsViewDTO>> GetInsuranceSubscriptionsBySubscriptionIDAsync(
    int SubscriptionID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InsuranceSubscriptionsViewDTO>();
    var dto = new InsuranceSubscriptionsViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetInsuranceSubscriptionsBySubscriptionID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@SubscriptionID", SqlDbType.Int).Value = SubscriptionID;
        var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var errorTypeParameter = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(messageParameter);
        command.Parameters.Add(errorTypeParameter);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            dto.SubscriptionID = reader.IsDBNull(reader.GetOrdinal("SubscriptionID")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubscriptionID"));
            dto.Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name"));
            dto.Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age"));
            dto.CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName"));
            dto.CoveragePercentage = reader.IsDBNull(reader.GetOrdinal("CoveragePercentage")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoveragePercentage"));
            dto.DefaultDurationMonths = reader.IsDBNull(reader.GetOrdinal("DefaultDurationMonths")) ? 0 : reader.GetInt32(reader.GetOrdinal("DefaultDurationMonths"));
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
        result.Message = "Database error occurred while fetching the insurancesubscriptions";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InsuranceSubscriptionsDTO>> AddInsuranceSubscriptionsAsync(
    InsuranceSubscriptionsDTO insurancesubscriptions,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InsuranceSubscriptionsDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddInsuranceSubscriptions", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@InsuranceID", SqlDbType.Int).Value = insurancesubscriptions.InsuranceID;
        command.Parameters.Add("@PersonID", SqlDbType.Int).Value = insurancesubscriptions.PersonID;
        command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = insurancesubscriptions.StartDate;
        command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = insurancesubscriptions.EndDate;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        insurancesubscriptions.SubscriptionID = (int)outputID.Value;
        result.Data = insurancesubscriptions;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the insurancesubscriptions.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InsuranceSubscriptionsUpdateDTO>> UpdateInsuranceSubscriptionsBySubscriptionIDAsync(
    InsuranceSubscriptionsUpdateDTO insurancesubscriptions,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InsuranceSubscriptionsUpdateDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateInsuranceSubscriptionsBySubscriptionID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@SubscriptionID", SqlDbType.Int).Value = (object?)insurancesubscriptions.SubscriptionID ?? DBNull.Value;
        command.Parameters.Add("@InsuranceID", SqlDbType.Int).Value = (object?)insurancesubscriptions.InsuranceID ?? DBNull.Value;
        command.Parameters.Add("@PersonID", SqlDbType.Int).Value = (object?)insurancesubscriptions.PersonID ?? DBNull.Value;
        command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = (object?)insurancesubscriptions.StartDate ?? DBNull.Value;
        command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = (object?)insurancesubscriptions.EndDate ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            insurancesubscriptions.SubscriptionID = reader.IsDBNull(reader.GetOrdinal("SubscriptionID")) ? 0 : reader.GetInt32(reader.GetOrdinal("SubscriptionID"));
            insurancesubscriptions.InsuranceID = reader.IsDBNull(reader.GetOrdinal("InsuranceID")) ? 0 : reader.GetInt32(reader.GetOrdinal("InsuranceID"));
            insurancesubscriptions.PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID"));
            insurancesubscriptions.StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("StartDate"));
            insurancesubscriptions.EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("EndDate"));
        }
        reader.Close();
        result.Data = insurancesubscriptions;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the insurancesubscriptions";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InsuranceSubscriptionsDTO>> DeleteInsuranceSubscriptionsBySubscriptionIDAsync(
    int subscriptionID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InsuranceSubscriptionsDTO>();
    var insurancesubscriptions = new InsuranceSubscriptionsDTO
    {
        SubscriptionID = subscriptionID,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteInsuranceSubscriptionsBySubscriptionID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@SubscriptionID", SqlDbType.Int).Value = insurancesubscriptions.SubscriptionID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = insurancesubscriptions;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the insurancesubscriptions";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

