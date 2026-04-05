
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class InvoicesData
{

    public static async Task<ApiResult<List<InvoicesViewDTO>>> GetAllInvoicesByPersonIDAsync(
    int PersonID,
    CancellationToken cancellationToken = default)
    {
        var result = new ApiResult<List<InvoicesViewDTO>>();
        var list = new List<InvoicesViewDTO>();
        try
        {
            await using var connection = new SqlConnection(connectionString._connectionString);
            await using var command = new SqlCommand("SP_GetAllInvoicesByPersonID", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (reader.Read())
            {
                list.Add(new InvoicesViewDTO
                {
                    ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                    PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID")),
                    PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID")),
                    PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                    AppointmentID = reader.IsDBNull(reader.GetOrdinal("AppointmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("AppointmentID")),
                    InvoiceDate = reader.IsDBNull(reader.GetOrdinal("InvoiceDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("InvoiceDate")),
                    TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                    InsuranceAmount = reader.IsDBNull(reader.GetOrdinal("InsuranceAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("InsuranceAmount")),
                    PatientAmount = reader.IsDBNull(reader.GetOrdinal("PatientAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PatientAmount")),
                    CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                    StatusDescription = reader.IsDBNull(reader.GetOrdinal("StatusDescription")) ? null : reader.GetString(reader.GetOrdinal("StatusDescription"))
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
    public static async Task<ApiResult<List<InvoicesViewDTO>>> GetAllInvoicesAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<InvoicesViewDTO>>();
    var list = new List<InvoicesViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllInvoices", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new InvoicesViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID")),
                PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID")),
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                AppointmentID = reader.IsDBNull(reader.GetOrdinal("AppointmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("AppointmentID")),
                InvoiceDate = reader.IsDBNull(reader.GetOrdinal("InvoiceDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("InvoiceDate")),
                TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                InsuranceAmount = reader.IsDBNull(reader.GetOrdinal("InsuranceAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("InsuranceAmount")),
                PatientAmount = reader.IsDBNull(reader.GetOrdinal("PatientAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PatientAmount")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                StatusDescription = reader.IsDBNull(reader.GetOrdinal("StatusDescription")) ? null : reader.GetString(reader.GetOrdinal("StatusDescription"))
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

public static async Task<ApiResult<InvoicesViewDTO>> GetInvoicesByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InvoicesViewDTO>();
    var dto = new InvoicesViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetInvoicesByID", connection) { CommandType = CommandType.StoredProcedure };
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
            dto.PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID"));
            dto.PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID"));
            dto.PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName"));
            dto.AppointmentID = reader.IsDBNull(reader.GetOrdinal("AppointmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("AppointmentID"));
            dto.InvoiceDate = reader.IsDBNull(reader.GetOrdinal("InvoiceDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("InvoiceDate"));
            dto.TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TotalAmount"));
            dto.InsuranceAmount = reader.IsDBNull(reader.GetOrdinal("InsuranceAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("InsuranceAmount"));
            dto.PatientAmount = reader.IsDBNull(reader.GetOrdinal("PatientAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PatientAmount"));
            dto.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("CreatedBy"));
            dto.StatusDescription = reader.IsDBNull(reader.GetOrdinal("StatusDescription")) ? null : reader.GetString(reader.GetOrdinal("StatusDescription"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the invoices";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InvoicesDTO>> AddInvoicesAsync(
    InvoicesDTO invoices,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InvoicesDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddInvoices", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@PatientID", SqlDbType.Int).Value = invoices.PatientID;
        command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = invoices.AppointmentID;
        command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = invoices.InvoiceDate;
        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = invoices.TotalAmount;
        command.Parameters.Add("@InsuranceAmount", SqlDbType.Decimal).Value = invoices.InsuranceAmount;
        command.Parameters.Add("@PatientAmount", SqlDbType.Decimal).Value = invoices.PatientAmount;
        command.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = invoices.CreatedBy;
        command.Parameters.Add("@StatusID", SqlDbType.Int).Value = invoices.StatusID;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        invoices.ID = (int)outputID.Value;
        result.Data = invoices;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the invoices.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InvoicesUpdateDTO>> UpdateInvoicesByIDAsync(
    InvoicesUpdateDTO invoices,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InvoicesUpdateDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateInvoicesByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)invoices.ID ?? DBNull.Value;
        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = (object?)invoices.TotalAmount ?? DBNull.Value;
        command.Parameters.Add("@StatusID", SqlDbType.Int).Value = (object?)invoices.StatusID ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            invoices.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            invoices.PatientID = reader.IsDBNull(reader.GetOrdinal("PatientID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientID"));
            invoices.AppointmentID = reader.IsDBNull(reader.GetOrdinal("AppointmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("AppointmentID"));
            invoices.InvoiceDate = reader.IsDBNull(reader.GetOrdinal("InvoiceDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("InvoiceDate"));
            invoices.TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TotalAmount"));
            invoices.InsuranceAmount = reader.IsDBNull(reader.GetOrdinal("InsuranceAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("InsuranceAmount"));
            invoices.PatientAmount = reader.IsDBNull(reader.GetOrdinal("PatientAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PatientAmount"));
            invoices.CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("CreatedBy"));
            invoices.StatusID = reader.IsDBNull(reader.GetOrdinal("StatusID")) ? 0 : reader.GetInt32(reader.GetOrdinal("StatusID"));
        }
        reader.Close();
        result.Data = invoices;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the invoices";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<InvoicesDTO>> DeleteInvoicesByIDAsync(
    int iD,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<InvoicesDTO>();
    var invoices = new InvoicesDTO
    {
        ID = iD,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteInvoicesByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = invoices.ID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = invoices;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the invoices";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

