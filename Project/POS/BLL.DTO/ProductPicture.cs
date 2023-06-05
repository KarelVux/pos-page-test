using Domain.Base;

namespace BLL.DTO;

public class ProductPicture : DomainEntityId
{
    public Guid PictureId { get; set; }
    public Picture? Picture { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
}