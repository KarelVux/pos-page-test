using Domain.App.Identity;
using Domain.Base;

namespace Domain.App;

public class BusinessManager : DomainEntityId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid BusinessId { get; set; }
    public Business? Business { get; set; }
}