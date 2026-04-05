using System.ComponentModel.DataAnnotations;

public class InsuranceDTO
{
    public int ID { get; set; }
    public string CompanyName { get; set; }
    public string PolicyNumber { get; set; }
    public int CoveragePercentage { get; set; }
    public int DefaultDurationMonths { get; set; }
    public string Notes { get; set; }

    public InsuranceDTO() { }

    public InsuranceDTO(int iD, string companyName, string policyNumber, int coveragePercentage, int defaultDurationMonths, string notes)
    {
        ID = iD;
        CompanyName = companyName;
        PolicyNumber = policyNumber;
        CoveragePercentage = coveragePercentage;
        DefaultDurationMonths = defaultDurationMonths;
        Notes = notes;
    }
}

