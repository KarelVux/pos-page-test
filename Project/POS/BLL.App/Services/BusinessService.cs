using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class BusinessService :
    BaseEntityService<BllDto.Business, DalDto.Business, IBusinessRepository>, IBusinessService
{
    protected IAppUOW Uow;

    public BusinessService(IAppUOW uow,
        IMapper<BllDto.Business, DalDto.Business> mapper)
        : base(uow.BusinessRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<BllDto.Business>> GetBusinessesWithIncludes(Guid settlementId,
        Guid? businessCategoryId = null)
    {
        var res = (await Repository.GetBusinessesWithIncludes(settlementId, businessCategoryId))!;

        return await MapBusinessEnumerationWithPicture(res);
    }

    /// <summary>
    /// Gets business products  page data with includes and product count is always bigger than zero
    /// </summary>
    /// <param name="id">Searchable business ID</param>
    /// <returns>Business details page data</returns>
    public async Task<BllDto.Business?> GetBusinessDetailsPageData(Guid id)
    {
        var business =
            await Repository.FindAsyncWithInclude(id);

        if (business == null)
        {
            return null;
        }

        return await MapBusinessWithPicture(business);
    }

    /// <summary>
    /// Gets businesses that user owns
    /// </summary>
    /// <param name="userId">User ID that owns the business</param>
    /// <returns>List of user owned businesses</returns>
    public async Task<IEnumerable<BllDto.Business>> AllAsyncWithBusinessOwnerAndPicture(Guid userId)
    {
        var res = (await Repository.AllAsyncWithBusinessOwner(userId))!;
        return await MapBusinessEnumerationWithPicture(res);
    }

    public async Task<IEnumerable<BllDto.Business>> AllAsyncWithInclude()
    {
        return (await Repository.AllAsyncWithInclude()).Select(e => Mapper.Map(e))!;
    }

    public async Task<BllDto.Business?> FindAsyncWithInclude(Guid id)
    {
        return Mapper.Map(await Repository.FindAsyncWithInclude(id));
    }

    /// <summary>
    /// Map enumerated data with picture
    /// </summary>
    /// <param name="businesses">Mappable enumeration</param>
    /// <returns>Mapped data</returns>
    private async Task<IEnumerable<BllDto.Business>> MapBusinessEnumerationWithPicture(
        IEnumerable<DalDto.Business> businesses)
    {
        List<BllDto.Business> returnable = new List<BllDto.Business>();
        foreach (var business in businesses.ToList())
        {
            var mappedValue = await MapBusinessWithPicture(business);
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
    /// <param name="business">Mappable DTO</param>
    /// <returns>Mapped data</returns>
    private async Task<BllDto.Business?> MapBusinessWithPicture(DalDto.Business business)
    {
        var bllBusiness = Mapper.Map(business);
        var businessPicture = await Uow.BusinessPictureRepository
            .FindViaBusinessId(business.Id);
        if (businessPicture != null && bllBusiness != null)
        {
            bllBusiness.PicturePath = businessPicture.Picture!.Path;
        }

        return bllBusiness;
    }
}