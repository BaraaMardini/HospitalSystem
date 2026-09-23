using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/appointments")]
public class AppointmentController : ControllerBase
{
    // =====================================================
    // VIEW = 64
    // =====================================================

    [HasPermission(64, 0)]
    [HttpGet("all", Name = "GetAllAppointment")]
    public async Task<ActionResult> GetAllAppointment(
        CancellationToken cancellationToken)
    {
        var result =
            await AppointmentService.GetAllAppointmentAsync(
                cancellationToken);

        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result);
    }


    // =====================================================
    // CREATE = 128
    // AUDIT
    // =====================================================

    [HasPermission(128, 0)]
    [Audit("CreateAppointment")]
    [HttpPost(Name = "AddAppointment")]
    public async Task<ActionResult> AddAppointment(
        [FromBody] AppointmentDTO dto,
        CancellationToken cancellationToken)
    {
        var result =
            await AppointmentService.AddAppointmentAsync(
                dto,
                cancellationToken);

        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result,
            newID: result.Data?.ID,
            routeName: nameof(GetAppointmentByID),
            routeParamName: "id");
    }


    // =====================================================
    // EDIT = 256
    // AUDIT
    // =====================================================

    [HasPermission(256, 0)]
    [Audit("UpdateAppointment")]
    [HttpPut("{id}", Name = "UpdateAppointmentByID")]
    public async Task<ActionResult> UpdateAppointmentByID(
        int id,
        [FromBody] AppointmentUpdateDTO dto,
        CancellationToken cancellationToken)
    {
        dto.ID = id;

        var result =
            await AppointmentService.UpdateAppointmentByIDAsync(
                dto,
                cancellationToken);

        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result);
    }


    // =====================================================
    // DELETE = 512
    // AUDIT
    // =====================================================

    [HasPermission(512, 0)]
    [Audit("DeleteAppointment")]
    [HttpDelete("{id}", Name = "DeleteAppointmentByID")]
    public async Task<ActionResult> DeleteAppointmentByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await AppointmentService.DeleteAppointmentByIDAsync(
                id,
                cancellationToken);

        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result);
    }


    // =====================================================
    // VIEW = 64
    // =====================================================

    [HasPermission(64, 0)]
    [HttpGet("SearchAppointment")]
    public async Task<ActionResult> SearchAppointment(
        [FromQuery] SearchAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await AppointmentService.SearchAppointment(
                request.DoctorName,
                request.PatientName,
                request.RoomNumber,
                request.StartDate,
                request.EndDate,
                cancellationToken);

        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result);
    }


    // =====================================================
    // VIEW = 64
    // =====================================================

    [HasPermission(64, 0)]
    [HttpGet("{id}", Name = "GetAppointmentByID")]
    public async Task<ActionResult> GetAppointmentByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await AppointmentService.GetAppointmentByIDAsync(
                id,
                cancellationToken);

        return ApiResponseHelper.GenerateApiResponse(
            this,
            result.ErrorType,
            result);

    }
}