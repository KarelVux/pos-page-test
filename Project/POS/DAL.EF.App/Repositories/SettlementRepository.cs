using Contracts.Base;
using DAL.Contracts.App;
using DAL.EF.Base;
using Domain.App;

namespace DAL.EF.App.Repositories;

public class SettlementRepository :
    EFBaseRepository<DAL.DTO.Settlement, Domain.App.Settlement, ApplicationDbContext>, ISettlementRepository
{
    public SettlementRepository(ApplicationDbContext dataContext,
        IMapper<DAL.DTO.Settlement, Domain.App.Settlement> mapper) : base(dataContext, mapper)
    {
    }
}