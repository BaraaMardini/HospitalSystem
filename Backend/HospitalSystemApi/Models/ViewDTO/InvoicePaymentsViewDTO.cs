using System.ComponentModel.DataAnnotations;

public class InvoicePaymentsViewDTO
{
    public int ID { get; set; }
    public int InvoiceID { get; set; }
    public int PatientID { get; set; }
    public int PersonID { get; set; }
    public string Name { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal RemainingAmount { get; set; }
    public string Notes { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public InvoicePaymentsViewDTO() { }

    public InvoicePaymentsViewDTO(int iD, int invoiceID, int patientID, int personID, string name, DateTime paymentDate, decimal amountPaid, decimal remainingAmount, string notes, string createdBy, DateTime createdAt)
    {
        ID = iD;
        InvoiceID = invoiceID;
        PatientID = patientID;
        PersonID = personID;
        Name = name;
        PaymentDate = paymentDate;
        AmountPaid = amountPaid;
        RemainingAmount = remainingAmount;
        Notes = notes;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
    }
}

