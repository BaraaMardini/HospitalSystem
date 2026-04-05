
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class PatientService
{

public static async Task<ApiResult<List<PatientViewDTO>>> GetAllPatientAsync(
    CancellationToken cancellationToken = default)
{
    var result = await PatientData.GetAllPatientAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<PatientViewDTO>>(
            null,
            "No Patients found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<PatientViewDTO>>(
        result.Data,
        "Patients retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<PatientViewDTO>> GetPatientByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await PatientData.GetPatientByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<PatientDTO>> AddPatientAsync(
    PatientDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<PatientDTO>
        {
            Data = null,
            Message = "Patient cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await PatientData.AddPatientAsync(dto, cancellationToken);
}


public static async Task<ApiResult<PatientUpdateDTO>> UpdatePatientByIDAsync(
    PatientUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<PatientUpdateDTO>
        {
            Data = null,
            Message = "Invalid Patient data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await PatientData.UpdatePatientByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<PatientDTO>> DeletePatientByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await PatientData.DeletePatientByIDAsync(
        ID,
        cancellationToken);
}

}

