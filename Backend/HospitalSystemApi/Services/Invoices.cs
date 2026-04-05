
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class InvoicesService
{

    public static async Task<ApiResult<List<InvoicesViewDTO>>> GetAllInvoicesByPersonIDAsync(
    int PersonID,
    CancellationToken cancellationToken = default)
    {
        var result = await InvoicesData.GetAllInvoicesByPersonIDAsync(
            PersonID,
            cancellationToken);

        if (result.Data == null || result.Data.Count == 0)
        {
            return new ApiResult<List<InvoicesViewDTO>>(
                null,
                "No Invoicess found.",
                ErrorType.NotFound
            );
        }

        return new ApiResult<List<InvoicesViewDTO>>(
            result.Data,
            "Invoicess retrieved successfully.",
            ErrorType.None
        );
    }

    public static async Task<ApiResult<List<InvoicesViewDTO>>> GetAllInvoicesAsync(
    CancellationToken cancellationToken = default)
{
    var result = await InvoicesData.GetAllInvoicesAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<InvoicesViewDTO>>(
            null,
            "No Invoicess found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<InvoicesViewDTO>>(
        result.Data,
        "Invoicess retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<InvoicesViewDTO>> GetInvoicesByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await InvoicesData.GetInvoicesByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<InvoicesDTO>> AddInvoicesAsync(
    InvoicesDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<InvoicesDTO>
        {
            Data = null,
            Message = "Invoices cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await InvoicesData.AddInvoicesAsync(dto, cancellationToken);
}


public static async Task<ApiResult<InvoicesUpdateDTO>> UpdateInvoicesByIDAsync(
    InvoicesUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<InvoicesUpdateDTO>
        {
            Data = null,
            Message = "Invalid Invoices data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await InvoicesData.UpdateInvoicesByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<InvoicesDTO>> DeleteInvoicesByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await InvoicesData.DeleteInvoicesByIDAsync(
        ID,
        cancellationToken);
}

}

