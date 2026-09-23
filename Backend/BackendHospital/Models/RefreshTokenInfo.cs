public class RefreshTokenInfo
{
    public int ID { get; set; }

    public int UserID { get; set; }

    public string RefreshTokenHash { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

}