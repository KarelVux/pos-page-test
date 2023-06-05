using Contracts.Base;
using DAL.Contracts.App;
using DAL.EF.Base;

namespace DAL.EF.App.Repositories;

public class BusinessCategoryRepository : EFBaseRepository<DAL.DTO.BusinessCategory, Domain.App.BusinessCategory, ApplicationDbContext>,
    IBusinessCategoryRepository
{
    public BusinessCategoryRepository(ApplicationDbContext dataContext, IMapper<DAL.DTO.BusinessCategory, Domain.App.BusinessCategory> mapper) : base(dataContext, mapper)
    {
    }
}