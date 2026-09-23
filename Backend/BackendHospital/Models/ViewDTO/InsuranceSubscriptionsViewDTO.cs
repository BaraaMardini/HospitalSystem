using System.ComponentModel.DataAnnotations;

public class InsuranceSubscriptionsViewDTO
{
    public int ID { get; set; }
    public string PersonName { get; set; }
    public decimal Price { get; set; }
    public string CompanyName { get; set; }
    public int CoveragePercentage { get; set; }
    public int DurationMonths { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public InsuranceSubscriptionsViewDTO() { }

    public InsuranceSubscriptionsViewDTO(int iD, string personName, decimal price, string companyName, int coveragePercentage, int durationMonths, DateTime startDate, DateTime endDate)
    {
        ID = iD;
        PersonName = personName;
        Price = price;
        CompanyName = companyName;
        CoveragePercentage = coveragePercentage;
        DurationMonths = durationMonths;
        StartDate = startDate;
        EndDate = endDate;
    }
}

