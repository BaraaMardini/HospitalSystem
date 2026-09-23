
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class AppointmentService
{

public static async Task<ApiResult<List<AppointmentViewDTO>>> GetAllAppointmentAsync(
    CancellationToken cancellationToken = default)
{
    var result = await AppointmentData.GetAllAppointmentAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<AppointmentViewDTO>>(
            null,
            "No Appointments found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<AppointmentViewDTO>>(
        result.Data,
        "Appointments retrieved successfully.",
        ErrorType.None
    );
}


public static async Task<ApiResult<AppointmentDTO>> AddAppointmentAsync(
    AppointmentDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<AppointmentDTO>
        {
            Data = null,
            Message = "Appointment cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await AppointmentData.AddAppointmentAsync(dto, cancellationToken);
}


public static async Task<ApiResult<AppointmentDTO>> UpdateAppointmentByIDAsync(
    AppointmentUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<AppointmentDTO>
        {
            Data = null,
            Message = "Invalid Appointment data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await AppointmentData.UpdateAppointmentByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<AppointmentDTO>> DeleteAppointmentByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await AppointmentData.DeleteAppointmentByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<List<AppointmentViewDTO>>> SearchAppointment(
    string? DoctorName, string? PatientName, string? RoomNumber, DateTime? StartDate, DateTime? EndDate,
    CancellationToken cancellationToken = default)
{
    var result = await AppointmentData.SearchAppointment(
        DoctorName, PatientName, RoomNumber, StartDate, EndDate,
        cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<AppointmentViewDTO>>(
            null,
            "No Appointments found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<AppointmentViewDTO>>(
        result.Data,
        "Appointments retrieved successfully.",
        ErrorType.None
    );
}


public static async Task<ApiResult<AppointmentViewDTO>> GetAppointmentByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await AppointmentData.GetAppointmentByIDAsync(
        ID,
        cancellationToken);
}

}

