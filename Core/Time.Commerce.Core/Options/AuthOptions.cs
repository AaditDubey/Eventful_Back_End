namespace Time.Commerce.Core.Options
{
    public partial class AuthOptions
    {
        public string Secret { get; set; }
        public string AuthenticateScheme { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
        public string ApiName { get; set; }
        public long TokenExpiresTimeMinutesIn { get; set; }
        public long RefreshTokenExpiresTimeMinutesIn { get; set; }
    }
}
