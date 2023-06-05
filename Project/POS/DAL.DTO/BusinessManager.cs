using Domain.Base;

namespace DAL.DTO;

public class BusinessManager : DomainEntityId
{
    public Guid AppUserId { get; set; }

    public Guid BusinessId { get; set; }
    public Business? Business { get; set; }
}