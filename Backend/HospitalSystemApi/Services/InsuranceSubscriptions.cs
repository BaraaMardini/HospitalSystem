
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class InsuranceSubscriptionsService
{

public static async Task<ApiResult<List<InsuranceSubscriptionsViewDTO>>> GetAllInsuranceSubscriptionsAsync(
    CancellationToken cancellationToken = default)
{
    var result = await InsuranceSubscriptionsData.GetAllInsuranceSubscriptionsAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<InsuranceSubscriptionsViewDTO>>(
            null,
            "No InsuranceSubscriptionss found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<InsuranceSubscriptionsViewDTO>>(
        result.Data,
        "InsuranceSubscriptionss retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<InsuranceSubscriptionsViewDTO>> GetInsuranceSubscriptionsBySubscriptionIDAsync(
    int SubscriptionID,
    CancellationToken cancellationToken = default)
{
  
    return await InsuranceSubscriptionsData.GetInsuranceSubscriptionsBySubscriptionIDAsync(
        SubscriptionID,
        cancellationToken);
}


public static async Task<ApiResult<InsuranceSubscriptionsDTO>> AddInsuranceSubscriptionsAsync(
    InsuranceSubscriptionsDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<InsuranceSubscriptionsDTO>
        {
            Data = null,
            Message = "InsuranceSubscriptions cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await InsuranceSubscriptionsData.AddInsuranceSubscriptionsAsync(dto, cancellationToken);
}


public static async Task<ApiResult<InsuranceSubscriptionsUpdateDTO>> UpdateInsuranceSubscriptionsBySubscriptionIDAsync(
    InsuranceSubscriptionsUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<InsuranceSubscriptionsUpdateDTO>
        {
            Data = null,
            Message = "Invalid InsuranceSubscriptions data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await InsuranceSubscriptionsData.UpdateInsuranceSubscriptionsBySubscriptionIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<InsuranceSubscriptionsDTO>> DeleteInsuranceSubscriptionsBySubscriptionIDAsync(
    int SubscriptionID,
    CancellationToken cancellationToken = default)
{
   

    return await InsuranceSubscriptionsData.DeleteInsuranceSubscriptionsBySubscriptionIDAsync(
        SubscriptionID,
        cancellationToken);
}

}

