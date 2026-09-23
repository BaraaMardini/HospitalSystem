
// =====================================================================
// InvoicePaymentsController
// InvoicePayments.View=1073741824 / Create=2147483648 / Edit=4294967296 / Delete=8589934592 (index=1)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/invoicepaymentss")]
[ApiController]
public class InvoicePaymentsController : ControllerBase
{
    [HasPermission(1073741824, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllInvoicePayments")]
    public async Task<ActionResult> GetAllInvoicePayments(
        CancellationToken cancellationToken)
    {
        var result = await InvoicePaymentsService.GetAllInvoicePaymentsAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(2147483648, 0)]
    [Audit("CreateInvoicePayments")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddInvoicePayments")]
    public async Task<ActionResult> AddInvoicePayments(
        [FromBody] InvoicePaymentsDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await InvoicePaymentsService.AddInvoicePaymentsAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetInvoicePaymentsByID), routeParamName: "id");
    }

    [HasPermission(4294967296, 0)]
    [Audit("UpdateInvoicePayments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateInvoicePaymentsByID")]
    public async Task<ActionResult> UpdateInvoicePaymentsByID(
        int id, [FromBody] InvoicePaymentsUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await InvoicePaymentsService.UpdateInvoicePaymentsByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(8589934592, 0)]
    [Audit("DeleteInvoicePayments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteInvoicePaymentsByID")]
    public async Task<ActionResult> DeleteInvoicePaymentsByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await InvoicePaymentsService.DeleteInvoicePaymentsByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(1073741824, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchInvoicePayments")]
    public async Task<ActionResult> SearchInvoicePayments(
       [FromQuery] SearchInvoicePaymentsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await InvoicePaymentsService.SearchInvoicePayments(
            request.InvoiceID, request.PersonName,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(1073741824, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetInvoicePaymentsByID")]
    public async Task<ActionResult> GetInvoicePaymentsByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await InvoicePaymentsService.GetInvoicePaymentsByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}
