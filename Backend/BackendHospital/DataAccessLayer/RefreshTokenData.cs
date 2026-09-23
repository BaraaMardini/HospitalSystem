using System.Data;
using Microsoft.Data.SqlClient;
using ConnectionString;
using BackendHospital.Models;

public static class RefreshTokenData
{
    // =====================================================
    // Create
    // =====================================================

    public static async Task<ApiResult<int>> Create(
        int userId,
        string refreshTokenHash,
        DateTime expiresAt,
        CancellationToken cancellationToken)
    {
        var result =
            new ApiResult<int>();

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_CreateRefreshToken",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };


            command.Parameters.Add(
                "@UserID",
                SqlDbType.Int).Value =
                    userId;


            command.Parameters.Add(
                "@RefreshTokenHash",
                SqlDbType.NVarChar,
                500).Value =
                    refreshTokenHash;


            command.Parameters.Add(
                "@ExpiresAt",
                SqlDbType.DateTime2).Value =
                    expiresAt;


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


            var returnedId =
                await command.ExecuteScalarAsync(
                    cancellationToken);


            result.Data =
                returnedId == null ||
                returnedId == DBNull.Value
                    ? 0
                    : Convert.ToInt32(
                        returnedId);


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
            result.Data = 0;

            result.Message =
                "Database error while creating refresh token.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }


    // =====================================================
    // Get By ID
    // =====================================================

    public static async Task<ApiResult<RefreshTokenInfo>> GetByID(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            new ApiResult<RefreshTokenInfo>();

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_GetRefreshTokenByID",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };


            command.Parameters.Add(
                "@ID",
                SqlDbType.Int).Value =
                    id;


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
                    new RefreshTokenInfo
                    {
                        ID =
                            reader.GetInt32(
                                reader.GetOrdinal("ID")),

                        UserID =
                            reader.GetInt32(
                                reader.GetOrdinal("UserID")),

                        RefreshTokenHash =
                            reader.GetString(
                                reader.GetOrdinal(
                                    "RefreshTokenHash")),

                        ExpiresAt =
                            reader.GetDateTime(
                                reader.GetOrdinal(
                                    "ExpiresAt")),

                        RevokedAt =
                            reader.IsDBNull(
                                reader.GetOrdinal(
                                    "RevokedAt"))
                                ? null
                                : reader.GetDateTime(
                                    reader.GetOrdinal(
                                        "RevokedAt"))
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
                "Database error while getting refresh token.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }


    // =====================================================
    // Rotate
    // =====================================================

    public static async Task<ApiResult<int>> Rotate(
        int oldTokenId,
        int userId,
        string newRefreshTokenHash,
        DateTime newExpiresAt,
        CancellationToken cancellationToken)
    {
        var result =
            new ApiResult<int>();

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_RotateRefreshToken",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };


            command.Parameters.Add(
                "@OldTokenID",
                SqlDbType.Int).Value =
                    oldTokenId;


            command.Parameters.Add(
                "@UserID",
                SqlDbType.Int).Value =
                    userId;


            command.Parameters.Add(
                "@NewRefreshTokenHash",
                SqlDbType.NVarChar,
                500).Value =
                    newRefreshTokenHash;


            command.Parameters.Add(
                "@NewExpiresAt",
                SqlDbType.DateTime2).Value =
                    newExpiresAt;


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


            var returnedId =
                await command.ExecuteScalarAsync(
                    cancellationToken);


            result.Data =
                returnedId == null ||
                returnedId == DBNull.Value
                    ? 0
                    : Convert.ToInt32(
                        returnedId);


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
            result.Data = 0;

            result.Message =
                "Database error while rotating refresh token.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }

    // =====================================================
    // Revoke
    // =====================================================

    public static async Task<ApiResult<int>> Revoke(
        int tokenId,
        int userId,
        CancellationToken cancellationToken)
    {
        var result =
            new ApiResult<int>();

        try
        {
            await using var connection =
                new SqlConnection(
                    connectionString._connectionString);

            await using var command =
                new SqlCommand(
                    "SP_RevokeRefreshToken",
                    connection)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };

            command.Parameters.Add(
                "@TokenID",
                SqlDbType.Int).Value =
                    tokenId;

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

            var returnedId =
                await command.ExecuteScalarAsync(
                    cancellationToken);

            result.Data =
                returnedId == null ||
                returnedId == DBNull.Value
                    ? 0
                    : Convert.ToInt32(
                        returnedId);

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
            result.Data = 0;

            result.Message =
                "Database error while revoking refresh token.";

            result.ErrorType =
                ErrorType.DatabaseError;
        }

        return result;
    }
}