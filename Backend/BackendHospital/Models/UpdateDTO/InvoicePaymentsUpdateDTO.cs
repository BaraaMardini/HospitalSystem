using System.ComponentModel.DataAnnotations;

public class InvoicePaymentsUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  Notes { get; set; }

    public InvoicePaymentsUpdateDTO() { }

    public InvoicePaymentsUpdateDTO(int iD, int invoiceID, DateTime paymentDate, decimal amountPaid, string notes)
    {
        Notes = notes;
    }
}

