using System.ComponentModel.DataAnnotations;

public class InsuranceSubscriptionsUpdateDTO
{
 [Required]
    public int  SubscriptionID { get; set; }
    public int?  InsuranceID { get; set; }
    public int?  PersonID { get; set; }
    public DateTime?  StartDate { get; set; }
    public DateTime?  EndDate { get; set; }

    public InsuranceSubscriptionsUpdateDTO() { }

    public InsuranceSubscriptionsUpdateDTO(int subscriptionID, int insuranceID, int personID, DateTime startDate, DateTime endDate)
    {
        SubscriptionID = subscriptionID;
        InsuranceID = insuranceID;
        PersonID = personID;
        StartDate = startDate;
        EndDate = endDate;
    }
}

