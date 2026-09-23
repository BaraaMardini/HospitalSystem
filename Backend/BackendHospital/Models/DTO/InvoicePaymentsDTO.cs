using System.ComponentModel.DataAnnotations;

public class InvoicePaymentsDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid InvoiceID.")]
    public int InvoiceID { get; set; }

    public decimal AmountPaid { get; set; }
    public string Notes { get; set; }

    public InvoicePaymentsDTO() { }

    public InvoicePaymentsDTO(int iD, int invoiceID, decimal amountPaid, string notes)
    {
        ID = iD;
        InvoiceID = invoiceID;
        AmountPaid = amountPaid;
        Notes = notes;
    }
}

