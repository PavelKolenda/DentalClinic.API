namespace DentalClinic.Services.Options;
public class JwtSettings
{
    public string? SigningKey { get; set; }
    public string? Issuer { get; set; }
    public int? DaysLiveTime { get; set; }
}
