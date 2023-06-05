using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IOrderService : IBaseRepository<BllDto.Order>,
    IOrderRepositoryCustom<BllDto.Order>
{
    // add your custom service methods here
  //  Task<BllDto.Order?> FindOrderViaInvoice(Guid invoiceId);
}