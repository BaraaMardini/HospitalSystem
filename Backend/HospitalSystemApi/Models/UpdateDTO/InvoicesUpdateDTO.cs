using System.ComponentModel.DataAnnotations;

public class InvoicesUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public int?  PatientID { get; set; }
    public int?  AppointmentID { get; set; }
    public DateTime?  InvoiceDate { get; set; }
    public decimal?  TotalAmount { get; set; }
    public decimal?  InsuranceAmount { get; set; }
    public decimal?  PatientAmount { get; set; }
    public int?  CreatedBy { get; set; }
    public int?  StatusID { get; set; }

    public InvoicesUpdateDTO() { }

    public InvoicesUpdateDTO(int iD, int patientID, int appointmentID, DateTime invoiceDate, decimal totalAmount, decimal insuranceAmount, decimal patientAmount, int createdBy, int statusID)
    {
        ID = iD;
        PatientID = patientID;
        AppointmentID = appointmentID;
        InvoiceDate = invoiceDate;
        TotalAmount = totalAmount;
        InsuranceAmount = insuranceAmount;
        PatientAmount = patientAmount;
        CreatedBy = createdBy;
        StatusID = statusID;
    }
}

