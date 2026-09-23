// =====================================================================
// InsuranceSubscriptionsController
// InsuranceSubscriptions.View=67108864 / Create=134217728 / Edit=268435456 / Delete=536870912 (index=1)
// (ملاحظة: مافي أكشن Update بالكود الأصلي يلي بعتيتو، فتركتو متل ما هو)
// =====================================================================
using Microsoft.AspNetCore.Mvc;

[Route("api/insurancesubscriptionss")]
[ApiController]
public class InsuranceSubscriptionsController : ControllerBase
{
    [HasPermission(67108864, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("all", Name = "GetAllInsuranceSubscriptions")]
    public async Task<ActionResult> GetAllInsuranceSubscriptions(
        CancellationToken cancellationToken)
    {
        var result = await InsuranceSubscriptionsService.GetAllInsuranceSubscriptionsAsync(cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(134217728, 0)]
    [Audit("CreateInsuranceSubscriptions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost(Name = "AddInsuranceSubscriptions")]
    public async Task<ActionResult> AddInsuranceSubscriptions(
        [FromBody] InsuranceSubscriptionsDTO dto,
        CancellationToken cancellationToken)
    {
        var result = await InsuranceSubscriptionsService.AddInsuranceSubscriptionsAsync(dto, cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetInsuranceSubscriptionsByID), routeParamName: "id");
    }

    [HasPermission(536870912, 0)]
    [Audit("DeleteInsuranceSubscriptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}", Name = "DeleteInsuranceSubscriptionsByID")]
    public async Task<ActionResult> DeleteInsuranceSubscriptionsByID(
        int id, CancellationToken cancellationToken)
    {
        var result = await InsuranceSubscriptionsService.DeleteInsuranceSubscriptionsByIDAsync(
            id
            , cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(67108864, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("SearchInsuranceSubscriptions")]
    public async Task<ActionResult> SearchInsuranceSubscriptions(
       [FromQuery] SearchInsuranceSubscriptionsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await InsuranceSubscriptionsService.SearchInsuranceSubscriptions(
            request.PersonName, request.CompanyName, request.DurationMonths, request.EndDate,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }

    [HasPermission(67108864, 0)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetInsuranceSubscriptionsByID")]
    public async Task<ActionResult> GetInsuranceSubscriptionsByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await InsuranceSubscriptionsService.GetInsuranceSubscriptionsByIDAsync(
            id,
            cancellationToken);
        return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
    }
}