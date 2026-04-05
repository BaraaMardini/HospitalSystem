
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class PeopleService
{

public static async Task<ApiResult<List<PeopleViewDTO>>> GetAllPeopleAsync(
    CancellationToken cancellationToken = default)
{
    var result = await PeopleData.GetAllPeopleAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<PeopleViewDTO>>(
            null,
            "No Peoples found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<PeopleViewDTO>>(
        result.Data,
        "Peoples retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<PeopleViewDTO>> GetPeopleByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await PeopleData.GetPeopleByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<PeopleDTO>> AddPeopleAsync(
    PeopleDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<PeopleDTO>
        {
            Data = null,
            Message = "People cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await PeopleData.AddPeopleAsync(dto, cancellationToken);
}


public static async Task<ApiResult<PeopleUpdateDTO>> UpdatePeopleByIDAsync(
    PeopleUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<PeopleUpdateDTO>
        {
            Data = null,
            Message = "Invalid People data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await PeopleData.UpdatePeopleByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<PeopleDTO>> DeletePeopleByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await PeopleData.DeletePeopleByIDAsync(
        ID,
        cancellationToken);
}

}

