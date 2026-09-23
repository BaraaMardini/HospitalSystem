
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
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                AmountPaid = reader.IsDBNull(reader.GetOrdinal("AmountPaid")) ? 0 : reader.GetDecimal(reader.GetOrdinal("AmountPaid")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                AppointmentID = reader.IsDBNull(reader.GetOrdinal("AppointmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("AppointmentID")),
                RemainingAmount = reader.IsDBNull(reader.GetOrdinal("RemainingAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RemainingAmount"))
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
        command.Parameters.Add("@AmountPaid", SqlDbType.Decimal).Value = invoicepayments.AmountPaid;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 50).Value = (object?)invoicepayments.Notes ?? DBNull.Value;
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

public static async Task<ApiResult<InvoicePaymentsDTO>> UpdateInvoicePaymentsByIDAsync(
    InvoicePaymentsUpdateDTO invoicepayments,
    CancellationToken cancellationToken = default)
{
InvoicePaymentsDTO invoicepaymentsDTO = new  InvoicePaymentsDTO();
    var result = new ApiResult<InvoicePaymentsDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateInvoicePaymentsByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)invoicepayments.ID ?? DBNull.Value;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = (object?)invoicepayments.Notes ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            invoicepaymentsDTO.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            invoicepaymentsDTO.InvoiceID = reader.IsDBNull(reader.GetOrdinal("InvoiceID")) ? 0 : reader.GetInt32(reader.GetOrdinal("InvoiceID"));
            invoicepaymentsDTO.AmountPaid = reader.IsDBNull(reader.GetOrdinal("AmountPaid")) ? 0 : reader.GetDecimal(reader.GetOrdinal("AmountPaid"));
            invoicepaymentsDTO.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
        }
        reader.Close();
        result.Data = invoicepaymentsDTO;
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

public static async Task<ApiResult<List<InvoicePaymentsViewDTO>>> SearchInvoicePayments(
int? InvoiceID,
string? PersonName,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<InvoicePaymentsViewDTO>>();
    var list = new List<InvoicePaymentsViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_SearchInvoicePayments", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = InvoiceID ;
        command.Parameters.Add("@PersonName", SqlDbType.NVarChar).Value = PersonName ;
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new InvoicePaymentsViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                InvoiceID = reader.IsDBNull(reader.GetOrdinal("InvoiceID")) ? 0 : reader.GetInt32(reader.GetOrdinal("InvoiceID")),
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                AmountPaid = reader.IsDBNull(reader.GetOrdinal("AmountPaid")) ? 0 : reader.GetDecimal(reader.GetOrdinal("AmountPaid")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                AppointmentID = reader.IsDBNull(reader.GetOrdinal("AppointmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("AppointmentID")),
                RemainingAmount = reader.IsDBNull(reader.GetOrdinal("RemainingAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RemainingAmount"))
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
            dto.PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName"));
            dto.PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("PaymentDate"));
            dto.AmountPaid = reader.IsDBNull(reader.GetOrdinal("AmountPaid")) ? 0 : reader.GetDecimal(reader.GetOrdinal("AmountPaid"));
            dto.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
            dto.AppointmentID = reader.IsDBNull(reader.GetOrdinal("AppointmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("AppointmentID"));
            dto.RemainingAmount = reader.IsDBNull(reader.GetOrdinal("RemainingAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RemainingAmount"));
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

}

