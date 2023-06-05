using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class OrderFeedbackService :
    BaseEntityService<BllDto.OrderFeedback, DalDto.OrderFeedback, IOrderFeedbackRepository>,
    IOrderFeedbackService
{
    protected IAppUOW Uow;

    public OrderFeedbackService(IAppUOW uow, IMapper<BllDto.OrderFeedback, DalDto.OrderFeedback> mapper)
        : base(uow.OrderFeedbackRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<BllDto.OrderFeedback>> AllAsyncWithInclude()
    {
        return (await Repository.AllAsyncWithInclude()).Select(e => Mapper.Map(e))!;
    }

    public async Task<BllDto.OrderFeedback?> FindAsyncWithInclude(Guid id)
    {
        return Mapper.Map(await Repository.FindAsyncWithInclude(id));
    }

    /// <summary>
    /// Find order feedback via order ID
    /// </summary>
    /// <param name="orderId">Order ID</param>
    /// <returns>Search results</returns>
    public async Task<BllDto.OrderFeedback?> FinOrderedFeedbackViaOderId(Guid orderId)
    {
        return Mapper.Map(await Repository.FinOrderedFeedbackViaOderId(orderId));
    }
}