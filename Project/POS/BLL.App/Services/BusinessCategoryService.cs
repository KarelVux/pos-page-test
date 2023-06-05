using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class BusinessCategoryService :
    BaseEntityService<BllDto.BusinessCategory, DalDto.BusinessCategory, IBusinessCategoryRepository>,
    IBusinessCategoryService
{
    protected IAppUOW Uow;

    public BusinessCategoryService(IAppUOW uow, IMapper<BllDto.BusinessCategory, DalDto.BusinessCategory> mapper)
        : base(uow.BusinessCategoryRepository, mapper)
    {
        Uow = uow;
    }
}