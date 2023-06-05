using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class ProductService :
    BaseEntityService<BllDto.Product, DalDto.Product, IProductRepository>, IProductService
{
    protected IAppUOW Uow;

    public ProductService(IAppUOW uow, IMapper<BllDto.Product, DalDto.Product> mapper)
        : base(uow.ProductRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<BllDto.Product>> AllAsyncWithInclude()
    {
        return (await Repository.AllAsyncWithInclude()).Select(e => Mapper.Map(e))!;
    }

    public async Task<BllDto.Product?> FindAsyncWithInclude(Guid id)
    {
        return Mapper.Map(await Repository.FindAsyncWithInclude(id));
    }

    /// <summary>
    /// Gets  business products where there
    /// </summary>
    /// <param name="businessId">Business ID  whose products are going to be searched</param>
    /// <param name="isFrozen">Checks if the product is frozen</param>
    /// <returns>List of products</returns>
    public async Task<IEnumerable<BllDto.Product>> GetBusinessProductsWithIncludes(Guid businessId, bool isFrozen)
    {
        var res = await Repository.GetBusinessProductsWithIncludes(businessId, isFrozen);

        return await MapProductEnumerationWithPicture(res);
    }

    /// <summary>
    /// Gets single business product
    /// </summary>
    /// <param name="productId">Searchable product ID</param>
    /// <param name="businessId">Business ID  whose products are going to be searched</param>
    /// <param name="isFrozen">Checks if the product is frozen</param>
    /// <returns>Single business product</returns>
    public async Task<BllDto.Product?> GetBusinessProduct(Guid productId, Guid businessId, bool isFrozen)
    {
        var res = await Repository.GetBusinessProduct(productId, businessId, isFrozen);
        return await MapProductWithPicture(res!);
    }

    /// <summary>
    /// Gets  business products where there
    /// </summary>
    /// <param name="businessId">Business ID  whose products are going to be searched</param>
    /// <returns>List of products</returns>
    public async Task<IEnumerable<BllDto.Product>> GetBusinessProductsWithIncludes(Guid businessId)
    {
        var res = await Repository.GetBusinessProductsWithIncludes(businessId);
        return await MapProductEnumerationWithPicture(res);
    }

    /// <summary>
    /// Gets single business product
    /// </summary>
    /// <param name="productId">Searchable product ID</param>
    /// <param name="businessId">Business ID  whose products are going to be searched</param>
    /// <returns>Single business product</returns>
    public async Task<BllDto.Product?> GetBusinessProduct(Guid productId, Guid businessId)
    {
        return Mapper.Map((await Repository.GetBusinessProduct(productId, businessId)));
    }


    /// <summary>
    /// Map enumerated data with picture
    /// </summary>
    /// <param name="products">Mappable enumeration</param>
    /// <returns>Mapped data</returns>
    private async Task<IEnumerable<BllDto.Product>> MapProductEnumerationWithPicture(
        IEnumerable<DalDto.Product> products)
    {
        List<BllDto.Product> returnable = new List<BllDto.Product>();
        foreach (var business in products.ToList())
        {
            var mappedValue = await MapProductWithPicture(business);
            if (mappedValue != null)
            {
                returnable.Add(mappedValue);
            }
        }

        return returnable;
    }


    /// <summary>
    /// Map data with picture
    /// </summary>
    /// <param name="product">Mappable DTO</param>
    /// <returns>Mapped data</returns>
    private async Task<BllDto.Product?> MapProductWithPicture(DalDto.Product product)
    {
        var bllProduct = Mapper.Map(product);
        var businessPicture = await Uow.ProductPictureRepository
            .FindProductPicture(product.Id);
        if (businessPicture != null && bllProduct != null)
        {
            bllProduct.PicturePath = businessPicture.Picture!.Path;
        }

        return bllProduct;
    }
}