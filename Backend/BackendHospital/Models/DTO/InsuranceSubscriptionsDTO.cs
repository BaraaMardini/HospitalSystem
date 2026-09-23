using System.ComponentModel.DataAnnotations;

public class InsuranceSubscriptionsDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid InsuranceID.")]
    public int InsuranceID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid PersonID.")]
    public int PersonID { get; set; }
    public DateTime StartDate { get; set; }

    public InsuranceSubscriptionsDTO() { }

    public InsuranceSubscriptionsDTO(int iD, int insuranceID, int personID, DateTime startDate)
    {
        ID = iD;
        InsuranceID = insuranceID;
        PersonID = personID;
        StartDate = startDate;
    }
}

