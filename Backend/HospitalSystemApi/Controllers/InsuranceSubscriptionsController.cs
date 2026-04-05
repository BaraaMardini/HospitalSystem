
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;


[Route("api/insurancesubscriptionss")]
[ApiController]
public class InsuranceSubscriptionsController : ControllerBase
{
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

 [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet("{subscriptionid}", Name = "GetInsuranceSubscriptionsBySubscriptionID")]
public async Task<ActionResult> GetInsuranceSubscriptionsBySubscriptionID(
    int subscriptionid,
    CancellationToken cancellationToken)
{
    var result = await InsuranceSubscriptionsService.GetInsuranceSubscriptionsBySubscriptionIDAsync(
        subscriptionid,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
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
        newID: result.Data?.SubscriptionID,
        routeName: nameof(GetInsuranceSubscriptionsBySubscriptionID),routeParamName:"subscriptionid" );
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpPut("{subscriptionid}", Name = "UpdateInsuranceSubscriptionsBySubscriptionID")]
public async Task<ActionResult> UpdateInsuranceSubscriptionsBySubscriptionID(
    int subscriptionid, [FromBody] InsuranceSubscriptionsUpdateDTO dto,
    CancellationToken cancellationToken)  
{  
      dto.SubscriptionID=subscriptionid;

    var result = await InsuranceSubscriptionsService.UpdateInsuranceSubscriptionsBySubscriptionIDAsync(
      dto,
        cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpDelete("{subscriptionid}", Name = "DeleteInsuranceSubscriptionsBySubscriptionID")]
public async Task<ActionResult> DeleteInsuranceSubscriptionsBySubscriptionID(
    int subscriptionid, CancellationToken cancellationToken)
{

    var result = await InsuranceSubscriptionsService.DeleteInsuranceSubscriptionsBySubscriptionIDAsync(
        subscriptionid
        , cancellationToken);

    return ApiResponseHelper.GenerateApiResponse(this, result.ErrorType, result);
}
}

