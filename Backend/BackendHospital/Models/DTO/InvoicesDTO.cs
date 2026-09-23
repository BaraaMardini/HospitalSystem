using System.ComponentModel.DataAnnotations;

public class InvoicesDTO
{
    public int ID { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid AppointmentID.")]
    public int AppointmentID { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal InsuranceAmount { get; set; }
    public decimal PatientAmount { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid StatusID.")]
    public int StatusID { get; set; }

    public InvoicesDTO() { }

    public InvoicesDTO(int iD, int appointmentID,  decimal totalAmount, decimal insuranceAmount, decimal patientAmount, int statusID)
    {
        ID = iD;
        AppointmentID = appointmentID;
        TotalAmount = totalAmount;
        InsuranceAmount = insuranceAmount;
        PatientAmount = patientAmount;
        StatusID = statusID;
    }
}

