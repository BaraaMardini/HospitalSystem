
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class InvoicePaymentsData
{
public static async Task<ApiResult<List<InvoicePaymentsViewDTO>>> GetAllInvoicePaymentsAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<InvoicePaymentsViewDTO>>();
    var list = new List<InvoicePaymentsViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllInvoicePayments", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new InvoicePaymentsViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                InvoiceID = reader.IsDBNull(reader.GetOrdinal("InvoiceID")) ? 0 : reader.GetInt32(reader.GetOrdinal("InvoiceID")),
                PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID")),
                PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                AmountPaid = reader.IsDBNull(reader.GetOrdinal("AmountPaid")) ? 0 : reader.GetDecimal(reader.GetOrdinal("AmountPaid")),
                RemainingAmount = reader.IsDBNull(reader.GetOrdinal("RemainingAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RemainingAmount")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
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

public static async Task<ApiResult<InvoicePaymentsViewDTO>> GetInvoicePaymentsByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InvoicePaymentsViewDTO>();
    var dto = new InvoicePaymentsViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetInvoicePaymentsByID", connection) { CommandType = CommandType.StoredProcedure };
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
            dto.InvoiceID = reader.IsDBNull(reader.GetOrdinal("InvoiceID")) ? 0 : reader.GetInt32(reader.GetOrdinal("InvoiceID"));
            dto.PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID"));
            dto.PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID"));
            dto.Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name"));
            dto.PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("PaymentDate"));
            dto.AmountPaid = reader.IsDBNull(reader.GetOrdinal("AmountPaid")) ? 0 : reader.GetDecimal(reader.GetOrdinal("AmountPaid"));
            dto.RemainingAmount = reader.IsDBNull(reader.GetOrdinal("RemainingAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RemainingAmount"));
            dto.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
            dto.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy"));
            dto.CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the invoicepayments";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InvoicePaymentsDTO>> AddInvoicePaymentsAsync(
    InvoicePaymentsDTO invoicepayments,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InvoicePaymentsDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddInvoicePayments", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoicepayments.InvoiceID;
        command.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = invoicepayments.PaymentDate;
        command.Parameters.Add("@AmountPaid", SqlDbType.Decimal).Value = invoicepayments.AmountPaid;
        command.Parameters.Add("@RemainingAmount", SqlDbType.Decimal).Value = invoicepayments.RemainingAmount;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 50).Value = (object?)invoicepayments.Notes ?? DBNull.Value;
        command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 50).Value = (object?)invoicepayments.CreatedBy ?? DBNull.Value;
        command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = invoicepayments.CreatedAt;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        invoicepayments.ID = (int)outputID.Value;
        result.Data = invoicepayments;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the invoicepayments.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InvoicePaymentsUpdateDTO>> UpdateInvoicePaymentsByIDAsync(
    InvoicePaymentsUpdateDTO invoicepayments,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InvoicePaymentsUpdateDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateInvoicePaymentsByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)invoicepayments.ID ?? DBNull.Value;
        command.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = (object?)invoicepayments.PaymentDate ?? DBNull.Value;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = (object?)invoicepayments.Notes ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            invoicepayments.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            invoicepayments.InvoiceID = reader.IsDBNull(reader.GetOrdinal("InvoiceID")) ? 0 : reader.GetInt32(reader.GetOrdinal("InvoiceID"));
            invoicepayments.PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("PaymentDate"));
            invoicepayments.AmountPaid = reader.IsDBNull(reader.GetOrdinal("AmountPaid")) ? 0 : reader.GetDecimal(reader.GetOrdinal("AmountPaid"));
            invoicepayments.RemainingAmount = reader.IsDBNull(reader.GetOrdinal("RemainingAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RemainingAmount"));
            invoicepayments.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
            invoicepayments.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy"));
            invoicepayments.CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
        }
        reader.Close();
        result.Data = invoicepayments;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the invoicepayments";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InvoicePaymentsDTO>> DeleteInvoicePaymentsByIDAsync(
    int iD,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InvoicePaymentsDTO>();
    var invoicepayments = new InvoicePaymentsDTO
    {
        ID = iD,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteInvoicePaymentsByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = invoicepayments.ID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = invoicepayments;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the invoicepayments";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

