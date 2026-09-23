using System.ComponentModel.DataAnnotations;

public class InvoicesViewDTO
{
    public int ID { get; set; }
    public int AppointmentID { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string PersonName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal InsuranceAmount { get; set; }
    public decimal PatientAmount { get; set; }
    public string StatusName { get; set; }
    public string StatusDescription { get; set; }

    public InvoicesViewDTO() { }

    public InvoicesViewDTO(int iD, int appointmentID, DateTime invoiceDate, string personName, decimal totalAmount, decimal insuranceAmount, decimal patientAmount, string statusName, string statusDescription)
    {
        ID = iD;
        AppointmentID = appointmentID;
        InvoiceDate = invoiceDate;
        PersonName = personName;
        TotalAmount = totalAmount;
        InsuranceAmount = insuranceAmount;
        PatientAmount = patientAmount;
        StatusName = statusName;
        StatusDescription = statusDescription;
    }
}

