
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class InvoicePaymentsService
{

public static async Task<ApiResult<List<InvoicePaymentsViewDTO>>> GetAllInvoicePaymentsAsync(
    CancellationToken cancellationToken = default)
{
    var result = await InvoicePaymentsData.GetAllInvoicePaymentsAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<InvoicePaymentsViewDTO>>(
            null,
            "No InvoicePaymentss found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<InvoicePaymentsViewDTO>>(
        result.Data,
        "InvoicePaymentss retrieved successfully.",
        ErrorType.None
    );
}


public static async Task<ApiResult<InvoicePaymentsDTO>> AddInvoicePaymentsAsync(
    InvoicePaymentsDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<InvoicePaymentsDTO>
        {
            Data = null,
            Message = "InvoicePayments cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await InvoicePaymentsData.AddInvoicePaymentsAsync(dto, cancellationToken);
}


public static async Task<ApiResult<InvoicePaymentsDTO>> UpdateInvoicePaymentsByIDAsync(
    InvoicePaymentsUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<InvoicePaymentsDTO>
        {
            Data = null,
            Message = "Invalid InvoicePayments data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await InvoicePaymentsData.UpdateInvoicePaymentsByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<InvoicePaymentsDTO>> DeleteInvoicePaymentsByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await InvoicePaymentsData.DeleteInvoicePaymentsByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<List<InvoicePaymentsViewDTO>>> SearchInvoicePayments(
    int? InvoiceID, string? PersonName,
    CancellationToken cancellationToken = default)
{
    var result = await InvoicePaymentsData.SearchInvoicePayments(
        InvoiceID, PersonName,
        cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<InvoicePaymentsViewDTO>>(
            null,
            "No InvoicePaymentss found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<InvoicePaymentsViewDTO>>(
        result.Data,
        "InvoicePaymentss retrieved successfully.",
        ErrorType.None
    );
}


public static async Task<ApiResult<InvoicePaymentsViewDTO>> GetInvoicePaymentsByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await InvoicePaymentsData.GetInvoicePaymentsByIDAsync(
        ID,
        cancellationToken);
}

}

