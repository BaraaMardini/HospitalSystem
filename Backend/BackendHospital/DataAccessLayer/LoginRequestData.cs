using System.Data;
using Microsoft.Data.SqlClient;
using ConnectionString;
using BackendHospital.Models;

public static class LoginRequestData
{
    // =====================================================
    // Login
    // =====================================================

    public static async Task<ApiResult<UserInfo>> LoginRequest(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var result =
            new ApiResult<UserInfo>();

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_LoginRequest",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };


            command.Parameters.Add(
                "@Username",
                SqlDbType.NVarChar,
                256).Value =
                    request.UserName ??
                    (object)DBNull.Value;


            var messageParameter =
                new SqlParameter(
                    "@Message",
                    SqlDbType.NVarChar,
                    250)
                {
                    Direction =
                        ParameterDirection.Output
                };


            var errorTypeParameter =
                new SqlParameter(
                    "@ErrorType",
                    SqlDbType.Int)
                {
                    Direction =
                        ParameterDirection.Output
                };


            command.Parameters.Add(
                messageParameter);

            command.Parameters.Add(
                errorTypeParameter);


            await connection.OpenAsync(
                cancellationToken);


            using var reader =
                await command.ExecuteReaderAsync(
                    cancellationToken);


            if (await reader.ReadAsync(
                    cancellationToken))
            {
                result.Data =
                    new UserInfo
                    {
                        UserID =
                            reader.GetInt32(
                                reader.GetOrdinal(
                                    "UserID")),

                        UserName =
                            reader.GetString(
                                reader.GetOrdinal(
                                    "UserName")),

                        PasswordHash =
                            reader.GetString(
                                reader.GetOrdinal(
                                    "PasswordHash")),

                        PermissionMask1 =
                            reader.GetInt64(
                                reader.GetOrdinal(
                                    "PermissionMask1")),

                        PermissionMask2 =
                            reader.GetInt64(
                                reader.GetOrdinal(
                                    "PermissionMask2")),

                        Role =
                            reader.GetString(
                                reader.GetOrdinal(
                                    "Role"))
                    };
            }


            result.Message =
                messageParameter.Value ==
                DBNull.Value
                    ? null
                    : messageParameter.Value?.ToString();


            int errorCode =
                errorTypeParameter.Value ==
                DBNull.Value
                    ? 0
                    : Convert.ToInt32(
                        errorTypeParameter.Value);


            result.ErrorType =
                ErrorTypeMapper.GetErrorType(
                    errorCode);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            result.Data = null;

            result.Message =
                "A database error occurred while processing the login request.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }


    // =====================================================
    // Get User By ID
    // =====================================================

    public static async Task<ApiResult<UserInfo>> GetUserByID(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var result =
            new ApiResult<UserInfo>();

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_GetUserByID",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };


            command.Parameters.Add(
                "@UserID",
                SqlDbType.Int).Value =
                    userId;


            var messageParameter =
                new SqlParameter(
                    "@Message",
                    SqlDbType.NVarChar,
                    250)
                {
                    Direction =
                        ParameterDirection.Output
                };


            var errorTypeParameter =
                new SqlParameter(
                    "@ErrorType",
                    SqlDbType.Int)
                {
                    Direction =
                        ParameterDirection.Output
                };


            command.Parameters.Add(
                messageParameter);

            command.Parameters.Add(
                errorTypeParameter);


            await connection.OpenAsync(
                cancellationToken);


            using var reader =
                await command.ExecuteReaderAsync(
                    cancellationToken);


            if (await reader.ReadAsync(
                    cancellationToken))
            {
                result.Data =
                    new UserInfo
                    {
                        UserID =
                            reader.GetInt32(
                                reader.GetOrdinal(
                                    "UserID")),

                        UserName =
                            reader.GetString(
                                reader.GetOrdinal(
                                    "UserName")),

                        PasswordHash =
                            reader.GetString(
                                reader.GetOrdinal(
                                    "PasswordHash")),

                        PermissionMask1 =
                            reader.GetInt64(
                                reader.GetOrdinal(
                                    "PermissionMask1")),

                        PermissionMask2 =
                            reader.GetInt64(
                                reader.GetOrdinal(
                                    "PermissionMask2")),

                        Role =
                            reader.GetString(
                                reader.GetOrdinal(
                                    "Role"))
                    };
            }


            result.Message =
                messageParameter.Value?.ToString();


            int errorCode =
                errorTypeParameter.Value ==
                DBNull.Value
                    ? 0
                    : Convert.ToInt32(
                        errorTypeParameter.Value);


            result.ErrorType =
                ErrorTypeMapper.GetErrorType(
                    errorCode);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            result.Data = null;

            result.Message =
                "Database error while getting user.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }
}