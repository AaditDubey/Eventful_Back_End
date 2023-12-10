using System.ComponentModel.DataAnnotations;
using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Identity
{
    public class RefreshToken : BaseEntity
    {
        //public RefreshToken() { }

        //public RefreshToken(Guid userId, string token, string ipAddress, DateTimeOffset expiresDate)
        //{
        //    Id = Guid.NewGuid();
        //    UserId = userId;
        //    Token = token;
        //    IpAddress = ipAddress;
        //    ExpiresDate = expiresDate;
        //    CreatedDate = DateTimeOffset.UtcNow;
        //}
        public string UserId { get; set; }
        public string Token { get; set; }
        public string IpAddress { get; set; }
        public string ReplacedByToken { get; set; }
        [Required] public DateTimeOffset ExpiresDate { get; set; }
        [Required] public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? RevokedDate { get; set; }
    }
}
