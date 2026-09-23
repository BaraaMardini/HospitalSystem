using BackendHospital.Models;

public static class RefreshTokenService
{
    public static async Task<ApiResult<int>> Create(
        int userId,
        string refreshTokenHash,
        DateTime expiresAt,
        CancellationToken cancellationToken)
    {
        return await RefreshTokenData.Create(
            userId,
            refreshTokenHash,
            expiresAt,
            cancellationToken);
    }


    public static async Task<ApiResult<RefreshTokenInfo>> GetByID(
        int id,
        CancellationToken cancellationToken)
    {
        return await RefreshTokenData.GetByID(
            id,
            cancellationToken);
    }


    public static async Task<ApiResult<int>> Rotate(
        int oldTokenId,
        int userId,
        string newRefreshTokenHash,
        DateTime newExpiresAt,
        CancellationToken cancellationToken)
    {
        return await RefreshTokenData.Rotate(
            oldTokenId,
            userId,
            newRefreshTokenHash,
            newExpiresAt,
            cancellationToken);
    }

    public static async Task<ApiResult<int>> Revoke(
    int tokenId,
    int userId,
    CancellationToken cancellationToken)
    {
        return await RefreshTokenData.Revoke(
            tokenId,
            userId,
            cancellationToken);
    }
}