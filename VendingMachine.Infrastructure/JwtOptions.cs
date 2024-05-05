namespace VendingMachine.Infrastructure
{
    public class JwtOptions
    {
        public string Cookie {  get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresHours { get; set; }
    }
}