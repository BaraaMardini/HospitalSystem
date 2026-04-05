
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route("api/invoicess")]
[ApiController]
public class InvoicesController : ControllerBase
{
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("personid/{personid}", Name = "GetAllInvoicesByPersonID")]
    public async Task<ActionResult> GetAllInvoicesByPersonID(
  int personid,
  CancellationToken cancellationToken)
    {
        var result = await InvoicesService.GetAllInvoicesByPersonIDAsync(
            personid,
            cancellationToken);

        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

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
        routeName: nameof(GetInvoicesByID),routeParamName:"id" );
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPut("{id}", Name = "UpdateInvoicesByID")]
public async Task<ActionResult> UpdateInvoicesByID(
    int id, [FromBody] InvoicesUpdateDTO dto,
    CancellationToken cancellationToken)  
{  
      dto.ID=id;

    var result = await InvoicesService.UpdateInvoicesByIDAsync(
      dto,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
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
}

