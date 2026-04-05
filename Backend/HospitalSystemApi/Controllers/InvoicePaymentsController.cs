
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route("api/invoicepaymentss")]
[ApiController]
public class InvoicePaymentsController : ControllerBase
{
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
        routeName: nameof(GetInvoicePaymentsByID),routeParamName:"id" );
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPut("{id}", Name = "UpdateInvoicePaymentsByID")]
public async Task<ActionResult> UpdateInvoicePaymentsByID(
    int id, [FromBody] InvoicePaymentsUpdateDTO dto,
    CancellationToken cancellationToken)  
{  
      dto.ID=id;

    var result = await InvoicePaymentsService.UpdateInvoicePaymentsByIDAsync(
      dto,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
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
}

