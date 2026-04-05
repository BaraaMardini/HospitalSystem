using System.ComponentModel.DataAnnotations;

public class InvoicePaymentsUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public int?  InvoiceID { get; set; }
    public DateTime?  PaymentDate { get; set; }
    public decimal?  AmountPaid { get; set; }
    public decimal?  RemainingAmount { get; set; }
    public string?  Notes { get; set; }
    public string?  CreatedBy { get; set; }
    public DateTime?  CreatedAt { get; set; }

    public InvoicePaymentsUpdateDTO() { }

    public InvoicePaymentsUpdateDTO(int iD, int invoiceID, DateTime paymentDate, decimal amountPaid, decimal remainingAmount, string notes, string createdBy, DateTime createdAt)
    {
        ID = iD;
        InvoiceID = invoiceID;
        PaymentDate = paymentDate;
        AmountPaid = amountPaid;
        RemainingAmount = remainingAmount;
        Notes = notes;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
    }
}

