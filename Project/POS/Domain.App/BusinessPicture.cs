using Domain.App;
using Domain.Base;

namespace Domain.App;

public class BusinessPicture : DomainEntityId
{
    public Guid PictureId { get; set; }
    public Picture? Picture { get; set; }


    public Guid BusinessId { get; set; }
    public Business? Business { get; set; }
}