using Contracts.Base;
using DAL.Contracts.App;
using DAL.DTO;
using DAL.EF.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Repositories;

public class OrderRepository : EFBaseRepository<DAL.DTO.Order, Domain.App.Order, ApplicationDbContext>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext dataContext, IMapper<DAL.DTO.Order, Domain.App.Order> mapper) : base(
        dataContext, mapper)
    {
    }

    public async Task<IEnumerable<DAL.DTO.Order>> AllAsyncWithInclude()
    {
        var res = await RepositoryDbSet
            .Include(o => o.Invoice)
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }

    public async Task<DAL.DTO.Order?> FindAsyncWithInclude(Guid id)
    {
        var res = await RepositoryDbSet
            .Include(o => o.Invoice)
            .FirstOrDefaultAsync(m => m.Id == id);

        return Mapper.Map(res);
    }
}