using System.ComponentModel.DataAnnotations;

public class InsuranceSubscriptionsDTO
{
    public int SubscriptionID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid InsuranceID.")]
    public int InsuranceID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid PersonID.")]
    public int PersonID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public InsuranceSubscriptionsDTO() { }

    public InsuranceSubscriptionsDTO(int subscriptionID, int insuranceID, int personID, DateTime startDate, DateTime endDate)
    {
        SubscriptionID = subscriptionID;
        InsuranceID = insuranceID;
        PersonID = personID;
        StartDate = startDate;
        EndDate = endDate;
    }
}

