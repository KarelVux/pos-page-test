using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class BusinessManagerService :
    BaseEntityService<BllDto.BusinessManager, DalDto.BusinessManager, IBusinessManagerRepository>,
    IBusinessManagerService
{
    protected IAppUOW Uow;

    public BusinessManagerService(IAppUOW uow, IMapper<BllDto.BusinessManager, DalDto.BusinessManager> mapper)
        : base(uow.BusinessManagerRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<BllDto.BusinessManager>> AllAsyncWithInclude()
    {
        return (await Repository.AllAsyncWithInclude()).Select(e => Mapper.Map(e))!;
    }

    public async Task<BllDto.BusinessManager?> FindAsyncWithInclude(Guid id)
    {
        return Mapper.Map(await Repository.FindAsyncWithInclude(id));
    }

    /// <summary>
    /// Gets user business
    /// </summary>
    /// <param name="businessId">Business ID</param>
    /// <param name="userId">User ID</param>
    /// <returns></returns>
    public async Task<BllDto.BusinessManager?> GetUserManagedBusiness(Guid businessId, Guid userId)
    {
        return Mapper.Map(await Repository.GetUserManagedBusiness(businessId, userId));
    }
}