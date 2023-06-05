using Domain.Base;
using Domain.Contracts.Base;

namespace DAL.DTO.Identity;

public class AppRefreshToken : BaseRefreshToken, IDomainEntityId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}