using System.ComponentModel.DataAnnotations;

public class InvoicesViewDTO
{
    public int ID { get; set; }
    public int PatientID { get; set; }
    public int PersonID { get; set; }
    public string PersonName { get; set; }
    public int AppointmentID { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal InsuranceAmount { get; set; }
    public decimal PatientAmount { get; set; }
    public int CreatedBy { get; set; }
    public string StatusDescription { get; set; }

    public InvoicesViewDTO() { }

    public InvoicesViewDTO(int iD, int patientID, int personID, string personName, int appointmentID, DateTime invoiceDate, decimal totalAmount, decimal insuranceAmount, decimal patientAmount, int createdBy, string statusDescription)
    {
        ID = iD;
        PatientID = patientID;
        PersonID = personID;
        PersonName = personName;
        AppointmentID = appointmentID;
        InvoiceDate = invoiceDate;
        TotalAmount = totalAmount;
        InsuranceAmount = insuranceAmount;
        PatientAmount = patientAmount;
        CreatedBy = createdBy;
        StatusDescription = statusDescription;
    }
}

