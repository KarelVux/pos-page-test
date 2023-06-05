using Domain.Base;

namespace BLL.DTO;

public class BusinessManager : DomainEntityId
{
    public Guid AppUserId { get; set; }

    public Guid BusinessId { get; set; }
    public Business? Business { get; set; }
}