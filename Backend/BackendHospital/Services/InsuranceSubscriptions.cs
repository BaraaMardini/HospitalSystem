
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


public static async Task<ApiResult<InsuranceSubscriptionsDTO>> DeleteInsuranceSubscriptionsByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await InsuranceSubscriptionsData.DeleteInsuranceSubscriptionsByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<List<InsuranceSubscriptionsViewDTO>>> SearchInsuranceSubscriptions(
    string? PersonName, string? CompanyName, int? DurationMonths, DateTime? EndDate,
    CancellationToken cancellationToken = default)
{
    var result = await InsuranceSubscriptionsData.SearchInsuranceSubscriptions(
        PersonName, CompanyName, DurationMonths, EndDate,
        cancellationToken);

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


public static async Task<ApiResult<InsuranceSubscriptionsViewDTO>> GetInsuranceSubscriptionsByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await InsuranceSubscriptionsData.GetInsuranceSubscriptionsByIDAsync(
        ID,
        cancellationToken);
}

}

