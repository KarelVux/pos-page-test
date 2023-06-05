using Contracts.Base;
using DAL.Contracts.App;
using DAL.DTO;
using DAL.EF.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Repositories;

public class OrderFeedbackRepository :
    EFBaseRepository<DAL.DTO.OrderFeedback, Domain.App.OrderFeedback, ApplicationDbContext>, IOrderFeedbackRepository
{
    public OrderFeedbackRepository(ApplicationDbContext dataContext,
        IMapper<DAL.DTO.OrderFeedback, Domain.App.OrderFeedback> mapper) : base(dataContext, mapper)
    {
    }

    public async Task<IEnumerable<DAL.DTO.OrderFeedback>> AllAsyncWithInclude()
    {
        var res = await RepositoryDbSet
            .Include(o => o.Order)
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }

    public async Task<DAL.DTO.OrderFeedback?> FindAsyncWithInclude(Guid id)
    {
        var res = await RepositoryDbSet
            .Include(o => o.Order)
            .FirstOrDefaultAsync(m => m.Id == id);

        return Mapper.Map(res);
    }

    /// <summary>
    /// Find order feedback via Order ID
    /// </summary>
    /// <param name="orderId">Order Id</param>
    /// <returns>Search result</returns>
    public async Task<OrderFeedback?> FinOrderedFeedbackViaOderId(Guid orderId)
    {
        var res = await RepositoryDbSet
            .FirstOrDefaultAsync(m => m.OrderId == orderId);
        return Mapper.Map(res);    }
}