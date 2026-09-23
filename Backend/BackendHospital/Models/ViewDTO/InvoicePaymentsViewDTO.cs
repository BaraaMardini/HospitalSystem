using System.ComponentModel.DataAnnotations;

public class InvoicePaymentsViewDTO
{
    public int ID { get; set; }
    public int InvoiceID { get; set; }
    public string PersonName { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal AmountPaid { get; set; }
    public string Notes { get; set; }
    public int AppointmentID { get; set; }
    public decimal RemainingAmount { get; set; }

    public InvoicePaymentsViewDTO() { }

    public InvoicePaymentsViewDTO(int iD, int invoiceID, string personName, DateTime paymentDate, decimal amountPaid, string notes, int appointmentID, decimal remainingAmount)
    {
        ID = iD;
        InvoiceID = invoiceID;
        PersonName = personName;
        PaymentDate = paymentDate;
        AmountPaid = amountPaid;
        Notes = notes;
        AppointmentID = appointmentID;
        RemainingAmount = remainingAmount;
    }
}

