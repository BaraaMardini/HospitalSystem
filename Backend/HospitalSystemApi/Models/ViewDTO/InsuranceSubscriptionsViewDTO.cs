using System.ComponentModel.DataAnnotations;

public class InsuranceSubscriptionsViewDTO
{
    public int SubscriptionID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string CompanyName { get; set; }
    public int CoveragePercentage { get; set; }
    public int DefaultDurationMonths { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public InsuranceSubscriptionsViewDTO() { }

    public InsuranceSubscriptionsViewDTO(int subscriptionID, string name, int age, string companyName, int coveragePercentage, int defaultDurationMonths, DateTime startDate, DateTime endDate)
    {
        SubscriptionID = subscriptionID;
        Name = name;
        Age = age;
        CompanyName = companyName;
        CoveragePercentage = coveragePercentage;
        DefaultDurationMonths = defaultDurationMonths;
        StartDate = startDate;
        EndDate = endDate;
    }
}

