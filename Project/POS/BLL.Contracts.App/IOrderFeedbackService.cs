using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IOrderFeedbackService : IBaseRepository<BllDto.OrderFeedback>,
    IOrderFeedbackRepositoryCustom<BllDto.OrderFeedback>
{
    // add your custom service methods here
    Task<BllDto.OrderFeedback?> FinOrderedFeedbackViaOderId(Guid orderId);
}