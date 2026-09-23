
// =====================================================================
// InvoicesController
// Invoices.View=17179869184 / Create=34359738368 / Edit=68719476736 / Delete=137438953472 (index=1)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/invoicess")]
[ApiController]
public class InvoicesController : ControllerBase
{
    [HasPermission(17179869184, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllInvoices")]
    public async Task<ActionResult> GetAllInvoices(
        CancellationToken cancellationToken)
    {
        var result = await InvoicesService.GetAllInvoicesAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(34359738368, 0)]
    [Audit("CreateInvoices")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddInvoices")]
    public async Task<ActionResult> AddInvoices(
        [FromBody] InvoicesDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await InvoicesService.AddInvoicesAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetInvoicesByID), routeParamName: "id");
    }

    [HasPermission(68719476736, 0)]
    [Audit("UpdateInvoices")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}", Name = "UpdateInvoicesByID")]
    public async Task<ActionResult> UpdateInvoicesByID(
        int id, [FromBody] InvoicesUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;
        var result = await InvoicesService.UpdateInvoicesByIDAsync(
            dto,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(137438953472, 0)]
    [Audit("DeleteInvoices")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteInvoicesByID")]
    public async Task<ActionResult> DeleteInvoicesByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await InvoicesService.DeleteInvoicesByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(17179869184, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchInvoices")]
    public async Task<ActionResult> SearchInvoices(
       [FromQuery] SearchInvoicesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await InvoicesService.SearchInvoices(
            request.AppointmentID, request.PersonName, request.StatusName,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(17179869184, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetInvoicesByID")]
    public async Task<ActionResult> GetInvoicesByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await InvoicesService.GetInvoicesByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}
