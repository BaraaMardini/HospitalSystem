
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using ConnectionString;

public static class DoctorData
{
public static async Task<ApiResult<List<DoctorViewDTO>>> GetAllDoctorAsync(
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<DoctorViewDTO>>();
    var list = new List<DoctorViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetAllDoctor", connection) { CommandType = CommandType.StoredProcedure };
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new DoctorViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                PeopleID = reader.IsDBNull(reader.GetOrdinal("PeopleID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PeopleID")),
                DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName")),
                SpecializationName = reader.IsDBNull(reader.GetOrdinal("SpecializationName")) ? null : reader.GetString(reader.GetOrdinal("SpecializationName")),
                Qualification = reader.IsDBNull(reader.GetOrdinal("Qualification")) ? null : reader.GetString(reader.GetOrdinal("Qualification")),
                DoctorLevel = reader.IsDBNull(reader.GetOrdinal("DoctorLevel")) ? null : reader.GetString(reader.GetOrdinal("DoctorLevel")),
                Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Salary"))
            });
        }
        result.Data = list;
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching data.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<DoctorDTO>> AddDoctorAsync(
    DoctorDTO doctor,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<DoctorDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_AddDoctor", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@PersonID", SqlDbType.Int).Value = doctor.PersonID;
        command.Parameters.Add("@SpecializationID", SqlDbType.Int).Value = doctor.SpecializationID;
        command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = doctor.DepartmentID;
        command.Parameters.Add("@Qualification", SqlDbType.NVarChar, 50).Value = (object?)doctor.Qualification ?? DBNull.Value;
        command.Parameters.Add("@DoctorLevel", SqlDbType.NVarChar, 50).Value = (object?)doctor.DoctorLevel ?? DBNull.Value;
        command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = doctor.Salary;
        var outputID = new SqlParameter("@NewID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputID);
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        doctor.ID = (int)outputID.Value;
        result.Data = doctor;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while adding the doctor.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<DoctorDTO>> UpdateDoctorByIDAsync(
    DoctorUpdateDTO doctor,
    CancellationToken cancellationToken = default)
{
DoctorDTO doctorDTO = new  DoctorDTO();
    var result = new ApiResult<DoctorDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_UpdateDoctorByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = (object?)doctor.ID ?? DBNull.Value;
        command.Parameters.Add("@SpecializationID", SqlDbType.Int).Value = (object?)doctor.SpecializationID ?? DBNull.Value;
        command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = (object?)doctor.DepartmentID ?? DBNull.Value;
        command.Parameters.Add("@Qualification", SqlDbType.NVarChar, 500).Value = (object?)doctor.Qualification ?? DBNull.Value;
        command.Parameters.Add("@DoctorLevel", SqlDbType.NVarChar, 500).Value = (object?)doctor.DoctorLevel ?? DBNull.Value;
        command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = (object?)doctor.Salary ?? DBNull.Value;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            doctorDTO.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            doctorDTO.PersonID = reader.IsDBNull(reader.GetOrdinal("PersonID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PersonID"));
            doctorDTO.SpecializationID = reader.IsDBNull(reader.GetOrdinal("SpecializationID")) ? 0 : reader.GetInt32(reader.GetOrdinal("SpecializationID"));
            doctorDTO.DepartmentID = reader.IsDBNull(reader.GetOrdinal("DepartmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("DepartmentID"));
            doctorDTO.Qualification = reader.IsDBNull(reader.GetOrdinal("Qualification")) ? null : reader.GetString(reader.GetOrdinal("Qualification"));
            doctorDTO.DoctorLevel = reader.IsDBNull(reader.GetOrdinal("DoctorLevel")) ? null : reader.GetString(reader.GetOrdinal("DoctorLevel"));
            doctorDTO.Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Salary"));
        }
        reader.Close();
        result.Data = doctorDTO;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while updating the doctor";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<DoctorDTO>> DeleteDoctorByIDAsync(
    int iD,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<DoctorDTO>();
    var doctor = new DoctorDTO
    {
        ID = iD,
    };
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_DeleteDoctorByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = doctor.ID;
        var outputMsg = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var outputErrorType = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(outputMsg);
        command.Parameters.Add(outputErrorType);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        result.Data = doctor;
        result.Message = outputMsg.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(outputErrorType.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while deleting the doctor";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<List<DoctorViewDTO>>> SearchDoctor(
string? PersonName,
string? DepartmentName,
string? SpecializationName,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<List<DoctorViewDTO>>();
    var list = new List<DoctorViewDTO>();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_SearchDoctor", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@PersonName", SqlDbType.NVarChar).Value = PersonName ;
        command.Parameters.Add("@DepartmentName", SqlDbType.NVarChar).Value = DepartmentName ;
        command.Parameters.Add("@SpecializationName", SqlDbType.NVarChar).Value = SpecializationName ;
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (reader.Read())
        {
            list.Add(new DoctorViewDTO
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID")),
                PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                PeopleID = reader.IsDBNull(reader.GetOrdinal("PeopleID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PeopleID")),
                DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName")),
                SpecializationName = reader.IsDBNull(reader.GetOrdinal("SpecializationName")) ? null : reader.GetString(reader.GetOrdinal("SpecializationName")),
                Qualification = reader.IsDBNull(reader.GetOrdinal("Qualification")) ? null : reader.GetString(reader.GetOrdinal("Qualification")),
                DoctorLevel = reader.IsDBNull(reader.GetOrdinal("DoctorLevel")) ? null : reader.GetString(reader.GetOrdinal("DoctorLevel")),
                Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Salary"))
            });
        }
        result.Data = list;
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching data.";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

public static async Task<ApiResult<DoctorViewDTO>> GetDoctorByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
    var result = new ApiResult<DoctorViewDTO>();
    var dto = new DoctorViewDTO();
    try
    {
        await using var connection = new SqlConnection(connectionString._connectionString);
        await using var command = new SqlCommand("SP_GetDoctorByID", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
        var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
        var errorTypeParameter = new SqlParameter("@ErrorType", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(messageParameter);
        command.Parameters.Add(errorTypeParameter);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (reader.Read())
        {
            dto.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ID"));
            dto.PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName"));
            dto.PeopleID = reader.IsDBNull(reader.GetOrdinal("PeopleID")) ? 0 : reader.GetInt32(reader.GetOrdinal("PeopleID"));
            dto.DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName"));
            dto.SpecializationName = reader.IsDBNull(reader.GetOrdinal("SpecializationName")) ? null : reader.GetString(reader.GetOrdinal("SpecializationName"));
            dto.Qualification = reader.IsDBNull(reader.GetOrdinal("Qualification")) ? null : reader.GetString(reader.GetOrdinal("Qualification"));
            dto.DoctorLevel = reader.IsDBNull(reader.GetOrdinal("DoctorLevel")) ? null : reader.GetString(reader.GetOrdinal("DoctorLevel"));
            dto.Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Salary"));
        }
        reader.Close();
        result.Data = dto;
        result.Message = messageParameter.Value?.ToString();
        result.ErrorType = ErrorTypeMapper.GetErrorType(Convert.ToInt32(errorTypeParameter.Value));
    }
    catch (Exception ex)
    {
        result.Data = null;
        result.Message = "Database error occurred while fetching the doctor";
        result.ErrorType = ErrorType.DatabaseError;
    }
    return result;
}

}

