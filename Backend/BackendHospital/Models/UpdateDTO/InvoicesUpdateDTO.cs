using System.ComponentModel.DataAnnotations;

public class InvoicesUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public decimal?  TotalAmount { get; set; }
    public int?  StatusID { get; set; }

    public InvoicesUpdateDTO() { }

    public InvoicesUpdateDTO(int iD, int appointmentID, DateTime invoiceDate, decimal totalAmount, decimal insuranceAmount, decimal patientAmount, int statusID)
    {
        TotalAmount = totalAmount;
        StatusID = statusID;
    }
}

