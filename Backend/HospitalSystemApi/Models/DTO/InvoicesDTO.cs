using System.ComponentModel.DataAnnotations;

public class InvoicesDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid PatientID.")]
    public int PatientID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid AppointmentID.")]
    public int AppointmentID { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal InsuranceAmount { get; set; }
    public decimal PatientAmount { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid CreatedBy.")]
    public int CreatedBy { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid StatusID.")]
    public int StatusID { get; set; }

    public InvoicesDTO() { }

    public InvoicesDTO(int iD, int patientID, int appointmentID, DateTime invoiceDate, decimal totalAmount, decimal insuranceAmount, decimal patientAmount, int createdBy, int statusID)
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

