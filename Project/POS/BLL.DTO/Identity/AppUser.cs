using Microsoft.AspNetCore.Identity;

namespace BLL.DTO.Identity;

public class AppUser : IdentityUser<Guid>
{
    public ICollection<AppRefreshToken>? AppRefreshTokens { get; set; }
    public ICollection<Invoice>? Invoices { get; set; }
    public ICollection<BusinessManager>? BusinessManagers { get; set; }
}