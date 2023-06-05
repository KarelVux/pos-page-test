using DAL.Contracts.Base;
using DAL.DTO;
using Domain.Contracts.Base;

namespace DAL.Contracts.App;

public interface IOrderFeedbackRepository : IBaseRepository<OrderFeedback>, IOrderFeedbackRepositoryCustom<OrderFeedback>
{
    // add here custom methods for repo only
    Task<OrderFeedback?> FinOrderedFeedbackViaOderId(Guid orderId);
}

public interface IOrderFeedbackRepositoryCustom<TEntity> : IBaseRepositoryWithInclude<TEntity>
    where TEntity : class, IDomainEntityId
{
    // add here shared methods between repo and service
}