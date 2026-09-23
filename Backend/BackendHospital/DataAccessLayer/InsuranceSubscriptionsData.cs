
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
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Price")),
                CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                CoveragePercentage = reader.IsDBNull(reader.GetOrdinal("CoveragePercentage")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoveragePercentage")),
                DurationMonths = reader.IsDBNull(reader.GetOrdinal("DurationMonths")) ? 0 : reader.GetInt32(reader.GetOrdinal("DurationMonths")),
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
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        insurancesubscriptions.ID = (int)outputID.Value;
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

public static async Task<ApiResult<InsuranceSubscriptionsDTO>> DeleteInsuranceSubscriptionsByIDAsync(
    int iD,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InsuranceSubscriptionsDTO>();
    var insurancesubscriptions = new InsuranceSubscriptionsDTO
    {
        ID = iD,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteInsuranceSubscriptionsByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = insurancesubscriptions.ID;
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

public static async Task<ApiResult<List<InsuranceSubscriptionsViewDTO>>> SearchInsuranceSubscriptions(
string? PersonName,
string? CompanyName,
int? DurationMonths,
DateTime? EndDate,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<InsuranceSubscriptionsViewDTO>>();
    var list = new List<InsuranceSubscriptionsViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_SearchInsuranceSubscriptions", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@PersonName", SqlDbType.NVarChar).Value = PersonName ;
        command.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = CompanyName ;
        command.Parameters.Add("@DurationMonths ", SqlDbType.Int).Value = DurationMonths;
        command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDate ;
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new InsuranceSubscriptionsViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Price")),
                CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                CoveragePercentage = reader.IsDBNull(reader.GetOrdinal("CoveragePercentage")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoveragePercentage")),
                DurationMonths = reader.IsDBNull(reader.GetOrdinal("DurationMonths")) ? 0 : reader.GetInt32(reader.GetOrdinal("DurationMonths")),
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

public static async Task<ApiResult<InsuranceSubscriptionsViewDTO>> GetInsuranceSubscriptionsByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InsuranceSubscriptionsViewDTO>();
    var dto = new InsuranceSubscriptionsViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetInsuranceSubscriptionsByID", connection) { CommandType = CommandType.StoredProcedure };
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
            dto.Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Price"));
            dto.CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName"));
            dto.CoveragePercentage = reader.IsDBNull(reader.GetOrdinal("CoveragePercentage")) ? 0 : reader.GetInt32(reader.GetOrdinal("CoveragePercentage"));
            dto.DurationMonths = reader.IsDBNull(reader.GetOrdinal("DurationMonths")) ? 0 : reader.GetInt32(reader.GetOrdinal("DurationMonths"));
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

}

